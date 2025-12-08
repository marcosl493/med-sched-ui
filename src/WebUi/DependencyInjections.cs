using Infrastructure;
using Infrastructure.HttpServices;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebUi;

public static class DependencyInjections
{
    public static IServiceCollection AddWebUi(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
        services.ConfigureApplicationCookie(config =>
        {
            config.Cookie.HttpOnly = true;
            config.LoginPath = "/Auth/Login";
            config.LogoutPath = "/Auth/Signout";
            config.AccessDeniedPath = "/Auth/AccessDenied";

        });
        services.AddOptions<AuthRepository.Options>()
            .BindConfiguration(AuthRepository.Options.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<MedSchedRepository.Options>()
            .BindConfiguration(MedSchedRepository.Options.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpContextAccessor();
        services.AddInfrastructure();
        return services;
    }
}
