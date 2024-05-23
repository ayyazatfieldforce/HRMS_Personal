using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs
{
    public class UserDTORegister
    {
        public required string UserId { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Role { get; set; }
        public required string PersonalEmail { get; set; }
        public required string CompanyEmail { get; set; }
    }
}
