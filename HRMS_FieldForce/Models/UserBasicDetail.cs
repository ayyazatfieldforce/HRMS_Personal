using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models
{
    public class UserBasicDetail
    {

        [Key]
        [Required]
        [ForeignKey("User")]
        public required string UserId { get; set; }

        [Required]
        public required string WorkingHours { get; set; }


        [Required]
        public required string ReportingTo { get; set; }


        [Required]
        public required string MaritalStatus { get; set; }


        [Required]
        public required DateTime DateOfBirth { get; set; }


        [Required]
        public required string ExperienceInFieldForce { get; set; }


        [Required]
        public required string TotalExperience { get; set; }


        [Required]
        public required string AccountNo { get; set; }


        [Required]
        public required string EOBI { get; set; }


        [Required]
        public required decimal GrossSalary { get; set; }


        [Required]
        public required string Benefits { get; set; }


        // Navigation property
        public User? User { get; set; }
    }
}
