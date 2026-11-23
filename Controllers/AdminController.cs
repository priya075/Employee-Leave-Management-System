using ELMS.Data.Interfaces;
using ELMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELMS.Controllers
{
    [Authorize (Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IEmployeeRepository moEmployeeRepository;


        private const string ROLE_EMPLOYEE = "Employee";

        private const string ROLE_MANAGER = "Manager";

        public AdminController (IEmployeeRepository foEmployeeRepository)
        {
            moEmployeeRepository = foEmployeeRepository;
        }

        public async Task<IActionResult> addEmployee()
        {
            AddEmployeeViewModel loViewModel = new AddEmployeeViewModel();

            loViewModel.lsManagers = await moEmployeeRepository.getManagersAsync();

            return View(loViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> addEmployee(AddEmployeeViewModel foModel)
        {
             if (foModel.stRole == ROLE_MANAGER)
            {
                ModelState.Remove(nameof(foModel.inManagerId));
                foModel.inManagerId = null;
            }
            else  if (foModel.stRole == ROLE_EMPLOYEE && foModel.inManagerId == null)
            {
                ModelState.AddModelError("inManagerId", "Manager is required for the employee");

                foModel.lsManagers = await moEmployeeRepository.getManagersAsync();

                return View(foModel);
            }

            if (!ModelState.IsValid)
            {
                foModel.lsManagers = await moEmployeeRepository.getManagersAsync();
                return View(foModel);
            }

            int? loManagerId = null;
            if(foModel.stRole == ROLE_EMPLOYEE)
            {
                loManagerId = foModel.inManagerId;
            }

            tblEmployees loEmployee = new tblEmployees
            {
                stFirstName = foModel.stFirstName,
                stLastName = foModel.stLastName,
                stEmail = foModel.stEmailAddress,
                stPhone = foModel.stPhone,
                stDepartment = foModel.stDepartment,
                stRole = foModel.stRole,
                inManagerId = foModel.stRole == ROLE_EMPLOYEE ? foModel.inManagerId : null
                
            };

            await moEmployeeRepository.addEmployeeAsync(loEmployee);

            return RedirectToAction("employeeList");
            
        }

        [HttpGet]
        public async Task<IActionResult> updateEmployee(int fiEmployeeId)
        {

            var loEmployee = await moEmployeeRepository.getEmployeeByIdAsync(fiEmployeeId);

            if (loEmployee == null)
            {
                return NotFound();
            }

            var loModel = new UpdateEmployeeViewModel
            {
                fiEmployeeId = loEmployee.inEmployeeId,
                stFirstName = loEmployee.stFirstName,
                stLastName = loEmployee.stLastName,
                stEmailAddress = loEmployee.stEmail,
                stPhone = loEmployee.stPhone,
                stDepartment = loEmployee.stDepartment,
                stRole = loEmployee.stRole,
                inManagerId = loEmployee.inManagerId,
                lsManagers = await moEmployeeRepository.getManagersAsync()
            };

            return View(loModel);
        }

        [HttpPost]
        public async Task<IActionResult> updateEmployee(UpdateEmployeeViewModel foModel)
        {
            if (foModel.stRole == ROLE_MANAGER)
            {
                foModel.inManagerId = null;
                ModelState.Remove(nameof(foModel.inManagerId));           
            }

            if (!ModelState.IsValid)
            {
                foModel.lsManagers = await moEmployeeRepository.getManagersAsync();
                return View(foModel);
            }

            var loEmployee = await moEmployeeRepository.getEmployeeByIdAsync(foModel.fiEmployeeId);
            
            if (loEmployee == null)
                return NotFound();


            loEmployee.stFirstName = foModel.stFirstName;
            loEmployee.stLastName = foModel.stLastName;
            loEmployee.stEmail = foModel.stEmailAddress;
            loEmployee.stPhone = foModel.stPhone;
            loEmployee.stDepartment = foModel.stDepartment;
            loEmployee.stRole = foModel.stRole;
            loEmployee.inManagerId = foModel.stRole == ROLE_EMPLOYEE ? foModel.inManagerId : null;

            await moEmployeeRepository.updateEmployeeAsync(loEmployee);

            return RedirectToAction("employeeList");

        }

        public async Task<IActionResult> employeeList()
        {
            var loEmployee = await moEmployeeRepository.getAllEmployeesAsync();
            return View(loEmployee);
        }
    }
}
