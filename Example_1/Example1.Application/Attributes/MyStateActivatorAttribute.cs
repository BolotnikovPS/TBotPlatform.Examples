﻿using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Attributes;

namespace Example1.Application.Attributes;

internal class MyStateActivatorAttribute(Type menuType) : StateActivatorAttribute(menuType)
{
    /// <summary>
    /// Перечень типов кнопок соответствующих состоянию
    /// </summary>
    public new EButtonsType[] ButtonsTypes
    {
        get => base.ButtonsTypes.Select(Enum.Parse<EButtonsType>).ToArray();
        set => base.ButtonsTypes = value.Select(z => z.ToString()).ToArray();
    }

    /// <summary>
    /// Перечень типов текста соответствующих состоянию
    /// </summary>}
    public new ETextsType[] TextsTypes
    {
        get => base.TextsTypes.Select(Enum.Parse<ETextsType>).ToArray();
        set => base.TextsTypes = value.Select(z => z.ToString()).ToArray();
    }

    /// <summary>
    /// Перечень типов команд соответствующих состоянию
    /// </summary>}
    public new ECommandsType[] CommandsTypes
    {
        get => base.CommandsTypes.Select(CommandCollection.Instance.GetKeyByValue).ToArray();
        set => base.CommandsTypes = value.Select(CommandCollection.Instance.GetValueByKey).ToArray();
    }

    /// <summary>
    /// Для какого бота доступно состояние
    /// </summary>
    public new EBotType OnlyForBot
    {
        get => Enum.Parse<EBotType>(base.OnlyForBot);
        set => base.OnlyForBot = value.ToString();
    }
}