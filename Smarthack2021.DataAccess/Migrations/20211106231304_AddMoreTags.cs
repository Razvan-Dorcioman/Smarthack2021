using Microsoft.EntityFrameworkCore.Migrations;

namespace Smarthack2021.DataAccess.Migrations
{
    public partial class AddMoreTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "PasswordObject",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "PasswordObject");
        }
    }
}
