-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 29/01/2024
-- Description: Procedure for foot note.
-- Script name: spFootNote.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spFOOT_NOTE
-- ====================================================================================
IF OBJECT_ID(N'spFOOT_NOTE', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spFOOT_NOTE
END
GO

CREATE PROCEDURE spFOOT_NOTE
@FOOTNOTE  ttFOOT_NOTE READONLY
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON

	-- Insert missing period
	INSERT INTO tblWDI_PERIOD
	(YEARS)
	SELECT DISTINCT tbltmp.YEARS
	FROM   @FOOTNOTE tbltmp
	WHERE  YEARS NOT IN (SELECT YEARS
	                     FROM   tblWDI_PERIOD)
    --Update
    UPDATE tblNote
    SET    tblNote.DESCRIPTIONS = tblTemp.DESCRIPTIONS
    FROM tblWDI_FOOT_NOTE tblNote
    INNER JOIN tblWDI_COUNTRY tblCountry
	ON         tblCountry.COUNTRY_ID = tblNote.COUNTRY_ID
	INNER JOIN tblWDI_SERIES tblSeries
	ON         tblSeries.SERIES_ID = tblNote.SERIES_ID
	INNER JOIN tblWDI_PERIOD tblPeriod
	ON         tblPeriod.PERIOD_ID = tblNote.PERIOD_ID
	INNER JOIN @FOOTNOTE tblTemp
	ON         tblTemp.COUNTRY_CODE = tblCountry.COUNTRY_CODE
	AND        tblTemp.SERIES_CODE  = tblSeries.SERIES_CODE
	AND        tblTemp.YEARS        = tblPeriod.YEARS

	-- Insert New
	INSERT INTO tblWDI_FOOT_NOTE
	( COUNTRY_ID
	, SERIES_ID
	, PERIOD_ID
	, DESCRIPTIONS)
	SELECT tblCountry.COUNTRY_ID
	,      tblSeries.SERIES_ID
	,      tblPeriod.PERIOD_ID
	,      tblTemp.DESCRIPTIONS
	FROM       @FOOTNOTE tblTemp
	INNER JOIN tblWDI_COUNTRY tblCountry
	ON         tblCountry.COUNTRY_CODE = tblTemp.COUNTRY_CODE
	INNER JOIN tblWDI_SERIES tblSeries
	ON         tblSeries.SERIES_CODE = tblTemp.SERIES_CODE
	INNER JOIN tblWDI_PERIOD tblPeriod
	ON         tblPeriod.YEARS = tblTemp.YEARS
	WHERE NOT EXISTS(SELECT 1
	                 FROM   tblWDI_FOOT_NOTE tblNote
					 WHERE  tblNote.COUNTRY_ID = tblCountry.COUNTRY_ID
					 AND    tblNote.PERIOD_ID = tblPeriod.PERIOD_ID
					 AND    tblNote.SERIES_ID = tblSeries.SERIES_ID)
END TRY           
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
  
    SELECT   
        @ErrorMessage  = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState    = ERROR_STATE();  
    
    RAISERROR (@ErrorMessage,  
               @ErrorSeverity,  
               @ErrorState);  
END CATCH
GO