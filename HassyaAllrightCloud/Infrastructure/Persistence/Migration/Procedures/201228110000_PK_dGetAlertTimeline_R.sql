USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetAlertSettingByCode32_R]    Script Date: 1/7/2021 3:08:32 PM ******/
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
-- Description	:   Get alert setting by code base on sheet excel 3.2
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dGetAlertTimeline_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@SyainCdSeq				INT,
		@AlertCd				INT,
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT MAIN.TenantCdSeq
        ,MAIN.AlertCdSeq
        ,MAIN.AlertKbn
        ,MAIN.AlertCd
        ,MAIN.AlertNm
        ,MAIN.DefaultVal
        ,MAIN.DefaultTimeline
        ,MAIN.DefaultZengo
        ,MAIN.DefaultDisplayKbn
        ,CUR.TenantCdSeq AS CurTenantCdSeq
        ,CUR.DefaultVal AS CurVal
        ,CUR.DefaultTimeline AS CurTimeline
        ,CUR.DefaultZengo AS CurZengo
        ,CUR.DefaultDisplayKbn AS CurDisplayKbn
        ,UAS.SyainCdSeq AS SyainCdSeq
        ,UAS.DisplayKbn AS UserDisplayKbn
FROM VPM_Alert MAIN
LEFT JOIN VPM_Alert CUR ON MAIN.AlertCd = CUR.AlertCd
        AND CUR.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_AlertSet UAS ON MAIN.AlertCd = UAS.AlertCd
        AND UAS.SyainCdSeq = @SyainCdSeq
WHERE MAIN.TenantCdSeq = 0
        AND MAIN.AlertCd = @AlertCd
        AND MAIN.SiyoKbn = 1

SET	@ROWCOUNT	=	@@ROWCOUNT
END
