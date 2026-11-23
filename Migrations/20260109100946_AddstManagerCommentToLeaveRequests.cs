using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELMS.Migrations
{
    /// <inheritdoc />
    public partial class AddstManagerCommentToLeaveRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "stManagerComment",
                table: "tblLeaveRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stManagerComment",
                table: "tblLeaveRequests");
        }
    }
}
