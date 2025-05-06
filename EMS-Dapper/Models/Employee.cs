using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_Dapper.Models
{
    public class Employee
    {

        public int Id { get; set; }

        public string EmployeeName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DesignationName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }

    }
}
