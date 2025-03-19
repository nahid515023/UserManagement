using Microsoft.AspNetCore.Identity;
using System;

namespace UserManagementApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
        public bool IsBlocked { get; set; } = false;
    }
}