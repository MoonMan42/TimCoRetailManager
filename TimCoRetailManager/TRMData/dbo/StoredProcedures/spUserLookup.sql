﻿CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
BEGIN
	set nocount on;

	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate
	FROM [dbo].[UserTable]
	WHERE Id = @Id;
END