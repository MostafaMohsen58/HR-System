using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ConvertDateTimeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "OfficialHoliday",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Attendance",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ContractDate",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Attendance",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(1, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Attendance",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(1, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 17, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "OfficialHoliday",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Attendance",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ContractDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "Attendance",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(2025, 7, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 16, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Attendance",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(2025, 7, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 2, 17, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
