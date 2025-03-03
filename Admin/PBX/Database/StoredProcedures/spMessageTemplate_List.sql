CREATE PROCEDURE [dbo].[spMessageTemplate_List]
AS
	SELECT top 5000 * from vwMessageTemplate
	Order by Id desc

RETURN 0
