using Microsoft.EntityFrameworkCore.Migrations;

namespace BankAPI.Migrations
{
    public partial class AddBranchCodeToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BranchId",
                table: "Accounts",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Branches_BranchId",
                table: "Accounts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Branches_BranchId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BranchId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Accounts");
        }
    }
}
