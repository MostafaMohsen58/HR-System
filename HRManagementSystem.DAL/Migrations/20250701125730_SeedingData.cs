using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Attendance",
                columns: new[] { "Id", "ArrivalTime", "Date", "DepartureTime", "EmployeeId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), "b2d5f000-165f-4c97-9eba-0c8779320d47" },
                    { 2, new DateTime(2025, 7, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), "b2d5f000-165f-4c97-9eba-0c8779320d47" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Attendance",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Attendance",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
