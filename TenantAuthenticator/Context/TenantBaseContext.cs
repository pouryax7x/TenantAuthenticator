using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using TenantAuthenticator.Interface;
using LinqKit;
namespace TenantAuthenticator.Context;
public abstract class TenantBaseContext : DbContext
{
    Dictionary<string, bool> IsAbleToRead = new Dictionary<string, bool>();
    public ICurrentTenantService _currentTenantService { get; }

    public TenantBaseContext(ICurrentTenantService currentTenantService)
    {
        _currentTenantService = currentTenantService;
        foreach (var resourcePermission in _currentTenantService.ResourcePermissions)
        {
            AddUnique(IsAbleToRead, resourcePermission.ResourceName, resourcePermission.IsAbleToRead);
        };
    }
    public TenantBaseContext(DbContextOptions options, ICurrentTenantService currentTenantService)
        : base(options)
    {
        _currentTenantService = currentTenantService;
        foreach (var resourcePermission in _currentTenantService.ResourcePermissions)
        {
            AddUnique(IsAbleToRead, resourcePermission.ResourceName, resourcePermission.IsAbleToRead);
        }
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateModifiedProperties();
        PreventFromInsert();
        PreventFromDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void PreventFromDelete()
    {
        var AddedEntry = ChangeTracker.Entries()
          .Where(e => e.State == EntityState.Deleted)
          .ToList();

        foreach (var entry in AddedEntry)
        {
            var InsertPermitRefused = _currentTenantService.ResourcePermissions.Any(x => x.ResourceName == GetTableName(entry) && x.IsAbleToDelete == false);

            if (InsertPermitRefused)
            {
                entry.State = EntityState.Unchanged;
            }
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var entityTypeClrType = entityType.ClrType;

            // Find and call the ConfigureGlobalFilters method using reflection
            MethodInfo method = GetType().GetMethod("ConfigureGlobalFilters", BindingFlags.Instance | BindingFlags.NonPublic);

            if (method != null)
            {
                MethodInfo genericMethod = method.MakeGenericMethod(entityTypeClrType);
                genericMethod.Invoke(this, new object[] { modelBuilder });
            }
            else
            {
                throw new Exception();
            }
        }
    }
    private void PreventFromInsert()
    {
        var AddedEntry = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added)
            .ToList();

        foreach (var entry in AddedEntry)
        {
            var InsertPermitRefused = _currentTenantService.ResourcePermissions.Any(x => x.ResourceName == GetTableName(entry) && x.IsAbleToInsert == false);

            if (InsertPermitRefused)
            {
                entry.State = EntityState.Detached;
            }
        }
    }

    protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder) where TEntity : class
    {
        string resourceName = typeof(TEntity).Name;
        Expression<Func<TEntity, bool>> selectFilter = e => true == IsAbleToRead[resourceName];
        //modelBuilder.Entity<TEntity>().HasQueryFilter(selectFilter);
        Expression<Func<TEntity, bool>> multiTenantFilter = e => true;
        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
        {
            multiTenantFilter = e => EF.Property<Guid>(e, "TenantId") == _currentTenantService.TenantId;
        }
        //// Create a lambda expression from the combined expression
        var lambda = selectFilter.And(multiTenantFilter);
        modelBuilder.Entity<TEntity>().HasQueryFilter(lambda);

    }



    private void UpdateModifiedProperties()
    {
        var modifiedEntries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .ToList();

        foreach (var entry in modifiedEntries)
        {
            var propertyNames = new string[] { "Username" };

            if (GetTableName(entry) == "Tenant")
            {
                foreach (var property in entry.OriginalValues.Properties)
                {
                    var propertyName = property.Name;

                    if (!propertyNames.Contains(propertyName))
                    {
                        entry.Property(propertyName).IsModified = false;
                    }
                }
            }
        }
    }

    private static string GetTableName(EntityEntry entry)
    {
        var entityType = entry.Metadata;
        return entityType?.GetTableName();
    }

    private static void AddUnique(Dictionary<string, bool> list, string key, bool value)
    {
        try
        {
            list.Add(key, value);
        }
        catch (Exception)
        {
        }

    }
}
