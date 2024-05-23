using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_FieldForce.Migrations
{
    /// <inheritdoc />
    public partial class userBasicDetailsTest5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBasicDetails_Users_UserId1",
                table: "UserBasicDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserBasicDetails_UserId1",
                table: "UserBasicDetails");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserBasicDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserBasicDetails",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserBasicDetails_UserId1",
                table: "UserBasicDetails",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBasicDetails_Users_UserId1",
                table: "UserBasicDetails",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
