using Microsoft.AspNetCore.Authorization;

namespace TenantAuthenticator.Entity.Auth;
public class SystemAuthorizeAttribute : AuthorizeAttribute
{
    public SystemAuthorizeAttribute(AuthorizeEnum authorizePlicies)
    {
        Policy = $"{(int)authorizePlicies}";
    }
}