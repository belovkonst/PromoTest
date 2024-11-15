using Promo.Domain.Interfaces.Services;
using Promo.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Promo.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicaton(this IServiceCollection services)
    {
        InitServices(services);
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService> ();
        services.AddScoped<IReferenceService, ReferenceService> ();
    }
}
