USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: AttendanceReport
-- View-ID				: [VPM_HenSyaSub]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/11/02
-- Description			: Get VPM_HenSyaSub from HOC_Master
-- =============================================

CREATE OR ALTER VIEW [dbo].[VPM_HenSyaSub] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_HenSyaSub]
	GO



