using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HRMS_FieldForce.Models;

public class User : IdentityUser
{
    [Key]
    public required string UserId { get; set; }

    [Required]
    public override required string PasswordHash { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public required string Role { get; set; }

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public required string PersonalEmail { get; set; }

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public required string CompanyEmail { get; set; }

    //public virtual ICollection<UserBasicDetails> userBasicDetails { get; set; }
    //public User()
    //{
    //}
    //public User(string userId, string passwordHash, string firstName, string lastName, DateTime dateOfBirth, string role, string personalEmail, string companyEmail)
    //{
    //    UserId = userId;
    //    PasswordHash = passwordHash;
    //    FirstName = firstName;
    //    LastName = lastName;
    //    DateOfBirth = dateOfBirth;
    //    Role = role;
    //    PersonalEmail = personalEmail;
    //    CompanyEmail = companyEmail;
    //}

}
