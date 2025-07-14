using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "Deduction", "FirstHoliday", "OverTime", "SecondHoliday", "Type" },
                values: new object[] { 1, 25.00m, 6, 50.00m, 7, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
