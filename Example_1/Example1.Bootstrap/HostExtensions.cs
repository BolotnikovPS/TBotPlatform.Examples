using Example1.Domain.Enums;
using Example1.Infrastructure.Dependencies;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;
using TBotPlatform.Extension;

namespace Example1.Bootstrap;

public static class HostExtensions
{
    public static IHostBuilder CreateHost(EBotType botType)
    {
        return Host
              .CreateDefaultBuilder()
              .UseDefaultServiceProvider(
                   (_, options) => { options.ValidateOnBuild = false; })
              .UseConsoleLifetime()
              .ConfigureAppConfiguration(
                   config => { config.AddConfiguration(ResolveConsulConfiguration(botType)); })
              .ConfigureServices(
                   (context, services) =>
                   {
                       services
                          .Configure<KestrelServerOptions>(context.Configuration.GetSection("Kestrel"))
                          .AddLogging(
                               loggingBuilder =>
                               {
                                   var logFilePath = context.Configuration.GetValue<string>("LogFilePath");

                                   if (logFilePath.IsNull())
                                   {
                                       throw new("Конфиг LogFilePath пустой.");
                                   }

                                   loggingBuilder
                                      .ClearProviders()
                                      .AddConfiguration(context.Configuration)
                                      .AddConsole()
                                      .AddSerilog()
                                      .AddFile(
                                           logFilePath,
                                           outputTemplate: "{Timestamp:o} {RequestId,13} [{Level:u3}] ({SourceContext}) {Message} ({EventId:x8}){NewLine}{Exception}"
                                           )
                                      .SetMinimumLevel(LogLevel.Trace)
                                      .AddFilter(filter => filter == LogLevel.Trace);
                               })
                          .AddInfrastructure(botType)
                          .AddHealthChecks()
                          .ForwardToPrometheus();
                   });
    }

    private static IConfiguration ResolveConsulConfiguration(EBotType botType)
    {
        var configuration = new ConfigurationBuilder()
                           .SetBasePath(AppContext.BaseDirectory)
                           .AddJsonFile("appsettings.json")
                           .Build();

        return new ConfigurationBuilder()
              .AddConsulConfig(configuration, botType)
              .AddConfiguration(configuration)
              .Build();
    }
}