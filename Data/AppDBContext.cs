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
        public DbSet<SubjectModel> SubjectTbl { get; set; }
        public DbSet<BatchModel> BatchTbl{ get; set; }
        public DbSet<StudentModel> StudentTbl{ get; set; }
        public DbSet<ClassModel> ClassTbl { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel {
                    UserId = 1,
                    Fullname = "Kishan Patel",
                    Email = "kish.v07@gmail.com",
                    Password = "Admin@123",
                    Role = UserRole.Admin,
                    MobileNo = "9925323126"
                },
                new UserModel {
                    UserId = 2,
                    Fullname = "Abhi Patel",
                    Email = "busyman2561@gmail.com",
                    Password = "Abhi@123",
                    Role = UserRole.Faculty,
                    MobileNo = "7383835015"
                }
            );
            modelBuilder.Entity<DepartmentModel>().HasData(
                new DepartmentModel {
                    DepartmentId = 1,
                    DepartmentName = "Computer Science"
                },
                new DepartmentModel {
                    DepartmentId = 2,
                    DepartmentName = "Information Technology"
                },
                new DepartmentModel {
                    DepartmentId = 3,
                    DepartmentName = "Diploma Studies"
                },
                new DepartmentModel {
                    DepartmentId = 4,
                    DepartmentName = "Management"
                }
            );
            modelBuilder.Entity<CourseModel>().HasData(
                new CourseModel
                {
                    CourseId = 1,
                    CourseName = "Master of Computer Application",
                    DepartmentId = 1
                }, new CourseModel
                {
                    CourseId = 2,
                    CourseName = "Bachelor of Computer Applicatiion",
                    DepartmentId = 1
                }, new CourseModel
                {
                    CourseId = 3,
                    CourseName = "Bachelor of Technology",
                    DepartmentId = 1
                }, new CourseModel
                {
                    CourseId = 4,
                    CourseName = "Bachelor of Technology",
                    DepartmentId = 2
                }
            );
            modelBuilder.Entity<SubjectModel>().HasData(
               new SubjectModel
               {
                   SubjectId = 1,
                   SubjectName = "Database Management System"
               },new SubjectModel
               {
                   SubjectId = 2,
                   SubjectName = "Data Structures"
               },new SubjectModel
               {
                   SubjectId = 3,
                   SubjectName = "Programing With Java"
               }
            );
            modelBuilder.Entity<ClassModel>().HasData(
               new ClassModel
               {
                   ClassId = 1,
                   BatchId=1,
                   Class = "A"
               }, new ClassModel
               {
                   ClassId = 2,
                   BatchId = 1,
                   Class = "B"
               }, new ClassModel
               {
                   ClassId = 3,
                   BatchId = 2,
                   Class = "A"
               }
            );
            modelBuilder.Entity<BatchModel>().HasData(
               new BatchModel
               {
                   BatchId = 1,
                   CourseId = 1,
                   Semester = 2,
                   Year = 2024,
                   StartDate = new DateOnly(2025, 01, 07),
                   EndDate= new DateOnly(2025,05,31)
               }, new BatchModel
               {
                   BatchId = 2,
                   CourseId = 2,
                   Semester = 6,
                   Year = 2022,
                   StartDate = new DateOnly(2025, 01, 16),
                   EndDate = new DateOnly(2025, 06, 05)
               }
            );
            modelBuilder.Entity<StudentModel>().HasData(
               new StudentModel
               {
                   StudentId = 1,
                   FullName = "Dhruv Hiteshbhai Patel",
                   FatherName = "Hiteshbhai Patel",
                   AadharCardNumber = "789654123012",
                   Address = "Satellite Road, Nr. Shivalik Plaza",
                   BloodGroup = BloodGroup.B_Positive,
                   Category = Category.OPEN,
                   City = "Ahmedabad",
                   ClassId = 2,
                   State = "Gujarat",
                   EnrollmentNumber = "24CSMCA00001",
                   MobileNo = "9876543210",
                   ParentMobileNo = "9825034567",
                   Email = "dpatel32@rku.ac.in",
                   DateOfBirth = new DateOnly(2003, 8, 21),
                   Country = "India",
                   MotherName = "Bhavnaben Hiteshbhai Patel",
                   PinCode = "380015",
                   Gender = Gender.Male
               }, new StudentModel
               {
                   StudentId = 2,
                   FullName = "Harsh Jitubhai Savani",
                   FatherName = "Jitubhai Savani",
                   AadharCardNumber = "854796321045",
                   Address = "150 Feet Ring Road, Nr. Indira Circle",
                   BloodGroup = BloodGroup.O_Positive,
                   Category = Category.OPEN,
                   City = "Rajkot",
                   ClassId = 3,
                   State = "Gujarat",
                   EnrollmentNumber = "24CSBCA00001",
                   MobileNo = "7984563210",
                   ParentMobileNo = "9825098745",
                   Email = "hsavani456@rku.ac.in",
                   DateOfBirth = new DateOnly(2002, 11, 12),
                   Country = "India",
                   MotherName = "Meenaben Jitubhai Savani",
                   PinCode = "360005",
                   Gender = Gender.Male
               }
            );
        }
    }
}
