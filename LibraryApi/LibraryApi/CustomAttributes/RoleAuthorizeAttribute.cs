using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.CustomAttributes;

public class RoleAuthorizeAttribute : TypeFilterAttribute
{
    public RoleAuthorizeAttribute(string role) : base(typeof(RoleAuthorizeFilter))
    {
        Arguments = new object[] { role };
    }
}