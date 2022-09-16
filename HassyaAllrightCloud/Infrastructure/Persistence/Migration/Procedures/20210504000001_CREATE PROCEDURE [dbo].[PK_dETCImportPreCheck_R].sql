USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[PK_dETCImportPreCheck_R]    Script Date: 2020/09/11 16:52:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   ETCImportConditionSetting
-- SP-ID		:   PK_dETCImportPreCheck_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetEtcImport
-- Date			:   2020/08/12
-- Author		:   Tra Nguyen 
-- Description	:   Check if Etc Import Data is exist
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dETCImportPreCheck_R]
	(
	-- Parameter
        @TenantCdSeq int,                                   --ログインしたユーザーのTenantCdSeq
		@CardNo	varchar(19),								--該当行のCardNo
		@UnkYmd varchar(8),								--該当行のUnkYmd
		@UnkTime varchar(6),								--該当行のUnkTime
		@SyaRyoCd int,								--該当行のSyaRyoCd
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
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
WHERE	TKD_EtcImport.TenantCdSeq = @TenantCdSeq
        AND CardNo = @CardNo
        AND UnkYmd = @UnkYmd
        AND UnkTime = @UnkTime
        AND TKD_EtcImport.SyaRyoCd = @SyaRyoCd

		SET @ROWCOUNT = @@ROWCOUNT
END
GO
