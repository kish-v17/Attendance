using Attendance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserModel> UserTbl {  get; set; }
        public DbSet<DepartmentModel> DepartmentTbl { get; set; }
        public DbSet<SubjectModel> SubjectTbl { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel {
                    UserId = 1,
                    Fullname = "Kishan Patel",
                    Email = "kish.v07@gmail.com",
                    Password = "Admin",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
