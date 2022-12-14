USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dChaterInquiryOtherOption_R]    Script Date: 3/2/2021 8:21:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetPaymentIncidentalTypeAsync
-- Date			:   2021/02/03
-- Author		:   T.L.DUY
-- Description	:   Get PaymentIncidentalType data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dPaymentIncidentalType_R]
		(
		--Parameter
			@TenantCdSeq			INT,
			@SeiFutSyu				TINYINT,
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
SELECT VPM_CodeKb.CodeKbnNm AS CodeKbnNm
FROM VPM_CodeKb
LEFT JOIN VPM_CodeSy
    ON VPM_CodeSy.CodeSyu = 'SEIFUTSYU'
WHERE VPM_CodeKb.CodeSyu = 'SEIFUTSYU'
    AND VPM_CodeKb.CodeKbn = @SeiFutSyu --　「SeiFutSyu」パラメタ
    AND ((VPM_CodeSy.KanriKbn = 1 AND VPM_CodeKb.TenantCdSeq = 0) OR (VPM_CodeSy.KanriKbn != 1 AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq))
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN
