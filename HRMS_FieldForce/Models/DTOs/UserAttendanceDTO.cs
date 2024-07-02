using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserAttendanceDTO
{
    public string day {  get; set; }
    public string checkIn { get; set; }
    public string checkOut { get; set; }
}
