using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleSeed : Migration
    {
        /// <inheritdoc />
        private readonly string _adminRoleId = Guid.NewGuid().ToString();
        private readonly string _userRoleId = Guid.NewGuid().ToString();
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[]
                {
            _adminRoleId,
            Guid.NewGuid().ToString(),
            "Hr",
            "HR"
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[]
                {
            _userRoleId,
            Guid.NewGuid().ToString(),
            "User",
            "USER"
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
           table: "AspNetRoles",
           keyColumn: "Id",
           keyValues: new object[] { _adminRoleId }
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValues: new object[] { _userRoleId }
            );
        }
    }
}
