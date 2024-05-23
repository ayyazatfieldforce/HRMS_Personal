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
                return BadRequest($"Team with id {request.UserId} does not exist.");
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



    }
}
