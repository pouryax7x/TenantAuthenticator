using System;
using System.Collections.Generic;

namespace TenantAuthenticator.Entity.Tenant;

public partial class Role
{
    public int Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; } = new List<RoleResourcePermission>();

    public virtual ICollection<RoleSystemPermission> RoleSystemPermissions { get; set; } = new List<RoleSystemPermission>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<TenantRole> TenantRoles { get; set; } = new List<TenantRole>();
}
