using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
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
    public class currentUser : ControllerBase
    {

        private readonly UserDBContext _context;

        public currentUser(UserDBContext userDBContext)
        {
            _context = userDBContext;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<User>> get()
        {
            try
            {
                string id = GetCurrentUser().UserID;
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
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
