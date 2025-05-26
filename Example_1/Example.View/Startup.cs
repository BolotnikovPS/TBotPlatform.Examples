﻿using Example1.Bootstrap.Middleware;
using HealthChecks.UI.Client;
using Prometheus;

namespace Example.View;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
           .AddEndpointsApiExplorer()
           .AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
           .UseMiddleware<RequestLoggingMiddleware>()
           .UseRouting()
           .UseCors()
           .UseHttpMetrics(
                options =>
                {
                    //чтобы различать ручки с одинаковым названием метода 
                    options.AddRouteParameter("version");
                })
           .UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();

                    endpoints.MapMetrics();

                    endpoints.MapHealthChecks(
                        "/healthz",
                        new()
                        {
                            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                        });
                });
    }
}