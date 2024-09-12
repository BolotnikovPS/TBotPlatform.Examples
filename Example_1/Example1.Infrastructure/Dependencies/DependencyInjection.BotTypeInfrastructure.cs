using Example1.Application.Abstractions;
using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Bots.Config;
using Example1.Infrastructure.Contexts;
using Example1.Infrastructure.Contexts.BotPlatform;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Example1.Infrastructure.Dependencies;

public static partial class DependencyInjection
{
    internal static IServiceCollection AddBotTypeInfrastructure(this IServiceCollection services, IConfigService configService, string[] tags)
        => services.AddFullDbContext<IBotPlatformDbContext, BotPlatformDbContext>(configService, tags);

    private static IServiceCollection AddFullDbContext<TContextService, TContextImplementation>(this IServiceCollection services, IConfigService configService, IEnumerable<string> tags)
        where TContextService : class where TContextImplementation : DbContext, TContextService, IMigratorContext
    {
        services
           .AddDbContext<TContextService, TContextImplementation>(
                options =>
                {
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                    if (options.IsConfigured)
                    {
                        return;
                    }

                    var connectionSettings = GetValueOrException<DataBaseSettings>(configService, EConfigKey.DataBase);

                    options.UseMySql(
                        connectionSettings.ConnectionDb,
                        ServerVersion.Parse(connectionSettings.VersionDb),
                        optionsBuilder =>
                        {
                            optionsBuilder.EnableRetryOnFailure(
                                10,
                                TimeSpan.FromSeconds(30),
                                null
                                );
                        });
                })
           .AddScoped<IMigratorContext, TContextImplementation>()
           .AddHealthChecks()
           .AddDbContextCheck<TContextImplementation>(typeof(TContextImplementation).Name, null, tags);

        return services;
    }
}