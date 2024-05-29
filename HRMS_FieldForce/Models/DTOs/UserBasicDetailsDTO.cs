using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserBasicDetailsDTO
{
    public required string WorkingHours { get; set; } = string.Empty;
    public required string ReportingTo { get; set; } = string.Empty;
    public required string MaritalStatus { get; set; } = string.Empty;

    [Required]
    public required DateTime DateOfBirth { get; set; } 
    public required string ExperienceInFieldForce { get; set; } = string.Empty;
    public required string TotalExperience { get; set; } = string.Empty;
    public required string AccountNo { get; set; } = string.Empty;
    public required string EOBI { get; set; } = string.Empty;
    public required decimal GrossSalary { get; set; }
    public required string Benefits { get; set; } = string.Empty;
}
