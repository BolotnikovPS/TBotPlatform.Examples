#nullable enable
using Example1.Application.CQ.DbContext.BotPlatformContext.Commands;
using Example1.Application.Extensions;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Contexts.BotPlatform.Enums;
using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using TBotPlatform.Common;
using TBotPlatform.Contracts.Abstractions.Factories;
using TBotPlatform.Contracts.Abstractions.Handlers;
using TBotPlatform.Contracts.Bots;
using TBotPlatform.Contracts.Bots.ChatUpdate;
using TBotPlatform.Extension;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = Example1.Domain.Contexts.BotPlatform.User;

namespace Example1.Application.Bots;

internal sealed class StartReceivingHandler(
    ILogger<StartReceivingHandler> logger,
    IMediator mediator,
    IStateFactory stateFactory,
    IStateContextFactory stateContextFactory,
    IMenuButtonFactory menuButtonFactory,
    IBotType botType
    ) : IStartReceivingHandler
{
    public async Task HandleUpdate(Update update, MarkupNextState? markupNextState, TelegramMessageUserData telegramUser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        User user;
        try
        {
            user = await mediator.Send(new GetOrCreateUserCommand(telegramUser), cancellationToken);

            if (user.IsNull())
            {
                return;
            }

            if (user.IsLock() && update.Type.NotIn(UpdateType.MyChatMember))
            {
                var stateHistory = stateFactory.LockState;
                await using var stateContext = await stateContextFactory.CreateStateContext(user, stateHistory, update, cancellationToken);

                return;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка определения пользователя для запроса {update}", update.ToJson());

            return;
        }

        if (update.Type.In(UpdateType.Message, UpdateType.CallbackQuery))
        {
            await DoWorkMessageAsync(user, update, markupNextState, cancellationToken);
            return;
        }

        if (update.Type.In(UpdateType.MyChatMember))
        {
            await DoWorkMemberAsync(user, update, cancellationToken);
        }
    }

    private async Task DoWorkMessageAsync(User user, Update update, MarkupNextState? markupNextState, CancellationToken cancellationToken)
    {
        try
        {
            var stateHistory = update.Type switch
            {
                UpdateType.Message => await DetermineStateAsync(user.ChatId, update, stateFactory, cancellationToken),
                UpdateType.CallbackQuery => stateFactory.GetStateByNameOrDefault(markupNextState?.State),
                _ => null,
            };

            if (stateHistory.IsNull())
            {
                stateHistory = await stateFactory.GetStateMain(user.ChatId, cancellationToken);
            }

            await using var stateContext = await stateContextFactory.CreateStateContext(user, stateHistory!, update, markupNextState, cancellationToken);

            if (stateContext.TryGetStateResult(out var stateResult) && (stateHistory!.MenuStateTypeOrNull.IsNotNull() || stateResult!.IsNeedUpdateMarkup))
            {
                var markUp = stateResult!.IsNeedUpdateMarkup
                    ? await stateFactory.GetLastStateWithMenu(user.ChatId, cancellationToken)
                    : stateHistory;
                await menuButtonFactory.UpdateMainButtonsByState(user, stateContext, markUp, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Для пользователя {user} возникло исключение", user.ToJson());
        }
    }

    private async Task DoWorkMemberAsync(User user, Update update, CancellationToken cancellationToken)
    {
        try
        {
            var chatMember = update.MyChatMember?.NewChatMember;

            if (chatMember.IsNull())
            {
                return;
            }

            if (chatMember!.Status.In(ChatMemberStatus.Kicked))
            {
                var updateUserCommand = new UpdateUserCommand(user.Id, EUserBlockType.KickedByUser);
                await mediator.Send(updateUserCommand, cancellationToken);

                return;
            }

            if (chatMember.Status.In(ChatMemberStatus.Member))
            {
                var blockType = botType.GetBotType().BotSetting.WithRegistration
                    ? EUserBlockType.Registration
                    : EUserBlockType.None;

                var updateUserCommand = new UpdateUserCommand(user.Id, blockType);
                await mediator.Send(updateUserCommand, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Для пользователя {user} возникло исключение.", user.ToJson());
        }
    }

    private static async Task<StateHistory?> DetermineStateAsync(long chatId, Update update, IStateFactory stateFactory, CancellationToken cancellationToken)
    {
        var stateTypeText = TextCollection.Instance.GetKeyByValue(update.Message?.ReplyToMessage?.Text);
        var stateTypeButton = ButtonCollection.Instance.GetKeyByValue(update.Message?.Text);
        var stateTypeCommand = CommandCollection.Instance.GetKeyByValue(update.Message?.Text);

        if (stateTypeButton == EButtonsType.None
            && stateTypeText == ETextsType.None
            && stateTypeCommand == ECommandsType.None
           )
        {
            return await stateFactory.GetBindStateOrNull(chatId, cancellationToken);
        }

        if (stateTypeButton != EButtonsType.None)
        {
            return stateTypeButton switch
            {
                EButtonsType.ToBack => await stateFactory.GetStatePreviousOrMain(chatId, cancellationToken),
                EButtonsType.ToBackMain => await stateFactory.GetStateMain(chatId, cancellationToken),
                _ => await stateFactory.GetStateByButtonsTypeOrDefault(chatId, stateTypeButton.ToString(), cancellationToken),
            };
        }

        if (stateTypeText != ETextsType.None)
        {
            return stateFactory.GetStateByTextsTypeOrDefault(chatId, stateTypeText.ToString());
        }

        if (stateTypeCommand != ECommandsType.None)
        {
            return await stateFactory.GetStateByCommandsTypeOrDefault(chatId, update.Message?.Text, cancellationToken);
        }

        return null;
    }
}