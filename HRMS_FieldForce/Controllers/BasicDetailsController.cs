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
            var role = HttpContext.Items["Role"] as string;
            var contextUserId = HttpContext.Items["UserId"] as string;

            if (role == "R1" || role == "R2")
            {
                // R1 and R2 can access any user details
                if (string.IsNullOrEmpty(id))
                {
                    var userBasicDetails = await _context.UserBasicDetails.ToListAsync();
                    return Ok(userBasicDetails);
                }
                else
                {
                    var userDetail = await _context.UserBasicDetails.FindAsync(id);
                    if (userDetail is null)
                    {
                        return NotFound($"UserBasicDetail with UserId {id} not found.");
                    }
                    return Ok(userDetail);
                }
            }
            else if (role == "R3")
            {
                // R3 can only access their own details
                if (id == contextUserId)
                {
                    var userDetail = await _context.UserBasicDetails.FindAsync(id);
                    if (userDetail is null)
                    {
                        return NotFound($"UserBasicDetail with UserId {id} not found.");
                    }
                    return Ok(userDetail);
                }
                else
                {
                    return Forbid("You do not have permission to access this user's details.");
                }
            }
            else
            {
                return Forbid("You do not have permission to access this resource.");
            }
        }



        [HttpPost]
        [Route("AddUserBasicDetails")]
        public async Task<ActionResult<UserBasicDetail>> AddUserBasicDetails(UserBasicDetailDTO request)
        {
            try
            {
                var role = HttpContext.Items["Role"] as string;

                if (role != "R1" && role != "R2")
                {
                    return Forbid("You do not have permission to add user basic details.");
                }

                var dbUser = await _context.Users.FindAsync(request.UserId);

                if (dbUser is null)
                {
                    return BadRequest($"User with id {request.UserId} does not exist.");
                }

                var userBasicDetail = new UserBasicDetail
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

                _context.UserBasicDetails.Add(userBasicDetail);
                await _context.SaveChangesAsync();

                return Ok(userBasicDetail);
            }
            catch 
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPatch]
        [Route("UpdateUserBasicDetails/{id}")]
        public async Task<ActionResult<UserBasicDetail>> UpdateUserBasicDetails(string id, UserBasicDetailDTO request)
        {
            try
            {
                var role = HttpContext.Items["Role"] as string;

                if (role != "R1" && role != "R2")
                {
                    return Forbid("You do not have permission to update user basic details.");
                }

                if (id != request.UserId)
                {
                    return BadRequest("User ID mismatch.");
                }

                var dbUser = await _context.Users.FindAsync(request.UserId);

                if (dbUser is null)
                {
                    return NotFound($"User with id {request.UserId} does not exist.");
                }

                var userBasicDetails = await _context.UserBasicDetails.FindAsync(id);
                if (userBasicDetails is null)
                {
                    return NotFound($"UserBasicDetail with UserId {id} not found.");
                }

                userBasicDetails.WorkingHours = request.WorkingHours;
                userBasicDetails.ReportingTo = request.ReportingTo;
                userBasicDetails.MaritalStatus = request.MaritalStatus;
                userBasicDetails.DateOfBirth = request.DateOfBirth;
                userBasicDetails.ExperienceInFieldForce = request.ExperienceInFieldForce;
                userBasicDetails.TotalExperience = request.TotalExperience;
                userBasicDetails.AccountNo = request.AccountNo;
                userBasicDetails.EOBI = request.EOBI;
                userBasicDetails.GrossSalary = request.GrossSalary;
                userBasicDetails.Benefits = request.Benefits;

                _context.Entry(userBasicDetails).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(userBasicDetails);
            }
            catch 
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpDelete]
        [Route("DeleteUserBasicDetails/{id}")]
        public async Task<ActionResult<bool>> DeleteUserBasicDetails(string id)
        {
            try
            {
                var role = HttpContext.Items["Role"] as string;

                if (role != "R1" && role != "R2")
                {
                    return Forbid("You do not have permission to delete user basic details.");
                }

                var userToDelete = await _context.UserBasicDetails.FindAsync(id);
                if (userToDelete == null)
                {
                    return NotFound($"UserBasicDetail with UserId {id} not found.");
                }

                _context.Entry(userToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
