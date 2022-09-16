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
-- SP-ID		:   PK_dHaichiBunruiCds_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Haichi BunruiCd List
-- Date			:   2021/05/27
-- Author		:   P.M.Nhat
-- Description	:   Get Haichi BunruiCd list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dHaichiBunruiCds_R]
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int
AS
BEGIN
	SELECT HAICHI.BunruiCdSeq AS HaiChiBunruiCdSeq
			,HAICHI.HaiSCdSeq AS HaiChiHaiSCdSeq
			,HAICHI.HaiSCd AS HaiChiHaiSCd
			,HAICHI.RyakuNm AS HaiChiHaiSNm
			,HAICHI.Jyus1 AS HaiChiJyus1
			,HAICHI.Jyus2 AS HaiChiJyus2
			,HAICHI.HaiSKigou AS HaiChiHaiSKigou
			,ISNULL(BUNRUI.CodeKbnNm, '') AS BUNRUICodeKbnNm
	FROM VPM_Haichi AS HAICHI
	LEFT JOIN VPM_CodeKb AS BUNRUI ON BUNRUI.CodeKbnSeq = HAICHI.BunruiCdSeq
			AND BUNRUI.CodeSyu = 'BUNRUICD'
			AND BUNRUI.SiyoKbn = 1
			AND BUNRUI.TenantCdSeq = (
					SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
					FROM VPM_CodeSy
					WHERE CodeSyu = 'BUNRUICD'
							AND KanriKbn <> 1
					)
	WHERE HAICHI.SiyoKbn = 1
			AND HAICHI.TenantCdSeq = @TenantCdSeq --@TenantCdSeq: ログインユーザーのテナン
	ORDER BY HAICHI.BunruiCdSeq ASC
			,HAICHI.HaiSCdSeq ASC								
END
