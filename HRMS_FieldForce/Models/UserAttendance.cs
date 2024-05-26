using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models;

public class UserAttendance
{
    [Key]
    [Required]
    [ForeignKey("User")]
    public required string UserId { get; set; }

    // Navigation property
    public User? User { get; set; }

    [Required]
    public string day {  get; set; } = DateTime.Now.DayOfWeek.ToString();

    [Required]
    public DateTime checkIn { get; set; } = DateTime.Now;

    [Required]
    public DateTime checkOut { get; set; }
}
