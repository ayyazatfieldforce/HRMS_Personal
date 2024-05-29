using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserDTOLogin
{

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public required string CompanyEmail { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; } = string.Empty;
}
