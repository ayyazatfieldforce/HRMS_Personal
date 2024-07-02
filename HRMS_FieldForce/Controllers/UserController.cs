using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDBContext _context;

        public UserController(UserDBContext userDBContext)
        {
            _context = userDBContext;
        }

        

        [HttpGet]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch
            {
                return BadRequest("");
            }
        }

        [HttpGet("Search")]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IEnumerable<object>>> SearchUsers([FromQuery] string? searchText)
        {
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
                return BadRequest("");
            }
        }

        //[HttpGet("GetOtherEmployeeInfo/{id}")]
        //[Authorize(Roles = "User")]
        //[EnableCors("AllowOrigin")]
        //public async Task<ActionResult> GetOtherEmployeeInfo(string id)
        //{
        //    try
        //    {
        //        var EmployeeInfo = await _context.Users.SingleOrDefaultAsync(user => user.UserId == id);
        //        //var EmployeeInfo = await _context.Users.FindAsync(id);
        //        var role = await _context.UserPersonalDetails
        //                        .Where(u => u.UserId == id.ToString())
        //                        .Select(u => u.Designation)
        //                        .FirstOrDefaultAsync();

        //        if (EmployeeInfo is null || role is null)
        //        {
        //            return BadRequest("");
        //        }
        //        var FilteredInfo = new
        //        {

        //            Email = EmployeeInfo.PersonalEmail,
        //            FName = EmployeeInfo.FirstName,
        //            LName = EmployeeInfo.LastName,
        //            Role = role

        //        };

        //        return BadRequest("");

        //    }
        //    catch
        //    {
        //        return BadRequest("");
        //    }
        //}


        [HttpGet("Details/{id}")]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetUserDetails(string id)
        {
            try
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
                    return NotFound("User details not found.");
                }

                return Ok(userDetails);
            }
            catch
            {
                return BadRequest("");
            }
        }

        [HttpGet("FilterRole")]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IEnumerable<object>>> FilterRole([FromQuery] string? role)
        {
            try
            {
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
                return BadRequest("");
            }
        }

        private CurrentUserJWT GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new CurrentUserJWT
                {
                    UserID = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    CompanyEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

    }
}
