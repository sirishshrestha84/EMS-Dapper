using Dapper;
using EMS_Dapper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace EMS_Dapper.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly DapperApplicationDbContext _db;
        
        public AuthenticationController (DapperApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if(!ModelState.IsValid)
            {
                return View(register);
            }
            using (var connection = _db.CreateConnection())
            {
                var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM [User] WHERE UserEmail = @UserEmail",
                    new { UserEmail = register.UserEmail });
                if (existingUser != null) 
                {
                    ViewBag.Error = "Email Already registered";
                    return View(register);
                }

                //Hash password using Bcrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.UserPassword);
                string insertQuery = @"
                    INSERT INTO [User] (UserEmail, UserPassword , HashPassword, Role)
                    VALUES (@UserEmail, @UserPassword , @HashPassword, @Role)";
                var result = await connection.ExecuteAsync(insertQuery, new
                {
                    UserEmail = register.UserEmail,
                    UserPassword = register.UserPassword,
                    HashPassword = hashedPassword,
                    Role =register.Role
                });
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Registration successful! Please log in.";
                    return RedirectToAction("Login");
                }

                ViewBag.Error = "Something went wrong. Please try again.";
                return View(register);
            }

        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login (string useremail, string userpassword)
        {
            using (var connection = _db.CreateConnection())
            {


                string query = "SELECT * FROM [User] WHERE UserEmail = @UserEmail AND UserPassword = @UserPassword";
                var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { UserEmail = useremail, Userpassword = userpassword });

                if(user != null && BCrypt.Net.BCrypt.Verify(userpassword, user.HashPassword))
                {
                    //HttpContext.Session.SetInt32["UserId"] = user.UserId;
                    //Session["UserEmail"] = user.UserEmail;
                    //Session["UserPassword"] = user.UserPassword;
                    //Session["Role"] = user.Role;
                    //return RedirectToAction("Index");
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserEmail", user.UserEmail);
                    //HttpContext.Session.SetString("UserPassword", user.UserPassword);
                    HttpContext.Session.SetString("Role", user.Role);

                    return RedirectToAction("Index","Home");

                }
                ViewBag.Error = "Invalid Email or password.";
                return View();

            }
        }

        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        //Access denied action
        public IActionResult ActionDenied()
        {
            return View();
        }
    }
}
