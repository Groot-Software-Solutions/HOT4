    ALTER TABLE tblWebRequest Add [WalletBalance] [money] NULL;
    ALTER TABLE tblWebRequest Add [Cost] [money] NULL;
    ALTER TABLE tblWebRequest Add [Discount] [money] NULL;
    ALTER TABLE tblWebRequest Add [Amount] [money] NULL;
	ALTER TABLE tblWebRequest drop column Request;

    GO


CREATE procedure [dbo].[xWebRequest_Save]
           @HotTypeID tinyint = null,
           @AccessID bigint,
           @StateID tinyint,
		   @IsRequest As Bit,
           @AgentReference varchar(50),
           @ReturnCode int = null,
		   @Reply varchar(max) = null,
           @ChannelID tinyint,
           @RechargeID bigint = null,
           @WalletBalance money = NULL,
           @Cost money = NULL,
           @Discount money = NULL,
           @Amount money = NULL
as

if @IsRequest = 1
		INSERT INTO [dbo].[tblWebRequest]
           ([HotTypeID]
           ,[AccessID]
           ,[StateID]
           ,[InsertDate]
           ,[AgentReference]
           ,[ChannelID]
           ,[WalletBalance]
           ,[Amount]
           )
     VALUES
           (@HotTypeID,
           @AccessID ,
           @StateID ,
           GetDate(),
           @AgentReference,
           @ChannelID ,
           @WalletBalance,
           @Amount
           )
else
		UPDATE [dbo].[tblWebRequest] SET 
			ReplyDate = GetDate(), 
			Reply = @Reply, 
			[StateID] = @StateID,
			RechargeId =  @RechargeID, 
			[ReturnCode] = @ReturnCode,
			[WalletBalance] = @WalletBalance,
			[Discount] = [Discount],
			[Cost] = @Cost 
				WHERE AccessId = @AccessID AND AgentReference = @AgentReference
GO


GO


CREATE PROCEDURE dbo.xWebRequest_Select

@AgentReference	nvarchar(50),
@AccessID bigint

AS
BEGIN
    SET NOCOUNT ON;
    SELECT *  from tblWebRequest where AgentReference = @AgentReference AND AccessID = @AccessID Order by WebID desc
END
GO

