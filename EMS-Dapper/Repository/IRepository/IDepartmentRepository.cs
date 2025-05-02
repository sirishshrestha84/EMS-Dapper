using EMS_Dapper.Models;

namespace EMS_Dapper.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task CreateAsync (Department department);
        Task UpdateAsync (Department department);   
        Task DeleteAsync (int id);
    }
}
