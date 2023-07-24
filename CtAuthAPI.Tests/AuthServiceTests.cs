using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using CtAuthAPI.Services;

namespace CtAuthAPI.Tests;

[TestFixture]
public class AuthServiceTests
{
    private Mock<ILogger<AuthService>> _mockLogger;
    private IAuthService _authService;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<AuthService>>();
        
        // assign sut
        _authService = new AuthService(_mockLogger.Object);
    }

    [Test]
    public void GenerateJwtToken_GivenValidEmail_ShouldGenerateJwtToken()
    {
        string email = "user1@example.com";
        string token = _authService.GenerateJwtToken(email);

        Assert.That(token, Is.Not.Null.Or.Empty);

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwtToken = handler.ReadJwtToken(token);

        Assert.That(jwtToken?.Subject, Is.EqualTo(email));
    }
    
}
