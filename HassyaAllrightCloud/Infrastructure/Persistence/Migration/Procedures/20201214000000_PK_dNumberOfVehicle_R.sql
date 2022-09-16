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
-- SP-ID		:   PK_dNumberOfVehicles_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get NumberOfVehicle List
-- Date			:   2020/12/14
-- Author		:   P.M.Nhat
-- Description	:   Get NumberOfVehicle list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dNumberOfVehicle_R
	-- Add the parameters for the stored procedure here
	@CompanyCdSeq int,
	@UnkYmd varchar(8)
AS
BEGIN
	SELECT
    		VPM_Eigyos.EigyoCdSeq AS EigyoCdSeq, --営業所コードSEQ
    		COUNT(VPM_SyaRyo.SyaRyoCdSeq) AS SyaRyoNum --車輌台数
	FROM
    		VPM_SyaRyo
    		LEFT JOIN VPM_HenSya ON VPM_HenSya.SyaRyoCdSeq = VPM_SyaRyo.SyaRyoCdSeq
    		LEFT JOIN VPM_Eigyos ON VPM_Eigyos.EigyoCdSeq = VPM_HenSya.EigyoCdSeq
	WHERE
    		@UnkYmd BETWEEN VPM_HenSya.StaYmd
    		AND VPM_HenSya.EndYmd
    		AND VPM_Eigyos.CompanyCdSeq = @CompanyCdSeq 
	GROUP BY
    		VPM_Eigyos.EigyoCdSeq
END
GO
