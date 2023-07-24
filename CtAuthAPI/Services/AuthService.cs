using CtAuthAPI.Models;

namespace CtAuthAPI.Services;

public interface IAuthService
{
    string GenerateToken(string email);
}

public class AuthService : IAuthService
{
    private readonly UserContext _context;

    public AuthService(UserContext context)
    {
        _context = context;
    }

    public string GenerateToken(string email)
    {
        throw new NotImplementedException();
    }
    
}