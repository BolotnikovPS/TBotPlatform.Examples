﻿using Example1.Application.Attributes;
using Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates.Lockers;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Application.Extensions;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Contexts.BotPlatform.Enums;
using Example1.Domain.Enums;
using MediatR;
using System.Text;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates;

[MyStateInlineActivator]
internal class UserInfoState(IMediator mediator) : MyBaseStateHandler
{
    public override async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var userFromState = await mediator.Send(
            new UserQuery(null, null, long.Parse(context.MarkupNextState.Data)),
            cancellationToken
            );

        var messageUserInfo = new StringBuilder($"Пользователь {userFromState.UserName}: {userFromState.FirstName} {userFromState.LastName}")
                             .AppendLine($"Тип пользователя: {userFromState.Role.ToString()}")
                             .AppendLine($"Блокировка: {userFromState.IsLock().ToString()}")
                             .AppendLine($"Дата регистрации: {userFromState.RegisterDate.ToRussian()}")
                             .AppendLine($"Id пользователя: {userFromState.TgUserId}")
                             .AppendLine($"Id чата: {userFromState.ChatId}")
                             .AppendLine()
                             .ToString();

        var userId = userFromState.Id.ToString();
        var buttons = new InlineMarkupList();

        if (userFromState.BlockType != EUserBlockType.KickedByUser)
        {
            buttons.Add(
                new MyInlineMarkupState(
                    userFromState.IsLock() ? EInlineButtonsType.ToUnLock : EInlineButtonsType.ToLock,
                    userFromState.IsLock() ? nameof(ToUnLockUserState) : nameof(ToLockUserState),
                    userId
                    )
                );
        }

        if (userFromState.BlockType == EUserBlockType.None)
        {
            buttons.Add(new MyInlineMarkupState(EInlineButtonsType.RefreshUserMenu, nameof(RefreshUserMenuState), userId));
        }

        buttons.Add(
            new MyInlineMarkupState(
                EInlineButtonsType.ToBack,
                userFromState.IsLock() ? nameof(NotActiveUsersState) : nameof(ActiveUsersState)
                )
            );

        await context.SendOrUpdateTextMessage(messageUserInfo, buttons, cancellationToken);
    }
}