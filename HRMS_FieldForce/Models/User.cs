using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("RoleAssigned")]
        public required string Role { get; set; }

        [EmailAddress]
        [Required]
        public required string PersonalEmail { get; set; }

        [EmailAddress]
        [Required]
        public required string CompanyEmail { get; set; }

        // Navigation property
        public Role? RoleAssigned { get; set; }


    }
}
