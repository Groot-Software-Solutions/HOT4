using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class changemodelname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblRecharge_tblPin_TblPinPinId",
                table: "tblRecharge");

            migrationBuilder.DropForeignKey(
                name: "FK_tblRecharge_tblSMS_TblSmsSmsid",
                table: "tblRecharge");

            migrationBuilder.RenameColumn(
                name: "TblSmsSmsid",
                table: "tblRecharge",
                newName: "Smsid");

            migrationBuilder.RenameColumn(
                name: "TblPinPinId",
                table: "tblRecharge",
                newName: "PinsPinId");

            migrationBuilder.RenameIndex(
                name: "IX_tblRecharge_TblSmsSmsid",
                table: "tblRecharge",
                newName: "IX_tblRecharge_Smsid");

            migrationBuilder.RenameIndex(
                name: "IX_tblRecharge_TblPinPinId",
                table: "tblRecharge",
                newName: "IX_tblRecharge_PinsPinId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblRecharge_tblPin_PinsPinId",
                table: "tblRecharge",
                column: "PinsPinId",
                principalTable: "tblPin",
                principalColumn: "PinID");

            migrationBuilder.AddForeignKey(
                name: "FK_tblRecharge_tblSMS_Smsid",
                table: "tblRecharge",
                column: "Smsid",
                principalTable: "tblSMS",
                principalColumn: "SMSID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblRecharge_tblPin_PinsPinId",
                table: "tblRecharge");

            migrationBuilder.DropForeignKey(
                name: "FK_tblRecharge_tblSMS_Smsid",
                table: "tblRecharge");

            migrationBuilder.RenameColumn(
                name: "Smsid",
                table: "tblRecharge",
                newName: "TblSmsSmsid");

            migrationBuilder.RenameColumn(
                name: "PinsPinId",
                table: "tblRecharge",
                newName: "TblPinPinId");

            migrationBuilder.RenameIndex(
                name: "IX_tblRecharge_Smsid",
                table: "tblRecharge",
                newName: "IX_tblRecharge_TblSmsSmsid");

            migrationBuilder.RenameIndex(
                name: "IX_tblRecharge_PinsPinId",
                table: "tblRecharge",
                newName: "IX_tblRecharge_TblPinPinId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblRecharge_tblPin_TblPinPinId",
                table: "tblRecharge",
                column: "TblPinPinId",
                principalTable: "tblPin",
                principalColumn: "PinID");

            migrationBuilder.AddForeignKey(
                name: "FK_tblRecharge_tblSMS_TblSmsSmsid",
                table: "tblRecharge",
                column: "TblSmsSmsid",
                principalTable: "tblSMS",
                principalColumn: "SMSID");
        }
    }
}
