USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dAcquisitionOfVacationRequestInformation_R]    Script Date: 5/10/2021 1:15:47 PM ******/
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
-- Description	:   Get alert setting by code base on sheet excel 3.4
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dAcquisitionOfVacationRequestInformation_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@SyainCdSeq				INT,
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT COUNT(*) As CountNumber
FROM TKD_SchYotei
INNER JOIN VPM_CodeKb ON TKD_SchYotei.YoteiType = VPM_CodeKb.CodeKbnSeq
        AND VPM_CodeKb.CodeSyu = 'YOTEITYPE'
        AND VPM_CodeKb.CodeKbn = 1
        AND VPM_CodeKb.SiyoKbn = 1
        AND VPM_CodeKb.TenantCdSeq = (
                SELECT CASE WHEN COUNT(*) = 0 THEN 0 ELSE @TenantCdSeq END AS TenantCdSeq
                FROM VPM_CodeKb
                WHERE VPM_CodeKb.CodeSyu = 'YOTEITYPE'
                        AND VPM_CodeKb.SiyoKbn = 1
                        AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq
                )
WHERE SyainCdSeq = @SyainCdSeq
        AND YoteiShoKbn = 1 -- 1:承認待ち 2:承認　3:却下


SET	@ROWCOUNT	=	@@ROWCOUNT
END
