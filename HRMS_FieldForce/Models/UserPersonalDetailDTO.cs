namespace HRMS_FieldForce.Models
{
    public class UserPersonalDetailDTO
    {
        public string UserId { get; set; }
        public string FatherName { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public string EmployeeStatus { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string? JobGrade { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;


    }
}
