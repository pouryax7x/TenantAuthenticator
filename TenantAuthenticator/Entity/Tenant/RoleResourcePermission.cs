using System;
using System.Collections.Generic;

namespace TenantAuthenticator.Entity.Tenant;

public partial class RoleResourcePermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int ResourcePermissionId { get; set; }

    public DateTime ExpireDate { get; set; }

    public virtual ResourcePermission ResourcePermission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
