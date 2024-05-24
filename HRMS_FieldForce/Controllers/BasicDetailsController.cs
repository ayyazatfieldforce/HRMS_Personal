using HRMS_FieldForce.Data;
using HRMS_FieldForce.DTOs;
using HRMS_FieldForce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBasicDetailsController : ControllerBase
    {
        private readonly DataContext _context;

        public UserBasicDetailsController(DataContext userDBContext)
        {
            _context = userDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<UserBasicDetail>> GetUserDetails([FromQuery] string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var UserBasicDetails = await _context.UserBasicDetails.ToListAsync();
                return Ok(UserBasicDetails);
            }
            else
            {
                var userDetail = await _context.UserBasicDetails.FindAsync(id);
                if (userDetail is null)
                {
                    return NotFound($"UserPersonalDetail with UserId {id} not found.");
                }
                return Ok(userDetail);
            }
        }


        [HttpPost]
        [Route("AddUserBasicDetails")]
        public async Task<ActionResult<UserBasicDetail>> AddUserBasicDetails(UserBasicDetailDTO request)
        {
            var dbUser = await _context.Users.FindAsync(request.UserId);

            if (dbUser is null)
            {
                return BadRequest($"User with id {request.UserId} does not exist.");
            }

            var UserBasicDetail = new UserBasicDetail
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
            _context.UserBasicDetails.Add(UserBasicDetail);
            await _context.SaveChangesAsync();
            return Ok(UserBasicDetail);
        }

        [HttpPatch]
        [Route("UpdateUserBasicDetails/(id)")]
        public async Task<ActionResult<UserBasicDetail>> UpdateUserBasicDetails(UserBasicDetailDTO request)
        {
            var dbUser = await _context.Users.FindAsync(request.UserId);

            if (dbUser is null)
            {
                return BadRequest($"User with id {request.UserId} does not exist.");
            }

            var UserBasicDetails = new UserBasicDetail
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

            _context.Entry(UserBasicDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(UserBasicDetails);
        }

        [HttpDelete]
        [Route("DeleteUserBasicDetails/(id)")]
        public bool DeleteUserBasicDetails(string id)
        {
            bool isDeleted = false;
            var userToDelete = _context.UserBasicDetails.Find(id);
            if (userToDelete != null)
            {
                isDeleted = true;
                _context.Entry(userToDelete).State = EntityState.Deleted;
                _context.SaveChangesAsync();
            }
            else
            {
                isDeleted = false;
            }
            return isDeleted;
        }
    }
}
