USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dTransportActualResult_R]    Script Date: 2020/09/03 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   TransportActualResult
-- SP-ID		:   PK_dTransportActualResult_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetTransportActualResult
-- Date			:   2020/09/03
-- Author		:   Tra Nguyen 
-- Description	:   Get data for Transport Actual Result report
------------------------------------------------------------
CREATE OR ALTER   PROCEDURE [dbo].[PK_dTransportActualResult_R]
	(
	-- Parameter
		@SyoriYmdStr			char(6),			-- 入力した年度の4月								
		@SyoriYmdEnd			char(6),			-- 入力した年度の来年の3月
		@CompnyCd				int,
		@StrEigyoCd				int,
		@EndEigyoCd				int,
		@StrUnsouKbn			int,
		@EndUnsouKbn			int,
		@TenantCdSeq			int,			-- ログインしたユーザーのTenantCdSeq
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS
BEGIN
	SELECT UnsouKbn
        ,ISNULL(JM_UnsouKbn.CodeKbnNm, '') AS UnsouKbnNm
        ,ISNULL(JM_UnsouKbn.RyakuNm, '') AS UnsouKbnRyaku
        ,SUM(NobeJyoCnt) AS TotalNobeJyoCnt
        ,SUM(NobeRinCnt) AS TotalNobeRinCnt
        ,SUM(NobeSumCnt) AS TotalNobeSumCnt
        ,SUM(NobeJitCnt) AS TotalNobeJitCnt
        ,SUM(JitJisaKm) AS TotalJitJisaKm
        ,SUM(JitKisoKm) AS TotalJitKisoKm
        ,SUM(JitJisaKm + JitKisoKm) AS TotalJitSumKm
        ,SUM(YusoJin) AS TotalYusoJin
        ,SUM(UnkoCnt) AS TotalUnkoCnt
        ,SUM(UnkoKikak1Cnt) AS TotalUnkoKikak1Cnt
        ,SUM(UnkoKikak2Cnt) AS TotalUnkoKikak2Cnt
        ,SUM(UnkoOthCnt) AS TotalUnkoOthCnt
        ,SUM(UnkoKikak1Cnt + UnkoKikak2Cnt + UnkoOthCnt) AS TotalUnkoOthAllCnt
        ,SUM(UnsoSyu) AS TotalUnsoSyu
FROM (
        SELECT UnsouKbn
                ,EigyoCdSeq
                ,KataKbn
                ,SyoriYm
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
                AND TKD_JitHou.SyoriYm >= @SyoriYmdStr				
                AND TKD_JitHou.SyoriYm <= @SyoriYmdEnd
        GROUP BY UnsouKbn
                ,EigyoCdSeq
                ,KataKbn
                ,SyoriYm
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
                                        ELSE @TenantCdSeq
                                        END AS TenantCdSeq
                        FROM VPM_CodeKb
                        WHERE VPM_CodeKb.CodeSyu = 'KATAKBN'
                                AND VPM_CodeKb.SiyoKbn = 1
                                AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq
                        )
        ) AS JM_KataKbn ON Main.KataKbn = CONVERT(TINYINT, JM_KataKbn.CodeKbn)
WHERE   ISNULL(JM_Compny.CompanyCd, 0) = @CompnyCd
        AND JM_Tenant.TenantCdSeq = @TenantCdSeq
        AND JM_Eigyos.EigyoCd >= CASE WHEN @StrEigyoCd IS NULL THEN JM_Eigyos.EigyoCd ELSE @StrEigyoCd END
        AND JM_Eigyos.EigyoCd <= CASE WHEN @EndEigyoCd  IS NULL THEN JM_Eigyos.EigyoCd ELSE @EndEigyoCd  END
        AND Main.UnsouKbn >= CASE WHEN @StrUnsouKbn  IS NULL THEN Main.UnsouKbn ELSE @StrUnsouKbn  END
        AND Main.UnsouKbn <= CASE WHEN @EndUnsouKbn  IS NULL THEN Main.UnsouKbn ELSE @EndUnsouKbn  END
GROUP BY UnsouKbn
        ,JM_UnsouKbn.CodeKbnNm
        ,JM_UnsouKbn.RyakuNm
ORDER BY UnsouKbn

SET @ROWCOUNT = @@ROWCOUNT
END
