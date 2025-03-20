using System;

namespace UserManagementApp.Models.ViewModels
{
    public class UserViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime RegistrationTime { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}