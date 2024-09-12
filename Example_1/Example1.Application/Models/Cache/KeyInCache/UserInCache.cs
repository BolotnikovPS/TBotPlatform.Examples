using Example1.Domain.Contexts.BotPlatform;
using TBotPlatform.Contracts.Abstractions.Cache;

namespace Example1.Application.Models.Cache.KeyInCache;

internal class UserInCache : IKeyInCache
{
    public User User { get; set; }

    public string Key => User.TgUserId.ToString();
}