using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class addprimarykeyfieldinstockdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "StockDataId",
                table: "tblStockData",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblStockData",
                table: "tblStockData",
                column: "StockDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblStockData",
                table: "tblStockData");

            migrationBuilder.DropColumn(
                name: "StockDataId",
                table: "tblStockData");
        }
    }
}
