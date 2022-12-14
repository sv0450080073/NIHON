/*
   2020年11月23日16:13:25
   User: usr_devhk
   Server: kobo-db-inst05.cgtphzvll9lw.ap-northeast-1.rds.amazonaws.com
   Database: HOC_Kashikiri
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
ALTER TABLE dbo.TKD_Unkobi ADD
	UnitPriceIndex numeric(3, 1) NULL
GO
ALTER TABLE dbo.TKD_Unkobi SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Unkobi', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Unkobi', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Unkobi', 'Object', 'CONTROL') as Contr_Per 