using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models
{
    public class LeaveCategory
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
