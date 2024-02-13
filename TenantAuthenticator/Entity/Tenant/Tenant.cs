namespace TenantAuthenticator.Entity.Tenant;

public partial class Tenant
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<ResourcePermission> ResourcePermissions { get; set; } = new List<ResourcePermission>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<TenantRole> TenantRoles { get; set; } = new List<TenantRole>();
}
