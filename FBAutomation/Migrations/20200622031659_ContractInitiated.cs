using Microsoft.EntityFrameworkCore.Migrations;

namespace FBAutomation.Migrations
{
    public partial class ContractInitiated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ContactInitiated",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactInitiated",
                table: "Users");
        }
    }
}
