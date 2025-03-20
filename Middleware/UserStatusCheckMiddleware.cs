using Microsoft.AspNetCore.Identity;
using UserManagementApp.Models;

namespace UserManagementApp.Middleware
{
    public class UserStatusCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserStatusCheckMiddleware> _logger;

        public UserStatusCheckMiddleware(RequestDelegate next, ILogger<UserStatusCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            if (context.Request.Path.StartsWithSegments("/Account/Login") || 
                context.Request.Path.StartsWithSegments("/Account/Register") || 
                context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var user = await userManager.GetUserAsync(context.User);
            if (user == null || user.IsBlocked)
            {
                _logger.LogInformation("User is blocked or deleted. Redirecting to login page.");
                await signInManager.SignOutAsync();
                context.Response.Redirect("/Account/Login");
                return;
            }
            user.LastLoginTime = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            await _next(context);
        }
    }

    public static class UserStatusCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserStatusCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserStatusCheckMiddleware>();
        }
    }
}