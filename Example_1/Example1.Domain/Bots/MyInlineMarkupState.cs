using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Bots.Markups.InlineMarkups;

namespace Example1.Domain.Bots;

public class MyInlineMarkupState(EInlineButtonsType buttonName, string state = null, string data = null)
    : InlineMarkupState(InlineButtonsCollection.Instance.GetValueByKey(buttonName), state, data);