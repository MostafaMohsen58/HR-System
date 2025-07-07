using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class testMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b2d5f001-165f-4c97-9eba-0c8779320d47");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "ContractDate", "DateOfBirth", "DepartmentId", "Email", "EmailConfirmed", "EndTime", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NationalId", "Nationality", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Salary", "SecurityStamp", "StartTime", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b2d5f001-165f-4c97-9eba-0c8779320d47", 0, "456 Side St, Cairo", "ConcurrencyStamp2", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "user@example.com", true, new DateTime(1, 1, 1, 17, 0, 0, 0, DateTimeKind.Unspecified), "Regular User", "Female", true, null, "29810000000001", "Egyptian", "USER@EXAMPLE.COM", "USER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEHy6rJ5zC8pPj3JZx5mZ7XhY9W7vV2lKjXhY7W8z5qKt1vY2bN3cXpLkY9wZ4g==", "01087654321", true, 8000m, "SecurityStamp2", new DateTime(1, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "user@example.com" });
        }
    }
}
