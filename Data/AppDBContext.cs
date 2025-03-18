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
                   StudentId= 1,
                   FullName="Jay Jerambhai Gorfad",
                   FatherName="Jerambhai Gorfad",
                   AadharCardNumber="947598246540",
                   Address="Gokul Park,Nr. Kothariya Chowkdi",
                   BloodGroup=BloodGroup.A_Positive,
                   Category=Category.OBC,
                   City="Rajkot",
                   ClassId=1,
                   State="Gujarat",
                   EnrollmentNumber= "2024CSṂCA00001",
                   MobileNo="7689468909",
                   ParentMobileNo="9925323126",
                   Email="jgorfad223@rku.ac.in",
                   DateOfBirth= new DateOnly(2004, 5, 15),
                   Country="India",
                   MotherName="Lilaben Jerambhai Gorfad",
                   PinCode="360002",
                   Gender=Gender.Male
               }, new StudentModel
               {
                   StudentId = 2,
                   FullName = "Abhi Ashwinbhai Dudhagara",
                   FatherName = "Ashwinbhai Dudhagara",
                   AadharCardNumber = "879865450987",
                   Address = "Sundaram Park, Nr. Bhaktinagar Road",
                   BloodGroup = BloodGroup.B_Positive,
                   Category = Category.OPEN,
                   City = "Rajkot",
                   ClassId =3,
                   State = "Gujarat",
                   EnrollmentNumber = "24CSBCA0001",
                   MobileNo = "9823484848",
                   ParentMobileNo = "9925323126",
                   Email = "adudhagara353@rku.ac.in",
                   DateOfBirth = new DateOnly(2004, 4, 25),
                   Country = "India",
                   MotherName = "Pramilaben Jerambhai Gorfad",
                   PinCode = "360002",
                   Gender = Gender.Male
               }
            );
        }
    }
}
