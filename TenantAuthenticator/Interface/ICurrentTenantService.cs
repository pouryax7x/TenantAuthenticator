using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantAuthenticator.Entity;

namespace TenantAuthenticator.Interface;
public interface ICurrentTenantService
{
    private static Guid TenantIdStatic;

    public Guid TenantId { get; }

    public void SetTenantId(Guid tenantId);
    public static Guid GetTenantId()
    {
        return TenantIdStatic;
    }
    public List<ResourcePromiss> ResourcePermissions { get; set; }
}
