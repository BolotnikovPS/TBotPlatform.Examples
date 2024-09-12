﻿using System.Collections.Frozen;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.EnumCollection;

namespace Example1.Domain.EnumCollection;

/// <summary>
/// Текста с расшифровками
/// </summary>
public class TextCollection : CollectionBase<ETextsType>
{
    protected override FrozenDictionary<ETextsType, string> DataCollection { get; } = new Dictionary<ETextsType, string>
    {
        [ETextsType.IsRefreshMenu] = "Вы точно хотите обновить основное меню всем пользователям❓",
        [ETextsType.MenuIsRefresh] = "👍 Запущен механизм обновления меню пользователей.",
    }.ToFrozenDictionary();

    public static TextCollection Instance { get; } = new();
}