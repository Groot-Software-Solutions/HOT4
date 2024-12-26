using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class createtriggerreservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[LogNewReservation] 
            //   ON  [dbo].[tblReservation]
            //   AFTER INSERT 
            //AS 
            //BEGIN
            //	insert into tblreservationLog
            //	(ReservationId,OldStateId,NewStateId)
            //	select i.reservationId,0,i.stateid from inserted i 
            //END
            //GO

            //ALTER TABLE [dbo].[tblReservation] ENABLE TRIGGER [LogNewReservation]
            //GO");


            //            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[LogReservationUpdates] 
            //   ON  [dbo].[tblReservation] 
            //   AFTER UPDATE
            //AS 
            //BEGIN

            //	SET NOCOUNT ON;

            //	insert into tblreservationLog
            //	(ReservationId,OldStateId,NewStateId)
            //	select i.reservationId,d.stateId,i.stateid from inserted i
            //	inner join deleted d on i.ReservationId = d.ReservationId

            //END
            //GO

            //ALTER TABLE [dbo].[tblReservation] ENABLE TRIGGER [LogReservationUpdates]
            //GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF exists [dbo].[LogNewReservation]");
            migrationBuilder.Sql("DROP TRIGGER IF exists [dbo].[LogReservationUpdates]");
        }
    }
}
