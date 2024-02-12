namespace TenantAuthenticator.Interface;

public interface ITokenService
{
    Task<string> CreateToken(string username, string password);
}