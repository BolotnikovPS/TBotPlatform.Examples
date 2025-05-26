using Example.View;
using Example1.Bootstrap;
using Example1.Domain.Enums;

await HostExtensions
     .CreateHost(EBotType.Example)
     .ConfigureWebHostDefaults(
          webBuilder =>
          {
              webBuilder
                 .UseStartup<Startup>()
                 .UseShutdownTimeout(TimeSpan.FromMilliseconds(10000));
          })
     .Build()
     .StartBotAsync();