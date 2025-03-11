using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
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

            migrationBuilder.UpdateData(
                table: "UserTbl",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 3, 11, 5, 41, 45, 613, DateTimeKind.Utc).AddTicks(7383), "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentTbl");

            migrationBuilder.UpdateData(
                table: "UserTbl",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 3, 9, 13, 37, 24, 341, DateTimeKind.Utc).AddTicks(8167), "AQAAAAIAAYagAAAAEO7YO1kEnkqL4XQOnhxnImcoKs0MrECptX1xG71taFCX5seVQQJPHKGypTqRv8gwfw==" });
        }
    }
}
