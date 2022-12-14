/*
   2021年3月9日11:04:26
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
ALTER TABLE dbo.TKD_Yyksho
	DROP CONSTRAINT DF_TKD_Yyksho_TenantCdSeq
GO
ALTER TABLE dbo.TKD_Yyksho
	DROP CONSTRAINT DF_TKD_Yyksho_UkeCd
GO
ALTER TABLE dbo.TKD_Yyksho
	DROP CONSTRAINT DF__Tmp_TKD_Y__TaxTy__1A573CBC
GO
ALTER TABLE dbo.TKD_Yyksho
	DROP CONSTRAINT DF__Tmp_TKD_Y__TaxGu__1B4B60F5
GO
ALTER TABLE dbo.TKD_Yyksho
	DROP CONSTRAINT DF__Tmp_TKD_Y__FeeGu__1C3F852E
GO
ALTER TABLE dbo.TKD_Yyksho
	DROP CONSTRAINT DF__Tmp_TKD_Y__FeeGu__1D33A967
GO
ALTER TABLE dbo.TKD_Yyksho
	DROP CONSTRAINT DF_TKD_Yyksho_GuitKin
GO
CREATE TABLE dbo.Tmp_TKD_Yyksho
	(
	TenantCdSeq int NOT NULL,
	UkeNo nchar(15) NOT NULL,
	UkeCd int NOT NULL,
	HenKai smallint NOT NULL,
	UkeYmd char(8) NOT NULL,
	YoyaSyu tinyint NOT NULL,
	YoyaKbnSeq int NOT NULL,
	KikakuNo int NOT NULL,
	TourCd char(8) NOT NULL,
	KasTourCdSeq int NOT NULL,
	UkeEigCdSeq int NOT NULL,
	SeiEigCdSeq int NOT NULL,
	IraEigCdSeq int NOT NULL,
	EigTanCdSeq int NOT NULL,
	InTanCdSeq int NOT NULL,
	YoyaNm nvarchar(100) NOT NULL,
	YoyaKana varchar(50) NOT NULL,
	TokuiSeq int NOT NULL,
	SitenCdSeq int NOT NULL,
	SirCdSeq int NOT NULL,
	SirSitenCdSeq int NOT NULL,
	TokuiTel char(14) NOT NULL,
	TokuiTanNm varchar(64) NOT NULL,
	TokuiFax char(14) NOT NULL,
	TokuiMail varchar(50) NOT NULL,
	UntKin numeric(18, 0) NOT NULL,
	ZeiKbn tinyint NOT NULL,
	Zeiritsu numeric(3, 1) NOT NULL,
	ZeiRui numeric(18, 0) NOT NULL,
	TaxTypeforGuider tinyint NOT NULL,
	TaxGuider numeric(18, 0) NOT NULL,
	TesuRitu numeric(3, 1) NOT NULL,
	TesuRyoG numeric(18, 0) NOT NULL,
	FeeGuiderRate numeric(3, 1) NOT NULL,
	FeeGuider numeric(18, 0) NOT NULL,
	GuitKin numeric(18, 0) NOT NULL,
	SeiKyuKbnSeq int NOT NULL,
	SeikYm char(6) NOT NULL,
	SeiTaiYmd char(8) NOT NULL,
	CanRit numeric(4, 1) NOT NULL,
	CanUnc int NOT NULL,
	CanZKbn tinyint NOT NULL,
	CanSyoR numeric(3, 1) NOT NULL,
	CanSyoG int NOT NULL,
	CanYmd char(8) NOT NULL,
	CanTanSeq int NOT NULL,
	CanRiy varchar(50) NOT NULL,
	CanFuYmd char(8) NOT NULL,
	CanFuTanSeq int NOT NULL,
	CanFuRiy varchar(50) NOT NULL,
	BikoTblSeq int NOT NULL,
	KSKbn tinyint NOT NULL,
	KHinKbn tinyint NOT NULL,
	KaknKais tinyint NOT NULL,
	KaktYmd char(8) NOT NULL,
	HaiSKbn tinyint NOT NULL,
	HaiIKbn tinyint NOT NULL,
	GuiWNin smallint NOT NULL,
	NippoKbn tinyint NOT NULL,
	YouKbn tinyint NOT NULL,
	NyuKinKbn tinyint NOT NULL,
	NCouKbn tinyint NOT NULL,
	SihKbn tinyint NOT NULL,
	SCouKbn tinyint NOT NULL,
	SiyoKbn tinyint NOT NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL,
	BikoNm nvarchar(250) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho SET (LOCK_ESCALATION = TABLE)
GO
DECLARE @v sql_variant 
SET @v = N'受付番号：[テナント3桁数][受付コード7桁数]'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Yyksho', N'COLUMN', N'UkeNo'
GO
DECLARE @v sql_variant 
SET @v = N'受付コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Yyksho', N'COLUMN', N'UkeCd'
GO
DECLARE @v sql_variant 
SET @v = N'消費税タイプ（ガイド用）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Yyksho', N'COLUMN', N'TaxTypeforGuider'
GO
DECLARE @v sql_variant 
SET @v = N'ガイド税'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Yyksho', N'COLUMN', N'TaxGuider'
GO
DECLARE @v sql_variant 
SET @v = N'手数料率（ガイド用）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Yyksho', N'COLUMN', N'FeeGuiderRate'
GO
DECLARE @v sql_variant 
SET @v = N'手数料税'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_TKD_Yyksho', N'COLUMN', N'FeeGuider'
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho ADD CONSTRAINT
	DF_TKD_Yyksho_TenantCdSeq DEFAULT ((0)) FOR TenantCdSeq
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho ADD CONSTRAINT
	DF_TKD_Yyksho_UkeCd DEFAULT ((0)) FOR UkeCd
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho ADD CONSTRAINT
	DF__Tmp_TKD_Y__TaxTy__1A573CBC DEFAULT ((3)) FOR TaxTypeforGuider
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho ADD CONSTRAINT
	DF__Tmp_TKD_Y__TaxGu__1B4B60F5 DEFAULT ((0)) FOR TaxGuider
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho ADD CONSTRAINT
	DF__Tmp_TKD_Y__FeeGu__1C3F852E DEFAULT ((0)) FOR FeeGuiderRate
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho ADD CONSTRAINT
	DF__Tmp_TKD_Y__FeeGu__1D33A967 DEFAULT ((0)) FOR FeeGuider
GO
ALTER TABLE dbo.Tmp_TKD_Yyksho ADD CONSTRAINT
	DF_TKD_Yyksho_GuitKin DEFAULT ((0)) FOR GuitKin
GO
IF EXISTS(SELECT * FROM dbo.TKD_Yyksho)
	 EXEC('INSERT INTO dbo.Tmp_TKD_Yyksho (TenantCdSeq, UkeNo, UkeCd, HenKai, UkeYmd, YoyaSyu, YoyaKbnSeq, KikakuNo, TourCd, KasTourCdSeq, UkeEigCdSeq, SeiEigCdSeq, IraEigCdSeq, EigTanCdSeq, InTanCdSeq, YoyaNm, YoyaKana, TokuiSeq, SitenCdSeq, SirCdSeq, SirSitenCdSeq, TokuiTel, TokuiTanNm, TokuiFax, TokuiMail, UntKin, ZeiKbn, Zeiritsu, ZeiRui, TaxTypeforGuider, TaxGuider, TesuRitu, TesuRyoG, FeeGuiderRate, FeeGuider, GuitKin, SeiKyuKbnSeq, SeikYm, SeiTaiYmd, CanRit, CanUnc, CanZKbn, CanSyoR, CanSyoG, CanYmd, CanTanSeq, CanRiy, CanFuYmd, CanFuTanSeq, CanFuRiy, BikoTblSeq, KSKbn, KHinKbn, KaknKais, KaktYmd, HaiSKbn, HaiIKbn, GuiWNin, NippoKbn, YouKbn, NyuKinKbn, NCouKbn, SihKbn, SCouKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID, BikoNm)
		SELECT TenantCdSeq, UkeNo, UkeCd, HenKai, UkeYmd, YoyaSyu, YoyaKbnSeq, KikakuNo, TourCd, KasTourCdSeq, UkeEigCdSeq, SeiEigCdSeq, IraEigCdSeq, EigTanCdSeq, InTanCdSeq, YoyaNm, YoyaKana, TokuiSeq, SitenCdSeq, SirCdSeq, SirSitenCdSeq, TokuiTel, TokuiTanNm, TokuiFax, TokuiMail, UntKin, ZeiKbn, Zeiritsu, ZeiRui, TaxTypeforGuider, TaxGuider, TesuRitu, TesuRyoG, FeeGuiderRate, FeeGuider, GuitKin, SeiKyuKbnSeq, SeikYm, SeiTaiYmd, CanRit, CanUnc, CanZKbn, CanSyoR, CanSyoG, CanYmd, CanTanSeq, CanRiy, CanFuYmd, CanFuTanSeq, CanFuRiy, BikoTblSeq, KSKbn, KHinKbn, KaknKais, KaktYmd, HaiSKbn, HaiIKbn, GuiWNin, NippoKbn, YouKbn, NyuKinKbn, NCouKbn, SihKbn, SCouKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID, BikoNm FROM dbo.TKD_Yyksho WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TKD_Yyksho
GO
EXECUTE sp_rename N'dbo.Tmp_TKD_Yyksho', N'TKD_Yyksho', 'OBJECT' 
GO
ALTER TABLE dbo.TKD_Yyksho ADD CONSTRAINT
	PK_TKD_Yyksho PRIMARY KEY NONCLUSTERED 
	(
	TenantCdSeq,
	UkeNo
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_Yyksho', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_Yyksho', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_Yyksho', 'Object', 'CONTROL') as Contr_Per 