--USE [HOC_Kashikiri]
--GO
/****** Object:  StoredProcedure [dbo].[PK_SpJitHouAnnual_R]    Script Date: 09/09/2020 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_SpJitHouAnnual_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get JitHou Report
-- Date			:   2020/09/09
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_SpJitHouAnnual_R]
    (
     -- Parameter
         @CompnyCd     VARCHAR(8)             --会社
	 ,   @StrDate      VARCHAR(8)             --処理年月
	 ,   @EndDate      VARCHAR(8)             --処理年月終了
	 ,   @StrEigyoCd   VARCHAR(8)             --営業所から
	 ,   @EndEigyoCd   VARCHAR(8)             --営業所まで
	 ,   @StrUnsouKbn  VARCHAR(8)             --運送から
	 ,   @EndUnsouKbn  VARCHAR(8)             --運送まで
	 ,   @TenantCdSeq  VARCHAR(8)
        -- Output
	 ,	@ROWCOUNT	   INTEGER OUTPUT	   -- 処理件数
    )
AS 
    -- Processing
	BEGIN
		SELECT UnsouKbn																														
        ,ISNULL(JM_UnsouKbn.CodeKbnNm, '') AS UnsouKbnNm																														
        ,ISNULL(JM_UnsouKbn.RyakuNm, '') AS UnsouKbnRyaku																														
        ,ISNULL(JM_Eigyos.EigyoCd, '') AS EigyoCd																														
        ,ISNULL(JM_Eigyos.EigyoNm, '') AS EigyoNm																														
        ,ISNULL(JM_Eigyos.RyakuNm, '') AS EigyoRyaku																														
        ,KataKbn																														
        ,ISNULL(JM_KataKbn.CodeKbnNm, '') AS KataKbnNm																														
        ,ISNULL(JM_KataKbn.RyakuNm, '') AS KataKbnRyaku																														
        ,NobeJyoCnt																														
        ,NobeRinCnt																														
        ,NobeSumCnt																														
        ,NobeJitCnt																														
        ,JitJisaKm																														
        ,JitKisoKm																														
        ,JitJisaKm + JitKisoKm AS JitSumKm																														
        ,YusoJin																														
        ,UnkoCnt																														
        ,UnkoKikak1Cnt																														
        ,UnkoKikak2Cnt																														
        ,UnkoOthCnt																														
        ,UnkoKikak1Cnt + UnkoKikak2Cnt + UnkoOthCnt AS UnkoOthAllCnt																														
        ,UnsoSyu																														
FROM (																														
        SELECT UnsouKbn																														
                ,EigyoCdSeq																														
                ,KataKbn																														
                ,SUM(NobeJyoCnt) AS NobeJyoCnt																														
                ,SUM(NobeRinCnt) AS NobeRinCnt																														
                ,SUM(NobeSumCnt) AS NobeSumCnt																														
                ,SUM(NobeJitCnt) AS NobeJitCnt																														
                ,SUM(JitJisaKm) AS JitJisaKm																														
                ,SUM(JitKisoKm) AS JitKisoKm																														
                ,SUM(YusoJin) AS YusoJin																														
                ,SUM(UnkoCnt) AS UnkoCnt																														
                ,SUM(UnkoKikak1Cnt) AS UnkoKikak1Cnt																														
                ,SUM(UnkoKikak2Cnt) AS UnkoKikak2Cnt																														
                ,SUM(UnkoOthCnt) AS UnkoOthCnt																														
                ,SUM(UnsoSyu) AS UnsoSyu																														
        FROM TKD_JitHou																														
        WHERE 1 = 1																														
                AND (@StrDate IS NULL OR TKD_JitHou.SyoriYm >= @StrDate)																														
                AND (@EndDate IS NULL OR TKD_JitHou.SyoriYm <= @EndDate)																														
        GROUP BY UnsouKbn																														
                ,EigyoCdSeq																														
                ,KataKbn																														
        ) AS Main																														
        LEFT JOIN VPM_Eigyos AS JM_Eigyos ON Main.EigyoCdSeq = JM_Eigyos.EigyoCdSeq																														
                AND JM_Eigyos.SiyoKbn = 1																														
        LEFT JOIN VPM_Compny AS JM_Compny ON JM_Eigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq																														
                AND JM_Compny.SiyoKbn = 1																														
        LEFT JOIN VPM_Tenant AS JM_Tenant ON JM_Compny.TenantCdSeq = JM_Tenant.TenantCdSeq																														
                AND JM_Tenant.SiyoKbn = 1																														
        LEFT JOIN (																														
        SELECT CodeKbn																														
                ,CodeKbnNm																														
                ,RyakuNm															
        FROM VPM_CodeKb																														
        WHERE CodeSyu = 'UNSOUKBN'																														
                AND SiyoKbn = 1																														
                AND VPM_CodeKb.TenantCdSeq = (																														
                        SELECT CASE 																														
                                        WHEN COUNT(*) = 0																														
                                                THEN 0																														
                                        ELSE @TenantCdSeq																														
                                        END AS TenantCdSeq																														
                        FROM VPM_CodeKb																														
                        WHERE VPM_CodeKb.CodeSyu = 'UNSOUKBN'																														
                                AND VPM_CodeKb.SiyoKbn = 1																														
                                AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																														
                        )																														
        ) AS JM_UnsouKbn ON Main.UnsouKbn = CONVERT(TINYINT, JM_UnsouKbn.CodeKbn)																														
        LEFT JOIN (																														
        SELECT CodeKbn																														
                ,CodeKbnNm																														
                ,RyakuNm																														
        FROM VPM_CodeKb																														
        WHERE CodeSyu = 'KATAKBN'																														
                AND SiyoKbn = 1																														
                AND VPM_CodeKb.TenantCdSeq = (																														
                        SELECT CASE 																														
                                        WHEN COUNT(*) = 0																														
                                                THEN 0																														
                                        ELSE　@TenantCdSeq																														
                                        END AS TenantCdSeq																														
                        FROM VPM_CodeKb																														
                        WHERE VPM_CodeKb.CodeSyu = 'KATAKBN'																														
                                AND VPM_CodeKb.SiyoKbn = 1																														
                                AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																														
                        )																														
        ) AS JM_KataKbn ON Main.KataKbn = CONVERT(TINYINT, JM_KataKbn.CodeKbn)																														
　　　　 WHERE 1 = 1									
                  AND (@CompnyCd IS NULL OR ISNULL(JM_Compny.CompanyCd, 0) = @CompnyCd)
                  AND (@StrEigyoCd IS NULL OR JM_Eigyos.EigyoCd >= @StrEigyoCd)
                  AND (@EndEigyoCd  IS NULL OR JM_Eigyos.EigyoCd <= @EndEigyoCd )
                  AND (@StrUnsouKbn IS NULL OR Main.UnsouKbn >= @StrUnsouKbn)
                  AND (@EndUnsouKbn IS NULL OR Main.UnsouKbn <= @EndUnsouKbn)
                  AND (@TenantCdSeq  IS NULL OR JM_Tenant.TenantCdSeq = @TenantCdSeq)
　　　　ORDER BY UnsouKbn																														
　　　　        ,JM_Eigyos.EigyoCd																														
　　　　        ,KataKbn																																													
　　　　	END
　　　　SET	@ROWCOUNT	=	@@ROWCOUNT
RETURN																													

