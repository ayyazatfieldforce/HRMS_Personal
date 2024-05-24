namespace HRMS_FieldForce.DTOs
{
    public class UserBasicDetailsDTO
    {

        public required string UserId { get; set; }
        public required string WorkingHours { get; set; }
        public required string ReportingTo { get; set; }
        public required string MaritalStatus { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string ExperienceInFieldForce { get; set; }
        public required string TotalExperience { get; set; }
        public required string AccountNo { get; set; }
        public required string EOBI { get; set; }
        public required decimal GrossSalary { get; set; }
        public required string Benefits { get; set; }

    }
}
