﻿using Example1.Application.Abstractions;
using Example1.Domain.Bots.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TBotPlatform.Extension;

namespace Example1.Infrastructure.ConfigServices;

internal class ConfigService(ILogger<ConfigService> logger, IConfiguration configuration) : IConfigService
{
    public T GetValueOrNull<T>(EConfigKey key)
    {
        var value = GetValueOrNull(key);

        return value.CheckAny()
            ? DeserializeObject<T>(key, value)
            : default;
    }

    public string GetValueOrNull(EConfigKey key)
    {
        var value = configuration
                   .GetSection(key.ToString())
                   .Get<string>(c => c.BindNonPublicProperties = true);

        return value.CheckAny()
            ? value
            : default;
    }

    private T DeserializeObject<T>(EConfigKey key, string value = null)
    {
        if (value.IsNull())
        {
            return default;
        }

        try
        {
            return value.FromJson<T>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "По ключу {key} произошла ошибка десериализации объекта {value}", key.ToString(), value);

            return default;
        }
    }
}