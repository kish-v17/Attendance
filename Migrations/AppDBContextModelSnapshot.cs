﻿// <auto-generated />
using System;
using Attendance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Attendance.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Attendance.Models.BatchModel", b =>
                {
                    b.Property<int>("BatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BatchId"));

                    b.Property<int?>("ClassModelClassId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("NumberOfClasses")
                        .HasColumnType("int");

                    b.Property<int>("Semester")
                        .HasColumnType("int");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("BatchId");

                    b.HasIndex("ClassModelClassId");

                    b.HasIndex("CourseId");

                    b.ToTable("BatchTbl");

                    b.HasData(
                        new
                        {
                            BatchId = 1,
                            CourseId = 1,
                            EndDate = new DateOnly(2025, 5, 31),
                            NumberOfClasses = 0,
                            Semester = 2,
                            StartDate = new DateOnly(2025, 1, 7),
                            Year = 2024
                        },
                        new
                        {
                            BatchId = 2,
                            CourseId = 2,
                            EndDate = new DateOnly(2025, 6, 5),
                            NumberOfClasses = 0,
                            Semester = 6,
                            StartDate = new DateOnly(2025, 1, 16),
                            Year = 2022
                        });
                });

            modelBuilder.Entity("Attendance.Models.ClassModel", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"));

                    b.Property<int>("BatchId")
                        .HasColumnType("int");

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudentModelStudentId")
                        .HasColumnType("int");

                    b.HasKey("ClassId");

                    b.HasIndex("BatchId");

                    b.HasIndex("StudentModelStudentId");

                    b.ToTable("ClassTbl");

                    b.HasData(
                        new
                        {
                            ClassId = 1,
                            BatchId = 1,
                            Class = "A"
                        },
                        new
                        {
                            ClassId = 2,
                            BatchId = 1,
                            Class = "B"
                        },
                        new
                        {
                            ClassId = 3,
                            BatchId = 2,
                            Class = "A"
                        });
                });

            modelBuilder.Entity("Attendance.Models.CourseModel", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<int?>("BatchModelBatchId")
                        .HasColumnType("int");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.HasKey("CourseId");

                    b.HasIndex("BatchModelBatchId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("CourseTbl");

                    b.HasData(
                        new
                        {
                            CourseId = 1,
                            CourseName = "Master of Computer Application",
                            DepartmentId = 1
                        },
                        new
                        {
                            CourseId = 2,
                            CourseName = "Bachelor of Computer Applicatiion",
                            DepartmentId = 1
                        },
                        new
                        {
                            CourseId = 3,
                            CourseName = "Bachelor of Technology",
                            DepartmentId = 1
                        },
                        new
                        {
                            CourseId = 4,
                            CourseName = "Bachelor of Technology",
                            DepartmentId = 2
                        });
                });

            modelBuilder.Entity("Attendance.Models.DepartmentModel", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<int?>("CourseModelCourseId")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DepartmentId");

                    b.HasIndex("CourseModelCourseId");

                    b.ToTable("DepartmentTbl");

                    b.HasData(
                        new
                        {
                            DepartmentId = 1,
                            DepartmentName = "Computer Science"
                        },
                        new
                        {
                            DepartmentId = 2,
                            DepartmentName = "Information Technology"
                        },
                        new
                        {
                            DepartmentId = 3,
                            DepartmentName = "Diploma Studies"
                        },
                        new
                        {
                            DepartmentId = 4,
                            DepartmentName = "Management"
                        });
                });

            modelBuilder.Entity("Attendance.Models.StudentModel", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<string>("AadharCardNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("BloodGroup")
                        .HasColumnType("int");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EnrollmentNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("MobileNo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("MotherName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ParentMobileNo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StudentId");

                    b.HasIndex("ClassId");

                    b.HasIndex("EnrollmentNumber")
                        .IsUnique();

                    b.ToTable("StudentTbl");

                    b.HasData(
                        new
                        {
                            StudentId = 1,
                            AadharCardNumber = "789654123012",
                            Address = "Satellite Road, Nr. Shivalik Plaza",
                            BloodGroup = 2,
                            Category = 0,
                            City = "Ahmedabad",
                            ClassId = 2,
                            Country = "India",
                            DateOfBirth = new DateOnly(2003, 8, 21),
                            Email = "dpatel32@rku.ac.in",
                            EnrollmentNumber = "24CSMCA00001",
                            FatherName = "Hiteshbhai Patel",
                            FullName = "Dhruv Hiteshbhai Patel",
                            Gender = 0,
                            MobileNo = "9876543210",
                            MotherName = "Bhavnaben Hiteshbhai Patel",
                            ParentMobileNo = "9825034567",
                            PinCode = "380015",
                            State = "Gujarat"
                        },
                        new
                        {
                            StudentId = 2,
                            AadharCardNumber = "854796321045",
                            Address = "150 Feet Ring Road, Nr. Indira Circle",
                            BloodGroup = 6,
                            Category = 0,
                            City = "Rajkot",
                            ClassId = 3,
                            Country = "India",
                            DateOfBirth = new DateOnly(2002, 11, 12),
                            Email = "hsavani456@rku.ac.in",
                            EnrollmentNumber = "24CSBCA00001",
                            FatherName = "Jitubhai Savani",
                            FullName = "Harsh Jitubhai Savani",
                            Gender = 0,
                            MobileNo = "7984563210",
                            MotherName = "Meenaben Jitubhai Savani",
                            ParentMobileNo = "9825098745",
                            PinCode = "360005",
                            State = "Gujarat"
                        });
                });

            modelBuilder.Entity("Attendance.Models.SubjectModel", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectId"));

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SubjectId");

                    b.ToTable("SubjectTbl");

                    b.HasData(
                        new
                        {
                            SubjectId = 1,
                            SubjectName = "Database Management System"
                        },
                        new
                        {
                            SubjectId = 2,
                            SubjectName = "Data Structures"
                        },
                        new
                        {
                            SubjectId = 3,
                            SubjectName = "Programing With Java"
                        });
                });

            modelBuilder.Entity("Attendance.Models.UserModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MobileNo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("UserTbl");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Email = "kish.v07@gmail.com",
                            Fullname = "Kishan Patel",
                            MobileNo = "9925323126",
                            Password = "Admin@123",
                            Role = 0
                        },
                        new
                        {
                            UserId = 2,
                            Email = "busyman2561@gmail.com",
                            Fullname = "Abhi Patel",
                            MobileNo = "7383835015",
                            Password = "Abhi@123",
                            Role = 1
                        });
                });

            modelBuilder.Entity("Attendance.Models.BatchModel", b =>
                {
                    b.HasOne("Attendance.Models.ClassModel", null)
                        .WithMany("Batches")
                        .HasForeignKey("ClassModelClassId");

                    b.HasOne("Attendance.Models.CourseModel", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Attendance.Models.ClassModel", b =>
                {
                    b.HasOne("Attendance.Models.BatchModel", "Batch")
                        .WithMany()
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Attendance.Models.StudentModel", null)
                        .WithMany("ClassModels")
                        .HasForeignKey("StudentModelStudentId");

                    b.Navigation("Batch");
                });

            modelBuilder.Entity("Attendance.Models.CourseModel", b =>
                {
                    b.HasOne("Attendance.Models.BatchModel", null)
                        .WithMany("Courses")
                        .HasForeignKey("BatchModelBatchId");

                    b.HasOne("Attendance.Models.DepartmentModel", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Attendance.Models.DepartmentModel", b =>
                {
                    b.HasOne("Attendance.Models.CourseModel", null)
                        .WithMany("Departments")
                        .HasForeignKey("CourseModelCourseId");
                });

            modelBuilder.Entity("Attendance.Models.StudentModel", b =>
                {
                    b.HasOne("Attendance.Models.ClassModel", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("Attendance.Models.BatchModel", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Attendance.Models.ClassModel", b =>
                {
                    b.Navigation("Batches");
                });

            modelBuilder.Entity("Attendance.Models.CourseModel", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("Attendance.Models.StudentModel", b =>
                {
                    b.Navigation("ClassModels");
                });
#pragma warning restore 612, 618
        }
    }
}
