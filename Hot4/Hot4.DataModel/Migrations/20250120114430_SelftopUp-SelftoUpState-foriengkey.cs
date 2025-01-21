using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class SelftopUpSelftoUpStateforiengkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tblSelfTopUp_StateId",
                table: "tblSelfTopUp",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblSelfTopUp_tblSelfTopUpState",
                table: "tblSelfTopUp",
                column: "StateId",
                principalTable: "tblSelfTopUpState",
                principalColumn: "SelfTopUpStateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblSelfTopUp_tblSelfTopUpState",
                table: "tblSelfTopUp");

            migrationBuilder.DropIndex(
                name: "IX_tblSelfTopUp_StateId",
                table: "tblSelfTopUp");
        }
    }
}
