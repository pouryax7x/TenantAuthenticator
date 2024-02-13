namespace TenantAuthenticator.Entity.Tenant;

public partial class TenantRole
{
    public int Id { get; set; }

    public DateTime ExpireDate { get; set; }

    public Guid TargetTenantId { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Tenant TargetTenant { get; set; } = null!;
}
