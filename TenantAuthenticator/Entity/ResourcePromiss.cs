namespace TenantAuthenticator.Entity;
public class ResourcePromiss
{
    public required string ResourceName { get; set; }
    public bool IsAbleToDelete { get; set; }
    public bool IsAbleToInsert { get; set; }
    public bool IsAbleToRead { get; set; }
    public required List<string> EditableFields { get; set; }
}
