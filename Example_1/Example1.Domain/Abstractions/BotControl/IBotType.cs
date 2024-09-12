using Example1.Domain.Bots;

namespace Example1.Domain.Abstractions.BotControl;

public interface IBotType
{
    BotTypeData GetBotType();
}