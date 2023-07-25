using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CtAuthAPI.Models;
using CtAuthAPI.Services;


namespace CtAuthAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    
    /// <summary>
    /// Requires valid auth token, returns all users in the database.
    /// </summary>
    /// <returns>All users in the database.</returns>
    [Authorize]
    [HttpGet("ListUsers")]
    public async Task<IActionResult> ListUsers()
    {
        List<User> users = await _userService.GetUsersAsync();
        
        return Ok(users);
    }

    /// <summary>
    /// Creates a user in the database.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(string name, string email, string password)
    {
        User user = await _userService.CreateUserAsync(name, email, password);
        
        return CreatedAtAction("CreateUser", user);
    }

    /// <summary>
    /// Returns a valid Jwt Token for an existing user.
    ///
    /// Valid credentials are required.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost("GetJwtToken")]
    public async Task<IActionResult> GetJwtToken(string email, string password)
    {
        // Verify the user exists
        User? user = await _userService.GetUserAsync(email, password);

        // If no user was matched, return blanket 401
        if (user == null)
            return Unauthorized();

        // If credentials are valid, generate a JWT
        string token = _authService.GenerateJwtToken(user.Email);

        // Return the token
        return Ok(token);
    }
    
}