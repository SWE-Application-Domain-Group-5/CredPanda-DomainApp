using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EliApp.Migrations
{
    public partial class AddeduserIDtoEntryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "EntryModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "EntryModel");
        }
    }
}
