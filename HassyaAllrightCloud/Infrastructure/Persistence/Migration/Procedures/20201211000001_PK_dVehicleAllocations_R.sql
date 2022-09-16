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
-- SP-ID		:   PK_dVehicleAllocations_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get VehicleAllocation List
-- Date			:   2020/12/11
-- Author		:   P.M.Nhat
-- Description	:   Get VehicleAllocation list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dVehicleAllocations_R
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
    		eTKD_Haisha.HaiSSryCdSeq AS HaiSSryCdSeq, --配車車輌コードＳＥＱ
    		eTKD_Haisha.SyaSyuRen AS SyaSyuRen, --車種連番
    		eTKD_Unkobi.DanTaNm AS DanTaNm, --団体名
    		ISNULL(eTKD_Haiin.SyainCdSeq, 0) AS SyainCdSeq, --社員コードＳＥＱ
    		ISNULL(eTKD_Haiin.HaiInRen, 0) AS HaiInRen, --配員連番
    		ISNULL(eTKD_Haiin.SyukinTime, ' ') AS SyukinTime, --出勤時間
    		ISNULL(eTKD_Haiin.TaiknTime, ' ') AS TaiknTime, --退勤時間
    		ISNULL(eTKD_Haiin.Syukinbasy, ' ') AS Syukinbasy, --出勤場所
    		ISNULL(eTKD_Haiin.TaiknBasy, ' ') AS TaiknBasy, --退勤場所
    		eTKD_Unkobi.UnkoJKbn AS UnkoJKbn, --運行状態区分
    		eTKD_Yyksho.KaknKais AS KaknKais, --確認回数
    		eTKD_Yyksho.KaktYmd AS KaktYmd, --確定年月日
    		eVPM_Eigyos.EigyoCdSeq AS EigyoCdSeq, --営業所コードＳＥＱ
    		ISNULL(eTKD_Koteik.SyukoTime, '') AS Kotei_SyukoTime, --行程出庫時間
    		ISNULL(eTKD_Koteik.KikTime, '') AS Kotei_KikTime, --行程帰庫時間
			eTKD_Haisha.HaiSKbn as HaiSKbn,
			eTKD_Haisha.KSKbn as KSKbn
	FROM
    		(
        		SELECT
            		*
        		FROM
            		dbo.TKD_Haisha
        		WHERE
            		TKD_Haisha.SiyoKbn = 1
    		) AS eTKD_Haisha
    		JOIN dbo.TKD_Yyksho AS eTKD_Yyksho ON eTKD_Yyksho.UkeNo = eTKD_Haisha.UkeNo
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
    		LEFT JOIN TKD_Haiin AS eTKD_Haiin ON eTKD_Haiin.UkeNo = eTKD_Haisha.UkeNo
    		AND eTKD_Haiin.UnkRen = eTKD_Haisha.UnkRen
    		AND eTKD_Haiin.TeiDanNo = eTKD_Haisha.TeiDanNo
    		AND eTKD_Haiin.BunkRen = eTKD_Haisha.BunkRen
    		AND eTKD_Haiin.SiyoKbn = 1
			LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe ON eVPM_KyoSHe.SyainCdSeq = eTKD_Haiin.SyainCdSeq
    		AND eTKD_Haisha.HaiSYmd BETWEEN eVPM_KyoSHe.StaYmd AND eVPM_KyoSHe.EndYmd
    		LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_Eigyos.EigyoCdSeq = eVPM_KyoSHe.EigyoCdSeq
    		LEFT JOIN TKD_Koteik AS eTKD_Koteik ON eTKD_Koteik.UkeNo = eTKD_Haisha.UkeNo
    		AND eTKD_Koteik.UnkRen = eTKD_Haisha.UnkRen
    		AND eTKD_Koteik.TeiDanNo = eTKD_Haisha.TeiDanNo
    		AND eTKD_Koteik.BunkRen = eTKD_Haisha.BunkRen
    		AND eTKD_Koteik.Nittei = (
        		DATEDIFF(
            		DAY,
            		CONVERT(datetime, @UnkYmd, 112),
            		CONVERT(datetime, eTKD_Haisha.SyuKoYmd, 112)
        		) + 1
    		)
    		AND eTKD_Koteik.TomKbn = 1
	WHERE
    		@UnkYmd BETWEEN eTKD_Haisha.SyuKoYmd
    		AND eTKD_Haisha.KikYmd
    		AND eTKD_Yyksho.TenantCdSeq = @TenantCdSeq 
END
GO
