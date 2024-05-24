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
            var dbUser = await _UserDBContext.Users.FindAsync(request.UserId);

            if (dbUser is null)
            {
                return BadRequest($"User with id {request.UserId} does not exist.");
            }

            var UserAttendance = new UserAttendance
            {
                UserId = request.UserId,
                day = DateTime.Now.DayOfWeek.ToString(),
                checkIn = request.checkIn,
                checkOut = request.checkOut
            };
            _UserDBContext.UserAttendances.Add(UserAttendance);
            await _UserDBContext.SaveChangesAsync();
            return Ok(UserAttendance);
        }
    }
}
