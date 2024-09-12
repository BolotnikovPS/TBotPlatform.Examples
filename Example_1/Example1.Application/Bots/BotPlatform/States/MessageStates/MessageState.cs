using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Contexts.BotPlatform;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.MessageStates;

internal class MessageState : IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await context.SendTextMessageAsync(
            "Добро пожаловать на борт, добрый путник!",
            cancellationToken
            );
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}