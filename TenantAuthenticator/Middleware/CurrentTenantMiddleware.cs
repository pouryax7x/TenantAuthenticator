﻿using Microsoft.AspNetCore.Http;
using TenantAuthenticator.Entity;
using TenantAuthenticator.Interface;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
namespace TenantAuthenticator.Middleware;
public class CurrentTenantMiddleware
{
    private readonly RequestDelegate _next;
    public CurrentTenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ICurrentTenantService currentTenantService, ITenantContext tenantContext)
    {
        Guid tenantId = Guid.NewGuid();
        try
        {
            tenantId = Guid.Parse(context.User?.FindFirstValue("TenantId"));
            currentTenantService.SetTenantId(tenantId);
            var roleResourcePermissionIds = context.User?.Claims.Where(x => x.Type == "RPI").Select(y => y.Value).ToList();
            var RoleResourcePermissions = tenantContext.ResourcePermissions.Where(x => roleResourcePermissionIds.Any(z => z == x.Id.ToString())).ToList();
            foreach (var roleResourcePermission in RoleResourcePermissions)
            {
                var editableFields = roleResourcePermission.EditableFields.Split(',').ToList();
                currentTenantService.ResourcePermissions.Add(new ResourcePromiss
                {
                    EditableFields = editableFields,
                    IsAbleToDelete = roleResourcePermission.CanDelete,
                    IsAbleToInsert = roleResourcePermission.CanInsert,
                    IsAbleToRead = roleResourcePermission.CanSelect,
                    ResourceName = roleResourcePermission.ResourceName
                });
            }
        }
        catch (Exception)
        {
            currentTenantService.SetTenantId(tenantId);
        }
        finally
        {
            await _next(context);
        }
    }
}

public static class CurrentTenantMiddlewareExtensions
{
    public static IApplicationBuilder UseCurrentTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CurrentTenantMiddleware>();
    }
}
