USE [HOC_Kashikiri]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TKD_InpCon]') AND type in (N'U'))
DROP TABLE [dbo].[TKD_InpCon]
GO

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
CREATE TABLE dbo.TKD_InpCon
	(
	SyainCdSeq int NOT NULL,
	FormNm varchar(20) NOT NULL,
	ItemNm varchar(50) NOT NULL,
	JoInput varchar(50) NOT NULL,
	NodeFlg tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.TKD_InpCon ADD CONSTRAINT
	PK_TKD_InpCon PRIMARY KEY CLUSTERED 
	(
	SyainCdSeq,
	FormNm,
	ItemNm
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.TKD_InpCon SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_InpCon', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_InpCon', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_InpCon', 'Object', 'CONTROL') as Contr_Per 