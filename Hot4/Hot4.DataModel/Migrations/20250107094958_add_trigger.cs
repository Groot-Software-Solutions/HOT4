using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class add_trigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgEnsurePasswordHash]");

            //migrationBuilder.Sql(@"IF OBJECT_ID('[dbo].[trgEnsurePasswordHash]', 'TR') IS NOT NULL
            //        DROP TRIGGER [dbo].[trgEnsurePasswordHash];");

            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[trgEnsurePasswordHash] ON  
 [dbo].[tblAccess] AFTER INSERT,UPDATE AS 
 BEGIN
 SET NOCOUNT ON;
 if((select count(*) from inserted where PasswordHash is null)>0)
 begin
 update tblAccess set 
 PasswordHash = LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', CONVERT(varchar(250), isnull(PasswordSalt, substring(LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', CONVERT(varchar, convert(varchar(10), AccountID ))), 2)),1,20))+AccessPassword)), 2)), 
 PasswordSalt = isnull(PasswordSalt,substring(LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', CONVERT(varchar, convert(varchar(10), AccountID ))), 2)),1,20)) 
 where PasswordHash is null 
 end
 END");

            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgOnlyOneActiveAccess_OnInsert]");
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[trgOnlyOneActiveAccess_OnInsert] 
    ON  [dbo].[tblAccess] 
    AFTER INSERT
 AS 
 BEGIN

    	SET NOCOUNT ON;


    	  update dbo.tblAccess set Deleted = 1
    	 where		AccessCode in (select AccessCode from inserted where deleted = 0  ) 
       		and not accessid   in (select accessid from inserted where deleted = 0  )

 END");

            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgOnlyOneActiveAccess_OnUpdate]");
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[trgOnlyOneActiveAccess_OnUpdate] 
    ON  [dbo].[tblAccess] 
    AFTER UPDATE
 AS 
 BEGIN

    	SET NOCOUNT ON;
       if ((select count(1) from inserted where deleted = 0 )>0) 
    	begin 
       		update dbo.tblAccess set Deleted = 1 
       		where AccessCode in (select AccessCode from inserted where deleted = 0  ) 
       		and not AccessID in (select top 1 Accessid from inserted where deleted = 0  ) 
    	end 
    	END");


            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgStopBlank]");
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[trgStopBlank] 
    ON  [dbo].[tblAccess] 
    AFTER INSERT,UPDATE
 AS 
 BEGIN

    	SET NOCOUNT ON;

    	 update dbo.tblAccess set AccessCode = 'nonameprovided'+convert(varchar(20),accessid)+'@hot.co.zw'
    	 where accessid  in ( select accessid from inserted
    	where AccessCode = '' or AccessCode is null )

 END");

            migrationBuilder.Sql("Drop trigger if exists [dbo].[AccountInsert_AddLimit]");
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[AccountInsert_AddLimit] 
    ON  [dbo].[tblAccount] 
    AFTER INSERT
 AS 
 BEGIN

    	SET NOCOUNT ON;

    	insert into tbllimits
    	([NetworkId],[LimitTypeId],[AccountId],[DailyLimit],[MonthlyLimit])
    	select 1,3,accountid,5000 ,5000 from inserted
    	union 
    	select 2,3,accountid,5000 ,5000 from inserted
 END");

            migrationBuilder.Sql("Drop trigger if exists [dbo].[LogNewReservation]");
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[LogNewReservation] 
     ON  [dbo].[tblReservation]
     AFTER INSERT 
  AS 
  BEGIN
     	insert into tblReservationLog
     	(ReservationId,OldStateId,NewStateId)
     	select i.reservationId,0,i.stateid from inserted i 
  END
  GO

  ALTER TABLE [dbo].[tblReservation] ENABLE TRIGGER [LogNewReservation]
  GO");

            migrationBuilder.Sql("Drop trigger if exists [dbo].[LogReservationUpdates]");
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[LogReservationUpdates] 
     ON  [dbo].[tblReservation] 
     AFTER UPDATE
  AS 
  BEGIN

     	SET NOCOUNT ON;

     	insert into tblReservationLog
     	(ReservationId,OldStateId,NewStateId)
     	select i.reservationId,d.stateId,i.stateid from inserted i
     	inner join deleted d on i.ReservationId = d.ReservationId

  END
  GO

  ALTER TABLE [dbo].[tblReservation] ENABLE TRIGGER [LogReservationUpdates]
  GO");


            migrationBuilder.Sql("Drop trigger if exists [dbo].[EmailPaymentInsert]");
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[EmailPaymentInsert] 
    ON  [dbo].[tblPayment] 
    AFTER INSERT
 AS 
 BEGIN

       		SET NOCOUNT ON;
    	Declare @ChannelID TinyInt
    	Select @ChannelID = ChannelID from Inserted i 
       		inner join tblAccess a on i.AccountID = a.AccountID
       		where ChannelID = 2

    	if @ChannelID = 2

    	Begin
       		declare @Accesscode varchar(max) 
       		declare @SMS varchar(max) 
       		set @AccessCode = 	  CAST((
          				  SELECT  tblAccess.AccessCode +'; '
          				  FROM   tblAccess  
          				  where AccountID in (select top 1 AccountID from inserted i)
          					and tblAccess.ChannelID=2
          					and deleted = 0						
          			 FOR XML PATH(''))as varchar(max))


       		select	Top 1 
          				@SMS = 'Dear ' + AccountName
          					+ '<br><br>Thank you, we have received Payment of '  + convert(varchar,Amount,1) 
          					+ '<br>    into your: ' + 
          								case	when PaymentTypeID = 16 then 'ZiG no Vat (ZESA)'
          										when PaymentTypeID = 17 then 'USD Vat'
          										when PaymentTypeID = 19 then 'USD no Vat (Utility)'
          										else 'ZiG Vat'
          										end
          					+ ' Stock Account'					
          					+ '<br>    done through a payment at: ' + PaymentSource 
          					+ '<br>    with PaymentID: ' + convert(varchar,i.paymentID)
          					+ '<br>    on: ' + convert(varchar, PaymentDate,106) 
          					+ '<br>    with the reference: ' + Reference 
          					+ '<br><br> Enjoy using our wonderful selection of prepaid and payment products from Cellular Networks and Suppliers in Zimbabwe'
          					+ '<br><br> Kind Regards - <br>The HOT Recharge Team <br> Airtime, Anywhere, Anytime'
          			from Inserted i
          				inner join tblPaymentSource s on i.PaymentSourceID = s.PaymentSourceID	
          				inner join tblAccess a on i.AccountID = a.AccountID
          				inner join tblAccount on i.AccountID = tblAccount.AccountID

          				where ChannelID = 2 and PaymentTypeID not in (12,20)
          				and Deleted = 0

       		if (not(@Accesscode='robert@smartdev.co.zw'))
          				begin
          					EXEC msdb.dbo.sp_send_dbmail 
          						@recipients = @AccessCode,
          						@body = @SMS,
          						@body_format='HTML',
          						@subject = 'HOT Recharge Payment Received',
          						@profile_name ='Frampol',
          						@execute_query_database = 'HOT4' 
          				end 
    	End

 END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgEnsurePasswordHash]");
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgOnlyOneActiveAccess_OnInsert");
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgOnlyOneActiveAccess_OnUpdate");
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgStopBlank]");

            migrationBuilder.Sql("DROP TRIGGER if exists [dbo].[AccountInsert_AddLimit]");

            migrationBuilder.Sql("DROP TRIGGER IF exists [dbo].[LogNewReservation]");
            migrationBuilder.Sql("DROP TRIGGER IF exists [dbo].[LogReservationUpdates]");

            migrationBuilder.Sql("DROP TRIGGER if exists [dbo].[EmailPaymentInsert]");
        }
    }
}
