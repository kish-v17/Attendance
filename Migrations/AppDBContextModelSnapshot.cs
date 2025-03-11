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

            modelBuilder.Entity("Attendance.Models.CourseModel", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.HasKey("CourseId");

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

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DepartmentId");

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
                });

            modelBuilder.Entity("Attendance.Models.UserModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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
                            CreatedAt = new DateTime(2025, 3, 11, 5, 41, 45, 613, DateTimeKind.Utc).AddTicks(7383),
                            Email = "kish.v07@gmail.com",
                            Fullname = "Kishan Patel",
                            Password = "Admin",
                            Role = 0
                        });
                });

            modelBuilder.Entity("Attendance.Models.CourseModel", b =>
                {
                    b.HasOne("Attendance.Models.DepartmentModel", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });
#pragma warning restore 612, 618
        }
    }
}
