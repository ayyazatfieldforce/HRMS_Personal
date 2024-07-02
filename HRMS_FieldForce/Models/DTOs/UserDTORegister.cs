﻿using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models.DTOs;

public class UserDTORegister
{
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string DateOfBirth { get; set; }

    [Required]
    public required string Role { get; set; }

    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public required string PersonalEmail { get; set; }

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public required string CompanyEmail { get; set; }
}
