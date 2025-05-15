using EMS_Dapper;
using EMS_Dapper.Filter;
using EMS_Dapper.Repository;
using EMS_Dapper.Repository.IRepository;
using EMS_Dapper.Unit_Of_Work;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DapperApplicationDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();


////Session 
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(5);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//Read Jwt Setting frm config
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];

//Cookies 
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //This is cookie
//    .AddCookie( options =>
//    {
//        options.LoginPath = "/Authentication/Login";
//        options.AccessDeniedPath = "/Authentication/ActionDenied";
//        options.Cookie.Name = "EMS_Auth";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
//        options.SlidingExpiration = true;
//        options.Cookie.SameSite = SameSiteMode.Strict;
//    });


////JWT Authentication
//var key = Encoding.ASCII.GetBytes(builder.Configuration["jwt setting : Secretkey"]);
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = false;
//        options.SaveToken = true;
//        options.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = context =>
//            {
//                //Extract Jwt From Cookies instead of Authorzation header
//                var token = context.Request.Cookies["AuthToken"];
//                if (!string.IsNullOrEmpty(token))
//                {
//                    context.Token = token;
//                }
//                return Task.CompletedTask;
//            }
//        };
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            IssuerSigningKey = new SymmetricSecurityKey(key),
//            ValidIssuer = builder.Configuration["jwt setting: Issuer"],
//            ValidAudience = builder.Configuration["jwt setting : Audience"],
//            ClockSkew = TimeSpan.Zero
//        };
//    });


//Combined authentication Setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // ✅ Get JWT from cookie instead of Authorization header
            var token = context.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };

    options.Events.OnChallenge = context =>
    {
        context.HandleResponse();
        context.Response.Redirect("/Authentication/ActionDenied");
        return Task.CompletedTask;
    };
});

//Add Authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
//});


builder.Services.AddAuthorization();
//builder.Services.AddControllersWithViews();

var app = builder.Build();

//Rotativa 
RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
