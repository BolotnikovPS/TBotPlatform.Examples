using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Contexts.BotPlatform.Enums;

namespace Example1.Application.Extensions;

internal static class ApplicationExtensions
{
    public static bool IsLock(this User user) => user.BlockType != EUserBlockType.None;
}