using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_FieldForce.Migrations
{
    /// <inheritdoc />
    public partial class RoleNavigaitonPropertyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleAssignedRoleID",
                table: "Users",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleAssignedRoleID",
                table: "Users",
                column: "RoleAssignedRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleAssignedRoleID",
                table: "Users",
                column: "RoleAssignedRoleID",
                principalTable: "Roles",
                principalColumn: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleAssignedRoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleAssignedRoleID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleAssignedRoleID",
                table: "Users");
        }
    }
}
