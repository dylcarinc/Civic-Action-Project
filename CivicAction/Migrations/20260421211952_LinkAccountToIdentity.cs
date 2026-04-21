using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicAction.Migrations
{
    /// <inheritdoc />
    public partial class LinkAccountToIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Account",
                newName: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_IdentityUserId",
                table: "Account",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AspNetUsers_IdentityUserId",
                table: "Account",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AspNetUsers_IdentityUserId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_IdentityUserId",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Account",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Account",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
