using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class addHolidayTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolidayTbl",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayTbl", x => x.HolidayId);
                });

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 1,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 25, 18, 45, 48, 51, DateTimeKind.Local).AddTicks(3078));

            migrationBuilder.UpdateData(
                table: "AttendanceTbl",
                keyColumn: "AttendanceId",
                keyValue: 2,
                column: "AttendanceDate",
                value: new DateTime(2025, 4, 25, 18, 45, 48, 51, DateTimeKind.Local).AddTicks(3096));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayTbl");

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
    }
}
