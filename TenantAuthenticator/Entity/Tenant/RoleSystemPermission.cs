using System;
using System.Collections.Generic;

namespace TenantAuthenticator.Entity.Tenant;

public partial class RoleSystemPermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int SystemPermissionId { get; set; }

    public DateTime ExpireDate { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual SystemPermission SystemPermission { get; set; } = null!;
}
