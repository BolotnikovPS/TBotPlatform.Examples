using Example1.Application.Abstractions.DBContext;
using Example1.Domain.Contexts.BotPlatform;
using Example1.Domain.Contexts.BotPlatform.Enums;
using Microsoft.EntityFrameworkCore;

namespace Example1.Infrastructure.Contexts.BotPlatform;

internal class BotPlatformDbContext(DbContextOptions<BotPlatformDbContext> options) : CustomDbContext(options), IBotPlatformDbContext, IMigratorContext
{
    public DbSet<Verification> Verifications { get; set; }
    public DbSet<FileBox> FilesBox { get; set; }

    public override void CustomOnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
           .Entity<Verification>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.UserId);

                    entity
                       .Property(e => e.Id)
                       .HasMaxLength(11);
                    entity
                       .Property(e => e.UserId)
                       .HasMaxLength(11);
                    entity
                       .Property(e => e.EventType)
                       .HasMaxLength(50)
                       .HasConversion(
                            v => v.ToString(),
                            v => (EUserEventType)Enum.Parse(typeof(EUserEventType), v)
                            )
                       .IsUnicode(false)
                       .HasDefaultValue(EUserEventType.None);

                    entity
                       .HasOne(e => e.User)
                       .WithMany()
                       .HasForeignKey(e => e.UserId)
                       .HasPrincipalKey(x => x.Id)
                       .OnDelete(DeleteBehavior.Cascade);
                });

        modelBuilder
           .Entity<FileBox>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.UserId);

                    entity
                       .Property(e => e.Id)
                       .HasMaxLength(11);
                    entity
                       .Property(e => e.UserId)
                       .HasMaxLength(11);
                    entity
                       .Property(e => e.Data)
                       .IsRequired();
                    entity
                       .Property(e => e.Name)
                       .IsRequired();
                    entity
                       .Property(e => e.Type)
                       .HasMaxLength(50)
                       .HasConversion(
                            v => v.ToString(),
                            v => (EFileType)Enum.Parse(typeof(EFileType), v)
                            )
                       .IsUnicode(false)
                       .HasDefaultValue(EFileType.None);
                    entity
                       .Property(e => e.Create)
                       .IsRequired();

                    entity
                       .HasOne(e => e.User)
                       .WithMany()
                       .HasForeignKey(e => e.UserId)
                       .HasPrincipalKey(x => x.Id)
                       .OnDelete(DeleteBehavior.Cascade);
                });
    }
}