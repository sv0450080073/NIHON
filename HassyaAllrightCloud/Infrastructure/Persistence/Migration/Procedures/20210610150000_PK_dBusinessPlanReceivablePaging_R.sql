USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBusinessPlanReceivablePaging_R]    Script Date: 01/20/2021 2:44:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dBusinessPlanReceivable_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get selected reveivable data for business plan
-- Date			:   2020/10/12
-- Author		:   N.N.T.AN
-- Description	:   Get selected reveivable for business plan list with conditions
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_dBusinessPlanReceivablePaging_R] (--Parameter
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
 @SaleOfficeKbn VARCHAR(MAX) = '', -- Sale Office 
 @SaleOfficeSelectiton VARCHAR(MAX) = '', --Selection Sale Office
 @StartBillingAddress VARCHAR(MAX) = '', --Start Billing Address
 @EndBillingAddress VARCHAR(MAX) = '', --End Billing Address
 @Skip									int,
 @Take									int

 ) AS -- Processing
 BEGIN			
	WITH 
    eTKD_Unkobi01 AS
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
   AND (@PaymentDate = '' OR eTKD_NyuSih01.NyuSihYmd <= @PaymentDate)
   ),
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
                               eTKD_NyShmi01.SeiFutSyu)
SELECT CONCAT(FORMAT(ISNULL(eVPM_Gyosya02.GyosyaCd, 0), '000'), '-', FORMAT(ISNULL(eVPM_Tokisk02.TokuiCd, 0), '0000'), '-', FORMAT(ISNULL(eVPM_TokiSt02.SitenCd, 0), '0000')) AS SeikyuCd,
       CONCAT(eVPM_Tokisk02.RyakuNm, '　', eVPM_TokiSt02.RyakuNm) AS SeikyuNm,
	   CAST(ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS VARCHAR) AS EigyoCd,
       ISNULL(eVPM_Eigyos01.EigyoCdSeq, 0) AS EigyoCdSeq,
       ISNULL(eVPM_Eigyos01.EigyoNm, ' ') AS EigyoNm,
       ISNULL(eVPM_Eigyos01.RyakuNm, ' ') AS EigyoRyak,
       CAST(ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS VARCHAR) AS SeiGyosyaCd,
       CAST(ISNULL(eVPM_Tokisk02.TokuiCd, 0)AS VARCHAR) AS SeiCd,
       ISNULL(eVPM_Tokisk02.TokuiNm, ' ') AS SeiCdNm,
       ISNULL(eVPM_Tokisk02.RyakuNm, ' ') AS SeiRyakuNm,
       CAST(ISNULL(eVPM_TokiSt02.SitenCd, 0)AS VARCHAR) AS SeiSitenCd,
       ISNULL(eVPM_TokiSt02.SitenNm, ' ') AS SeiSitenCdNm,
       ISNULL(eVPM_TokiSt02.RyakuNm, ' ') AS SeiSitRyakuNm,
       ISNULL(eVPM_Gyosya02.GyosyaNm, ' ') AS SeiGyosyaCdNm,
        SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1) THEN TKD_Mishum.UriGakKin
               ELSE 0
           END AS bigint)) AS UnUriGakKin,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1) THEN TKD_Mishum.SyaRyoSyo
               ELSE 0
           END AS bigint)) AS UnSyaRyoSyo,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1) THEN TKD_Mishum.SyaRyoTes
               ELSE 0
           END AS bigint))AS UnSyaRyoTes,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1) THEN TKD_Mishum.NyuKinRui
               ELSE 0
           END AS bigint))AS UnNyuKinRui,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (5) THEN TKD_Mishum.UriGakKin
               ELSE 0
           END AS bigint))AS GaUriGakKin,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (5) THEN TKD_Mishum.SyaRyoSyo
               ELSE 0
           END AS bigint))AS GaSyaRyoSyo,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (5) THEN TKD_Mishum.SyaRyoTes
               ELSE 0
           END AS bigint))AS GaSyaRyoTes,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (5) THEN TKD_Mishum.NyuKinRui
               ELSE 0
           END AS bigint))AS GaNyuKinRui,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
               ELSE TKD_Mishum.UriGakKin
           END AS bigint))AS SoUriGakKin,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
               ELSE TKD_Mishum.SyaRyoSyo
           END AS bigint))AS SoSyaRyoSyo,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
               ELSE TKD_Mishum.SyaRyoTes
           END AS bigint))AS SoSyaRyoTes,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
               ELSE TKD_Mishum.NyuKinRui
           END AS bigint))AS SoNyuKinRui,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (7) THEN TKD_Mishum.UriGakKin
               ELSE 0
           END AS bigint))AS CaUriGakKin,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (7) THEN TKD_Mishum.SyaRyoSyo
               ELSE 0
           END AS bigint))AS CaSyaRyoSyo,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (7) THEN TKD_Mishum.NyuKinRui
               ELSE 0
           END AS bigint))AS CaNyuKinRui,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0)
               ELSE 0
           END AS bigint))AS UnNyukinG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (5) THEN ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0)
               ELSE 0
           END AS bigint))AS GaNyukinG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
               ELSE ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0)
           END AS bigint))AS SoNyukinG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (7) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0)
               ELSE 0
           END AS bigint))AS CaNyukinG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1) THEN TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0))
               ELSE 0
           END AS bigint))AS UnMisyuG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (5) THEN TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0))
               ELSE 0
           END AS bigint))AS GaMisyuG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
               ELSE TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0))
           END AS bigint))AS SoMisyuG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (7) THEN TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0))
               ELSE 0
           END AS bigint))AS CaMisyuG,
       SUM(CAST(CASE
               WHEN TKD_Mishum.SeiFutSyu IN (1) THEN TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0))
               ELSE 0
           END + CASE
                     WHEN TKD_Mishum.SeiFutSyu IN (5) THEN TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0))
                     ELSE 0
                 END + CASE
                           WHEN TKD_Mishum.SeiFutSyu IN (1, 5, 7) THEN 0
                           ELSE TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0))
                       END + CASE
                                 WHEN TKD_Mishum.SeiFutSyu IN (7) THEN TKD_Mishum.SeiKin - (ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0))
                                 ELSE 0
                             END AS bigint))AS MisyuG

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
AND eTKD_Yyksho01.TenantCdSeq = eVPM_Seisan01.TenantCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeSyu = 'SEIFUTSYU'
AND TKD_Mishum.SeiFutSyu = eVPM_CodeKb01.CodeKbn
AND eVPM_CodeKb01.TenantCdSeq =
  (SELECT CASE
              WHEN COUNT(*) = 0 THEN 0
              ELSE @TenantCdSeq
          END AS TenantCdSeq
   FROM VPM_CodeKb
   WHERE VPM_CodeKb.CodeSyu = 'SEIFUTSYU'
     AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq )
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02 ON eVPM_CodeKb02.CodeSyu = 'ZEIKBN'
AND (((TKD_Mishum.SeiFutSyu = 1
       OR TKD_Mishum.SeiFutSyu = 7)
      AND eTKD_Yyksho01.ZeiKbn = eVPM_CodeKb02.CodeKbn)
     OR (NOT (TKD_Mishum.SeiFutSyu = 1
              OR TKD_Mishum.SeiFutSyu = 7)
         AND eTKD_FutTum11.ZeiKbn = eVPM_CodeKb02.CodeKbn))
AND eVPM_CodeKb02.TenantCdSeq =
  (SELECT CASE
              WHEN COUNT(*) = 0 THEN 0
              ELSE @TenantCdSeq
          END AS TenantCdSeq
   FROM VPM_CodeKb
   WHERE VPM_CodeKb.CodeSyu = 'ZEIKBN'
     AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq )
LEFT JOIN eTKD_NyShmi02 AS eTKD_NyShmi11 ON TKD_Mishum.UkeNo = eTKD_NyShmi11.UkeNo
AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi11.SeiFutSyu
LEFT JOIN eTKD_NyShmi03 AS eTKD_NyShmi12 ON TKD_Mishum.UkeNo = eTKD_NyShmi12.UkeNo
AND TKD_Mishum.FutuUnkRen = eTKD_NyShmi12.UnkRen
AND TKD_Mishum.FutTumRen = eTKD_NyShmi12.FutTumRen
AND TKD_Mishum.SeiFutSyu = eTKD_NyShmi12.SeiFutSyu
WHERE 
  (@CompanyCdSeq = -1 OR (eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq))
     AND (NOT(@StartPaymentDate <> '' AND @EndPaymentDate <> '') OR ((eTKD_Yyksho01.YoyaSyu = 1 AND eTKD_Yyksho01.SeiTaiYmd BETWEEN @StartPaymentDate AND @EndPaymentDate) OR (eTKD_Yyksho01.YoyaSyu = 2 AND eTKD_Yyksho01.SeiTaiYmd BETWEEN @StartPaymentDate AND @EndPaymentDate)))
  AND (@StartPaymentDate = '' OR ((eTKD_Yyksho01.YoyaSyu = 1 AND eTKD_Yyksho01.SeiTaiYmd >= @StartPaymentDate) OR (eTKD_Yyksho01.YoyaSyu = 2 AND eTKD_Yyksho01.CanYmd >= @StartPaymentDate)))
     AND (@EndPaymentDate = '' OR ((eTKD_Yyksho01.YoyaSyu = 1 AND eTKD_Yyksho01.SeiTaiYmd <= @EndPaymentDate) OR (eTKD_Yyksho01.YoyaSyu = 2 AND eTKD_Yyksho01.CanYmd >= @EndPaymentDate)))
  AND (@StartReceptionNumber = 0 OR eTKD_Yyksho01.UkeCd >= @StartReceptionNumber)
  AND (@EndReceptionNumber = 0 OR eTKD_Yyksho01.UkeCd <= @EndReceptionNumber)
  AND (@StartReservation = 0 OR (eVPM_YoyKbn01.YoyaKbn >= @StartReservation))
  AND (@EndReservation = 0 OR (eVPM_YoyKbn01.YoyaKbn <= @EndReservation))
  AND (@BillingType = '' OR TKD_Mishum.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
  AND (@UnpaidSelection = '0' OR (@UnpaidSelection = '1' AND SeiKin = CouKesRui) OR (@UnpaidSelection = '2' AND (SeiKin <> CouKesRui AND CouKesRui <> 0)) OR (@UnpaidSelection = '3' AND CouKesRui = 0))
  AND (@SaleOfficeSelectiton = '' OR (@SaleOfficeKbn = '請求営業所' AND eVPM_Eigyos01.EigyoCdSeq = @SaleOfficeSelectiton) OR (@SaleOfficeKbn = '受付営業所' AND eVPM_Eigyos02.EigyoCdSeq = @SaleOfficeSelectiton))
  AND FORMAT(eVPM_Gyosya02.GyosyaCd, '000') + FORMAT(eVPM_Tokisk02.TokuiCd, '0000') + FORMAT(eVPM_TokiSt02.SitenCd, '0000') >= @StartBillingAddress
  AND FORMAT(eVPM_Gyosya02.GyosyaCd, '000') + FORMAT(eVPM_Tokisk02.TokuiCd, '0000') + FORMAT(eVPM_TokiSt02.SitenCd, '0000') <= @EndBillingAddress
  AND TKD_Mishum.SeiKin - CASE
                              WHEN TKD_Mishum.SeiFutSyu IN (1,
                                                            7) THEN ISNULL(eTKD_NyShmi11.NyukinG, 0) + ISNULL(eTKD_NyShmi11.FuriTes, 0)
                              ELSE ISNULL(eTKD_NyShmi12.NyukinG, 0) + ISNULL(eTKD_NyShmi12.FuriTes, 0)
                          END <> 0 
						  
						  GROUP BY eVPM_Eigyos01.EigyoCd,
                                            eVPM_Eigyos01.EigyoCdSeq,
                                            eVPM_Eigyos01.EigyoNm, 
                                            eVPM_Eigyos01.RyakuNm,
                                            eVPM_Gyosya02.GyosyaCd, 
                                            eVPM_Tokisk02.TokuiCd, 
                                            eVPM_Tokisk02.TokuiNm, 
                                            eVPM_Tokisk02.RyakuNm, 
                                            eVPM_TokiSt02.SitenCd, 
                                            eVPM_TokiSt02.SitenNm, 
                                            eVPM_TokiSt02.RyakuNm, 
                                            eVPM_Gyosya02.GyosyaNm
											
  ORDER BY EigyoCd,
           SeiGyosyaCd,
           SeiCd,
           SeiSitenCd
		   OFFSET @Skip ROWS FETCH FIRST @Take ROWS only
		   
 END