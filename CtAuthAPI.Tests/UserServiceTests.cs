using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using CtAuthAPI.Models;
using CtAuthAPI.Services;

namespace CtAuthAPI.Tests;

[TestFixture]
public class UserServiceTests
{
    private Mock<ILogger<UserService>> _mockLogger;
    private UserContext _context;
    private IUserService _userService;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<UserService>>();
        
        // get options for in memory db to using for context testing
        var options = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: "UserDB")
            .Options;
        
        // assign in memory db context for tests
        _context = new UserContext(options);
        
        // assign sut
        _userService = new UserService(_context, _mockLogger.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
    }
    
    [Test]
    public async Task GetUsersAsync_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new User { Name = "TEST USER 1", Email = "user1@example.com", Password = "USER1_PASSWORD"},
            new User { Name = "TEST USER 2", Email = "user2@example.com", Password = "USER2_PASSWORD"}
        };
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    
        List<User> results = await _userService.GetUsersAsync();
    
        Assert.IsNotNull(results);
        Assert.IsInstanceOf<List<User>>(results);
        Assert.That(results.Count, Is.EqualTo(users.Count));
    }
    
    [Test]
    public async Task GetUser_ReturnsUser()
    {
        // setup
        var user = new User { Name = "TEST USER 2", Email = "user2@example.com", Password = "USER2_PASSWORD"};
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // act
        User result = await _userService.GetUserAsync(user.Email, user.Password);
    
        // assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<User>(result);
    
        Assert.That(result.Email, Is.EqualTo(user.Email));
        Assert.That(result.Password, Is.Not.EqualTo(user.Password)); // assert we are not returning the original password
    }
    
    [Test]
    public async Task CreateUserAsync_ProvidedUserDetails_CreatesUser()
    {
        // setup
        var user = new User { Name = "TEST USER 1", Email = "user1@example.com", Password = "USER1_PASSWORD"};

        // act
        User result = await _userService.CreateUserAsync(user.Name, user.Email, user.Password);

        // assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<User>(result);

        Assert.That(user.Name, Is.EqualTo(result.Name));
        Assert.That(user.Email, Is.EqualTo(result.Email));
        Assert.That(user.Password, Is.Not.EqualTo(result.Password)); // assert we are hashing the password
        
        // check the context and the password is not the original
        Assert.That(_context.Users.Any(u => u.Name == user.Name && u.Email == user.Email && u.Password != user.Password), Is.True);
    }
    
}
