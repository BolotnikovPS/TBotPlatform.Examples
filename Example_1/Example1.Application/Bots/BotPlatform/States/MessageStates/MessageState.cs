using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.MessageStates;

internal class MessageState : MyBaseStateHandler
{
    public override async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await context.SendTextMessage(
            "Добро пожаловать на борт, добрый путник!",
            cancellationToken
            );
    }
}