USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetDepositPaymentGridAsync
-- Date			:   2020/09/11
-- Author		:   T.L.DUY
-- Description	:   Get deposit payment grid data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dDepositPaymentGrid_R] 
	(
	-- Parameter
		@UkeNo				NVARCHAR(100),
		@FutuUnkRen			SMALLINT,
		@SeiFutSyu			TINYINT,
		@FutTumRen			SMALLINT,
		@Offset				INT,					--Offset rows data
		@Limit				INT,					--Limit rows data
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	-- Processing
BEGIN
SELECT *, COUNT(*) OVER(ORDER BY (SELECT NULL)) AS CountNumber,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 1 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllCash,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 2 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllTransfer,

SUM(CAST(eTKD_NyShmi_NyShCu.FurKesG as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalAllTransferFee,
SUM(CAST(eTKD_NyShmi_NyShCu.KyoKesG as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalAllTransferSupport,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 3 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllCard,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 4 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllCommercialPaper,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 5 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllOffset,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 6 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllAdjustment,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 91 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllOther1,

SUM(CAST((CASE WHEN eTKD_NyShmi_NyShCu.NS_NyuSihSyu = 92 THEN eTKD_NyShmi_NyShCu.KesG ELSE 0 END) as bigint)) 
OVER(ORDER BY(SELECT NULL)) as TotalAllOther2,

SUM(CAST((eTKD_NyShmi_NyShCu.KesG + eTKD_NyShmi_NyShCu.FurKesG + eTKD_NyShmi_NyShCu.KyoKesG) as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalAllTotalDeposit,
SUM(CAST(eTKD_NyShmi_NyShCu.CouKesG as bigint)) OVER(ORDER BY(SELECT NULL)) as TotalAllCouponAppliedAmount
FROM (
		SELECT ISNULL(TKD_NyShmi.UkeNo, 0) AS UkeNo,
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
			ISNULL(TKD_NyShmi.UpdYmd, '') AS UpdYmd,
			ISNULL(TKD_NyShmi.UpdTime, '') AS UpdTime,
			ISNULL(eTKD_NyuSih01.NyuSihTblSeq, 0) AS NS_NyuSihTblSeq,
			ISNULL(eTKD_NyuSih01.NyuSihKbn, 0) AS NS_NyuSihKbn,
			ISNULL(eTKD_NyuSih01.NyuSihSyu, 0) AS NS_NyuSihSyu,
			ISNULL(eTKD_NyuSih01.CardSyo, '') AS NS_CardSyo,
			ISNULL(eTKD_NyuSih01.CardDen, '') AS NS_CardDen,
			ISNULL(eTKD_NyuSih01.NyuSihG, 0) AS  NS_NyuSihG,
			ISNULL(eTKD_NyuSih01.FuriTes, 0) AS NS_FuriTes,
			ISNULL(eTKD_NyuSih01.KyoRyoKin, 0) AS NS_KyoRyoKin,
			ISNULL(eTKD_NyuSih01.BankCd, '') AS NS_BankCd,
			ISNULL(eTKD_NyuSih01.BankSitCd, '') AS NS_BankSitCd,
			ISNULL(eTKD_NyuSih01.YokinSyu, 0) AS NS_YokinSyu,
			ISNULL(eTKD_NyuSih01.TegataYmd, '') AS NS_TegataYmd,
			ISNULL(eTKD_NyuSih01.TegataNo, '') AS NS_TegataNo,
			ISNULL(eTKD_NyuSih01.EtcSyo1, '') AS NS_EtcSyo1,
			ISNULL(eTKD_NyuSih01.EtcSyo2, '') AS NS_EtcSyo2,
			ISNULL(eTKD_NyuSih01.UpdYmd, '') AS  NS_UpdYmd,
			ISNULL(eTKD_NyuSih01.UpdTime, '') AS NS_UpdTime,
			ISNULL(eTKD_NyuSih01.NyuSihYmd, '') AS NyuSihHakoYmd,
			ISNULL(eTKD_NyuSih01.NyuSihEigSeq, 0) AS NyuSihEigSeq,
			ISNULL(eVPM_Eigyos01.EigyoCd, '') AS NyuSihEigCd,
			ISNULL(eVPM_Eigyos01.RyakuNm, '') AS NyuSihEigNm,
			ISNULL(eVPM_Bank01.BankNm, '') AS BankNm,
			ISNULL(eVPM_BankSt01.BankSitNm, '') AS BankStNm,
			ISNULL(eVPM_CodeKb01.CodeKbnNm, '') AS YokinSyuNm,
			ISNULL(eTKD_NyShCu01.NyuKesiKbn, 0) AS NyuKesiKbn,
			CASE eTKD_NyuSih01.NyuSihSyu
				WHEN 7 THEN ISNULL(eTKD_NyShCu01.CouKesG, 0)
				ELSE 0 END AS CouKesG,
			CASE eTKD_NyuSih01.NyuSihSyu
				WHEN 7 THEN ISNULL(eTKD_NyShCu01.UpdYmd, '')
				ELSE '' END AS NSC_UpdYmd,
			CASE eTKD_NyuSih01.NyuSihSyu
				WHEN 7 THEN ISNULL(eTKD_NyShCu01.UpdTime, '')
				ELSE '' END AS NSC_UpdTime,
			ISNULL(eTKD_Coupon01.CouNo, '') AS CouNo,
			ISNULL(eTKD_Coupon01.CouGkin, 0) AS CouGkin,
			ISNULL(eTKD_Coupon01.UpdYmd, '') AS COU_UpdYmd,
			ISNULL(eTKD_Coupon01.UpdTime, '' ) AS COU_UpdTime,
			0 AS  COU_NyuSihRen
	-- 入金支払明細テーブル
		FROM TKD_NyShmi
	-- 入金支払テーブル
		LEFT JOIN TKD_NyuSih AS eTKD_NyuSih01
			ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
	-- 入金支払クーポンテーブル
		LEFT JOIN TKD_NyShCu AS eTKD_NyShCu01
			ON TKD_NyShmi.UkeNo = eTKD_NyShCu01.UkeNo
			AND TKD_NyShmi.NyuSihKbn = eTKD_NyShCu01.NyuSihKbn
			AND TKD_NyShmi.SeiFutSyu = eTKD_NyShCu01.SeiFutSyu
			AND TKD_NyShmi.UnkRen =   eTKD_NyShCu01.UnkRen
			AND TKD_NyShmi.YouTblSeq = eTKD_NyShCu01.YouTblSeq
			AND TKD_NyShmi.FutTumRen = eTKD_NyShCu01.FutTumRen
			AND TKD_NyShmi.CouTblSeq = eTKD_NyShCu01.CouTblSeq
	-- クーポンテーブル
		LEFT JOIN TKD_Coupon AS eTKD_Coupon01
			ON  eTKD_NyuSih01.NyuSihSyu = 7
			AND TKD_NyShmi.CouTblSeq = eTKD_Coupon01.CouTblSeq
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
			AND eVPM_CodeKb01.TenantCdSeq = 0
			AND eTKD_NyuSih01.YokinSyu = eVPM_CodeKb01.CodeKbn
		WHERE TKD_NyShmi.NyuSihKbn = 1 -- 固定
			AND TKD_NyShmi.UkeNo = @UkeNo -- 選択した行のUkeNo
			AND TKD_NyShmi.UnkRen = @FutuUnkRen -- 選択した行のFutuUnkRen
			AND TKD_NyShmi.SeiFutSyu = @SeiFutSyu -- 選択した行のSeiFutSyu
			AND TKD_NyShmi.FutTumRen = @FutTumRen -- 選択した行のFutTumRen
			AND TKD_NyShmi.YouTblSeq = 0 -- 固定
			AND TKD_NyShmi.SiyoKbn = 1 -- 固定
UNION 
	SELECT *
	FROM (
		SELECT  ISNULL(TKD_NyShCu.UkeNo, 0) AS UkeNo,  
			0 AS NyuSihRen  ,  
			ISNULL(TKD_NyShCu.NyuSihKbn, 0) AS NyuSihKbn  ,   
			ISNULL(TKD_NyShCu.SeiFutSyu, 0) AS SeiFutSyu  ,   
			ISNULL(TKD_NyShCu.UnkRen, 0) AS UnkRen  ,   
			ISNULL(TKD_NyShCu.YouTblSeq, 0) AS YouTblSeq  ,   
			0 AS KesG,   
			0 AS FurKesG,
			0 AS KyoKesG, 
			ISNULL(TKD_NyShCu.FutTumRen, 0) AS FutTumRen,   
			ISNULL(TKD_NyShCu.NyuSihCouRen, 0) AS NyuSihCouRen, 
			0 AS NyuSihTblSeq, 
			ISNULL(TKD_NyShCu.CouTblSeq, 0) AS CouTblSeq, 
			ISNULL(TKD_NyShCu.SiyoKbn, 0) AS SiyoKbn,  
			'' AS UpdYmd,  
			'' AS UpdTime, 
			0 AS NS_NyuSihTblSeq, 
			0 AS NS_NyuSihKbn,  
			0 AS NS_NyuSihSyu, 
			'' AS NS_CardSyo,  
			'' AS NS_CardDen, 
			0 AS NS_NyuSihG, 
			0 AS NS_FuriTes,
			0 AS KyoRyoKin,
			'' AS NS_BankCd,
			'' AS NS_BankSitCd, 
			0 AS NS_YokinSyu, 
			'' AS NS_TegataYmd, 
			'' AS NS_TegataNo,  
			'' AS NS_EtcSyo1, 
			'' AS NS_EtcSyo2,  
			'' AS NS_UpdYmd, 
			'' AS NS_UpdTime, 
			ISNULL(eTKD_Coupon01.HakoYmd, '') AS  NyuSihHakoYmd,  
			ISNULL(eTKD_Coupon01.NyuSihEigSeq, 0) AS  NyuSihEigSeq,  
			ISNULL(eVPM_Eigyos01.EigyoCd, '') AS NyuSihEigCd,  
			ISNULL(eVPM_Eigyos01.RyakuNm, '') AS NyuSihEigNm,  
			'' AS BankNm  , 
			'' AS BankStNm  , 
			'' AS YokinSyuNm  , 
			ISNULL(TKD_NyShCu.NyuKesiKbn, 0) AS NyuKesiKbn, 
			ISNULL(TKD_NyShCu.CouKesG, 0) AS CouKesG  ,   
			ISNULL(TKD_NyShCu.UpdYmd, '') AS NSC_UpdYmd  ,   
			ISNULL(TKD_NyShCu.UpdTime, '') AS NSC_UpdTime  ,  
			ISNULL(eTKD_Coupon01.CouNo, '') AS CouNo,   
			ISNULL(eTKD_Coupon01.CouGkin, 0) AS CouGkin  ,  
			ISNULL(eTKD_Coupon01.UpdYmd, '') AS COU_UpdYmd  , 
			ISNULL(eTKD_Coupon01.UpdTime, '') AS COU_UpdTime  , 
			ISNULL(eTKD_NyShmi.NyuSihRen, 0) AS COU_NyuSihRen  
		FROM  TKD_NyShCu
		LEFT JOIN TKD_Coupon AS eTKD_Coupon01
			ON TKD_NyShCu.CouTblSeq = eTKD_Coupon01.CouTblSeq 
		LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01
			ON eTKD_Coupon01.NyuSihEigSeq = eVPM_Eigyos01.EigyoCdSeq
		LEFT JOIN (
			SELECT TKD_NyShmi.UkeNo, 
				TKD_NyShmi.NyuSihKbn, 
				TKD_NyShmi.SeiFutSyu, 
				TKD_NyShmi.UnkRen, 
				TKD_NyShmi.YouTblSeq, 
				TKD_NyShmi.FutTumRen, 
				TKD_NyShmi.NyuSihCouRen, 
				MAX(TKD_NyShmi.NyuSihRen) AS NyuSihRen  		
			FROM TKD_NyShmi
			INNER JOIN TKD_NyuSih AS eTKD_NyuSih
				ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih.NyuSihTblSeq AND eTKD_NyuSih.SiyoKbn = 1  		
			WHERE TKD_NyShmi.SiyoKbn = 1  		
			GROUP BY
				TKD_NyShmi.UkeNo,
				TKD_NyShmi.NyuSihKbn,
				TKD_NyShmi.SeiFutSyu,
				TKD_NyShmi.UnkRen,
				TKD_NyShmi.YouTblSeq,
				TKD_NyShmi.FutTumRen,
				TKD_NyShmi.NyuSihCouRen) AS eTKD_NyShmi
			ON TKD_NyShCu.UkeNo = eTKD_NyShmi.UkeNo
			AND TKD_NyShCu.NyuSihKbn = eTKD_NyShmi.NyuSihKbn
			AND TKD_NyShCu.SeiFutSyu = eTKD_NyShmi.SeiFutSyu
			AND TKD_NyShCu.UnkRen = eTKD_NyShmi.UnkRen
			AND TKD_NyShCu.YouTblSeq = eTKD_NyShmi.YouTblSeq
			AND TKD_NyShCu.FutTumRen = eTKD_NyShmi.FutTumRen
			AND TKD_NyShCu.NyuSihCouRen = eTKD_NyShmi.NyuSihCouRen
		WHERE NOT EXISTS (
			SELECT * FROM TKD_NyShmi  
			LEFT JOIN TKD_NyuSih AS eTKD_NyuSih01
				ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq  
			WHERE  eTKD_NyuSih01.NyuSihSyu = 7  
				AND TKD_NyShmi.UkeNo = TKD_NyShCu.UkeNo 
				AND TKD_NyShmi.NyuSihKbn = TKD_NyShCu.NyuSihKbn 
				AND TKD_NyShmi.SeiFutSyu = TKD_NyShCu.SeiFutSyu  
				AND TKD_NyShmi.UnkRen = TKD_NyShCu.UnkRen 
				AND TKD_NyShmi.YouTblSeq = TKD_NyShCu.YouTblSeq 
				AND TKD_NyShmi.FutTumRen = TKD_NyShCu.FutTumRen  
				AND TKD_NyShmi.CouTblSeq = TKD_NyShCu.CouTblSeq)) AS eTKD_NyShCu
	WHERE eTKD_NyShCu.NyuSihKbn = 1 -- 固定
		AND eTKD_NyShCu.UkeNo = @UkeNo -- 選択した行のUkeNo
		AND eTKD_NyShCu.UnkRen = @FutuUnkRen -- 選択した行のFutuUnkRen
		AND eTKD_NyShCu.SeiFutSyu = @SeiFutSyu -- 選択した行のSeiFutSyu
		AND eTKD_NyShCu.FutTumRen = @FutTumRen -- 選択した行のFutTumRen
		AND eTKD_NyShCu.YouTblSeq = 0 -- 固定
		AND eTKD_NyShCu.SiyoKbn   = 1 -- 固定
) AS eTKD_NyShmi_NyShCu
ORDER BY NyuSihHakoYmd, NyuSihRen
OFFSET @Offset ROWS
FETCH NEXT 
CASE WHEN @Limit = 0 THEN 10 ELSE @Limit 
END ROWS ONLY
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN