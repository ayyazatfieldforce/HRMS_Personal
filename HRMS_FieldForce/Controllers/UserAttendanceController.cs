using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using HRMS_FieldForce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAttendanceController : ControllerBase
    {
        private readonly UserDBContext _UserDBContext;

        public UserAttendanceController(UserDBContext userDBContext)
        {
            _UserDBContext = userDBContext;
        }
        [HttpGet]
        public async Task<ActionResult<UserAttendance>> GetUserAttendance([FromQuery] string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var UserAttendances = await _UserDBContext.UserAttendances.ToListAsync();
                return Ok(UserAttendances);
            }
            else
            {
                var userDetail = await _UserDBContext.UserAttendances.FindAsync(id);
                if (userDetail is null)
                {
                    return NotFound($"Attendance with UserId {id} not found.");
                }
                return Ok(userDetail);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserAttendance>> userCheckIn (UserAttendanceDTO request)
        {

            var UserIDfromJWT = GetCurrentUserID();

            var UserAttendance = new UserAttendance
            {
                UserId = UserIDfromJWT.UserID,
                day = DateTime.Now.DayOfWeek.ToString(),
                checkIn = request.checkIn,
                checkOut = request.checkOut
            };
            _UserDBContext.UserAttendances.Add(UserAttendance);
            await _UserDBContext.SaveChangesAsync();
            return Ok(UserAttendance);
        }

        [HttpGet("JWTCheck")]
        [Authorize]
        public IActionResult userEndpoint()
        {
            var currentUser = GetCurrentUserID();
            return Ok($"hello {currentUser.UserID} what is your name?");
        }

        private CurrentUserJWT GetCurrentUserID()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new CurrentUserJWT
                {
                    UserID = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    CompanyEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

    }
}
