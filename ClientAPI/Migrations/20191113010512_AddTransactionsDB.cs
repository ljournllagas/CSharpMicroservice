using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientAPI.Migrations
{
    public partial class AddTransactionsDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    BranchCode = table.Column<string>(nullable: true),
                    ClientCode = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    TransactionType = table.Column<string>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
