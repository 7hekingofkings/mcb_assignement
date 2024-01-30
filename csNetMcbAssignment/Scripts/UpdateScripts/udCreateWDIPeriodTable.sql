-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 29/01/2024
-- Description: Creation of period table.
-- Script name: udCreateWDIPeriodTable.sql

-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_PERIOD
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_PERIOD', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_PERIOD
	(
	  PERIOD_ID  INTEGER IDENTITY(1,1) NOT NULL,
	  YEARS      INTEGER               NOT NULL,
	  CONSTRAINT PK_tblWDI_PERIOD_ID PRIMARY KEY(PERIOD_ID),
	)
END
GO

