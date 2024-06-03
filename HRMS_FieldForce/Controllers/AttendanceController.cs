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
                // Finding the attendance entry for the user and date
                var attendance = await _context.Attendances.FindAsync(attendanceDTO.UserId, attendanceDTO.Date);
                if (attendance == null)
                {
                    return NotFound("Attendance entry not found.");
                }

                // Updating check-out time
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
                IQueryable<Attendance> query = _context.Attendances;

                // Apply filters if provided
                if (!string.IsNullOrEmpty(filter.UserId))
                {
                    query = query.Where(a => a.UserId == filter.UserId);
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
                return BadRequest($"Failed to retrieve attendance records: {ex.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAttendance(string id, string date)
        {
            try
            {
                // Find the attendance record to delete
                var attendance = await _context.Attendances.FindAsync(id, date);
                if (attendance == null)
                {
                    return NotFound("Attendance record not found.");
                }

                // Remove the attendance record
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


