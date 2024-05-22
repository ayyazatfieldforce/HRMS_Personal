using HRMS_FieldForce.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS_FieldForce.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}
