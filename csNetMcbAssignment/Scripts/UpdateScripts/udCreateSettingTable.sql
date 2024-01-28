-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 28/01/2024
-- Description: Creation of setting table.
-- Script name: udCreateSettingTable.cs
-- ====================================================================================

-- ====================================================================================
-- Create table tblSETTING
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblSETTING', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblSETTING
	(
	  SETTING_ID    INTEGER IDENTITY(1,1) NOT NULL,
      IMPORT_PATH   VARCHAR(250)              NULL,
	  IMPORTED_PATH VARCHAR(250)              NULL,
	  EXECUTE_EVERY INTEGER                   NULL,
	  CONSTRAINT PK_tblSETTING_ID PRIMARY KEY(SETTING_ID)
	)
END
GO
