using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;


namespace CtAuthAPI.Services;

public interface IAuthService
{
    string GenerateJwtToken(string email);
}

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly string _secretKey;

    public AuthService(IConfiguration config, ILogger<AuthService> logger)
    {
        _secretKey = config.GetSection("AuthSettings:SecretKey").Value;
        _logger = logger;
    }

    public string GenerateJwtToken(string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}