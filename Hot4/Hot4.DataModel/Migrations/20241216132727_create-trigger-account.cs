using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class createtriggeraccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[AccountInsert_AddLimit] 
            //   ON  [dbo].[tblAccount] 
            //   AFTER INSERT
            //AS 
            //BEGIN

            //	SET NOCOUNT ON;

            //	insert into tbllimits
            //	([NetworkId],[LimitTypeId],[AccountId],[DailyLimit],[MonthlyLimit])
            //	select 1,3,accountid,5000 ,5000 from inserted
            //	union 
            //	select 2,3,accountid,5000 ,5000 from inserted
            //END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER if exists [dbo].[AccountInsert_AddLimit]");
        }
    }
}
