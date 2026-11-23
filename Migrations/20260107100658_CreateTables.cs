using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELMS.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEmployees",
                columns: table => new
                {
                    inEmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stDepartment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    inManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployees", x => x.inEmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "tblLeaveRequests",
                columns: table => new
                {
                    itLeaveRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    itEmployeeId = table.Column<int>(type: "int", nullable: false),
                    dtStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dtEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    stReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dtAppliedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    inManagerId = table.Column<int>(type: "int", nullable: true),
                    dtDecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLeaveRequests", x => x.itLeaveRequestId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEmployees");

            migrationBuilder.DropTable(
                name: "tblLeaveRequests");
        }
    }
}
