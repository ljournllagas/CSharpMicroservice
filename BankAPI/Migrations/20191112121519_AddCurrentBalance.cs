using Microsoft.EntityFrameworkCore.Migrations;

namespace BankAPI.Migrations
{
    public partial class AddCurrentBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentBalance",
                table: "Accounts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentBalance",
                table: "Accounts");
        }
    }
}
