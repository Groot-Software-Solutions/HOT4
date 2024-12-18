using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class createtriggeraccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create TRIGGER [dbo].[trgEnsurePasswordHash] ON  
[dbo].[tblAccess] AFTER INSERT,UPDATE AS 
BEGIN
SET NOCOUNT ON;
if((select count(*) from inserted where PasswordHash is null)>0)
begin
update tblAccess set 
PasswordHash = LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', CONVERT(varchar(250), isnull(PasswordSalt, substring(LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', CONVERT(varchar, convert(varchar(10), AccountID ))), 2)),1,20))+AccessPassword)), 2)), 
PasswordSalt = isnull(PasswordSalt,substring(LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', CONVERT(varchar, convert(varchar(10), AccountID ))), 2)),1,20)) 
where PasswordHash is null 
end
END");

            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[trgOnlyOneActiveAccess_OnInsert] 
   ON  [dbo].[tblAccess] 
   AFTER INSERT
AS 
BEGIN
	
	SET NOCOUNT ON;

    
	  update dbo.tblaccess set Deleted = 1
	 where		AccessCode in (select AccessCode from inserted where deleted = 0  ) 
		and not accessid   in (select accessid from inserted where deleted = 0  )

END");

            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[trgOnlyOneActiveAccess_OnUpdate] 
   ON  [dbo].[tblAccess] 
   AFTER UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;
      if ((select count(1) from inserted where deleted = 0 )>0) 
	begin 
		update dbo.tblaccess set Deleted = 1 
		where AccessCode in (select AccessCode from inserted where deleted = 0  ) 
		and not AccessID in (select top 1 Accessid from inserted where deleted = 0  ) 
	end 
	END");

            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[trgStopBlank] 
   ON  [dbo].[tblAccess] 
   AFTER INSERT,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

	 update dbo.tblaccess set AccessCode = 'nonameprovided'+convert(varchar(20),accessid)+'@hot.co.zw'
	 where accessid  in ( select accessid from inserted
	where AccessCode = '' or AccessCode is null )

END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgEnsurePasswordHash]");
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgOnlyOneActiveAccess_OnInsert");
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgOnlyOneActiveAccess_OnUpdate");
            migrationBuilder.Sql("Drop trigger if exists [dbo].[trgStopBlank]");
        }
    }
}
