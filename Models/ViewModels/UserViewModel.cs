using System;

namespace UserManagementApp.Models.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime RegistrationTime { get; set; }
        public bool IsBlocked { get; set; }
    }
}