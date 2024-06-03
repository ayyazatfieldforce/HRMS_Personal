using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.DTOs
{
    public class AttendanceDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string CheckInTime { get; set; }

        public string? CheckOutTime { get; set; }

        [StringLength(10)]
        public string? WorkFrom { get; set; }
    }
}
