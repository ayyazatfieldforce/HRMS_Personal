using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models
{
    public class Attendence
    {
        public class Attendance
        {
            // User and Date will make up Composite Primary Key
            [Required]
            [ForeignKey("User")]
            public string UserId { get; set; }

            [Required]
            public String Date { get; set; }

            [Required]
            public String CheckInTime { get; set; }

            public String? CheckOutTime { get; set; }

            [StringLength(10)]
            public string? WorkFrom { get; set; }

            public User? User { get; set; }
        }
    }
}
