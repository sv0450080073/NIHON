/*
   2020年12月28日14:43:04
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
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__BikoN__17A35695
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__18977ACE
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__198B9F07
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__1A7FC340
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__1B73E779
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__1C680BB2
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__1D5C2FEB
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__1E505424
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__1F44785D
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__20389C96
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__212CC0CF
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__2220E508
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__23150941
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__24092D7A
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__24FD51B3
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__25F175EC
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__26E59A25
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__27D9BE5E
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__28CDE297
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__29C206D0
GO
ALTER TABLE dbo.TKD_Haisha
	DROP CONSTRAINT DF__TKD_Haish__Custo__2AB62B09
GO
CREATE TABLE dbo.Tmp_TKD_Haisha
	(
	UkeNo nchar(15) NOT NULL,
	UnkRen smallint NOT NULL,
	SyaSyuRen smallint NOT NULL,
	TeiDanNo smallint NOT NULL,
	BunkRen smallint NOT NULL,
	HenKai smallint NOT NULL,
	GoSya char(5) NOT NULL,
	GoSyaJyn smallint NOT NULL,
	BunKSyuJyn smallint NOT NULL,
	SyuEigCdSeq int NOT NULL,
	KikEigSeq int NOT NULL,
	HaiSSryCdSeq int NOT NULL,
	KSSyaRSeq int NOT NULL,
	DanTaNm2 nvarchar(100) NOT NULL,
	IkMapCdSeq int NOT NULL,
	IkNm nvarchar(50) NOT NULL,
	SyuKoYmd char(8) NOT NULL,
	SyuKoTime char(4) NOT NULL,
	SyuPaTime char(4) NOT NULL,
	HaiSYmd char(8) NOT NULL,
	HaiSTime char(4) NOT NULL,
	HaiSCdSeq int NOT NULL,
	HaiSNm nvarchar(50) NOT NULL,
	HaiSJyus1 nvarchar(30) NOT NULL,
	HaiSJyus2 nvarchar(30) NOT NULL,
	HaiSKigou char(6) NOT NULL,
	HaiSKouKCdSeq int NOT NULL,
	HaiSKouKNm nchar(20) NULL,
	HaiSBinCdSeq int NOT NULL,
	HaiSBinNm nchar(20) NULL,
	HaiSSetTime char(4) NOT NULL,
	KikYmd char(8) NOT NULL,
	KikTime char(4) NOT NULL,
	TouYmd char(8) NOT NULL,
	TouChTime char(4) NOT NULL,
	TouCdSeq int NOT NULL,
	TouNm nvarchar(50) NOT NULL,
	TouJyusyo1 nvarchar(30) NOT NULL,
	TouJyusyo2 nvarchar(30) NOT NULL,
	TouKigou char(6) NOT NULL,
	TouKouKCdSeq int NOT NULL,
	TouSKouKNm nchar(20) NULL,
	TouBinCdSeq int NOT NULL,
	TouBinNm nchar(20) NULL,
	TouSBinNm nchar(20) NULL,
	TouSetTime char(4) NOT NULL,
	JyoSyaJin smallint NOT NULL,
	PlusJin smallint NOT NULL,
	DrvJin smallint NOT NULL,
	GuiSu smallint NOT NULL,
	GuideSiteiEigyoCdSeq int NULL,
	SyukinTime char(4) NULL,
	OthJinKbn1 tinyint NOT NULL,
	OthJin1 smallint NOT NULL,
	OthJinKbn2 tinyint NOT NULL,
	OthJin2 smallint NOT NULL,
	KSKbn tinyint NOT NULL,
	KHinKbn tinyint NOT NULL,
	HaiSKbn tinyint NOT NULL,
	HaiIKbn tinyint NOT NULL,
	GuiWNin smallint NOT NULL,
	NippoKbn tinyint NOT NULL,
	YouTblSeq int NOT NULL,
	YouKataKbn tinyint NOT NULL,
	SyaRyoUnc int NOT NULL,
	SyaRyoSyo int NOT NULL,
	SyaRyoTes int NOT NULL,
	YoushaUnc int NOT NULL,
	YoushaSyo int NOT NULL,
	YoushaTes int NOT NULL,
	PlatNo char(20) NOT NULL,
	UkeJyKbnCd tinyint NOT NULL,
	SijJoKbn1 tinyint NOT NULL,
	SijJoKbn2 tinyint NOT NULL,
	SijJoKbn3 tinyint NOT NULL,
	SijJoKbn4 tinyint NOT NULL,
	SijJoKbn5 tinyint NOT NULL,
	RotCdSeq int NOT NULL,
	BikoTblSeq int NOT NULL,
	HaiCom nvarchar(100) NOT NULL,
	BikoNm nvarchar(250) NOT NULL,
	CustomItems1 nvarchar(100) NOT NULL,
	CustomItems2 nvarchar(100) NOT NULL,
	CustomItems3 nvarchar(100) NOT NULL,
	CustomItems4 nvarchar(100) NOT NULL,
	CustomItems5 nvarchar(100) NOT NULL,
	CustomItems6 nvarchar(100) NOT NULL,
	CustomItems7 nvarchar(100) NOT NULL,
	CustomItems8 nvarchar(100) NOT NULL,
	CustomItems9 nvarchar(100) NOT NULL,
	CustomItems10 nvarchar(100) NOT NULL,
	CustomItems11 nvarchar(100) NOT NULL,
	CustomItems12 nvarchar(100) NOT NULL,
	CustomItems13 nvarchar(100) NOT NULL,
	CustomItems14 nvarchar(100) NOT NULL,
	CustomItems15 nvarchar(100) NOT NULL,
	CustomItems16 nvarchar(100) NOT NULL,
	CustomItems17 nvarchar(100) NOT NULL,
	CustomItems18 nvarchar(100) NOT NULL,
	CustomItems19 nvarchar(100) NOT NULL,
	CustomItems20 nvarchar(100) NOT NULL,
	SiyoKbn tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TKD_Haisha SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__BikoN__17A35695 DEFAULT ('') FOR BikoNm
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__18977ACE DEFAULT ('') FOR CustomItems1
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__198B9F07 DEFAULT ('') FOR CustomItems2
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__1A7FC340 DEFAULT ('') FOR CustomItems3
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__1B73E779 DEFAULT ('') FOR CustomItems4
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__1C680BB2 DEFAULT ('') FOR CustomItems5
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__1D5C2FEB DEFAULT ('') FOR CustomItems6
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__1E505424 DEFAULT ('') FOR CustomItems7
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__1F44785D DEFAULT ('') FOR CustomItems8
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__20389C96 DEFAULT ('') FOR CustomItems9
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__212CC0CF DEFAULT ('') FOR CustomItems10
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__2220E508 DEFAULT ('') FOR CustomItems11
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__23150941 DEFAULT ('') FOR CustomItems12
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__24092D7A DEFAULT ('') FOR CustomItems13
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__24FD51B3 DEFAULT ('') FOR CustomItems14
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__25F175EC DEFAULT ('') FOR CustomItems15
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__26E59A25 DEFAULT ('') FOR CustomItems16
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__27D9BE5E DEFAULT ('') FOR CustomItems17
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__28CDE297 DEFAULT ('') FOR CustomItems18
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__29C206D0 DEFAULT ('') FOR CustomItems19
GO
ALTER TABLE dbo.Tmp_TKD_Haisha ADD CONSTRAINT
	DF__TKD_Haish__Custo__2AB62B09 DEFAULT ('') FOR CustomItems20
GO
IF EXISTS(SELECT * FROM dbo.TKD_Haisha)
	 EXEC('INSERT INTO dbo.Tmp_TKD_Haisha (UkeNo, UnkRen, SyaSyuRen, TeiDanNo, BunkRen, HenKai, GoSya, GoSyaJyn, BunKSyuJyn, SyuEigCdSeq, KikEigSeq, HaiSSryCdSeq, KSSyaRSeq, DanTaNm2, IkMapCdSeq, IkNm, SyuKoYmd, SyuKoTime, SyuPaTime, HaiSYmd, HaiSTime, HaiSCdSeq, HaiSNm, HaiSJyus1, HaiSJyus2, HaiSKigou, HaiSKouKCdSeq, HaiSKouKNm, HaiSBinCdSeq, HaiSBinNm, HaiSSetTime, KikYmd, KikTime, TouYmd, TouChTime, TouCdSeq, TouNm, TouJyusyo1, TouJyusyo2, TouKigou, TouKouKCdSeq, TouSKouKNm, TouBinCdSeq, TouBinNm, TouSBinNm, TouSetTime, JyoSyaJin, PlusJin, DrvJin, GuiSu, GuideSiteiEigyoCdSeq, SyukinTime, OthJinKbn1, OthJin1, OthJinKbn2, OthJin2, KSKbn, KHinKbn, HaiSKbn, HaiIKbn, GuiWNin, NippoKbn, YouTblSeq, YouKataKbn, SyaRyoUnc, SyaRyoSyo, SyaRyoTes, YoushaUnc, YoushaSyo, YoushaTes, PlatNo, UkeJyKbnCd, SijJoKbn1, SijJoKbn2, SijJoKbn3, SijJoKbn4, SijJoKbn5, RotCdSeq, BikoTblSeq, HaiCom, BikoNm, CustomItems1, CustomItems2, CustomItems3, CustomItems4, CustomItems5, CustomItems6, CustomItems7, CustomItems8, CustomItems9, CustomItems10, CustomItems11, CustomItems12, CustomItems13, CustomItems14, CustomItems15, CustomItems16, CustomItems17, CustomItems18, CustomItems19, CustomItems20, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT UkeNo, UnkRen, SyaSyuRen, TeiDanNo, BunkRen, HenKai, GoSya, GoSyaJyn, BunKSyuJyn, SyuEigCdSeq, KikEigSeq, HaiSSryCdSeq, KSSyaRSeq, DanTaNm2, IkMapCdSeq, IkNm, SyuKoYmd, SyuKoTime, SyuPaTime, HaiSYmd, HaiSTime, HaiSCdSeq, HaiSNm, HaiSJyus1, HaiSJyus2, HaiSKigou, HaiSKouKCdSeq, HaiSKouKNm, HaiSBinCdSeq, HaiSBinNm, HaiSSetTime, KikYmd, KikTime, TouYmd, TouChTime, TouCdSeq, TouNm, TouJyusyo1, TouJyusyo2, TouKigou, TouKouKCdSeq, TouSKouKNm, TouBinCdSeq, TouBinNm, TouSBinNm, TouSetTime, JyoSyaJin, PlusJin, DrvJin, GuiSu, GuideSiteiEigyoCdSeq, SyukinTime, OthJinKbn1, OthJin1, OthJinKbn2, OthJin2, KSKbn, KHinKbn, HaiSKbn, HaiIKbn, GuiWNin, NippoKbn, YouTblSeq, YouKataKbn, SyaRyoUnc, SyaRyoSyo, SyaRyoTes, YoushaUnc, YoushaSyo, YoushaTes, PlatNo, UkeJyKbnCd, SijJoKbn1, SijJoKbn2, SijJoKbn3, SijJoKbn4, SijJoKbn5, RotCdSeq, BikoTblSeq, HaiCom, BikoNm, CustomItems1, CustomItems2, CustomItems3, CustomItems4, CustomItems5, CustomItems6, CustomItems7, CustomItems8, CustomItems9, CustomItems10, CustomItems11, CustomItems12, CustomItems13, CustomItems14, CustomItems15, CustomItems16, CustomItems17, CustomItems18, CustomItems19, CustomItems20, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_Haisha WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TKD_Haisha
GO
EXECUTE sp_rename N'dbo.Tmp_TKD_Haisha', N'TKD_Haisha', 'OBJECT' 
GO
ALTER TABLE dbo.TKD_Haisha ADD CONSTRAINT
	PK_TKD_Haisha PRIMARY KEY NONCLUSTERED 
	(
	UkeNo,
	UnkRen,
	TeiDanNo,
	BunkRen
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Haisha', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Haisha', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Haisha', 'Object', 'CONTROL') as Contr_Per 