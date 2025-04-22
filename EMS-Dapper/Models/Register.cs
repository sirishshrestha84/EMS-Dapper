using Microsoft.AspNetCore.Identity;

namespace EMS_Dapper.Models
{
    public class Register
    {
        public string? UserEmail { get; set; }
        public string? UserPassword {get; set; }
        public string? HashPassword { get; set; }
        public string? Role {  get; set; }
    }
}
