using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELMS.Models
{
    public class tblLeaveRequests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int itLeaveRequestId {  get; set; }
        [Required]
        public int itEmployeeId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime dtStartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime dtEndDate { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        public string stReason { get; set; }
       
        public string stStatus { get; set; }

        public DateTime dtAppliedDate { get; set; }

        public int? inManagerId { get; set; }

        public DateTime? dtDecisionDate { get; set; }

        public string? stManagerComment { get; set; }
    }
}
