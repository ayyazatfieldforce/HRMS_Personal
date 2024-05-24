using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRMS_FieldForce.Models;

public class UserPersonalDetail
{


    [Key]
    [Required]
    [ForeignKey("User")]
    public required string UserId { get; set; }


    [Required]
    public required string FatherName { get; set; }

    [Required]
    public required string CNIC { get; set; }

    [Required]
    public required string Phone { get; set; }

    [Required]
    public required string EmergencyContact { get; set; }

    [Required]
    public required string EmployeeStatus { get; set; }

    [Required]
    public required string Branch { get; set; }

    [Required]
    public required string Department { get; set; }

    [Required]
    public required string Designation { get; set; }

    
    public required string? JobGrade { get; set; }

    [Required]
    public DateTime JoiningDate { get; set; }

    [Required]
    public required string Address { get; set; }

    [Required]
    public required string PermanentAddress { get; set; }

    // Navigation property
    public User? User { get; set; }
}
