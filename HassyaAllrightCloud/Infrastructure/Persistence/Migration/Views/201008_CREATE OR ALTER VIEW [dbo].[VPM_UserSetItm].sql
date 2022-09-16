USE [HOC_Notification]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- 個人設定項目マスタテーブル
CREATE OR ALTER VIEW [dbo].[VPM_UserSetItm] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_UserSetItm]
	GO

-- 個人設定値マスタテーブル
CREATE OR ALTER VIEW [dbo].[VPM_UserSetItmVal] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_UserSetItmVal]
	GO

-- 社員マスタテーブル
CREATE OR ALTER VIEW [dbo].[VPM_Syain] AS
	SELECT *
	FROM [HOC_Master].[dbo].[TPM_Syain]
	GO
