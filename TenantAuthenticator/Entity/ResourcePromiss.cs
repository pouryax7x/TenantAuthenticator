using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantAuthenticator.Entity;
public class ResourcePromiss
{
    public string ResourceName { get; set; }
    public bool IsAbleToDelete { get; set; }
    public bool IsAbleToInsert { get; set; }
    public bool IsAbleToRead { get; set; }
    public List<string> EditableFields { get; set; }
}
