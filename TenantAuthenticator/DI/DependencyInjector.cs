using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TenantAuthenticator.Context;
using TenantAuthenticator.Interface;
using TenantAuthenticator.Services.Auth;
using TenantAuthenticator.Services.Tenant;

namespace TenantAuthenticator.DI;
public static class DependencyInjector
{
    public static IServiceCollection AddTenantDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<ITenantContext>(provider => provider.GetRequiredService<TenantContext>());
        _ = services.AddScoped<ICurrentTenantService, CurrentTenantService>();
        _ = services.AddScoped<ITokenService, TokenService>();


        _ = services.AddDbContext<TenantContext>(options =>
        {
            _ = options.UseSqlServer(configuration.GetConnectionString("Tenant"),
            builder => builder.MigrationsAssembly(typeof(TenantContext).Assembly.FullName));
            _ = options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        });
        _ = services.AddTenantAuthentication(configuration);
        _ = services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        return services;
    }
}
