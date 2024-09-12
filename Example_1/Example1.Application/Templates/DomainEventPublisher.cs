using Example1.Domain.Abstractions.Publishers.EventDomain;
using MediatR;
using Microsoft.Extensions.Logging;
using TBotPlatform.Extension;

namespace Example1.Application.Templates;

internal class DomainEventPublisher(
    ILogger<DomainEventPublisher> logger,
    IPublisher publisher
    ) : IEventDomainPublisher
{
    async Task IEventDomainPublisher.PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            logger.LogInformation("Сообщение для {domainEventPublisher} типа {messageType}: {message}", nameof(DomainEventPublisher), message.GetType().FullName, message.ToJson());

            await publisher.Publish(message, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в {domainEventPublisher} типа {messageType}: {message}", nameof(DomainEventPublisher), message.GetType().FullName, message.ToJson());
            throw;
        }
    }
}