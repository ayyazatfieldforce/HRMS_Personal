using HRMS_FieldForce.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS_FieldForce.Models.DBcontext
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBasicDetails> UserBasicDetails { get; set; }
        public DbSet<UserPersonalDetail> UserPersonalDetails { get; set; }

    }
}
