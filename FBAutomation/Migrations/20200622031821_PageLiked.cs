using Microsoft.EntityFrameworkCore.Migrations;

namespace FBAutomation.Migrations
{
    public partial class PageLiked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PageLiked",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageLiked",
                table: "Users");
        }
    }
}
