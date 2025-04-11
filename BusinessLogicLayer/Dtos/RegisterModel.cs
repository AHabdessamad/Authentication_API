﻿using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Models
{
    public enum Role { Admin, User } 
    public class RegisterModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;

    }
}
