using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        modelBuilder.Entity<ResourcePermission>(entity =>
        {
            entity.ToTable("ResourcePermission");

            entity.Property(e => e.EditableFields).IsUnicode(false);
            entity.Property(e => e.ResourceName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Tenant).WithMany(p => p.ResourcePermissions)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResourcePermission_Tenant");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Roles)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_Tenant");
        });

        modelBuilder.Entity<RoleResourcePermission>(entity =>
        {
            entity.ToTable("Role_ResourcePermission");

            entity.Property(e => e.ExpireDate).HasColumnType("datetime");

            entity.HasOne(d => d.ResourcePermission).WithMany(p => p.RoleResourcePermissions)
                .HasForeignKey(d => d.ResourcePermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_ResourcePermission_ResourcePermission1");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleResourcePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_ResourcePermission_Role1");
        });

        modelBuilder.Entity<RoleSystemPermission>(entity =>
        {
            entity.ToTable("Role_SystemPermission");

            entity.Property(e => e.ExpireDate).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleSystemPermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_SystemPermission_Role");

            entity.HasOne(d => d.SystemPermission).WithMany(p => p.RoleSystemPermissions)
                .HasForeignKey(d => d.SystemPermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_SystemPermission_SystemPermission");
        });

        modelBuilder.Entity<SystemPermission>(entity =>
        {
            entity.ToTable("SystemPermission");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("Tenant");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<TenantRole>(entity =>
        {
            entity.ToTable("Tenant-Role");

            entity.Property(e => e.ExpireDate).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.TenantRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant-Role_Role");

            entity.HasOne(d => d.TargetTenant).WithMany(p => p.TenantRoles)
                .HasForeignKey(d => d.TargetTenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant-Role_Tenant");
        });
    }
}
