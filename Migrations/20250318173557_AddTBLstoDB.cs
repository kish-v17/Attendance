using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class AddTBLstoDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectTbl",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTbl", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "UserTbl",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTbl", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BatchTbl",
                columns: table => new
                {
                    BatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    NumberOfClasses = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClassModelClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchTbl", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "ClassModel",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentModelStudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassModel", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_ClassModel_BatchTbl_BatchId",
                        column: x => x.BatchId,
                        principalTable: "BatchTbl",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentTbl",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnrollmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MotherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    AadharCardNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ParentMobileNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PinCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTbl", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_StudentTbl_ClassModel_ClassId",
                        column: x => x.ClassId,
                        principalTable: "ClassModel",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTbl",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    BatchModelBatchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTbl", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_CourseTbl_BatchTbl_BatchModelBatchId",
                        column: x => x.BatchModelBatchId,
                        principalTable: "BatchTbl",
                        principalColumn: "BatchId");
                });

            migrationBuilder.CreateTable(
                name: "DepartmentTbl",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CourseModelCourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTbl", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_DepartmentTbl_CourseTbl_CourseModelCourseId",
                        column: x => x.CourseModelCourseId,
                        principalTable: "CourseTbl",
                        principalColumn: "CourseId");
                });

            migrationBuilder.InsertData(
                table: "DepartmentTbl",
                columns: new[] { "DepartmentId", "CourseModelCourseId", "DepartmentName" },
                values: new object[,]
                {
                    { 1, null, "Computer Science" },
                    { 2, null, "Information Technology" },
                    { 3, null, "Diploma Studies" },
                    { 4, null, "Management" }
                });

            migrationBuilder.InsertData(
                table: "SubjectTbl",
                columns: new[] { "SubjectId", "SubjectName" },
                values: new object[,]
                {
                    { 1, "Database Management System" },
                    { 2, "Data Structures" },
                    { 3, "Programing With Java" }
                });

            migrationBuilder.InsertData(
                table: "UserTbl",
                columns: new[] { "UserId", "Email", "Fullname", "MobileNo", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "kish.v07@gmail.com", "Kishan Patel", "9925323126", "Admin@123", 0 },
                    { 2, "busyman2561@gmail.com", "Abhi Patel", "7383835015", "Abhi@123", 1 }
                });

            migrationBuilder.InsertData(
                table: "CourseTbl",
                columns: new[] { "CourseId", "BatchModelBatchId", "CourseName", "DepartmentId" },
                values: new object[,]
                {
                    { 1, null, "Master of Computer Application", 1 },
                    { 2, null, "Bachelor of Computer Applicatiion", 1 },
                    { 3, null, "Bachelor of Technology", 1 },
                    { 4, null, "Bachelor of Technology", 2 }
                });

            migrationBuilder.InsertData(
                table: "BatchTbl",
                columns: new[] { "BatchId", "ClassModelClassId", "CourseId", "EndDate", "NumberOfClasses", "Semester", "StartDate", "Year" },
                values: new object[,]
                {
                    { 1, null, 1, new DateOnly(2025, 5, 31), 0, 2, new DateOnly(2025, 1, 7), 2024 },
                    { 2, null, 2, new DateOnly(2025, 6, 5), 0, 6, new DateOnly(2025, 1, 16), 2022 }
                });

            migrationBuilder.InsertData(
                table: "ClassModel",
                columns: new[] { "ClassId", "BatchId", "Class", "StudentModelStudentId" },
                values: new object[,]
                {
                    { 1, 1, "A", null },
                    { 2, 1, "B", null },
                    { 3, 2, "A", null }
                });

            migrationBuilder.InsertData(
                table: "StudentTbl",
                columns: new[] { "StudentId", "AadharCardNumber", "Address", "BloodGroup", "Category", "City", "ClassId", "Country", "DateOfBirth", "Email", "EnrollmentNumber", "FatherName", "FullName", "Gender", "MobileNo", "MotherName", "ParentMobileNo", "PinCode", "State" },
                values: new object[,]
                {
                    { 1, "947598246540", "Gokul Park,Nr. Kothariya Chowkdi", 0, 1, "Rajkot", 1, "India", new DateOnly(2004, 5, 15), "jgorfad223@rku.ac.in", "2024CSṂCA00001", "Jerambhai Gorfad", "Jay Jerambhai Gorfad", 0, "7689468909", "Lilaben Jerambhai Gorfad", "9925323126", "360002", "Gujarat" },
                    { 2, "879865450987", "Sundaram Park, Nr. Bhaktinagar Road", 2, 0, "Rajkot", 3, "India", new DateOnly(2004, 4, 25), "adudhagara353@rku.ac.in", "24CSBCA0001", "Ashwinbhai Dudhagara", "Abhi Ashwinbhai Dudhagara", 0, "9823484848", "Pramilaben Jerambhai Gorfad", "9925323126", "360002", "Gujarat" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchTbl_ClassModelClassId",
                table: "BatchTbl",
                column: "ClassModelClassId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchTbl_CourseId",
                table: "BatchTbl",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassModel_BatchId",
                table: "ClassModel",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassModel_StudentModelStudentId",
                table: "ClassModel",
                column: "StudentModelStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTbl_BatchModelBatchId",
                table: "CourseTbl",
                column: "BatchModelBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTbl_DepartmentId",
                table: "CourseTbl",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTbl_CourseModelCourseId",
                table: "DepartmentTbl",
                column: "CourseModelCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTbl_ClassId",
                table: "StudentTbl",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTbl_EnrollmentNumber",
                table: "StudentTbl",
                column: "EnrollmentNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTbl_ClassModel_ClassModelClassId",
                table: "BatchTbl",
                column: "ClassModelClassId",
                principalTable: "ClassModel",
                principalColumn: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchTbl_CourseTbl_CourseId",
                table: "BatchTbl",
                column: "CourseId",
                principalTable: "CourseTbl",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassModel_StudentTbl_StudentModelStudentId",
                table: "ClassModel",
                column: "StudentModelStudentId",
                principalTable: "StudentTbl",
                principalColumn: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTbl_DepartmentTbl_DepartmentId",
                table: "CourseTbl",
                column: "DepartmentId",
                principalTable: "DepartmentTbl",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchTbl_ClassModel_ClassModelClassId",
                table: "BatchTbl");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTbl_ClassModel_ClassId",
                table: "StudentTbl");

            migrationBuilder.DropForeignKey(
                name: "FK_BatchTbl_CourseTbl_CourseId",
                table: "BatchTbl");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentTbl_CourseTbl_CourseModelCourseId",
                table: "DepartmentTbl");

            migrationBuilder.DropTable(
                name: "SubjectTbl");

            migrationBuilder.DropTable(
                name: "UserTbl");

            migrationBuilder.DropTable(
                name: "ClassModel");

            migrationBuilder.DropTable(
                name: "StudentTbl");

            migrationBuilder.DropTable(
                name: "CourseTbl");

            migrationBuilder.DropTable(
                name: "BatchTbl");

            migrationBuilder.DropTable(
                name: "DepartmentTbl");
        }
    }
}
