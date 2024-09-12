using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Bots;

namespace Example1.Application.Bots;

internal class BotType(BotTypeData botTypeData) : IBotType
{
    public BotTypeData GetBotType() => botTypeData;
}