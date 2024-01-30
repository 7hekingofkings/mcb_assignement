-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 30/01/2024
-- Description: Creation of corruption table.
-- Script name: udCreateCorruptionTable.sql
-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_CORRUPTION
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_CORRUPTION', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_CORRUPTION
	(
	  CORRUPTION_ID INTEGER IDENTITY(1,1) NOT NULL,
	  COUNTRY_ID    INTEGER               NOT NULL,
	  PERIOD_ID     INTEGER               NOT NULL,
	  CPI_CORE      FLOAT                     NULL,
	  RANKED        INTEGER                   NULL,
	  SOURCES       FLOAT                     NULL,
	  ERROR         FLOAT                     NULL,
	  CONSTRAINT PK_tblWDI_CORRUPTION_ID PRIMARY KEY(CORRUPTION_ID),
	  CONSTRAINT FK_tblWDI_CORRUPTION_COUNTRY_ID FOREIGN KEY(COUNTRY_ID)
	  REFERENCES tblWDI_COUNTRY(COUNTRY_ID),
	  CONSTRAINT FK_tblWDI_CORRUPTION_PERIOD_ID FOREIGN KEY(PERIOD_ID)
	  REFERENCES tblWDI_PERIOD(PERIOD_ID)
	)
END
GO

