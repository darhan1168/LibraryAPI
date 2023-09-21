using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryApi.CustomAttributes;

public class RoleAuthorizeFilter : IAuthorizationFilter
{
    private readonly string _role;

    public RoleAuthorizeFilter(string role)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (user.Claims.FirstOrDefault(c => c.Value == _role) == null)
        {
            context.Result = new NotFoundResult();
            return;
        }
    }
}