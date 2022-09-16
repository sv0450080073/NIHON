-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetHoliday_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List Holiday
-- Date			:   2021/04/12
-- Author		:   P.M.Nhat
-- Description	:   Get list holiday with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dGetHoliday_R
	-- Add the parameters for the stored procedure here
	@CompanyCdSeq INT,					
	@SyainCdSeq INT,					
	@UnkYmd CHAR(8),					
	@Period INT,					
	@Times INT,					
	@RefDate CHAR(8),					
	@KobanTable KobanTableType readonly,					
	@DriverNaikinOnly TINYINT,
	@DelUkeNo NCHAR(15),
	@DelUnkRen SMALLINT,
	@DelTeiDanNo SMALLINT,
	@DelBunkRen SMALLINT
AS
BEGIN
	DECLARE @StartDate1 DATE,			
	@StartDate2 DATE;			
				
	SET			
	    @StartDate1 = dbo.FP_KaizenKijunStartDate(			
	        @RefDate,			
	        (			
	            SELECT			
	                MIN(UnkYmd)			
	            FROM			
	                @KobanTable			
	        ),			
	        @Period,			
	        @Times			
	    );			
				
	SET			
	    @StartDate2 = dbo.FP_KaizenKijunStartDate(			
	        @RefDate,			
	        (			
	            SELECT			
	                MAX(UnkYmd)			
	            FROM			
	                @KobanTable			
	        ),			
	        @Period,			
	        @Times			
	    );			
				
	--追加交番が2期間の間にある場合は、両方の期間のデータを取得する。			
	IF(			
	    @StartDate1 IS NOT NULL			
	    AND @StartDate1 <> @StartDate2			
	)			
	SET			
	    @Times = @Times + DATEDIFF(day, @StartDate1, @StartDate2) / @Period;			
				
	--追加交番データがない場合、運行日を対象日として取得する。			
	IF(@StartDate1 IS NULL)			
	SET			
	    @StartDate1 = dbo.FP_KaizenKijunStartDate(@RefDate, @UnkYmd, @Period, @Times);			
				
	-- 期間一覧取得			
	WITH PeriodTable AS (			
	    SELECT			
	        1 + Number AS Num,			
	        FORMAT(			
	            DATEADD(DAY, @Period * Number, @StartDate1),			
	            'yyyyMMdd'			
	        ) AS StaDate,			
	        FORMAT(			
	            DATEADD(DAY, @Period * (Number + 1) - 1, @StartDate1),			
	            'yyyyMMdd'			
	        ) AS EndDate			
	    FROM			
	        master.dbo.spt_values			
	    WHERE			
	        Type = 'P'			
	        AND Number BETWEEN 0			
	        AND @Times -1			
	)			
	SELECT			
	    TKD_Koban.SyainCdSeq,			
	    PeriodTable.StaDate,			
	    PeriodTable.EndDate,			
	    COUNT(*) AS LeaveCnt			
	FROM			
	    PeriodTable			
	    JOIN TKD_Koban ON TKD_Koban.UnkYmd BETWEEN PeriodTable.StaDate			
	    AND PeriodTable.EndDate			
	    JOIN VPM_KyoSHe AS eVPM_KyoSHe ON TKD_Koban.SyainCdSeq = eVPM_KyoSHe.SyainCdSeq			
	    AND @UnkYmd BETWEEN eVPM_KyoSHe.StaYmd			
	    AND eVPM_KyoSHe.EndYmd			
	    JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_Eigyos.EigyoCdSeq = eVPM_KyoSHe.EigyoCdSeq			
	    AND eVPM_Eigyos.CompanyCdSeq = @CompanyCdSeq			
	    JOIN VPM_Syokum ON VPM_Syokum.SyokumuCdSeq = eVPM_KyoSHe.SyokumuCdSeq			
	    AND (			
	        @DriverNaikinOnly = 0			
	        OR (			
	            @DriverNaikinOnly = 1			
	            AND VPM_Syokum.SyokumuKbn IN (1, 2, 5)			
	        )			
	    ) -- 運転手と内勤			
	    JOIN TKD_Kikyuj ON TKD_Kikyuj.KinKyuTblCdSeq = TKD_Koban.KinKyuTblCdSeq			
	    AND TKD_Kikyuj.SiyoKbn = 1			
	    JOIN VPM_KinKyu ON VPM_KinKyu.KinKyuCdSeq = TKD_Kikyuj.KinKyuCdSeq			
	    AND VPM_KinKyu.KinKyuKbn = 2 -- 休日			
	WHERE			
	    TKD_Koban.SyugyoKbn = 1			
	    AND TKD_Koban.SiyoKbn = 1			
	    AND LEN(TRIM(TKD_Koban.SyukinYmd)) = 8			
	    AND LEN(TRIM(TKD_Koban.SyukinTime)) = 4			
	    AND LEN(TRIM(TKD_Koban.TaikinYmd)) = 8			
	    AND LEN(TRIM(TKD_Koban.TaiknTime)) = 4			
	    AND DATEDIFF(			
	        Minute,			
	        CONCAT(			
	            TKD_Koban.SyukinYmd,			
	            ' ',			
	            STUFF(TKD_Koban.SyukinTime, 3, 0, ':')			
	        ),			
	        CONCAT(			
	            TKD_Koban.TaikinYmd,			
	            ' ',			
	            STUFF(TKD_Koban.TaiknTime, 3, 0, ':')			
	        )			
	    ) >= 1439 --24時間(00:00～23:59)			
	    AND (			
	        @CompanyCdSeq IS NULL			
	        OR eVPM_Eigyos.CompanyCdSeq = @CompanyCdSeq			
	    )			
	    AND (			
	        @SyainCdSeq = 0			
	        OR eVPM_KyoSHe.SyainCdSeq = @SyainCdSeq			
	    )			
	    AND (			
	        @DelUkeNo = ''		
	        OR @DelUnkRen = 0			
	        OR @DelTeiDanNo = 0			
	        OR @DelBunkRen = 0			
	        OR CONCAT(			
	            TKD_Koban.UkeNo,			
	            '_',			
	            TKD_Koban.UnkRen,			
	            '_',			
	            TKD_Koban.TeiDanNo,			
	            '_',			
	            TKD_Koban.BunkRen			
	        ) <> CONCAT(			
	            @DelUkeNo,			
	            '_',			
	            @DelUnkRen,			
	            '_',			
	            @DelTeiDanNo,			
	            '_',			
	            @DelBunkRen			
	        )			
	    )			
	GROUP BY			
	    PeriodTable.StaDate,			
	    PeriodTable.EndDate,			
	    TKD_Koban.SyainCdSeq			
	ORDER BY			
	    TKD_Koban.SyainCdSeq,			
	    PeriodTable.StaDate,			
	    PeriodTable.EndDate					
END
GO
