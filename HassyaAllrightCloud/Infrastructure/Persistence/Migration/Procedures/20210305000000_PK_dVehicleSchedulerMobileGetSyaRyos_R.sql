USE [HOC_Kashikiri]
GO
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
-- SP-ID		:   PK_dVehicleSchedulerMobileGetSyaRyos_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get SyaRyo List
-- Date			:   2021/02/18
-- Author		:   P.M.Nhat
-- Description	:   Get SyaRyo list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dVehicleSchedulerMobileGetSyaRyos_R 
	@EigyoCdSeq int
AS
BEGIN
	SELECT 
		DISTINCT VPM_SyaRyo.SyaRyoCdSeq,
		VPM_SyaRyo.SyaRyoNm
	FROM VPM_HenSya
	LEFT JOIN VPM_SyaRyo
		ON VPM_HenSya.SyaRyoCdSeq = VPM_SyaRyo.SyaRyoCdSeq
	WHERE VPM_HenSya.EigyoCdSeq = @EigyoCdSeq
END
GO
