-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 29/01/2024
-- Description: Creation of country series table.
-- Script name: udCreateWDITimeSeries.sql
-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_TIMES_SERIES
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_TIMES_SERIES', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_TIME_SERIES
	(
	  TIME_SERIES_ID INTEGER IDENTITY(1,1) NOT NULL,
      SERIES_ID      INTEGER               NOT NULL,
	  PERIOD_ID      INTEGER               NOT NULL,
	  DESCRIPTIONS   VARCHAR(500)              NULL,
	  CONSTRAINT PK_tblWDI_TIME_SERIES_ID PRIMARY KEY(TIME_SERIES_ID),
	  CONSTRAINT FK_tblWDI_TIME_SERIES_SERIES_ID FOREIGN KEY(SERIES_ID)
	  REFERENCES tblWDI_SERIES(SERIES_ID),
	  CONSTRAINT FK_tblWDI_TIME_SERIES_PERIOD_ID FOREIGN KEY(PERIOD_ID)
	  REFERENCES tblWDI_PERIOD(PERIOD_ID)
	)
END
GO

