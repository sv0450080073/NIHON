USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dAdvancePaymentDetailHeaderList_R]    Script Date: 2021/06/10 10:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	[PK_dAdvancePaymentDetailHeaderList_R]
-- DB-Name		:	[HOC_Kashikiri]
-- Name			:	立替明細書ヘッダーデータ取得
-- Date			:	2021/06/10
-- Author		:	N.T.HIEU
-- Description	:	立替明細書ヘッダーデータ取得
---------------------------------------------------
-- Update		: nhhkieuanh-2021/03/03
-- Comment		: Cast int to bigint type
----------------------------------------------------

ALTER         PROCEDURE [dbo].[PK_dAdvancePaymentDetailHeaderList_R]
	(
			@TenantCdSeq		INT				-- テナントコード
		,	@ReceiptNumber		NVARCHAR(MAX)		-- 受付番号
		,	@DispatchDate		VARCHAR(8)		-- 配車日付
		,	@ArrivalDate		VARCHAR(8)		-- 到着日付
		--,	@StartBillAddress	VARCHAR(12)		-- 請求先開始
		--,	@EndBillAddress		VARCHAR(12)		-- 請求先終了
		,	@StartCustomer		VARCHAR(12)		-- 得意先開始
		,	@EndCustomer		VARCHAR(12)		-- 得意先終了
		,   @GyosyaFrom         INT
	    ,   @GyosyaTo           INT
	    ,   @TokuiFrom          INT
	    ,   @TokuiTo            INT
	    ,   @SitenFrom          INT
	    ,   @SitenTo            INT
	)
AS
BEGIN
SELECT ISNULL(eTKD_Yyksho11.UkeNo, 0) AS UkeNo,
	ISNULL(eTKD_Unkobi11.UnkRen, 0) AS UnkRen,
	MAX(ISNULL(eTKD_Unkobi11.DanTaNm, '')) AS DanTaNm,
	MAX(ISNULL(eTKD_Unkobi11.HaiSYmd, '')) AS HaiSYmd,
	MAX(ISNULL(eTKD_Unkobi11.TouYmd, '')) AS TouYmd,
	MAX(ISNULL(eVPM_Gyosya11.GyosyaCd, '')) AS GyosyaCd,
	MAX(ISNULL(eVPM_Tokisk11.TokuiCd, 0)) AS TokuiCd,
	MAX(ISNULL(eVPM_Tokisk11.TokuiNm, '')) AS TokuiNm,
	MAX(ISNULL(eVPM_TokiSt11.SitenCd, 0)) AS SitenCd,
	MAX(ISNULL(eVPM_TokiSt11.SitenNm, '')) AS SitenNm,
	MAX(ISNULL(eVPM_Tokisk12.TokuiNm, '')) AS SeiTokuiNm,
	MAX(ISNULL(eVPM_TokiSt12.SitenNm, '')) AS SeiSitenNm,
	MAX(ISNULL(eVPM_Eigyos11.ZipCd, '')) AS SeiEigZipCd,
	MAX(ISNULL(eVPM_Eigyos11.Jyus1, '')) AS SeiEigJyus1,
	MAX(ISNULL(eVPM_Eigyos11.Jyus2, '')) AS SeiEigJyus2,
	MAX(ISNULL(eVPM_Eigyos11.EigyoNm, '')) AS SeiEigEigyoNm,
	MAX(ISNULL(eVPM_Eigyos11.TelNo, '')) AS SeiEigTelNo,
	MAX(ISNULL(eVPM_Eigyos11.FaxNo, '')) AS SeiEigFaxNo,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn1 IN (1) THEN ISNULL(eVPM_Bank11.BankNm, '')
			ELSE ''
		END) AS BankNm1,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn1 IN (1) THEN ISNULL(eVPM_BankSt11.BankSitNm, '')
			ELSE ''
		END) AS BankSitNm1,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn1 IN (1) THEN ISNULL(eVPM_CodeKb11.CodeKbnNm, '')
			ELSE ''
		END) AS YokinSyuNm1,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn1 IN (1) THEN ISNULL(eVPM_Eigyos11.KouzaNo1, '')
			ELSE ''
		END) AS KouzaNo1,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn2 IN (1) THEN ISNULL(eVPM_Bank12.BankNm, '')
			ELSE ''
		END) AS BankNm2,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn2 IN (1) THEN ISNULL(eVPM_BankSt12.BankSitNm, '')
			ELSE ''
		END) AS BankSitNm2,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn2 IN (1) THEN ISNULL(eVPM_CodeKb12.CodeKbnNm, '')
			ELSE ''
		END) AS YokinSyuNm2,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn2 IN (1) THEN ISNULL(eVPM_Eigyos11.KouzaNo2, '')
			ELSE ''
		END) AS KouzaNo2,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn3 IN (1) THEN ISNULL(eVPM_Bank13.BankNm, '')
			ELSE ''
		END) AS BankNm3,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn3 IN (1) THEN ISNULL(eVPM_BankSt13.BankSitNm, '')
			ELSE ''
		END) AS BankSitNm3,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn3 IN (1) THEN ISNULL(eVPM_CodeKb13.CodeKbnNm, '')
			ELSE ''
		END) AS YokinSyuNm3,
	MAX(CASE
			WHEN eVPM_TokEig11.BankKbn3 IN (1) THEN ISNULL(eVPM_Eigyos11.KouzaNo3, '')
			ELSE ''
		END) AS KouzaNo3,
	MAX(ISNULL(eVPM_Eigyos11.KouzaMeigi, '')) AS KouzaMeigi,
	MAX(ISNULL(eVPM_Compny11.CompanyNm, '')) AS SeiEigCompanyNm,
	CAST(SUM(CASE
			WHEN eTKD_MFutTu11.Suryo <> 0 THEN CAST(ISNULL(eTKD_MFutTu11.UriGakKin, 0) AS BIGINT) + CAST(ISNULL(eTKD_MFutTu11.SyaRyoSyo, 0) AS BIGINT) 
			ELSE CAST(ISNULL(eTKD_FutTum11.UriGakKin, 0) AS BIGINT) + CAST(ISNULL(eTKD_FutTum11.SyaRyoSyo, 0) AS BIGINT)
		END) AS bigint) AS KinGaku,
	0 AS EtcGaku,
	0 AS TatekaeGaku,
	MAX(SyaSyuDaiCnt) AS SyaSyuDaiCnt 
FROM TKD_Yyksho AS eTKD_Yyksho11
INNER JOIN
	(SELECT UkeNo,
		MIN(UnkRen) AS UnkRen
	FROM TKD_Unkobi
	WHERE SiyoKbn = 1
	GROUP BY UkeNo) AS eTKD_Unkobi01
	ON eTKD_Yyksho11.UkeNo = eTKD_Unkobi01.UkeNo
LEFT JOIN TKD_Unkobi AS eTKD_Unkobi11 
	ON eTKD_Unkobi01.UkeNo = eTKD_Unkobi11.UkeNo
	AND eTKD_Unkobi01.UnkRen = eTKD_Unkobi11.UnkRen
	AND eTKD_Unkobi11.SiyoKbn = 1
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11
	ON eTKD_Yyksho11.TokuiSeq = eVPM_Tokisk11.TokuiSeq
	AND eTKD_Yyksho11.SeiTaiYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd AND eVPM_Tokisk11.SiyoEndYmd
	AND eVPM_Tokisk11.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya11
	ON eVPM_Tokisk11.GyosyaCdSeq = eVPM_Gyosya11.GyosyaCdSeq
	AND eVPM_Gyosya11.TenantCdSeq = eVPM_Tokisk11.TenantCdSeq -- 2021/05/28 ADD
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt11
	ON eTKD_Yyksho11.TokuiSeq = eVPM_TokiSt11.TokuiSeq
	AND eTKD_Yyksho11.SitenCdSeq = eVPM_TokiSt11.SitenCdSeq
	AND eTKD_Yyksho11.SeiTaiYmd BETWEEN eVPM_TokiSt11.SiyoStaYmd AND eVPM_TokiSt11.SiyoEndYmd
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk12
	ON eVPM_TokiSt11.SeiCdSeq = eVPM_Tokisk12.TokuiSeq
	AND eTKD_Yyksho11.SeiTaiYmd BETWEEN eVPM_Tokisk12.SiyoStaYmd AND eVPM_Tokisk12.SiyoEndYmd
	AND eVPM_Tokisk12.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya12
	ON eVPM_Tokisk12.GyosyaCdSeq = eVPM_Gyosya12.GyosyaCdSeq
	AND eVPM_Gyosya12.TenantCdSeq = eVPM_Tokisk12.TenantCdSeq -- 2021/05/28 ADD
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt12
	ON eVPM_TokiSt11.SeiCdSeq = eVPM_TokiSt12.TokuiSeq
	AND eVPM_TokiSt11.SeiSitenCdSeq = eVPM_TokiSt12.SitenCdSeq
	AND eTKD_Yyksho11.SeiTaiYmd BETWEEN eVPM_TokiSt12.SiyoStaYmd AND eVPM_TokiSt12.SiyoEndYmd
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos11
	ON eTKD_Yyksho11.SeiEigCdSeq = eVPM_Eigyos11.EigyoCdSeq
	AND eVPM_Eigyos11.SiyoKbn = 1
LEFT JOIN VPM_TokEig AS eVPM_TokEig11
	ON eVPM_TokiSt12.TokuiSeq = eVPM_TokEig11.TokuiSeq
	AND eVPM_TokiSt12.SitenCdSeq = eVPM_TokEig11.SitenCdSeq
	AND eVPM_TokEig11.EigyoCdSeq = eVPM_Eigyos11.EigyoCdSeq
	AND eTKD_Yyksho11.SeiTaiYmd BETWEEN eVPM_TokEig11.SiyoStaYmd AND eVPM_TokEig11.SiyoEndYmd
LEFT JOIN VPM_Compny AS eVPM_Compny11
	ON eVPM_Eigyos11.CompanyCdSeq = eVPM_Compny11.CompanyCdSeq
	AND eVPM_Compny11.SiyoKbn = 1
LEFT JOIN VPM_Bank AS eVPM_Bank11
	ON eVPM_Eigyos11.BankCd1 = eVPM_Bank11.BankCd
LEFT JOIN VPM_Bank AS eVPM_Bank12
	ON eVPM_Eigyos11.BankCd2 = eVPM_Bank12.BankCd
LEFT JOIN VPM_Bank AS eVPM_Bank13
	ON eVPM_Eigyos11.BankCd3 = eVPM_Bank13.BankCd
LEFT JOIN VPM_BankSt AS eVPM_BankSt11
	ON eVPM_Eigyos11.BankCd1 = eVPM_BankSt11.BankCd
	AND eVPM_Eigyos11.BankSitCd1 = eVPM_BankSt11.BankSitCd
LEFT JOIN VPM_BankSt AS eVPM_BankSt12
	ON eVPM_Eigyos11.BankCd2 = eVPM_BankSt12.BankCd
	AND eVPM_Eigyos11.BankSitCd2 = eVPM_BankSt12.BankSitCd
LEFT JOIN VPM_BankSt AS eVPM_BankSt13
	ON eVPM_Eigyos11.BankCd3 = eVPM_BankSt13.BankCd
	AND eVPM_Eigyos11.BankSitCd3 = eVPM_BankSt13.BankSitCd
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb11
	ON eVPM_CodeKb11.CodeSyu = 'YOKINSYU'
	AND eVPM_Eigyos11.YokinSyu1 = eVPM_CodeKb11.CodeKbn
	AND eVPM_CodeKb11.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb12
	ON eVPM_CodeKb12.CodeSyu = 'YOKINSYU'
	AND eVPM_Eigyos11.YokinSyu2 = eVPM_CodeKb12.CodeKbn
	AND eVPM_CodeKb12.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb13
	ON eVPM_CodeKb13.CodeSyu = 'YOKINSYU'
	AND eVPM_Eigyos11.YokinSyu3 = eVPM_CodeKb13.CodeKbn
	AND eVPM_CodeKb13.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
INNER JOIN TKD_FutTum AS eTKD_FutTum11
	ON eTKD_Unkobi01.UkeNo = eTKD_FutTum11.UkeNo
	AND eTKD_Unkobi01.UnkRen = eTKD_FutTum11.UnkRen
	AND eTKD_FutTum11.FutTumKbn = 1
	AND eTKD_FutTum11.SiyoKbn = 1
INNER JOIN VPM_Futai AS eVPM_Futai11
	ON eTKD_FutTum11.FutTumCdSeq = eVPM_Futai11.FutaiCdSeq
	AND eVPM_Futai11.SiyoKbn = 1
	AND eVPM_Futai11.FutGuiKbn <> 5
LEFT JOIN TKD_MFutTu AS eTKD_MFutTu11
	ON eTKD_FutTum11.UkeNo = eTKD_MFutTu11.UkeNo
	AND eTKD_FutTum11.UnkRen = eTKD_MFutTu11.UnkRen
	AND eTKD_FutTum11.FutTumKbn = eTKD_MFutTu11.FutTumKbn
	AND eTKD_FutTum11.FutTumRen = eTKD_MFutTu11.FutTumRen
	AND eTKD_MFutTu11.SiyoKbn = 1
	AND eTKD_MFutTu11.Suryo <> 0
LEFT JOIN TKD_Haisha AS eTKD_Haisha11
	ON eTKD_Unkobi01.UkeNo = eTKD_Haisha11.UkeNo
	AND eTKD_Unkobi01.UnkRen = eTKD_Haisha11.UnkRen
	AND eTKD_MFutTu11.TeiDanNo = eTKD_Haisha11.TeiDanNo
	AND eTKD_MFutTu11.BunkRen = eTKD_Haisha11.BunkRen
	AND eTKD_Haisha11.SiyoKbn = 1
LEFT JOIN
	(SELECT UkeNo,
		UnkRen,
		SUM(SyaSyuDai) AS SyaSyuDaiCnt
	FROM TKD_YykSyu
	WHERE SiyoKbn = 1
	GROUP BY UkeNo,
		UnkRen) AS eTKD_YykSyu11
	ON eTKD_Unkobi11.UkeNo = eTKD_YykSyu11.UkeNo
	AND eTKD_Unkobi11.UnkRen = eTKD_YykSyu11.UnkRen
WHERE eTKD_Yyksho11.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
　　　　　AND eTKD_Yyksho11.YoyaSyu = 1
　　　　　AND eTKD_FutTum11.SeisanKbn = 1
　　　　　AND eTKD_FutTum11.FutTumKbn = 1
　　　　　AND (@ReceiptNumber IS NULL OR @ReceiptNumber = '' OR eTKD_Yyksho11.UkeNo IN (select value from string_split(@ReceiptNumber, ',')))				-- 予約番号
　　　　　AND (@DispatchDate IS NULL OR @DispatchDate = '' OR eTKD_Unkobi11.HaiSYmd >= @DispatchDate)							-- 配車日
		 AND (@ArrivalDate IS NULL OR @ArrivalDate = '' OR eTKD_Unkobi11.TouYmd <= @ArrivalDate)								-- 到着日
　　　　　--AND (@StartBillAddress IS NULL OR @StartBillAddress = '' OR FORMAT(eVPM_Gyosya12.GyosyaCd, '000') + FORMAT(eVPM_Tokisk12.TokuiCd, '0000') + FORMAT(eVPM_TokiSt12.SitenCd, '0000') >= @StartBillAddress)      -- 請求先 開始
　　　　　--AND (@EndBillAddress IS NULL OR @EndBillAddress = '' OR FORMAT(eVPM_Gyosya12.GyosyaCd, '000') + FORMAT(eVPM_Tokisk12.TokuiCd, '0000') + FORMAT(eVPM_TokiSt12.SitenCd, '0000') <= @EndBillAddress)        -- 請求先 終了
         AND (@GyosyaFrom = 0 OR (FORMAT(eVPM_Gyosya12.GyosyaCd,'000') + FORMAT(eVPM_Tokisk12.TokuiCd,'0000') + FORMAT(eVPM_TokiSt12.SitenCd,'0000')) >= (FORMAT(@GyosyaFrom,'000') + FORMAT(@TokuiFrom,'0000') + FORMAT(@SitenFrom,'0000'))) 																																				
	     AND (@GyosyaTo = 0 OR (FORMAT(eVPM_Gyosya12.GyosyaCd,'000') + FORMAT(eVPM_Tokisk12.TokuiCd,'0000') + FORMAT(eVPM_TokiSt12.SitenCd,'0000')) <= (FORMAT(@GyosyaTo,'000') + FORMAT(@TokuiTo,'0000') + FORMAT(@SitenTo,'0000')))	
　　　　　AND (@StartCustomer IS NULL OR @StartCustomer = '' OR FORMAT(eVPM_Gyosya11.GyosyaCd, '000') + FORMAT(eVPM_Tokisk11.TokuiCd, '0000') +	FORMAT(eVPM_TokiSt11.SitenCd, '0000') >= @StartCustomer)			-- 請求先 開始
　　　　　AND (@EndCustomer IS NULL OR @EndCustomer = '' OR FORMAT(eVPM_Gyosya11.GyosyaCd, '000') + FORMAT(eVPM_Tokisk11.TokuiCd, '0000') +	FORMAT(eVPM_TokiSt11.SitenCd, '0000') <= @EndCustomer)			-- 請求先 終了
GROUP BY ISNULL(eTKD_Yyksho11.UkeNo, 0), ISNULL(eTKD_Unkobi11.UnkRen, 0)
END