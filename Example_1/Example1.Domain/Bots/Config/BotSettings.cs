using TBotPlatform.Contracts.Bots.ChatUpdate.Enums;

namespace Example1.Domain.Bots.Config;

public class BotSettings
{
    public bool WithRegistration { get; set; }

    public List<EChatType> ChatTypes { get; set; }
}