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
        public DbSet<CourseModel> CourseTbl { get; set; }
        public DbSet<SubjectModel>SubjectTbl { get; set; }
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
            modelBuilder.Entity<DepartmentModel>().HasData(
                new DepartmentModel {
                    DepartmentId = 1,
                    DepartmentName= "Computer Science"
                },
                new DepartmentModel {
                    DepartmentId = 2,
                    DepartmentName= "Information Technology"
                },
                new DepartmentModel {
                    DepartmentId = 3,
                    DepartmentName= "Diploma Studies"
                },
                new DepartmentModel {
                    DepartmentId = 4,
                    DepartmentName= "Management"
                }
            );
            modelBuilder.Entity<CourseModel>().HasData(
                new CourseModel
                {
                    CourseId = 1,
                    CourseName="Master of Computer Application",
                    DepartmentId=1
                },new CourseModel
                {
                    CourseId = 2,
                    CourseName="Bachelor of Computer Applicatiion",
                    DepartmentId=1
                },new CourseModel
                {
                    CourseId = 3,
                    CourseName="Bachelor of Technology",
                    DepartmentId=1
                },new CourseModel
                {
                    CourseId = 4,
                    CourseName= "Bachelor of Technology",
                    DepartmentId=2
                }
            );
        }
    }
}
