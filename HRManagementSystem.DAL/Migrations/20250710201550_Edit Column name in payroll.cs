using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditColumnnameinpayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeductedDays",
                table: "PayRoll",
                newName: "PresentDays");

            migrationBuilder.RenameColumn(
                name: "DaysPresent",
                table: "PayRoll",
                newName: "DeductionInHours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PresentDays",
                table: "PayRoll",
                newName: "DeductedDays");

            migrationBuilder.RenameColumn(
                name: "DeductionInHours",
                table: "PayRoll",
                newName: "DaysPresent");
        }
    }
}
