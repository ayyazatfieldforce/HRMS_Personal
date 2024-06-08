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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var role = HttpContext.Items["Role"] as string;

            if (role != "R1" && role != "R2")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to access this resource.");
            }

            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users. Please try again later.");
            }
        }
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchUsers([FromQuery] string? searchText)
        {
            var role = HttpContext.Items["Role"] as string;

            if (role != "R1" && role != "R2")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to access this resource.");
            }

            try
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    // If searchText is empty, return all users
                    var allUsers = await _context.Users.ToListAsync();
                    return Ok(allUsers);
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
                        result.Add(new { Id = "FName", firstName = user.FirstName, userId = user.UserId });
                    }

                    if (user.CompanyEmail.ToLower().Contains(searchLower))
                    {
                        result.Add(new { Id = "CEmail", companyEmail = user.CompanyEmail, userId = user.UserId });
                    }

                    if (user.Role.ToLower().Contains(searchLower))
                    {
                        result.Add(new { Id = "Role", role = user.Role, userId = user.UserId });
                    }
                }

                return Ok(result);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching users. Please try again later.");
            }
        }


        [HttpGet("Details/{id}")]
        public async Task<ActionResult> GetUserDetails(string id)
        {
            try
            {
                var role = HttpContext.Items["Role"] as string;

                if (role != "R1" && role != "R2")
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to access this resource.");
                }

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
                    return NotFound("User details not found.");
                }

                return Ok(userDetails);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching user details. Please try again later.");
            }
        }

        [HttpGet("FilterRole")]
        public async Task<ActionResult<IEnumerable<object>>> FilterRole([FromQuery] string? role)
        {
            try
            {
                var userRole = HttpContext.Items["Role"] as string;

                if (userRole != "R1" && userRole != "R2")
                {
                    return Forbid("You do not have permission to access this resource.");
                }

                IQueryable<object> query = _context.Users
                    .Where(u => string.IsNullOrEmpty(role) || u.Role.ToLower() == role.ToLower())
                    .Select(u => new
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        DateOfBirth = u.DateOfBirth,
                        PersonalEmail = u.PersonalEmail,
                        CompanyEmail = u.CompanyEmail
                    });

                var users = await query.ToListAsync();

                return Ok(users);
            }
            catch 
            {
                // Log the exception (ex) if necessary
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }



    }
}
