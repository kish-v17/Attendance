using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Migrations
{
    /// <inheritdoc />
    public partial class AddtableTOdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "UserTbl",
                columns: new[] { "UserId", "CreatedAt", "Email", "Fullname", "Password", "Role" },
                values: new object[] { 1, new DateTime(2025, 3, 9, 13, 37, 24, 341, DateTimeKind.Utc).AddTicks(8167), "kish.v07@gmail.com", "Kishan Patel", "AQAAAAIAAYagAAAAEO7YO1kEnkqL4XQOnhxnImcoKs0MrECptX1xG71taFCX5seVQQJPHKGypTqRv8gwfw==", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTbl");
        }
    }
}
