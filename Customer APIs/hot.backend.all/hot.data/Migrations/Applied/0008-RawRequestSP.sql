CREATE TABLE [dbo].[tblRawRequest](
	[RawRequestID] [bigint] IDENTITY(20000000,1) NOT NULL,
	[AgentReference] [varchar](50) NOT NULL,
	[AccessCode] [varchar](50) NOT NULL,
	[Headers] [varchar](1014) NULL,
	[Method] [varchar](10) NULL,
	[AbsoluteUri] [varchar](2048) NULL,
	[Body] [varchar](max) NULL,
	[StatusCode] [varchar](10) NULL,
	[ResponseBody] [varchar](max) NULL,
	[RequestDate] [datetime] NOT NULL,
	[ResponseDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


CREATE procedure [dbo].[xRawRequest_Save]
           @Headers varchar(1024) = null,
           @Method varchar(10) = null,
           @AbsoluteUri varchar(2048) = null,
           @Body varchar(max) = null,
           @StatusCode varchar(10) = null,
           @ResponseBody varchar(max) = null,
           @AgentReference varchar(50),
           @AccessCode varchar(50),
		   @IsRequest bit
as


if @IsRequest = 1
		INSERT INTO [dbo].[tblRawRequest] VALUES
            (@AgentReference, @AccessCode, @Headers, @Method, @AbsoluteUri, @Body, NULL, NULL, GetDate(), NULL)
else
		UPDATE [dbo].[tblRawRequest] Set ResponseBody = @ResponseBody, StatusCode = @StatusCode, ResponseDate = GetDate()
		where AgentReference = @AgentReference AND AccessCode = @AccessCode
GO




CREATE procedure [dbo].[xRawRequest_Select]

@agentReference	varchar(50),
@accessCode varchar(50)

as

select * from tblRawRequest
	where AgentReference = @agentReference AND AccessCode = @accessCode
	
GO	