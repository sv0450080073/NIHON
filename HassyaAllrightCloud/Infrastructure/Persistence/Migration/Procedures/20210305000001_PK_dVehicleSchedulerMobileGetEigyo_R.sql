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
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE PK_dVehicleSchedulerMobileGetEigyo_R
	@SyaRyoCdSeq int
AS
BEGIN
	SELECT 
		MAX(VPM_Eigyos.EigyoCdSeq) AS EigyoCdSeq,
		MAX(VPM_Eigyos.EigyoCd) AS EigyoCd,
		MAX(VPM_Eigyos.RyakuNm) AS EigyoName
	FROM VPM_HenSya
	LEFT JOIN VPM_Eigyos
		ON VPM_HenSya.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
	WHERE VPM_HenSya.SyaRyoCdSeq = @SyaRyoCdSeq
		AND VPM_Eigyos.SiyoKbn = 1
END
GO
