using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_FieldForce.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveCategory",
                table: "UserLeaves");

            migrationBuilder.DropColumn(
                name: "LeaveType",
                table: "UserLeaves");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeaveCategory",
                table: "UserLeaves",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LeaveType",
                table: "UserLeaves",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
