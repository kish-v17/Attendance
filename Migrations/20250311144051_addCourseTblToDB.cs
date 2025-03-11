using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class addCourseTblToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "UserTbl",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 11, 14, 40, 48, 69, DateTimeKind.Utc).AddTicks(9940));

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

            migrationBuilder.DeleteData(
                table: "DepartmentTbl",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DepartmentTbl",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DepartmentTbl",
                keyColumn: "DepartmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DepartmentTbl",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "UserTbl",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 11, 5, 41, 45, 613, DateTimeKind.Utc).AddTicks(7383));
        }
    }
}
