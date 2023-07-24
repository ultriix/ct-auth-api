﻿using System.ComponentModel.DataAnnotations;

namespace CtAuthAPI.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}