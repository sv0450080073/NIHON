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
-- SP-ID		:   PK_dWorkTimes_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get WorkTime List
-- Date			:   2020/12/18
-- Author		:   P.M.Nhat
-- Description	:   Get WorkTime list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dWorkTimes_R
	-- Add the parameters for the stored procedure here
	@StartYmd varchar(8),
	@UnkYmd varchar(8)
AS
BEGIN
	SELECT			
    		eVPM_KyoSHe.SyainCdSeq AS SyainCdSeq, --社員コードSEQ			
    		ISNULL(eTKD_Koban.SyukinTime, '') AS SyukinTime, --出勤時間			
    		ISNULL(eTKD_Koban.TaiknTime, '') AS TaiknTime, --退勤時間			
    		ISNULL(eTKD_Koban.KouSTime, '') AS KouSTime, --拘束時間	
		eTKD_Koban.UnkYmd AS UnkYmd
	FROM			
    		VPM_KyoSHe AS eVPM_KyoSHe			
    		LEFT JOIN TKD_Koban AS eTKD_Koban ON eVPM_KyoSHe.SyainCdSeq = eTKD_Koban.SyainCdSeq			
    		AND UnkYmd BETWEEN @StartYmd AND @UnkYmd			
	WHERE			
    		@UnkYmd BETWEEN eVPM_KyoSHe.StaYmd			
    		AND eVPM_KyoSHe.EndYmd			
END
GO
