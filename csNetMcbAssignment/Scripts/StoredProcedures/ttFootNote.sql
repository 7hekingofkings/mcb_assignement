-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 30/01/2024
-- Description: Table type for foot note.
-- Script name: ttFootNote.sql
-- ====================================================================================

-- ====================================================================================
-- Create Table type ttFOOT_NOTE
-- ====================================================================================
IF OBJECT_ID(N'ttFOOT_NOTE', N'TT') IS NOT NULL
BEGIN
	DROP TYPE ttFOOT_NOTE
END
GO

CREATE TYPE ttFOOT_NOTE AS TABLE
( COUNTRY_CODE VARCHAR(3)
, SERIES_CODE  VARCHAR(200)
, YEARS        INT
, DESCRIPTIONS VARCHAR(MAX))
GO


