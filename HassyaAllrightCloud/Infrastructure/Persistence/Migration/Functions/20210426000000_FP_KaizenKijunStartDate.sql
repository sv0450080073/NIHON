USE [HOC_Kashikiri]	
GO	
SET	
    ANSI_NULLS ON	
GO	
SET	
    QUOTED_IDENTIFIER ON	
GO	
    CREATE	
    OR ALTER FUNCTION [dbo].[FP_KaizenKijunStartDate] --パラメータ	
    (	
        @RefDate CHAR(8),	
        @TouDate CHAR(8),	
        @PeriodDays INT,	
        @Times INT	
    ) RETURNS CHAR(8) AS BEGIN DECLARE @Ref_Date DATE = @RefDate,	
    @Tou_Date DATE = @TouDate,	
    @ReturnDate DATE;	
	
SET	
    @ReturnDate = (	
        DATEADD(	
            DAY,	
            -(	
                DATEDIFF(DAY, @Ref_Date, @Tou_Date) %(@PeriodDays * @Times)	
            ),	
            @Tou_Date	
        )	
    );	
	
RETURN FORMAT(@ReturnDate, 'yyyyMMdd')	
END	
