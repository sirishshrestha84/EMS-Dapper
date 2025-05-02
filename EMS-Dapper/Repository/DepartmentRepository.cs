using Dapper;
using EMS_Dapper.Models;
using EMS_Dapper.Repository.IRepository;

namespace EMS_Dapper.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DapperApplicationDbContext _context;

        public DepartmentRepository(DapperApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Department>("SELECT * FROM dbo.GetAllDepartments()");
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Department>(
                "SELECT * FROM Departments WHERE DepartmentId= @Id", new { Id = id });
        }

        public async Task CreateAsync(Department department)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("CreateDepartment", new { department.Name }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Department department)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("UpdateDepartment", new
            {
                department.DepartmentId,
                department.Name
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DeleteDepartment", new { DepartmentId = id }, commandType: System.Data.CommandType.StoredProcedure);
        }

    }
}
