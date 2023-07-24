
namespace CtAuthAPI.Services;

public interface IAuthService
{
    string GenerateJwtToken(string email);
}

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    
    public AuthService(ILogger<AuthService> logger)
    {
        _logger = logger;
    }

    public string GenerateJwtToken(string email)
    {
        throw new NotImplementedException();
    }
    
}