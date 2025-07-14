using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditSettingSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Deduction", "OverTime" },
                values: new object[] { 2.00m, 3.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Deduction", "OverTime" },
                values: new object[] { 25.00m, 50.00m });
        }
    }
}
