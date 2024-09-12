using LordOfTheHouse.Domain.Contexts;
using LordOfTheHouse.Domain.Contexts.Enums;
using LordOfTheHouse.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LordOfTheHouse.Persistence
{
    internal class TgBotDBContext : DbContext, ITgBotDBContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<CalendarOfEvents> CalendarOfEvents { get; set; }
        private readonly IConfiguration _configuration;

        private TgBotDBContext() { }

        public TgBotDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("ConnectionDB");
                var versionDB = _configuration.GetConnectionString("VersionDB");

                if (!connectionString.CheckAny())
                    throw new Exception("Configuration Not Found");

                if (!versionDB.CheckAny())
                    throw new Exception("Version Not Found");

                optionsBuilder.UseMySql(connectionString, ServerVersion.Parse(versionDB));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder
                .Entity<Users>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.UserId);

                    entity.Property(e => e.Id)
                            .HasMaxLength(11);
                    entity.Property(e => e.UserId)
                            .IsRequired();
                    entity.Property(e => e.ChatId)
                            .IsRequired();
                    entity.Property(e => e.UserName);
                    entity.Property(e => e.FirstName);
                    entity.Property(e => e.LastName);
                    entity.Property(e => e.Role)
                            .IsRequired()
                            .HasMaxLength(50)
                            .HasConversion(
                                v => v.ToString(),
                                v => (EUsersRoles)Enum.Parse(typeof(EUsersRoles), v))
                            .IsUnicode(false)
                            .HasDefaultValue(EUsersRoles.User);
                    entity.Property(e => e.BlockType)
                            .HasMaxLength(50)
                            .HasConversion(
                                v => v.ToString(),
                                v => (EUsersBlockType)Enum.Parse(typeof(EUsersBlockType), v))
                            .IsUnicode(false)
                            .HasDefaultValue(EUsersBlockType.None);
                    entity.Property(e => e.EventType)
                            .HasMaxLength(50)
                            .HasConversion(
                                v => v.ToString(),
                                v => (EUsersEventType)Enum.Parse(typeof(EUsersEventType), v))
                            .IsUnicode(false)
                            .HasDefaultValue(EUsersEventType.None);
                    entity.Property(e => e.RegisterDate);
                });

            modelBuilder
                .Entity<CalendarOfEvents>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.UserId);

                    entity.Property(e => e.Id)
                            .HasMaxLength(11);
                    entity.Property(e => e.UserId)
                            .IsRequired()
                            .HasMaxLength(11);
                    entity.Property(e => e.Name)
                            .IsRequired();
                    entity.Property(e => e.Description)
                            .IsRequired(false);
                    entity.Property(e => e.DateTime);
                    entity.Property(e => e.Periods)
                            .IsRequired()
                            .HasMaxLength(50)
                            .HasConversion(
                                v => v.ToString(),
                                v => (ECalendarPeriods)Enum.Parse(typeof(ECalendarPeriods), v))
                            .IsUnicode(false)
                            .HasDefaultValue(ECalendarPeriods.None);
                    entity.Property(e => e.Status)
                            .IsRequired()
                            .HasMaxLength(50)
                            .HasConversion(
                                v => v.ToString(),
                                v => (ECalendarStatus)Enum.Parse(typeof(ECalendarStatus), v))
                            .IsUnicode(false)
                            .HasDefaultValue(ECalendarStatus.None);
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}