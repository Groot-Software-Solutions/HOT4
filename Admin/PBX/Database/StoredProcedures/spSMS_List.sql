CREATE PROCEDURE [dbo].[spSMS_List]
AS
	SELECT top 5000 * from vwSMS
	Order by Date desc

RETURN 0
