using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("ListUsers")]
    // TODO: lock by bearer auth
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
    public async Task<IActionResult> Authenticate(string email, string password)
    {
        return StatusCode(418);
    }
}