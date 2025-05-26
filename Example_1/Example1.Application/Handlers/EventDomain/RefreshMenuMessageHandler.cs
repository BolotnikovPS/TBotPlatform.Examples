using Example1.Application.Contracts.Messages.DomainMessage;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Domain.Abstractions.Publishers.EventDomain;
using Example1.Domain.Contexts.BotPlatform.Enums;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Factories;

namespace Example1.Application.Handlers.EventDomain;

internal class RefreshMenuMessageHandler(IMediator mediator, IStateContextFactory stateContextFactory, IStateFactory stateFactory, IMenuButtonFactory menuButtonFactory) : IEventDomainMessageHandler<RefreshMenuMessage>
{
    public async Task Handle(RefreshMenuMessage message, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(null, message.UserId, EUserBlockType.None), cancellationToken);

        foreach (var user in users)
        {
            var state = stateFactory.GetStateByNameOrDefault();
            await using var stateContext = stateContextFactory.GetStateContext(user);

            await menuButtonFactory.UpdateMainButtonsByState(user, stateContext, state, cancellationToken);
        }
    }
}