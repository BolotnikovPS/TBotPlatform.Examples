using Example1.Domain.Contexts.BotPlatform;
using Microsoft.EntityFrameworkCore;

namespace Example1.Application.Abstractions.DBContext;

public interface IBotPlatformDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Verification> Verifications { get; set; }
    DbSet<FileBox> FilesBox { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}