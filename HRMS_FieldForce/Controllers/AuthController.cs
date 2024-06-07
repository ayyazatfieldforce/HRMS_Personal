using HRMS_FieldForce.Data;
using HRMS_FieldForce.DTOs;
using HRMS_FieldForce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class AuthController : ControllerBase
    {
        public static User? _user;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
         
        
        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]

        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var dbUser = await _context.Users.FindAsync(request.CompanyEmail);

            if (dbUser is not null)
            {
                return BadRequest($"Team with id {dbUser.CompanyEmail} already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var _user = new User
            {
                CompanyEmail = request.CompanyEmail,
                LastName = request.LastName,
                FirstName = request.FirstName,
                UserId = request.UserId,
                Role = request.Role,
                PersonalEmail = request.PersonalEmail,
                DateOfBirth = request.DateOfBirth,
                HashPassword = passwordHash
            };

            _context.Users.Add(_user);
            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();
            return Ok("User Added Succfuly");

        }

        [HttpPost("login")]

        public async Task<ActionResult<string>> Login(LoginDTO request)
        {

            var dbUser = await _context.Users.SingleOrDefaultAsync(user => user.CompanyEmail == request.CompanyEmail);

            if (dbUser is null)
            {
                return BadRequest("Company Email or Password  is incorrect");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, dbUser.HashPassword))
            {
                return BadRequest("Company Email or Password  is incorrect");
            }

            var token = CreateToken(dbUser);


            return Ok(token);



        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {

            new Claim(ClaimTypes.Role, user.Role),  
            new Claim(ClaimTypes.NameIdentifier,user.UserId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials:creds
                );  

            var jwt=new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
            
        }



    }
}
