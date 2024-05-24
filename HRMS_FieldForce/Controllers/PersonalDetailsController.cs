using HRMS_FieldForce.Data;
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

            var dbUser = await _context.Users.FindAsync(request.UserId);

            if (dbUser is null)
            {
                return BadRequest($"User with id {request.UserId} doesnot exists.");
            }

            // Assume that UserId is provided in the DTO for simplicity.
            // In a real-world scenario, you might get it from the authenticated user context.
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



        [HttpGet]
        public async Task<ActionResult<UserPersonalDetail>> GetUserPersonalDetails([FromQuery] string? id)
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




        [HttpPut("{id}")]
        public async Task<ActionResult<UserPersonalDetail>> PutUserPersonalDetail(string id, UserPersonalDetailDTO request)
        {
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool UserPersonalDetailExists(string id)
        {
            return _context.UserPersonalDetails.Any(e => e.UserId == id);
        }
    }



}
