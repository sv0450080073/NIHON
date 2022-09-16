USE [HOC_Kashikiri]	
GO	
	
SET	
    ANSI_NULLS ON	
GO	
SET	
    QUOTED_IDENTIFIER ON	
GO	
    CREATE FUNCTION [dbo].[FP_DistinctStringAgg](@value NVARCHAR(max)) RETURNS NVARCHAR(max) AS BEGIN	
SELECT	
    @value = STRING_AGG(value, ',')	
FROM	
    (	
        SELECT	
            DISTINCT value	
        FROM	
            STRING_SPLIT(@value, ',')	
    ) a RETURN @value	
END	
