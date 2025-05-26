using Telegram.Bot.Types.Enums;

namespace Example1.Domain.Bots.Config;

public class BotSettings
{
    public bool WithRegistration { get; set; }

    public List<ChatType> ChatTypes { get; set; }
}