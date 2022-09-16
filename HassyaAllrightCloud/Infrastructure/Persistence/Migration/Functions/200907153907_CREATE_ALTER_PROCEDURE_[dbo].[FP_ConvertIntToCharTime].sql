USE [HOC_Kashikiri]
GO

/****** Object:  UserDefinedFunction [dbo].[FP_ConvertIntToCharTime]    Script Date: 2020/09/07 15:39:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<KieuAnhNHH,,Name>
-- Create date: <14-Oct-2020>
-- Description:	<Convert int of time to char>
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[FP_ConvertIntToCharTime] 
	(
	-- Add the parameters for the function here
		@Hours			int,		-- 分1
		@Minutes		int			-- 時1
	)
	RETURNS VARCHAR(5)
AS
	BEGIN
		-- Declare the return variable here
		DECLARE @HoursAfterConv VARCHAR(3) 
		DECLARE @MinutesAfterConv VARCHAR(2) 

		SET @HoursAfterConv = CONVERT(VARCHAR(3), @Hours);
		SET @MinutesAfterConv = CONVERT(VARCHAR(2), @Minutes);

		IF DATALENGTH( @HoursAfterConv ) < 2 
			SET @HoursAfterConv = '0' + @HoursAfterConv
			
		IF DATALENGTH( @MinutesAfterConv ) < 2 
			SET @MinutesAfterConv = '0' + @MinutesAfterConv

		-- Return the result of the function
		RETURN @HoursAfterConv + @MinutesAfterConv

	END
GO


