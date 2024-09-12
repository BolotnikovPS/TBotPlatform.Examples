using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Abstractions.CQRS.Command;
using Example1.Domain.Contexts.BotPlatform.Enums;
using Microsoft.EntityFrameworkCore;
using TBotPlatform.Extension;

namespace Example1.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record UpdateOrCreateVerificationCommand(int UserId, EUserEventType? EventType) : ICommand;

internal class UpdateOrCreateVerificationCommandHandler(
    IBotPlatformDbContext tgBotDbContext
    ) : ICommandHandler<UpdateOrCreateVerificationCommand>
{
    public async Task Handle(UpdateOrCreateVerificationCommand request, CancellationToken cancellationToken)
    {
        var result = await tgBotDbContext.Verifications.FirstOrDefaultAsync(z => z.UserId == request.UserId, cancellationToken);

        if (result.IsNull())
        {
            result = new()
            {
                UserId = request.UserId,
            };

            await tgBotDbContext.Verifications.AddAsync(result, cancellationToken);
            await tgBotDbContext.SaveChangesAsync(cancellationToken);
        }

        if (request.EventType.HasValue)
        {
            result.EventType = request.EventType;
        }

        tgBotDbContext.Verifications.Update(result);
        await tgBotDbContext.SaveChangesAsync(cancellationToken);
    }
}