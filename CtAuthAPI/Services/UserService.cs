﻿using CtAuthAPI.Models;

namespace CtAuthAPI.Services;

public interface IUserService
{
    Task<List<User>> GetUsersAsync();
    Task<User> GetUserAsync(string email, string password);
    Task<User> CreateUserAsync(string name, string email, string password);
}

public class UserService : IUserService
{
    private readonly UserContext _context;

    public UserService(UserContext context)
    {
        _context = context;
    }
    
    public Task<List<User>> GetUsersAsync()
    {
        throw new NotImplementedException();
    }
    
    public Task<User> GetUserAsync(string email, string password)
    {
        throw new NotImplementedException();
    }
    
    public Task<User> CreateUserAsync(string name, string email, string password)
    {
        throw new NotImplementedException();
    }
    
}