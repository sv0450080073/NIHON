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

USE [HOC_Kashikiri]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   [PK_dNyShmi_R]
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get NyShmi List
-- Date			:   2020/10/15
-- Author		:   Tra Nguyen Lam
-- Description	:   Get NyShmi List with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dNyShmi_R] 
	@UkeNo									varchar(15),
	@UnkRen									smallint,
	@SeiFutSyu								tinyint,
	@FutTumRen								smallint,
	@YouTblSeq								int,
	@TenantCdSeq							int
AS
BEGIN
	SELECT ROW_NUMBER() OVER (ORDER BY eTKD_NyuSih01.NyuSihYmd ASC,
		TKD_NyShmi.NyuSihRen) as [No],
		ISNULL(TKD_NyShmi.UkeNo, 0) AS UkeNo,
		ISNULL(TKD_NyShmi.NyuSihRen, 0) AS NyuSihRen,
		ISNULL(TKD_NyShmi.NyuSihKbn, 0) AS NyuSihKbn,
		ISNULL(TKD_NyShmi.SeiFutSyu, 0) AS SeiFutSyu,
		ISNULL(TKD_NyShmi.UnkRen, 0) AS UnkRen,
		ISNULL(TKD_NyShmi.YouTblSeq, 0) AS YouTblSeq,
		ISNULL(TKD_NyShmi.KesG, 0) AS KesG,
		ISNULL(TKD_NyShmi.FurKesG, 0) AS FurKesG,
		ISNULL(TKD_NyShmi.KyoKesG, 0) AS KyoKesG,
		ISNULL(TKD_NyShmi.FutTumRen, 0) AS FutTumRen,
		ISNULL(TKD_NyShmi.NyuSihCouRen, 0) AS NyuSihCouRen,
		ISNULL(TKD_NyShmi.NyuSihTblSeq, 0) AS NyuSihTblSeq,
		ISNULL(TKD_NyShmi.CouTblSeq, 0) AS CouTblSeq,
		ISNULL(TKD_NyShmi.SiyoKbn, 0) AS SiyoKbn,
		ISNULL(TKD_NyShmi.UpdYmd, ' ') AS UpdYmd,
		ISNULL(TKD_NyShmi.UpdTime, ' ') AS UpdTime,
		ISNULL(eTKD_NyuSih01.NyuSihTblSeq, 0) AS NSNyuSihTblSeq,
		ISNULL(eTKD_NyuSih01.NyuSihKbn, 0) AS NSNyuSihKbn,
		ISNULL(eTKD_NyuSih01.NyuSihSyu, 0) AS NSNyuSihSyu,
		ISNULL(eTKD_NyuSih01.CardSyo, ' ') AS NSCardSyo,
		ISNULL(eTKD_NyuSih01.CardDen, ' ') AS NSCardDen,
		ISNULL(eTKD_NyuSih01.NyuSihG, 0) AS NSNyuSihG,
		ISNULL(eTKD_NyuSih01.FuriTes, 0) AS NSFuriTes,
		ISNULL(eTKD_NyuSih01.KyoRyoKin, 0) AS NSKyoRyoKin,
		ISNULL(eTKD_NyuSih01.BankCd, ' ') AS NSBankCd,
		ISNULL(eTKD_NyuSih01.BankSitCd, ' ') AS NSBankSitCd,
		ISNULL(eTKD_NyuSih01.YokinSyu, 0) AS NSYokinSyu,
		ISNULL(eTKD_NyuSih01.TegataYmd, ' ') AS NSTegataYmd,
		ISNULL(eTKD_NyuSih01.TegataNo, ' ') AS NSTegataNo,
		ISNULL(eTKD_NyuSih01.EtcSyo1, ' ') AS NSEtcSyo1,
		ISNULL(eTKD_NyuSih01.EtcSyo2, ' ') AS NSEtcSyo2,
		ISNULL(eTKD_NyuSih01.UpdYmd, ' ') AS NSUpdYmd,
		ISNULL(eTKD_NyuSih01.UpdTime, ' ') AS NSUpdTime,
		ISNULL(eTKD_NyuSih01.NyuSihYmd, ' ') AS NyuSihHakoYmd,
		ISNULL(eTKD_NyuSih01.NyuSihEigSeq, 0) AS NyuSihEigSeq,
		ISNULL(eVPM_Eigyos01.EigyoCd, ' ') AS NyuSihEigCd,
		ISNULL(eVPM_Eigyos01.RyakuNm, ' ') AS NyuSihEigNm,
		ISNULL(eVPM_Bank01.BankNm, ' ') AS BankNm,
		ISNULL(eVPM_BankSt01.BankSitNm, ' ') AS BankStNm,
		ISNULL(eVPM_CodeKb01.CodeKbnNm, ' ') AS YokinSyuNm,
		ISNULL(eTKD_NyShCu01.NyuKesiKbn, 0) AS NyuKesiKbn,
		CASE eTKD_NyuSih01.NyuSihSyu
			WHEN 7 THEN ISNULL(eTKD_NyShCu01.CouKesG, 0)
			ELSE 0
		END AS CouKesG,
		CASE eTKD_NyuSih01.NyuSihSyu
			WHEN 7 THEN ISNULL(eTKD_NyShCu01.UpdYmd, '')
			ELSE ''
		END AS NSC_UpdYmd,
		CASE eTKD_NyuSih01.NyuSihSyu
			WHEN 7 THEN ISNULL(eTKD_NyShCu01.UpdTime, '')
			ELSE ''
		END AS NSC_UpdTime,
		ISNULL(eTKD_Coupon01.CouNo, ' ') AS CouNo,
		ISNULL(eTKD_Coupon01.CouGkin, 0) AS CouGkin,
		ISNULL(eTKD_Coupon01.UpdYmd, ' ') AS COU_UpdYmd,
		ISNULL(eTKD_Coupon01.UpdTime, ' ') AS COU_UpdTime,
		0 AS COU_NyuSihRen
	-- 入金支払明細テーブル
	FROM TKD_NyShmi 
	-- 入金支払テーブル
	LEFT JOIN TKD_NyuSih AS eTKD_NyuSih01
		ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
		AND eTKD_NyuSih01.TenantCdSeq = @TenantCdSeq
	-- 入金支払クーポンテーブル
	LEFT JOIN TKD_NyShCu AS eTKD_NyShCu01
		ON TKD_NyShmi.UkeNo = eTKD_NyShCu01.UkeNo
		AND TKD_NyShmi.NyuSihKbn = eTKD_NyShCu01.NyuSihKbn
		AND TKD_NyShmi.SeiFutSyu = eTKD_NyShCu01.SeiFutSyu
		AND TKD_NyShmi.UnkRen = eTKD_NyShCu01.UnkRen
		AND TKD_NyShmi.YouTblSeq = eTKD_NyShCu01.YouTblSeq
		AND TKD_NyShmi.FutTumRen = eTKD_NyShCu01.FutTumRen
		AND TKD_NyShmi.CouTblSeq = eTKD_NyShCu01.CouTblSeq
	-- クーポンテーブル
	LEFT JOIN TKD_Coupon AS eTKD_Coupon01
		ON eTKD_NyuSih01.NyuSihSyu = 7
		AND TKD_NyShmi.CouTblSeq = eTKD_Coupon01.CouTblSeq
		AND eTKD_Coupon01.TenantCdSeq = @TenantCdSeq
	-- 営業所マスタ
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
		ON eTKD_NyuSih01.NyuSihEigSeq = eVPM_Eigyos01.EigyoCdSeq
	-- 銀行マスタ
	LEFT JOIN VPM_Bank AS eVPM_Bank01
		ON eTKD_NyuSih01.BankCd = eVPM_Bank01.BankCd
	-- 銀行支店マスタ
	LEFT JOIN VPM_BankSt AS eVPM_BankSt01
		ON eTKD_NyuSih01.BankCd = eVPM_BankSt01.BankCd
		AND eTKD_NyuSih01.BankSitCd = eVPM_BankSt01.BankSitCd
	-- コード区分マスタ 預金種別
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01
		ON eVPM_CodeKb01.CodeSyu = 'YOKINSYU'
		AND eTKD_NyuSih01.YokinSyu = eVPM_CodeKb01.CodeKbn
		AND eVPM_CodeKb01.TenantCdSeq = @TenantCdSeq
	WHERE TKD_NyShmi.NyuSihKbn = 1 -- 固定
		AND TKD_NyShmi.UkeNo = @UkeNo --  選択した行のUkeNo
		AND TKD_NyShmi.UnkRen = @UnkRen --  選択した行のUnkRen
		AND TKD_NyShmi.SeiFutSyu = @SeiFutSyu --  選択した行のSeiFutSyu
		AND TKD_NyShmi.FutTumRen = @FutTumRen --  選択した行のFutTumRen
		AND TKD_NyShmi.YouTblSeq = @YouTblSeq --  選択した行のYouTblSeq
		AND TKD_NyShmi.SiyoKbn = 1
	ORDER BY NyuSihHakoYmd ASC,
		NyuSihRen

	END
GO


