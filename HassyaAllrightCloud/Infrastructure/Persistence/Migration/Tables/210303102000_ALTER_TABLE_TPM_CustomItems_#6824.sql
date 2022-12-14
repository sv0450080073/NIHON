USE [HOC_Master]
GO
/*
   2021年3月3日10:13:59
   User: 
   Server: CPU000730
   Database: HOC_Master
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.TPM_CustomItems.CostomName', N'Tmp_CustomName', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.TPM_CustomItems.Tmp_CustomName', N'CustomName', 'COLUMN' 
GO
ALTER TABLE dbo.TPM_CustomItems SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TPM_CustomItems', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TPM_CustomItems', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TPM_CustomItems', 'Object', 'CONTROL') as Contr_Per 