USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dVehicleAllocations_R]    Script Date: 5/27/2021 9:23:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dVehicleAllcation_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get VehicleAllocation List
-- Date			:   2021/04/12
-- Author		:   P.M.Nhat
-- Description	:   Get VehicleAllocation list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dVehicleAllocations_R]
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int,
	@UnkYmd varchar(8)
AS
BEGIN
	SELECT				
	    eTKD_Haisha.UkeNo AS UkeNo, --受付番号				
	    eTKD_Haisha.UnkRen AS UnkRen, --運行日連番				
	    eTKD_Haisha.TeiDanNo AS TeiDanNo, --悌団番号				
	    eTKD_Haisha.BunkRen AS BunkRen, --分割連番				
	    eTKD_Haisha.SyuKoYmd AS SyuKoYmd, --出庫年月日				
	    eTKD_Haisha.SyuKoTime AS SyuKoTime, --出庫時間				
	    eTKD_Haisha.HaiSYmd AS HaiSYmd, --配車年月日				
	    eTKD_Haisha.HaiSTime AS HaiSTime, --配車時間				
	    eTKD_Haisha.KikYmd AS KikYmd, --帰庫年月日				
	    eTKD_Haisha.KikTime AS KikTime, --帰庫時間				
	    eTKD_Haisha.TouYmd AS TouYmd, --到着年月日				
	    eTKD_Haisha.TouChTime AS TouChTime, --到着時間				
	    eTKD_Haisha.HaiSNm AS HaiSNm, --配車地名				
	    eTKD_Haisha.TouNm AS TouNm, --到着地名				
	    eTKD_Haisha.DrvJin AS DrvJin, --運転手数				
	    eTKD_Haisha.GuiSu AS GuiSu, --ガイド数				
	    eTKD_Haisha.KSKbn AS KSKbn, --仮車区分				
	    eTKD_Haisha.HaiSKbn AS HaiSKbn, --配車区分				
	    eTKD_Haisha.HaiSSryCdSeq AS HaiSSryCdSeq, --配車車輌コードＳＥＱ				
	    eTKD_Haisha.SyaSyuRen AS SyaSyuRen, --車種連番				
	    eTKD_Unkobi.DanTaNm AS DanTaNm, --団体名				
	    ISNULL(eTKD_Haiin.SyainCdSeq, 0) AS SyainCdSeq, --社員コードＳＥＱ				
	    ISNULL(eTKD_Haiin.HaiInRen, 0) AS HaiInRen, --配員連番				
	    ISNULL(eTKD_Haiin.SyukinTime, '') AS SyukinTime, --出勤時間				
	    ISNULL(eTKD_Haiin.TaiknTime, '') AS TaiknTime, --退勤時間				
	    ISNULL(eTKD_Haiin.Syukinbasy, '') AS Syukinbasy, --出勤場所				
	    ISNULL(eTKD_Haiin.TaiknBasy, '') AS TaiknBasy, --退勤場所				
	    ISNULL(eTKD_Haiin.RyaSyokumuKbn, 0) AS SyokumuKbn, --配員職務				
	    eTKD_Unkobi.UnkoJKbn AS UnkoJKbn, --運行状態区分				
	    eTKD_Yyksho.KaknKais AS KaknKais, --確認回数				
	    eTKD_Yyksho.KaktYmd AS KaktYmd, --確定年月日				
	    CASE				
	        WHEN eTKD_Koteik.SyukoTime IS NOT NULL THEN eTKD_Koteik.SyukoTime				
	        WHEN eTKD_Koteik.SyukoTime IS NULL				
	        AND eTKD_Haisha.SyuKoYmd = @UnkYmd THEN eTKD_Haisha.SyuKoTime				
	        WHEN eTKD_Koteik.SyukoTime IS NULL				
	        AND eTKD_Haisha.SyuKoYmd < @UnkYmd THEN '0000'				
	        ELSE ''				
	    END AS KoteiSyukoTime, --行程出庫時間				
	    CASE				
	        WHEN eTKD_Koteik.KikTime IS NOT NULL THEN eTKD_Koteik.KikTime				
	        WHEN eTKD_Koteik.KikTime IS NULL				
	        AND eTKD_Haisha.KikYmd = @UnkYmd THEN eTKD_Haisha.KikTime				
	        WHEN eTKD_Koteik.KikTime IS NULL				
	        AND eTKD_Haisha.KikYmd > @UnkYmd THEN '2359'				
	        ELSE ''				
	    END AS KoteiKikTime, --行程帰庫時間
	    (
        	SELECT
            	CONCAT(UpdYmd, UpdTime)
        	FROM
            	TKD_Haisha
        	WHERE
            	UkeNo = eTKD_Haisha.UkeNo
            	AND UnkRen = eTKD_Haisha.UnkRen
            	AND TeiDanNo = eTKD_Haisha.TeiDanNo
            	AND BunkRen = eTKD_Haisha.BunkRen
    	) AS HaishaUpdYmdTime,
    	ISNULL((
        	SELECT
            	MAX(CONCAT(UpdYmd, UpdTime))
        	FROM
            	TKD_Haiin
        	WHERE
            	UkeNo = eTKD_Haisha.UkeNo
            	AND UnkRen = eTKD_Haisha.UnkRen
            	AND TeiDanNo = eTKD_Haisha.TeiDanNo
            	AND BunkRen = eTKD_Haisha.BunkRen
    	), '') AS HaiinUpdYmdTime
	FROM				
	    (				
	        SELECT				
	            *				
	        FROM				
	            dbo.TKD_Haisha				
	        WHERE				
	            SiyoKbn = 1				
	            AND KSKbn <> 1 --未仮車以外				
	            AND YouTblSeq = 0 --傭車以外				
	    ) AS eTKD_Haisha				
	    JOIN dbo.TKD_Yyksho AS eTKD_Yyksho ON eTKD_Yyksho.UkeNo = eTKD_Haisha.UkeNo	
		AND eTKD_Yyksho.YoyaSyu = 1
	    AND eTKD_Yyksho.SiyoKbn = 1				
	    JOIN TKD_Unkobi AS eTKD_Unkobi ON eTKD_Unkobi.SiyoKbn = 1				
	    AND eTKD_Unkobi.UkeNo = eTKD_Haisha.UkeNo				
	    AND eTKD_Unkobi.UnkRen = eTKD_Haisha.UnkRen				
	    JOIN TKD_YykSyu AS eTKD_YykSyu01 ON eTKD_YykSyu01.SiyoKbn = 1				
	    AND eTKD_YykSyu01.UkeNo = eTKD_Haisha.UkeNo				
	    AND eTKD_YykSyu01.UnkRen = eTKD_Haisha.UnkRen				
	    AND eTKD_YykSyu01.SyaSyuRen = eTKD_Haisha.SyaSyuRen				
	    JOIN (				
	        SELECT				
	            eTKD_YykSyu00.UkeNo,				
	            eTKD_YykSyu00.UnkRen,				
	            SUM(eTKD_YykSyu00.SyaSyuDai) AS SumSyaSyuDai				
	        FROM				
	            TKD_YykSyu AS eTKD_YykSyu00				
	        WHERE				
	            eTKD_YykSyu00.SiyoKbn = 1				
	        GROUP BY				
	            eTKD_YykSyu00.UkeNo,				
	            eTKD_YykSyu00.UnkRen				
	    ) AS eTKD_YykSyu02 ON eTKD_YykSyu02.UkeNo = eTKD_Haisha.UkeNo				
	    AND eTKD_YykSyu02.UnkRen = eTKD_Haisha.UnkRen				
	    LEFT JOIN (				
	        SELECT				
	            TKD_Haiin.*,				
	            CASE				
	                WHEN TKD_Haiin.SyokumuKbn IN (1, 2) THEN 1				
	                WHEN TKD_Haiin.SyokumuKbn IN (3, 4) THEN 3				
	                WHEN TKD_Haiin.SyokumuKbn = 0				
	                AND VPM_Syokum.SyokumuKbn IN (1, 2) THEN 1				
	                WHEN TKD_Haiin.SyokumuKbn = 0				
	                AND VPM_Syokum.SyokumuKbn IN (3, 4) THEN 3				
	                ELSE 0				
	            END AS RyaSyokumuKbn				
	        FROM				
	            TKD_Haiin				
	            LEFT JOIN VPM_KyoSHe ON VPM_KyoSHe.SyainCdSeq = TKD_Haiin.SyainCdSeq				
	            AND @UnkYmd BETWEEN VPM_KyoSHe.StaYmd				
	            AND VPM_KyoSHe.EndYmd				
	            LEFT JOIN VPM_Syokum ON VPM_Syokum.SyokumuCdSeq = VPM_KyoSHe.SyokumuCdSeq				
	        WHERE				
	            TKD_Haiin.SiyoKbn = 1				
	            AND (				
	                TKD_Haiin.SyokumuKbn IN (1, 2, 3, 4)				
	                OR (				
	                    TKD_Haiin.SyokumuKbn = 0				
	                    AND VPM_Syokum.SyokumuKbn IN (1, 2, 3, 4)				
	                )				
	            )				
	    ) AS eTKD_Haiin ON eTKD_Haiin.UkeNo = eTKD_Haisha.UkeNo				
	    AND eTKD_Haiin.UnkRen = eTKD_Haisha.UnkRen				
	    AND eTKD_Haiin.TeiDanNo = eTKD_Haisha.TeiDanNo				
	    AND eTKD_Haiin.BunkRen = eTKD_Haisha.BunkRen				
	    LEFT JOIN TKD_Koteik AS eTKD_Koteik ON eTKD_Koteik.UkeNo = eTKD_Haisha.UkeNo				
	    AND eTKD_Koteik.UnkRen = eTKD_Haisha.UnkRen				
	    AND eTKD_Koteik.TeiDanNo = eTKD_Haisha.TeiDanNo				
	    AND eTKD_Koteik.BunkRen = eTKD_Haisha.BunkRen				
	    AND eTKD_Koteik.Nittei = (				
	        DATEDIFF(				
	            DAY,
				eTKD_Haisha.SyuKoYmd,	
	            @UnkYmd	
	        ) + 1				
	    )				
	    AND eTKD_Koteik.TomKbn = 1				
	WHERE				
	    @UnkYmd BETWEEN eTKD_Haisha.SyuKoYmd				
	    AND eTKD_Haisha.KikYmd				
	    AND eTKD_Yyksho.TenantCdSeq = @TenantCdSeq								
END
