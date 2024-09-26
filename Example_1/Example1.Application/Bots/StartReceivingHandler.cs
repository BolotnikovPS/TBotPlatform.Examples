#nullable enable
using Example1.Application.CQ.DbContext.BotPlatformContext.Commands;
using Example1.Application.Extensions;
using Example1.Domain.Abstractions.BotControl;
using Example1.Domain.Contexts.BotPlatform.Enums;
using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using TBotPlatform.Contracts.Abstractions.Factories;
using TBotPlatform.Contracts.Abstractions.Handlers;
using TBotPlatform.Contracts.Bots;
using TBotPlatform.Contracts.Bots.ChatUpdate;
using TBotPlatform.Contracts.Bots.ChatUpdate.Enums;
using TBotPlatform.Extension;
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
    public async Task HandleUpdateAsync(ChatUpdate chatUpdate, MarkupNextState? markupNextState, TelegramMessageUserData telegramUser, CancellationToken cancellationToken)
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

            if (user.IsLock() && chatUpdate.UpdateType.NotIn(EChatUpdateType.MyChatMember))
            {
                var stateHistory = stateFactory.GetLockState();
                await using var stateContext = await stateContextFactory.CreateStateContextAsync(user, stateHistory, chatUpdate, cancellationToken);

                return;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка определения пользователя для запроса {update}", chatUpdate.ToJson());

            return;
        }

        if (chatUpdate.UpdateType.In(EChatUpdateType.Message, EChatUpdateType.CallbackQuery))
        {
            await DoWorkMessageAsync(user, chatUpdate, markupNextState, cancellationToken);
            return;
        }

        if (chatUpdate.UpdateType.In(EChatUpdateType.MyChatMember))
        {
            await DoWorkMemberAsync(user, chatUpdate, cancellationToken);
        }
    }

    private async Task DoWorkMessageAsync(User user, ChatUpdate chatMessage, MarkupNextState? markupNextState, CancellationToken cancellationToken)
    {
        try
        {
            var stateHistory = chatMessage.UpdateType switch
            {
                EChatUpdateType.Message => await DetermineStateAsync(user.ChatId, chatMessage, stateFactory, cancellationToken),
                EChatUpdateType.CallbackQuery => stateFactory.GetStateByNameOrDefault(markupNextState?.State),
                _ => null,
            };

            if (stateHistory.IsNull())
            {
                stateHistory = await stateFactory.GetStateMainAsync(user.ChatId, cancellationToken);
            }

            await using var stateContext = await stateContextFactory.CreateStateContextAsync(user, stateHistory!, chatMessage, markupNextState, cancellationToken);

            var stateResult = stateContextFactory.GetStateResult(stateContext);

            if (stateHistory!.MenuStateType.IsNotNull() || stateResult!.IsNeedUpdateMarkup)
            {
                var markUp = stateResult!.IsNeedUpdateMarkup
                    ? await stateFactory.GetLastStateWithMenuAsync(user.ChatId, cancellationToken)
                    : stateHistory;
                await menuButtonFactory.UpdateMarkupByStateAsync(user, stateContext, markUp, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Для пользователя {user} возникло исключение", user.ToJson());
        }
    }

    private async Task DoWorkMemberAsync(User user, ChatUpdate chatMessage, CancellationToken cancellationToken)
    {
        try
        {
            var chatMember = chatMessage.MyMemberUpdateOrNull?.NewChatMember;

            if (chatMember.IsNull())
            {
                return;
            }

            if (chatMember!.Status.In(EChatMemberStatus.Kicked))
            {
                var updateUserCommand = new UpdateUserCommand(user.Id, EUserBlockType.KickedByUser);
                await mediator.Send(updateUserCommand, cancellationToken);

                return;
            }

            if (chatMember.Status.In(EChatMemberStatus.Member))
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

    private static async Task<StateHistory?> DetermineStateAsync(long chatId, ChatUpdate chatMessage, IStateFactory stateFactory, CancellationToken cancellationToken)
    {
        var stateTypeText = TextCollection.Instance.GetKeyByValue(chatMessage.Message.ReplyToMessageOrNull?.Text);
        var stateTypeButton = ButtonCollection.Instance.GetKeyByValue(chatMessage.Message.Text);
        var stateTypeCommand = CommandCollection.Instance.GetKeyByValue(chatMessage.Message.Text);

        if (stateTypeButton == EButtonsType.None
            && stateTypeText == ETextsType.None
            && stateTypeCommand == ECommandsType.None
           )
        {
            return await stateFactory.GetBindStateOrNullAsync(chatId, cancellationToken);
        }

        if (stateTypeButton != EButtonsType.None)
        {
            return stateTypeButton switch
            {
                EButtonsType.ToBack => await stateFactory.GetStatePreviousOrMainAsync(chatId, cancellationToken),
                EButtonsType.ToBackMain => await stateFactory.GetStateMainAsync(chatId, cancellationToken),
                _ => await stateFactory.GetStateByButtonsTypeOrDefaultAsync(chatId, stateTypeButton.ToString(), cancellationToken),
            };
        }

        if (stateTypeText != ETextsType.None)
        {
            return stateFactory.GetStateByTextsTypeOrDefault(chatId, stateTypeText.ToString());
        }

        if (stateTypeCommand != ECommandsType.None)
        {
            return await stateFactory.GetStateByCommandsTypeOrDefaultAsync(chatId, chatMessage.Message.Text, cancellationToken);
        }

        return null;
    }
}