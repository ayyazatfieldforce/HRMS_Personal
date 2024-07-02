using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using HRMS_FieldForce.Models.DTOs;
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
    public class UserBasicDetailsController : ControllerBase
    {
        private readonly UserDBContext _UserDBContext;

        public UserBasicDetailsController(UserDBContext userDBContext)
        {
            _UserDBContext = userDBContext;
        }

        [HttpGet]
        [EnableCors("AllowOrigin")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserBasicDetails>> GetUserDetails()
        {
            string id = GetCurrentUser().UserID;
            var userDetail = await _UserDBContext.UserBasicDetails.FindAsync(id);
            if (userDetail is null)
            {
                return NotFound($"UserPersonalDetail with UserId {id} not found.");
            }
            return Ok(userDetail);
            //return Ok(await _UserDBContext.UserBasicDetails.ToListAsync());
        }


        [HttpPost]
        [EnableCors("AllowOrigin")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserBasicDetails>> AddUserBasicDetails(UserBasicDetailsDTO request)
        {
            string id = GetCurrentUser().UserID;
            var dbUser = await _UserDBContext.Users.FindAsync(id);

            if (dbUser is null)
            {
                return BadRequest($"User with id {id} does not exist.");
            }

            var UserBasicDetails = new UserBasicDetails
            {
                UserId = id,
                WorkingHours = request.WorkingHours,
                ReportingTo = request.ReportingTo,
                MaritalStatus = request.MaritalStatus,
                DateOfBirth = request.DateOfBirth,
                ExperienceInFieldForce = request.ExperienceInFieldForce,
                TotalExperience = request.TotalExperience,
                AccountNo = request.AccountNo,
                EOBI = request.EOBI,
                GrossSalary = request.GrossSalary,
                Benefits = request.Benefits
            };
            _UserDBContext.UserBasicDetails.Add(UserBasicDetails);
            await _UserDBContext.SaveChangesAsync();
            return Ok(UserBasicDetails);
        }

        [HttpPatch]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserBasicDetails>> UpdateUserBasicDetails(UserBasicDetailsDTO request)
        {
            string id = GetCurrentUser().UserID;
            var dbUser = await _UserDBContext.Users.FindAsync(id);

            if (dbUser is null)
            {
                return BadRequest($"User with id {id} does not exist.");
            }

            var UserBasicDetails = new UserBasicDetails
            {
                UserId = id,
                WorkingHours = request.WorkingHours,
                ReportingTo = request.ReportingTo,
                MaritalStatus = request.MaritalStatus,
                DateOfBirth = request.DateOfBirth,
                ExperienceInFieldForce = request.ExperienceInFieldForce,
                TotalExperience = request.TotalExperience,
                AccountNo = request.AccountNo,
                EOBI = request.EOBI,
                GrossSalary = request.GrossSalary,
                Benefits = request.Benefits
            };

            _UserDBContext.Entry(UserBasicDetails).State = EntityState.Modified;
            await _UserDBContext.SaveChangesAsync();
            return Ok(UserBasicDetails);
        }

        [HttpDelete]
        [Authorize(Roles = "User")]
        public bool DeleteUserBasicDetails()
        {
            string id = GetCurrentUser().UserID;
            bool isDeleted = false;
            var userToDelete = _UserDBContext.UserBasicDetails.Find(id);
            if (userToDelete != null)
            {
                isDeleted = true;
                _UserDBContext.Entry(userToDelete).State = EntityState.Deleted;
                _UserDBContext.SaveChangesAsync();
            }
            else
            {
                isDeleted = false;
            }
            return isDeleted;
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
