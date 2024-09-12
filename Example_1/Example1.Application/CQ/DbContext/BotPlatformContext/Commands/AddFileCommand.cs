using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Abstractions.CQRS.Command;
using Example1.Domain.Abstractions.Helpers;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Contexts.BotPlatform.Enums;

namespace Example1.Application.CQ.DbContext.BotPlatformContext.Commands;

internal record AddFileCommand(int UserId, byte[] FileData, string FileName, EFileType FileType) : ICommand<int>;

internal class AddFileCommandHandler(
    IBotPlatformDbContext tgBotDbContext,
    IDateTimeHelper dateTimeHelper
    ) : ICommandHandler<AddFileCommand, int>
{
    public async Task<int> Handle(AddFileCommand request, CancellationToken cancellationToken)
    {
        var file = new FileBox
        {
            UserId = request.UserId,
            Data = request.FileData,
            Name = request.FileName,
            Type = request.FileType,
            Create = dateTimeHelper.GetLocalDateNow(),
        };

        await tgBotDbContext.FilesBox.AddAsync(file, cancellationToken);
        await tgBotDbContext.SaveChangesAsync(cancellationToken);

        return file.Id;
    }
}