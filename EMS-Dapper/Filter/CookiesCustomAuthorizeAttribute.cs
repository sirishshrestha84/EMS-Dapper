using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EMS_Dapper.Filter
{
    public class CookiesCustomAuthorizeAttribute : Attribute , IAuthorizationFilter

    {
        private readonly string[] _roles;
        public CookiesCustomAuthorizeAttribute(string roles)
        {
            _roles = roles.Split(',').Select(r => r.Trim()).ToArray();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                //Not Logged in 
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return;
            }
            var userRole = user.FindFirstValue(ClaimTypes.Role);
            if(string.IsNullOrEmpty(userRole) || !_roles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
            {
                    // Authenticated, but role mismatch
                    context.Result = new RedirectToActionResult("ActionDenied", "Authentication", null);
                }
            }
    }
}
