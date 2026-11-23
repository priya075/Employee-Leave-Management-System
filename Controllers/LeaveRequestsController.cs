using ELMS.Common.Enums;
using ELMS.Data;
using ELMS.Models;
using ELMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace ELMS.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        private const string STATUS_APPROVED = "Approved";
        private const string STATUS_REJECTED = "Rejected";

        private readonly ApplicationDbContext moDbContext;

        private readonly UserContext moUser;

        public LeaveRequestsController(ApplicationDbContext foContext, UserContext foUser)
        {
            moDbContext = foContext;
            moUser = foUser;
        }

        private static string getSql(string fsStoredProcedure)
        {
            return $"EXEC {fsStoredProcedure}";
        }

        public IActionResult apply()
        {
            if(moUser.miEmployeeId == 0)
            {
                return RedirectToAction("login", "Account");
            }

            if (moUser.mbManager) 
            {
                return RedirectToAction("teamLeaveHistory");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> apply(tblLeaveRequests foLeave)
        {
            int liLoggedInUserId = moUser.miEmployeeId;

            if(liLoggedInUserId == 0)
            {
                return RedirectToAction("login", "Account");
            }

            ModelState.Remove("stStatus");

            ModelState.Remove("dtAppliedDate");

            ModelState.Remove("stManagerComment");

            foLeave.itEmployeeId = liLoggedInUserId;

            foLeave.stStatus = LeaveStatus.Pending.ToString();

            foLeave.dtAppliedDate = DateTime.Now;

            int liRequestedDays = (foLeave.dtEndDate - foLeave.dtStartDate).Days+1;

            if (liRequestedDays > 30)
            {
                ModelState.AddModelError("dtEndDate", "You cannot apply for more than 30 days of leave at a time");
                return View(foLeave);
            }

            int? liManagerId = moDbContext.tblEmployees
                                .Where(x => x.inEmployeeId == liLoggedInUserId)
                                .Select(x => x.inManagerId)
                                .FirstOrDefault();

            foLeave.inManagerId = liManagerId;

            if (foLeave.dtEndDate < foLeave.dtStartDate)
            {
                ModelState.AddModelError("dtEndDate", "End date cannot be earlier than Start date");
                return View(foLeave);
            }

            if (!ModelState.IsValid)
            {
                return View(foLeave);
            }

            List<SqlParameter> loSqlParameters = new List<SqlParameter>();

            loSqlParameters.Add(new SqlParameter("@EmployeeId", foLeave.itEmployeeId));
            loSqlParameters.Add(new SqlParameter("@StartDate", foLeave.dtStartDate));
            loSqlParameters.Add(new SqlParameter("@EndDate", foLeave.dtEndDate));
            loSqlParameters.Add(new SqlParameter("@Reason", foLeave.stReason));
            loSqlParameters.Add(new SqlParameter("@ManagerId", foLeave.inManagerId ?? (object)DBNull.Value));

            await moDbContext.Database.ExecuteSqlRawAsync(
                getSql(
                    "sp_ApplyLeave @EmployeeId, @StartDate, @EndDate, @Reason, @ManagerId"),
                    loSqlParameters.Cast<object>().ToArray()
                );

            return RedirectToAction("leaveHistory");

        }

        public async Task<IActionResult> leaveHistory()
        {
            int liLoggedInUserId = moUser.miEmployeeId;
            List<SqlParameter> loSqlParameters = new List<SqlParameter>();
            
            loSqlParameters.Add(new SqlParameter("@EmployeeId", liLoggedInUserId));

            var loLeaves = await moDbContext.tblLeaveRequests.FromSqlRaw(
                getSql("sp_GetEmployeeLeaveHistory @EmployeeId"),
                loSqlParameters.ToArray()
                ).ToListAsync();
            return View(loLeaves);
        }


        public async Task<IActionResult> cancel(int fiLeaveRequestId)
        {
            var loLeave = await moDbContext.tblLeaveRequests.FirstOrDefaultAsync(x => x.itLeaveRequestId == fiLeaveRequestId);

            if(loLeave == null)
            {
                return NotFound();
            }

            if(loLeave.stStatus != LeaveStatus.Pending.ToString())
            {
                return RedirectToAction("leaveHistory");
            }

            loLeave.stStatus = LeaveStatus.Cancelled.ToString();

            loLeave.dtDecisionDate = DateTime.Now;

            await moDbContext.SaveChangesAsync();

            return RedirectToAction("leaveHistory");
        }

        public async Task<IActionResult> teamLeaveHistory() 
        {
            int liLoggedInUserId = moUser.miEmployeeId;

            if(liLoggedInUserId == 0)
            {
                return RedirectToAction("login", "Account");
            }

            List<SqlParameter> loSqlParameters = new List<SqlParameter>();

            loSqlParameters.Add(new SqlParameter("@ManagerId", liLoggedInUserId));

            var loLeaves = await moDbContext.tblLeaveRequests.FromSqlRaw(
                getSql("sp_GetTeamLeaveRequests @ManagerId"),
                loSqlParameters.ToArray()
                ).ToListAsync();

            return View(loLeaves);
        }

        [HttpPost]
        public async Task<IActionResult> approve(int fiLeaveRequestId, string fsManagerComment)
        {
            List<SqlParameter> loSqlParameters = new List<SqlParameter>();

            loSqlParameters.Add(new SqlParameter("@LeaveRequestId", fiLeaveRequestId));
            loSqlParameters.Add(new SqlParameter("@Status", STATUS_APPROVED));
            loSqlParameters.Add(new SqlParameter("@ManagerComment", fsManagerComment));

            await moDbContext.Database.ExecuteSqlRawAsync(
                getSql("sp_UpdateLeaveStatus @LeaveRequestId, @Status, @ManagerComment"),
                loSqlParameters.ToArray()
                );

            return RedirectToAction("teamLeaveHistory");
        }

        [HttpPost]
        public async Task<IActionResult> reject(int fiLeaveRequestId, string fsManagerComment)
        {
            List<SqlParameter> loSqlParameters = new List<SqlParameter>();

            loSqlParameters.Add(new SqlParameter("@LeaveRequestId", fiLeaveRequestId));
            loSqlParameters.Add(new SqlParameter("@Status", STATUS_REJECTED));
            loSqlParameters.Add(new SqlParameter("@ManagerComment", fsManagerComment));

            await moDbContext.Database.ExecuteSqlRawAsync(
                getSql("sp_UpdateLeaveStatus @LeaveRequestId, @Status, @ManagerComment"),
                loSqlParameters.ToArray()
                );

            return RedirectToAction("teamLeaveHistory");
        }
    }
}
