using Example1.Domain.Bots.Config;

namespace Example1.Application.Abstractions;

public interface IConfigService
{
    T GetValueOrNull<T>(EConfigKey key);

    string GetValueOrNull(EConfigKey key);
}