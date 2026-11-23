using ELMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ELMS.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<tblEmployees> tblEmployees { get; set; }

        public DbSet<tblLeaveRequests> tblLeaveRequests { get; set; }
    }
}
