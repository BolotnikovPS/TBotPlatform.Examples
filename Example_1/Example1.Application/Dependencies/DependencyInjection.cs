using Example1.Application.Abstractions;
using Example1.Application.Bots;
using Example1.Application.CQ.Behaviour;
using Example1.Application.Templates;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Abstractions.Publishers.EventDomain;
using Example1.Domain.Bots.Config;
using Example1.Domain.Enums;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TBotPlatform.Common.Dependencies;

namespace Example1.Application.Dependencies;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, EBotType botType)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        services
           .AddSingleton<IBotType, BotType>(
                serviceProvider =>
                {
                    var configService = serviceProvider.GetRequiredService<IConfigService>();

                    var botSettings = configService.GetValueOrNull<BotSettings>(EConfigKey.BotSettings);
                    return new(new(botType, botSettings));
                })
           .AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(executingAssembly);
                    cfg.NotificationPublisher = new TaskWhenAllPublisher();
                })
           .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
           .AddScoped<IEventDomainPublisher, DomainEventPublisher>()
           .AddHelpers()
           .AddStates(executingAssembly, botType.ToString())
           .AddFactories(executingAssembly)
           .AddReceivingHandler<StartReceivingHandler>();

        return services;
    }
}