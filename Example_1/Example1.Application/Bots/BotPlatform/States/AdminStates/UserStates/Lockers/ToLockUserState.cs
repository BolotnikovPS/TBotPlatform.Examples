using Example1.Application.Attributes;
using Example1.Application.CQ.DbContext.BotPlatformContext.Commands;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Contexts.BotPlatform.Enums;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates.Lockers;

[MyStateInlineActivator]
internal class ToLockUserState(IMediator mediator) : MyBaseStateHandler
{
    private const string Text = "Пользователь заблокирован.";

    public override async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new UpdateUserCommand(long.Parse(context.MarkupNextState.Data), EUserBlockType.Fraud),
            cancellationToken
            );

        await context.UpdateMarkupTextAndDropButton(Text, cancellationToken);
    }
}