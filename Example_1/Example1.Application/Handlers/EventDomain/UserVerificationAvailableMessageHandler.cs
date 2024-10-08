﻿using Example1.Application.Contracts.Messages.DomainMessage;
using Example1.Application.CQ.DbContext.BotPlatformContext.Commands;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Domain.Abstractions.Publishers.EventDomain;
using Example1.Domain.Contexts.BotPlatform.Enums;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Factories;
using TBotPlatform.Extension;

namespace Example1.Application.Handlers.EventDomain;

internal class UserVerificationAvailableMessageHandler(IMediator mediator, IStateContextFactory stateContextFactory, IStateFactory stateFactory, IMenuButtonFactory menuButtonFactory)
    : IEventDomainMessageHandler<UserVerificationAvailableMessage>
{
    public async Task Handle(UserVerificationAvailableMessage message, CancellationToken cancellationToken)
    {
        var userQuery = new UserQuery(null, null, message.UserId);
        var user = await mediator.Send(userQuery, cancellationToken);

        if (user.IsNull())
        {
            return;
        }

        var state = stateFactory.GetStateByNameOrDefault();
        await using var stateContext = stateContextFactory.CreateStateContext(user);

        await stateContext.SendTextMessageAsync("🔓 Вы разблокированы по решению администратора.", cancellationToken);

        var updateOrCreateVerificationCommand = new UpdateOrCreateVerificationCommand(user.Id, EUserEventType.None);
        await mediator.Send(updateOrCreateVerificationCommand, cancellationToken);

        var updateUserCommand = new UpdateUserCommand(user.Id, EUserBlockType.None);
        await mediator.Send(updateUserCommand, cancellationToken);

        await menuButtonFactory.UpdateMarkupByStateAsync(user, stateContext, state, cancellationToken);
    }
}