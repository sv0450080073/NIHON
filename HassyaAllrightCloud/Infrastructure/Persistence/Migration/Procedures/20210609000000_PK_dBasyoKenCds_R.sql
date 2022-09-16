USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBasyoKenCds_R]    Script Date: 6/9/2021 3:35:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dBasyoKenCds_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Basyo KenCd List
-- Date			:   2021/05/27
-- Author		:   P.M.Nhat
-- Description	:   Get Basyo KenCd list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dBasyoKenCds_R]
	-- Add the parameters for the stored procedure here
	@TenantCdSeq int
AS
BEGIN
	SELECT BASYO.BasyoKenCdSeq AS BasyoBasyoKenCdSeq
			,KEN.RyakuNm AS CodeKbCodeKbnNm
			,BASYO.BasyoMapCdSeq AS BasyoBasyoMapCdSeq
			,BASYO.BasyoMapCd AS BasyoBasyoMapCd
			,BASYO.BasyoNm AS BasyoBasyoNm
			,BASYO.RyakuNm AS BasyoRyakuNm
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
			AND BASYO.SiyoIkiKbn = 1 --・托ｼ夊｡後″蜈井ｽｿ逕ｨ      
			AND BASYO.TenantCdSeq = @TenantCdSeq
	ORDER BY BASYO.BasyoKenCdSeq ASC
			,BASYO.BasyoMapCdSeq ASC								
END
