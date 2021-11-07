using Microsoft.EntityFrameworkCore.Migrations;

namespace Smarthack2021.DataAccess.Migrations
{
    public partial class AddManyToManyRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CryptographicalKeyObject_Users_UserId",
                table: "CryptographicalKeyObject");

            migrationBuilder.DropForeignKey(
                name: "FK_PasswordObject_Users_UserId",
                table: "PasswordObject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PasswordObject",
                table: "PasswordObject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CryptographicalKeyObject",
                table: "CryptographicalKeyObject");

            migrationBuilder.RenameTable(
                name: "PasswordObject",
                newName: "Passwords");

            migrationBuilder.RenameTable(
                name: "CryptographicalKeyObject",
                newName: "Keys");

            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "Passwords",
                newName: "Tags");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordObject_UserId",
                table: "Passwords",
                newName: "IX_Passwords_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CryptographicalKeyObject_UserId",
                table: "Keys",
                newName: "IX_Keys_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passwords",
                table: "Passwords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Keys",
                table: "Keys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Keys_Users_UserId",
                table: "Keys",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_Users_UserId",
                table: "Passwords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keys_Users_UserId",
                table: "Keys");

            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_Users_UserId",
                table: "Passwords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passwords",
                table: "Passwords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Keys",
                table: "Keys");

            migrationBuilder.RenameTable(
                name: "Passwords",
                newName: "PasswordObject");

            migrationBuilder.RenameTable(
                name: "Keys",
                newName: "CryptographicalKeyObject");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "PasswordObject",
                newName: "Tag");

            migrationBuilder.RenameIndex(
                name: "IX_Passwords_UserId",
                table: "PasswordObject",
                newName: "IX_PasswordObject_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Keys_UserId",
                table: "CryptographicalKeyObject",
                newName: "IX_CryptographicalKeyObject_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PasswordObject",
                table: "PasswordObject",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CryptographicalKeyObject",
                table: "CryptographicalKeyObject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CryptographicalKeyObject_Users_UserId",
                table: "CryptographicalKeyObject",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordObject_Users_UserId",
                table: "PasswordObject",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
