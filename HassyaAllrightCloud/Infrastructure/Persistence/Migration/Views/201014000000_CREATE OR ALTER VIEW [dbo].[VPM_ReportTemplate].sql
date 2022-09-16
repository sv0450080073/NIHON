USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VPM_ReportTemplate] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_ReportTemplate]
	GO

