using Example1.Domain.EnumCollection;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Bots.Buttons;

namespace Example1.Domain.Bots;

public class MyMainButton(EButtonsType button) : MainButton(ButtonCollection.Instance.GetValueByKey(button));