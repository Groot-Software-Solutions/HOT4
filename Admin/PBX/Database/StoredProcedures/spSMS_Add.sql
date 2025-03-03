CREATE PROCEDURE [dbo].[spSMS_Add]
	@Mobile NVARCHAR(12) , 
    @Text NVARCHAR(1000) , 
    @Date DATETIME ='01/01/01', 
    @DirectionId SMALLINT = 0, 
    @StatusId SMALLINT = 0, 
    @User NVARCHAR(150) = ''
AS
    declare @Id table(Id int) 
	insert into SMS
	(
        [Mobile],
        [Text] ,
        [Date] , 
        [DirectionId],
        [StatusId] ,
        [User] 
    ) 
    OUTPUT inserted.Id into @Id 
	SELECT 
        @Mobile,
        @Text ,
        case when @Date = '01/01/01' then GETDATE() else @Date end, 
        @DirectionId,
        @StatusId,
        case 
            when @User = '' then SYSTEM_USER 
            when @User is null then SYSTEM_USER 
            else @User 
         end

RETURN (select top 1 Id  from @Id)
