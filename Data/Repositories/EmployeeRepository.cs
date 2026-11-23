using ELMS.Data.Interfaces;
using ELMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ELMS.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext moContext;

        public EmployeeRepository(ApplicationDbContext foContext)
        {
            moContext = foContext;
        }

        public async Task<List<tblEmployees>> getManagersAsync()
        {
            return await moContext.tblEmployees
                        .Where(x => x.stRole == "Manager")
                        .ToListAsync();
        }

        public async Task addEmployeeAsync(tblEmployees foemployee)
        {
            moContext.tblEmployees.Add(foemployee);
            await moContext.SaveChangesAsync();
        }

        public async Task updateEmployeeAsync(tblEmployees foemployee)
        {
            moContext.tblEmployees.Update(foemployee);
            await moContext.SaveChangesAsync();
        }

        public async Task<tblEmployees?> getEmployeeByIdAsync(int fiEmployeeId)
        {
            return await moContext.tblEmployees.FirstOrDefaultAsync(x => x.inEmployeeId == fiEmployeeId);
        }

        public async Task<List<tblEmployees>> getAllEmployeesAsync()
        {
            return await moContext.tblEmployees.OrderBy(x => x.stFirstName).ToListAsync();
        }
    }
}
