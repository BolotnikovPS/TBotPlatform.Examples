namespace Example1.Domain.Abstractions.Publishers.EventDomain;

public interface IEventDomainPublisher
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : IEventDomainMessage;
}