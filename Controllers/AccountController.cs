using ELMS.Common.Constants;
using ELMS.Common.Enums;
using ELMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace ELMS.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext moDbContext;

        private const string ROLE_ADMIN = "Admin";
        private const string ROLE_MANAGER = "Manager";

        public AccountController(ApplicationDbContext foDbContext)
        {
            moDbContext = foDbContext;
        }
        public IActionResult index()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> login(string fsEmail)
        {
            if (string.IsNullOrWhiteSpace(fsEmail))
            {
                ModelState.AddModelError("", "Email is required");
                return View();
            }

            var loEmployee = moDbContext.tblEmployees.FirstOrDefault(x => x.stEmail == fsEmail.Trim());

            if (loEmployee == null)
            {
                ModelState.AddModelError("", "Invalid Email. Try Again");
                return View();
            }

            var loClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loEmployee.stEmail),
                new Claim(ClaimTypes.Role, loEmployee.stRole),
                new Claim("EmployeeId", loEmployee.inEmployeeId.ToString())
            };

            var loIdentity = new ClaimsIdentity(loClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            var loPrincipal = new ClaimsPrincipal(loIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, loPrincipal);


            HttpContext.Session.SetInt32(SessionKeys.lsEmployeeId, loEmployee.inEmployeeId);
            
            HttpContext.Session.SetString(SessionKeys.lsRole, loEmployee.stRole);
            
            HttpContext.Session.SetString(SessionKeys.lsManager, loEmployee.stRole == "Manager" ? "true" : "false");

            if (loEmployee.stRole == ROLE_ADMIN)
            {
                return RedirectToAction("employeeList", "Admin");
            }
            else if (loEmployee.stRole == ROLE_MANAGER)
            {
                return RedirectToAction("teamLeaveHistory", "LeaveRequests");
            }
            else
            {
                return RedirectToAction("apply", "LeaveRequests");
            }
        }

        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login","Account");
        }

    }
}
