USE [HOC_Kashikiri]
GO

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
CREATE TABLE dbo.Tmp_TKD_Haisha
	(
	UkeNo nchar(15) NOT NULL,
	UnkRen smallint NOT NULL,
	SyaSyuRen smallint NOT NULL,
	TeiDanNo smallint NOT NULL,
	BunkRen smallint NOT NULL,
	HenKai smallint NOT NULL,
	GoSya char(4) NOT NULL,
	GoSyaJyn smallint NOT NULL,
	BunKSyuJyn smallint NOT NULL,
	SyuEigCdSeq int NOT NULL,
	KikEigSeq int NOT NULL,
	HaiSSryCdSeq int NOT NULL,
	KSSyaRSeq int NOT NULL,
	DanTaNm2 varchar(100) NOT NULL,
	IkMapCdSeq int NOT NULL,
	IkNm varchar(50) NOT NULL,
	SyuKoYmd char(8) NOT NULL,
	SyuKoTime char(4) NOT NULL,
	SyuPaTime char(4) NOT NULL,
	HaiSYmd char(8) NOT NULL,
	HaiSTime char(4) NOT NULL,
	HaiSCdSeq int NOT NULL,
	HaiSNm varchar(50) NOT NULL,
	HaiSJyus1 varchar(30) NOT NULL,
	HaiSJyus2 varchar(30) NOT NULL,
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
	TouNm varchar(50) NOT NULL,
	TouJyusyo1 varchar(30) NOT NULL,
	TouJyusyo2 varchar(30) NOT NULL,
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
	HaiCom varchar(100) NOT NULL,
	SiyoKbn tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TKD_Haisha SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.TKD_Haisha)
	 EXEC('INSERT INTO dbo.Tmp_TKD_Haisha (UkeNo, UnkRen, SyaSyuRen, TeiDanNo, BunkRen, HenKai, GoSya, GoSyaJyn, BunKSyuJyn, SyuEigCdSeq, KikEigSeq, HaiSSryCdSeq, KSSyaRSeq, DanTaNm2, IkMapCdSeq, IkNm, SyuKoYmd, SyuKoTime, SyuPaTime, HaiSYmd, HaiSTime, HaiSCdSeq, HaiSNm, HaiSJyus1, HaiSJyus2, HaiSKigou, HaiSKouKCdSeq, HaiSBinCdSeq, HaiSSetTime, KikYmd, KikTime, TouYmd, TouChTime, TouCdSeq, TouNm, TouJyusyo1, TouJyusyo2, TouKigou, TouKouKCdSeq, TouBinCdSeq, TouSetTime, JyoSyaJin, PlusJin, DrvJin, GuiSu, OthJinKbn1, OthJin1, OthJinKbn2, OthJin2, KSKbn, KHinKbn, HaiSKbn, HaiIKbn, GuiWNin, NippoKbn, YouTblSeq, YouKataKbn, SyaRyoUnc, SyaRyoSyo, SyaRyoTes, YoushaUnc, YoushaSyo, YoushaTes, PlatNo, UkeJyKbnCd, SijJoKbn1, SijJoKbn2, SijJoKbn3, SijJoKbn4, SijJoKbn5, RotCdSeq, BikoTblSeq, HaiCom, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT UkeNo, UnkRen, SyaSyuRen, TeiDanNo, BunkRen, HenKai, GoSya, GoSyaJyn, BunKSyuJyn, SyuEigCdSeq, KikEigSeq, HaiSSryCdSeq, KSSyaRSeq, DanTaNm2, IkMapCdSeq, IkNm, SyuKoYmd, SyuKoTime, SyuPaTime, HaiSYmd, HaiSTime, HaiSCdSeq, HaiSNm, HaiSJyus1, HaiSJyus2, HaiSKigou, HaiSKouKCdSeq, HaiSBinCdSeq, HaiSSetTime, KikYmd, KikTime, TouYmd, TouChTime, TouCdSeq, TouNm, TouJyusyo1, TouJyusyo2, TouKigou, TouKouKCdSeq, TouBinCdSeq, TouSetTime, JyoSyaJin, PlusJin, DrvJin, GuiSu, OthJinKbn1, OthJin1, OthJinKbn2, OthJin2, KSKbn, KHinKbn, HaiSKbn, HaiIKbn, GuiWNin, NippoKbn, YouTblSeq, YouKataKbn, SyaRyoUnc, SyaRyoSyo, SyaRyoTes, YoushaUnc, YoushaSyo, YoushaTes, PlatNo, UkeJyKbnCd, SijJoKbn1, SijJoKbn2, SijJoKbn3, SijJoKbn4, SijJoKbn5, RotCdSeq, BikoTblSeq, HaiCom, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_Haisha WITH (HOLDLOCK TABLOCKX)')
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
CREATE CLUSTERED INDEX TKD_Haisha1 ON dbo.TKD_Haisha
	(
	SiyoKbn DESC,
	UkeNo,
	UnkRen,
	TeiDanNo,
	BunkRen
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX SyukoYmd_SyukoTime ON dbo.TKD_Haisha
	(
	SyuKoYmd,
	SyuKoTime
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX HaisYmdTime ON dbo.TKD_Haisha
	(
	HaiSYmd,
	HaiSTime
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX TouYmdTime ON dbo.TKD_Haisha
	(
	TouYmd,
	TouChTime
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX KikYmd_KikTime ON dbo.TKD_Haisha
	(
	KikYmd,
	KikTime
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
--------------------------------------------------------------------
-- System-Name	:	新発車オーライシステム
-- Module-Name	:	貸切バスモジュール
-- TR-ID		:	GK_Haisha01
-- DB-Name		:	配車テーブル
-- Name			:	配車テーブルの履歴作成トリガー
-- Date			:	2008/02/12
-- Author		:	K.Wajima
-- Descriotion	:	配車テーブルのトリガー
-- 					履歴ファイルへ更新後履歴の出力
--------------------------------------------------------------------
-- Update		:	
-- Comment		:	
--------------------------------------------------------------------
CREATE TRIGGER	[dbo].[GK_Haisha01]	ON	dbo.TKD_Haisha
	AFTER	INSERT,	UPDATE,	DELETE
AS
IF NOT EXISTS (SELECT * FROM deleted)
	-- 挿入時の処理
		BEGIN
			INSERT INTO LKD_Haisha 
				(
						LogShKbn
					,	HenKeyItm
					,	UkeNo
					,	UnkRen
					,	SyaSyuRen
					,	TeiDanNo
					,	BunkRen
					,	HenKai
					,	GoSya
					,	GoSyaJyn
					,	BunKSyuJyn
					,	SyuEigCdSeq
					,	KikEigSeq
					,	HaiSSryCdSeq
					,	KSSyaRSeq
					,	DanTaNm2
					,	IkMapCdSeq
					,	IkNm
					,	SyuKoYmd
					,	SyuKoTime
					,	SyuPaTime
					,	HaiSYmd
					,	HaiSTime
					,	HaiSCdSeq
					,	HaiSNm
					,	HaiSJyus1
					,	HaiSJyus2
					,	HaiSKigou
					,	HaiSKouKCdSeq
					,	HaiSBinCdSeq
					,	HaiSSetTime
					,	KikYmd
					,	KikTime
					,	TouYmd
					,	TouChTime
					,	TouCdSeq
					,	TouNm
					,	TouJyusyo1
					,	TouJyusyo2
					,	TouKigou
					,	TouKouKCdSeq
					,	TouBinCdSeq
					,	TouSetTime
					,	JyoSyaJin
					,	PlusJin
					,	DrvJin
					,	GuiSu
					,	OthJinKbn1
					,	OthJin1
					,	OthJinKbn2
					,	OthJin2
					,	KSKbn
					,	KHinKbn
					,	HaiSKbn
					,	HaiIKbn
					,	GuiWNin
					,	NippoKbn
					,	YouTblSeq
					,	YouKataKbn
					,	SyaRyoUnc
					,	SyaRyoSyo
					,	SyaRyoTes
					,	YoushaUnc
					,	YoushaSyo
					,	YoushaTes
					,	PlatNo
					,	UkeJyKbnCd
					,	SijJoKbn1
					,	SijJoKbn2
					,	SijJoKbn3
					,	SijJoKbn4
					,	SijJoKbn5
					,	RotCdSeq
					,	BikoTblSeq
					,	HaiCom
					,	SiyoKbn
					,	UpdYmd
					,	UpdTime
					,	UpdSyainCd
					,	UpdPrgID
				)
			SELECT
					1
				,	CONVERT(CHAR(8),UkeNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),UnkRen)	+	CHAR(9)
				+	CONVERT(CHAR(3),TeiDanNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),BunkRen)
				,	UkeNo
				,	UnkRen
				,	SyaSyuRen
				,	TeiDanNo
				,	BunkRen
				,	HenKai
				,	GoSya
				,	GoSyaJyn
				,	BunKSyuJyn
				,	SyuEigCdSeq
				,	KikEigSeq
				,	HaiSSryCdSeq
				,	KSSyaRSeq
				,	DanTaNm2
				,	IkMapCdSeq
				,	IkNm
				,	SyuKoYmd
				,	SyuKoTime
				,	SyuPaTime
				,	HaiSYmd
				,	HaiSTime
				,	HaiSCdSeq
				,	HaiSNm
				,	HaiSJyus1
				,	HaiSJyus2
				,	HaiSKigou
				,	HaiSKouKCdSeq
				,	HaiSBinCdSeq
				,	HaiSSetTime
				,	KikYmd
				,	KikTime
				,	TouYmd
				,	TouChTime
				,	TouCdSeq
				,	TouNm
				,	TouJyusyo1
				,	TouJyusyo2
				,	TouKigou
				,	TouKouKCdSeq
				,	TouBinCdSeq
				,	TouSetTime
				,	JyoSyaJin
				,	PlusJin
				,	DrvJin
				,	GuiSu
				,	OthJinKbn1
				,	OthJin1
				,	OthJinKbn2
				,	OthJin2
				,	KSKbn
				,	KHinKbn
				,	HaiSKbn
				,	HaiIKbn
				,	GuiWNin
				,	NippoKbn
				,	YouTblSeq
				,	YouKataKbn
				,	SyaRyoUnc
				,	SyaRyoSyo
				,	SyaRyoTes
				,	YoushaUnc
				,	YoushaSyo
				,	YoushaTes
				,	PlatNo
				,	UkeJyKbnCd
				,	SijJoKbn1
				,	SijJoKbn2
				,	SijJoKbn3
				,	SijJoKbn4
				,	SijJoKbn5
				,	RotCdSeq
				,	BikoTblSeq
				,	HaiCom
				,	SiyoKbn
				,	UpdYmd
				,	UpdTime
				,	UpdSyainCd
				,	UpdPrgID
			FROM inserted
		END
	ELSE IF NOT EXISTS (SELECT * FROM inserted)
	-- 削除時の処理
		BEGIN
			INSERT INTO LKD_Haisha 
				(
						LogShKbn
					,	HenKeyItm
					,	UkeNo
					,	UnkRen
					,	SyaSyuRen
					,	TeiDanNo
					,	BunkRen
					,	HenKai
					,	GoSya
					,	GoSyaJyn
					,	BunKSyuJyn
					,	SyuEigCdSeq
					,	KikEigSeq
					,	HaiSSryCdSeq
					,	KSSyaRSeq
					,	DanTaNm2
					,	IkMapCdSeq
					,	IkNm
					,	SyuKoYmd
					,	SyuKoTime
					,	SyuPaTime
					,	HaiSYmd
					,	HaiSTime
					,	HaiSCdSeq
					,	HaiSNm
					,	HaiSJyus1
					,	HaiSJyus2
					,	HaiSKigou
					,	HaiSKouKCdSeq
					,	HaiSBinCdSeq
					,	HaiSSetTime
					,	KikYmd
					,	KikTime
					,	TouYmd
					,	TouChTime
					,	TouCdSeq
					,	TouNm
					,	TouJyusyo1
					,	TouJyusyo2
					,	TouKigou
					,	TouKouKCdSeq
					,	TouBinCdSeq
					,	TouSetTime
					,	JyoSyaJin
					,	PlusJin
					,	DrvJin
					,	GuiSu
					,	OthJinKbn1
					,	OthJin1
					,	OthJinKbn2
					,	OthJin2
					,	KSKbn
					,	KHinKbn
					,	HaiSKbn
					,	HaiIKbn
					,	GuiWNin
					,	NippoKbn
					,	YouTblSeq
					,	YouKataKbn
					,	SyaRyoUnc
					,	SyaRyoSyo
					,	SyaRyoTes
					,	YoushaUnc
					,	YoushaSyo
					,	YoushaTes
					,	PlatNo
					,	UkeJyKbnCd
					,	SijJoKbn1
					,	SijJoKbn2
					,	SijJoKbn3
					,	SijJoKbn4
					,	SijJoKbn5
					,	RotCdSeq
					,	BikoTblSeq
					,	HaiCom
					,	SiyoKbn
					,	UpdYmd
					,	UpdTime
					,	UpdSyainCd
					,	UpdPrgID
				)
			SELECT
					3
				,	CONVERT(CHAR(8),UkeNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),UnkRen)	+	CHAR(9)
				+	CONVERT(CHAR(3),TeiDanNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),BunkRen)
				,	UkeNo
				,	UnkRen
				,	SyaSyuRen
				,	TeiDanNo
				,	BunkRen
				,	HenKai
				,	GoSya
				,	GoSyaJyn
				,	BunKSyuJyn
				,	SyuEigCdSeq
				,	KikEigSeq
				,	HaiSSryCdSeq
				,	KSSyaRSeq
				,	DanTaNm2
				,	IkMapCdSeq
				,	IkNm
				,	SyuKoYmd
				,	SyuKoTime
				,	SyuPaTime
				,	HaiSYmd
				,	HaiSTime
				,	HaiSCdSeq
				,	HaiSNm
				,	HaiSJyus1
				,	HaiSJyus2
				,	HaiSKigou
				,	HaiSKouKCdSeq
				,	HaiSBinCdSeq
				,	HaiSSetTime
				,	KikYmd
				,	KikTime
				,	TouYmd
				,	TouChTime
				,	TouCdSeq
				,	TouNm
				,	TouJyusyo1
				,	TouJyusyo2
				,	TouKigou
				,	TouKouKCdSeq
				,	TouBinCdSeq
				,	TouSetTime
				,	JyoSyaJin
				,	PlusJin
				,	DrvJin
				,	GuiSu
				,	OthJinKbn1
				,	OthJin1
				,	OthJinKbn2
				,	OthJin2
				,	KSKbn
				,	KHinKbn
				,	HaiSKbn
				,	HaiIKbn
				,	GuiWNin
				,	NippoKbn
				,	YouTblSeq
				,	YouKataKbn
				,	SyaRyoUnc
				,	SyaRyoSyo
				,	SyaRyoTes
				,	YoushaUnc
				,	YoushaSyo
				,	YoushaTes
				,	PlatNo
				,	UkeJyKbnCd
				,	SijJoKbn1
				,	SijJoKbn2
				,	SijJoKbn3
				,	SijJoKbn4
				,	SijJoKbn5
				,	RotCdSeq
				,	BikoTblSeq
				,	HaiCom
				,	SiyoKbn
				,	UpdYmd
				,	UpdTime
				,	UpdSyainCd
				,	UpdPrgID
			FROM deleted
		END
	ELSE
		-- 更新時の処理
		BEGIN
			INSERT INTO LKD_Haisha
				(
						LogShKbn
					,	HenKeyItm
					,	UkeNo
					,	UnkRen
					,	SyaSyuRen
					,	TeiDanNo
					,	BunkRen
					,	HenKai
					,	GoSya
					,	GoSyaJyn
					,	BunKSyuJyn
					,	SyuEigCdSeq
					,	KikEigSeq
					,	HaiSSryCdSeq
					,	KSSyaRSeq
					,	DanTaNm2
					,	IkMapCdSeq
					,	IkNm
					,	SyuKoYmd
					,	SyuKoTime
					,	SyuPaTime
					,	HaiSYmd
					,	HaiSTime
					,	HaiSCdSeq
					,	HaiSNm
					,	HaiSJyus1
					,	HaiSJyus2
					,	HaiSKigou
					,	HaiSKouKCdSeq
					,	HaiSBinCdSeq
					,	HaiSSetTime
					,	KikYmd
					,	KikTime
					,	TouYmd
					,	TouChTime
					,	TouCdSeq
					,	TouNm
					,	TouJyusyo1
					,	TouJyusyo2
					,	TouKigou
					,	TouKouKCdSeq
					,	TouBinCdSeq
					,	TouSetTime
					,	JyoSyaJin
					,	PlusJin
					,	DrvJin
					,	GuiSu
					,	OthJinKbn1
					,	OthJin1
					,	OthJinKbn2
					,	OthJin2
					,	KSKbn
					,	KHinKbn
					,	HaiSKbn
					,	HaiIKbn
					,	GuiWNin
					,	NippoKbn
					,	YouTblSeq
					,	YouKataKbn
					,	SyaRyoUnc
					,	SyaRyoSyo
					,	SyaRyoTes
					,	YoushaUnc
					,	YoushaSyo
					,	YoushaTes
					,	PlatNo
					,	UkeJyKbnCd
					,	SijJoKbn1
					,	SijJoKbn2
					,	SijJoKbn3
					,	SijJoKbn4
					,	SijJoKbn5
					,	RotCdSeq
					,	BikoTblSeq
					,	HaiCom
					,	SiyoKbn
					,	UpdYmd
					,	UpdTime
					,	UpdSyainCd
					,	UpdPrgID
				)
			SELECT
					2
				,	CONVERT(CHAR(8),deleted.UkeNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),deleted.UnkRen)	+	CHAR(9)
				+	CONVERT(CHAR(3),deleted.TeiDanNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),deleted.BunkRen)
				,	inserted.UkeNo
				,	inserted.UnkRen
				,	inserted.SyaSyuRen
				,	inserted.TeiDanNo
				,	inserted.BunkRen
				,	inserted.HenKai
				,	inserted.GoSya
				,	inserted.GoSyaJyn
				,	inserted.BunKSyuJyn
				,	inserted.SyuEigCdSeq
				,	inserted.KikEigSeq
				,	inserted.HaiSSryCdSeq
				,	inserted.KSSyaRSeq
				,	inserted.DanTaNm2
				,	inserted.IkMapCdSeq
				,	inserted.IkNm
				,	inserted.SyuKoYmd
				,	inserted.SyuKoTime
				,	inserted.SyuPaTime
				,	inserted.HaiSYmd
				,	inserted.HaiSTime
				,	inserted.HaiSCdSeq
				,	inserted.HaiSNm
				,	inserted.HaiSJyus1
				,	inserted.HaiSJyus2
				,	inserted.HaiSKigou
				,	inserted.HaiSKouKCdSeq
				,	inserted.HaiSBinCdSeq
				,	inserted.HaiSSetTime
				,	inserted.KikYmd
				,	inserted.KikTime
				,	inserted.TouYmd
				,	inserted.TouChTime
				,	inserted.TouCdSeq
				,	inserted.TouNm
				,	inserted.TouJyusyo1
				,	inserted.TouJyusyo2
				,	inserted.TouKigou
				,	inserted.TouKouKCdSeq
				,	inserted.TouBinCdSeq
				,	inserted.TouSetTime
				,	inserted.JyoSyaJin
				,	inserted.PlusJin
				,	inserted.DrvJin
				,	inserted.GuiSu
				,	inserted.OthJinKbn1
				,	inserted.OthJin1
				,	inserted.OthJinKbn2
				,	inserted.OthJin2
				,	inserted.KSKbn
				,	inserted.KHinKbn
				,	inserted.HaiSKbn
				,	inserted.HaiIKbn
				,	inserted.GuiWNin
				,	inserted.NippoKbn
				,	inserted.YouTblSeq
				,	inserted.YouKataKbn
				,	inserted.SyaRyoUnc
				,	inserted.SyaRyoSyo
				,	inserted.SyaRyoTes
				,	inserted.YoushaUnc
				,	inserted.YoushaSyo
				,	inserted.YoushaTes
				,	inserted.PlatNo
				,	inserted.UkeJyKbnCd
				,	inserted.SijJoKbn1
				,	inserted.SijJoKbn2
				,	inserted.SijJoKbn3
				,	inserted.SijJoKbn4
				,	inserted.SijJoKbn5
				,	inserted.RotCdSeq
				,	inserted.BikoTblSeq
				,	inserted.HaiCom
				,	inserted.SiyoKbn
				,	inserted.UpdYmd
				,	inserted.UpdTime
				,	inserted.UpdSyainCd
				,	inserted.UpdPrgID
			FROM inserted,deleted
		END
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Haisha', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Haisha', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Haisha', 'Object', 'CONTROL') as Contr_Per 