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
    public class PersonalDetailsController : ControllerBase
    {

        private readonly DataContext _context;

        public PersonalDetailsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<UserPersonalDetail>> PostUserPersonalDetail(UserPersonalDetailDTO request)
        {
            try
            {

                var role = HttpContext.Items["Role"] as string;

                var module = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleName == "addPersonalDetails");
                if (module == null)
                {
                    return BadRequest("Module 13 does not exist.");
                }

                bool hasPermission = await _context.Permissions.AnyAsync(p =>
                    p.Role == role &&
                    p.Module == module.ModuleID &&
                    p.Permission == 1);

                if (!hasPermission)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have the required permission to add user personal details.");
                }

                if (role != "R1" && role != "R2")
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to add user personal details.");
                }

                var dbUser = await _context.Users.FindAsync(request.UserId);

                if (dbUser == null)
                {
                    return BadRequest($"User with id {request.UserId} does not exist.");
                }

                string userId = request.UserId;

                var userPersonalDetail = new UserPersonalDetail
                {
                    UserId = userId,
                    FatherName = request.FatherName,
                    CNIC = request.CNIC,
                    Phone = request.Phone,
                    EmergencyContact = request.EmergencyContact,
                    EmployeeStatus = request.EmployeeStatus,
                    Branch = request.Branch,
                    Department = request.Department,
                    Designation = request.Designation,
                    JobGrade = request.JobGrade,
                    JoiningDate = request.JoiningDate,
                    Address = request.Address,
                    PermanentAddress = request.PermanentAddress
                };

                _context.UserPersonalDetails.Add(userPersonalDetail);
                await _context.SaveChangesAsync();

                return Ok("User Personal Details Added");
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding user personal details. Please try again later.");
            }
        }


        [HttpGet]
        public async Task<ActionResult<UserPersonalDetail>> GetUserPersonalDetails([FromQuery] string? id)
        {
            var role = HttpContext.Items["Role"] as string;
            var userId = HttpContext.Items["UserId"] as string;

            if (role == "R1" || role == "R2")
            {
                if (string.IsNullOrEmpty(id))
                {
                    var userPersonalDetails = await _context.UserPersonalDetails.ToListAsync();
                    return Ok(userPersonalDetails);
                }
                else
                {
                    var userDetail = await _context.UserPersonalDetails.FindAsync(id);
                    if (userDetail is null)
                    {
                        return NotFound($"UserPersonalDetail with UserId {id} not found.");
                    }
                    return Ok(userDetail);
                }
            }
            else if (role == "R3")
            {
                if (id == userId)
                {
                    var userDetail = await _context.UserPersonalDetails.FindAsync(id);
                    if (userDetail is null)
                    {
                        return NotFound($"UserPersonalDetail with UserId {id} not found.");
                    }
                    return Ok(userDetail);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to access this account.");
                }
            }

            return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to access this account.");
        }




        [HttpPut("{id}")]
        public async Task<ActionResult<UserPersonalDetail>> PutUserPersonalDetail(string id, UserPersonalDetailDTO request)
        {
            var role = HttpContext.Items["Role"] as string;

            if (role != "R1" && role != "R2")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to update user personal details.");
            }

            if (id != request.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            var userPersonalDetail = await _context.UserPersonalDetails.FindAsync(id);

            if (userPersonalDetail == null)
            {
                return NotFound($"UserPersonalDetail with UserId {id} not found.");
            }

            userPersonalDetail.FatherName = request.FatherName;
            userPersonalDetail.CNIC = request.CNIC;
            userPersonalDetail.Phone = request.Phone;
            userPersonalDetail.EmergencyContact = request.EmergencyContact;
            userPersonalDetail.EmployeeStatus = request.EmployeeStatus;
            userPersonalDetail.Branch = request.Branch;
            userPersonalDetail.Department = request.Department;
            userPersonalDetail.Designation = request.Designation;
            userPersonalDetail.JobGrade = request.JobGrade;
            userPersonalDetail.JoiningDate = request.JoiningDate;
            userPersonalDetail.Address = request.Address;
            userPersonalDetail.PermanentAddress = request.PermanentAddress;

            _context.Entry(userPersonalDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPersonalDetailExists(id))
                {
                    return NotFound($"UserPersonalDetail with UserId {id} not found.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the record. Please try again later.");
                }
            }
            catch 
            {
      
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the record. Please try again later.");
            }

            return NoContent();
        }

        private bool UserPersonalDetailExists(string id)
        {
            return _context.UserPersonalDetails.Any(e => e.UserId == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPersonalDetail(string id)
        {
            var role = HttpContext.Items["Role"] as string;

            if (role != "R1" && role != "R2")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to delete user personal details.");
            }

            try
            {
                var userPersonalDetail = await _context.UserPersonalDetails.FindAsync(id);
                if (userPersonalDetail == null)
                {
                    return NotFound($"UserPersonalDetail with UserId {id} not found.");
                }

                _context.UserPersonalDetails.Remove(userPersonalDetail);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the record. Please try again later.");
            }
        }



    }


}


