using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserAttendanceDTO
{
    public TimeOnly checkIn { get; set; }
    public TimeOnly checkOut { get; set; }
}
