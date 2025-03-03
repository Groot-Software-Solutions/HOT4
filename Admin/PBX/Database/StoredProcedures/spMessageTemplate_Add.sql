CREATE PROCEDURE [dbo].[spMessageTemplate_Add]
	
    @Text NVARCHAR(1000) , 
    @Name NVARCHAR(50) = ''
AS
    declare @Id table(Id int) 
	insert into MessageTemplate
	(
        [Name],
        [Text]
    ) 
    OUTPUT inserted.Id into @Id 
	SELECT  
        @Name,
        @Text 

RETURN (select top 1 Id  from @Id)
