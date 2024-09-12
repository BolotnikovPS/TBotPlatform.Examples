using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Abstractions.Cache;
using TBotPlatform.Contracts.Bots.States;

namespace Example1.Application.Bots.BotPlatform;

internal abstract class MyBaseState(ICacheService cacheService)
    : BaseState(cacheService)
{
    protected static string GetDescription(ETextsType key)
        => TextCollection.Instance.GetValueByKey(key);
}