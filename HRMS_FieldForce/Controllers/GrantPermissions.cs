using HRMS_FieldForce.Data;
using HRMS_FieldForce.DTOs;
using HRMS_FieldForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrantPermissionsController : ControllerBase
    {
        private readonly DataContext _context;

        public GrantPermissionsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/GrantPermissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrantPermission>>> GetGrantPermissions()
        {
            return await _context.Permissions.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<GrantPermission>> PostGrantPermission(GrantPermissionDTO grantPermissionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the permission already exists
            if (GrantPermissionExists(grantPermissionDTO.Role, grantPermissionDTO.Module, grantPermissionDTO.Permission))
            {
                return Conflict("The permission already exists.");
            }

            // Create a new GrantPermission object
            var grantPermission = new GrantPermission
            {
                Role = grantPermissionDTO.Role,
                Module = grantPermissionDTO.Module,
                Permission = grantPermissionDTO.Permission
            };

            // Add the new GrantPermission to the database
            _context.Permissions.Add(grantPermission);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGrantPermissions), new { role = grantPermission.Role, module = grantPermission.Module, permission = grantPermission.Permission }, grantPermission);
        }

        // Helper method to check if the permission already exists
        private bool GrantPermissionExists(string role, int module, int permission)
        {
            return _context.Permissions.Any(e => e.Role == role && e.Module == module && e.Permission == permission);
        }
    }
}
