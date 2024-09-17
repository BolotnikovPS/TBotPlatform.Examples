using Example1.Application.Attributes;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using TBotPlatform.Contracts;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Abstractions.Contexts.AsyncDisposable;

namespace Example1.Application.Bots.BotPlatform.States.MessageStates;

[MyStateInlineActivator]
internal class MessageCloseState(ICacheService cacheService) : MyBaseState(cacheService), IMyState
{
    public async Task HandleAsync(IStateContext context, User user, CancellationToken cancellationToken)
    {
        await context.UpdateMarkupTextAndDropButtonAsync(InlineButtonsCollection.Instance.GetValueByKey(EInlineButtonsType.ToClose), cancellationToken);

        context.SetNeedUpdateMarkup();

        await RemoveValuesInCacheAsync(user.Id, cancellationToken);
    }

    public Task HandleCompleteAsync(IStateContext context, User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleErrorAsync(IStateContext context, User user, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
}