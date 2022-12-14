USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBillingAddress_R]    Script Date: 8/21/2020 9:29:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetBillingAddressesAsync
-- Date			:   2020/08/10
-- Author		:   T.L.DUY
-- Description	:   Get billing address data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dBillingAddress_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,				-- TenantCdSeq
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	-- Processing
BEGIN
	SELECT
		VPM_Tokisk.TokuiSeq AS SeiCdSeq,
		VPM_TokiSt.SitenCdSeq AS SeiSitCdSeq,
	    concat(																									
	        FORMAT(VPM_Gyosya.GyosyaCd,'000'),																									
	        FORMAT(VPM_Tokisk.TokuiCd,'0000'),																									
	        FORMAT(VPM_TokiSt.SitenCd,'0000')																									
	    ) AS Code,																									
	    concat(																									
	        FORMAT(VPM_Tokisk.TokuiCd,'0000'),																									
	        ' : ',																									
	        VPM_Tokisk.RyakuNm,																									
	        ' ',																									
	        FORMAT(VPM_TokiSt.SitenCd,'0000'),																									
	        ' : ',																									
	        VPM_TokiSt.SitenNm																									
	    ) AS Name,
	    concat(																									
	        FORMAT(VPM_Tokisk.TokuiCd,'0000'),																									
	        ' : ',																									
	        VPM_Tokisk.RyakuNm,																									
	        ' ',																									
	        FORMAT(VPM_TokiSt.SitenCd,'0000'),																									
	        ' : ',																									
	        VPM_TokiSt.SitenNm																									
	    ) AS CodeText	
		FROM																									
	    VPM_Tokisk																									
	    INNER JOIN VPM_TokiSt ON VPM_TokiSt.TokuiSeq = VPM_Tokisk.TokuiSeq																									
	    AND (																									
	        CAST(VPM_TokiSt.SiyoStaYmd AS DATE) <= CAST(GETDATE() AS DATE)																									
	    )																									
	    AND (																									
	        CAST(VPM_TokiSt.SiyoEndYmd AS DATE) >= CAST(GETDATE() AS DATE)																									
	    )																									
	    INNER JOIN VPM_Gyosya ON VPM_Tokisk.GyosyaCdSeq = VPM_Gyosya.GyosyaCdSeq																									
	    AND VPM_Gyosya.SiyoKbn = 1																									
		WHERE																									
	    (																									
	        CAST(VPM_Tokisk.SiyoStaYmd AS DATE) <= CAST(GETDATE() AS DATE)																									
	    )																									
	    AND (																									
	        CAST(VPM_Tokisk.SiyoEndYmd AS DATE) >= CAST(GETDATE() AS DATE)																									
	    )																									
	    AND VPM_Tokisk.TenantCdSeq = @TenantCdSeq																								
		ORDER BY	 Code	
		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN