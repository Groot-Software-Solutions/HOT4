ALTER VIEW [dbo].[vwSMS]
AS
SELECT     dbo.tblSMS.SMSID, dbo.tblSMS.SmppID, dbo.tblState.StateID, dbo.tblState.State, dbo.tblPriority.PriorityID, dbo.tblPriority.Priority, dbo.tblSMS.Direction, 
                      dbo.tblSMS.Mobile, dbo.tblSMS.SMSText, dbo.tblSMS.SMSDate, dbo.tblSMS.InsertDate, dbo.tblSMS.SMSID_In
FROM         dbo.tblPriority INNER JOIN
                      dbo.tblSMS ON dbo.tblPriority.PriorityID = dbo.tblSMS.PriorityID INNER JOIN
                      dbo.tblState ON dbo.tblSMS.StateID = dbo.tblState.StateID
GO
