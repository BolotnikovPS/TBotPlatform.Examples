using LordOfTheHouse.Persistence.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LordOfTheHouse.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<ITgBotDBContext, TgBotDBContext>(ServiceLifetime.Transient);

            return services;
        }
    }
}