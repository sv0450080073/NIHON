/*
   2020年11月24日8:40:49
   User: 
   Server: CPU000730
   Database: HOC_Master_20201012
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
CREATE TABLE dbo.Tmp_TPM_CodeKb
	(
	TenantCdSeq int NOT NULL,
	CodeKbnSeq int NOT NULL IDENTITY (1, 1),
	CodeSyu varchar(20) NOT NULL,
	CodeKbn varchar(10) NOT NULL,
	CodeKbnNm nvarchar(50) NOT NULL,
	RyakuNm nvarchar(50) NOT NULL,
	CodeSeiKbn varchar(10) NOT NULL,
	SiyoKbn tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TPM_CodeKb SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_TPM_CodeKb ON
GO
IF EXISTS(SELECT * FROM dbo.TPM_CodeKb)
	 EXEC('INSERT INTO dbo.Tmp_TPM_CodeKb (TenantCdSeq, CodeKbnSeq, CodeSyu, CodeKbn, CodeKbnNm, RyakuNm, CodeSeiKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT TenantCdSeq, CodeKbnSeq, CodeSyu, CodeKbn, CONVERT(nvarchar(50), CodeKbnNm), CONVERT(nvarchar(50), RyakuNm), CodeSeiKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TPM_CodeKb WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_TPM_CodeKb OFF
GO
DROP TABLE dbo.TPM_CodeKb
GO
EXECUTE sp_rename N'dbo.Tmp_TPM_CodeKb', N'TPM_CodeKb', 'OBJECT' 
GO
ALTER TABLE dbo.TPM_CodeKb ADD CONSTRAINT
	TPM_CodeKb1 PRIMARY KEY CLUSTERED 
	(
	CodeKbnSeq
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TPM_CodeKb', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TPM_CodeKb', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TPM_CodeKb', 'Object', 'CONTROL') as Contr_Per 