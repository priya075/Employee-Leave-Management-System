using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ELMS.Models
{
    public class tblEmployees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int inEmployeeId {  get; set; }
        [Required]
        public string stFirstName { get; set; }
        [Required]
        public string stLastName { get; set; }
        [Required]
        [EmailAddress]
        public string stEmail { get; set; }
        [Required]
        public string stPhone { get; set; }
        [Required]
        public string stDepartment { get; set; }

        public int? inManagerId { get; set; }

        public string stRole {  get; set; }
    }
}
