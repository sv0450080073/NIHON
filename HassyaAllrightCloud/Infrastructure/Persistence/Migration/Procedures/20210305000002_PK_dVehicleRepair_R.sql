USE [HOC_Kashikiri]
GO
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
-- SP-ID		:   PK_dVehicleRepair_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get VehicleRepair List
-- Date			:   2021/02/18
-- Author		:   P.M.Nhat
-- Description	:   Get VehicleRepair list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dVehicleRepair_R
	@StartYmd varchar(8),
	@EndYmd varchar(8),
	@SyaRyoCdSeq int
AS
BEGIN
	SELECT eTKD_Shuri01.ShuriCdSeq,
		 eTKD_Shuri01.SyaRyoCdSeq,
		 eTKD_Shuri01.ShuriSYmd,
		 eTKD_Shuri01.ShuriSTime,
		 eTKD_Shuri01.ShuriEYmd,
		 eTKD_Shuri01.ShuriETime,
		 eTKD_Shuri01.BikoNm,
		 VPM_SyaRyo.SyaRyoNm,
		 ISNULL(eVPM_CodeKb01.CodeKbn, '') AS SyuRiCd_CodeKbn,
		 ISNULL(eVPM_CodeKb01.CodeKbnNm, '') AS SyuRiCd_CodeKbnNm,
		 ISNULL(eVPM_CodeKb01.RyakuNm, '') AS SyuRiCd_RyakuNm,
		 ISNULL(eVPM_HenSya03.EigyoCdSeq, 0) AS EigyoCdSeq,
		 ISNULL(eVPM_Eigyos04.EigyoCd, 0) AS Eigyos_EigyoCd,
		 ISNULL(eVPM_Eigyos04.EigyoNm, '') AS Eigyos_EigyoNm,
		 ISNULL(eVPM_Eigyos04.RyakuNm, '') AS Eigyos_RyakuNm
	FROM TKD_Shuri AS eTKD_Shuri01
	LEFT JOIN VPM_SyaRyo
		 ON eTKD_Shuri01.SyaRyoCdSeq = VPM_SyaRyo.SyaRyoCdSeq
	LEFT JOIN TKD_ShuYmd
		 ON TKD_ShuYmd.ShuriTblSeq = eTKD_Shuri01.ShuriTblSeq
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01
		 ON eVPM_CodeKb01.CodeKbnSeq = eTKD_Shuri01.ShuriCdSeq
	LEFT JOIN VPM_HenSya AS eVPM_HenSya03
		 ON eVPM_HenSya03.SyaRyoCdSeq = eTKD_Shuri01.SyaRyoCdSeq
		 AND eTKD_Shuri01.ShuriSYmd BETWEEN eVPM_HenSya03.StaYmd AND eVPM_HenSya03.EndYmd
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos04
		 ON eVPM_Eigyos04.EigyoCdSeq = eVPM_HenSya03.EigyoCdSeq
	LEFT JOIN VPM_Compny AS eVPM_Compny05
		 ON eVPM_Compny05.CompanyCdSeq = eVPM_Eigyos04.CompanyCdSeq
	WHERE eTKD_Shuri01.SiyoKbn = 1
		 AND TKD_ShuYmd.SiyoKbn = 1
		 AND eTKD_Shuri01.SyaRyoCdSeq = @SyaRyoCdSeq
		 AND eTKD_Shuri01.ShuriSYmd <= @EndYmd
		 AND eTKD_Shuri01.ShuriEYmd  >= @StartYmd
	ORDER BY Eigyos_EigyoCd, EigyoCdSeq
END
GO
