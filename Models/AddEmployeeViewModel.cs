using System.ComponentModel.DataAnnotations;

namespace ELMS.Models
{
    public class AddEmployeeViewModel
    {
        [Required]
        public string stFirstName { get; set; }

        [Required]
        public string stLastName { get; set; }

        [Required]
        [EmailAddress]
        public string stEmailAddress { get; set; }

        [Required]
        public string stDepartment { get; set; }

        [Required]
        public string stPhone { get; set; }

        [Required]
        public string stRole {  get; set; }

        public int? inManagerId { get; set; }

        public List<tblEmployees> lsManagers { get; set; } = new();
    }
}
