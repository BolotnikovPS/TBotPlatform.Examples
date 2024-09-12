using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Abstractions.CQRS.Command;
using Example1.Domain.Abstractions.Helpers;
using Example1.Domain.Bots;
using Example1.Domain.Contexts.BotPlatform.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TBotPlatform.Contracts.Bots.ChatUpdate;
using TBotPlatform.Contracts.Bots.ChatUpdate.Enums;
using TBotPlatform.Extension;
using User = Example1.Domain.Contexts.BotPlatform.User;

namespace Example1.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record GetOrCreateUserCommand(TelegramMessageUserData TelegramUser) : ICommand<User>;

internal class GetOrCreateUserCommandHandler(
    ILogger<GetOrCreateUserCommandHandler> logger,
    IBotPlatformDbContext tgBotDbContext,
    IDateTimeHelper dateTimeHelper,
    IBotType botType
    ) : ICommandHandler<GetOrCreateUserCommand, User>
{
    public async Task<User> Handle(GetOrCreateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.TelegramUser.IsNull()
            || request.TelegramUser.UserOrNull.IsNull()
            || request.TelegramUser.UserOrNull!.IsBot
           )
        {
            var error = new Exception("Не удалось определить пользователя Telegram");
            logger.LogError(error, "");

            throw error;
        }

        var tgUser = request.TelegramUser.UserOrNull;

        logger.LogInformation("Получение пользователя по FromUser = {userId}", tgUser!.TgUserId);

        var result = await tgBotDbContext.Users.FirstOrDefaultAsync(z => z.TgUserId == tgUser.TgUserId, cancellationToken);

        if (result.IsNotNull())
        {
            return result;
        }

        logger.LogInformation("Создание пользователя по FromUser = {userId}", tgUser.TgUserId);

        var chatIdEx = new Exception("Не смогли определить данные чата");
        var chat = request.TelegramUser.ChatOrNull;

        if (chat.IsNull())
        {
            logger.LogError(chatIdEx, "");

            throw chatIdEx;
        }

        ValidChatData(botType.GetBotType(), chat!.Type);

        var blockType = botType.GetBotType().BotSetting.WithRegistration
            ? EUserBlockType.Registration
            : EUserBlockType.None;

        result = new()
        {
            TgUserId = tgUser.TgUserId,
            UserName = tgUser.UserName ?? "",
            ChatId = chat.Id,
            FirstName = tgUser.FirstName,
            LastName = tgUser.LastName ?? "",
            RegisterDate = dateTimeHelper.GetLocalDateTimeNow().Date,
            BlockType = blockType,
            Role = EUserRoles.User,
        };

        await tgBotDbContext.Users.AddAsync(result, cancellationToken);
        await tgBotDbContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    private static void ValidChatData(BotTypeData botTypeData, EChatType chatType)
    {
        if (botTypeData.BotSetting.IsNotNull()
            && botTypeData.BotSetting.ChatTypes.IsNotNull()
            && chatType.NotIn(botTypeData.BotSetting.ChatTypes.ToArray())
           )
        {
            return;
        }

        throw new("Бот не поддерживает диалоги данного типа");
    }
}