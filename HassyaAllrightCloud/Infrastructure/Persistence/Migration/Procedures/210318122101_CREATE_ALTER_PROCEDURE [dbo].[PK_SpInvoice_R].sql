USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_SpInvoice_R]    Script Date: 03/18/2021 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_SpInvoice_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Invoice List Data
-- Date			:   2021/03/18
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_SpInvoice_R]
    (
     -- Parameter
	     @TenantCdSeq           VARCHAR(8)
     ,   @EigyoCdSeq           VARCHAR(8)             
	 ,   @SeikYm                VARCHAR(8)             
	 ,   @StaInvoicingDate      VARCHAR(8)  
     ,   @EndInvoicingDate      VARCHAR(8)	 
	 ,   @StaInvoiceOutNum      VARCHAR(8)
	 ,   @StaInvoiceSerNum      VARCHAR(8)
	 ,   @EndInvoiceOutNum      VARCHAR(8)
	 ,   @EndInvoiceSerNum      VARCHAR(8)
	 ,   @GyosyaCd              VARCHAR(8)
	 ,   @TokuiCd               VARCHAR(8)
	 ,   @SitenCd               VARCHAR(8)
	 
        -- Output
	 ,	@ROWCOUNT	   INTEGER OUTPUT	   -- 処理件数
    )
AS 
    -- Processing
	BEGIN
		SELECT																																		
    		  TKD_Seikyu.SeiOutSeq,																																		
    		  TKD_Seikyu.SeiRen,																																		
    		  TKD_Seikyu.SeikYm,																																		
    		  TKD_Seikyu.ZenKurG,																																		
    		  TKD_Seikyu.KonUriG,																																		
    		  TKD_Seikyu.KonSyoG,																																		
    		  TKD_Seikyu.KonTesG,																																		
    		  TKD_Seikyu.KonNyuG,																																		
    		  TKD_Seikyu.KonSeiG,																																		
    		  CASE																																		
        	  	WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn, 0) = 1 THEN ISNULL(eVPM_Tokisk11.TokuiNm, ' ')																																		
        	  	ELSE ISNULL(eTKD_SeiPrS11.TokuiNm, ' ')																																		
    		  END AS TokuiNm,																																		
    		  CASE																																		
        	  	WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn, 0) = 1 THEN ISNULL(eVPM_TokiSt11.SitenNm, ' ')																																		
        	  	ELSE ISNULL(eTKD_SeiPrS11.SitenNm, ' ')																																		
    		  END AS SitenNm,																																		
    		  ISNULL(eVPM_Eigyos11.EigyoNm, ' ') AS SeiEigEigyoNm,																																		
    		  ISNULL(eVPM_Tokist11.TokuiTanNm, ' ') AS TokuiTanNm,																																		
    		  ISNULL(eVPM_Compny11.CompanyNm, ' ') AS SeiEigCompanyNm,																																		
			  ISNULL(eTKD_SeiPrS11.SeiHatYmd, ' ') AS SeiHatYmd																																		
	    FROM																																		
    		  TKD_Seikyu
              LEFT JOIN TKD_SeiPrS AS eTKD_SeiPrS11 ON TKD_Seikyu.SeiOutSeq = eTKD_SeiPrS11.SeiOutSeq
              AND TKD_Seikyu.TenantCdSeq = eTKD_SeiPrS11.TenantCdSeq
              LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11 ON TKD_Seikyu.TokuiSeq = eVPM_Tokisk11.TokuiSeq
              AND TKD_Seikyu.SiyoEndYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd
              AND eVPM_Tokisk11.SiyoEndYmd
              AND eVPM_Tokisk11.TenantCdSeq = TKD_Seikyu.TenantCdSeq
              LEFT JOIN VPM_TokiSt AS eVPM_TokiSt11 ON TKD_Seikyu.TokuiSeq = eVPM_TokiSt11.TokuiSeq
              AND TKD_Seikyu.SitenCdSeq = eVPM_TokiSt11.SitenCdSeq
              AND TKD_Seikyu.SiyoEndYmd BETWEEN eVPM_TokiSt11.SiyoStaYmd
              AND eVPM_TokiSt11.SiyoEndYmd
              LEFT JOIN VPM_Gyosya AS eVPM_Gyosya11 ON eVPM_Tokisk11.GyosyaCdSeq = eVPM_Gyosya11.GyosyaCdSeq
			  AND eVPM_Tokisk11.TenantCdSeq = eVPM_Gyosya11.TenantCdSeq
              LEFT JOIN VPM_Eigyos AS eVPM_Eigyos11 ON eTKD_SeiPrS11.SeiEigCdSeq = eVPM_Eigyos11.EigyoCdSeq
              LEFT JOIN VPM_Compny AS eVPM_Compny11 ON eVPM_Eigyos11.CompanyCdSeq = eVPM_Compny11.CompanyCdSeq																																		
        WHERE																																		
              TKD_Seikyu.SiyoKbn = 1
			  AND TKD_Seikyu.TenantCdSeq= @TenantCdSeq
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
			  AND (@GyosyaCd IS NULL OR @GyosyaCd = '' OR eVPM_Gyosya11.GyosyaCd = @GyosyaCd )
			  AND (@TokuiCd IS NULL OR @TokuiCd = '' OR eVPM_Tokisk11.TokuiCd = @TokuiCd )
			  AND (@SitenCd IS NULL OR @SitenCd = '' OR eVPM_TokiSt11.SitenCd = @SitenCd )
			  OPTION(RECOMPILE)
	END
    SET	@ROWCOUNT	=	@@ROWCOUNT
RETURN																													

