using BAL.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASME_Community_API.Authorization
{
    public class SuperAdminOrAdminHandler : AuthorizationHandler<SuperAdminOrAdminRequirement, int>
    {
        protected override Task HandleRequirementAsync(
     AuthorizationHandlerContext context,
     SuperAdminOrAdminRequirement requirement,
     int resourceUserId)
        {
            // 1. SuperAdmin → full access
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //  2. Get logged-in user id once
            var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Task.CompletedTask;
            }

            //  3. Admin → can access ONLY own data
            if (context.User.IsInRole("Admin"))
            {
                if (userId == resourceUserId && Helper.IsActive(userId))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}