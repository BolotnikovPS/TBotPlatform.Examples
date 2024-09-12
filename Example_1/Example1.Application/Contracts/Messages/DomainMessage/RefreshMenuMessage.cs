using Example1.Domain.Abstractions.Publishers.EventDomain;

namespace Example1.Application.Contracts.Messages.DomainMessage;

internal record RefreshMenuMessage(int? UserId = null) : IEventDomainMessage;