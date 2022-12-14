/*
   2020年12月14日13:30:06
   User: 
   Server: CPU000730
   Database: HOC_Kashikiri_20201012
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
CREATE TABLE dbo.Tmp_TKD_Koteik
	(
	UkeNo nchar(15) NOT NULL,
	UnkRen smallint NOT NULL,
	TeiDanNo smallint NOT NULL,
	BunkRen smallint NOT NULL,
	TomKbn tinyint NOT NULL,
	Nittei smallint NOT NULL,
	HenKai smallint NOT NULL,
	TeiDanNittei smallint NOT NULL,
	TeiDanTomKbn tinyint NOT NULL,
	SyuEigCdSeq int NOT NULL,
	SyukoNm nvarchar(50) NOT NULL,
	SyukoTime char(4) NULL,
	HaiSTime char(4) NOT NULL,
	SyuPaCdSeq int NOT NULL,
	SyuPaNm nvarchar(50) NOT NULL,
	SyuPaTime char(4) NOT NULL,
	KeiyuMapCdSeq int NOT NULL,
	KeiyuNm nvarchar(50) NOT NULL,
	TouCdSeq int NOT NULL,
	TouNm nvarchar(50) NOT NULL,
	TouChTime char(4) NOT NULL,
	KikEigSeq int NOT NULL,
	KikoNm nvarchar(50) NOT NULL,
	KikTime char(4) NULL,
	SHakuMapCdSeq int NOT NULL,
	SHakuNm nvarchar(50) NOT NULL,
	TaikTime char(4) NOT NULL,
	KyuKMapCdSeq int NOT NULL,
	KyuKNm nvarchar(50) NOT NULL,
	KyuKTime char(4) NOT NULL,
	KyuKStaTime char(4) NOT NULL,
	KyuKEndTime char(4) NOT NULL,
	BikoNm nvarchar(50) NOT NULL,
	JisaIPKm numeric(7, 2) NOT NULL,
	JisaKSKm numeric(7, 2) NOT NULL,
	KisoIPkm numeric(7, 2) NOT NULL,
	KisoKOKm numeric(7, 2) NOT NULL,
	SiyoKbn tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TKD_Koteik SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = N'受付番号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'UkeNo'
GO
DECLARE @v sql_variant 
SET @v = N'運行日連番'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'UnkRen'
GO
DECLARE @v sql_variant 
SET @v = N'悌団番号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TeiDanNo'
GO
DECLARE @v sql_variant 
SET @v = N'分割連番'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'BunkRen'
GO
DECLARE @v sql_variant 
SET @v = N'泊区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TomKbn'
GO
DECLARE @v sql_variant 
SET @v = N'日程'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'Nittei'
GO
DECLARE @v sql_variant 
SET @v = N'変更回数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'HenKai'
GO
DECLARE @v sql_variant 
SET @v = N'悌団日程'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TeiDanNittei'
GO
DECLARE @v sql_variant 
SET @v = N'悌団泊区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TeiDanTomKbn'
GO
DECLARE @v sql_variant 
SET @v = N'出庫営業所コードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SyuEigCdSeq'
GO
DECLARE @v sql_variant 
SET @v = N'出庫地名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SyukoNm'
GO
DECLARE @v sql_variant 
SET @v = N'配車時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'HaiSTime'
GO
DECLARE @v sql_variant 
SET @v = N'出発地コードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SyuPaCdSeq'
GO
DECLARE @v sql_variant 
SET @v = N'出発地名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SyuPaNm'
GO
DECLARE @v sql_variant 
SET @v = N'出発時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SyuPaTime'
GO
DECLARE @v sql_variant 
SET @v = N'経由地マップコードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KeiyuMapCdSeq'
GO
DECLARE @v sql_variant 
SET @v = N'経由地名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KeiyuNm'
GO
DECLARE @v sql_variant 
SET @v = N'到着地コードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TouCdSeq'
GO
DECLARE @v sql_variant 
SET @v = N'到着地名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TouNm'
GO
DECLARE @v sql_variant 
SET @v = N'到着時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TouChTime'
GO
DECLARE @v sql_variant 
SET @v = N'帰庫営業所コードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KikEigSeq'
GO
DECLARE @v sql_variant 
SET @v = N'帰庫地名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KikoNm'
GO
DECLARE @v sql_variant 
SET @v = N'宿泊地マップコードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SHakuMapCdSeq'
GO
DECLARE @v sql_variant 
SET @v = N'宿泊地名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SHakuNm'
GO
DECLARE @v sql_variant 
SET @v = N'待機時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'TaikTime'
GO
DECLARE @v sql_variant 
SET @v = N'休憩地マップコードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KyuKMapCdSeq'
GO
DECLARE @v sql_variant 
SET @v = N'休憩地名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KyuKNm'
GO
DECLARE @v sql_variant 
SET @v = N'休憩時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KyuKTime'
GO
DECLARE @v sql_variant 
SET @v = N'休憩開始時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KyuKStaTime'
GO
DECLARE @v sql_variant 
SET @v = N'休憩終了時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KyuKEndTime'
GO
DECLARE @v sql_variant 
SET @v = N'備考'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'BikoNm'
GO
DECLARE @v sql_variant 
SET @v = N'実車一般キロ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'JisaIPKm'
GO
DECLARE @v sql_variant 
SET @v = N'実車高速キロ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'JisaKSKm'
GO
DECLARE @v sql_variant 
SET @v = N'回送一般キロ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KisoIPkm'
GO
DECLARE @v sql_variant 
SET @v = N'回送高速キロ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'KisoKOKm'
GO
DECLARE @v sql_variant 
SET @v = N'使用区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'SiyoKbn'
GO
DECLARE @v sql_variant 
SET @v = N'最終更新年月日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'UpdYmd'
GO
DECLARE @v sql_variant 
SET @v = N'最終更新時間'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'UpdTime'
GO
DECLARE @v sql_variant 
SET @v = N'最終更新社員コードＳＥＱ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'UpdSyainCd'
GO
DECLARE @v sql_variant 
SET @v = N'最終更新プログラムＩＤ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Koteik', N'COLUMN', N'UpdPrgID'
GO
IF EXISTS(SELECT * FROM dbo.TKD_Koteik)
	 EXEC('INSERT INTO dbo.Tmp_TKD_Koteik (UkeNo, UnkRen, TeiDanNo, BunkRen, TomKbn, Nittei, HenKai, TeiDanNittei, TeiDanTomKbn, SyuEigCdSeq, SyukoNm, HaiSTime, SyuPaCdSeq, SyuPaNm, SyuPaTime, KeiyuMapCdSeq, KeiyuNm, TouCdSeq, TouNm, TouChTime, KikEigSeq, KikoNm, SHakuMapCdSeq, SHakuNm, TaikTime, KyuKMapCdSeq, KyuKNm, KyuKTime, KyuKStaTime, KyuKEndTime, BikoNm, JisaIPKm, JisaKSKm, KisoIPkm, KisoKOKm, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT UkeNo, UnkRen, TeiDanNo, BunkRen, TomKbn, Nittei, HenKai, TeiDanNittei, TeiDanTomKbn, SyuEigCdSeq, SyukoNm, HaiSTime, SyuPaCdSeq, SyuPaNm, SyuPaTime, KeiyuMapCdSeq, KeiyuNm, TouCdSeq, TouNm, TouChTime, KikEigSeq, KikoNm, SHakuMapCdSeq, SHakuNm, TaikTime, KyuKMapCdSeq, KyuKNm, KyuKTime, KyuKStaTime, KyuKEndTime, BikoNm, JisaIPKm, JisaKSKm, KisoIPkm, KisoKOKm, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_Koteik WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TKD_Koteik
GO
EXECUTE sp_rename N'dbo.Tmp_TKD_Koteik', N'TKD_Koteik', 'OBJECT' 
GO
ALTER TABLE dbo.TKD_Koteik ADD CONSTRAINT
	PK_TKD_Koteik PRIMARY KEY CLUSTERED 
	(
	UkeNo,
	UnkRen,
	TeiDanNo,
	BunkRen,
	TomKbn,
	Nittei
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Koteik', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Koteik', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Koteik', 'Object', 'CONTROL') as Contr_Per 