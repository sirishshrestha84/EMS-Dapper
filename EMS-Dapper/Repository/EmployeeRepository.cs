using Dapper;
using EMS_Dapper.Models;
using EMS_Dapper.Repository.IRepository;
using Microsoft.Identity.Client;

namespace EMS_Dapper.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperApplicationDbContext _context;

        public EmployeeRepository(DapperApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetPagedEmployeesAsync(int page, int pageSize)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Employee>(
                "SELECT * FROM GetPagedEmployees1(@PageNumber, @PageSize)",
                new {PageNumber = page, PageSize = pageSize});
        }

        public async Task<int> GetTotalEmployeeCountAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Employees");
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Employee>(
                "SELECT Name as EmployeeName, Id, Email, DepartmentId,DesignationId FROM Employees WHERE Id = @Id",
                new { Id = id });
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("CreateEmployee", new
            {
                Name = employee.EmployeeName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("UpdateEmployee", new
            {
                Id = employee.Id,
                Name = employee.EmployeeName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DeleteEmployee", new { Id = id }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Department>("SELECT * FROM Departments");
        }

        public async Task<IEnumerable<Designation>> GetDesignationsAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Designation>("SELECT * FROM Designations");
        }
    }
}
