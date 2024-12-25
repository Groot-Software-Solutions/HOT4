using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class createtriggerpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
				  FROM   tblaccess  
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
            migrationBuilder.Sql("DROP TRIGGER if exists [dbo].[EmailPaymentInsert]");
        }
    }
}
