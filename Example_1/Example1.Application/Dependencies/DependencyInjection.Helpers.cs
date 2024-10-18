using Example1.Application.Helpers;
using Example1.Domain.Abstractions.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Example1.Application.Dependencies;

public static partial class DependencyInjection
{
    internal static IServiceCollection AddHelpers(this IServiceCollection services)
        => services
          .AddSingleton<IDeclensionHelper, DeclensionHelper>()
          .AddSingleton<IDateTimeHelper, DateTimeHelper>();
}