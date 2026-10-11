using ASME_Community_API.Authorization;
using BAL.Services;
using Microsoft.AspNetCore.Authorization;

public class SuperAdminOrAdminOrUserHandler
    : AuthorizationHandler<SuperAdminOrAdminOrUserRequirement, int>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SuperAdminOrAdminOrUserRequirement requirement,
        int resourceUserId)
    {
        // 1. SuperAdmin → full access
        if (context.User.IsInRole("SuperAdmin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

     
        var userIdClaim = context.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return Task.CompletedTask;
        }

        if (context.User.IsInRole("Admin"))
        {
            if (!Helper.IsBlocked(userId)) 
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        if (context.User.IsInRole("User"))
        {
            if (userId == resourceUserId && Helper.IsActive(userId))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}