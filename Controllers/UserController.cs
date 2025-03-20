using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Models;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!await IsUserActiveAsync())
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            var users = _userManager.Users
                .OrderByDescending(u => u.LastLoginTime)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = u.Name ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    LastLoginTime = u.LastLoginTime,
                    RegistrationTime = u.RegistrationTime,
                    IsBlocked = u.IsBlocked
                }).ToList();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsers(List<string> userIds)
        {
            if (!await IsUserActiveAsync())
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsBlocked = true;
                    await _userManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsers(List<string> userIds)
        {
            if (!await IsUserActiveAsync())
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.IsBlocked = false;
                    await _userManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(List<string> userIds)
        {
            if (!await IsUserActiveAsync())
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }
            return RedirectToAction("Index");
        }

        private async Task<bool> IsUserActiveAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user != null && !user.IsBlocked;
        }
    }
}