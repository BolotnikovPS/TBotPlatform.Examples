using Example1.Application.Attributes;
using Example1.Application.Contracts.Messages.DomainMessage;
using Example1.Application.CQ.DbContext.BotPlatformContext.Commands;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Abstractions.CQRS;
using Example1.Domain.Abstractions.Publishers.EventDomain;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates.Lockers;

[MyStateInlineActivator]
internal class ToUnLockUserState(ISenderRun senderRun, IEventDomainPublisher domainPublisher) : IMyState
{
    private const string Text = "Пользователь разблокирован.";

    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await senderRun.SendAsync(new UpdateUserCommand(long.Parse(context.MarkupNextState.Data), EUserBlockType.None), cancellationToken);

        await domainPublisher.PublishAsync(
            new UserVerificationAvailableMessage(int.Parse(context.MarkupNextState.Data)),
            cancellationToken
            );

        await context.UpdateMarkupTextAndDropButtonAsync(Text, cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}