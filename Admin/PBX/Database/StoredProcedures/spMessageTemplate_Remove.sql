CREATE PROCEDURE [dbo].[spMessageTemplate_Remove]
    @Id int
AS
    Delete from MessageTemplate 
    where Id = @Id

RETURN @Id