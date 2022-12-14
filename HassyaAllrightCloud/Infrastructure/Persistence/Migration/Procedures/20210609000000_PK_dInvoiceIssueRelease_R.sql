USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dInvoiceIssueRelease_R]    Script Date: 3/18/2021 3:10:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetInvoiceIssueReleasesAsyncQuery
-- Date			:   2020/10/27
-- Author		:   N.T.HIEU
-- Description	:   Get invoice issue release data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dInvoiceIssueRelease_R] 
	(
	-- Parameter
		 @TenantCdSeq			INT,
		 @BillOutputSeq			VARCHAR(10),				
		 @BillSerialNumber		VARCHAR(10),
		 @StartBillIssuedDate	VARCHAR(10),
		 @EndBillIssuedDate		VARCHAR(10),
		 @StartBillAddress		VARCHAR(11),
		 @EndBillAddress		VARCHAR(11),
		 @Offset				INT,					--Offset rows data
		 @Limit					INT,					--Limit rows data
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0
	-- Processing
BEGIN
WITH eTKD_SeiMei03 AS (
	SELECT TKD_SeiMei.SeiOutSeq,
		TKD_SeiMei.SeiRen,
		MIN(TKD_Yyksho.SeiTaiYmd) AS MinSeiTaiYmd,
		MAX(TKD_Yyksho.SeiTaiYmd) AS MaxSeiTaiYmd,
		MAX(TKD_SeiPrS.SeiHatYmd) AS SeiHatYmd
	FROM TKD_SeiMei
	INNER JOIN TKD_Yyksho
		ON TKD_SeiMei.UkeNo = TKD_Yyksho.UkeNo
		AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq 
	    LEFT JOIN TKD_SeiPrS
		ON TKD_SeiPrS.SeiOutSeq = TKD_SeiMei.SeiOutSeq
	WHERE TKD_SeiMei.TenantCdSeq = @TenantCdSeq
	GROUP BY TKD_SeiMei.SeiOutSeq,
		TKD_SeiMei.SeiRen
)

SELECT COUNT(*) OVER(ORDER BY (SELECT NULL)) AS CountNumber,
	TKD_Seikyu.*,
	eTKD_SeiPrS01.*,
	ISNULL(eVPM_Eigyos01.EigyoCdSeq, 0) AS SeiEigyoCdSeq,
	ISNULL(eVPM_Eigyos01.EigyoCd , 0) AS SeiEigyoCd,
	ISNULL(eVPM_Eigyos01.EigyoNm , ' ') AS SeiEigyoNm,
	ISNULL(eVPM_Eigyos01.RyakuNm , ' ') AS SeiEigyoRyak,
	ISNULL(eVPM_Gyosya02.GyosyaCd , 0) AS SeiGyosyaCd,
	ISNULL(eVPM_Gyosya02.GyosyaNm , ' ') AS SeiGyosyaCdNm,
	ISNULL(eVPM_Tokisk02.TokuiCd , 0) AS SeiCd,
	ISNULL(eVPM_Tokisk02.TokuiNm , ' ') AS SeiCdNm,
	ISNULL(eVPM_Tokisk02.RyakuNm , ' ') AS SeiRyakuNm,
	ISNULL(eVPM_TokiSt02.SitenCd , 0) AS SeiSitenCd,
	ISNULL(eVPM_TokiSt02.SitenNm , ' ') AS SeiSitenCdNm,
	ISNULL(eVPM_TokiSt02.RyakuNm , ' ') AS SeiSitRyakuNm,
	ISNULL(eTKD_SeiMei11.MinSeiTaiYmd , ' ') AS MinSeiTaiYmd,
	ISNULL(eTKD_SeiMei11.MaxSeiTaiYmd , ' ') AS MaxSeiTaiYmd,
	ISNULL(eTKD_SeiMei11.SeiHatYmd , ' ') AS SeiHatYmd,
	ISNULL(eVPM_Tokisk02.SiyoStaYmd, 0) AS TSiyoStaYmd,
	ISNULL(eVPM_Tokisk02.SiyoEndYmd, 0) AS TSiyoEndYmd,
	ISNULL(eVPM_TokiSt02.SiyoStaYmd, 0) AS SSiyoStaYmd,
	ISNULL(eVPM_TokiSt02.SiyoEndYmd, 0) AS SSiyoEndYmd
-- 請求書テーブル
FROM TKD_Seikyu
-- 請求書印刷指示
LEFT JOIN TKD_SeiPrS AS eTKD_SeiPrS01
	ON TKD_Seikyu.SeiOutSeq = eTKD_SeiPrS01.SeiOutSeq
	AND TKD_Seikyu.TenantCdSeq = eTKD_SeiPrS01.TenantCdSeq
-- 請求営業所
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
	ON eTKD_SeiPrS01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
-- 得意先支店
INNER JOIN VPM_TokiSt AS eVPM_TokiSt01
	ON TKD_Seikyu.TokuiSeq = eVPM_TokiSt01.TokuiSeq
	AND TKD_Seikyu.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
	AND TKD_Seikyu.SiyoEndYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
-- 請求先
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02 
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
	AND TKD_Seikyu.SiyoEndYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
	AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq
-- 請求先支店
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02
	ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
	AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
	AND TKD_Seikyu.SiyoEndYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
-- 請求先業者
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02
	ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
	AND eVPM_Gyosya02.TenantCdSeq = eVPM_Tokisk02.TenantCdSeq -- 2021/05/27 ADD
-- 請求明細
INNER JOIN eTKD_SeiMei03 AS eTKD_SeiMei11
	ON TKD_Seikyu.SeiOutSeq = eTKD_SeiMei11.SeiOutSeq
	AND TKD_Seikyu.SeiRen = eTKD_SeiMei11.SeiRen
WHERE TKD_Seikyu.SiyoKbn = 1
	AND eTKD_SeiPrS01.SiyoKbn = 1
	AND TKD_Seikyu.TenantCdSeq = @TenantCdSeq
	AND (@BillOutputSeq IS NULL OR @BillOutputSeq = '' OR TKD_Seikyu.SeiOutSeq = CAST(@BillOutputSeq as int))
	AND (@BillSerialNumber IS NULL OR @BillSerialNumber = '' OR TKD_Seikyu.SeiRen = CAST(@BillSerialNumber as smallint))
	AND (@StartBillIssuedDate IS NULL OR @StartBillIssuedDate = '' OR eTKD_SeiPrS01.SeiHatYmd >= @StartBillIssuedDate)
	AND (@EndBillIssuedDate IS NULL OR @EndBillIssuedDate = '' OR eTKD_SeiPrS01.SeiHatYmd <= @EndBillIssuedDate)
	AND (@StartBillAddress IS NULL OR @StartBillAddress = '' OR FORMAT(eVPM_Gyosya02.GyosyaCd, '000') + FORMAT(eVPM_Tokisk02.TokuiCd, '0000') + FORMAT(eVPM_TokiSt02.SitenCd, '0000') >= @StartBillAddress)
	AND (@EndBillAddress IS NULL OR @EndBillAddress = '' OR FORMAT(eVPM_Gyosya02.GyosyaCd, '000') + FORMAT(eVPM_Tokisk02.TokuiCd, '0000') + FORMAT(eVPM_TokiSt02.SitenCd, '0000') <= @EndBillAddress)
	ORDER BY TKD_Seikyu.SeiOutSeq, TKD_Seikyu.SeiRen
	OFFSET @Offset ROWS
FETCH NEXT @Limit ROWS ONLY
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN