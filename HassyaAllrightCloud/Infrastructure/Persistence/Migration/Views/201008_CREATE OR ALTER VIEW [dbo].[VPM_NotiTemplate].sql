USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- 通知テンプレートテーブル
CREATE OR ALTER VIEW [dbo].[VPM_NotiTemplate] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_NotiTemplate]
	GO

-- 通知テンプレート変数テーブル
CREATE OR ALTER VIEW [dbo].[VPM_NotiTemplateVar] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_NotiTemplateVar]
	GO

