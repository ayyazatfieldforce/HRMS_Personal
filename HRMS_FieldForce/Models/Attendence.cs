using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models
{
    public class Attendence
    {
        public class Attendance
        {
            //User and Date will make up Composite Primary Key
            [Required]
            [ForeignKey("User")]
            public required string UserId { get; set; }

            [Required]
            public required DateTime Date { get; set; }

            public DateTime? CheckInTime { get; set; }

            public DateTime? CheckOutTime { get; set; }

            [StringLength(10)]
            public string? WorkFrom { get; set; }

         
            public User? User { get; set; }
        }
    }
}
