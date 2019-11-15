using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientAPI.Migrations
{
    public partial class AddAccountNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Accounts");
        }
    }
}
