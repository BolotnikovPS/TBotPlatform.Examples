using LordOfTheHouse.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LordOfTheHouse.Persistence.Abstractions
{
    public interface ITgBotDBContext
    {
        DbSet<Users> Users { get; set; }
        DbSet<CalendarOfEvents> CalendarOfEvents { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
