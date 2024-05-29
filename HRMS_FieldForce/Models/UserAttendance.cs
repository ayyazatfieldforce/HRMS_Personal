using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HRMS_FieldForce.Models;


public class UserAttendance
{
    [Key, Column(Order = 0)]
    [Required]
    [ForeignKey("User")]
    public required string UserId { get; set; }

    // Navigation property
    public User? User { get; set; }

    [Required]
    public string day {  get; set; } = DateTime.Now.DayOfWeek.ToString();

    [Key, Column(Order = 1)]
    [Required]
    public DateTime checkIn { get; set; } = DateTime.Now;

    [Required]
    public DateTime checkOut { get; set; }
}
