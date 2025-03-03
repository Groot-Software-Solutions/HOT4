CREATE PROCEDURE [dbo].[spMessageTemplate_Get]
	@Id int
AS
	SELECT * from vwMessageTemplate
	where Id = @Id

RETURN 0
