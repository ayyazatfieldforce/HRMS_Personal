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

        //public DbSet<Attendance> Attendances { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Attendance>()
        //        .HasKey(a => new { a.UserId, a.Date });
        //}
    }
}
