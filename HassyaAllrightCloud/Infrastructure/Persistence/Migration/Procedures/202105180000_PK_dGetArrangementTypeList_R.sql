-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetArrangementTypeList_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get List ArrangementType
-- Date			:   2021/05/18
-- Author		:   P.M.Nhat
-- Description	:   Get list ArrangementType with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dGetArrangementTypeList_R 
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int
AS
BEGIN
	SELECT CodeKbnNm AS TypeName																			
	      ,CONVERT(INT, CodeKbn) AS TypeCode																				
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
	WHERE eTPM_CodeKb1.CodeSyu = 'TEHAIKBN'																								
	        AND eTPM_CodeKb1.SiyoKbn = 1																								
END
GO
