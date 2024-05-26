using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserAttendanceDTO
{
    public DateTime checkIn { get; set; }
    public DateTime checkOut { get; set; }
}
