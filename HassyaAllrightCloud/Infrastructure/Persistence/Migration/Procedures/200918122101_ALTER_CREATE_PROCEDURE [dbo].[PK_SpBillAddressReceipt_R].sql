USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_SpBillAddressReceipt_R]    Script Date: 9/18/2020 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_SpBillAddressReceipt
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Bill Address Receipt
-- Date			:   2020/09/18
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_SpBillAddressReceipt_R]
    (
     -- Parameter
         @EigyoCdSeq            VARCHAR(8)             
	 ,   @SeikYm                VARCHAR(8)             
	 ,   @StaInvoicingDate      VARCHAR(8)  
     ,   @EndInvoicingDate      VARCHAR(8)	 
	 ,   @TenantCdSeq           VARCHAR(8)
	 ,   @StaInvoiceOutNum      VARCHAR(8)
	 ,   @StaInvoiceSerNum      VARCHAR(8)
	 ,   @EndInvoiceOutNum      VARCHAR(8)
	 ,   @EndInvoiceSerNum      VARCHAR(8)
	 ,   @StaBillingAddress     VARCHAR(11)
	 ,   @EndBillingAddress     VARCHAR(11)
	 
        -- Output
	 ,	@ROWCOUNT	   INTEGER OUTPUT	   -- 処理件数
    )
AS 
    -- Processing
	BEGIN
	SELECT																																		
          DISTINCT ISNULL(eVPM_Gyosya11.GyosyaCd, 0) AS GyosyaCd,
		  ISNULL(eVPM_Tokisk11.TokuiSeq, 0) AS TokuiSeq,
          ISNULL(eVPM_Tokisk11.TokuiCd, 0) AS TokuiCd,																																		
          ISNULL(eVPM_TokiSt11.SitenCd, 0) AS SitenCd,																																		
          ISNULL(eVPM_Tokisk11.RyakuNm, ' ') AS TokuiNm,																																		
          ISNULL(eVPM_TokiSt11.RyakuNm, ' ') AS SitenNm																																		
    FROM																																		
          TKD_Seikyu																																		
          LEFT JOIN TKD_SeiPrS AS eTKD_SeiPrS11 ON TKD_Seikyu.SeiOutSeq = eTKD_SeiPrS11.SeiOutSeq																																		
          LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11 ON TKD_Seikyu.TokuiSeq = eVPM_Tokisk11.TokuiSeq																																		
          AND TKD_Seikyu.SiyoEndYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd																																		
          AND eVPM_Tokisk11.SiyoEndYmd																																		
          AND eVPM_Tokisk11.TenantCdSeq = @TenantCdSeq 																																		
          LEFT JOIN VPM_TokiSt AS eVPM_TokiSt11 ON TKD_Seikyu.TokuiSeq = eVPM_TokiSt11.TokuiSeq																																		
          AND TKD_Seikyu.SitenCdSeq = eVPM_TokiSt11.SitenCdSeq																																		
          AND TKD_Seikyu.SiyoEndYmd BETWEEN eVPM_TokiSt11.SiyoStaYmd																																		
          AND eVPM_TokiSt11.SiyoEndYmd																																		
          LEFT JOIN VPM_Gyosya AS eVPM_Gyosya11 ON eVPM_Tokisk11.GyosyaCdSeq = eVPM_Gyosya11.GyosyaCdSeq																																		
          LEFT JOIN VPM_Eigyos AS eVPM_Eigyos11 ON eTKD_SeiPrS11.SeiEigCdSeq = eVPM_Eigyos11.EigyoCdSeq																																																									
    WHERE   1 = 1 
			AND TKD_Seikyu.SiyoKbn = 1
			AND NOT EXISTS (																																		
        				SELECT																																		
            				*																																		
        				FROM																																		
            				TKD_Ryoshu																																		
        				WHERE																																		
            				TKD_Seikyu.SeiOutSeq = TKD_Ryoshu.SeiOutSeq																																		
            				AND TKD_Seikyu.SeiRen = TKD_Ryoshu.SeiRen																																		
            				AND TKD_Ryoshu.SiyoKbn = 1																																		
    		)
            AND (@EigyoCdSeq IS NULL OR @EigyoCdSeq = '' OR eVPM_Eigyos11.EigyoCdSeq = @EigyoCdSeq)
            AND (@SeikYm IS NULL OR @SeikYm = '' OR TKD_Seikyu.SeikYm = @SeikYm )
            AND (@StaInvoicingDate  IS NULL OR @StaInvoicingDate = '' OR eTKD_SeiPrS11.SeiHatYmd >= @StaInvoicingDate)
            AND (@EndInvoicingDate  IS NULL OR @EndInvoicingDate = '' OR eTKD_SeiPrS11.SeiHatYmd <= @EndInvoicingDate)
			AND (((@StaInvoiceOutNum IS NULL OR @StaInvoiceOutNum = '') OR (@StaInvoiceSerNum  IS NULL OR @StaInvoiceSerNum = '')) OR FORMAT(TKD_Seikyu.SeiOutSeq, '00000000') + FORMAT(TKD_Seikyu.SeiRen, '0000') >=  @StaInvoiceOutNum + @StaInvoiceSerNum)
			AND (((@EndInvoiceOutNum IS NULL OR @EndInvoiceOutNum = '') OR (@EndInvoiceSerNum  IS NULL OR @EndInvoiceSerNum = '')) OR FORMAT(TKD_Seikyu.SeiOutSeq, '00000000') + FORMAT(TKD_Seikyu.SeiRen, '0000') <=  @EndInvoiceOutNum + @EndInvoiceSerNum)
	        AND (@StaBillingAddress IS NULL OR @StaBillingAddress = '' OR  FORMAT(eVPM_Gyosya11.GyosyaCd, '000') + FORMAT(eVPM_Tokisk11.TokuiCd, '0000') + FORMAT(eVPM_TokiSt11.SitenCd, '0000') >= @StaBillingAddress)
			AND (@EndBillingAddress IS NULL OR @EndBillingAddress = '' OR  FORMAT(eVPM_Gyosya11.GyosyaCd, '000') + FORMAT(eVPM_Tokisk11.TokuiCd, '0000') + FORMAT(eVPM_TokiSt11.SitenCd, '0000') <= @EndBillingAddress)
	END
    SET	@ROWCOUNT	=	@@ROWCOUNT
RETURN																													

