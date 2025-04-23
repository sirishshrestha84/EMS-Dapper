using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EMS_Dapper.Filter
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _role;

        public CustomAuthorizeAttribute(string role)
        {
            _role = role.Split(',');
        }

        //public void OnAuthorization (AuthorizationFilterContext context)
        //{
        //    var userRole = context.HttpContext.Session.GetString("Role");
        //    var userid = context.HttpContext.Session.GetInt32("UserId");

        //    if(userid == null)
        //    {
        //        context.Result = new RedirectToActionResult("Login", "Authentication", null);
        //    }

        //    if (string.IsNullOrEmpty(userRole) || !_role.Any(role =>
        //       role.Trim().Equals(userRole, StringComparison.OrdinalIgnoreCase)))
        //    {
        //        // Redirect to AccessDenied or Login if role not matched
        //        context.Result = new RedirectToActionResult("ActionDenied", "Authentication", null);
        //    }
        //}
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetString("Role");
            var userid = context.HttpContext.Session.GetInt32("UserId");

            if (userid == null)
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return; // ✅ Exit here to prevent further checks
            }

            if (string.IsNullOrEmpty(userRole) || !_role.Any(role =>
                role.Trim().Equals(userRole, StringComparison.OrdinalIgnoreCase)))
            {
                context.Result = new RedirectToActionResult("ActionDenied", "Authentication", null);
            }
        }


    }
}
