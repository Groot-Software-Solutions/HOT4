/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if (isnull((select count(*) from Status),0)=0) 
begin 
insert into Status 
select 0, 'New'     union
select 1, 'Read'    union
select 2, 'Success' union
select 3, 'Failed'  
end 