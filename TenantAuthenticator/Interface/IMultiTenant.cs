using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantAuthenticator.Interface;
public interface IMultiTenant
{
    public Guid TenantId { get; set; }
}
