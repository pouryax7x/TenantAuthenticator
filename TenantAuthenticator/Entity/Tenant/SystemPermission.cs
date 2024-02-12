using System;
using System.Collections.Generic;

namespace TenantAuthenticator.Entity.Tenant;

public partial class SystemPermission
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RoleSystemPermission> RoleSystemPermissions { get; set; } = new List<RoleSystemPermission>();
}


