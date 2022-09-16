USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetListVehicleRepairData_R]    Script Date: 2020/10/28 09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dGetListVehicleRepairData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetListVehicleRepair
-- Date			:   2020/10/28
-- Author		:   Tra Nguyen 
-- Description	:   Get Vehicle Repair List For AttendanceReport
CREATE OR ALTER PROCEDURE PK_dGetListVehicleRepairData_R
	@ProcessingDate varchar(8),
	@EigyoCdFrom		int,
	@EigyoCdTo		int,
	@CompanyCdSeq	int
AS
BEGIN
	SELECT eTKD_Shuri01.ShuriCdSeq,
		 eTKD_Shuri01.SyaRyoCdSeq,
		 ISNULL(eVPM_CodeKb01.CodeKbn, '') AS SyuRiCdCodeKbn,
		 ISNULL(eVPM_CodeKb01.CodeKbnNm, '') AS SyuRiCdCodeKbnNm,
		 ISNULL(eVPM_CodeKb01.RyakuNm, '') AS SyuRiCdRyakuNm,
		 ISNULL(eVPM_HenSya03.EigyoCdSeq, 0) AS EigyoCdSeq,
		 ISNULL(eVPM_Eigyos04.EigyoCd, 0) AS EigyosEigyoCd,
		 ISNULL(eVPM_Eigyos04.EigyoNm, '') AS EigyosEigyoNm,
		 ISNULL(eVPM_Eigyos04.RyakuNm, '') AS EigyosRyakuNm
	FROM TKD_Shuri AS eTKD_Shuri01
	INNER JOIN
		 (SELECT eTKD_ShuYmd00.ShuriTblSeq
		 FROM TKD_ShuYmd AS eTKD_ShuYmd00
		 WHERE eTKD_ShuYmd00.ShuriYmd = @ProcessingDate -- 運行年月日
		 GROUP BY eTKD_ShuYmd00.ShuriTblSeq) AS eTKD_ShuYmd01 
		 ON eTKD_ShuYmd01.ShuriTblSeq = eTKD_Shuri01.ShuriTblSeq
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
		 AND eVPM_Compny05.CompanyCdSeq = @CompanyCdSeq							--「会社」コンボボックスの値のCompanyCdSeq
		 AND (@EigyoCdFrom = 0 OR eVPM_Eigyos04.EigyoCd >= @EigyoCdFrom)		-- 配車営業所コード　開始のEigyoCd
		 AND (@EigyoCdTo = 0 OR eVPM_Eigyos04.EigyoCd <= @EigyoCdTo)			--配車営業所コード　終了のEigyoCd
	ORDER BY EigyosEigyoCd, EigyoCdSeq
END
GO
