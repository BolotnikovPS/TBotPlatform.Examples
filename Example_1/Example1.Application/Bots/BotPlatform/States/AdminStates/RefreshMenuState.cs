using Example1.Application.Attributes;
using Example1.Application.Bots.BotPlatform.States.MessageStates;
using Example1.Application.Contracts.Messages.DomainMessage;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Abstractions.Publishers.EventDomain;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;
using TBotPlatform.Contracts.Bots.Markups;
using TBotPlatform.Extension;

namespace Example1.Application.Bots.BotPlatform.States.AdminStates;

[MyStateInlineActivator(ButtonsTypes = [EButtonsType.RefreshMenu,])]
internal class RefreshMenuState(IEventDomainPublisher domainPublisher, ICacheService cacheService) : MyBaseState(cacheService), IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        if (context.MarkupNextState.IsNotNull())
        {
            await domainPublisher.PublishAsync(new RefreshMenuMessage(), cancellationToken);

            await context.SendOrUpdateTextMessage(GetDescription(ETextsType.MenuIsRefresh), cancellationToken);

            return;
        }

        var buttons = new InlineMarkupList
        {
            new MyInlineMarkupState(EInlineButtonsType.Yes, nameof(RefreshMenuState)),
            new MyInlineMarkupState(EInlineButtonsType.ToClose, nameof(MessageCloseState)),
        };

        await context.SendOrUpdateTextMessage(GetDescription(ETextsType.IsRefreshMenu), buttons, cancellationToken);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}