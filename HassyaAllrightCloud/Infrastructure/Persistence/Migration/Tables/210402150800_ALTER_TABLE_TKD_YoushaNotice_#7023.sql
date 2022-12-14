/*
   2021年4月2日15:00:18
   User: 
   Server: CPU000730
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
CREATE TABLE dbo.Tmp_TKD_YoushaNotice
	(
	MotoTenantCdSeq int NOT NULL,
	MotoUkeNo nchar(15) NOT NULL,
	MotoUnkRen smallint NOT NULL,
	MotoYouTblSeq int NOT NULL,
	HaiSYmd char(8) NULL,
	HaiSTime char(4) NULL,
	TouYmd char(8) NULL,
	TouChTime char(4) NULL,
	BigtypeNum smallint NULL,
	MediumtypeNum smallint NULL,
	SmalltypeNum smallint NULL,
	DanTaNm nvarchar(100) NULL,
	UkeTenantCdSeq int NULL,
	MotoTokuiSeq int NULL,
	MotoSitenCdSeq int NULL,
	UnReadKbn tinyint NULL,
	RegiterKbn tinyint NULL,
	SiyoKbn tinyint NULL,
	UpdYmd char(8) NULL,
	UpdTime char(6) NULL,
	UpdSyainCd int NULL,
	UpdPrgID char(10) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TKD_YoushaNotice SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.TKD_YoushaNotice)
	 EXEC('INSERT INTO dbo.Tmp_TKD_YoushaNotice (MotoTenantCdSeq, MotoUkeNo, MotoUnkRen, MotoYouTblSeq, HaiSYmd, BigtypeNum, MediumtypeNum, SmalltypeNum, DanTaNm, UkeTenantCdSeq, MotoTokuiSeq, MotoSitenCdSeq, UnReadKbn, RegiterKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT MotoTenantCdSeq, MotoUkeNo, MotoUnkRen, MotoYouTblSeq, HasYmd, BigtypeNum, MediumtypeNum, SmalltypeNum, DanTaNm, UkeTenantCdSeq, MotoTokuiSeq, MotoSitenCdSeq, UnReadKbn, RegiterKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_YoushaNotice WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TKD_YoushaNotice
GO
EXECUTE sp_rename N'dbo.Tmp_TKD_YoushaNotice', N'TKD_YoushaNotice', 'OBJECT' 
GO
ALTER TABLE dbo.TKD_YoushaNotice ADD CONSTRAINT
	PK_TKD_YoushaNotice PRIMARY KEY CLUSTERED 
	(
	MotoTenantCdSeq,
	MotoUkeNo,
	MotoUnkRen,
	MotoYouTblSeq
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_YoushaNotice', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_YoushaNotice', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_YoushaNotice', 'Object', 'CONTROL') as Contr_Per 