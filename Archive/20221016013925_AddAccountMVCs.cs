using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EliApp.Migrations
{
    public partial class AddAccountMVCs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    AccountDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountSubcategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountInitialBalance = table.Column<float>(type: "real", nullable: false),
                    AccountCurrentBalance = table.Column<float>(type: "real", nullable: false),
                    AccountCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountUserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountOrder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountStatement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountComment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountModel");
        }
    }
}
