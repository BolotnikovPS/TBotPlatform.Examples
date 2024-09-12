using System.Reflection;
using Example1.Application.Templates;
using Example1.Domain.Abstractions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Example1.Application.Dependencies;

public static partial class DependencyInjection
{
    public static IServiceCollection AddMap(this IServiceCollection services, Assembly executingAssembly)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(executingAssembly);

        services
           .AddSingleton<IMapper>(new Mapper(typeAdapterConfig))
           .AddScoped<IMap, Map>();

        return services;
    }
}