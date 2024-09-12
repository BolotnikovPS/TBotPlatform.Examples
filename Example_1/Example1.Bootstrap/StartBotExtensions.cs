using Example1.Application.Abstractions.DBContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Example1.Bootstrap;

public static class StartBotExtensions
{
    public static async Task StartBotAsync(this IHost host)
    {
        await UpMigrationsAsync(host.Services);

        await host.RunAsync();
    }

    private static async Task UpMigrationsAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        foreach (var contextType in scope.ServiceProvider.GetServices<IMigratorContext>())
        {
            await contextType.MigrateAsync();
        }
    }
}