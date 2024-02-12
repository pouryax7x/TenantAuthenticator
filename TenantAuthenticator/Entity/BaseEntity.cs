using TenantAuthenticator.Interface;
using TenantAuthenticator.Services.Common;
using TenantAuthenticator.Services.Tenant;

namespace TenantAuthenticator.Entity;
public abstract class BaseEntity
{
    protected BaseEntity()
    {
        if (this is not IMultiTenant entity)
        {
            return;
        }
        var tenantId = CurrentTenantService.GetTenantId();
        ObjectHelper.TrySetProperty(entity, x => x.TenantId, () => tenantId);
    }
}
