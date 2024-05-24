using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserAttendanceDTO
{
    public required string UserId { get; set; }
    public DateTime checkIn { get; set; } = DateTime.Now;
    public DateTime checkOut { get; set; } = DateTime.Now;
}
