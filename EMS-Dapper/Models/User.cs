namespace EMS_Dapper.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserEmail { get; set; }
        
        public string? UserPassword { get; set; }
        public string? HashPassword { get; set; }
        public string? Role { get; set; }
    }
}
