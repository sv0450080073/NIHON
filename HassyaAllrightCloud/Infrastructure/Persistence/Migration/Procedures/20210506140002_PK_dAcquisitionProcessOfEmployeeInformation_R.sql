USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dAcquisitionProcessOfEmployeeInformation_R]    Script Date: 5/6/2021 1:53:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetAlertSettingByCode
-- Date			:   2020/12/22
-- Author		:   T.L.DUY
-- Description	:   Get alert setting by code base on sheet excel 3.9
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dAcquisitionProcessOfEmployeeInformation_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@StaYmd					CHAR(8),
		@EndYmd					CHAR(8),
		@SyokumuKbnList			NVARCHAR(100),
		@CompanyCdSeq			INT,
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT ISNULL(KYOSHE.SyainCdSeq, 0) as SyainCdSeq --AS '社員Seq'
        ,ISNULL(KYOSHE.SyokumuCdSeq, 0) as SyokumuCdSeq --AS '職務Seq'
        ,SYOKUM.SyokumuNm --AS '職務名'
        ,ISNULL(SYOKUM.SyokumuKbn, 0) as SyokumuKbn --AS '職務区分'
        ,SHOKUMU.CodeKbnNm --AS '職務区分名'
        ,KYOSHE.TenkoNo --AS '点呼番号'
        ,ISNULL(KYOSHE.EigyoCdSeq, 0) as EigyoCdSeq --AS '営業所Seq'
        ,ISNULL(EIGYOS.EigyoCd, 0) as EigyoCd --AS '営業所コード'
        ,EIGYOS.RyakuNm --AS '営業所名'
        ,SYAIN.SyainCd --AS '社員コード'
        ,SYAIN.SyainNm --AS '社員名称'
        ,KYOSHE.StaYmd --AS '開始年月日'
        ,KYOSHE.EndYmd --AS '終了年月日'
        ,ISNULL(KAISHA.CompanyCdSeq, 0) as CompanyCdSeq --AS '会社コードSeq'
        ,ISNULL(KAISHA.CompanyCd, 0) as CompanyCd --AS '会社コード'
        ,KAISHA.CompanyNm --AS '会社名'
FROM VPM_KyoSHe AS KYOSHE
LEFT JOIN VPM_Syain AS SYAIN ON SYAIN.SyainCdSeq = KYOSHE.SyainCdSeq
LEFT JOIN VPM_Syokum AS SYOKUM ON SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
        AND SYOKUM.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_CodeKb AS SHOKUMU ON SHOKUMU.CodeKbn = SYOKUM.SyokumuKbn
        AND SHOKUMU.CodeSyu = 'SYOKUMUKBN'
        AND SHOKUMU.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_Eigyos AS EIGYOS ON EIGYOS.EigyoCdSeq = KYOSHE.EigyoCdSeq
INNER JOIN VPM_Compny AS KAISHA ON KAISHA.CompanyCdSeq = EIGYOS.CompanyCdSeq
        AND KAISHA.TenantCdSeq = @TenantCdSeq
WHERE KAISHA.CompanyCdSeq = @CompanyCdSeq
        AND KYOSHE.StaYmd <= @StaYmd
        AND KYOSHE.EndYmd >= @EndYmd
        AND SYOKUM.SyokumuKbn IN (SELECT value FROM STRING_SPLIT(@SyokumuKbnList, ','))

SET	@ROWCOUNT	=	@@ROWCOUNT
END
