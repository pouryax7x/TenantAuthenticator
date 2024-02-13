using TenantAuthenticator.Entity;
using TenantAuthenticator.Interface;

namespace TenantAuthenticator.Services.Tenant;
public class CurrentTenantService : ICurrentTenantService
{
    public List<ResourcePromiss> ResourcePermissions { get; set; } = [];
    private Guid _tenantId { get; set; }
    private static Guid TenantIdStatic;
    public Guid TenantId => _tenantId;
    public static Guid GetTenantId()
    {
        return TenantIdStatic;
    }
    public void SetTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        TenantIdStatic = tenantId;
    }
}
