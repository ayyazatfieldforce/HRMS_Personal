using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_FieldForce.Migrations
{
    /// <inheritdoc />
    public partial class userBasicDetailsTest10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
