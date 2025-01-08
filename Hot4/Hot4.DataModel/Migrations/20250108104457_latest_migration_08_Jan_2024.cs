using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class latest_migration_08_Jan_2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_tblRecharge_tblPin_PinsPinId",
            //    table: "tblRecharge");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_tblRecharge_tblSMS_Smsid",
            //    table: "tblRecharge");

            //migrationBuilder.DropIndex(
            //    name: "IX_tblRecharge_PinsPinId",
            //    table: "tblRecharge");

            //migrationBuilder.DropIndex(
            //    name: "IX_tblRecharge_Smsid",
            //    table: "tblRecharge");

            //migrationBuilder.DropColumn(
            //    name: "PinsPinId",
            //    table: "tblRecharge");

            //migrationBuilder.DropColumn(
            //    name: "Smsid",
            //    table: "tblRecharge");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<long>(
            //    name: "PinsPinId",
            //    table: "tblRecharge",
            //    type: "bigint",
            //    nullable: true);

            //migrationBuilder.AddColumn<long>(
            //    name: "Smsid",
            //    table: "tblRecharge",
            //    type: "bigint",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_tblRecharge_PinsPinId",
            //    table: "tblRecharge",
            //    column: "PinsPinId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_tblRecharge_Smsid",
            //    table: "tblRecharge",
            //    column: "Smsid");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tblRecharge_tblPin_PinsPinId",
            //    table: "tblRecharge",
            //    column: "PinsPinId",
            //    principalTable: "tblPin",
            //    principalColumn: "PinID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tblRecharge_tblSMS_Smsid",
            //    table: "tblRecharge",
            //    column: "Smsid",
            //    principalTable: "tblSMS",
            //    principalColumn: "SMSID");
        }
    }
}
