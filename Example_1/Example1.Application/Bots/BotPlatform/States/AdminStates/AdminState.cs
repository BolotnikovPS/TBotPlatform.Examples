using Example1.Application.Attributes;
using Example1.Application.Bots.BotPlatform.MenuButton;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates;

[MyStateActivator(typeof(AdminMenuButton), ButtonsTypes = [EButtonsType.Admin,])]
internal class AdminState : MyBaseStateHandler
{
    public override Task Handle(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;
}