USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetVehicleData_R]    Script Date: 2020/10/28 09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dGetVehicleData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetListVehicle
-- Date			:   2020/10/28
-- Author		:   Tra Nguyen 
-- Description	:   Get Vehicle List For AttendanceReport
CREATE OR ALTER PROCEDURE PK_dGetVehicleData_R
	@ProcessingDate		varchar(8),
	@EigyoCdFrom		int,
	@EigyoCdTo			int,
	@CompanyCdSeq		int,
	@TenantCdSeq		int
AS
BEGIN
	SELECT eVPM_HenSya01.SyaRyoCdSeq,
		 ISNULL(eVPM_SyaRyo01.SyaRyoCd, 0) AS SyaRyoSyaRyoCd,
		 ISNULL(eVPM_SyaRyo01.SyaRyoNm, '') AS SyaRyoSyaRyoNm,
		 ISNULL(eVPM_SyaRyo01.SyaSyuCdSeq, 0) AS SyaRyoSyaSyuCdSeq,
		 ISNULL(eVPM_SyaSyu01.SyaSyuCd, 0) AS SyaSyuSyaSyuCd,
		 ISNULL(eVPM_SyaSyu01.SyaSyuNm, '') AS SyaSyuSyaSyuNm,
		 ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS EigyosEigyoCd,
		 ISNULL(eVPM_Eigyos01.EigyoNm, '') AS EigyosEigyoNm,
		 ISNULL(eVPM_Eigyos01.RyakuNm, '') AS EigyosRyakuNm
	FROM VPM_HenSya AS eVPM_HenSya01
	LEFT JOIN VPM_SyaRyo AS eVPM_SyaRyo01
		 ON eVPM_HenSya01.SyaRyoCdSeq = eVPM_SyaRyo01.SyaRyoCdSeq
	LEFT JOIN VPM_SyaSyu AS eVPM_SyaSyu01
		 ON eVPM_SyaRyo01.SyaSyuCdSeq = eVPM_SyaSyu01.SyaSyuCdSeq
		 AND eVPM_SyaSyu01.TenantCdSeq = @TenantCdSeq --ロギングしたユーザーのTenantCdSeq
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
		 ON eVPM_HenSya01.EigyoCdSeq = eVPM_Eigyos01.EigyoCdSeq
	LEFT JOIN VPM_Compny AS eVPM_Compny01
		 ON eVPM_Eigyos01.CompanyCdSeq = eVPM_Compny01.CompanyCdSeq
		 AND eVPM_Compny01.TenantCdSeq = @TenantCdSeq --ロギングしたユーザーのTenantCdSeq
	INNER JOIN VPM_HenSyaSub AS JM_HenSyaSub
		 ON JM_HenSyaSub.SyaRyoCdSeq = eVPM_HenSya01.SyaRyoCdSeq
		 AND JM_HenSyaSub.EndYmd = eVPM_HenSya01.EndYmd
		 AND (JM_HenSyaSub.KasNorSysKbn = 1 OR JM_HenSyaSub.KasNorSysKbn = 2)
	WHERE eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq --「会社」コンボボックスの値のCompanyCdSeq
		 AND eVPM_SyaRyo01.NinkaKbn = 1
		 AND eVPM_HenSya01.StaYmd <= @ProcessingDate	-- 運行年月日 yyyyMMdd
		 AND eVPM_HenSya01.EndYmd >= @ProcessingDate	--運行年月日 yyyyMMdd
		 AND (@EigyoCdFrom = 0 OR eVPM_Eigyos01.EigyoCd >= @EigyoCdFrom)		--配車営業所コード　開始のEigyoCd
		 AND (@EigyoCdTo = 0 OR eVPM_Eigyos01.EigyoCd <= @EigyoCdTo)			--配車営業所コード　終了のEigyoCd
	ORDER BY EigyosEigyoCd,
		 eVPM_HenSya01.EigyoCdSeq,
		 CAST(eVPM_SyaSyu01.SyaSyuCd　AS int),
		 CAST(eVPM_SyaRyo01.SyaRyoCd　AS int)
END
GO
