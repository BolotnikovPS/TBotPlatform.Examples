using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Abstractions.CQRS.Query;
using Example1.Domain.Contexts.BotPlatform;
using Microsoft.EntityFrameworkCore;

namespace Example1.Application.CQ.DbContext.BotPlatformContext.Queries;

internal record VerificationQuery(int UserId) : IQuery<Verification>;

internal class VerificationQueryHandler(
    IBotPlatformDbContext tgBotDbContext
    ) : IQueryHandler<VerificationQuery, Verification>
{
    public async Task<Verification> Handle(VerificationQuery request, CancellationToken cancellationToken)
    {
        return await tgBotDbContext.Verifications.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
    }
}