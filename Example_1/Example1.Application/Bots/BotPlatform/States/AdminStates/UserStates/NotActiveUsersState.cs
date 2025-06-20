﻿using Example1.Application.Attributes;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Application.Extensions;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Pagination;
using TBotPlatform.Extension;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates;

[MyStateInlineActivator]
internal class NotActiveUsersState(IMediator mediator) : MyBaseStateHandler
{
    public override async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(), cancellationToken);
        users = users
               .Where(
                    z => z.Id != user.Id
                         && z.IsLock()
                    )
               .ToList();

        if (users.IsNull())
        {
            await context.SendTextMessage("Список заблокированных пользователей пустой", cancellationToken);

            return;
        }

        if (context.MarkupNextState.TryParsePagination(out var result))
        {
            await context.SendOrUpdateTextMessage("Список заблокированных пользователей в боте.", users.CreateNotActiveUserButtons(result), cancellationToken);

            return;
        }

        await context.SendOrUpdateTextMessage("Список заблокированных пользователей в боте.", users.CreateNotActiveUserButtons(1), cancellationToken);
    }
}