-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 28/01/2024
-- Description: Procedure for setting.
-- Script name: spSETTING.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spSETTING.sql
-- ====================================================================================
IF OBJECT_ID(N'spSETTING', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spSETTING
END
GO

CREATE PROCEDURE spSETTING
@SETTING_ID     INTEGER OUTPUT,
@IMPORT_PATH    VARCHAR(250),
@IMPORTED_PATH  VARCHAR(250),
@EXECUTE_EVERY  INTEGER,
@OPERATION_TYPE INTEGER
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON

	-- Insert setting
	IF @OPERATION_TYPE = 0
	BEGIN
        IF (SELECT COUNT(*)
            FROM   tblSETTING) <= 0
        BEGIN
            INSERT INTO tblSETTING
            ( IMPORT_PATH              
	        , IMPORTED_PATH          
            , EXECUTE_EVERY)
            VALUES
            ( @IMPORT_PATH
            , @IMPORTED_PATH
            , @EXECUTE_EVERY)

            SELECT @SETTING_ID = SCOPE_IDENTITY()
        END
        ELSE
        BEGIN
            UPDATE tblSETTING
            SET    IMPORT_PATH     = @IMPORT_PATH
            ,      IMPORTED_PATH   = @IMPORTED_PATH
            ,      EXECUTE_EVERY   = @EXECUTE_EVERY
        END		
    END

    -- Select setting
    IF @OPERATION_TYPE = 1
    BEGIN
        SELECT SETTING_ID
        ,      IMPORT_PATH              
	    ,      IMPORTED_PATH          
        ,      EXECUTE_EVERY          
        FROM   tblSETTING
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
