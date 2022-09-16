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
-- SP-ID		:   PK_dDestinationList_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Destination List
-- Date			:   2021/06/08
-- Author		:   P.M.Nhat
-- Description	:   Get Destination list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dDestinationList_R
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int
AS
BEGIN
	SELECT BASYO.BasyoKenCdSeq AS 'BasyoKenCdSeq'
			,KEN.RyakuNm AS 'TpnCodeKbnRyakuNm'
			,BASYO.BasyoMapCdSeq AS 'BasyoMapCdSeq'
			,BASYO.BasyoMapCd AS 'BasyoMapCd'
			,BASYO.BasyoNm AS 'BasyoNm'
			,BASYO.RyakuNm AS 'VpmBasyoRyakuNm'
			,KEN.CodeKbn AS 'CodeKbn'
			,KEN.CodeKbnSeq AS 'CodeKbnSeq'
	FROM VPM_Basyo AS BASYO
	INNER JOIN VPM_CodeKb AS KEN ON KEN.CodeKbnSeq = BASYO.BasyoKenCdSeq
			AND KEN.CodeSyu = 'KENCD'
			AND KEN.SiyoKbn = 1
			AND KEN.TenantCdSeq = (
					SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
					FROM VPM_CodeSy
					WHERE CodeSyu = 'KENCD'
							AND KanriKbn <> 1
					)
	WHERE BASYO.SiyoKbn = 1
			AND BASYO.SiyoIkiKbn = 1 --１：行き先使用      
			AND BASYO.TenantCdSeq = @TenantCdSeq
	ORDER BY BASYO.BasyoKenCdSeq ASC
			,BASYO.BasyoMapCdSeq ASC
END
GO
