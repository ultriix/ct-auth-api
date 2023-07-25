using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CtAuthAPI.Models;
using CtAuthAPI.Services;


namespace CtAuthAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(ILogger<UserController> logger, IUserService userService, IAuthService authService)
    {
        _logger = logger;
        _userService = userService;
        _authService = authService;
    }

    [Authorize]
    [HttpGet("ListUsers")]
    public async Task<IActionResult> GetAll()
    {
        return StatusCode(418);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        return StatusCode(418);
    }

    [HttpPost("GetJwtToken")]
    public async Task<IActionResult> GetJwtToken(string email, string password)
    {
        // TODO: Get user first to ensure they exist

        // If credentials are valid, generate a JWT
        string token = _authService.GenerateJwtToken(email);

        // Return the token
        return Ok(token);
    }
    
}