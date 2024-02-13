using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
namespace TenantAuthenticator.Services.Auth;
public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {

        //return base.GetPolicyAsync(policyName);

        string[] systemPermissionIds = policyName.Split(',');

        AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
            .RequireClaim("SPID", systemPermissionIds)
            .Build();

        return Task.FromResult(policy);
    }
}
