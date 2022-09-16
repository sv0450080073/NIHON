
USE [HOC_Kashikiri]
GO

declare @sql varchar(max) = 'CREATE OR ALTER VIEW [dbo].[VPM_Kinou] AS SELECT * FROM HOC_Master..TPM_Kinou;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_GpKanr] AS SELECT * FROM HOC_Master..TPM_GpKanr;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_KinouGpKanr] AS SELECT * FROM HOC_Master..TPM_KinouGpKanr;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_UserKe] AS SELECT * FROM HOC_Master..TPM_UserKe;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_LicenseService] AS SELECT * FROM HOC_Master..TPM_LicenseService;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_TenantLicense] AS SELECT * FROM HOC_Master..TPM_TenantLicense;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_Group] AS SELECT * FROM HOC_Master..TPM_Group;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_Menu] AS SELECT * FROM HOC_Master..TPM_Menu;'
EXEC(@sql)
SET @sql = 'CREATE OR ALTER VIEW [dbo].[VPM_MenuResource] AS SELECT * FROM HOC_Master..TPM_MenuResource;'
EXEC(@sql)