using BCrypt.Net;
using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using HRMS_FieldForce.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserDBContext _userDB;
        public AuthController(IConfiguration configuration, UserDBContext userDBContext)
        {
            _configuration = configuration;
            _userDB = userDBContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register (UserDTORegister request)
        {
            var dbUser = await _userDB.Users.SingleOrDefaultAsync(user => user.CompanyEmail == request.CompanyEmail);
            if (dbUser != null)
            {
                return BadRequest("User already Exist");
            }

            var PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var _user = new User
            {
                CompanyEmail = request.CompanyEmail,
                LastName = request.LastName,
                FirstName = request.FirstName,
                UserId = request.UserId,
                Role = request.Role,
                PersonalEmail = request.PersonalEmail,
                DateOfBirth = request.DateOfBirth,
                PasswordHash = PasswordHash
            };
            
            _userDB.Users.Add(_user);
            await _userDB.SaveChangesAsync();
            return _user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login (UserDTOLogin request)
        {
            var dbUser = await _userDB.Users.SingleOrDefaultAsync(user => user.CompanyEmail == request.CompanyEmail);
            if (dbUser == null)
            {
                return BadRequest("Username or Password is Wrong");
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password, dbUser.PasswordHash))
            {
                return BadRequest("Username or Password is Wrong");
            }
            var token = CreateToken(dbUser);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var key = _configuration.GetValue<string>("Appsettings:Token");
            var keyBytes = Encoding.UTF8.GetBytes(key!);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                     {
                     new Claim(ClaimTypes.NameIdentifier, user.UserId),
                     new Claim(ClaimTypes.Email, user.CompanyEmail),
                     new Claim(ClaimTypes.Role, user.Role)
                 }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                                      new SymmetricSecurityKey(keyBytes),
                                      SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
    }
}
