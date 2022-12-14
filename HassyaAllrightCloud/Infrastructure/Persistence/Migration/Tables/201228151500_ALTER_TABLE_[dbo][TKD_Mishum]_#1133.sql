/*
   2020年12月28日13:00:52
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
CREATE TABLE dbo.Tmp_TKD_Mishum
	(
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
ALTER TABLE dbo.Tmp_TKD_Mishum SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.TKD_Mishum)
	 EXEC('INSERT INTO dbo.Tmp_TKD_Mishum (UkeNo, MisyuRen, HenKai, SeiFutSyu, UriGakKin, SyaRyoSyo, SyaRyoTes, SeiKin, NyuKinRui, CouKesRui, FutuUnkRen, FutTumRen, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT UkeNo, MisyuRen, HenKai, SeiFutSyu, CONVERT(bigint, UriGakKin), CONVERT(bigint, SyaRyoSyo), CONVERT(bigint, SyaRyoTes), CONVERT(bigint, SeiKin), NyuKinRui, CouKesRui, FutuUnkRen, FutTumRen, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_Mishum WITH (HOLDLOCK TABLOCKX)')
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
--------------------------------------------------------------------
-- System-Name	:	新発車オーライシステム
-- Module-Name	:	貸切バスモジュール
-- TR-ID		:	GK_Mishum01
-- DB-Name		:	未収明細テーブル
-- Name			:	未収明細テーブルの履歴作成トリガー
-- Date			:	2008/02/14
-- Author		:	K.Wajima
-- Descriotion	:	未収明細テーブルのトリガー
-- 					履歴ファイルへ更新後履歴の出力
--------------------------------------------------------------------
-- Update		:	
-- Comment		:	
--------------------------------------------------------------------
CREATE TRIGGER	[dbo].[GK_Mishum01]	ON	dbo.TKD_Mishum
	AFTER	INSERT,	UPDATE,	DELETE
AS

	IF NOT EXISTS (SELECT * FROM deleted)
	-- 挿入時の処理
		BEGIN
			INSERT INTO LKD_Mishum 
				(
						LogShKbn
					,	HenKeyItm
					,	UkeNo
					,	MisyuRen
					,	HenKai
					,	SeiFutSyu
					,	UriGakKin
					,	SyaRyoSyo
					,	SyaRyoTes
					,	SeiKin
					,	NyuKinRui
					,	CouKesRui
					,	FutuUnkRen
					,	FutTumRen
					,	SiyoKbn
					,	UpdYmd
					,	UpdTime
					,	UpdSyainCd
					,	UpdPrgID
				)
			SELECT
					1
				,	CONVERT(CHAR(8),UkeNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),MisyuRen)
				,	UkeNo
				,	MisyuRen
				,	HenKai
				,	SeiFutSyu
				,	UriGakKin
				,	SyaRyoSyo
				,	SyaRyoTes
				,	SeiKin
				,	NyuKinRui
				,	CouKesRui
				,	FutuUnkRen
				,	FutTumRen
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
			INSERT INTO LKD_Mishum 
				(
						LogShKbn
					,	HenKeyItm
					,	UkeNo
					,	MisyuRen
					,	HenKai
					,	SeiFutSyu
					,	UriGakKin
					,	SyaRyoSyo
					,	SyaRyoTes
					,	SeiKin
					,	NyuKinRui
					,	CouKesRui
					,	FutuUnkRen
					,	FutTumRen
					,	SiyoKbn
					,	UpdYmd
					,	UpdTime
					,	UpdSyainCd
					,	UpdPrgID
				)
			SELECT
					3
				,	CONVERT(CHAR(8),UkeNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),MisyuRen)
				,	UkeNo
				,	MisyuRen
				,	HenKai
				,	SeiFutSyu
				,	UriGakKin
				,	SyaRyoSyo
				,	SyaRyoTes
				,	SeiKin
				,	NyuKinRui
				,	CouKesRui
				,	FutuUnkRen
				,	FutTumRen
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
			INSERT INTO LKD_Mishum
				(
						LogShKbn
					,	HenKeyItm
					,	UkeNo
					,	MisyuRen
					,	HenKai
					,	SeiFutSyu
					,	UriGakKin
					,	SyaRyoSyo
					,	SyaRyoTes
					,	SeiKin
					,	NyuKinRui
					,	CouKesRui
					,	FutuUnkRen
					,	FutTumRen
					,	SiyoKbn
					,	UpdYmd
					,	UpdTime
					,	UpdSyainCd
					,	UpdPrgID
				)
			SELECT
					2
				,	CONVERT(CHAR(8),deleted.UkeNo)	+	CHAR(9)
				+	CONVERT(CHAR(3),deleted.MisyuRen)
				,	inserted.UkeNo
				,	inserted.MisyuRen
				,	inserted.HenKai
				,	inserted.SeiFutSyu
				,	inserted.UriGakKin
				,	inserted.SyaRyoSyo
				,	inserted.SyaRyoTes
				,	inserted.SeiKin
				,	inserted.NyuKinRui
				,	inserted.CouKesRui
				,	inserted.FutuUnkRen
				,	inserted.FutTumRen
				,	inserted.SiyoKbn
				,	inserted.UpdYmd
				,	inserted.UpdTime
				,	inserted.UpdSyainCd
				,	inserted.UpdPrgID
			FROM inserted,deleted
		END
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Mishum', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Mishum', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Mishum', 'Object', 'CONTROL') as Contr_Per 