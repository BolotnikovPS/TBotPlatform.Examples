using Example1.Application.Attributes;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Application.Extensions;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Contexts.BotPlatform.Enums;
using MediatR;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Pagination;
using TBotPlatform.Extension;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates;

[MyStateInlineActivator]
internal class ActiveUsersState(IMediator mediator) : IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(
            new UsersQuery(null, null, EUserBlockType.None),
            cancellationToken
            );
        users = users.Where(z => z.Id != user.Id).ToList();

        if (users.IsNull())
        {
            await context.SendTextMessageAsync("Список активных пользователей пустой", cancellationToken);

            return;
        }

        if (context.MarkupNextState.TryParsePagination(out var result))
        {
            await context.SendOrUpdateTextMessageAsync("Список активных пользователей в боте.", users.CreateActiveUserButtons(result), cancellationToken);

            return;
        }

        await context.SendOrUpdateTextMessageAsync("Список активных пользователей в боте.", users.CreateActiveUserButtons(1), cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}