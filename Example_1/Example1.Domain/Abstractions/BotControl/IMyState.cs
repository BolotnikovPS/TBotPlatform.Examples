using Example1.Domain.Contexts.BotPlatform;
using TBotPlatform.Contracts.Abstractions.State;

namespace Example1.Domain.Abstractions.BotControl;

public interface IMyState : IState<User>;