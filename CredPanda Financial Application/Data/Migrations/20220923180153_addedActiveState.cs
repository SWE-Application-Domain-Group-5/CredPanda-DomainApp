using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CredPanda_Financial_Application.Data.Migrations
{
    public partial class addedActiveState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "User");
        }
    }
}
