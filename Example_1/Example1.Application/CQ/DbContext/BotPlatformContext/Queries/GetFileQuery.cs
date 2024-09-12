using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Abstractions.CQRS.Query;
using Example1.Domain.Contexts.BotPlatform;
using Microsoft.EntityFrameworkCore;

namespace Example1.Application.CQ.DbContext.BotPlatformContext.Queries;

internal record GetFileQuery(int FileId) : IQuery<FileBox>;

internal class GetFileQueryHandler(
    IBotPlatformDbContext tgBotDbContext
    ) : IQueryHandler<GetFileQuery, FileBox>
{
    public async Task<FileBox> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        return await tgBotDbContext.FilesBox.FirstOrDefaultAsync(z => z.Id == request.FileId, cancellationToken);
    }
}