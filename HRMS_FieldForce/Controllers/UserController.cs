using HRMS_FieldForce.Data;
using HRMS_FieldForce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext userDBContext)
        {
            _context = userDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchUsers([FromQuery] string? searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                // If searchText is empty, return all users
                return await _context.Users.ToListAsync();
            }

            var searchLower = searchText.ToLower();

            var query = _context.Users
                .Where(u => EF.Functions.Like(u.FirstName.ToLower(), "%" + searchLower + "%") ||
                            EF.Functions.Like(u.CompanyEmail.ToLower(), "%" + searchLower + "%") ||
                            EF.Functions.Like(u.Role.ToLower(), "%" + searchLower + "%"));

            var users = await query.ToListAsync();

            var result = new List<object>();

            foreach (var user in users)
            {
                if (user.FirstName.ToLower().Contains(searchLower))
                {
                    result.Add(new { firstName = user.FirstName, userId = user.UserId });
                }

                if (user.CompanyEmail.ToLower().Contains(searchLower))
                {
                    result.Add(new { companyEmail = user.CompanyEmail, userId = user.UserId });
                }

                if (user.Role.ToLower().Contains(searchLower))
                {
                    result.Add(new { role = user.Role, userId = user.UserId });
                }
            }

            return Ok(result);
        }

        [HttpGet("Details/{id}")]
        public async Task<ActionResult> GetUserDetails(string id)
        {
            var userDetails = await (from basicDetail in _context.UserBasicDetails
                                     join personalDetail in _context.UserPersonalDetails on basicDetail.UserId equals personalDetail.UserId
                                     join user in _context.Users on basicDetail.UserId equals user.UserId
                                     where basicDetail.UserId == id
                                     select new
                                     {
                                         user.UserId,
                                         user.FirstName,
                                         user.LastName,
                                         user.DateOfBirth,
                                         user.Role,
                                         user.PersonalEmail,
                                         user.CompanyEmail,
                                         basicDetail.WorkingHours,
                                         basicDetail.ReportingTo,
                                         basicDetail.MaritalStatus,
                                         basicDetail.ExperienceInFieldForce,
                                         basicDetail.TotalExperience,
                                         basicDetail.AccountNo,
                                         basicDetail.EOBI,
                                         basicDetail.GrossSalary,
                                         basicDetail.Benefits,
                                         personalDetail.FatherName,
                                         personalDetail.CNIC,
                                         personalDetail.Phone,
                                         personalDetail.EmergencyContact,
                                         personalDetail.EmployeeStatus,
                                         personalDetail.Branch,
                                         personalDetail.Department,
                                         personalDetail.Designation,
                                         personalDetail.JobGrade,
                                         personalDetail.JoiningDate,
                                         personalDetail.Address,
                                         personalDetail.PermanentAddress
                                     }).FirstOrDefaultAsync();

            if (userDetails == null)
            {
                return NotFound();
            }

            return Ok(userDetails);
        }

    }
}
