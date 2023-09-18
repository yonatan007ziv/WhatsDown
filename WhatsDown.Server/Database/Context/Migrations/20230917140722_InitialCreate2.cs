using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsDown.Server.Database.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userInformationUserId",
                table: "AccountCredentials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccountCredentials_userInformationUserId",
                table: "AccountCredentials",
                column: "userInformationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCredentials_UserInformation_userInformationUserId",
                table: "AccountCredentials",
                column: "userInformationUserId",
                principalTable: "UserInformation",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCredentials_UserInformation_userInformationUserId",
                table: "AccountCredentials");

            migrationBuilder.DropIndex(
                name: "IX_AccountCredentials_userInformationUserId",
                table: "AccountCredentials");

            migrationBuilder.DropColumn(
                name: "userInformationUserId",
                table: "AccountCredentials");
        }
    }
}
