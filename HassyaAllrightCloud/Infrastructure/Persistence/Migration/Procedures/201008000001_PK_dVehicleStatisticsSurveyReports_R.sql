-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAllrightCloud
-- Module-Name	:   HassyaAllrightCloud
-- SP-ID		:   PK_dVehicleStatisticsSurveyReports_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Vehicle Statistics Survey List
-- Date			:   2020/10/08
-- Author		:   P.M.NHAT
-- Description	:   Get vehicle statistics survey list with conditions for report
-- =============================================
CREATE OR ALTER PROCEDURE PK_dVehicleStatisticsSurveyReports_R 
	-- Add the parameters for the stored procedure here
	@Date			varchar(6),
	@EndOfMonth		varchar(8),
	@LastMonth		varchar(6),
	@CompnyCd		int,
	@StrEigyoCd		int,
	@EndEigyoCd		int,
	@StrUnsouKbn	tinyint,
	@EndUnsouKbn	tinyint,
	@TenantCdSeq	int
AS
BEGIN
	SELECT ISNULL(Main.UnsouKbn, 0) AS UnsouKbn
        ,ISNULL(JM_UnsouKbn.CodeKbnNm, '') AS UnsouKbnNm
        ,ISNULL(JM_UnsouKbn.RyakuNm, '') AS UnsouKbnRyaku
        ,ISNULL(Main.NenryoKbn, 0) AS NenryoKbn
        ,ISNULL(JM_NenryoKbn.CodeKbn, '') AS NenryoKbnNm
        ,ISNULL(JM_NenryoKbn.RyakuNm, '') AS NenryoKbnRyaku
        ,ISNULL(Main.YusoJin, 0) AS YusoJin
        ,ISNULL(Main.NobeSumCnt, 0) AS NobeSumCnt
        ,ISNULL(Main.NobeJitCnt, 0) AS NobeJitCnt
        ,ISNULL(Main.JitSumKm, 0) AS JitSumKm
        ,ISNULL(Main.JitJisaKm, 0) AS JitJisaKm
        ,ISNULL(Main.JitKisoKm, 0) AS JitKisoKm
        ,ISNULL(Main.UnkoCnt, 0) AS UnkoCnt
        ,ISNULL(Main.NobeRinCnt, 0) AS NobeRinCnt
        ,ISNULL(JT_EndOfMonthCnt.EndOfMonthCnt, 0) AS EndOfMonthCnt
        ,ISNULL(JT_LastMonth.YusoJin, 0) AS LastMonthYusoJin
	FROM (
			SELECT UnsouKbn
					,NenryoKbn
					,SUM(YusoJin) AS YusoJin
					,SUM(NobeSumCnt) AS NobeSumCnt
					,SUM(NobeJitCnt) AS NobeJitCnt
					,SUM(JitJisaKm) + SUM(JitKisoKm) AS JitSumKm
					,SUM(JitJisaKm) AS JitJisaKm
					,SUM(JitKisoKm) AS JitKisoKm
					,SUM(UnkoCnt) AS UnkoCnt
					,SUM(NobeRinCnt) AS NobeRinCnt
			FROM TKD_JitHou
			LEFT JOIN VPM_Eigyos AS JM_Eigyos ON TKD_JitHou.EigyoCdSeq = JM_Eigyos.EigyoCdSeq
					AND JM_Eigyos.SiyoKbn = 1
			LEFT JOIN VPM_Compny AS JM_Compny ON JM_Eigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq
					AND JM_Compny.SiyoKbn = 1
			LEFT JOIN VPM_Tenant AS JM_Tenant ON JM_Compny.TenantCdSeq = JM_Tenant.TenantCdSeq
					AND JM_Tenant.SiyoKbn = 1
			WHERE TKD_JitHou.SyoriYm = @Date
					AND ISNULL(JM_Compny.CompanyCd, 0) = @CompnyCd
					AND (@StrEigyoCd = 0 OR JM_Eigyos.EigyoCd >= @StrEigyoCd)
					AND (@EndEigyoCd = 0 OR JM_Eigyos.EigyoCd <= @EndEigyoCd)
			GROUP BY UnsouKbn
					,NenryoKbn
			) AS Main
	LEFT JOIN (
			SELECT UnsoKbn
					,COUNT(UnsoKbn) AS EndOfMonthCnt
			FROM (
					SELECT *
							,CASE 
									WHEN VPM_SyaRyo.NinkaKbn = 1
											THEN 1
									WHEN VPM_SyaRyo.NinkaKbn = 3
											THEN 2
									WHEN VPM_SyaRyo.NinkaKbn <> 1
											AND VPM_SyaRyo.NinkaKbn <> 3
											THEN 3
									END UnsoKbn
					FROM VPM_SyaRyo
					WHERE VPM_SyaRyo.NinkaKbn <> 7
					) AS VPM_SyaRyo
			INNER JOIN VPM_SyaSyu ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
					AND VPM_SyaSyu.SiyoKbn = 1
			INNER JOIN VPM_HenSya ON VPM_SyaRyo.SyaRyoCdSeq = VPM_HenSya.SyaRyoCdSeq
					AND VPM_HenSya.StaYmd <= @EndOfMonth
					AND VPM_HenSya.ENDYmd >= @EndOfMonth
			LEFT JOIN VPM_Eigyos ON VPM_HenSya.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
					AND VPM_Eigyos.SiyoKbn = 1
			LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
					AND VPM_Compny.SiyoKbn = 1
			LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
					AND VPM_Tenant.SiyoKbn = 1
			WHERE ISNULL(VPM_Compny.CompanyCd, 0) = @CompnyCd
					AND (@StrEigyoCd = 0 OR VPM_Eigyos.EigyoCd >= @StrEigyoCd)
					AND (@EndEigyoCd = 0 OR VPM_Eigyos.EigyoCd <= @EndEigyoCd)
			GROUP BY VPM_SyaRyo.UnsoKbn
			) AS JT_EndOfMonthCnt ON Main.UnsouKbn = JT_EndOfMonthCnt.UnsoKbn
	LEFT JOIN (
			SELECT UnsouKbn
					,SUM(YusoJin) AS YusoJin
			FROM TKD_JitHou
			LEFT JOIN VPM_Eigyos AS JM_Eigyos ON TKD_JitHou.EigyoCdSeq = JM_Eigyos.EigyoCdSeq
					AND JM_Eigyos.SiyoKbn = 1
			LEFT JOIN VPM_Compny AS JM_Compny ON JM_Eigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq
					AND JM_Compny.SiyoKbn = 1
			LEFT JOIN VPM_Tenant AS JM_Tenant ON JM_Compny.TenantCdSeq = JM_Tenant.TenantCdSeq
					AND JM_Tenant.SiyoKbn = 1
			WHERE TKD_JitHou.SyoriYm = @LastMonth
					AND ISNULL(JM_Compny.CompanyCd, 0) = @CompnyCd
					AND (@StrEigyoCd = 0 OR JM_Eigyos.EigyoCd >= @StrEigyoCd)
					AND (@EndEigyoCd = 0 OR JM_Eigyos.EigyoCd <= @EndEigyoCd)
			GROUP BY UnsouKbn
			) AS JT_LastMonth ON Main.UnsouKbn = JT_LastMonth.UnsouKbn
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
			WHERE CodeSyu = 'NENRYOKBN'
					AND SiyoKbn = 1
					AND VPM_CodeKb.TenantCdSeq = (
							SELECT CASE 
											WHEN COUNT(*) = 0
													THEN 0
											ELSE @TenantCdSeq
											END AS TenantCdSeq
							FROM VPM_CodeKb
							WHERE VPM_CodeKb.CodeSyu = 'NENRYOKBN'
									AND VPM_CodeKb.SiyoKbn = 1
									AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq
							)
			) AS JM_NenryoKbn ON Main.NenryoKbn = CONVERT(TINYINT, JM_NenryoKbn.CodeKbn)
	WHERE 1 = 1
			AND (@StrUnsouKbn = 0 OR Main.UnsouKbn >= @StrUnsouKbn)
			AND (@EndUnsouKbn = 0 OR Main.UnsouKbn <= @EndUnsouKbn)
	ORDER BY Main.UnsouKbn
			,Main.NenryoKbn
END
GO
