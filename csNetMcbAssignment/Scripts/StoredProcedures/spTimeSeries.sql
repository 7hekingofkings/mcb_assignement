-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 29/01/2024
-- Description: Procedure for time sereis.
-- Script name: spTimeSeries.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spTimeSeries
-- ====================================================================================
IF OBJECT_ID(N'spTimeSeries', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spTimeSeries
END
GO

CREATE PROCEDURE spTimeSeries
@TIME_SERIES_ID INTEGER OUTPUT,         
@SERIES_CODE    VARCHAR(200),
@YEARS          INTEGER,
@DESCRIPTIONS   VARCHAR(500)
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON

    DECLARE @PERIOD_ID INTEGER    
    DECLARE @SERIES_ID INTEGER

    IF NOT EXISTS (SELECT 1
                   FROM   tblWDI_PERIOD
                   WHERE  YEARS = @YEARS)
    BEGIN
        INSERT INTO tblWDI_PERIOD
        (YEARS)
        VALUES
        (@YEARS)

        SET @PERIOD_ID = SCOPE_IDENTITY()
    END
    ELSE
    BEGIN
        SELECT @PERIOD_ID = PERIOD_ID
        FROM   tblWDI_PERIOD
        WHERE  YEARS = @YEARS
    END

	SET @SERIES_ID = (SELECT tblSeries.SERIES_ID
                      FROM   tblWDI_SERIES tblSeries
					  WHERE  tblSeries.SERIES_CODE = @SERIES_CODE)

    IF NOT EXISTS(SELECT 1
                  FROM   tblWDI_TIME_SERIES tblTimeSeries
                  WHERE  tblTimeSeries.SERIES_ID = @SERIES_ID
				  AND    tblTimeSeries.PERIOD_ID = @PERIOD_ID)
    BEGIN
		INSERT INTO tblWDI_TIME_SERIES
		( SERIES_ID
		, PERIOD_ID
		, DESCRIPTIONS)
		VALUES
		( @SERIES_ID
		, @PERIOD_ID
		, @DESCRIPTIONS)

		SET @TIME_SERIES_ID = SCOPE_IDENTITY()
    END
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