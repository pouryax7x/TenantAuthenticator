namespace TenantAuthenticator.Entity.Tenant;

public partial class ResourcePermission
{
    public int Id { get; set; }

    public Guid TenantId { get; set; }

    public string EditableFields { get; set; } = null!;

    public string ResourceName { get; set; } = null!;

    public bool CanUpdate { get; set; }

    public bool CanDelete { get; set; }

    public bool CanSelect { get; set; }

    public bool CanInsert { get; set; }

    public virtual ICollection<RoleResourcePermission> RoleResourcePermissions { get; set; } = new List<RoleResourcePermission>();

    public virtual Tenant Tenant { get; set; } = null!;
}
