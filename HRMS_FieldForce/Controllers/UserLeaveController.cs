using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using HRMS_FieldForce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // Leave Category list Controller
        [HttpGet("LeaveCategoryList")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<LeaveCategory>> LeaveCategoryList()
        {
            try
            {
                var leaveCategories = await _context.leaveCategories.ToListAsync();
                return Ok(leaveCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("LeaveCategoryList")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLeaveCategory(LeaveCategory leaveCategory)
        {
            try
            {
                var NewLeaveCategory = new LeaveCategory
                {
                    Id = leaveCategory.Id,
                    Category = leaveCategory.Category
                };
                _context.leaveCategories.Add(NewLeaveCategory);
                await _context.SaveChangesAsync();
                return Ok(NewLeaveCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("LeaveCategoryList")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteLeaveCategory(int id)
        {
            try
            {
                bool isDeleted = false;
                var toDelete = _context.leaveCategories.Find(id);
                if (toDelete != null)
                {
                    isDeleted = true;
                    _context.leaveCategories.Entry(toDelete).State = EntityState.Deleted;
                    _context.SaveChangesAsync();
                    return Ok(isDeleted);
                }
                return BadRequest(isDeleted);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        // Leave Type list Controller
        [HttpGet("LeaveTypeList")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<LeaveCategory>> LeaveTypeList()
        {
            try
            {
                var leaveTypes = await _context.leaveTypes.ToListAsync();
                return Ok(leaveTypes);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost("LeaveTypeList")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLeaveType(LeaveType leaveType)
        {
            try
            {
                var NewLeaveType = new LeaveType
                {
                    Id = leaveType.Id,
                    Type = leaveType.Type
                };
                _context.leaveTypes.Add(leaveType);
                await _context.SaveChangesAsync();
                return Ok(leaveType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("LeaveTypeList")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteLeaveType(int id)
        {
            try
            {
                bool isDeleted = false;
                var toDelete = _context.leaveTypes.Find(id);
                if (toDelete != null)
                {
                    isDeleted = true;
                    _context.leaveTypes.Entry(toDelete).State = EntityState.Deleted;
                    _context.SaveChangesAsync();
                    return Ok(isDeleted);
                }
                return BadRequest(isDeleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Leave Management Controllers
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetLeave([FromQuery] UserLeaveFilterDTO filter)
        {
            try
            {
                IQueryable<UserLeave> query = _context.UserLeaves;

                if (filter.ApplyDate != null)
                {
                    query = query.Where(a => a.ApplyDate == filter.ApplyDate);
                }

                //if (!string.IsNullOrEmpty(filter.LeaveCategory))
                //{
                //    query = query.Where(a => a.LeaveCategory == filter.LeaveCategory);
                //}

                //if (!string.IsNullOrEmpty(filter.LeaveType))
                //{
                //    query = query.Where(a => a.LeaveType == filter.LeaveType);
                //}

                if (!string.IsNullOrEmpty(filter.Reason))
                {
                    query = query.Where(a => a.Reason == filter.Reason);
                }

                if (filter.ToDate != null)
                {
                    query = query.Where(a => a.ToDate == filter.ToDate);
                }

                var userLeaves = await query.ToListAsync();

                return Ok(userLeaves);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve attendance records: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ApplyLeave(UserLeaveDTO user)
        {
            try
            {
                var id = GetCurrentUser().UserID;
                var leaveType = _context.leaveTypes.Where(a => a.Type == user.LeaveType).FirstOrDefault();
                var leavecategory = _context.leaveCategories.Where(a => a.Category == user.LeaveCategory).FirstOrDefault();
                var userLeave = new UserLeave
                {
                    UserId = id,
                    ApplyDate = user.ApplyDate,
                    ToDate = user.ToDate,
                    LeaveTypeID = leaveType.Id,
                    LeaveCategoryID = leavecategory.Id,
                    Reason = user.Reason,
                    status = "Pending",
                };
                _context.UserLeaves.Add(userLeave);
                await _context.SaveChangesAsync();
                return Ok(userLeave);
            }
            catch (Exception ex)
            {
                return BadRequest($"cannot apply for the same date again. \n{ex.Message}");
            }
        }

        [HttpPatch]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserLeave>> EditLeave(UserLeaveDTO user)
        {
            var id = GetCurrentUser().UserID;
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

                //if (!string.IsNullOrEmpty(user.LeaveCategory))
                //{
                //    leave.LeaveCategory = user.LeaveCategory;
                //}

                //if (!string.IsNullOrEmpty(user.LeaveType))
                //{
                //    leave.LeaveType = user.LeaveType;
                //}

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
                    return Ok(isDeleted);
                }
                return BadRequest(isDeleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("maxID")]
        //public ActionResult<LeaveCategory> GetMaxUserID()
        //{
        //    try
        //    {
        //        var lastUser = _context.leaveCategories.OrderByDescending(u => u.Id).FirstOrDefault();
        //        if (lastUser != null)
        //        {
        //            return lastUser;
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

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
