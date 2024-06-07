using HRMS_FieldForce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<UserAttendance> UserAttendances { get; set; }
        public DbSet<UserLeave> UserLeaves { get; set; }
        public DbSet<LeaveCategory> leaveCategories { get; set; }
        public DbSet<LeaveType> leaveTypes { get; set; }

        // for composite key in attendace module
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAttendance>().HasKey(ua => new { ua.UserId, ua.day });
            modelBuilder.Entity<UserLeave>().HasKey(ua => new { ua.UserId, ua.ApplyDate });
        }

    }
}
