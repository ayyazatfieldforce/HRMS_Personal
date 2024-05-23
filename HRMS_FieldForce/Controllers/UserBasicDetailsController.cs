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
        [Route("GetUserBasicDetails")]
        public async Task<IEnumerable<UserBasicDetails>> GetUserDetails()
        {
            return await _UserDBContext.UserBasicDetails.ToListAsync();
        }

        [HttpPost]
        [Route("AddUserBasicDetails")]
        public async Task<ActionResult<UserBasicDetails>> AddUserBasicDetails(UserBasicDetailsDTO request)
        {
            var dbUser = await _UserDBContext.Users.FindAsync(request.UserId);

            if (dbUser is null)
            {
                return BadRequest($"Team with id {request.UserId} does not exist.");
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

        
    }
}
