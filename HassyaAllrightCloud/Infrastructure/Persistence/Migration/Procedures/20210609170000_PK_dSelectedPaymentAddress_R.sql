USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSelectedPaymentAddress_R]    Script Date: 11/30/2020 3:52:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dSelectedPaymentAddress_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get payment address data List
-- Date			:   2020/10/12
-- Author		:   N.N.T.AN
-- Description	:   Get payment address with conditions
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_dSelectedPaymentAddress_R] (--Parameter
 @TenantCdSeq int, --Tenant Id
 @SalesOfficeKbn varchar(max) = '',
 @StartPaymentDate varchar(max) = '',
 @EndPaymentDate varchar(max) = '',
 @CompanyCdSeq int,
 @StartReceptionNumber bigint,
 @EndReceptionNumber bigint,
 @StartReservationCategory int,
 @EndReservationCategory int,
 @BillingTypeSelection varchar(max)= '',
 @StartTransferBank varchar(max)= '',
 @EndTransferBank varchar(max)= '',
 @PaymentMethod varchar(max)= '',
 @SalesOfficeSelection varchar(max),
 @StartBillingAddress varchar(max)= '',
 @EndBillingAddress varchar(max)= '',
 --Output
 @ROWCOUNT INTEGER OUTPUT -- ROWCOUNT
 ) AS -- Processing
BEGIN
SELECT																																								
		    DISTINCT CAST(ISNULL(eVPM_Gyosya02.GyosyaCd, 0) AS varchar) AS CSeiGyosyaCd,																																								
		    CAST(ISNULL(eVPM_Tokisk02.TokuiCd, 0)AS varchar) AS CSeiCd,																																								
		    CAST(ISNULL(eVPM_TokiSt02.SitenCd, 0)AS varchar) AS CSeiSitenCd,																																								
		    CAST(ISNULL(eVPM_TokiSt02.SeiCdSeq, 0) AS varchar) AS CSeiCdSeq,																																								
		    CAST(ISNULL(eVPM_TokiSt02.SeiSitenCdSeq, 0) AS varchar) AS CSeiSitenCdSeq,																																								
		    ISNULL(eVPM_Gyosya02.GyosyaNm, '' ) AS CGyosyaNm,																																								
		    ISNULL(eVPM_Tokisk02.RyakuNm, '' ) AS CSeiRyakuNm,																																								
		    ISNULL(eVPM_TokiSt02.RyakuNm, '' ) AS CSitRyakuNm,																																								
		    ISNULL(eVPM_Tokisk02.SiyoStaYmd, '' ) AS CSeiSiyoStaYmd,																																								
		    ISNULL(eVPM_Tokisk02.SiyoEndYmd, '' ) AS CSeiSiyoEndYmd,																																								
		    ISNULL(eVPM_TokiSt02.SiyoStaYmd, '' ) AS CSitSiyoStaYmd,																																								
		    ISNULL(eVPM_TokiSt02.SiyoEndYmd, '' ) AS CSitSiyoEndYmd																																								
		FROM																																								
		    TKD_Yyksho AS eTKD_Yyksho01																																								
		    LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01 ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq																																								
		    AND eTKD_Yyksho01.TenantCdSeq = eVPM_YoyKbn01.TenantCdSeq																																							
		    LEFT JOIN TKD_NyShmi AS eTKD_NyShmi01 ON eTKD_Yyksho01.UkeNo = eTKD_NyShmi01.UkeNo																																								
		    AND eTKD_NyShmi01.NyuSihKbn = 1																																								
		    AND eTKD_NyShmi01.SiyoKbn = 1																																								
		    AND (																																								
		        (																																								
		            eTKD_Yyksho01.YoyaSyu = 1																																								
		            AND eTKD_NyShmi01.SeiFutSyu <> 7																																								
		        )																																								
		        OR (																																								
		            eTKD_Yyksho01.YoyaSyu = 2																																								
		            AND eTKD_NyShmi01.SeiFutSyu = 7																																								
		        )																																								
		    )			
			AND eTKD_NyShmi01.TenantCdSeq = @TenantCdSeq
		    JOIN TKD_NyuSih AS eTKD_NyuSih01 ON eTKD_NyShmi01.NyuSihTblSeq = eTKD_NyuSih01.NyuSihTblSeq																																								
		    AND eTKD_NyuSih01.SiyoKbn = 1 
			AND eTKD_NyuSih01.TenantCdSeq = @TenantCdSeq 
			JOIN VPM_Eigyos AS eVPM_Eigyos01 ON
			(@SalesOfficeKbn = '' OR (@SalesOfficeKbn = '請求営業所' AND eVPM_Eigyos01.EigyoCdSeq = eTKD_Yyksho01.SeiEigCdSeq) OR (@SalesOfficeKbn = '受付営業所' AND eVPM_Eigyos01.EigyoCdSeq = eTKD_Yyksho01.UkeEigCdSeq)
			OR (@SalesOfficeKbn = '入金営業所' AND eVPM_Eigyos01.EigyoCdSeq = eTKD_NyuSih01.NyuSihEigSeq))
		    JOIN VPM_Compny AS eVPM_Compny01 ON eVPM_Compny01.CompanyCdSeq = eVPM_Eigyos01.CompanyCdSeq																																								
		    AND eVPM_Compny01.SiyoKbn = 1																																								
		    INNER JOIN VPM_Tokisk AS eVPM_Tokisk01 ON eTKD_Yyksho01.TokuiSeq = eVPM_Tokisk01.TokuiSeq																																								
		    AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd																																								
		    AND eVPM_Tokisk01.SiyoEndYmd																																								
		    AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq																																							
		    INNER JOIN VPM_TokiSt AS eVPM_TokiSt01 ON eTKD_Yyksho01.TokuiSeq = eVPM_TokiSt01.TokuiSeq																																								
		    AND eTKD_Yyksho01.SitenCdSeq = eVPM_TokiSt01.SitenCdSeq																																								
		    AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd																																								
		    AND eVPM_TokiSt01.SiyoEndYmd																																								
		    LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01 ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq		
			AND eVPM_Tokisk01.TenantCdSeq = eVPM_Gyosya01.TenantCdSeq
		    LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02 ON eVPM_TokiSt01.SeiCdSeq = eVPM_Tokisk02.TokuiSeq																																								
		    AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd																																								
		    AND eVPM_Tokisk02.SiyoEndYmd		
			AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq
		    LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02 ON eVPM_TokiSt01.SeiCdSeq = eVPM_TokiSt02.TokuiSeq																																								
		    AND eVPM_TokiSt01.SeiSitenCdSeq = eVPM_TokiSt02.SitenCdSeq																																								
		    AND eTKD_Yyksho01.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd																																								
		    AND eVPM_TokiSt02.SiyoEndYmd																																								
		    LEFT JOIN VPM_Gyosya AS eVPM_Gyosya02 ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya02.GyosyaCdSeq	
			AND eVPM_Tokisk02.TenantCdSeq = eVPM_Gyosya02.TenantCdSeq
		    LEFT JOIN VPM_Bank AS eVPM_Bank01 ON eTKD_NyuSih01.BankCd = eVPM_Bank01.BankCd																																								
		    LEFT JOIN VPM_BankSt AS eVPM_BankSt01 ON eTKD_NyuSih01.BankCd = eVPM_BankSt01.BankCd																																								
		    AND eTKD_NyuSih01.BankSitCd = eVPM_BankSt01.BankSitCd																																								
		Where																																								
		    eTKD_Yyksho01.TenantCdSeq = @TenantCdSeq																																							
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
			AND FORMAT(eVPM_Gyosya02.GyosyaCd, '000') + FORMAT(eVPM_Tokisk02.TokuiCd, '0000') + FORMAT(eVPM_TokiSt02.SitenCd, '0000') >= @StartBillingAddress
			AND FORMAT(eVPM_Gyosya02.GyosyaCd, '000') + FORMAT(eVPM_Tokisk02.TokuiCd, '0000') + FORMAT(eVPM_TokiSt02.SitenCd, '0000') <= @EndBillingAddress
		order by																																								
		    CSeiGyosyaCd asc,																																								
		    CSeiCd asc,																																								
		    CSeiCdSeq asc,																																								
		    CSeiSiyoStaYmd asc,																																								
		    CSeiSiyoEndYmd asc,																																								
		    CSeiSitenCd asc,																																								
		    CSeiSitenCdSeq asc,																																								
		    CSitSiyoStaYmd asc,																																								
		    CSitSiyoEndYmd asc																																								
																																																																								
SET @ROWCOUNT = @@ROWCOUNT END RETURN