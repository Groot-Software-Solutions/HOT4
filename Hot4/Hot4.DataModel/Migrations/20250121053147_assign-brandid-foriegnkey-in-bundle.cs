using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class assignbrandidforiegnkeyinbundle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "BrandID",
                table: "tblBundles",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_tblBundles_BrandID",
                table: "tblBundles",
                column: "BrandID");

            migrationBuilder.AddForeignKey(
                name: "FK_tblBundle_tblBrand",
                table: "tblBundles",
                column: "BrandID",
                principalTable: "tblBrand",
                principalColumn: "BrandID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblBundle_tblBrand",
                table: "tblBundles");

            migrationBuilder.DropIndex(
                name: "IX_tblBundles_BrandID",
                table: "tblBundles");

            migrationBuilder.AlterColumn<int>(
                name: "BrandID",
                table: "tblBundles",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
