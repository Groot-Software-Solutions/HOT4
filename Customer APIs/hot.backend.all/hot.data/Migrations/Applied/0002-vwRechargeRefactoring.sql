
CREATE procedure [dbo].[xRecharge_Find2]

@AccountID		bigint,
@Mobile			varchar	(50)

as

select * from (
	select top 20 r.*
	from tblRecharge r
		inner join tblAccess a on r.AccessID = a.AccessID
	where	AccountID = @AccountID
		and Mobile like '%' + @Mobile + '%'
		
order by RechargeID desc) as c
GO


CREATE procedure [dbo].[xRecharge_Select2]

@RechargeID		bigint

as

select * from tblRecharge where RechargeID = @RechargeID

GO

CREATE TYPE dbo.tblBrandList
AS TABLE
(
  BrandID INT
);
GO


CREATE procedure [dbo].[xRecharge_Pending]
  @brandList AS dbo.tblBrandList READONLY
AS

Declare @RechargeID Bigint

Begin Tran

Set @RechargeID = (
	select top 1 r.RechargeID from @brandList b
	LEFT JOIN tblRecharge r
	ON r.BrandID = b.BrandID
	where r.StateID = 0   
	order by r.RechargeID asc);
update tblRecharge set StateID = 1 where RechargeID = @RechargeID;
select top 1 * from tblRecharge where RechargeID = @RechargeID;

COMMIT
GO
