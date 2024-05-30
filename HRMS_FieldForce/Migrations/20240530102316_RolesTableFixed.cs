using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_FieldForce.Migrations
{
    /// <inheritdoc />
    public partial class RolesTableFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBasicDetail_Users_UserId",
                table: "UserBasicDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBasicDetail",
                table: "UserBasicDetail");

            migrationBuilder.RenameTable(
                name: "UserBasicDetail",
                newName: "UserBasicDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBasicDetails",
                table: "UserBasicDetails",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBasicDetails_Users_UserId",
                table: "UserBasicDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBasicDetails_Users_UserId",
                table: "UserBasicDetails");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBasicDetails",
                table: "UserBasicDetails");

            migrationBuilder.RenameTable(
                name: "UserBasicDetails",
                newName: "UserBasicDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBasicDetail",
                table: "UserBasicDetail",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBasicDetail_Users_UserId",
                table: "UserBasicDetail",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
