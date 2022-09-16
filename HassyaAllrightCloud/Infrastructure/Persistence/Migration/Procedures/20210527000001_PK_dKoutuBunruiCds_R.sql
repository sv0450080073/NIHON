USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dKoutuBunruiCds_R]    Script Date: 5/27/2021 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dKoutuBunruiCds_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Koutu BunruiCd List
-- Date			:   2021/05/27
-- Author		:   P.M.Nhat
-- Description	:   Get Koutu BunruiCd list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dKoutuBunruiCds_R]
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int,
	@Ymd varchar(8)
AS
BEGIN
	SELECT KOUTU.BunruiCdSeq AS KoutuBunruiCdSeq
			,KOUTU.KoukCdSeq AS KoutuKoukCdSeq
			,KOUTU.KoukCd AS KoutuKoukCd
			,KOUTU.KoukNm AS KoutuRyakuNm
			,ISNULL(BUNRUI.CodeKbnNm, '') AS BUNRUICodeKbnNm
			,ISNULL(BIN.BinCdSeq, 0) AS BinBinCdSeq
			,ISNULL(BIN.BinCd, 0) AS BinBinCd
			,ISNULL(BIN.BinNm, '') AS BinBinNm
			,ISNULL(BIN.SyuPaTime, '') AS BINSyuPaTime
			,ISNULL(BIN.TouChTime, '') AS BINTouChTime
			,ISNULL(BIN.SiyoStaYmd, '') AS BINSiyoStaYmd
			,ISNULL(BIN.SiyoEndYmd, '') AS BINSiyoEndYmd
	FROM VPM_Koutu AS KOUTU
	LEFT JOIN VPM_CodeKb AS BUNRUI ON BUNRUI.CodeKbnSeq = KOUTU.BunruiCdSeq
			AND BUNRUI.CodeSyu = 'BUNRUICD'
			AND BUNRUI.SiyoKbn = 1
			AND BUNRUI.TenantCdSeq = (
					SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
					FROM VPM_CodeSy
					WHERE CodeSyu = 'BUNRUICD'
							AND KanriKbn <> 1
					)
	LEFT JOIN VPM_Bin AS BIN ON BIN.KoukCdSeq = KOUTU.KoukCdSeq
			AND BIN.TenantCdSeq = @TenantCdSeq --@TenantCdSeq: ログインユーザーのテナントコード     
			AND BIN.SiyoStaYmd <= @Ymd --@Ymd: 対象の配車年月日また到着年月日      
			AND BIN.SiyoEndYmd >= @Ymd --@Ymd: 対象の配車年月日また到着年月日      
	WHERE KOUTU.SiyoKbn = 1
	ORDER BY KOUTU.BunruiCdSeq ASC
			,KOUTU.KoukCdSeq ASC								
END
