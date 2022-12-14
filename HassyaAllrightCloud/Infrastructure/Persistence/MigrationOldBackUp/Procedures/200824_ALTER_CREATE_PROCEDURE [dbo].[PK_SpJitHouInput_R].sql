USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_SpJitHouInput_R]    Script Date: 8/24/2020 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_SpJitHouInput_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get JitHou Result Input
-- Date			:   2020/08/24
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_SpJitHouInput_R]
    (
     -- Parameter
         @CompanyCdSeq     VARCHAR(8)             --会社
	 ,   @StrDate          VARCHAR(8)             --処理年月
	 ,   @StrEigyoCd       VARCHAR(8)             --営業所から
	 ,   @StrUnsouKbn      VARCHAR(8)             --運送から
     ,   @TenantCdSeq      VARCHAR(8)
        -- Output
	 ,	@ROWCOUNT	   INTEGER OUTPUT	   -- 処理件数
    )
AS 
    -- Processing
	BEGIN
		SELECT TKD_JitHou.*																												
        ,ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS EigCdSeq_EigyoCd																												
        ,ISNULL(eVPM_Eigyos01.EigyoNm, ' ') AS EigCdSeq_EigyoNm																												
        ,ISNULL(eVPM_Eigyos01.RyakuNm, ' ') AS EigCdSeq_RyakuNm																												
        ,ISNULL(eVPM_CodeKb01.CodeKbnNm, ' ') AS KataKbn_CodeKbnNm																												
        ,ISNULL(eVPM_CodeKb01.RyakuNm, ' ') AS KataKbn_RyakuNm																												
        ,ISNULL(eVPM_CodeKb02.CodeKbnNm, ' ') AS NenryoKbn_CodeKbnNm																												
        ,ISNULL(eVPM_CodeKb02.RyakuNm, ' ') AS NenryoKbn_RyakuNm																												
        ,ISNULL(eVPM_Syain01.SyainCd, ' ') AS UpdSyainCd_SyainCd																												
        ,ISNULL(eVPM_Syain01.SyainNm, ' ') AS UpdSyainCd_SyainNm																												
        FROM TKD_JitHou																												
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01 ON TKD_JitHou.EigyoCdSeq = eVPM_Eigyos01.EigyoCdSeq																												
                AND eVPM_Eigyos01.SiyoKbn = 1																												
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeSyu = 'KATAKBN'																												
                AND eVPM_CodeKb01.CodeKbn = dbo.FP_RpZero(1, TKD_JitHou.KataKbn)																												
                AND eVPM_CodeKb01.SiyoKbn = 1																												
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02 ON eVPM_CodeKb02.CodeSyu = 'NENRYOKBN'																												
                AND eVPM_CodeKb02.CodeKbn = dbo.FP_RpZero(1, TKD_JitHou.NenryoKbn)																												
                AND eVPM_CodeKb02.SiyoKbn = 1																												
        LEFT JOIN VPM_Syain AS eVPM_Syain01 ON TKD_JitHou.UpdSyainCd = eVPM_Syain01.SyainCdSeq
        LEFT JOIN VPM_Compny AS eVPM_Compny01 ON eVPM_Compny01.CompanyCdSeq = eVPM_Eigyos01.CompanyCdSeq
        AND eVPM_Compny01.SiyoKbn = 1

        WHERE   (@StrDate IS NULL OR TKD_JitHou.SyoriYm = @StrDate)
                AND (@CompanyCdSeq IS NULL OR eVPM_Eigyos01.CompanyCdSeq = @CompanyCdSeq)
                AND (@StrEigyoCd IS NULL OR eVPM_Eigyos01.EigyoCd = @StrEigyoCd)
                AND (@StrUnsouKbn IS NULL OR TKD_JitHou.UnsouKbn = @StrUnsouKbn)
                AND eVPM_Compny01.TenantCdSeq = @TenantCdSeq
	END
    SET	@ROWCOUNT	=	@@ROWCOUNT
RETURN																													

