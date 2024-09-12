﻿using Example1.Domain.Contexts.BotPlatform.Enums;

namespace Example1.Domain.Contexts.BotPlatform;

public class Verification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public EUserEventType? EventType { get; set; }

    public virtual User User { get; set; }
}