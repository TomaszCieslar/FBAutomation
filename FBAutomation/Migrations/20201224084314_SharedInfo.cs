using Microsoft.EntityFrameworkCore.Migrations;

namespace FBAutomation.Migrations
{
    public partial class SharedInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SharedInfo",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SharedInfo",
                table: "Users");
        }
    }
}
