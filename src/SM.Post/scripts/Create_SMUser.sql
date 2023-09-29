﻿Use socialMedia
GO

IF NOT EXISTS(SELECT * FROM sys.server_principals WHERE name = 'SMUser')
BEGIN
	CREATE LOGIN SMUser WITH PASSWORD=N'Password01', DEFAULT_DATABASE=socialMedia
END

IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = 'SMUser')
BEGIN
	EXEC sp_adduser 'SMUser', 'SMUser', 'db_owner';
END