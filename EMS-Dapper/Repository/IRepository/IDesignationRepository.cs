using EMS_Dapper.Models;

namespace EMS_Dapper.Repository.IRepository
{
    public interface IDesignationRepository
    {
        Task<IEnumerable<Designation>> GetAllAsync();
        Task<Designation?> GetByIdAsync(int id);
        Task CreateAsync(Designation designation);
        Task UpdateAsync(Designation designation);
        Task DeleteAsync(int id);
    }
}
