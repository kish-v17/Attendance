using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class updateLectureTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "LectureStatusTbl",
                newName: "ScheduleDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "FillDate",
                table: "LectureStatusTbl",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "LectureStatusTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 1,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 25, 17, 47, 23, 604, DateTimeKind.Local).AddTicks(8750));

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 2,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 25, 17, 47, 23, 604, DateTimeKind.Local).AddTicks(8779));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FillDate",
                table: "LectureStatusTbl");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "LectureStatusTbl");

            migrationBuilder.RenameColumn(
                name: "ScheduleDate",
                table: "LectureStatusTbl",
                newName: "Date");

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
        }
    }
}
