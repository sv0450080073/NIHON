USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSelectedReceivableListReceivable_R]    Script Date: 05/20/2021 4:12:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dSelectedReceivableListReceivable_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get selected receivable list data for receivable list
-- Date			:   2020/10/28
-- Author		:   N.N.T.AN
-- Description	:   Get selected receivable list for receivable list with conditions
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_dSelectedReceivableListReceivable_R] (--Parameter
 @TenantCdSeq INTEGER, --Tenant Id
 @CompanyCdSeq int, --Company Id
 @PaymentDate VARCHAR(MAX) = '', --Payment Date
 @StartPaymentDate VARCHAR(MAX) = '', --Start 
 @EndPaymentDate VARCHAR(MAX) = '', --End Payment Date
 @StartReceptionNumber bigint, --Start Reception Numer
 @EndReceptionNumber bigint, --End Reception Numer
 @StartReservation int, --Start Reservation
 @EndReservation int, --End Reservation
 @BillingType VARCHAR(MAX) = '', --Billing Type
 @UnpaidSelection VARCHAR(MAX) = '', --Unpaid Selection
 @SaleOfficeKbn VARCHAR(MAX) = '',
 @SaleOfficeSelectiton VARCHAR(MAX) = '', 
 @CSeiGyosyaCd VARCHAR(MAX) = '',
 @CSeiCd VARCHAR(MAX) = '',
 @CSeiSitenCd VARCHAR(MAX) = '',
 @CSeiCdSeq VARCHAR(MAX) = '',
 @CSeiSiyoStaYmd VARCHAR(MAX) = '',
 @CSeiSiyoEndYmd VARCHAR(MAX) = '',
 @CSeiSitenCdSeq VARCHAR(MAX) = '',
 @CSitSiyoStaYmd VARCHAR(MAX) = '',
 @CSitSiyoEndYmd VARCHAR(MAX) = '',
 @Skip INT,
 @Take INT,
 --Output
 @TotalCount				    int OUTPUT, -- ROWCOUNT
 @TotalAllTaxAmount				bigint OUTPUT,
 @TotalAllSaleAmount			bigint OUTPUT,
 @TotalAllFeeAmount				bigint OUTPUT,
 @TotalAllBillingAmount			bigint OUTPUT,
 @TotalAllDepositAmount			bigint OUTPUT,
 @TotalAllUnpaidAmount			bigint OUTPUT,
 @TotalAllCouponAmount			bigint OUTPUT
 ) AS -- Processing
 BEGIN
 IF OBJECT_ID(N'tempdb..#TempTable') IS NOT NULL
	BEGIN
	DROP TABLE #TempTable
	END;
 		WITH eTKD_Unkobi01 AS
  (SELECT TKD_Unkobi.UkeNo,
          MIN(TKD_Unkobi.UnkRen) AS UnkRen
   FROM TKD_Unkobi
   WHERE TKD_Unkobi.SiyoKbn = 1 
   GROUP BY TKD_Unkobi.UkeNo),
     eTKD_Unkobi02 AS
  (SELECT eTKD_Unkobi.UkeNo,
          eTKD_Unkobi.UnkRen,
          eTKD_Unkobi.HaiSYmd,
          eTKD_Unkobi.TouYmd,
          eTKD_Unkobi.IkNm,
          eTKD_Unkobi.YouKbn
   FROM TKD_Unkobi AS eTKD_Unkobi
	JOIN eTKD_Unkobi01 ON eTKD_Unkobi.UkeNo = eTKD_Unkobi01.UkeNo
	AND eTKD_Unkobi.UnkRen = eTKD_Unkobi01.UnkRen ),
     eTKD_NyShmi01 AS
  (SELECT TKD_NyShmi.UkeNo AS UkeNo,
          TKD_NyShmi.UnkRen AS UnkRen,
          TKD_NyShmi.FutTumRen AS FutTumRen,
          TKD_NyShmi.SeiFutSyu AS SeiFutSyu,
          TKD_NyShmi.KesG AS NyukinG,
          TKD_NyShmi.FurKesG AS FuriTes
   FROM TKD_NyShmi
   JOIN TKD_NyuSih AS eTKD_NyuSih01 ON TKD_NyShmi.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
   AND TKD_NyShmi.NyuSihKbn = 1
   AND TKD_NyShmi.SiyoKbn = 1
   AND eTKD_NyuSih01.SiyoKbn = 1 
   WHERE TKD_NyShmi.TenantCdSeq = @TenantCdSeq
   AND eTKD_NyuSih01.TenantCdSeq = @TenantCdSeq
   AND (@PaymentDate = '' OR eTKD_NyuSih01.NyuSihYmd <= @PaymentDate)),
     eTKD_NyShmi02 AS
  (SELECT eTKD_NyShmi01.UkeNo AS UkeNo,
          eTKD_NyShmi01.SeiFutSyu AS SeiFutSyu,
          SUM(eTKD_NyShmi01.NyukinG) AS NyukinG,
          SUM(eTKD_NyShmi01.FuriTes) AS FuriTes
   FROM eTKD_NyShmi01 GROUP BY eTKD_NyShmi01.UkeNo,
                               eTKD_NyShmi01.SeiFutSyu),
     eTKD_NyShmi03 AS
  (SELECT eTKD_NyShmi01.UkeNo AS UkeNo,
          eTKD_NyShmi01.UnkRen AS UnkRen,
          eTKD_NyShmi01.FutTumRen AS FutTumRen,
          eTKD_NyShmi01.SeiFutSyu AS SeiFutSyu,
          SUM(eTKD_NyShmi01.NyukinG) AS NyukinG,
          SUM(eTKD_NyShmi01.FuriTes) AS FuriTes
   FROM eTKD_NyShmi01 GROUP BY eTKD_NyShmi01.UkeNo,
                               eTKD_NyShmi01.UnkRen,
                               eTKD_NyShmi01.FutTumRen,
                               eTKD_NyShmi01.SeiFutSyu),
	eCodeKbn1Temp AS
	(
	SELECT * FROM VPM_CodeKb 
 WHERE CodeSyu = 'SEIFUTSYU'
 and TenantCdSeq =
  (SELECT CASE
              WHEN COUNT(*) = 0 THEN 0
              ELSE @TenantCdSeq
          END AS TenantCdSeq
   FROM VPM_CodeKb
   WHERE VPM_CodeKb.CodeSyu = 'SEIFUTSYU'
     AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq )
	),
	eCodeKbn2Temp AS
	(
	 SELECT * FROM VPM_CodeKb 
 WHERE CodeSyu = 'ZEIKBN'
 and TenantCdSeq =
  (SELECT CASE
              WHEN COUNT(*) = 0 THEN 0
              ELSE @TenantCdSeq
          END AS TenantCdSeq
   FROM VPM_CodeKb
   WHERE VPM_CodeKb.CodeSyu = 'ZEIKBN'
     AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq )
	)
SELECT TKD_Mishum.*,
	   CAST(TKD_Mishum.FutuUnkRen AS VARCHAR) AS FutuUnkRenChar,
       CAST(ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS VARCHAR)AS EigyoCd,
       CAST(ISNULL(eVPM_Eigyos01.EigyoCdSeq, 0)AS VARCHAR) AS EigyoCdSeq,
       CAST(ISNULL(eVPM_Eigyos01.EigyoNm, ' ')AS VARCHAR) AS EigyoNm,
       CAST(ISNULL(eVPM_Eigyos01.RyakuNm, ' ')AS VARCHAR) AS EigyoRyak,
       CAST(ISNULL(eVPM_Gyosya02.GyosyaCd, 0)AS VARCHAR) AS SeiGyosyaCd,
       CAST(ISNULL(eVPM_Tokisk02.TokuiCd, 0)AS VARCHAR) AS SeiCd,
       CAST(ISNULL(eVPM_Tokisk02.TokuiNm, ' ')AS VARCHAR) AS SeiCdNm,
       CAST(ISNULL(eVPM_Tokisk02.RyakuNm, ' ')AS VARCHAR) AS SeiRyakuNm,
       CAST(ISNULL(eVPM_TokiSt02.SitenCd, 0)AS VARCHAR) AS SeiSitenCd,
       CAST(ISNULL(eVPM_TokiSt02.SitenNm, ' ')AS VARCHAR) AS SeiSitenCdNm,
       CAST(ISNULL(eVPM_TokiSt02.RyakuNm, ' ')AS VARCHAR) AS SeiSitRyakuNm,
       CAST(ISNULL(eVPM_Gyosya02.GyosyaNm, ' ')AS VARCHAR) AS SeiGyosyaCdNm,
       CAST(CASE
           WHEN eTKD_Yyksho01.YoyaSyu = 1 THEN eTKD_Yyksho01.SeiTaiYmd
           ELSE eTKD_Yyksho01.CanYmd
       END AS VARCHAR)AS SeiTaiYmd,
       CAST(ISNULL(eVPM_Eigyos02.EigyoCd, 0) AS VARCHAR)AS UkeEigyoCd,
       CAST(ISNULL(eVPM_Eigyos02.EigyoNm, ' ') AS VARCHAR)AS UkeEigyoNm,
       CAST(ISNULL(eVPM_Eigyos02.RyakuNm, ' ') AS VARCHAR)AS UkeRyakuNm,
       CAST(ISNULL(eVPM_Tokisk01.TokuiCd, 0) AS VARCHAR)AS TokuiCd,
       CAST(ISNULL(eVPM_Tokisk01.TokuiNm, ' ') AS VARCHAR)AS TokuiNm,
       CAST(ISNULL(eVPM_Tokisk01.RyakuNm, ' ') AS VARCHAR)AS TokRyakuNm,
       CAST(ISNULL(eVPM_TokiSt01.SitenCd, 0) AS VARCHAR)AS SitenCd,
       CAST(ISNULL(eVPM_TokiSt01.SitenNm, ' ') AS VARCHAR)AS SitenNm,
       CAST(ISNULL(eVPM_TokiSt01.RyakuNm, ' ') AS VARCHAR)AS SitRyakuNm,
       CAST(ISNULL(eVPM_Gyosya01.GyosyaCd, 0) AS VARCHAR)AS GyosyaCd,
       CAST(ISNULL(eVPM_Gyosya01.GyosyaNm, ' ') AS VARCHAR)AS GyosyaNm,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1, 5,
                                         7) THEN ISNULL(eTKD_Unkobi11.HaiSYmd, ' ')
           ELSE ISNULL(eTKD_Unkobi12.HaiSYmd, ' ')
       END AS VARCHAR)AS HaiSYmd,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1, 5,
                                         7) THEN ISNULL(eTKD_Unkobi11.TouYmd, ' ')
           ELSE ISNULL(eTKD_Unkobi12.TouYmd, ' ')
       END AS VARCHAR)AS TouYmd,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1, 5,
                                         7) THEN ISNULL(eTKD_Unkobi11.IkNm, ' ')
           ELSE ISNULL(eTKD_Unkobi12.IkNm, ' ')
       END AS VARCHAR)AS IkNm,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1, 5,
                                         7) THEN ISNULL(eTKD_Yyksho01.YoyaNm, ' ')
           ELSE ISNULL(eTKD_Unkobi12.DanTaNm, ' ')
       END AS VARCHAR)AS DanTaNm,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1) THEN ISNULL(eTKD_Yyksho01.ZeiKbn, 0)
		   WHEN TKD_Mishum.SeiFutSyu IN (5) THEN ISNULL(eTKD_Yyksho01.TaxTypeforGuider, 0)
		   WHEN TKD_Mishum.SeiFutSyu IN (7) THEN ISNULL(eTKD_Yyksho01.CanZKbn, 0)
           ELSE ISNULL(eTKD_FutTum11.ZeiKbn, 0)
       END AS VARCHAR)AS ZeiKbn,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1, 5) THEN ISNULL(eTKD_Yyksho01.Zeiritsu, 0)
		   WHEN TKD_Mishum.SeiFutSyu IN (7) THEN ISNULL(eTKD_Yyksho01.CanSyoR, 0)
           ELSE ISNULL(eTKD_FutTum11.Zeiritsu, 0)
       END AS int)AS Zeiritsu,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1) THEN ISNULL(eTKD_Yyksho01.TesuRitu, 0)
		   WHEN TKD_Mishum.SeiFutSyu IN (5) THEN ISNULL(eTKD_Yyksho01.FeeGuiderRate, 0)
           ELSE ISNULL(eTKD_FutTum11.TesuRitu, 0)
       END AS int)AS TesuRitu,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ' '
           ELSE ISNULL(eTKD_FutTum11.FutTumNm, ' ')
       END AS VARCHAR)AS FutTumNm,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ' '
           ELSE ISNULL(CAST(eTKD_FutTum11.FutTumKbn AS VARCHAR), ' ')
       END AS VARCHAR)AS FutTumKbn,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ' '
           ELSE ISNULL(eTKD_FutTum11.SeisanNm, ' ')
       END AS VARCHAR)AS SeisanNm,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ' '
           ELSE ISNULL(CAST(eTKD_FutTum11.Suryo AS int), 0)
       END AS int)AS Suryo,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ' '
           ELSE ISNULL(CAST(eTKD_FutTum11.TanKa AS int), 0)
       END AS int)AS TanKa,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN 0
           ELSE ISNULL(eVPM_Seisan01.SeisanCd, 0)
       END AS VARCHAR)AS SeisanCd,
       ISNULL(eVPM_CodeKb01.CodeKbnNm, ' ') AS SeiFutSyuNm,
       ISNULL(eVPM_CodeKb02.CodeKbnNm, ' ') AS ZeiKbnNm,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0)
           ELSE ISNULL(eTKD_NyShmi12.NyukinG, 0)
       END AS INT )AS NyukinG,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ISNULL(eTKD_NyShmi11.FuriTes, 0)
           ELSE ISNULL(eTKD_NyShmi12.FuriTes, 0)
       END AS INT)AS FuriTes,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ISNULL(eTKD_Yyksho01.NyuKinKbn, 0)
           ELSE ISNULL(eTKD_FutTum11.NyuKinKbn, 0)
       END AS VARCHAR)AS NyuKinKbn,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ISNULL(eTKD_Yyksho01.NCouKbn, 0)
           ELSE ISNULL(eTKD_FutTum11.NCouKbn, 0)
       END AS VARCHAR)AS NCouKbn,
       CAST(CASE
           WHEN TKD_Mishum.SeiFutSyu IN (1,
                                         7) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0)
           ELSE ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0)
       END AS VARCHAR)AS NyukinRuiG,
       TKD_Mishum.SeiKin - CASE
                               WHEN TKD_Mishum.SeiFutSyu IN (1,
                                                             7) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0)
                               ELSE ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0)
                           END AS MisyuG,
                           ISNULL(eVPM_Tokisk02.SiyoStaYmd, 0) AS TSiyoStaYmd,
                           ISNULL(eVPM_Tokisk02.SiyoEndYmd, 0) AS TSiyoEndYmd,
                           ISNULL(eVPM_TokiSt02.SiyoStaYmd, 0) AS SSiyoStaYmd,
                           ISNULL(eVPM_TokiSt02.SiyoEndYmd, 0) AS SSiyoEndYmd,
                           ISNULL(eTKD_Unkobi11.YouKbn, 0) AS YouKbn

	INTO #TempTable
FROM TKD_Mishum
JOIN TKD_Yyksho AS eTKD_Yyksho01 ON TKD_Mishum.UkeNo = eTKD_Yyksho01.UkeNo
AND TKD_Mishum.SiyoKbn = 1
AND eTKD_Yyksho01.SiyoKbn = 1
AND (TKD_Mishum.SeiFutSyu <> 7
     AND eTKD_Yyksho01.YoyaSyu = 1
     OR TKD_Mishum.SeiFutSyu = 7
     AND eTKD_Yyksho01.YoyaSyu = 2)
AND eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01 ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
AND eTKD_Yyksho01.TenantCdSeq = eVPM_YoyKbn01.TenantCdSeq
JOIN VPM_Eigyos AS eVPM_Eigyos01 ON eTKD_Yyksho01.SeiEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
JOIN VPM_Eigyos AS eVPM_Eigyos02 ON eTKD_Yyksho01.UkeEigCdSeq = eVPM_Eigyos02.EigyoCdSeq
JOIN VPM_Compny AS eVPM_Compny01 ON eVPM_Compny01.CompanyCdSeq = eVPM_Eigyos01.CompanyCdSeq
AND eVPM_Compny01.SiyoKbn = 1
JOIN VPM_Tokisk AS eVPM_Tokisk01 ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
JOIN VPM_TokiSt AS eVPM_TokiSt01 ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01 ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
AND eVPM_Tokisk01.TenantCdSeq = eVPM_Gyosya01.TenantCdSeq
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02 ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02 ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02 ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
AND eVPM_Tokisk02.TenantCdSeq = eVPM_Gyosya02.TenantCdSeq
LEFT JOIN eTKD_Unkobi02 AS eTKD_Unkobi11 ON TKD_Mishum.UkeNo = eTKD_Unkobi11.UkeNo
LEFT JOIN TKD_Unkobi AS eTKD_Unkobi12 ON TKD_Mishum.UkeNo = eTKD_Unkobi12.UkeNo
AND TKD_Mishum.FutuUnkRen = eTKD_Unkobi12.UnkRen
AND eTKD_Unkobi12.SiyoKbn = 1
LEFT JOIN TKD_FutTum AS eTKD_FutTum11 ON TKD_Mishum.UkeNo = eTKD_FutTum11.UkeNo
AND TKD_Mishum.FutuUnkRen = eTKD_FutTum11.UnkRen
AND TKD_Mishum.FutTumRen = eTKD_FutTum11.FutTumRen
AND eTKD_FutTum11.SiyoKbn = 1
AND ((TKD_Mishum.SeiFutSyu = 6
      AND eTKD_FutTum11.FutTumKbn = 2)
     OR (TKD_Mishum.SeiFutSyu <> 6
         AND eTKD_FutTum11.FutTumKbn = 1))
LEFT JOIN VPM_Seisan AS eVPM_Seisan01 ON eTKD_FutTum11.SeisanCdSeq = eVPM_Seisan01.SeisanCdSeq
AND eTKD_Yyksho01.TenantCdSeq = eVPM_Seisan01.TenantCdSeq
LEFT JOIN eCodeKbn1Temp AS eVPM_CodeKb01 ON
TKD_Mishum.SeiFutSyu = eVPM_CodeKb01.CodeKbn
LEFT JOIN eCodeKbn2Temp AS eVPM_CodeKb02 ON (((TKD_Mishum.SeiFutSyu = 1
       OR TKD_Mishum.SeiFutSyu = 7)
      AND eTKD_Yyksho01.ZeiKbn = eVPM_CodeKb02.CodeKbn)
     OR (NOT (TKD_Mishum.SeiFutSyu = 1
              OR TKD_Mishum.SeiFutSyu = 7)
         AND eTKD_FutTum11.ZeiKbn = eVPM_CodeKb02.CodeKbn))
LEFT JOIN eTKD_NyShmi02 AS eTKD_NyShmi11 ON TKD_Mishum.UkeNo = eTKD_NyShmi11.UkeNo
AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi11.SeiFutSyu
LEFT JOIN eTKD_NyShmi03 AS eTKD_NyShmi12 ON TKD_Mishum.UkeNo = eTKD_NyShmi12.UkeNo
AND TKD_Mishum.FutuUnkRen = eTKD_NyShmi12.UnkRen
AND TKD_Mishum.FutTumRen = eTKD_NyShmi12.FutTumRen
AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi12.SeiFutSyu
WHERE 
			(@CompanyCdSeq = -1 OR (eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq))
			AND (@StartPaymentDate = '' OR ((eTKD_Yyksho01.YoyaSyu = 1 AND eTKD_Yyksho01.SeiTaiYmd >= @StartPaymentDate) OR (eTKD_Yyksho01.YoyaSyu = 2 AND eTKD_Yyksho01.CanYmd >= @StartPaymentDate)))
		    AND (@EndPaymentDate = '' OR ((eTKD_Yyksho01.YoyaSyu = 1 AND eTKD_Yyksho01.SeiTaiYmd <= @EndPaymentDate) OR (eTKD_Yyksho01.YoyaSyu = 2 AND eTKD_Yyksho01.CanYmd <= @EndPaymentDate)))
			AND (@StartReceptionNumber = 0 OR eTKD_Yyksho01.UkeCd >= @StartReceptionNumber)
			AND (@EndReceptionNumber = 0 OR eTKD_Yyksho01.UkeCd <= @EndReceptionNumber)
			AND (@StartReservation = 0 OR (eVPM_YoyKbn01.YoyaKbn >= @StartReservation))
		    AND (@EndReservation = 0 OR (eVPM_YoyKbn01.YoyaKbn <= @EndReservation))
			AND (@BillingType = '' OR TKD_Mishum.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
			AND (@UnpaidSelection = '0' OR (@UnpaidSelection = '1' AND SeiKin = CouKesRui) OR (@UnpaidSelection = '2' AND (SeiKin <> CouKesRui AND CouKesRui <> 0)) OR (@UnpaidSelection = '3' AND CouKesRui = 0))
			AND (@SaleOfficeSelectiton = '' OR (@SaleOfficeKbn = '請求営業所' AND eVPM_Eigyos01.EigyoCdSeq = @SaleOfficeSelectiton) OR (@SaleOfficeKbn = '受付営業所' AND eVPM_Eigyos02.EigyoCdSeq = @SaleOfficeSelectiton))
			AND (@CSeiGyosyaCd = '' OR (eVPM_Gyosya02.GyosyaCd = @CSeiGyosyaCd))
			AND (@CSeiCd = '' OR (eVPM_Tokisk02.TokuiCd = @CSeiCd))
			AND (@CSeiSitenCd = '' OR (eVPM_TokiSt02.SitenCd = @CSeiSitenCd))
			AND (@CSeiCdSeq = '' OR (eVPM_TokiSt01.SeiCdSeq = @CSeiCdSeq))
			AND (@CSeiSiyoStaYmd = '' OR (eVPM_Tokisk01.SiyoStaYmd = @CSeiSiyoStaYmd))
			AND (@CSeiSiyoEndYmd = '' OR (eVPM_Tokisk01.SiyoEndYmd = @CSeiSiyoEndYmd))
			AND (@CSeiSitenCdSeq = '' OR (eVPM_TokiSt01.SeiSitenCdSeq = @CSeiSitenCdSeq))
			AND (@CSitSiyoStaYmd = '' OR (eVPM_TokiSt02.SiyoStaYmd = @CSitSiyoStaYmd))
			AND (@CSitSiyoEndYmd = '' OR (eVPM_TokiSt02.SiyoEndYmd = @CSitSiyoEndYmd))
  AND TKD_Mishum.SeiKin - CASE
                              WHEN TKD_Mishum.SeiFutSyu IN (1,
                                                            7) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0)
                              ELSE ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0)
                          END <> 0
						  SELECT @TotalAllSaleAmount = ISNULL(SUM(CAST(UriGakKin as bigint)), 0), @TotalAllTaxAmount = ISNULL(SUM(CAST(SyaRyoSyo as bigint)), 0), @TotalAllFeeAmount = ISNULL(SUM(CAST(SyaRyoTes as bigint)), 0), 
						  @TotalAllBillingAmount = ISNULL(SUM(CAST(SeiKin as bigint)), 0), @TotalAllDepositAmount = ISNULL(SUM(CAST(NyukinG as bigint)), 0), @TotalAllUnpaidAmount = ISNULL(SUM(CAST(SeiKin - NyukinG - FuriTes as bigint)), 0), @TotalAllCouponAmount = ISNULL(SUM(CAST(CouKesRui as bigint)), 0) FROM #TempTable
			SELECT @TotalCount = CAST(ISNULL(COUNT(*), 0) AS INT) FROM #TempTable
			SELECT * from #TempTable 
  ORDER BY     
	EigyoCd,
    SeiGyosyaCd,
    SeiCd,
    SeiSitenCd,
    SeiTaiYmd,
    UkeNo,
    FutuUnkRen,
    FutTumRen
OFFSET @Skip ROWS FETCH FIRST @Take ROWS only
END
 