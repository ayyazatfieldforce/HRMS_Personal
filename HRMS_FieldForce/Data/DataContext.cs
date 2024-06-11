using HRMS_FieldForce.Models;
using Microsoft.EntityFrameworkCore;
using static HRMS_FieldForce.Models.Attendence;

namespace HRMS_FieldForce.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPersonalDetail> UserPersonalDetails { get; set; }
        public DbSet<UserBasicDetail> UserBasicDetails { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Permission> Actions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<GrantPermission> Permissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attendance>()
                .HasKey(a => new { a.UserId, a.Date });

            modelBuilder.Entity<GrantPermission>()
                .HasKey(a => new { a.Role, a.Module,a.Permission });
        }
    }
}
