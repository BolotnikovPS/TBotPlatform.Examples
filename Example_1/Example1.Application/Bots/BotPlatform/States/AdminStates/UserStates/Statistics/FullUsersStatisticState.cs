using Example1.Application.Attributes;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Application.Extensions;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Abstractions.Helpers;
using MediatR;
using System.Text;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.FileDatas;
using User = Example1.Domain.Contexts.BotPlatform.User;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates.Statistics;

[MyStateInlineActivator]
internal class FullUsersStatisticState(IMediator mediator, IDateTimeHelper dateTimeHelper) : IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(), cancellationToken);

        var sbText = new StringBuilder($"Всего пользователей: {users.Count}")
                    .AppendLine()
                    .AppendLine($"Заблокированных пользователей: {users.Count(z => z.IsLock())}")
                    .AppendLine()
                    .AppendLine($"Админов: {users.Count(z => z.IsAdmin())}")
                    .AppendLine();

        var groupUsers = users
                        .GroupBy(x => x.RegisterDate)
                        .Select(
                             group => new
                             {
                                 Date = group.Key,
                                 Count = group.Count(),
                             })
                        .OrderBy(y => y.Date);

        var csv = new StringBuilder();

        foreach (var item in groupUsers)
        {
            csv.AppendLine($"{item.Date.ToRussian()}; {item.Count}");
        }

        await using var fileStream = new MemoryStream();

        var file = new FileDataBase
        {
            Bytes = Encoding.UTF8.GetBytes(csv.ToString()),
            Name = $"users_{dateTimeHelper.GetLocalDateNow().ToRussian()}.csv",
        };

        await context.SendDocumentAsync(file, cancellationToken);
        await context.UpdateMarkupTextAndDropButtonAsync(sbText.ToString(), cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}