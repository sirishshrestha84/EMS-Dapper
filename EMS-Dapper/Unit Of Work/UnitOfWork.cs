using EMS_Dapper.Repository;
using EMS_Dapper.Repository.IRepository;

namespace EMS_Dapper.Unit_Of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperApplicationDbContext _context;

        public IEmployeeRepository Employee { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IDesignationRepository Designation { get; private set; }

        public UnitOfWork(DapperApplicationDbContext context)
        {
            _context = context;
            Employee = new EmployeeRepository(_context);
            Department = new DepartmentRepository(_context);
            Designation = new DesignationRepository(_context);

        }
        //This is useful if you plan to implement transaction management in the future
        public Task CommitAsync()
        {
            // For raw Dapper, you handle commits manually per connection.
            // Placeholder for future transaction support if needed.
            return Task.CompletedTask;
        }
    }
}
