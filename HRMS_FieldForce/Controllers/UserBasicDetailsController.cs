using HRMS_FieldForce.Models;
using HRMS_FieldForce.Models.DBcontext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_FieldForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBasicDetailsController : ControllerBase
    {
        private readonly UserDBContext _UserDBContext;

        public UserBasicDetailsController(UserDBContext userDBContext)
        {
            _UserDBContext = userDBContext;
        }

        [HttpGet]
        [Route("GetUserBasicDetails")]
        public async Task<IEnumerable<UserBasicDetails>> GetUserDetails()
        {
            return await _UserDBContext.UserBasicDetails.ToListAsync();
        }

        [HttpPost]
        [Route("AddUserBasicDetails")]
        public async Task<UserBasicDetails> AddStudentDetails(UserBasicDetails request)
        {
            _UserDBContext.UserBasicDetails.Add(request);
            await _UserDBContext.SaveChangesAsync();
            return request;
        }

        [HttpPatch]
        [Route("UpdateUserBasicDetails/(id)")]
        public async Task<UserBasicDetails> UpdateStudentDetails(UserBasicDetails request)
        {
            _UserDBContext.Entry(request).State = EntityState.Modified;
            await _UserDBContext.SaveChangesAsync();
            return request;
        }

        [HttpDelete]
        [Route("DeleteUserBasicDetails/(id)")]
        public bool DeleteStudentDetails(int id)
        {
            bool isDeleted = false;
            var stu = _UserDBContext.UserBasicDetails.Find(id);
            if (stu != null)
            {
                isDeleted = true;
                _UserDBContext.Entry(stu).State = EntityState.Deleted;
                _UserDBContext.SaveChangesAsync();
            }
            else
            {
                isDeleted = false;
            }
            return isDeleted;
        }
    }
}
