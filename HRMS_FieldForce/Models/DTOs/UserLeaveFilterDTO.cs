namespace HRMS_FieldForce.Models.DTOs
{
    public class UserLeaveFilterDTO
    {
        public DateOnly? ApplyDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public string? LeaveCategory { get; set; }

        public string? LeaveType { get; set; }

        public string? Reason { get; set; }
    }
}
