using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using HRMS_FieldForce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLeaveController : ControllerBase
    {
        private readonly UserDBContext _context;

        public UserLeaveController(UserDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserLeave()
        {
            var id = GetCurrentUser().UserID;
            return Ok(await _context.UserLeaves.Where(user => user.UserId == id).ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ApplyLeave(UserLeaveDTO user)
        {
            var id = GetCurrentUser().UserID;
            var userLeave = new UserLeave
            {
                UserId = id,
                ApplyDate = user.ApplyDate,
                ToDate = user.ToDate,
                LeaveType = user.LeaveType,
                LeaveCategory = user.LeaveCategory,
                Reason = user.Reason,
                status = "Pending",
            };
            _context.UserLeaves.Add(userLeave);
            await _context.SaveChangesAsync();
            return Ok(userLeave);

        }

        [HttpPatch]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> EditLeave(UserLeaveDTO user)
        {
            var id = GetCurrentUser().UserID;
            //var userToEdit = _context.UserLeaves.Find(id, user.ApplyDate);
            //if (userToEdit == null)
            //{
            //    return BadRequest("user not found");
            //}
            //var userLeave = new UserLeave
            //{
            //    UserId = id,
            //    ApplyDate = user.ApplyDate,
            //    ToDate = user.ToDate,
            //    LeaveType = user.LeaveType,
            //    LeaveCategory = user.LeaveCategory,
            //    Reason = user.Reason,
            //    status = "Pending",
            //};
            //_context.UserLeaves.Entry(userLeave).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            //return Ok(userLeave);

            try
            {
                // Find the attendance record to update
                var leave = await _context.UserLeaves.FindAsync(id, user.ApplyDate);
                if (leave == null)
                {
                    return NotFound("Attendance record not found.");
                }

                // Update the fields if provided
                if (user.ApplyDate != null)
                {
                    leave.ApplyDate = user.ApplyDate;
                }

                if (user.ToDate != null)
                {
                    leave.ToDate = user.ToDate;
                }

                if (!string.IsNullOrEmpty(user.LeaveCategory))
                {
                    leave.LeaveCategory = user.LeaveCategory;
                }

                if (!string.IsNullOrEmpty(user.LeaveType))
                {
                    leave.LeaveType = user.LeaveType;
                }

                if (!string.IsNullOrEmpty(user.Reason))
                {
                    leave.Reason = user.Reason;
                }

                // Save changes
                await _context.SaveChangesAsync();

                return Ok("Leave record updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update Leave record: {ex.Message}");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User")]
        public IActionResult DeleteLeave(DateOnly date)
        {
            try
            {
                string id = GetCurrentUser().UserID;
                bool isDeleted = false;
                var userToDelete = _context.UserLeaves.Find(id,date);
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
                return Ok(isDeleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
