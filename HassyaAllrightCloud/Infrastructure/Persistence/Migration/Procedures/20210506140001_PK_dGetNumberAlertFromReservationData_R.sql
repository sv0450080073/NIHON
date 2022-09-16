USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetNumberAlertFromReservationData_R]    Script Date: 5/6/2021 9:37:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetAlertSettingByCode
-- Date			:   2020/12/22
-- Author		:   T.L.DUY
-- Description	:   Get alert setting by code base on sheet excel 3.3
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dGetNumberAlertFromReservationData_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@SeiTaiYmd				CHAR(8),
		@AlertNumber			INT,
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT COUNT(*) As CountNumber
FROM TKD_Yyksho AS eTKD_Yyksho
LEFT JOIN TKD_Unkobi AS eTKD_Unkobi ON eTKD_Yyksho.UkeNo = eTKD_Unkobi.UkeNo
        AND eTKD_Unkobi.SiyoKbn = 1
WHERE eTKD_Yyksho.TenantCdSeq = @TenantCdSeq
        AND eTKD_Yyksho.SeiTaiYmd <= @SeiTaiYmd
        AND ((@AlertNumber = 1002 AND eTKD_Unkobi.KSKbn = 1)
		OR (@AlertNumber = 1001 AND ISNULL(eTKD_Yyksho.KaktYmd, '') = '')
		OR (@AlertNumber = 2001 AND eTKD_Unkobi.HaiSKbn = 1)
		OR (@AlertNumber = 2002 AND eTKD_Unkobi.HaiIKbn = 1)
		OR (@AlertNumber = 2006 AND eTKD_Unkobi.NippoKbn = 1))

SET	@ROWCOUNT	=	@@ROWCOUNT
END
