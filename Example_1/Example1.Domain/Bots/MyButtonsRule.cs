using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Bots.Buttons;

namespace Example1.Domain.Bots;

public class MyButtonsRule(EButtonsType button) : ButtonsRule(ButtonCollection.Instance.GetValueByKey(button));