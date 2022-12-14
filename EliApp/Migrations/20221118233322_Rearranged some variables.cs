using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EliApp.Migrations
{
    public partial class Rearrangedsomevariables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ledgers_Capacity",
                table: "AccountModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ledgers_Capacity",
                table: "AccountModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
