using System.Security.Claims;
using DoeMais.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace DoeMais.Authorization.Handlers;

public class OwnerOrAdminHandler : AuthorizationHandler<OwnerOrAdminRequirement, IResourceWithUserId>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerOrAdminRequirement requirement,
        IResourceWithUserId resource)
    {
        var userIdClaim = context.User.FindFirst("UserId")?.Value;
        var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value
            ?? context.User.FindFirst("role")?.Value;

        if (roleClaim == "Admin")
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (userIdClaim != null && resource.UserId == long.Parse(userIdClaim))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}