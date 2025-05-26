using Example1.Application.Attributes;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using TBotPlatform.Common;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.MessageStates;

[MyStateInlineActivator]
internal class MessageCloseState(ICacheService cacheService) : MyBaseState(cacheService), IMyState
{
    public async Task Handle(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await context.UpdateMarkupTextAndDropButton(InlineButtonsCollection.Instance.GetValueByKey(EInlineButtonsType.ToClose), cancellationToken);

        context.SetNeedUpdateMarkup();

        await RemoveValuesInCache(user.Id);
    }

    public Task HandleComplete(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleError(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}