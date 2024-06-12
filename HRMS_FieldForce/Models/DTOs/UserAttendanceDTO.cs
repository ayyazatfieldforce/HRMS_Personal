using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserAttendanceDTO
{
    public DateTime checkIn { get; set; } = DateTime.Now;
    public DateTime checkOut { get; set; } = DateTime.Now;
}
