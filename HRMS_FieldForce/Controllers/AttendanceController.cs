using HRMS_FieldForce.Data;
using HRMS_FieldForce.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HRMS_FieldForce.Models.Attendence;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly DataContext _context;
        public AttendanceController(DataContext userDBContext)
        {
            _context = userDBContext;
        }


        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] AttendanceDTO attendanceDTO)
        {
            try
            {
                var userIdFromContext = HttpContext.Items["UserId"] as string;

                if (userIdFromContext == null || userIdFromContext != attendanceDTO.UserId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to check in for this user.");
                }

                var attendance = new Attendance
                {
                    UserId = attendanceDTO.UserId,
                    Date = attendanceDTO.Date,
                    CheckInTime = attendanceDTO.CheckInTime,
                    WorkFrom = attendanceDTO.WorkFrom
                };

                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                return Ok("Check-in successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to check in: {ex.Message}");
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] AttendanceDTO attendanceDTO)
        {
            try
            {
                var userIdFromContext = HttpContext.Items["UserId"] as string;

                if (userIdFromContext == null || userIdFromContext != attendanceDTO.UserId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to check out for this user.");
                }

                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.UserId == attendanceDTO.UserId && a.Date == attendanceDTO.Date);

                if (attendance == null)
                {
                    return NotFound("Attendance entry not found.");
                }

                attendance.CheckOutTime = attendanceDTO.CheckOutTime;

                _context.Entry(attendance).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Check-out successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to check out: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendance([FromQuery] AttendanceFilterDTO filter)
        {
            try
            {
                var role = HttpContext.Items["Role"] as string;
                var userIdFromContext = HttpContext.Items["UserId"] as string;

                IQueryable<Attendance> query = _context.Attendances;

                if (role == "R1" || role == "R2")
                {
                    if (!string.IsNullOrEmpty(filter.UserId))
                    {
                        query = query.Where(a => a.UserId == filter.UserId);
                    }
                }
                else if (role == "R3")
                {
                    if (!string.IsNullOrEmpty(filter.UserId))
                    {
                        if (filter.UserId != userIdFromContext)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to search for this user.");
                        }
                    }
                    else
                    {
                        query = query.Where(a => a.UserId == userIdFromContext);
                    }
                }

                if (!string.IsNullOrEmpty(filter.Date))
                {
                    query = query.Where(a => a.Date == filter.Date);
                }

                if (!string.IsNullOrEmpty(filter.CheckInTime))
                {
                    query = query.Where(a => a.CheckInTime == filter.CheckInTime);
                }

                if (!string.IsNullOrEmpty(filter.CheckOutTime))
                {
                    query = query.Where(a => a.CheckOutTime == filter.CheckOutTime);
                }

                if (!string.IsNullOrEmpty(filter.WorkFrom))
                {
                    query = query.Where(a => a.WorkFrom == filter.WorkFrom);
                }

                var attendances = await query.ToListAsync();

                return Ok(attendances);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve attendance: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAttendance(string id, string date)
        {
            try
            {
                var role = HttpContext.Items["Role"] as string;

                if (role != "R1" && role != "R2")
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to delete attendance records.");
                }

                var attendance = await _context.Attendances.FindAsync(id, date);
                if (attendance == null)
                {
                    return NotFound("Attendance record not found.");
                }

                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();

                return Ok("Attendance record deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete attendance record: {ex.Message}");
            }
        }

        [HttpPut("{id}/{date}")]
        public async Task<IActionResult> UpdateAttendance(string id, string date, [FromBody] AttendanceDTO updateDto)
        {
            try
            {
                var role = HttpContext.Items["Role"] as string;

                if (role != "R1" && role != "R2")
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to update attendance records.");
                }

                // Find the attendance record to update
                var attendance = await _context.Attendances.FindAsync(id, date);
                if (attendance == null)
                {
                    return NotFound("Attendance record not found.");
                }

                // Update the fields if provided
                if (!string.IsNullOrEmpty(updateDto.CheckInTime))
                {
                    attendance.CheckInTime = updateDto.CheckInTime;
                }

                if (!string.IsNullOrEmpty(updateDto.CheckOutTime))
                {
                    attendance.CheckOutTime = updateDto.CheckOutTime;
                }

                if (!string.IsNullOrEmpty(updateDto.WorkFrom))
                {
                    attendance.WorkFrom = updateDto.WorkFrom;
                }

                // Save changes
                await _context.SaveChangesAsync();

                return Ok("Attendance record updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update attendance record: {ex.Message}");
            }
        }

    }

}


