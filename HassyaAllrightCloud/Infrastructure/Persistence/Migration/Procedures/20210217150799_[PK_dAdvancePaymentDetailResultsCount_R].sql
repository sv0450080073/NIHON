USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dAdvancePaymentDetailResultsCount_R]    Script Date: 2021/02/17 15:04:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	[]
-- DB-Name		:	[HOC_Kashikiri]
-- Name			:	[PK_dAdvancePaymentDetailResultsCount]
-- Date			:	2020/09/21
-- Author		:	nhhkieuanh
-- Description	:	立替明細書出力画面のデータあるどうか確認
---------------------------------------------------
-- Update		: NTLanAnh-2020/12/14
-- Comment		: Update where conditions clause
----------------------------------------------------

ALTER       PROCEDURE [dbo].[PK_dAdvancePaymentDetailResultsCount_R]
	(
			@TenantCdSeq		INT				-- テナントコード
		,	@ReceiptNumber		NVARCHAR(MAX)		-- 受付番号
		,	@DispatchDate		VARCHAR(8)		-- 配車日付
		,	@ArrivalDate		VARCHAR(8)		-- 到着日付
		,	@StartBillAddress	VARCHAR(12)		-- 請求先開始
		,	@EndBillAddress		VARCHAR(12)		-- 請求先終了
		,	@StartCustomer		VARCHAR(12)		-- 得意先開始
		,	@EndCustomer		VARCHAR(12)		-- 得意先終了
	)
AS
BEGIN
SELECT COUNT(*) AS Count
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
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt12
　　　　　ON eVPM_TokiSt11.SeiCdSeq = eVPM_TokiSt12.TokuiSeq
　　　　　AND eVPM_TokiSt11.SeiSitenCdSeq = eVPM_TokiSt12.SitenCdSeq
　　　　　AND eTKD_Yyksho11.SeiTaiYmd BETWEEN eVPM_TokiSt12.SiyoStaYmd AND eVPM_TokiSt12.SiyoEndYmd
INNER JOIN TKD_FutTum AS eTKD_FutTum11
　　　　　ON eTKD_Unkobi01.UkeNo = eTKD_FutTum11.UkeNo
　　　　　AND eTKD_Unkobi01.UnkRen = eTKD_FutTum11.UnkRen
　　　　　AND eTKD_FutTum11.FutTumKbn = 1
　　　　　AND eTKD_FutTum11.SiyoKbn = 1
WHERE eTKD_Yyksho11.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
　　　　　AND eTKD_Yyksho11.YoyaSyu = 1
　　　　　AND eTKD_FutTum11.SeisanKbn = 1
　　　　　AND eTKD_FutTum11.FutTumKbn = 1
　　　　　AND (@ReceiptNumber IS NULL OR @ReceiptNumber = '' OR eTKD_Yyksho11.UkeNo IN (select value from string_split(@ReceiptNumber, ',')))				-- 予約番号
　　　　　AND (@DispatchDate IS NULL OR @DispatchDate = '' OR eTKD_Unkobi11.HaiSYmd >= @DispatchDate)							-- 配車日
		 AND (@ArrivalDate IS NULL OR @ArrivalDate = '' OR eTKD_Unkobi11.TouYmd <= @ArrivalDate)								-- 到着日
　　　　　AND (@StartBillAddress IS NULL OR @StartBillAddress = '' OR FORMAT(eVPM_Gyosya12.GyosyaCd, '000') + FORMAT(eVPM_Tokisk12.TokuiCd, '0000') + FORMAT(eVPM_TokiSt12.SitenCd, '0000') >= @StartBillAddress)      -- 請求先 開始
　　　　　AND (@EndBillAddress IS NULL OR @EndBillAddress = '' OR FORMAT(eVPM_Gyosya12.GyosyaCd, '000') + FORMAT(eVPM_Tokisk12.TokuiCd, '0000') + FORMAT(eVPM_TokiSt12.SitenCd, '0000') <= @EndBillAddress)        -- 請求先 終了
　　　　　AND (@StartCustomer IS NULL OR @StartCustomer = '' OR FORMAT(eVPM_Gyosya11.GyosyaCd, '000') + FORMAT(eVPM_Tokisk11.TokuiCd, '0000') +	FORMAT(eVPM_TokiSt11.SitenCd, '0000') >= @StartCustomer)			-- 請求先 開始
　　　　　AND (@EndCustomer IS NULL OR @EndCustomer = '' OR FORMAT(eVPM_Gyosya11.GyosyaCd, '000') + FORMAT(eVPM_Tokisk11.TokuiCd, '0000') +	FORMAT(eVPM_TokiSt11.SitenCd, '0000') <= @EndCustomer)			-- 請求先 終了
END