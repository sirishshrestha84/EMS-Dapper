using EMS_Dapper.Repository.IRepository;

namespace EMS_Dapper.Unit_Of_Work
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employee { get; }
        IDepartmentRepository Department {  get; }
        IDesignationRepository Designation { get; }
        Task CommitAsync();
    }
}
