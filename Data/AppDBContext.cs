﻿using Attendance.Models;
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
        public DbSet<ScheduleModel> ScheduleTbl { get; set; }
        public DbSet<AttendanceModel> AttendanceTbl { get; set; }
        public DbSet<LectureStatusModel> LectureStatusTbl { get; set; }
        public DbSet<HolidayModel> HolidayTbl { get; set; }
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
                    Email = "adudhagara353@rku.ac.in",
                    Password = "Abhi@123",
                    Role = UserRole.Faculty,
                    MobileNo = "7383835015"
                },
                new UserModel {
                    UserId = 3,
                    Fullname = "Jay Gorfad",
                    Email = "jgorfad223@rku.ac.in",
                    Password = "Jay@1234",
                    Role = UserRole.Faculty,
                    MobileNo = "9925323126"
                }
            );
            modelBuilder.Entity<DepartmentModel>().HasData(
                new DepartmentModel {
                    DepartmentId = 1,
                    DepartmentName = "Computer Science",
                    DepartmentShortName="CS"             
                },
                new DepartmentModel {
                    DepartmentId = 2,
                    DepartmentName = "Information Technology",
                    DepartmentShortName="IT"
                },
                new DepartmentModel {
                    DepartmentId = 3,
                    DepartmentName = "Diploma Studies",
                    DepartmentShortName = "SDS"
                }
            );
            modelBuilder.Entity<CourseModel>().HasData(
                new CourseModel
                {
                    CourseId = 1,
                    CourseName = "Master of Computer Application",
                    CourseShortName="MCA",
                    DepartmentId = 1
                }, new CourseModel
                {
                    CourseId = 2,
                    CourseName = "Bachelor of Computer Applicatiion",
                    CourseShortName="BCA",
                    DepartmentId = 1
                }, new CourseModel
                {
                    CourseId = 3,
                    CourseName = "Bachelor of TECHnology",
                    CourseShortName="B.Tech",
                    DepartmentId = 1
                }, new CourseModel
                {
                    CourseId = 4,
                    CourseName = "Bachelor of TECHnology",
                    CourseShortName="B.Tech",
                    DepartmentId = 2
                }
            );
            modelBuilder.Entity<BatchModel>().HasData(
               new BatchModel
               {
                   BatchId = 1,
                   CourseId = 1,
                   Semester = 2,
                   NumberOfClasses = 1,
                   Year = 2024,
                   StartDate = new DateOnly(2025, 01, 07),
                   EndDate = new DateOnly(2025, 05, 31)
               }, new BatchModel
               {
                   BatchId = 2,
                   CourseId = 2,
                   Semester = 6,
                   NumberOfClasses = 2,
                   Year = 2022,
                   StartDate = new DateOnly(2025, 01, 16),
                   EndDate = new DateOnly(2025, 06, 05)
               }
            );
            modelBuilder.Entity<SubjectModel>().HasData(
               new SubjectModel
               {
                   SubjectId = 1,
                   SubjectName = "Database Management System",
                   SubjectShortName="DBMS"
               },new SubjectModel
               {
                   SubjectId = 2,
                   SubjectName = "Data Structures Algorithms",
                   SubjectShortName="DSA"
               },new SubjectModel
               {
                   SubjectId = 3,
                   SubjectName = "Programing with Java",
                   SubjectShortName="JAVA"
               }
            );
            modelBuilder.Entity<ClassModel>().HasData(
               new ClassModel
               {
                   ClassId = 1,
                   BatchId=1,
                   ClassName = "A"
               }, new ClassModel
               {
                   ClassId = 2,
                   BatchId = 2,
                   ClassName = "A"
               }, new ClassModel
               {
                   ClassId = 3,
                   BatchId = 2,
                   ClassName = "B"
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
                   BloodGroup = BloodGroups.B_Positive,
                   Category = Categories.OPEN,
                   City = "Ahmedabad",
                   ClassId = 2,
                   State = "Gujarat",
                   EnrollmentNumber = "24CSMCA0001",
                   MobileNo = "9876543210",
                   ParentMobileNo = "9825034567",
                   Email = "dpatel32@rku.ac.in",
                   DateOfBirth = new DateOnly(2003, 8, 21),
                   Country = "India",
                   MotherName = "Bhavnaben Hiteshbhai Patel",
                   PinCode = "380015",
                   Gender = Genders.Male
               }, new StudentModel
               {
                   StudentId = 2,
                   FullName = "Harsh Jitubhai Savani",
                   FatherName = "Jitubhai Savani",
                   AadharCardNumber = "854796321045",
                   Address = "150 Feet Ring Road, Nr. Indira Circle",
                   BloodGroup = BloodGroups.O_Positive,
                   Category = Categories.OPEN,
                   City = "Rajkot",
                   ClassId = 3,
                   State = "Gujarat",
                   EnrollmentNumber = "24CSBCA0001",
                   MobileNo = "7984563210",
                   ParentMobileNo = "9825098745",
                   Email = "hsavani456@rku.ac.in",
                   DateOfBirth = new DateOnly(2002, 11, 12),
                   Country = "India",
                   MotherName = "Meenaben Jitubhai Savani",
                   PinCode = "360005",
                   Gender = Genders.Male
               }
            );
            modelBuilder.Entity<ScheduleModel>().HasData(
                new ScheduleModel
                {
                    ScheduleId = 1,
                    SubjectId = 1,
                    FacultyId = 2,
                    ClassId = 1,
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(9, 45, 0),
                    Day = DaysOfWeek.Monday
                },
                new ScheduleModel
                {
                    ScheduleId = 2,
                    SubjectId = 2,
                    FacultyId = 3,
                    ClassId = 1,
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(11, 40, 0),
                    Day = DaysOfWeek.Monday
                }
            );
            modelBuilder.Entity<AttendanceModel>()
            .HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete

            modelBuilder.Entity<AttendanceModel>()
                .HasOne(a => a.Schedule)
                .WithMany()
                .HasForeignKey(a => a.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AttendanceModel>().HasData(
               new AttendanceModel { AttendanceId = 1, ScheduleId = 1, StudentId = 1, Status = AttendanceStatus.Present },
               new AttendanceModel { AttendanceId = 2, ScheduleId = 2, StudentId = 1, Status = AttendanceStatus.Absent }
            );
            
        }
    }
}
