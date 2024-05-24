using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using HRMS_FieldForce.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<UserBasicDetails>> GetUserDetails([FromQuery] string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var UserBasicDetails = await _UserDBContext.UserBasicDetails.ToListAsync();
                return Ok(UserBasicDetails);
            }
            else
            {
                var userDetail = await _UserDBContext.UserBasicDetails.FindAsync(id);
                if (userDetail is null)
                {
                    return NotFound($"UserPersonalDetail with UserId {id} not found.");
                }
                return Ok(userDetail);
            }
        }


        [HttpPost]
        public async Task<ActionResult<UserBasicDetails>> AddUserBasicDetails(UserBasicDetailsDTO request)
        {
            var dbUser = await _UserDBContext.Users.FindAsync(request.UserId);

            if (dbUser is null)
            {
                return BadRequest($"User with id {request.UserId} does not exist.");
            }

            var UserBasicDetails = new UserBasicDetails
            {
                UserId = request.UserId,
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
        public async Task<ActionResult<UserBasicDetails>> UpdateUserBasicDetails(UserBasicDetailsDTO request)
        {
            var dbUser = await _UserDBContext.Users.FindAsync(request.UserId);

            if (dbUser is null)
            {
                return BadRequest($"User with id {request.UserId} does not exist.");
            }

            var UserBasicDetails = new UserBasicDetails
            {
                UserId = request.UserId,
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
        public bool DeleteUserBasicDetails(string id)
        {
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
    }
}
