USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBirthDay_R]    Script Date: 12/10/2020 11:19:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dBirthDay_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get birthday data table
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get birthday data with conditions
------------------------------------------------------------
CREATE OR ALTER      PROCEDURE [dbo].[PK_dBirthDay_R]
		(
		--Parameter
			@TenantCdSeq		INT,
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		SELECT
    eVPM_Syain.SyainCdSeq AS SyainCdSeq, --社員コードSEQ
    eVPM_Syain.SyainCd AS SyainCd, --社員コード
    eVPM_Syain.SyainNm AS SyainNm, --社員名
    eVPM_Syain.BirthYmd AS BirthYmd, --生年月日
    VPM_Syokum.SyokumuKbn AS SyokumuKbn --職務区分
FROM
    VPM_Syain AS eVPM_Syain
    JOIN VPM_KyoSHe AS eVPM_KyoSHe ON eVPM_KyoSHe.SyainCdSeq = eVPM_Syain.SyainCdSeq
    AND (
        CAST(eVPM_KyoSHe.StaYmd AS DATE) <= CAST(GETDATE() AS DATE)
    )
    AND (
        CAST(eVPM_KyoSHe.EndYmd AS DATE) >= CAST(GETDATE() AS DATE)
    )
    LEFT JOIN VPM_Syokum ON eVPM_KyoSHe.SyokumuCdSeq = VPM_Syokum.SyokumuCdSeq
    LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_KyoSHe.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq
    AND eVPM_Eigyos.SiyoKbn = 1
    LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Eigyos.CompanyCdSeq = eVPM_Compny.CompanyCdSeq
    AND eVPM_Compny.SiyoKbn = 1
WHERE
    eVPM_Compny.TenantCdSeq = @TenantCdSeq
    AND SyokumuKbn IN (1, 2, 3, 4, 5)
    AND SUBSTRING(eVPM_Syain.BirthYmd, 5, 4) BETWEEN @FromDate
    AND @ToDate

		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN