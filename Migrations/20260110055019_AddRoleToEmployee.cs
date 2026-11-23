using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELMS.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "stRole",
                table: "tblEmployees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stRole",
                table: "tblEmployees");
        }
    }
}
