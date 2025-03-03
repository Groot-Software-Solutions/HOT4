CREATE PROCEDURE [dbo].[spSMS_GetByString]
	@Id varchar(12)
AS
	SELECT top 200 * from vwSMS
	where Mobile = @Id
	order by Id desc

RETURN 0
