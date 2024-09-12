using Example1.Application.Attributes;
using Example1.Application.Bots.BotPlatform.MenuButton;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates;

[MyStateActivator(typeof(AdminMenuButton), ButtonsTypes = [EButtonsType.Admin,])]
internal class AdminState : IMyState
{
    public Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}