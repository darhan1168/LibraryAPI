using System.Security.Claims;
using LibraryApi.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApiUnitTests;

public class GenericControllerTestsSettings<TController> where TController : Controller
{
    protected void SettingsHttpConnection(TController controller)
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "username"), 
                new Claim(ClaimTypes.Role, UserRole.Admin.ToString())
            }, "test"))
        };
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }
}