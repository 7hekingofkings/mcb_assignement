-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 27/01/2023
-- Description: Creation of WDI country table.
-- Script name: udCreateWDICountryTable.sql
-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_COUNTRY
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_COUNTRY', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_COUNTRY
	(
		COUNTRY_ID                  INTEGER IDENTITY(1,1) NOT NULL,
        COUNTRY_CODE                VARCHAR(3)            NOT NULL,
		SHORT_NAME                  VARCHAR(50)               NULL,
		TABLE_NAME                  VARCHAR(200)              NULL,
		LONG_NAME                   VARCHAR(500)              NULL,
		ALPHA_CODE                  VARCHAR(2)                NULL,
		CURRENCY_UNIT               VARCHAR(250)              NULL,
		SPECIAL_NOTES               VARCHAR(500)              NULL,
		REGION                      VARCHAR(200)              NULL,
		INCOME_GROUP                VARCHAR(200)              NULL,
		WB_CODE                     VARCHAR(2)            NOT NULL,
		NATIONAL_ACCOUNT_BASE_YEAR  VARCHAR(100)              NULL,
		NATIONAL_ACCOUNT_REFE_YEAR  VARCHAR(100)              NULL,
		SNA_PRICE_VALUATION         VARCHAR(100)              NULL,
		LENDING_CATEGORY            VARCHAR(10)               NULL,
		OTHER_GROUPS                VARCHAR(10)               NULL,
		NATIONAL_ACCOUNTS           VARCHAR(200)              NULL,
		CONVERSION_FACTOR           VARCHAR(100)              NULL,
		PPP_SURVEY_YEAR             VARCHAR(10)               NULL,
		BALANCE_PAYMENT_MANUAL      VARCHAR(50)               NULL,
		EXTERNAL_DEBT_STATUS        VARCHAR(20)               NULL,
		SYSTEM_TRADE                VARCHAR(100)              NULL,
		ACCOUNTING_CONCEPT          VARCHAR(100)              NULL,
		IMF_DATA                    VARCHAR(200)              NULL,
		POPULATION_SENSUS           VARCHAR(500)              NULL,
		HOUSEHOLD_SURVEY            VARCHAR(250)              NULL,
		INCOME_EXPENDITURE_DATA     VARCHAR(200)              NULL,
		REGISTRATION_COMPLETE       BIT                   NOT NULL,
		AGRICULTURAL_CENSUS         VARCHAR(200)              NULL,
		INDUSTRIAL_DATA             VARCHAR(50)               NULL,
		TRADE_DATA                  VARCHAR(50)               NULL,
		CONSTRAINT PK_tblWDI_COUNTRY_ID PRIMARY KEY(COUNTRY_ID),
		CONSTRAINT CK_COUNTRY_CODE_LEN CHECK(LEN(COUNTRY_CODE) =3),
	)
END
GO
