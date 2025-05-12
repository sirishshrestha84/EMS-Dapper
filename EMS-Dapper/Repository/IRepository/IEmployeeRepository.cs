using EMS_Dapper.Models;
using System.Threading.Tasks;

namespace EMS_Dapper.Repository.IRepository
{
    public interface IEmployeeRepository 
    {
        Task<IEnumerable<Employee>> GetPagedEmployeesAsync(int pageNumber, int pageSize);
        Task<int> GetTotalEmployeeCountAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);

        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<IEnumerable<Designation>> GetDesignationsAsync();
        Task<IEnumerable<DepartmentChartData>> GetEmployeeCountByDepartmentAsync();

        Task<IEnumerable<DesignationChartData>> GetEmployeeCountByDesignationAsync();
    }
}
