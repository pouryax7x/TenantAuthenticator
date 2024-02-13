using Microsoft.EntityFrameworkCore;
using TenantAuthenticator.Entity.Tenant;
using TenantAuthenticator.Interface;

namespace TenantAuthenticator.Context;
public partial class TenantContext : DbContext, ITenantContext
{
    public TenantContext(DbContextOptions<TenantContext> options) : base(options)
    {
    }
    public TenantContext() : base()
    {
    }

    public virtual DbSet<ResourcePermission> ResourcePermissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleResourcePermission> RoleResourcePermissions { get; set; }

    public virtual DbSet<RoleSystemPermission> RoleSystemPermissions { get; set; }

    public virtual DbSet<SystemPermission> SystemPermissions { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<TenantRole> TenantRoles { get; set; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        _ = modelBuilder.Entity<ResourcePermission>(entity =>
        {
            _ = entity.ToTable("ResourcePermission");

            _ = entity.Property(e => e.EditableFields).IsUnicode(false);
            _ = entity.Property(e => e.ResourceName)
                .HasMaxLength(50)
                .IsUnicode(false);

            _ = entity.HasOne(d => d.Tenant).WithMany(p => p.ResourcePermissions)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResourcePermission_Tenant");
        });

        _ = modelBuilder.Entity<Role>(entity =>
        {
            _ = entity.ToTable("Role");

            _ = entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            _ = entity.HasOne(d => d.Tenant).WithMany(p => p.Roles)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_Tenant");
        });

        _ = modelBuilder.Entity<RoleResourcePermission>(entity =>
        {
            _ = entity.ToTable("Role_ResourcePermission");

            _ = entity.Property(e => e.ExpireDate).HasColumnType("datetime");

            _ = entity.HasOne(d => d.ResourcePermission).WithMany(p => p.RoleResourcePermissions)
                .HasForeignKey(d => d.ResourcePermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_ResourcePermission_ResourcePermission1");

            _ = entity.HasOne(d => d.Role).WithMany(p => p.RoleResourcePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_ResourcePermission_Role1");
        });

        _ = modelBuilder.Entity<RoleSystemPermission>(entity =>
        {
            _ = entity.ToTable("Role_SystemPermission");

            _ = entity.Property(e => e.ExpireDate).HasColumnType("datetime");

            _ = entity.HasOne(d => d.Role).WithMany(p => p.RoleSystemPermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_SystemPermission_Role");

            _ = entity.HasOne(d => d.SystemPermission).WithMany(p => p.RoleSystemPermissions)
                .HasForeignKey(d => d.SystemPermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_SystemPermission_SystemPermission");
        });

        _ = modelBuilder.Entity<SystemPermission>(entity =>
        {
            _ = entity.ToTable("SystemPermission");

            _ = entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        _ = modelBuilder.Entity<Tenant>(entity =>
        {
            _ = entity.ToTable("Tenant");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Password).HasMaxLength(50);
            _ = entity.Property(e => e.Username).HasMaxLength(50);
        });

        _ = modelBuilder.Entity<TenantRole>(entity =>
        {
            _ = entity.ToTable("Tenant-Role");

            _ = entity.Property(e => e.ExpireDate).HasColumnType("datetime");

            _ = entity.HasOne(d => d.Role).WithMany(p => p.TenantRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant-Role_Role");

            _ = entity.HasOne(d => d.TargetTenant).WithMany(p => p.TenantRoles)
                .HasForeignKey(d => d.TargetTenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant-Role_Tenant");
        });
    }
}
