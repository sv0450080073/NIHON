USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dHenSya_R]]    Script Date: 2020/09/03 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   TransportActualResult
-- SP-ID		:   [PK_dHenSya_R]
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetHenSya
-- Date			:   2021/03/19
-- Author		:   Tra Nguyen 
-- Description	:   Get hensya for Transport Actual Result report
------------------------------------------------------------
CREATE OR ALTER   PROCEDURE [dbo].[PK_dHenSya_R]
	(
	-- Parameter
		@SyoriYmd				char(8),						
		@CompnyCd				int,
		@StrEigyoCd				int,
		@EndEigyoCd				int,
		@TenantCdSeq			int,			-- ログインしたユーザーのTenantCdSeq
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS
BEGIN
	SELECT COUNT(VPM_HenSya.SyaRyoCdSeq) AS JigyoCarSumCnt
FROM VPM_HenSya
LEFT JOIN VPM_Eigyos ON VPM_HenSya.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
        AND VPM_Eigyos.SiyoKbn = 1
LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
        AND VPM_Compny.SiyoKbn = 1
LEFT JOIN VPM_Syaryo ON VPM_HenSya.SyaryoCdSeq = VPM_Syaryo.SyaryoCdSeq
WHERE VPM_HenSya.StaYmd <= @SyoriYmd
        AND VPM_HenSya.EndYmd >= @SyoriYmd
        AND VPM_Syaryo.NinkaKbn = 1
        AND (@StrEigyoCd IS NULL OR VPM_Eigyos.EigyoCd >= @StrEigyoCd)
        AND (@EndEigyoCd IS NULL OR VPM_Eigyos.EigyoCd <= @EndEigyoCd)
        AND VPM_Compny.TenantCdSeq = @TenantCdSeq

SET @ROWCOUNT = @@ROWCOUNT
END
