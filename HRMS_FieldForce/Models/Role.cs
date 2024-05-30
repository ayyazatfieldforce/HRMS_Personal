using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models
{
    public class Role
    {
        [Key]
        public required string RoleID { get; set; }

        [Required]
        public required string RoleName { get; set; }

        // Navigation property
        public ICollection<User>? Users { get; set; }

    }
}
