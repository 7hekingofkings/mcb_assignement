-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 30/01/2024
-- Description: Table corruption index.
-- Script name: ttCorruption.sql
-- ====================================================================================

-- ====================================================================================
-- Create Table type ttCORRUPTION
-- ====================================================================================
IF OBJECT_ID(N'ttCORRUPTION', N'TT') IS NOT NULL
BEGIN
	DROP TYPE ttCORRUPTION
END
GO

CREATE TYPE ttCORRUPTION AS TABLE
( COUNTRY_NAME   VARCHAR(250)
, COUNTRY_CODE   VARCHAR(3)
, REGION         VARCHAR(200)
, YEARS          INT
, CPI_SCORE      FLOAT
, RANKS          FLOAT
, SOURCES        FLOAT
, STANDARD_ERROR FLOAT)
GO


