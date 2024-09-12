using Example1.Application.Abstractions;
using Example1.Domain.Bots.Config;
using Example1.Domain.Enums;
using Example1.Infrastructure.ConfigServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TBotPlatform.Extension;
using Winton.Extensions.Configuration.Consul;
using Winton.Extensions.Configuration.Consul.Parsers;

namespace Example1.Infrastructure.Dependencies;

public static partial class DependencyInjection
{
    private static TimeSpan PollWaitTime => TimeSpan.FromSeconds(15.0);

    public static IConfigurationBuilder AddConsulConfig(this IConfigurationBuilder builder, IConfiguration configuration, EBotType botType)
    {
        var consulSettings = configuration
                            .GetSection("Consul")
                            .Get<ConfigSettings>(c => c.BindNonPublicProperties = true);

        if (consulSettings.IsNull())
        {
            throw new ArgumentNullException(nameof(consulSettings));
        }

        return builder
           .AddConsul(
                consulSettings.Prefix + botType,
                options =>
                {
                    options.ConsulConfigurationOptions = cco => { cco.Address = new(consulSettings.Address); };
                    options.Optional = true;
                    options.PollWaitTime = PollWaitTime;
                    options.ReloadOnChange = true;
                    options.Parser = new SimpleConfigurationParser();
                });
    }

    public static IServiceCollection AddConfigService(this IServiceCollection services)
    {
        services.AddSingleton<IConfigService, ConfigService>();

        return services;
    }

    private static T GetValueOrException<T>(IConfigService configService, EConfigKey key)
    {
        var result = configService.GetValueOrNull<T>(key);

        if (result.IsNull())
        {
            throw new NullReferenceException(key.ToString());
        }

        return result;
    }

    private static string GetValueOrException(IConfigService configService, EConfigKey key)
    {
        var result = configService.GetValueOrNull(key);

        if (result.IsNull())
        {
            throw new NullReferenceException(key.ToString());
        }

        return result;
    }
}