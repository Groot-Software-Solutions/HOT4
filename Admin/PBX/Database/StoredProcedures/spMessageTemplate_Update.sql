CREATE PROCEDURE [dbo].[spMessageTemplate_Update]
    @Id int,
	@Name NVARCHAR(50) , 
    @Text NVARCHAR(1000)
AS
    update MessageTemplate set 
       Name = @Name,
       Text = @Text
    where Id = @Id

RETURN @Id