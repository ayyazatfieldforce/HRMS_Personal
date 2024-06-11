using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRMS_FieldForce.Models
{
    public class GrantPermission
    {

        [Required]
        [ForeignKey("Roles")]
        public required string Role { get; set; }
        [Required]
        [ForeignKey("Modules")]
        public required int Module { get; set; }
        [Required]
        [ForeignKey("Actions")]
        public required int Permission { get; set; }
        public Role? Roles { get; set; }
        public Module? Modules { get; set; }
        public Permission? Actions{ get; set; }
    }
}
