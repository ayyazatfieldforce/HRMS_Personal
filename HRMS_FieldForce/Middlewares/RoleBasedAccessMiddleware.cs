using HRMS_FieldForce.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace HRMS_FieldForce.Middlewares
{
    public class RoleBasedAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public RoleBasedAccessMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
 
            var path = context.Request.Path.Value;
            if (path.StartsWith("/swagger") || path.StartsWith("/api-docs")|| path.StartsWith("/api/Auth/login"))
            {
                await _next(context);
                return;
            }
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
                    var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                    context.Items["Role"] = role;
                    context.Items["UserId"] = userId;
                }
                catch
                {
                    context.Response.StatusCode =(int) HTTPCallStatus.NotAuthenticated;



                    await context.Response.WriteAsync($"{(int)HTTPCallStatus.NotAuthenticated}: Token is not authenticated");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HTTPCallStatus.NotAuthenticated;
                
                await context.Response.WriteAsync($"{(int)HTTPCallStatus.NotAuthenticated}: Token is missing");
                return;
            }

            await _next(context);
        }
    }
}
