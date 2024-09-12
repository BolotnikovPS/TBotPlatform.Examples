using System.Collections.Frozen;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.EnumCollection;

namespace Example1.Domain.EnumCollection;

public class CommandCollection : CollectionBase<ECommandsType>
{
    protected override FrozenDictionary<ECommandsType, string> DataCollection { get; } = new Dictionary<ECommandsType, string>
    {
        [ECommandsType.Start] = "/start",
    }.ToFrozenDictionary();

    public static CommandCollection Instance { get; } = new();
}