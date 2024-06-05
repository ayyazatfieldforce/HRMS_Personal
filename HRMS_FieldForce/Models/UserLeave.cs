using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace HRMS_FieldForce.Models
{
    public class UserLeave
    {
        [Key, Column(Order = 0)]
        [Required]
        [ForeignKey("User")]
        public required string UserId { get; set; }

        // Navigation property
        public User? User { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public DateOnly ApplyDate { get; set; }

        [Required]
        public DateOnly ToDate { get; set; }

        [Required]
        public string LeaveCategory { get; set; }

        [Required]
        public string LeaveType { get; set; }

        public string Reason { get; set; }

        [Required]
        public string status { get; set; }
    }
}
