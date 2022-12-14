USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBirthDay_R]    Script Date: 2021/01/22 10:46:00 ******/
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
ALTER        PROCEDURE [dbo].[PK_dBirthDay_R]
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
	(CASE WHEN SUBSTRING(@FromDate, 1, 4) = SUBSTRING(@ToDate, 1, 4)
          THEN CONCAT(SUBSTRING(@FromDate, 1, 4) , SUBSTRING(eVPM_Syain.BirthYmd, 5, 4)) 
		  WHEN SUBSTRING(eVPM_Syain.BirthYmd, 5, 4) >= SUBSTRING(@FromDate, 5, 4)
		  THEN CONCAT(SUBSTRING(@FromDate, 1, 4) , SUBSTRING(eVPM_Syain.BirthYmd, 5, 4))
		  ELSE CONCAT(SUBSTRING(@ToDate, 1, 4) , SUBSTRING(eVPM_Syain.BirthYmd, 5, 4))
	 END) AS BirthYmd, --生年月日s
	 eVPM_Syain.BirthYmd a,
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
    AND ( (  (CASE WHEN SUBSTRING(@FromDate, 1, 4) = SUBSTRING(@ToDate, 1, 4)
               THEN SUBSTRING(eVPM_Syain.BirthYmd, 5, 4) ELSE CONCAT(SUBSTRING(@FromDate, 1, 4) , SUBSTRING(eVPM_Syain.BirthYmd, 5, 4)) 
			END)
			BETWEEN 
            (CASE WHEN SUBSTRING(@FromDate, 1, 4) = SUBSTRING(@ToDate, 1, 4)
               THEN SUBSTRING(@FromDate, 5, 4) ELSE @FromDate
			END) 
			AND 
			(CASE WHEN SUBSTRING(@FromDate, 1, 4) = SUBSTRING(@ToDate, 1, 4)
               THEN SUBSTRING(@ToDate, 5, 4) ELSE CONCAT(SUBSTRING(@FromDate, 1, 4) , '1231') 
			END)
         )
    OR (  (CASE WHEN SUBSTRING(@FromDate, 1, 4) = SUBSTRING(@ToDate, 1, 4)
               THEN SUBSTRING(eVPM_Syain.BirthYmd, 5, 4) ELSE CONCAT(SUBSTRING(@ToDate, 1, 4) , SUBSTRING(eVPM_Syain.BirthYmd, 5, 4)) 
			END)
			BETWEEN 
            (CASE WHEN SUBSTRING(@FromDate, 1, 4) = SUBSTRING(@ToDate, 1, 4)
               THEN SUBSTRING(@FromDate, 5, 4) ELSE CONCAT(SUBSTRING(@ToDate, 1, 4) , '0101')
			END) 
			AND 
			(CASE WHEN SUBSTRING(@FromDate, 1, 4) = SUBSTRING(@ToDate, 1, 4)
               THEN SUBSTRING(@ToDate, 5, 4) ELSE @ToDate 
			END)
         )
      )
		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN

