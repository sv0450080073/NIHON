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
-- SP-ID		:   PK_dPreDayEndTimes_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get PreDayEndTime List
-- Date			:   2020/12/18
-- Author		:   P.M.Nhat
-- Description	:   Get PreDayEndTime list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dPreDayEndTimes_R 
	-- Add the parameters for the stored procedure here
	@PreviousYmd varchar(8),
	@UnkYmd varchar(8),
	@CompanyCdSeq int
AS
BEGIN
	SELECT				
    		eVPM_KyoSHe.SyainCdSeq AS SyainCdSeq --社員コードSEQ				
    		,ISNULL(eTKD_Koban.TaiknTime, '') AS ZenjituTaiknTime --前日終了時間				
	FROM				
    		VPM_KyoSHe AS eVPM_KyoSHe				
    		LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_KyoSHe.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq				
    		LEFT JOIN (				
        		SELECT				
            		SyainCdSeq AS SyainCdSeq,				
            		MAX(TaiknTime) AS TaiknTime				
        		FROM				
            		TKD_Koban				
        		WHERE				
            		UnkYmd = @PreviousYmd				
            		AND KouZokPtnKbn <> 8 --休日				
        		GROUP BY				
            		SyainCdSeq				
    		) AS eTKD_Koban ON eTKD_Koban.SyainCdSeq = eVPM_KyoSHe.SyainCdSeq				
	WHERE				
    		eVPM_Eigyos.CompanyCdSeq = @CompanyCdSeq				
    		AND @UnkYmd BETWEEN eVPM_KyoSHe.StaYmd				
    		AND eVPM_KyoSHe.EndYmd				
END
GO
