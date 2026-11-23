using ELMS.Models;

namespace ELMS.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<tblEmployees>> getManagersAsync();
        Task addEmployeeAsync(tblEmployees foEmployee);

        Task updateEmployeeAsync(tblEmployees foEmployee);

        Task<tblEmployees> getEmployeeByIdAsync(int fiEmployyeid);

        Task<List<tblEmployees>> getAllEmployeesAsync();
    }
}
