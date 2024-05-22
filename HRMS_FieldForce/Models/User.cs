using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models
{
    public class User
    {
        [Key]
        public required string UserId { get; set; }

        [Required]
        public required string HashPassword { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public required string Role { get; set; }

        [EmailAddress]
        [Required]
        public required string PersonalEmail { get; set; }

        [EmailAddress]
        [Required]
        public required string CompanyEmail { get; set; }


    }
}
