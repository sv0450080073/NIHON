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
-- Name			:   GetOutDataTableAsync
-- Date			:   2020/08/10
-- Author		:   T.L.DUY
-- Description	:   Get outdatatable data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dOutDataTable_R] 
	(
	-- Parameter
		 @EigyoCd			INT,				-- 営業所コード
		 @CompanyCd			INT,				-- 会社コード
		 @UkeNo			NVARCHAR(15),				-- 受付番号
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	-- Processing
BEGIN



	SELECT																																		
	    ISNULL(																																		
	        (																																		
	            CASE																																		
	                WHEN eVPM_Eigyos01.RyakuNm = ' ' THEN eVPM_Eigyos01.EigyoNm																																		
	                ELSE eVPM_Eigyos01.RyakuNm																																		
	            END																																		
	        ),																																		
	        ' '																																		
	    ) AS EigyoNm,																																		
	    ISNULL(eVPM_Compny02.SyoriYm, '') AS SyoriYm,																																		
	    ISNULL(eVPM_TokiSt05.ZipCd, '') AS TokuiZipCd,																																		
	    ISNULL(eVPM_TokiSt05.Jyus1, '') AS TokuiJyus1,																																		
	    ISNULL(eVPM_TokiSt05.Jyus2, '') AS TokuiJyus2,																																		
	    ISNULL(eVPM_Tokisk04.TokuiNm, '') AS TokuiTokuiNm,																																		
	    ISNULL(eVPM_TokiSt05.SitenNm, '') AS TokuiSitenNm,																																		
	    ISNULL(eVPM_TokiSt07.ZipCd, '') AS SeikyZipCd,																																		
	    ISNULL(eVPM_TokiSt07.Jyus1, '') AS SeikyJyus1,																																		
	    ISNULL(eVPM_TokiSt07.Jyus2, '') AS SeikyJyus2,																																		
	    ISNULL(eVPM_Tokisk06.TokuiNm, '') AS SeikyTokuiNm,																																		
	    ISNULL(eVPM_TokiSt07.SitenNm, '') AS SeikySitenNm,																																		
	    ISNULL(eVPM_Eigyos08.EigyoCdSeq, 0) AS SeiEigyoCdSeq,																																		
	    ISNULL(eVPM_Eigyos08.EigyoCd, 0) AS SeiEigyoCd,																																		
	    ISNULL(eVPM_Eigyos08.EigyoNm, '') AS SeiEigyoNm		
	FROM																																		
	    (																																		
	        SELECT 																																																																
	            @EigyoCd AS EigyoCd,																																		
	            @CompanyCd AS CompanyCd,																																		
	            @UkeNo AS UkeNo																																												
	    ) AS eDefault01																																		
	    LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01 ON eVPM_Eigyos01.EigyoCd = eDefault01.EigyoCd																																		
	    LEFT JOIN VPM_Compny AS eVPM_Compny02 ON eVPM_Compny02.CompanyCd = eDefault01.CompanyCd																																		
	    LEFT JOIN TKD_Yyksho AS eTKD_Yyksho03 ON eTKD_Yyksho03.UkeNo = eDefault01.UkeNo																																		
	    LEFT JOIN VPM_Tokisk AS eVPM_Tokisk04 ON eVPM_Tokisk04.TokuiSeq = eTKD_Yyksho03.TokuiSeq																																		
	    AND eTKD_Yyksho03.SeiTaiYmd BETWEEN eVPM_Tokisk04.SiyoStaYmd																																		
	    AND eVPM_Tokisk04.SiyoEndYmd																																		
	    LEFT JOIN VPM_TokiSt AS eVPM_TokiSt05 ON eVPM_TokiSt05.TokuiSeq = eTKD_Yyksho03.TokuiSeq																																		
	    AND eVPM_TokiSt05.SitenCdSeq = eTKD_Yyksho03.SitenCdSeq																																		
	    AND eTKD_Yyksho03.SeiTaiYmd BETWEEN eVPM_TokiSt05.SiyoStaYmd																																		
	    AND eVPM_TokiSt05.SiyoEndYmd																																		
	    LEFT JOIN VPM_Tokisk AS eVPM_Tokisk06 ON eVPM_Tokisk06.TokuiSeq = eVPM_TokiSt05.SeiCdSeq																																		
	    AND eTKD_Yyksho03.SeiTaiYmd BETWEEN eVPM_Tokisk06.SiyoStaYmd																																		
	    AND eVPM_Tokisk06.SiyoEndYmd																																		
	    LEFT JOIN VPM_TokiSt AS eVPM_TokiSt07 ON eVPM_TokiSt07.TokuiSeq = eVPM_TokiSt05.SeiCdSeq																																		
	    AND eVPM_TokiSt07.SitenCdSeq = eVPM_TokiSt05.SeiSitenCdSeq																																		
	    AND eTKD_Yyksho03.SeiTaiYmd BETWEEN eVPM_TokiSt07.SiyoStaYmd																																		
	    AND eVPM_TokiSt07.SiyoEndYmd																																		
	    LEFT JOIN VPM_Eigyos AS eVPM_Eigyos08 ON eVPM_Eigyos08.EigyoCdSeq = eTKD_Yyksho03.SeiEigCdSeq	
		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN