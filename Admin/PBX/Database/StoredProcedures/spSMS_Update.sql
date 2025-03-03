CREATE PROCEDURE [dbo].[spSMS_Update]
    @Id int,
	@Mobile NVARCHAR(12) , 
    @Text NVARCHAR(1000) , 
    @Date datetime,
    @DirectionId SMALLINT = 0, 
    @StatusId SMALLINT = 0, 
    @User NVARCHAR(150) = ''
AS
    update SMS set 
       Mobile = @Mobile,
       Text = @Text ,
       DirectionId = @DirectionId,
       StatusId = @StatusId,
       [User] = (case when @User = '' then SYSTEM_USER else @User end)
    where Id = @Id

RETURN @Id
