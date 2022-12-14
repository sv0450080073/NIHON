/*
   Thursday, February 18, 20212:44:49 PM
   User: 
   Server: localhost
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
CREATE TABLE dbo.Tmp_TKD_Mishum
	(
	UkeNo nchar(15) NOT NULL,
	MisyuRen smallint NOT NULL,
	HenKai smallint NOT NULL,
	SeiFutSyu tinyint NOT NULL,
	UriGakKin int NOT NULL,
	SyaRyoSyo int NOT NULL,
	SyaRyoTes int NOT NULL,
	SeiKin int NOT NULL,
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
ALTER TABLE dbo.Tmp_TKD_Mishum SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.TKD_Mishum)
	 EXEC('INSERT INTO dbo.Tmp_TKD_Mishum (UkeNo, MisyuRen, HenKai, SeiFutSyu, UriGakKin, SyaRyoSyo, SyaRyoTes, SeiKin, NyuKinRui, CouKesRui, FutuUnkRen, FutTumRen, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT UkeNo, MisyuRen, HenKai, SeiFutSyu, CONVERT(int, UriGakKin), CONVERT(int, SyaRyoSyo), CONVERT(int, SyaRyoTes), CONVERT(int, SeiKin), NyuKinRui, CouKesRui, FutuUnkRen, FutTumRen, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_Mishum WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TKD_Mishum
GO
EXECUTE sp_rename N'dbo.Tmp_TKD_Mishum', N'TKD_Mishum', 'OBJECT' 
GO
ALTER TABLE dbo.TKD_Mishum ADD CONSTRAINT
	TKD_Mishum1 PRIMARY KEY CLUSTERED 
	(
	UkeNo,
	MisyuRen
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Mishum', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Mishum', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Mishum', 'Object', 'CONTROL') as Contr_Per 