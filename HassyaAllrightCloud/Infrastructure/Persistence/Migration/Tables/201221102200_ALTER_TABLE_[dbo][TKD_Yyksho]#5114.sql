/*
   2020年12月21日10:19:04
   User: sa
   Server: CPU000724\SQLEXPRESS
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
ALTER TABLE dbo.TKD_Yyksho ADD
	BikoNm nvarchar(250) NULL
GO
ALTER TABLE dbo.TKD_Yyksho SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Yyksho', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Yyksho', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Yyksho', 'Object', 'CONTROL') as Contr_Per 