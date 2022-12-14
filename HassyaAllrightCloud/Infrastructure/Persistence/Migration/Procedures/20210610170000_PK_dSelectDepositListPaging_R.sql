USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSelectDepositListPaging_R]    Script Date: 11/12/2020 4:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dSelectDepositList_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get selected deposit  data List
-- Date			:   2020/10/12
-- Author		:   N.N.T.AN
-- Description	:   Get deposit list with conditions
------------------------------------------------------------

CREATE OR ALTER         PROCEDURE [dbo].[PK_dSelectDepositListPaging_R] (--Parameter
 @TenantCdSeq INTEGER, --Tenant Id
 @SalesOfficeKbn varchar(max) = '',
 @StartPaymentDate varchar(max)= '',
 @EndPaymentDate varchar(max)= '',
 @CompanyCdSeq int,
 @StartReceptionNumber bigint,
 @EndReceptionNumber bigint,
 @StartReservationCategory int,
 @EndReservationCategory int,
 @BillingTypeSelection varchar(max)= '',
 @StartTransferBank varchar(max)= '',
 @EndTransferBank varchar(max)= '',
 @PaymentMethod varchar(max)= '',
 @SalesOfficeSelection varchar(max)= '',
 @StartBillingAddress varchar(max)= '',
 @EndBillingAddress varchar(max)= '',
 @CSeiGyosyaCd varchar(max)= '',
 @CSeiCd varchar(max)= '',
 @CSeiSitenCd varchar(max)= '',
 @CSeiCdSeq varchar(max)= '',
 @CSeiSiyoStaYmd varchar(max)= '',
 @CSeiSiyoEndYmd varchar(max)= '',
 @CSeiSitenCdSeq varchar(max)= '',
 @CSitSiyoStaYmd varchar(max)= '',
 @CSitSiyoEndYmd varchar(max)= '',
 @Skip int,
 @Take int
 ) AS -- Processing
 BEGIN
SELECT eTKD_NyShmi01.*,
       eTKD_NyuSih01.*,
	   CAST(eTKD_NyuSih01.NyuSihSyu AS VARCHAR) AS NyuSihSyuS,
	   CAST(eTKD_NyShmi01.UnkRen AS VARCHAR) AS UnkRenS,
       CAST(ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS VARCHAR) AS EigyoCd,
       ISNULL(eVPM_Eigyos01.EigyoCdSeq, 0) AS EigyoCdSeq,
       ISNULL(eVPM_Eigyos01.EigyoNm, '') AS EigyoNm,
       ISNULL(eVPM_Eigyos01.RyakuNm, '') AS EigyoRyak,
       CAST(ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS VARCHAR) AS SeiGyosyaCd,
       CAST(ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS VARCHAR) AS SeiCd,
       ISNULL(eVPM_Tokisk02.TokuiNm, '') AS SeiCdNm,
       ISNULL(eVPM_Tokisk02.RyakuNm, '') AS SeiRyakuNm,
       CAST(ISNULL(eVPM_TokiSt02.SitenCd, 0) AS VARCHAR) AS SeiSitenCd,
       ISNULL(eVPM_TokiSt02.SitenNm, '') AS SeiSitenCdNm,
       ISNULL(eVPM_TokiSt02.RyakuNm, '') AS SeiSitRyakuNm,
       ISNULL(eVPM_Gyosya02.GyosyaNm, '') AS SeiGyosyaCdNm,
       CAST(ISNULL(eVPM_Eigyos02.EigyoCd, 0) AS VARCHAR) AS UkeEigCd,
       ISNULL(eVPM_Eigyos02.EigyoNm, '') AS UkeEigyoNm,
       ISNULL(eVPM_Eigyos02.RyakuNm, '') AS UkeRyakuNm,
       ISNULL(eVPM_CodeKb01.CodeKbnNm, '') AS SeiFutSyuNm,
       ISNULL(eVPM_CodeKb02.CodeKbnNm, '') AS NyuKinTejNm,
       CAST(ISNULL(eVPM_Tokisk01.TokuiCd, 0) AS VARCHAR) AS TokuiCd,
       ISNULL(eVPM_Tokisk01.TokuiNm, '') AS TokuiNm,
       ISNULL(eVPM_Tokisk01.RyakuNm, '') AS TokRyakuNm,
       CAST(ISNULL(eVPM_TokiSt01.SitenCd, 0) AS VARCHAR) AS SitenCd,
       ISNULL(eVPM_TokiSt01.SitenNm, '') AS SitenNm,
       ISNULL(eVPM_TokiSt01.RyakuNm, '') AS SitRyakuNm,
       CAST(ISNULL(eVPM_Gyosya01.GyosyaCd, 0) AS VARCHAR) AS GyosyaCd,
       ISNULL(eVPM_Gyosya01.GyosyaNm, '') AS GyosyaNm,
       ISNULL((CASE
                   WHEN ISNULL(eTKD_NyShmi01.SeiFutSyu, 0) IN(1, 5, 7) THEN eTKD_Unkobi03.HaiSYmd
                   ELSE eTKD_Unkobi04.HaiSYmd
               END), '') AS HaiSYmd,
       ISNULL((CASE
                   WHEN ISNULL(eTKD_NyShmi01.SeiFutSyu, 0) IN(1, 5, 7) THEN eTKD_Unkobi03.TouYmd
                   ELSE eTKD_Unkobi04.TouYmd
               END), '') AS TouYmd,
       ISNULL((CASE
                   WHEN ISNULL(eTKD_NyShmi01.SeiFutSyu, 0) IN(1, 5, 7) THEN eTKD_Unkobi03.IkNm
                   ELSE eTKD_Unkobi04.IkNm
               END), '') AS IkNm,
       ISNULL((CASE
                   WHEN ISNULL(eTKD_NyShmi01.SeiFutSyu, 0) IN(1, 5, 7) THEN eTKD_Yyksho01.YoyaNm
                   ELSE eTKD_Unkobi04.DanTaNm
               END), '') AS DanTaNm,
       ISNULL((CASE
                   WHEN ISNULL(eTKD_NyShmi01.SeiFutSyu, 0) IN(1, 7) THEN ''
                   ELSE eTKD_FutTum01.FutTumNm
               END), '') AS FutTumNm,
       ISNULL(eVPM_Bank01.BankNm, '') AS BankNm,
       ISNULL(eVPM_Bank01.RyakuNm, '') AS BankRyak,
       ISNULL(eVPM_BankSt01.BankSitNm, '') AS BankSitNm,
       ISNULL(eVPM_BankSt01.RyakuNm, '') AS BankSitRyak,
       CAST(ISNULL((CASE
                   WHEN ISNULL(eTKD_NyuSih01.NyuSihSyu, 0) = 7 THEN eTKD_Coupon01.CouGkin
                   ELSE 0
               END), 0) AS VARCHAR)AS CouGkin,
       ISNULL((CASE
                   WHEN ISNULL(eTKD_NyuSih01.NyuSihSyu, 0) = 7 THEN eTKD_Coupon01.CouNo
                   ELSE ''
               END), '') AS CouNo,
       ISNULL(eTKD_Yyksho01.SeiTaiYmd, '') AS SeiTaiYmd,
       ISNULL(eTKD_TokuYm.SeikYm, '') AS SeikYm,
       ISNULL(eVPM_Tokisk02.SiyoStaYmd, 0) AS TSiyoStaYmd,
       ISNULL(eVPM_Tokisk02.SiyoEndYmd, 0) AS TSiyoEndYmd,
       ISNULL(eVPM_TokiSt02.SiyoStaYmd, 0) AS SSiyoStaYmd,
       ISNULL(eVPM_TokiSt02.SiyoEndYmd, 0) AS SSiyoEndYmd,
       eTKD_Unkobi03.YouKbn AS YouKbn
FROM TKD_Yyksho AS eTKD_Yyksho01
LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01 ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
AND eTKD_Yyksho01.TenantCdSeq = eVPM_YoyKbn01.TenantCdSeq	
LEFT JOIN TKD_NyShmi AS eTKD_NyShmi01 ON eTKD_Yyksho01.UkeNo = eTKD_NyShmi01.UkeNo
AND eTKD_NyShmi01.NyuSihKbn = 1
AND eTKD_NyShmi01.SiyoKbn = 1
AND ((eTKD_Yyksho01.YoyaSyu = 1
      AND eTKD_NyShmi01.SeiFutSyu <> 7)
     OR (eTKD_Yyksho01.YoyaSyu = 2
         AND eTKD_NyShmi01.SeiFutSyu = 7))
         AND eTKD_NyShmi01.TenantCdSeq = @TenantCdSeq
JOIN TKD_NyuSih AS eTKD_NyuSih01 ON eTKD_NyShmi01.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq
AND eTKD_NyuSih01.SiyoKbn = 1 
AND eTKD_NyuSih01.TenantCdSeq = @TenantCdSeq
JOIN VPM_Eigyos AS eVPM_Eigyos01 ON 
(@SalesOfficeKbn = '' OR (@SalesOfficeKbn = '請求営業所' AND eVPM_Eigyos01.EigyoCdSeq = eTKD_Yyksho01.SeiEigCdSeq) OR (@SalesOfficeKbn = '受付営業所' AND eVPM_Eigyos01.EigyoCdSeq = eTKD_Yyksho01.UkeEigCdSeq)
			OR (@SalesOfficeKbn = '入金営業所' AND eVPM_Eigyos01.EigyoCdSeq = eTKD_NyuSih01.NyuSihEigSeq))
JOIN VPM_Compny AS eVPM_Compny01 ON eVPM_Compny01.CompanyCdSeq = eVPM_Eigyos01.CompanyCdSeq
AND eVPM_Compny01.SiyoKbn = 1
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01 ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01 ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq
AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01 ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
AND eVPM_Tokisk01.TenantCdSeq = eVPM_Gyosya01.TenantCdSeq
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02 ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02 ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq
AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq
AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02 ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq
AND eVPM_Tokisk02.TenantCdSeq = eVPM_Gyosya02.TenantCdSeq
JOIN VPM_Eigyos AS eVPM_Eigyos02 ON eVPM_Eigyos02.EigyoCdSeq = eTKD_Yyksho01.UkeEigCdSeq
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeKbn = FORMAT(eTKD_NyShmi01.SeiFutSyu, '00')
AND eVPM_CodeKb01.CodeSyu = 'SEIFUTSYU'
AND eVPM_CodeKb01.TenantCdSeq =
  (SELECT CASE
              WHEN COUNT(*) = 0 THEN 0
              ELSE @TenantCdSeq
          END AS TenantCdSeq
   FROM VPM_CodeKb
   WHERE VPM_CodeKb.CodeSyu = 'SEIFUTSYU'
     AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq )
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02 ON eVPM_CodeKb02.CodeKbn = dbo.FP_RpZero(2, eTKD_NyuSih01.NyuSihSyu)
AND eVPM_CodeKb02.CodeSyu = 'NYUSIHSYU'
AND eVPM_CodeKb02 .TenantCdSeq =
  (SELECT CASE
              WHEN COUNT(*) = 0 THEN 0
              ELSE @TenantCdSeq
          END AS TenantCdSeq
   FROM VPM_CodeKb
   WHERE VPM_CodeKb.CodeSyu = 'NYUSIHSYU'
     AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq )
JOIN
  (SELECT TKD_Unkobi01.UkeNo,
          TKD_Unkobi01.HaiSYmd,
          TKD_Unkobi01.TouYmd,
          TKD_Unkobi01.DanTaNm,
          TKD_Unkobi01.IkNm,
          TKD_Unkobi01.YouKbn
   FROM TKD_Unkobi AS TKD_Unkobi01
   WHERE TKD_Unkobi01.SiyoKbn = 1
     AND TKD_Unkobi01.UnkRen =
       (SELECT min(TKD_Unkobi02.UnkRen)
        FROM TKD_Unkobi AS TKD_Unkobi02
        WHERE TKD_Unkobi02.SiyoKbn = 1
          AND TKD_Unkobi02.UkeNo = TKD_Unkobi01.UkeNo ) ) AS eTKD_Unkobi03 ON eTKD_NyShmi01.UkeNo = eTKD_Unkobi03.UkeNo
LEFT JOIN TKD_Unkobi AS eTKD_Unkobi04 ON eTKD_Unkobi04.SiyoKbn = 1
AND eTKD_Unkobi04.UkeNo = eTKD_NyShmi01.UkeNo
AND eTKD_Unkobi04.UnkRen = eTKD_NyShmi01.UnkRen
LEFT JOIN TKD_FutTum AS eTKD_FutTum01 ON eTKD_FutTum01.SiyoKbn = 1
AND eTKD_FutTum01.UkeNo = eTKD_NyShmi01.UkeNo
AND eTKD_FutTum01.UnkRen = eTKD_NyShmi01.UnkRen
AND eTKD_FutTum01.FutTumRen = eTKD_NyShmi01.FutTumRen
AND ((eTKD_NyShmi01.SeiFutSyu = 6
      AND eTKD_FutTum01.FutTumKbn = 2)
     OR (eTKD_NyShmi01.SeiFutSyu <> 6
         AND eTKD_FutTum01.FutTumKbn = 1))
LEFT JOIN VPM_Bank AS eVPM_Bank01 ON eTKD_NyuSih01.BankCd = eVPM_Bank01.BankCd
LEFT JOIN VPM_BankSt AS eVPM_BankSt01 ON eTKD_NyuSih01.BankCd = eVPM_BankSt01.BankCd
AND eTKD_NyuSih01.BankSitCd = eVPM_BankSt01.BankSitCd
LEFT JOIN TKD_Coupon AS eTKD_Coupon01 ON eTKD_Coupon01.SiyoKbn = 1
AND eTKD_Coupon01.CouTblSeq = eTKD_NyShmi01.CouTblSeq
AND eTKD_Coupon01.TenantCdSeq = @TenantCdSeq
LEFT JOIN TKD_TokuYm AS eTKD_TokuYm ON eVPM_TokiSt02.TokuiSeq = eTKD_TokuYm.TokuiSeq
AND eVPM_TokiSt02.SitenCdSeq = eTKD_TokuYm.SitenCdSeq
AND eTKD_Yyksho01.SeiEigCdSeq = eTKD_TokuYm.EigyoCdSeq
WHERE eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq
AND (@StartPaymentDate = '' OR (eTKD_NyuSih01.NyuSihYmd >= @StartPaymentDate))
AND (@EndPaymentDate = '' OR (eTKD_NyuSih01.NyuSihYmd <= @EndPaymentDate))
AND (@CompanyCdSeq = -1 OR (eVPM_Compny01.CompanyCdSeq = @CompanyCdSeq))
AND (@StartReceptionNumber = 0 OR (eTKD_Yyksho01.UkeCd >= @StartReceptionNumber))
AND (@EndReceptionNumber = 0 OR (eTKD_Yyksho01.UkeCd <= @EndReceptionNumber))
AND (@StartReservationCategory = 0 OR (eVPM_YoyKbn01.YoyaKbn >= @StartReservationCategory))
AND (@EndReservationCategory = 0 OR (eVPM_YoyKbn01.YoyaKbn <= @EndReservationCategory))
AND (@BillingTypeSelection = '' OR (eTKD_NyShmi01.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingTypeSelection, ','))))
AND (@StartTransferBank = '' OR (CONCAT(eVPM_Bank01.BankCd, eVPM_BankSt01.BankSitCd) >= @StartTransferBank))
AND (@EndTransferBank = '' OR (CONCAT(eVPM_Bank01.BankCd, eVPM_BankSt01.BankSitCd) <= @EndTransferBank))
AND (@PaymentMethod = '' OR (eTKD_NyuSih01.NyuSihSyu IN (SELECT value FROM STRING_SPLIT(@PaymentMethod, ','))))
AND (@SalesOfficeSelection = '' OR (eVPM_Eigyos01.EigyoCdSeq = @SalesOfficeSelection))
AND (@CSeiGyosyaCd = '' OR (eVPM_Gyosya02.GyosyaCd = @CSeiGyosyaCd))
AND (@CSeiCd = '' OR (eVPM_Tokisk02.TokuiCd = @CSeiCd))
AND (@CSeiSitenCd = '' OR (eVPM_TokiSt02.SitenCd = @CSeiSitenCd))
AND (@CSeiCdSeq = '' OR (eVPM_TokiSt02.SeiCdSeq = @CSeiCdSeq))
AND (@CSeiSiyoStaYmd = '' OR (eVPM_Tokisk01.SiyoStaYmd = @CSeiSiyoStaYmd))
AND (@CSeiSiyoEndYmd = '' OR (eVPM_Tokisk01.SiyoEndYmd = @CSeiSiyoEndYmd))
AND (@CSeiSitenCdSeq = '' OR (eVPM_TokiSt02.SeiSitenCdSeq = @CSeiSitenCdSeq))
AND (@CSitSiyoStaYmd = '' OR (eVPM_TokiSt02.SiyoStaYmd = @CSitSiyoStaYmd))
AND (@CSitSiyoEndYmd = '' OR (eVPM_TokiSt02.SiyoEndYmd = @CSitSiyoEndYmd))
ORDER BY eVPM_Eigyos01.EigyoCd ASC,
         eVPM_Gyosya02.GyosyaCd ASC,
         eVPM_Tokisk02.TokuiCd ASC,
         eVPM_TokiSt02.SitenCd ASC,
         eVPM_TokiSt02.SeiCdSeq ASC,
         eVPM_TokiSt02.SeiSitenCdSeq ASC,
         eTKD_NyuSih01.NyuSihYmd ASC,
         eTKD_NyShmi01.UkeNo ASC,
         eTKD_NyShmi01.UnkRen ASC,
         eTKD_NyShmi01.SeiFutSyu ASC,
         eTKD_NyShmi01.FutTumRen ASC
		 OFFSET @Skip ROWS FETCH FIRST @Take ROWS only
		 END RETURN