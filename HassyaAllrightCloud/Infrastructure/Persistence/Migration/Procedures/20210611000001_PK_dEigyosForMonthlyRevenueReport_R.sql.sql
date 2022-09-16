USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[PK_dEigyosForMonthlyRevenueReport_R]    Script Date: 2020/08/14 16:52:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   RevenueSummary
-- SP-ID		:   PK_dEigyosForMonthlyRevenueReport_R
-- DB-Name		:   HOC_Master
-- Name			:   GetEigyoListForMonthlyRevenueReport
-- Date			:   2020/08/12
-- Author		:   Tra Nguyen 
-- Description	:   Get Eigyo List For Monthly Transportation Revenue Report
------------------------------------------------------------
CREATE OR ALTER  PROCEDURE [dbo].[PK_dEigyosForMonthlyRevenueReport_R]
	(
	-- Parameter
		@UriYmdFrom				CHAR(8), -- format yyyyMMdd
		@UriYmdTo				CHAR(8), -- format yyyyMMdd
		@CompanyCd				INT,
		@EigyoCdFrom			INT,
		@EigyoCdTo				INT,
		@UkeNoFrom				CHAR(15),
		@UkeNoTo				CHAR(15),
		@YoyaKbnFrom			INT,
		@YoyaKbnTo				INT,
		@TenantCdSeq			INT,
		@EigyoKbn				INT,
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS
BEGIN
SELECT DISTINCT eVPM_Eigyos.EigyoCd AS CEigyoCd
    ,eVPM_Eigyos.EigyoCdSeq AS CEigyoCdSeq
    ,eVPM_Eigyos.EigyoNm AS CEigyoNm
    ,eVPM_Eigyos.RyakuNm AS CEigyoRyakuNm
FROM TKD_Yyksho AS eTKD_Yyksho
JOIN TKD_Unkobi AS eTKD_Unkobi ON eTKD_Unkobi.UkeNo = eTKD_Yyksho.UkeNo
JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_Eigyos.EigyoCdSeq = CASE WHEN @EigyoKbn = 1 THEN eTKD_Yyksho.SeiEigCdSeq 
																ELSE eTKD_Yyksho.UkeEigCdSeq END --@EigyoKbn = 2
JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Tenant.TenantCdSeq = eVPM_Compny.TenantCdSeq
JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eVPM_YoyKbn.YoyaKbnSeq = eTKD_Yyksho.YoyaKbnSeq
        AND eVPM_YoyKbn.TenantCdSeq = @TenantCdSeq
WHERE		    (@UriYmdFrom IS NULL OR eTKD_Unkobi.UriYmd >= @UriYmdFrom)
		AND (@UriYmdTo IS NULL OR eTKD_Unkobi.UriYmd <= @UriYmdTo)
		AND (@CompanyCd IS NULL OR eVPM_Compny.CompanyCd = @CompanyCd)
		AND (@EigyoCdFrom IS NULL OR eVPM_Eigyos.EigyoCd >= @EigyoCdFrom)
		AND (@EigyoCdTo IS NULL OR eVPM_Eigyos.EigyoCd <= @EigyoCdTo)
		AND (@UkeNoFrom IS NULL OR eTKD_Yyksho.UkeNo >= @UkeNoFrom)
		AND (@UkeNoTo IS NULL OR eTKD_Yyksho.UkeNo <= @UkeNoTo)
        AND (@YoyaKbnFrom IS NULL OR eVPM_YoyKbn.YoyaKbn >= @YoyaKbnFrom)
        AND (@YoyaKbnTo IS NULL OR eVPM_YoyKbn.YoyaKbn <= @YoyaKbnTo)
        AND (@TenantCdSeq IS NULL OR eVPM_Tenant.TenantCdSeq = @TenantCdSeq)
ORDER BY CEigyoCd
        ,CEigyoCdSeq
        ,CEigyoNm
        ,CEigyoRyakuNm
OPTION (FORCE ORDER)
SET @ROWCOUNT = @@ROWCOUNT
END
GO


