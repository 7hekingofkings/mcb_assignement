-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 29/01/2024
-- Description: Creation of foot note table.
-- Script name: udCreateWDIFootNoteTable.sql
-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_FOOT_NOTE
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_FOOT_NOTE', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_FOOT_NOTE
	(
	  FOOT_NOTE_ID INTEGER IDENTITY(1,1) NOT NULL,
	  COUNTRY_ID   INTEGER               NOT NULL,
      SERIES_ID     INTEGER              NOT NULL,
	  PERIOD_ID     INTEGER              NOT NULL,
	  DESCRIPTIONS   VARCHAR(MAX)            NULL,
	  CONSTRAINT PK_tblWDI_FOOT_NOTE_ID PRIMARY KEY(FOOT_NOTE_ID),
	  CONSTRAINT FK_tblWDI_FOOT_NOTE_COUNTRY_ID FOREIGN KEY(COUNTRY_ID)
	  REFERENCES tblWDI_COUNTRY(COUNTRY_ID),
	  CONSTRAINT FK_tblWDI_FOOT_NOTE_SERIES_ID FOREIGN KEY(SERIES_ID)
	  REFERENCES tblWDI_SERIES(SERIES_ID),
	  CONSTRAINT FK_tblWDI_FOOT_NOTE_PERIOD_ID FOREIGN KEY(PERIOD_ID)
	  REFERENCES tblWDI_PERIOD(PERIOD_ID)
	)
END
GO

