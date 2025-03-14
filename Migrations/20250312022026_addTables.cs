﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentTbl",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTbl", x => x.DepartmentId);
                });

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
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTbl", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CourseTbl",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTbl", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_CourseTbl_DepartmentTbl_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentTbl",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DepartmentTbl",
                columns: new[] { "DepartmentId", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "Computer Science" },
                    { 2, "Information Technology" },
                    { 3, "Diploma Studies" },
                    { 4, "Management" }
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
                columns: new[] { "UserId", "CreatedAt", "Email", "Fullname", "Password", "Role" },
                values: new object[] { 1, new DateTime(2025, 3, 12, 2, 20, 22, 589, DateTimeKind.Utc).AddTicks(603), "kish.v07@gmail.com", "Kishan Patel", "Admin", 0 });

            migrationBuilder.InsertData(
                table: "CourseTbl",
                columns: new[] { "CourseId", "CourseName", "DepartmentId" },
                values: new object[,]
                {
                    { 1, "Master of Computer Application", 1 },
                    { 2, "Bachelor of Computer Applicatiion", 1 },
                    { 3, "Bachelor of Technology", 1 },
                    { 4, "Bachelor of Technology", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTbl_DepartmentId",
                table: "CourseTbl",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTbl");

            migrationBuilder.DropTable(
                name: "SubjectTbl");

            migrationBuilder.DropTable(
                name: "UserTbl");

            migrationBuilder.DropTable(
                name: "DepartmentTbl");
        }
    }
}
