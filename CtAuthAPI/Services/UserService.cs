using Microsoft.EntityFrameworkCore;
using CtAuthAPI.Models;
using CtAuthAPI.Utils;

namespace CtAuthAPI.Services;

public interface IUserService
{
    /// <summary>
    /// For development, will seed the database with test users.
    /// </summary>
    Task SeedUsersAsync();
    
    /// <summary>
    /// Returns all users in the database.
    /// </summary>
    /// <returns></returns>
    Task<List<User>> GetUsersAsync();
    
    /// <summary>
    /// Gets an existing user matching an email and plain password.
    ///
    /// The password will be verified against the matching users hashed password.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<User?> GetUserAsync(string email, string password);
    
    /// <summary>
    /// Creates a user in the database.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<User> CreateUserAsync(string name, string email, string password);
}

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserContext _context;

    public UserService(UserContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    
    public async Task SeedUsersAsync()
    {
        if (_context.Users.Any()) return;
        
        await CreateUserAsync("TEST USER 1", "user1@example.com", "USER1_PASSWORD");
        await CreateUserAsync("TEST USER 2", "user2@example.com", "USER2_PASSWORD");
        await CreateUserAsync("TEST USER 3", "user3@example.com", "USER3_PASSWORD");
    }
    
    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<User?> GetUserAsync(string email, string password)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        // return null if either no user exists or password is incorrect
        if(user == null || !PasswordUtils.VerifyPassword(user.Password, password))
            return null;
        
        return user;
    }
    
    public async Task<User> CreateUserAsync(string name, string email, string password)
    {
        var user = new User
        {
            Name = name,
            Email = email,
            Password = PasswordUtils.HashPassword(password)
        };
        
        // store in db
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return user;
    }

}