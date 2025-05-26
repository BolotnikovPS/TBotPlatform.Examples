using Example1.Application.Abstractions;
using Example1.Application.Attributes;
using Example1.Application.Bots.BotPlatform.States.MessageStates;
using Example1.Application.Extensions;
using Example1.Domain.Abstractions.Helpers;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;
using TBotPlatform.Contracts.Bots.Markups.InlineMarkups;
using TBotPlatform.Extension;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates;

[MyStateInlineActivator(ButtonsTypes = [EButtonsType.ListJobs,])]
internal class JobState(ISchedulerJobFactory schedulerFactory, IDateTimeHelper dateTimeHelper) : MyBaseStateHandler
{
    private const string NoJobText = "Нет джобов.";

    public override async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        var jobs = await schedulerFactory.GetJobListAsync(cancellationToken);

        if (jobs.IsNull())
        {
            await context.SendTextMessage(NoJobText, cancellationToken);

            return;
        }

        if (context.MarkupNextState.IsNull())
        {
            var inlineButtons = new InlineMarkupList();

            foreach (var job in jobs)
            {
                inlineButtons.Add(new InlineMarkupState(job.Description, nameof(JobState), job.Name));
            }

            inlineButtons.Add(new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)));

            await context.SendOrUpdateTextMessage($"Список джобов на {dateTimeHelper.GetLocalDateTimeNow().ToRussianWithHours()}", inlineButtons, null, cancellationToken);

            return;
        }

        if (context.MarkupNextState.Data.CheckAny())
        {
            var job = jobs.Find(z => z.Name == context.MarkupNextState.Data);

            if (job.IsNull())
            {
                await context.SendOrUpdateTextMessage($"🛑 Задача {context.MarkupNextState.Data} не найдена.", cancellationToken);

                return;
            }

            await schedulerFactory.StartJobAsync(job, cancellationToken);

            await context.SendOrUpdateTextMessage($"💪 Задача {job.Name} запущена.", cancellationToken);
        }
    }
}