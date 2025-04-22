using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class addLectureStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DepartmentTbl",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.CreateTable(
                name: "LectureStatusTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureStatusTbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureStatusTbl_ScheduleTbl_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "ScheduleTbl",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 1,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 22, 18, 37, 33, 253, DateTimeKind.Local).AddTicks(7342));

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 2,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 22, 18, 37, 33, 253, DateTimeKind.Local).AddTicks(7357));

            migrationBuilder.CreateIndex(
                name: "IX_LectureStatusTbl_ScheduleId",
                table: "LectureStatusTbl",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LectureStatusTbl");

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 1,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 13, 21, 31, 6, 558, DateTimeKind.Local).AddTicks(1945));

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 2,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 13, 21, 31, 6, 558, DateTimeKind.Local).AddTicks(1962));

            migrationBuilder.InsertData(
                table: "DepartmentTbl",
                columns: new[] { "DepartmentId", "CourseModelCourseId", "DepartmentName", "DepartmentShortName" },
                values: new object[] { 4, null, "Management", "SOM" });
        }
    }
}
