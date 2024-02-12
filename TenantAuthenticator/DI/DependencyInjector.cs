using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantAuthenticator.Context;
using TenantAuthenticator.Interface;
using TenantAuthenticator.Services.Auth;
using TenantAuthenticator.Services.Tenant;

namespace TenantAuthenticator.DI;
public static class DependencyInjector
{
    public static IServiceCollection AddTenantDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITenantContext>(provider => provider.GetRequiredService<TenantContext>());
        services.AddScoped<ICurrentTenantService, CurrentTenantService>();
        services.AddScoped<ITokenService, TokenService>();


        services.AddDbContext<TenantContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Tenant"),
            builder => builder.MigrationsAssembly(typeof(TenantContext).Assembly.FullName));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        });
        services.AddTenantAuthentication(configuration);
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        return services;
    }
}
