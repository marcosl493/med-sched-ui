using Application.Interfaces.Repositories;
using Infrastructure.HttpHandlers;
using Infrastructure.HttpServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<MedSchedAuthHandler>();
        services.AddHttpClient<IAuthRepository, AuthRepository>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<AuthRepository.Options>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });
        services.AddHttpClient<IMedSchedRepository, MedSchedRepository>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<MedSchedRepository.Options>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        }).AddHttpMessageHandler<MedSchedAuthHandler>();
        services.AddLogging();
        return services;
    }
    private static IServiceCollection AddLogging(this IServiceCollection services)
    {
        services.AddSerilog((services, lc) => lc
                .ReadFrom.Configuration(services.GetRequiredService<IConfiguration>())
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());
        return services;
    }
}
