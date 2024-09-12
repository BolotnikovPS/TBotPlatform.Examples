using Example1.Domain.Bots;

namespace Example1.Application.Abstractions;

public interface ISchedulerJobFactory
{
    Task<List<BotJobData>> GetJobListAsync(CancellationToken cancellationToken);
    Task StartJobAsync(BotJobData botJobData, CancellationToken cancellationToken);
}