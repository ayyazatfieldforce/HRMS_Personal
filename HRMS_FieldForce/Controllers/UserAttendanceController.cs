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
    public class UserAttendanceController : ControllerBase
    {
        private readonly UserDBContext _UserDBContext;

        public UserAttendanceController(UserDBContext userDBContext)
        {
            _UserDBContext = userDBContext;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<UserAttendance>> GetUserAttendance()
        {
            string id = GetCurrentUser().UserID;
            var userDetail = await _UserDBContext.UserAttendances
                            .Where(user => user.UserId == id)
                            .ToListAsync();
            if (userDetail is null)
            {
                return NotFound($"Attendance with UserId {id} not found.");
            }
            return Ok(userDetail);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<UserAttendance>> MarkUserAttendance(UserAttendanceDTO request)
        {
            var UserIDfromJWT = GetCurrentUser();

            var UserAttendance = new UserAttendance
            {
                UserId = UserIDfromJWT.UserID,
                day = request.day,
                checkIn = request.checkIn,
                checkOut = request.checkOut
            };
            _UserDBContext.UserAttendances.Add(UserAttendance);
            await _UserDBContext.SaveChangesAsync();
            return Ok(UserAttendance);
        }

        [HttpPatch]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<UserAttendance>> UpdateUserBasicDetails(UserAttendanceDTO request)
        {
            string id = GetCurrentUser().UserID;
            var dbUser = await _UserDBContext.UserAttendances.FindAsync(id, request.day);

            if (dbUser is null)
            {
                return BadRequest($"User with id {id} does not exist.");
            }

            var UserAttendance = new UserAttendance
            {
                UserId = id,
                day = request.day,
                checkIn = request.checkIn,
                checkOut = request.checkOut
            };

            _UserDBContext.Entry(UserAttendance).State = EntityState.Modified;
            await _UserDBContext.SaveChangesAsync();
            return Ok(UserAttendance);
        }

        [HttpDelete]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public bool DeleteAttendance(string day)
        {
            string id = GetCurrentUser().UserID;
            bool isDeleted;
            var userToDelete = _UserDBContext.UserAttendances.Find(id, day);
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
