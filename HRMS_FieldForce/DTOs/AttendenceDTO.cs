namespace HRMS_FieldForce.Models
{
    public class AttendenceDTO
    {
        public required string UserId { get; set; }
        public required DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public required string WorkFrom { get; set; }
    }
}
