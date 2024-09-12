using Example1.Application.Contracts.Messages.DomainMessage;
using Example1.Application.CQ.DbContext.BotPlatformContext.Commands;
using Example1.Application.CQ.DbContext.BotPlatformContext.Queries;
using Example1.Domain.Abstractions.CQRS;
using Example1.Domain.Abstractions.Publishers.EventDomain;
using Example1.Domain.Contexts.BotPlatform.Enums;
using TBotPlatform.Contracts.Abstractions.Factories;
using TBotPlatform.Extension;

namespace Example1.Application.Handlers.EventDomain;

internal class UserVerificationBlockMessageHandler(ISenderRun senderRun, IStateContextFactory stateContextFactory) : IEventDomainMessageHandler<UserVerificationBlockMessage>
{
    public async Task Handle(UserVerificationBlockMessage message, CancellationToken cancellationToken)
    {
        var user = await senderRun.SendAsync(
            new UserQuery(null, null, message.UserId),
            cancellationToken
            );

        if (user.IsNull())
        {
            return;
        }

        await using var stateContext = stateContextFactory.CreateStateContext(user);

        await stateContext.SendTextMessageAsync("🔒 Вы заблокированы по решению администратора.", cancellationToken);

        await senderRun.SendAsync(
            new UpdateOrCreateVerificationCommand(user.Id, EUserEventType.None),
            cancellationToken
            );
    }
}