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
-- System-Name	:   HassyaAllrightCloud
-- Module-Name	:   HassyaAllrightCloud
-- SP-ID		:   PK_dDailyBatchCopyList_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get daily batch copy list
-- Date			:   2020/10/14
-- Author		:   P.M.NHAT
-- Description	:   Get daily batch copy list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dDailyBatchCopyList_R
	-- Add the parameters for the stored procedure here
	@UkeNo varchar(max),
	@TenantCdSeq int
AS
BEGIN
	WITH eTKD_YykSyu01
	AS (
        SELECT TKD_YykSyu.UkeNo
                ,SUM(TKD_YykSyu.SyaSyuDai) AS SumSyaSyuDai
                ,SUM(TKD_YykSyu.SyaSyuTan) AS SumSyaSyuTan
                ,SUM(TKD_YykSyu.SyaRyoUnc) AS SumSyaRyoUnc
                ,SUM(TKD_YykSyu.DriverNum) AS SumDriverNum
                ,SUM(TKD_YykSyu.UnitBusPrice) AS SumUnitBusPrice
                ,SUM(TKD_YykSyu.UnitBusFee) AS SumUnitBusFee
                ,SUM(TKD_YykSyu.GuiderNum) AS SumGuiderNum
                ,SUM(TKD_YykSyu.UnitGuiderPrice) AS SumUnitGuiderPrice
                ,SUM(TKD_YykSyu.UnitGuiderFee) AS SumUnitGuiderFee
        FROM TKD_YykSyu
        LEFT JOIN TKD_Yyksho ON TKD_YykSyu.UkeNo = TKD_Yyksho.UkeNo
                AND TKD_YykSyu.SiyoKbn = 1
        GROUP BY TKD_YykSyu.UkeNo
    )

	SELECT ISNULL(eTKD_Yyksho01.UkeNo, 0) AS UkeNo
			,ISNULL(eTKD_Unkobi01.HaiSYmd, '') AS HaiSYmd
			,ISNULL(eTKD_Unkobi01.TouYmd, '') AS TouYmd
			,ISNULL(eVPM_Tokisk01.RyakuNm, '') AS TokisakiNm
			,ISNULL(eVPM_TokiSt01.RyakuNm, '') AS TokisitenNm
			,ISNULL(eTKD_Unkobi01.DanTaNm, '') AS DanTaNm
			,ISNULL(eTKD_Unkobi01.IkNm, '') AS IkNm
			,ISNULL(eTKD_YykSyu01.SumDriverNum, '') AS DriverNum
			,ISNULL(eTKD_YykSyu01.SumGuiderNum, 0) AS GuiderNum
			,ISNULL(eTKD_YykSyu01.SumUnitBusPrice, 0) AS Unchin
			,ISNULL(eTKD_YykSyu01.SumUnitBusFee, 0) AS Ryokin
			,ISNULL(eTKD_YykSyu01.SumSyaSyuTan, 0) AS SyaSyuTan
			,ISNULL(eTKD_YykSyu01.SumSyaSyuDai, 0) AS SyaSyuDai
			,ISNULL(eTKD_YykSyu01.SumSyaRyoUnc, 0) AS SyaRyoUnc
			,ISNULL(eTKD_YykSyu01.SumUnitGuiderPrice, 0) AS UnitGuiderPrice
			,ISNULL(eTKD_YykSyu01.SumUnitGuiderFee, 0) AS UnitGuiderFee
	FROM TKD_Yyksho AS eTKD_Yyksho01
	LEFT JOIN TKD_Unkobi AS eTKD_Unkobi01 ON eTKD_Yyksho01.UkeNo = eTKD_Unkobi01.UkeNo
			AND eTKD_Unkobi01.SiyoKbn = 1
	LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01 ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
			AND eTKD_Yyksho01.TenantCdSeq = eVPM_Tokisk01.TenantCdSeq
			AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd
					AND eVPM_Tokisk01.SiyoEndYmd
	LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01 ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
			AND eVPM_Gyosya01.SiyoKbn = 1
	LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01 ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
			AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
			AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd
					AND eVPM_TokiSt01.SiyoEndYmd
	LEFT JOIN eTKD_YykSyu01 ON eTKD_YykSyu01.UkeNo = eTKD_Yyksho01.UkeNo
	WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
			AND eTKD_Yyksho01.UkeNo IN (SELECT value FROM STRING_SPLIT(@UkeNo, ','))
END
GO
