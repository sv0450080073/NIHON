USE [HOC_Kashikiri]
GO

/****** Object:  UserDefinedFunction [dbo].[FP_CalCharTime]    Script Date: 2020/10/08 15:39:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<KieuAnhNHH,,Name>
-- Create date: <2020-10-08>
-- Description:	<Convert 4 characters of time to int for calculating>
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[FP_CalCharTime] 
	(
	-- Add the parameters for the function here
		@Time1		VARCHAR(4)
	,	@Time2		VARCHAR(4)
	)
	RETURNS VARCHAR(4)
AS
	BEGIN
		-- Declare the return variable here
		DECLARE @HoursAfterCal VARCHAR(2) 
		DECLARE @MinutesAfterCal VARCHAR(2) 

		-- Add the T-SQL statements to compute the return value here
		SET @HoursAfterCal = CONVERT(int, SUBSTRING(@Time1, 1, 2)) + CONVERT(int, SUBSTRING(@Time2, 1, 2)) + (((CONVERT(int, SUBSTRING(@Time1, 3, 2))) + (CONVERT(int, SUBSTRING(@Time2, 3, 2)))) / 60)
		SET @MinutesAfterCal = ((CONVERT(int, SUBSTRING(@Time1, 3, 2))) + (CONVERT(int, SUBSTRING(@Time2, 3, 2)))) % 60

		IF DATALENGTH( @HoursAfterCal ) < 2 
			SET @HoursAfterCal = '0' + @HoursAfterCal
			
		IF DATALENGTH( @MinutesAfterCal ) < 2 
			SET @MinutesAfterCal = '0' + @MinutesAfterCal

		-- Return the result of the function
		RETURN @HoursAfterCal + @MinutesAfterCal

	END
GO


