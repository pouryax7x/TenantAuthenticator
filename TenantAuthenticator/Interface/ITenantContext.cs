using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantAuthenticator.Entity.Tenant;

namespace TenantAuthenticator.Interface;
public interface ITenantContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public DbSet<ResourcePermission> ResourcePermissions { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<RoleResourcePermission> RoleResourcePermissions { get; set; }

    public DbSet<RoleSystemPermission> RoleSystemPermissions { get; set; }

    public DbSet<SystemPermission> SystemPermissions { get; set; }

    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TenantRole> TenantRoles { get; set; }
}
