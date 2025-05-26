using Example1.Domain.Bots.Config;
using Example1.Domain.Enums;

namespace Example1.Domain.Bots;

public sealed class BotTypeData(EBotType botType, BotSettings botSetting)
{
    public EBotType BotType { get; } = botType;
    public BotSettings BotSetting { get; } = botSetting;
}