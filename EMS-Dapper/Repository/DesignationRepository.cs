using Dapper;
using EMS_Dapper.Models;
using EMS_Dapper.Repository.IRepository;
using System.Data;

namespace EMS_Dapper.Repository
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly DapperApplicationDbContext _context;

        public DesignationRepository(DapperApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Designation>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Designation>("SELECT * FROM dbo.GetAllDesignations()");
        }

        public async Task<Designation?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Designation>(
                "SELECT * FROM Designations WHERE DesignationId = @Id", new { Id = id });
        }
        
        public async Task CreateAsync(Designation designation)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "INSERT INTO Designations (DesignationName) VALUES (@DesignationName)",
                new { designation.DesignationName });
        }

        public async Task UpdateAsync(Designation designation)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("UpdateDesignation", new
            {
                designation.DesignationId,
                designation.DesignationName,
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DeleteDesignation", new { DesignationId = id }, commandType: CommandType.StoredProcedure);
        }
    }
}
