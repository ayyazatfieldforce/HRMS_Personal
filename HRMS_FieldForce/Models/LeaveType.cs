using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models
{
    public class LeaveType
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
