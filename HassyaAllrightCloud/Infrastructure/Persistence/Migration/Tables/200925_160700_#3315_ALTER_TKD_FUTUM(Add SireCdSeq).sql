/*
   Friday, September 25, 20204:05:32 PM
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
ALTER TABLE dbo.TKD_FutTum
	DROP CONSTRAINT DF_TKD_FutTum_SirSitenCdSeq
GO
ALTER TABLE dbo.TKD_FutTum
	DROP CONSTRAINT DF_TKD_FutTum_SirTanKa
GO
ALTER TABLE dbo.TKD_FutTum
	DROP CONSTRAINT DF_TKD_FutTum_SirSuryo
GO
ALTER TABLE dbo.TKD_FutTum
	DROP CONSTRAINT DF_TKD_FutTum_SirGakKin
GO
ALTER TABLE dbo.TKD_FutTum
	DROP CONSTRAINT DF_TKD_FutTum_SirZeiKbn
GO
ALTER TABLE dbo.TKD_FutTum
	DROP CONSTRAINT DF_TKD_FutTum_SirZeiritsu
GO
ALTER TABLE dbo.TKD_FutTum
	DROP CONSTRAINT DF_TKD_FutTum_SirSyaRyoSyo
GO
CREATE TABLE dbo.Tmp_TKD_FutTum
	(
	UkeNo nchar(15) NOT NULL,
	UnkRen smallint NOT NULL,
	FutTumKbn tinyint NOT NULL,
	FutTumRen smallint NOT NULL,
	HenKai smallint NOT NULL,
	Nittei smallint NOT NULL,
	TomKbn tinyint NOT NULL,
	FutTumCdSeq int NOT NULL,
	FutTumNm nvarchar(50) NOT NULL,
	HasYmd char(8) NOT NULL,
	IriRyoChiCd tinyint NOT NULL,
	IriRyoCd char(3) NOT NULL,
	IriRyoNm nvarchar(30) NOT NULL,
	DeRyoChiCd tinyint NOT NULL,
	DeRyoCd char(3) NOT NULL,
	DeRyoNm nvarchar(30) NOT NULL,
	SeisanCdSeq int NOT NULL,
	SeisanNm nvarchar(50) NOT NULL,
	SeisanKbn tinyint NOT NULL,
	TanKa int NOT NULL,
	Suryo smallint NOT NULL,
	UriGakKin int NOT NULL,
	ZeiKbn tinyint NOT NULL,
	Zeiritsu numeric(3, 1) NOT NULL,
	SyaRyoSyo int NOT NULL,
	TesuRitu numeric(3, 1) NOT NULL,
	SyaRyoTes int NOT NULL,
	NyuKinKbn tinyint NOT NULL,
	NCouKbn tinyint NOT NULL,
	BikoNm nvarchar(50) NOT NULL,
	ExpItem nvarchar(255) NOT NULL,
	SortJun smallint NOT NULL,
	SirSitenCdSeq int NOT NULL,
	SirTanKa int NOT NULL,
	SirSuryo smallint NOT NULL,
	SirGakKin int NOT NULL,
	SirZeiKbn tinyint NOT NULL,
	SirZeiritsu numeric(3, 1) NOT NULL,
	SirSyaRyoSyo int NOT NULL,
	SiyoKbn tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL,
	SireCdSeq int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TKD_FutTum SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_TKD_FutTum ADD CONSTRAINT
	DF_TKD_FutTum_SirSitenCdSeq DEFAULT ((0)) FOR SirSitenCdSeq
GO
ALTER TABLE dbo.Tmp_TKD_FutTum ADD CONSTRAINT
	DF_TKD_FutTum_SirTanKa DEFAULT ((0)) FOR SirTanKa
GO
ALTER TABLE dbo.Tmp_TKD_FutTum ADD CONSTRAINT
	DF_TKD_FutTum_SirSuryo DEFAULT ((0)) FOR SirSuryo
GO
ALTER TABLE dbo.Tmp_TKD_FutTum ADD CONSTRAINT
	DF_TKD_FutTum_SirGakKin DEFAULT ((0)) FOR SirGakKin
GO
ALTER TABLE dbo.Tmp_TKD_FutTum ADD CONSTRAINT
	DF_TKD_FutTum_SirZeiKbn DEFAULT ((0)) FOR SirZeiKbn
GO
ALTER TABLE dbo.Tmp_TKD_FutTum ADD CONSTRAINT
	DF_TKD_FutTum_SirZeiritsu DEFAULT ((0)) FOR SirZeiritsu
GO
ALTER TABLE dbo.Tmp_TKD_FutTum ADD CONSTRAINT
	DF_TKD_FutTum_SirSyaRyoSyo DEFAULT ((0)) FOR SirSyaRyoSyo
GO
IF EXISTS(SELECT * FROM dbo.TKD_FutTum)
	 EXEC('INSERT INTO dbo.Tmp_TKD_FutTum (UkeNo, UnkRen, FutTumKbn, FutTumRen, HenKai, Nittei, TomKbn, FutTumCdSeq, FutTumNm, HasYmd, IriRyoChiCd, IriRyoCd, IriRyoNm, DeRyoChiCd, DeRyoCd, DeRyoNm, SeisanCdSeq, SeisanNm, SeisanKbn, TanKa, Suryo, UriGakKin, ZeiKbn, Zeiritsu, SyaRyoSyo, TesuRitu, SyaRyoTes, NyuKinKbn, NCouKbn, BikoNm, ExpItem, SortJun, SirSitenCdSeq, SirTanKa, SirSuryo, SirGakKin, SirZeiKbn, SirZeiritsu, SirSyaRyoSyo, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT UkeNo, UnkRen, FutTumKbn, FutTumRen, HenKai, Nittei, TomKbn, FutTumCdSeq, FutTumNm, HasYmd, IriRyoChiCd, IriRyoCd, IriRyoNm, DeRyoChiCd, DeRyoCd, DeRyoNm, SeisanCdSeq, SeisanNm, SeisanKbn, TanKa, Suryo, UriGakKin, ZeiKbn, Zeiritsu, SyaRyoSyo, TesuRitu, SyaRyoTes, NyuKinKbn, NCouKbn, BikoNm, ExpItem, SortJun, SirSitenCdSeq, SirTanKa, SirSuryo, SirGakKin, SirZeiKbn, SirZeiritsu, SirSyaRyoSyo, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_FutTum WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TKD_FutTum
GO
EXECUTE sp_rename N'dbo.Tmp_TKD_FutTum', N'TKD_FutTum', 'OBJECT' 
GO
ALTER TABLE dbo.TKD_FutTum ADD CONSTRAINT
	TKD_FutTum2 PRIMARY KEY NONCLUSTERED 
	(
	UkeNo,
	UnkRen,
	FutTumKbn,
	FutTumRen
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_FutTum', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_FutTum', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_FutTum', 'Object', 'CONTROL') as Contr_Per 