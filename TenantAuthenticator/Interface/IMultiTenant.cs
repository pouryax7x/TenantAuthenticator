namespace TenantAuthenticator.Interface;
public interface IMultiTenant
{
    public Guid TenantId { get; set; }
}
