using Example1.Domain.Bots;
using Example1.Domain.Enums;
using TBotPlatform.Contracts.Abstractions;
using TBotPlatform.Contracts.Bots.Buttons;
using TBotPlatform.Contracts.Bots.Users;

namespace Example1.Application.Bots.BotPlatform.MenuButton;

internal class AdminMenuButton : IMenuButton
{
    public Task<ButtonsRuleMassiveList> GetMarkUpAsync<T>(T user)
        where T : UserBase
    {
        var menu = new ButtonsRuleMassiveList
        {
            new()
            {
                ButtonsRules =
                [
                    new MyButtonsRule(EButtonsType.GetAllUsers),
                    new MyButtonsRule(EButtonsType.GetUsersStatistic),
                ],
            },
            new()
            {
                ButtonsRules =
                [
                    new MyButtonsRule(EButtonsType.ListJobs),
                    new MyButtonsRule(EButtonsType.RefreshMenu),
                ],
            },
            new()
            {
                ButtonsRules =
                [
                    new MyButtonsRule(EButtonsType.ToBackMain),
                ],
            },
        };

        return Task.FromResult(menu);
    }
}