using Example1.Application.Attributes;
using Example1.Application.Bots.BotPlatform.States.MessageStates;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Application.Extensions;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Enums;
using MediatR;
using System.Text;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates.UserStates.Statistics;

[MyStateInlineActivator(ButtonsTypes = [EButtonsType.GetUsersStatistic,])]
internal class ShortUsersStatisticState(IMediator mediator) : MyBaseStateHandler
{
    public override async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new UsersQuery(), cancellationToken);

        var sbText = new StringBuilder($"Всего пользователей: {users.Count}")
                    .AppendLine()
                    .AppendLine($"Заблокированных пользователей: {users.Count(z => z.IsLock())}")
                    .AppendLine()
                    .AppendLine($"Админов: {users.Count(z => z.IsAdmin())}");

        var inlineButtons = new InlineMarkupList
        {
            new MyInlineMarkupState(EInlineButtonsType.UsersDetailedStatistics, nameof(FullUsersStatisticState)),
            new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)),
        };

        await context.SendOrUpdateTextMessage(sbText.ToString(), inlineButtons, cancellationToken);
    }
}