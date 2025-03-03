CREATE PROCEDURE [dbo].[spSMS_Get]
	@Id int
AS
	SELECT * from vwSMS
	where Id = @Id

RETURN 0
