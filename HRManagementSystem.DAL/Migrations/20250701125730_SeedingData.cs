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
                table: "Department",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "IT" },
                    { 2, "HR" }
            });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] {
                    "Id", "FullName", "Nationality", "Salary", "NationalId", "Gender",
                    "Address", "DateOfBirth", "ContractDate", "StartTime", "EndTime",
                    "DepartmentId", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
                    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
                    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled",
                    "AccessFailedCount"
                },
                values: new object[,]
                {
                    {
                        "b2d5f000-165f-4c97-9eba-0c8779320d47",
                        "Admin User",
                        "Egyptian",
                        15000m,
                        "29810000000000",
                        "Male",
                        "123 Main St, Cairo",
                        new DateTime(1990, 1, 1),
                        new DateTime(2020, 1, 1),
                        new DateTime(1, 1, 1, 8, 0, 0),
                        new DateTime(1, 1, 1, 16, 0, 0),
                        2,
                        "admin@example.com",
                        "ADMIN@EXAMPLE.COM",
                        "admin@example.com",
                        "ADMIN@EXAMPLE.COM",
                        false,
                        "AQAAAAIAAYagAAAAEHy6rJ5zC8pPj3JZx5mZ7XhY9W7vV2lKjXhY7W8z5qKt1vY2bN3cXpLkY9wZ4g==",
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        "01012345678",
                        false,
                        false,
                        true,
                        0
                    }
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "b2d5f000-165f-4c97-9eba-0c8779320d47", AddRolesSeed.adminRoleId } // Admin -> HR
                }
            );

            migrationBuilder.InsertData(
                table: "Attendance",
                columns: new[] { "Id", "ArrivalTime", "Date", "DepartureTime", "EmployeeId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 16, 30, 0, 0, DateTimeKind.Unspecified), "b2d5f000-165f-4c97-9eba-0c8779320d47" },
                    { 2, new DateTime(2025, 7, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), "b2d5f000-165f-4c97-9eba-0c8779320d47" }
                }
            );
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
