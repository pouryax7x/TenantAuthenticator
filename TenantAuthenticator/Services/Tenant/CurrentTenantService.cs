using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantAuthenticator.Entity;
using TenantAuthenticator.Interface;

namespace TenantAuthenticator.Services.Tenant;
public class CurrentTenantService : ICurrentTenantService
{
    public List<ResourcePromiss> ResourcePermissions { get; set; } = new List<ResourcePromiss>();
    private Guid _tenantId { get; set; }
    private static Guid TenantIdStatic;
    public Guid TenantId
    {
        get
        {
            return _tenantId;
        }
    }
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
