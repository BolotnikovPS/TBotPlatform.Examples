using Example1.Application.Abstractions;
using Example1.Application.Dependencies;
using Example1.Domain.Bots.Config;
using Example1.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using TBotPlatform.Common.Dependencies;
using TBotPlatform.Contracts.Bots.Config;

namespace Example1.Infrastructure.Dependencies;

public static partial class DependencyInjection
{
    private static readonly string[] Tags = ["readiness",];

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, EBotType botType)
    {
        services.AddConfigService();

        var configService = services.BuildServiceProvider().GetRequiredService<IConfigService>();

        var redis = GetValueOrException(configService, EConfigKey.Redis);
        var telegram = GetValueOrException<TelegramSettings>(configService, EConfigKey.Telegram);

        services
           .AddTelegramContextHostedService(telegram)
           .AddCache(redis, botType.ToString(), Tags)
           .AddBotTypeInfrastructure(configService, Tags)
           .AddApplication(botType)
           .AddQuartzScheduler(botType);

        return services;
    }
}