using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;

namespace Example1.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record RemoveFileCommand(List<int> FileIds) : ICommand;

internal class RemoveFileCommandHandler(
    IBotPlatformDbContext tgBotDbContext
    ) : ICommandHandler<RemoveFileCommand>
{
    public async Task Handle(RemoveFileCommand request, CancellationToken cancellationToken)
    {
        var images = await tgBotDbContext.FilesBox.Where(z => request.FileIds.Contains(z.Id)).ToListAsync(cancellationToken);

        foreach (var item in images)
        {
            tgBotDbContext.FilesBox.Remove(item);
        }

        await tgBotDbContext.SaveChangesAsync(cancellationToken);
    }
}