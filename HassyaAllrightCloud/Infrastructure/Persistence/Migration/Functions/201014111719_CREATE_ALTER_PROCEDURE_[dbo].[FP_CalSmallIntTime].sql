USE [HOC_Kashikiri]
GO
/****** Object:  UserDefinedFunction [dbo].[FP_CalSmallIntTime]    Script Date: 2020/10/14 11:17:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<KieuAnhNHH,,Name>
-- Create date: <14-Oct-2020>
-- Description:	<Convert smallint of time to varchar>
-- =============================================
CREATE OR ALTER   FUNCTION [dbo].[FP_CalSmallIntTime] 
	(
	-- Add the parameters for the function here
		@Hours1			SMALLINT		-- 時1
	,	@Minutes1		SMALLINT		-- 分1
	,	@Hours2			SMALLINT		-- 時2
	,	@Minutes2		SMALLINT		-- 分2
	)
	RETURNS VARCHAR(4)
AS
	BEGIN
		-- Declare the return variable here
		DECLARE @HoursAfterCal VARCHAR(2) 
		DECLARE @MinutesAfterCal VARCHAR(2) 

		-- Add the T-SQL statements to compute the return value here
		SET @HoursAfterCal = CONVERT(VARCHAR(2), (@Hours1 + @Hours2) + ((@Minutes1 + @Minutes2) / 60))
		SET @MinutesAfterCal = CONVERT(VARCHAR(2), ((@Minutes1 + @Minutes2) % 60))

		IF DATALENGTH( @HoursAfterCal ) < 2 
			SET @HoursAfterCal = '0' + @HoursAfterCal
			
		IF DATALENGTH( @MinutesAfterCal ) < 2 
			SET @MinutesAfterCal = '0' + @MinutesAfterCal

		-- Return the result of the function
		RETURN @HoursAfterCal + @MinutesAfterCal

	END
