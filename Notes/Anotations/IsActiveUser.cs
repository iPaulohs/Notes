using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Notes.Services;
using System.Security.Claims;

namespace Notes.Anotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class IsActiveUser : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if(user != null && user.Identity.IsAuthenticated)
        {
            var userService = context.HttpContext.RequestServices.GetService<IUserService>();
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isActive = await userService.IsUserActiveAsync(userId);

            if (!isActive)
            {
                context.Result = new ObjectResult(new { error = "O usuário está desativado. Ative-o para realizar esta ação." })
                {
                    StatusCode = 403
                };
                return;
            }
        }
    }
}
