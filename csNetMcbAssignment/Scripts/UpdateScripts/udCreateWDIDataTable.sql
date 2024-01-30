-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 30/01/2024
-- Description: Creation of data table.
-- Script name: udCreateWDIFootNoteTable.sql
-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_DATA
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_DATA', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_DATA
	(
	  DATA_ID    INTEGER IDENTITY(1,1) NOT NULL,
	  COUNTRY_ID INTEGER               NOT NULL,
      SERIES_ID  INTEGER               NOT NULL,
	  PERIOD_ID  INTEGER               NOT NULL,
	  AMOUNT     FLOAT                 NOT NULL,
	  CONSTRAINT PK_tblWDI_DATA_ID PRIMARY KEY(DATA_ID),
	  CONSTRAINT FK_tblWDI_DATA_COUNTRY_ID FOREIGN KEY(COUNTRY_ID)
	  REFERENCES tblWDI_COUNTRY(COUNTRY_ID),
	  CONSTRAINT FK_tblWDI_DATA_SERIES_ID FOREIGN KEY(SERIES_ID)
	  REFERENCES tblWDI_SERIES(SERIES_ID),
	  CONSTRAINT FK_tblWDI_DATA_PERIOD_ID FOREIGN KEY(PERIOD_ID)
	  REFERENCES tblWDI_PERIOD(PERIOD_ID)
	)
END
GO

