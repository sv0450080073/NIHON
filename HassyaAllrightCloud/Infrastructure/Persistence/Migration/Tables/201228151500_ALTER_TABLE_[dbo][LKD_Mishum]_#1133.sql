/*
   2020年12月28日12:58:50
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
CREATE TABLE dbo.Tmp_LKD_Mishum
	(
	LogSeq int NOT NULL IDENTITY (1, 1),
	LogShKbn tinyint NOT NULL,
	HenKeyItm nvarchar(4000) NOT NULL,
	UkeNo nchar(15) NOT NULL,
	MisyuRen smallint NOT NULL,
	HenKai smallint NOT NULL,
	SeiFutSyu tinyint NOT NULL,
	UriGakKin bigint NOT NULL,
	SyaRyoSyo bigint NOT NULL,
	SyaRyoTes bigint NOT NULL,
	SeiKin bigint NOT NULL,
	NyuKinRui numeric(9, 0) NOT NULL,
	CouKesRui int NOT NULL,
	FutuUnkRen smallint NOT NULL,
	FutTumRen smallint NOT NULL,
	SiyoKbn tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_LKD_Mishum SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_LKD_Mishum ON
GO
IF EXISTS(SELECT * FROM dbo.LKD_Mishum)
	 EXEC('INSERT INTO dbo.Tmp_LKD_Mishum (LogSeq, LogShKbn, HenKeyItm, UkeNo, MisyuRen, HenKai, SeiFutSyu, UriGakKin, SyaRyoSyo, SyaRyoTes, SeiKin, NyuKinRui, CouKesRui, FutuUnkRen, FutTumRen, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT LogSeq, LogShKbn, HenKeyItm, UkeNo, MisyuRen, HenKai, SeiFutSyu, CONVERT(bigint, UriGakKin), CONVERT(bigint, SyaRyoSyo), CONVERT(bigint, SyaRyoTes), CONVERT(bigint, SeiKin), NyuKinRui, CouKesRui, FutuUnkRen, FutTumRen, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.LKD_Mishum WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_LKD_Mishum OFF
GO
DROP TABLE dbo.LKD_Mishum
GO
EXECUTE sp_rename N'dbo.Tmp_LKD_Mishum', N'LKD_Mishum', 'OBJECT' 
GO
ALTER TABLE dbo.LKD_Mishum ADD CONSTRAINT
	LKD_Mishum1 PRIMARY KEY CLUSTERED 
	(
	LogSeq
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.LKD_Mishum', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.LKD_Mishum', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.LKD_Mishum', 'Object', 'CONTROL') as Contr_Per 