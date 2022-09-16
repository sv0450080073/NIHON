USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetListRestraintTime_R]    Script Date: 6/2/2021 2:45:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetListRestraintTime_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List RestraintTime
-- Date			:   2021/04/12
-- Author		:   P.M.Nhat
-- Description	:   Get list restraint-time with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dGetListRestraintTime_R]
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
	    @Times = @Times + DATEDIFF(DAY, @StartDate1, @StartDate2) / @Period;		
			
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
	        MASTER.dbo.spt_values		
	    WHERE		
	        TYPE = 'P'		
	        AND Number BETWEEN 0		
	        AND @Times -1		
	),		
	-- 拘束時間取得		
	eTKD_Koban01 AS (		
	    SELECT		
	        eTKD_Koban.SyainCdSeq AS SyainCdSeq,		
	        eTKD_Koban.UnkYmd,		
	        MIN(		
	            CONCAT(eTKD_Koban.SyukinYmd, eTKD_Koban.SyukinTime)		
	        ) AS SyukinYmdTime,		
	        MAX(		
	            CONCAT(eTKD_Koban.TaikinYmd, eTKD_Koban.TaiknTime)		
	        ) AS TaiknYmdTime,		
	        DATEDIFF(		
	            MINUTE,		
	            MIN(		
	                CONCAT(		
	                    eTKD_Koban.SyukinYmd,		
	                    ' ',		
	                    STUFF(eTKD_Koban.SyukinTime, 3, 0, ':')		
	                )		
	            ),		
	            MAX(		
	                CONCAT(		
	                    eTKD_Koban.TaikinYmd,		
	                    ' ',		
	                    STUFF(TaiknTime, 3, 0, ':')		
	                )		
	            )		
	        ) AS KousokuMinute,		
	        STRING_AGG(eTKD_Koban.DrvJin, ',') AS DrvJin		
	    FROM		
	        (		
	            SELECT		
	                TKD_Koban.SyainCdSeq,		
	                TKD_Koban.UnkYmd,		
	                TKD_Koban.SyukinYmd,		
	                TKD_Koban.SyukinTime,		
	                TKD_Koban.TaikinYmd,		
	                TKD_Koban.TaiknTime,		
	                TKD_Haisha.DrvJin		
	            FROM		
	                TKD_Koban		
	                JOIN VPM_KyoSHe AS eVPM_KyoSHe ON TKD_Koban.SyainCdSeq = eVPM_KyoSHe.SyainCdSeq		
	                AND @UnkYmd BETWEEN eVPM_KyoSHe.StaYmd		
	                AND eVPM_KyoSHe.EndYmd		
	                JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_Eigyos.EigyoCdSeq = eVPM_KyoSHe.EigyoCdSeq		
	                JOIN VPM_Syokum ON VPM_Syokum.SyokumuCdSeq = eVPM_KyoSHe.SyokumuCdSeq		
	                AND (		
	                    @DriverNaikinOnly = 0		
	                    OR (		
	                        @DriverNaikinOnly = 1		
	                        AND VPM_Syokum.SyokumuKbn IN (1, 2, 5)		
	                    )		
	                ) -- 運転手と内勤		
	                LEFT JOIN TKD_Kikyuj ON TKD_Kikyuj.KinKyuTblCdSeq = TKD_Koban.KinKyuTblCdSeq		
	                AND TKD_Kikyuj.SiyoKbn = 1		
	                LEFT JOIN VPM_KinKyu ON VPM_KinKyu.KinKyuCdSeq = TKD_Kikyuj.KinKyuCdSeq		
	                LEFT JOIN TKD_Haisha ON TKD_Haisha.UkeNo = TKD_Koban.UkeNo		
	                AND TKD_Haisha.UnkRen = TKD_Koban.UnkRen		
	                AND TKD_Haisha.TeiDanNo = TKD_Koban.TeiDanNo		
	                AND TKD_Haisha.BunkRen = TKD_Koban.BunkRen		
	            WHERE		
	                TKD_Koban.SyugyoKbn = 1		
	                AND TKD_Koban.SiyoKbn = 1		
	                AND (		
	                    (		
	                        TKD_Koban.KinKyuTblCdSeq <> 0		
	                        AND VPM_KinKyu.KinKyuKbn = 1 -- 勤務		
	                    )		
	                    OR (LEN(TRIM(TKD_Koban.UkeNo)) = 15)		
	                )		
	                AND LEN(TRIM(TKD_Koban.SyukinYmd)) = 8		
	                AND LEN(TRIM(TKD_Koban.SyukinTime)) = 4		
	                AND LEN(TRIM(TKD_Koban.TaikinYmd)) = 8		
	                AND LEN(TRIM(TKD_Koban.TaiknTime)) = 4		
	                AND TKD_Koban.UnkYmd BETWEEN FORMAT(@StartDate1, 'yyyyMMdd')		
	                AND FORMAT(		
	                    DATEADD(DAY, @Period * @Times + 1, @StartDate1),		
	                    'yyyyMMdd'		
	                )		
	                AND (		
	                    @CompanyCdSeq IS NULL		
	                    OR eVPM_Eigyos.CompanyCdSeq = @CompanyCdSeq		
	                )		
	                AND (		
	                    @SyainCdSeq = 0	
	                    OR TKD_Koban.SyainCdSeq = @SyainCdSeq		
	                )		
	                AND (		
	                    @DelUkeNo = ''		
	                    OR @DelUnkRen = 0		
	                    OR @DelTeiDanNo  = 0		
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
	            UNION		
	            ALL		
	            SELECT		
	                VPM_KyoSHe.SyainCdSeq,		
	                eKobanTable.UnkYmd,		
	                eKobanTable.SyukinYmd,		
	                eKobanTable.SyukinTime,		
	                eKobanTable.TaikinYmd,		
	                eKobanTable.TaiknTime,		
	                TKD_Haisha.DrvJin		
	            FROM		
	                VPM_KyoSHe		
	                JOIN @KobanTable AS eKobanTable ON @UnkYmd BETWEEN VPM_KyoSHe.StaYmd		
	                AND VPM_KyoSHe.EndYmd		
	                AND eKobanTable.UnkYmd BETWEEN FORMAT(@StartDate1, 'yyyyMMdd')		
	                AND FORMAT(		
	                    DATEADD(DAY, @Period * @Times + 1, @StartDate1),		
	                    'yyyyMMdd'		
	                )		
	                JOIN VPM_Eigyos ON VPM_Eigyos.EigyoCdSeq = VPM_KyoSHe.EigyoCdSeq		
	                JOIN VPM_Syokum ON VPM_Syokum.SyokumuCdSeq = VPM_KyoSHe.SyokumuCdSeq		
	                AND (		
	                    @DriverNaikinOnly = 0		
	                    OR (		
	                        @DriverNaikinOnly = 1		
	                        AND VPM_Syokum.SyokumuKbn IN (1, 2, 5)		
	                    )		
	                ) -- 運転手と内勤		
	                LEFT JOIN TKD_Haisha ON TKD_Haisha.UkeNo = eKobanTable.UkeNo		
	                AND TKD_Haisha.UnkRen = eKobanTable.UnkRen		
	                AND TKD_Haisha.TeiDanNo = eKobanTable.TeiDanNo		
	                AND TKD_Haisha.BunkRen = eKobanTable.BunkRen		
	            WHERE		
	                (		
	                    @CompanyCdSeq IS NULL		
	                    OR VPM_Eigyos.CompanyCdSeq = @CompanyCdSeq		
	                )		
	                AND (		
	                    @SyainCdSeq = 0	
	                    OR VPM_KyoSHe.SyainCdSeq = @SyainCdSeq		
	                )		
	                AND LEN(TRIM(eKobanTable.SyukinYmd)) = 8
                	AND LEN(TRIM(eKobanTable.SyukinTime)) = 4
                	AND LEN(TRIM(eKobanTable.TaikinYmd)) = 8
                	AND LEN(TRIM(eKobanTable.TaiknTime)) = 4
	        ) AS eTKD_Koban		
	    GROUP BY		
	        eTKD_Koban.SyainCdSeq,		
	        eTKD_Koban.UnkYmd		
	),		
	-- 休息期間取得		
	eTKD_Koban02 AS (		
	    SELECT		
	        TKD_Koban.SyainCdSeq AS SyainCdSeq,		
	        TKD_Koban.UnkYmd,		
	        DATEDIFF(		
	            MINUTE,		
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
	        ) AS KyusokuMinute		
	    FROM		
	        TKD_Koban		
	        JOIN eTKD_Koban01 ON eTKD_Koban01.SyainCdSeq = TKD_Koban.SyainCdSeq		
	        AND eTKD_Koban01.UnkYmd = TKD_Koban.UnkYmd		
	        AND eTKD_Koban01.SyukinYmdTime <= CONCAT(TKD_Koban.SyukinYmd, TKD_Koban.SyukinTime)		
	        AND eTKD_Koban01.TaiknYmdTime >= CONCAT(TKD_Koban.TaikinYmd, TKD_Koban.TaiknTime)		
	        LEFT JOIN TKD_Kikyuj ON TKD_Kikyuj.KinKyuTblCdSeq = TKD_Koban.KinKyuTblCdSeq		
	        AND TKD_Kikyuj.SiyoKbn = 1		
	        LEFT JOIN VPM_KinKyu ON VPM_KinKyu.KinKyuCdSeq = TKD_Kikyuj.KinKyuCdSeq		
	    WHERE		
	        TKD_Koban.SyugyoKbn = 1		
	        AND TKD_Koban.SiyoKbn = 1		
	        AND TKD_Koban.KinKyuTblCdSeq <> 0		
	        AND VPM_KinKyu.KinKyuKbn = 3 -- 分割休息期間		
	        AND LEN(TRIM(TKD_Koban.SyukinYmd)) = 8		
	        AND LEN(TRIM(TKD_Koban.SyukinTime)) = 4		
	        AND LEN(TRIM(TKD_Koban.TaikinYmd)) = 8		
	        AND LEN(TRIM(TKD_Koban.TaiknTime)) = 4		
	),		
	-- 休息期間運行日小計取得		
	eTKD_Koban03 AS (		
	    SELECT		
	        SyainCdSeq,		
	        UnkYmd,		
	        SUM(KyusokuMinute) AS KyusokuMinute		
	    FROM		
	        eTKD_Koban02		
	    GROUP BY		
	        SyainCdSeq,		
	        UnkYmd		
	),		
	--24時間内の追加拘束時間		
	eTKD_Koban04 AS (
    SELECT
        eTKD_Koban011.SyainCdSeq,
        eTKD_Koban011.UnkYmd,
        CASE
            WHEN RIGHT(eTKD_Koban012.TaiknYmdTime, 4) >= RIGHT(eTKD_Koban011.SyukinYmdTime, 4) 
                THEN DATEDIFF(
                    MINUTE,
                    STUFF(
                        STUFF(eTKD_Koban012.SyukinYmdTime, 9, 0, ' '),
                        12,
                        0,
                        ':'
                    ),
                    DATEADD(
                        DAY,
                        1,
                        STUFF(
                            STUFF(eTKD_Koban011.SyukinYmdTime, 9, 0, ' '),
                            12,
                            0,
                            ':'
                        )
                    )
                )
            ELSE DATEDIFF(
                MINUTE,
                STUFF(
                    STUFF(eTKD_Koban012.SyukinYmdTime, 9, 0, ' '),
                    12,
                    0,
                    ':'
                ),
                STUFF(
                    STUFF(eTKD_Koban012.TaiknYmdTime, 9, 0, ' '),
                    12,
                    0,
                    ':'
                )
            )
        END AS KousokuMinute
    FROM
        eTKD_Koban01 AS eTKD_Koban011
        JOIN eTKD_Koban01 AS eTKD_Koban012 ON eTKD_Koban012.SyainCdSeq = eTKD_Koban011.SyainCdSeq
        AND eTKD_Koban012.UnkYmd BETWEEN eTKD_Koban011.UnkYmd
        AND FORMAT(
            DATEADD(DAY, 1, eTKD_Koban011.UnkYmd),
            'yyyyMMdd'
        )
        AND LEFT(eTKD_Koban012.SyukinYmdTime, 8) = FORMAT(
            DATEADD(DAY, 1, LEFT(eTKD_Koban011.SyukinYmdTime, 8)),
            'yyyyMMdd'
        )
        AND RIGHT(eTKD_Koban012.SyukinYmdTime, 4) < RIGHT(eTKD_Koban011.SyukinYmdTime, 4)
	)
	SELECT		
	    eTKD_Koban01.SyainCdSeq AS SyainCdSeq, -- 社員コードSEQ		
	    MAX(PeriodTable.Num) AS Num, --期間目		
	    PeriodTable.StaDate AS StaDate, -- 期間開始日		
	    PeriodTable.EndDate AS EndDate, -- 期間終了日		
	    dbo.FP_DistinctStringAgg(ISNULL(STRING_AGG(eTKD_Koban01.DrvJin, ','), '')) AS DrvJin, --運転手数配列		
	    CASE		
	        WHEN @Period = 1 THEN SUM(		
	            eTKD_Koban01.KousokuMinute - ISNULL(eTKD_Koban03.KyusokuMinute, 0) + ISNULL(eTKD_Koban04.KousokuMinute, 0)		
	        )		
	        ELSE SUM(		
	            eTKD_Koban01.KousokuMinute - ISNULL(eTKD_Koban03.KyusokuMinute, 0)		
	        )		
	    END AS KousokuMinute -- 拘束時間（分）		
	FROM		
	    PeriodTable		
	    JOIN eTKD_Koban01 ON eTKD_Koban01.UnkYmd BETWEEN PeriodTable.StaDate		
	    AND PeriodTable.EndDate		
	    LEFT JOIN eTKD_Koban03 ON eTKD_Koban03.UnkYmd = eTKD_Koban01.UnkYmd		
	    AND eTKD_Koban03.SyainCdSeq = eTKD_Koban01.SyainCdSeq		
	    LEFT JOIN eTKD_Koban04 ON eTKD_Koban04.UnkYmd = eTKD_Koban01.UnkYmd		
	    AND eTKD_Koban04.SyainCdSeq = eTKD_Koban01.SyainCdSeq		
	GROUP BY		
	    PeriodTable.StaDate,		
	    PeriodTable.EndDate,		
	    eTKD_Koban01.SyainCdSeq		
	ORDER BY		
	    eTKD_Koban01.SyainCdSeq,		
	    PeriodTable.StaDate,		
	    PeriodTable.EndDate			
END
