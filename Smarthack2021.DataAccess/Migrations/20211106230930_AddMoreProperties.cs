using Microsoft.EntityFrameworkCore.Migrations;

namespace Smarthack2021.DataAccess.Migrations
{
    public partial class AddMoreProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CryptographicalKeyObject",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "CryptographicalKeyObject");
        }
    }
}
