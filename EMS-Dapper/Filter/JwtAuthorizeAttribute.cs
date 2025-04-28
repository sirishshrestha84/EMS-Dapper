using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt; // For JwtSecurityTokenHandler
using Microsoft.IdentityModel.Tokens; // For TokenValidationParameters and SymmetricSecurityKey
using System.Text; // For Encoding.UTF8

namespace EMS_Dapper.Filter
{
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;
       
        //Constructor to initialize roles, assuming roles are passed as comma-separated string
        public JwtAuthorizeAttribute(string roles)
        {
            _roles = roles.Split(',');
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Get the JWT token from the Authorization header
            //var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            //if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}
            var token = context.HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token not found in cookies.");
                context.Result = new UnauthorizedResult();
                return;
            }
            Console.WriteLine($"Token: {token}");


            //var token = authorizationHeader.Substring("Bearer".Length).Trim();

            try
            {
                //Decode and validate JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                //var key = Encoding.UTF8.GetBytes("a333e12a452ca80776cbe1cdb27c6b0ff794ac4f73d928af499695042ed07c3ef2193f6fae88bb61c41e77cd138cfe75f77946132d63c6516455e0f24198cc7cd2c18188244ec476e3985ab1fb793da3e764402a7814eeb504c451ed3dfc176c0350cce1eef415940a30a7af121923cee3fc9b95229ed39cb802922b9c6fdf76289e610f7a85650250a12ed1253c806c7f69021560e4e1ba2e6a294ff7212d04f6c40ebc5506860ce1679ddcb5d35a407ce2dfd4ec4d67b823f77ed4ed15277d52633b9189f2f17cf43184080f3a76ea553a4aac7cc3c0ddf4344cf9728f79b66d28930c7803bf64802eaa3174bff2d2053cdfbbdccb38b91b9ad3906b32caeb");
                //Get key from configuration
                var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var key = Encoding.UTF8.GetBytes(config["JWT:SecretKey"]);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JWT:Issuer"],
                    ValidAudience = config["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                //Attach the user principal to the context for user in controllers
                context.HttpContext.User = principal;

                //Get the roles from the validated token
                var userRoles = principal.FindAll(System.Security.Claims.ClaimTypes.Role)
                                          .Select(c => c.Value)
                                          .ToList();

                //Check if the user has any of the roles reuqired by this action
                if (_roles.Any() && !_roles.Any(role => userRoles.Contains(role)))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (Exception ex)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtAuthorizeAttribute>>();
                Console.WriteLine($"JWT Authorization failed: {ex.Message}");
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
