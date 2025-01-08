using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class addpkwebrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_tblWebRequest",
                table: "tblWebRequest",
                column: "WebID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblWebRequest",
                table: "tblWebRequest");
        }
    }
}
