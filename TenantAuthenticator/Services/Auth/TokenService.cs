using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TenantAuthenticator.Interface;
namespace TenantAuthenticator.Services.Auth;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ITenantContext _tenantContext;
    private readonly ICurrentTenantService _currentTenantService;

    public TokenService(IConfiguration configuration, ITenantContext tenantContext, ICurrentTenantService currentTenantService)
    {
        _configuration = configuration;
        _tenantContext = tenantContext;
        _currentTenantService = currentTenantService;
    }


    public string CreateToken(string username, string password)
    {
        Entity.Tenant.Tenant? tenant = _tenantContext.Tenants.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
        if (tenant == null) { throw new Exception(); };
        _currentTenantService.SetTenantId(tenant.Id);
        List<Entity.Tenant.TenantRole> tenantRoles = _tenantContext.TenantRoles
            .Include(x => x.Role).ThenInclude(x => x.RoleSystemPermissions)
            .Include(x => x.Role).ThenInclude(x => x.RoleResourcePermissions)
            .Where(x => x.TargetTenantId == tenant.Id && x.ExpireDate >= DateTime.Now)
            .ToList();
        List<int> ResourcePermissionIdList = [];
        List<int> SystemPermissionIdList = [];

        foreach (Entity.Tenant.TenantRole? tenantRole in tenantRoles)
        {
            List<int> tenantResourcePermissions = tenantRole.Role.RoleResourcePermissions.Where(x => x.ExpireDate >= DateTime.Now).Select(x => x.ResourcePermissionId).ToList();
            List<int> tenantSystemPermissions = tenantRole.Role.RoleSystemPermissions.Where(x => x.ExpireDate >= DateTime.Now).Select(x => x.SystemPermissionId).ToList();
            AddUnique(ResourcePermissionIdList, tenantResourcePermissions);
            AddUnique(SystemPermissionIdList, tenantSystemPermissions);
        }

        // authentication successful so generate jwt token
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_configuration.GetSection("AuthKey").Value);
        ClaimsIdentity claims = new();
        claims.AddClaim(new Claim("TenantId", tenant.Id.ToString()));
        foreach (int RPI in ResourcePermissionIdList)
        {
            claims.AddClaim(new Claim("RPI", RPI.ToString()));
        }
        foreach (int SPI in SystemPermissionIdList)
        {
            claims.AddClaim(new Claim("SPI", SPI.ToString()));
        }


        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(999999),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }

    private static void AddUnique(List<int> list, List<int>? numbers)
    {
        if (numbers == null)
        {
            return;
        }
        foreach (int number in numbers)
        {
            if (!list.Contains(number))
            {
                list.Add(number);
            }
        }
    }

    Task<string> ITokenService.CreateToken(string username, string password)
    {
        throw new NotImplementedException();
    }
}

