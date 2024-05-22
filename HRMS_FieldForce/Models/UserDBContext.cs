using Microsoft.EntityFrameworkCore;

namespace HRMS_FieldForce.Models
{
    public class UserDBContext:DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options):base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
