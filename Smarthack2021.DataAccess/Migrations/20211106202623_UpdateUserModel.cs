using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Smarthack2021.DataAccess.Migrations
{
    public partial class UpdateUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptographicalKeyObject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncryptedPrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncryptedPublicKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptographicalKeyObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptographicalKeyObject_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordObject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncryptedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordObject_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryptographicalKeyObject_UserId",
                table: "CryptographicalKeyObject",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordObject_UserId",
                table: "PasswordObject",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptographicalKeyObject");

            migrationBuilder.DropTable(
                name: "PasswordObject");
        }
    }
}
