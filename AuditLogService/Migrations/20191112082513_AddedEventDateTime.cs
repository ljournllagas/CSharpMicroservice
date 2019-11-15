using Microsoft.EntityFrameworkCore.Migrations;

namespace AuditLogService.Migrations
{
    public partial class AddedEventDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventDateTime",
                table: "AuditLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDateTime",
                table: "AuditLog");
        }
    }
}
