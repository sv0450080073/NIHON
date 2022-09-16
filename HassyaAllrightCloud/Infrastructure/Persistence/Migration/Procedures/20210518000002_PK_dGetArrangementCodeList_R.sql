USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetArrangementCodeList_R]    Script Date: 5/18/2021 3:34:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetArrangementCodeList_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List ArrangementCode
-- Date			:   2021/05/18
-- Author		:   P.M.Nhat
-- Description	:   Get list ArrangementCode with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dGetArrangementCodeList_R
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int
AS
BEGIN
	SELECT CodeKbnNm AS CodeKbnName
		  ,CodeKbnSeq
	FROM VPM_CodeKb AS eTPM_CodeKb1																								
	LEFT JOIN VPM_CodeSy ON eTPM_CodeKb1.CodeSyu = VPM_CodeSy.CodeSyu																								
	        AND (																								
	                (																								
	                        VPM_CodeSy.KanriKbn = 1																								
	                        AND eTPM_CodeKb1.TenantCdSeq = 0																								
	                        )																								
	                OR (																								
	                        VPM_CodeSy.KanriKbn != 1																								
	                        AND eTPM_CodeKb1.TenantCdSeq = @TenantCdSeq																								
	                        )																								
	                )																								
	WHERE eTPM_CodeKb1.CodeSyu = 'KENCD'																								
	        AND eTPM_CodeKb1.SiyoKbn = 1																								
END
