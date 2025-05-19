using Dapper;
using DocumentFormat.OpenXml.Drawing;
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

        //Action to show employee in department in chart format
        public async Task<IEnumerable<DepartmentChartData>> GetEmployeeCountByDepartmentAsync()
        {
            var sql = @"
        SELECT d.Name AS DepartmentName, COUNT(e.Id) AS EmployeeCount
        FROM Employees e
        INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
        GROUP BY d.Name
    ";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DepartmentChartData>(sql);
        }

        public async Task<IEnumerable<DesignationChartData>> GetEmployeeCountByDesignationAsync()
        {
            string sql = @"
            SELECT DesignationName, COUNT(*) AS EmployeeCount
            FROM Employees
            INNER JOIN Designations ON Employees.DesignationId = Designations.DesignationId
            GROUP BY DesignationName";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DesignationChartData>(sql);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                          SELECT e.Id, e.Name AS EmployeeName, e.Email,
                          d.Name AS DepartmentName,
                          des.DesignationName
                          FROM Employees e
                          INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                          INNER JOIN Designations des ON e.DesignationId = des.DesignationId";
                return await connection.QueryAsync<Employee>(sql);
            }
        }

        public async Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(string? departmentName, string? designationName, int page, int pageSize)
        {
            using var connection = _context.CreateConnection();

            var sql = @"
SELECT 
            e.Id, 
            e.Name AS EmployeeName, 
            e.Email,
            d.Name AS DepartmentName,
            des.DesignationName
        FROM Employees e
        INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
        INNER JOIN Designations des ON e.DesignationId = des.DesignationId
        WHERE (@DepartmentName IS NULL OR d.Name = @DepartmentName)
          AND (@DesignationName IS NULL OR des.DesignationName = @DesignationName)
        ORDER BY e.Id
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            return await connection.QueryAsync<Employee>(sql, new
            {
                DepartmentName = departmentName,
                DesignationName = designationName,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize

            });
        }

        public async Task<int> GetFilteredEmployeeCountAsync(string? departmentName, string? designationName)
        {
            using var connection = _context.CreateConnection();

            var sql = @"
        SELECT COUNT(*)
        FROM Employees e
        INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
        INNER JOIN Designations des ON e.DesignationId = des.DesignationId
        WHERE (@DepartmentName IS NULL OR d.Name = @DepartmentName)
          AND (@DesignationName IS NULL OR des.DesignationName = @DesignationName)";

            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                DepartmentName = departmentName,
                DesignationName = designationName
            });
        }



    }
}
