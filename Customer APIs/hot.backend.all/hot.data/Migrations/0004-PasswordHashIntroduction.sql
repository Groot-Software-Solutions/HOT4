ALTER TABLE tblAccess Add PasswordHash varchar(50) null;
ALTER TABLE tblAccess Add PasswordSalt varchar(50) null;
ALTER TABLE tblAccess Add InsertDate DateTime NULL;
GO

CREATE procedure [dbo].[xAccess_Save2]

@AccessID		bigint,
@AccountID		bigint,
@ChannelID		tinyint,
@AccessCode		varchar	(50),
@PasswordHash	varchar	(50),
@PasswordSalt	varchar	(50)
as
if @AccessID = 0
    begin
    insert into tblAccess (AccountID, ChannelID, AccessCode, AccessPassword, PasswordHash, PasswordSalt, Deleted, InsertDate) 
    values(@AccountID, @ChannelID, @AccessCode, 'DEPRECATED', @PasswordHash, @PasswordSalt, 0, GetDate())
    select @AccessID = scope_identity()
    end
else
    update tblAccess set
            AccountID = @AccountID,
            ChannelID = @ChannelID,
            AccessCode = @AccessCode,
            PasswordHash = @PasswordHash,
            PasswordSalt = @PasswordSalt,
            Deleted = 0
        where AccessID = @AccessID

select @AccessID as AccessID
GO

CREATE procedure [dbo].[xAccess_PasswordChange2]
@AccessID bigint,
@PasswordHash varchar(50),
@PasswordSalt varchar(50)

as
update tblAccess set
    AccessPassword = 'DEPRECATED',
    PasswordHash = @PasswordHash,
    PasswordSalt = @PasswordSalt
    where AccessID = @AccessID
GO


CREATE procedure [dbo].[xAccess_SelectLogin2]
@AccessCode varchar(50)
as
select * from vwAccess where AccessCode = @AccessCode collate SQL_Latin1_General_CP1_CS_AS AND Deleted = 0
GO


ALTER VIEW [dbo].[vwAccess]
AS
SELECT     dbo.tblAccess.AccessID, dbo.tblAccess.AccountID, dbo.tblChannel.ChannelID, dbo.tblChannel.Channel, dbo.tblAccess.AccessCode, dbo.tblAccess.AccessPassword, dbo.tblAccess.PasswordHash, dbo.tblAccess.PasswordSalt,
                      dbo.tblAccess.Deleted
FROM         dbo.tblAccess INNER JOIN
                      dbo.tblChannel ON dbo.tblAccess.ChannelID = dbo.tblChannel.ChannelID

GO
