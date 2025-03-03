CREATE PROCEDURE [dbo].[spSMS_Remove]
    @Id int
AS
    Delete from SMS 
    where Id = @Id

RETURN @Id
