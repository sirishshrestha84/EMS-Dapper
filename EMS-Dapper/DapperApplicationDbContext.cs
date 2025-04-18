using Microsoft.Data.SqlClient;
using System.Data;

namespace EMS_Dapper
{
    public class DapperApplicationDbContext
    {
        private readonly IConfiguration _configuration;

        public DapperApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
