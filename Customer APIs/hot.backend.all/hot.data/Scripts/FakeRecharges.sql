 -- SET-UP A TEST RECHARGE:


 DECLARE @DealerMobile nvarchar(50) = '0772213890'

-- ECONET
DECLARE @Mobile nvarchar(50) = '0772322227'
DECLARE @Brand integer = 5

-- NETONE
-- DECLARE @Mobile nvarchar(50) = '0716025745'
-- DECLARE @Brand integer = 3

-- TELECEL JUICEO
-- DECLARE @Mobile nvarchar(50) = '0733352804'
-- DECLARE @Brand integer = 4

-- AFRICOM
-- DECLARE @Mobile nvarchar(50) = '08644082421'
-- DECLARE @Brand integer = 16

INSERT INTO tblRecharge (StateID, AccessID, Amount, Discount, Mobile, BrandID, RechargeDate, InsertDate)
  VALUES (0, 10480232,	0.1,	0, 	@Mobile,	@Brand,	GETDATE(),	GETDATE())

DECLARE @RechargeID integer

SELECT Top 1 @RechargeID = RechargeID FROM tblRecharge Order by InsertDate Desc

INSERT INTO [dbo].[tblSMS] ([SmppID],[StateID] ,[PriorityID],[Direction],[Mobile],[SMSText],[SMSDate],[SMSID_In],[InsertDate])
     VALUES (1, 2, 1, 1, @DealerMobile, 'Test Text', GETDATE(), null, GETDATE())

DECLARE @SMSID integer
SELECT TOP 1 @SMSID = SMSID FROM tblSMS ORDER BY InsertDate Desc

INSERT INTO [dbo].[tblSMSRecharge]([RechargeID],[SMSID])
     VALUES
           (@RechargeID, @SMSID)
GO

  select TOP 2 * from tblSMS order by SMSID desc
  select TOP 3 * from tblRecharge order by RechargeID desc
  Select top 3 * from tblrechargeprepaid order by RechargeID desc
GO