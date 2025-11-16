using Infrastructure;
using Infrastructure.HttpServices;

namespace WebUi;

public static class DependencyInjections
{
    public static IServiceCollection AddWebUi(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddOptions<AuthRepository.Options>()
            .BindConfiguration(AuthRepository.Options.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddInfrastructure();
        return services;
    }
}
