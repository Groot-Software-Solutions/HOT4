using Microsoft.EntityFrameworkCore.Migrations;

namespace BillPayments.Infrastructure.Data.Migrations
{
    public partial class AddBackgroundResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastResponse",
                table: "BackgroundTasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastResponse",
                table: "BackgroundTasks");
        }
    }
}
