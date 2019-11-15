using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientAPI.Migrations
{
    public partial class AddRemainingBalanceToTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RemainingBalance",
                table: "Transactions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "Transactions");
        }
    }
}
