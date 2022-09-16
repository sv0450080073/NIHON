USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dETCs_R]    Script Date: 4/23/2021 3:37:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dETCs_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get ETC List
-- Date			:   2020/09/11
-- Author		:   P.M.Nhat
-- Description	:   Get ETC list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dETCs_R] 
	-- Add the parameters for the stored procedure here
	@SyaRyoCompanyCd		int,				-- 車輌会社								
	@SyaRyoEigyoCd			int,				-- 車輌営業所								
	@SyaRyoCdFrom			int,				-- 開始車輌								
	@SyaRyoCdTo				int,				-- 終了車輌								
	@UnkYmdFrom				varchar(8),			-- 開始ETC利用年月日								
	@UnkYmdTo				varchar(8),			-- 終了ETC利用年月日				
	@KikoYmdFrom			varchar(8),			-- 開始帰庫年月日				
	@KikoYmdTo				varchar(8),			-- 開始帰庫年月日				
	@SortOrder				tinyint,			-- 並び順				
	@TensoKbn				tinyint,			-- 転送区分				
	@ListFileName			nvarchar(max),		
	@TenantCdSeq			int				
AS
BEGIN
	SELECT TKD_EtcImport.*		
			,ISNULL(JM_SyaRyo.SyaRyoNm, ' ') AS SyaRyoNm		
			,ISNULL(JM_IriRyokin.RoadCorporationKbn, ' ') AS IriRoadCorporationKbn		
			,ISNULL(JM_IriRyokin.RoadCorporationName, ' ') AS IriRoadCorporationNm		
			,ISNULL(JM_IriRyokin.DouroName, ' ') AS IriDouroNm		
			,ISNULL(JM_IriRyokin.RyokinNm, ' ') AS IriRyokinNm		
			,ISNULL(JM_IriRyokin.RyakuNm, ' ') AS IriRyakuNm		
			,ISNULL(JM_DeRyokin.RoadCorporationKbn, ' ') AS DeRoadCorporationKbn		
			,ISNULL(JM_DeRyokin.RoadCorporationName, ' ') AS DeRoadCorporationNm		
			,ISNULL(JM_DeRyokin.DouroName, ' ') AS DeDouroNm		
			,ISNULL(JM_DeRyokin.RyokinNm, ' ') AS DeRyokinNm		
			,ISNULL(JM_DeRyokin.RyakuNm, ' ') AS DeRyakuNm		
			,ISNULL(JM_Futai.FutaiCd, 0) AS FutaiCd		
			,ISNULL(JM_Seisan.SeisanCd, 0) AS SeisanCd		
			,ISNULL(JM_Futai.FutaiNm, 0) AS JM_FutTumNm		
			,ISNULL(JM_Seisan.SeisanNm, 0) AS JM_SeisanNm		
			,ISNULL(JM_Eigyos.RyakuNm, '') AS SyaRyoEigyoNm		
			,ISNULL(eVPM_Syain01.SyainCd, ' ') AS UpdSyainCd_SyainCd		
			,ISNULL(eVPM_Syain01.SyainNm, ' ') AS UpdSyainCd_SyainNm		
	FROM TKD_EtcImport		
	LEFT JOIN VPM_SyaRyo AS JM_SyaRyo ON TKD_EtcImport.SyaRyoCd = JM_SyaRyo.SyaRyoCd		
	LEFT JOIN VPM_Ryokin AS JM_IriRyokin ON TKD_EtcImport.IriRyoChiCd = JM_IriRyokin.RyokinTikuCd		
			AND TKD_EtcImport.IriRyoCd = JM_IriRyokin.RyokinCd		
	LEFT JOIN VPM_Ryokin AS JM_DeRyokin ON TKD_EtcImport.DeRyoChiCd = JM_DeRyokin.RyokinTikuCd		
			AND TKD_EtcImport.DeRyoCd = JM_DeRyokin.RyokinCd		
	LEFT JOIN VPM_Futai AS JM_Futai ON TKD_EtcImport.FutTumCdSeq = JM_Futai.FutaiCdSeq		
			AND JM_Futai.SiyoKbn = 1		
			AND JM_Futai.TenantCdSeq = @TenantCdSeq		
	LEFT JOIN VPM_Syain AS eVPM_Syain01 ON TKD_EtcImport.UpdSyainCd = eVPM_Syain01.SyainCdSeq		
	LEFT JOIN VPM_Seisan AS JM_Seisan ON TKD_EtcImport.SeisanCdSeq = JM_Seisan.SeisanCdSeq		
			AND JM_Seisan.SiyoKbn = 1		
	LEFT JOIN VPM_HenSya AS JM_Hensya ON JM_SyaRyo.SyaRyoCdSeq = JM_Hensya.SyaRyoCdSeq		
			AND JM_Hensya.StaYmd <= TKD_EtcImport.UnkYmd		
			AND JM_Hensya.EndYmd >= TKD_EtcImport.UnkYmd		
	LEFT JOIN VPM_Eigyos AS JM_Eigyos ON JM_Hensya.EigyoCdSeq = JM_Eigyos.EigyoCdSeq		
			AND JM_Eigyos.SiyoKbn = 1		
	LEFT JOIN VPM_Compny AS JM_Compny ON JM_Eigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq		
			AND JM_Compny.SiyoKbn = 1		
	LEFT JOIN VPM_Tenant AS JM_Tenant ON JM_Compny.TenantCdSeq = JM_Tenant.TenantCdSeq		
			AND JM_Tenant.SiyoKbn = 1																																
	WHERE 1 = 1																														
	        AND (@TenantCdSeq = 0 OR TKD_EtcImport.TenantCdSeq = @TenantCdSeq)																														
	        AND (@SyaRyoCompanyCd = 0 OR JM_Compny.CompanyCdSeq = @SyaRyoCompanyCd)																														
	        AND (@SyaRyoEigyoCd = 0 OR JM_Eigyos.EigyoCd = @SyaRyoEigyoCd)																														
	        AND (@SyaRyoCdFrom = 0 OR TKD_EtcImport.SyaRyoCd >= @SyaRyoCdFrom)																														
	        AND (@SyaRyoCdTo = 0 OR TKD_EtcImport.SyaRyoCd <= @SyaRyoCdTo)																														
	        AND (@UnkYmdFrom = '' OR TKD_EtcImport.UnkYmd >= @UnkYmdFrom)																														
	        AND (@UnkYmdTo = '' OR TKD_EtcImport.UnkYmd <= @UnkYmdTo)																														
	        AND (@KikoYmdFrom = '' OR Substring(TKD_EtcImport.ExpItem,4,8) >= @KikoYmdFrom)																														
	        AND (@KikoYmdTo = '' OR Substring(TKD_EtcImport.ExpItem,4,8) <= @KikoYmdTo)																														
	        AND (@TensoKbn <> 0 OR TKD_EtcImport.TensoKbn = 0) -- IF @TensoKbn = 「未転送」: Add this condition																														
	        AND (@TensoKbn <> 1 OR TKD_EtcImport.TensoKbn = 1) -- IF @TensoKbn = 「転送済」: Add this condition																														
	        AND (@ListFileName = '' OR TKD_EtcImport.FileName IN ( SELECT value FROM STRING_SPLIT(@ListFileName,','))) -- IF have @ListFileName from ETC取込画面: Add this condition																														
			 -- IF @SortOrder = 「車輌コード」: Add below ORDER																														
	ORDER BY CASE WHEN @SortOrder = 0 THEN TKD_EtcImport.SyaryoCd END,
			 CASE WHEN @SortOrder = 0 THEN TKD_EtcImport.UnkYmd END,
			 CASE WHEN @SortOrder = 0 THEN TKD_EtcImport.UkeNo END,
			 -- IF @SortOrder = 「受付番号」: Add below ORDER	
			 CASE WHEN @SortOrder = 1 THEN TKD_EtcImport.UnkYmd END,
			 CASE WHEN @SortOrder = 1 THEN TKD_EtcImport.UkeNo END,
			 CASE WHEN @SortOrder = 1 THEN TKD_EtcImport.SyaryoCd END
END
