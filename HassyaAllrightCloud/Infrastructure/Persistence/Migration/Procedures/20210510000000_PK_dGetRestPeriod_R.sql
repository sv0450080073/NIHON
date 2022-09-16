USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetRestPeriod_R]    Script Date: 5/10/2021 9:41:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetRestPeriod_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List RestPeriod
-- Date			:   2021/04/12
-- Author		:   P.M.Nhat
-- Description	:   Get list rest-period with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dGetRestPeriod_R]
	-- Add the parameters for the stored procedure here
	@UnkYmd CHAR(8),										
	@CompanyCdSeq INT,										
	@SyainCdSeq INT,										
	@DriverNaikinOnly TINYINT,										
	@KobanTable KobanTableType readonly,
	@DelUkeNo NCHAR(15),
	@DelUnkRen SMALLINT,
	@DelTeiDanNo SMALLINT,
	@DelBunkRen SMALLINT
AS
BEGIN
	DECLARE @StaYmd CHAR(8),		
	@EndYmd CHAR(8);		
			
	SET		
	    @StaYmd = FORMAT(		
	        DATEADD(		
	            DAY,		
	            -1,		
	            (		
	                SELECT		
	                    MIN(UnkYmd)		
	                FROM		
	                    @KobanTable		
	            )		
	        ),		
	        'yyyyMMdd'		
	    );		
			
	SET		
	    @EndYmd = (		
	        SELECT		
	            MAX(UnkYmd)		
	        FROM		
	            @KobanTable		
	    );		
			
	IF(@StaYmd IS NULL) BEGIN		
	SET		
	    @StaYmd = @UnkYmd;		
			
	SET		
	    @EndYmd = @UnkYmd;		
			
	END;		
			
	WITH eTKD_Koban01 AS (		
	    SELECT		
	        eTKD_Koban.SyainCdSeq,		
	        eTKD_Koban.UnkYmd,		
	        LEFT(		
	            MIN(		
	                CONCAT(eTKD_Koban.SyukinYmd, eTKD_Koban.SyukinTime)		
	            ),		
	            8		
	        ) AS SyukinYmd,		
	        RIGHT(		
	            MIN(		
	                CONCAT(eTKD_Koban.SyukinYmd, eTKD_Koban.SyukinTime)		
	            ),		
	            4		
	        ) AS SyukinTime,		
	        LEFT(		
	            MAX(		
	                CONCAT(eTKD_Koban.TaikinYmd, eTKD_Koban.TaiknTime)		
	            ),		
	            8		
	        ) AS TaikinYmd,		
	        RIGHT(		
	            MAX(		
	                CONCAT(eTKD_Koban.TaikinYmd, eTKD_Koban.TaiknTime)		
	            ),		
	            4		
	        ) AS TaiknTime,		
	        dbo.FP_DistinctStringAgg(ISNULL(STRING_AGG(eTKD_Koban.DrvJin, ','), '')) AS DrvJin --運転手数配列		
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
	                JOIN VPM_KyoSHe ON TKD_Koban.SyainCdSeq = VPM_KyoSHe.SyainCdSeq		
	                AND @UnkYmd BETWEEN VPM_KyoSHe.StaYmd		
	                AND VPM_KyoSHe.EndYmd		
	                JOIN VPM_Eigyos ON VPM_Eigyos.EigyoCdSeq = VPM_KyoSHe.EigyoCdSeq		
	                AND VPM_Eigyos.CompanyCdSeq = @CompanyCdSeq		
	                JOIN VPM_Syokum ON VPM_Syokum.SyokumuCdSeq = VPM_KyoSHe.SyokumuCdSeq		
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
	                AND TKD_Koban.UnkYmd BETWEEN @StaYmd		
	                AND FORMAT(DATEADD(DAY, 31, @EndYmd), 'yyyyMMdd') -- 1ヶ月以内		
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
	                AND (		
	                    @CompanyCdSeq IS NULL		
	                    OR VPM_Eigyos.CompanyCdSeq = @CompanyCdSeq		
	                )		
	                AND (		
	                    @SyainCdSeq = 0		
	                    OR TKD_Koban.SyainCdSeq = @SyainCdSeq		
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
	                JOIN VPM_Eigyos ON VPM_Eigyos.EigyoCdSeq = VPM_KyoSHe.EigyoCdSeq		
	                AND VPM_Eigyos.CompanyCdSeq = @CompanyCdSeq		
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
	-- 分割休息期間の取得		
	eTKD_Koban02 AS (		
	    SELECT		
	        TKD_Koban.SyainCdSeq AS SyainCdSeq,		
	        TKD_Koban.UnkYmd,		
	        DATEDIFF(		
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
	        ) AS KyusokuMinute		
	    FROM		
	        TKD_Koban		
	        JOIN eTKD_Koban01 ON eTKD_Koban01.SyainCdSeq = TKD_Koban.SyainCdSeq		
	        AND eTKD_Koban01.UnkYmd = TKD_Koban.UnkYmd	
			AND eTKD_Koban01.SyukinYmd = TKD_Koban.SyukinYmd
	        AND eTKD_Koban01.SyukinTime <= TKD_Koban.SyukinTime
			AND eTKD_Koban01.TaikinYmd = TKD_Koban.TaikinYmd
	        AND eTKD_Koban01.TaiknTime >= TKD_Koban.TaiknTime	
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
	-- 運行日により分割休息期間小計取得		
	eTKD_Koban03 AS (		
	    SELECT		
	        SyainCdSeq,		
	        UnkYmd,		
	        COUNT(KyusokuMinute) AS KyusokuCnt,		
	        SUM(KyusokuMinute) AS KyusokuMinute		
	    FROM		
	        eTKD_Koban02		
	    GROUP BY		
	        SyainCdSeq,		
	        UnkYmd		
	)		
	SELECT		
	    eTKD_Koban10.SyainCdSeq AS SyainCdSeq,		
	    eTKD_Koban10.UnkYmd AS UnkYmd,		
	    eTKD_Koban10.DrvJin AS DrvJin,		
	    ISNULL(eTKD_Koban13.KyusokuCnt, 0) AS KyusokuCnt,		
	    DATEDIFF(		
	        Minute,		
	        CONCAT(		
	            eTKD_Koban10.TaikinYmd,		
	            ' ',		
	            STUFF(eTKD_Koban10.TaiknTime, 3, 0, ':')		
	        ),		
	        CONCAT(		
	            eTKD_Koban11.SyukinYmd,		
	            ' ',		
	            STUFF(eTKD_Koban11.SyukinTime, 3, 0, ':')		
	        )		
	    ) + ISNULL(eTKD_Koban13.KyusokuMinute, 0) AS KyusokuMinute		
	FROM		
	    eTKD_Koban01 AS eTKD_Koban10		
	    CROSS APPLY (		
	        SELECT		
	            TOP 1 *		
	        FROM		
	            eTKD_Koban01		
	        WHERE		
	            SyainCdSeq = eTKD_Koban10.SyainCdSeq		
	            AND UnkYmd > eTKD_Koban10.UnkYmd		
	    ) AS eTKD_Koban11		
	    LEFT JOIN eTKD_Koban03 AS eTKD_Koban13 ON eTKD_Koban13.SyainCdSeq = eTKD_Koban10.SyainCdSeq		
	    AND eTKD_Koban13.UnkYmd = eTKD_Koban10.UnkYmd		
	WHERE		
	    eTKD_Koban10.UnkYmd BETWEEN @StaYmd		
	    AND @EndYmd						
END
