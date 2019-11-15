using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientAPI.Migrations
{
    public partial class AddIsValid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Accounts");
        }
    }
}
