using Example1.Application.Contracts.Messages.DomainMessage;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Domain.Abstractions.CQRS;
using Example1.Domain.Abstractions.Publishers.EventDomain;
using Example1.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Contracts.Abstractions.Factories;

namespace Example1.Application.Handlers.EventDomain;

internal class RefreshMenuMessageHandler(ISenderRun senderRun, IStateContextFactory stateContextFactory, IStateFactory stateFactory) : IEventDomainMessageHandler<RefreshMenuMessage>
{
    public async Task Handle(RefreshMenuMessage message, CancellationToken cancellationToken)
    {
        var users = await senderRun.SendAsync(new UsersQuery(null, message.UserId, EUserBlockType.None), cancellationToken);

        foreach (var user in users)
        {
            var state = stateFactory.GetStateByNameOrDefault();
            await using var stateContext = stateContextFactory.CreateStateContext(user);

            await stateContextFactory.UpdateMarkupByStateAsync(user, stateContext, state, cancellationToken);
        }
    }
}