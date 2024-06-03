using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.DTOs
{
    public class AttendanceFilterDTO
    {
       
        public string? UserId { get; set; }

       
        public string? Date { get; set; }

       
        public string? CheckInTime { get; set; }

        public string? CheckOutTime { get; set; }

        [StringLength(10)]
        public string? WorkFrom { get; set; }
    }
}
