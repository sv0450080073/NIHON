/*
   2020年11月23日16:07:23
   User: usr_devhk
   Server: kobo-db-inst05.cgtphzvll9lw.ap-northeast-1.rds.amazonaws.com
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
CREATE TABLE dbo.Tmp_TKD_BookingMaxMinFareFeeCalc
	(
	UkeNo nchar(15) NOT NULL,
	UnkRen smallint NOT NULL,
	SyaSyuRen smallint NOT NULL,
	TransportationPlaceCodeSeq int NOT NULL,
	KataKbn tinyint NOT NULL,
	ZeiKbn tinyint NOT NULL,
	Zeiritsu numeric(3, 1) NOT NULL,
	RunningKmSum numeric(7, 2) NOT NULL,
	RunningKmCalc numeric(7, 2) NOT NULL,
	RestraintTimeSum char(5) NOT NULL,
	RestraintTimeCalc char(5) NOT NULL,
	ServiceKmSum numeric(7, 2) NOT NULL,
	ServiceTimeSum char(5) NOT NULL,
	MidnightEarlyMorningTimeSum char(5) NOT NULL,
	MidnightEarlyMorningTimeCalc char(5) NOT NULL,
	ChangeDriverRunningKmSum numeric(7, 2) NOT NULL,
	ChangeDriverRunningKmCalc numeric(7, 2) NOT NULL,
	ChangeDriverRestraintTimeSum char(5) NOT NULL,
	ChangeDriverRestraintTimeCalc char(5) NOT NULL,
	ChangeDriverMidnightEarlyMorningTimeSum char(5) NOT NULL,
	ChangeDriverMidnightEarlyMorningTimeCalc char(5) NOT NULL,
	WaribikiKbn tinyint NOT NULL,
	AnnualContractFlag tinyint NOT NULL,
	SpecialFlg tinyint NOT NULL,
	FareMaxAmount int NOT NULL,
	FareMinAmount int NOT NULL,
	FeeMaxAmount int NOT NULL,
	FeeMinAmount int NOT NULL,
	UnitPriceMaxAmount int NOT NULL,
	UnitPriceMinAmount int NOT NULL,
	FareMaxAmountforKm int NOT NULL,
	FareMinAmountforKm int NOT NULL,
	FareMaxAmountforHour int NOT NULL,
	FareMinAmountforHour int NOT NULL,
	ChangeDriverFareMaxAmountforKm int NOT NULL,
	ChangeDriverFareMinAmountforKm int NOT NULL,
	ChangeDriverFareMaxAmountforHour int NOT NULL,
	ChangeDriverFareMinAmountforHour int NOT NULL,
	MidnightEarlyMorningFeeMaxAmount int NOT NULL,
	MidnightEarlyMorningFeeMinAmount int NOT NULL,
	SpecialVehicalFeeMaxAmount int NOT NULL,
	SpecialVehicalFeeMinAmount int NOT NULL,
	FareIndex numeric(3, 1) NULL,
	FeeIndex numeric(3, 1) NULL,
	UnitPriceIndex numeric(3, 1) NULL,
	UpdYmd char(8) NOT NULL,
	UpdTime char(6) NOT NULL,
	UpdSyainCd int NOT NULL,
	UpdPrgID char(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_TKD_BookingMaxMinFareFeeCalc SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.TKD_BookingMaxMinFareFeeCalc)
	 EXEC('INSERT INTO dbo.Tmp_TKD_BookingMaxMinFareFeeCalc (UkeNo, UnkRen, SyaSyuRen, TransportationPlaceCodeSeq, KataKbn, ZeiKbn, Zeiritsu, RunningKmSum, RunningKmCalc, RestraintTimeSum, RestraintTimeCalc, ServiceKmSum, ServiceTimeSum, MidnightEarlyMorningTimeSum, MidnightEarlyMorningTimeCalc, ChangeDriverRunningKmSum, ChangeDriverRunningKmCalc, ChangeDriverRestraintTimeSum, ChangeDriverRestraintTimeCalc, ChangeDriverMidnightEarlyMorningTimeSum, ChangeDriverMidnightEarlyMorningTimeCalc, WaribikiKbn, AnnualContractFlag, SpecialFlg, FareMaxAmount, FareMinAmount, FeeMaxAmount, FeeMinAmount, UnitPriceMaxAmount, UnitPriceMinAmount, FareMaxAmountforKm, FareMinAmountforKm, FareMaxAmountforHour, FareMinAmountforHour, ChangeDriverFareMaxAmountforKm, ChangeDriverFareMinAmountforKm, ChangeDriverFareMaxAmountforHour, ChangeDriverFareMinAmountforHour, MidnightEarlyMorningFeeMaxAmount, MidnightEarlyMorningFeeMinAmount, SpecialVehicalFeeMaxAmount, SpecialVehicalFeeMinAmount, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID)
		SELECT UkeNo, UnkRen, SyaSyuRen, TransportationPlaceCodeSeq, KataKbn, ZeiKbn, Zeiritsu, RunningKmSum, RunningKmCalc, RestraintTimeSum, RestraintTimeCalc, ServiceKmSum, ServiceTimeSum, MidnightEarlyMorningTimeSum, MidnightEarlyMorningTimeCalc, ChangeDriverRunningKmSum, ChangeDriverRunningKmCalc, ChangeDriverRestraintTimeSum, ChangeDriverRestraintTimeCalc, ChangeDriverMidnightEarlyMorningTimeSum, ChangeDriverMidnightEarlyMorningTimeCalc, WaribikiKbn, AnnualContractFlag, SpecialFlg, FareMaxAmount, FareMinAmount, FeeMaxAmount, FeeMinAmount, UnitPriceMaxAmount, UnitPriceMinAmount, FareMaxAmountforKm, FareMinAmountforKm, FareMaxAmountforHour, FareMinAmountforHour, ChangeDriverFareMaxAmountforKm, ChangeDriverFareMinAmountforKm, ChangeDriverFareMaxAmountforHour, ChangeDriverFareMinAmountforHour, MidnightEarlyMorningFeeMaxAmount, MidnightEarlyMorningFeeMinAmount, SpecialVehicalFeeMaxAmount, SpecialVehicalFeeMinAmount, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID FROM dbo.TKD_BookingMaxMinFareFeeCalc WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.TKD_BookingMaxMinFareFeeCalc
GO
EXECUTE sp_rename N'dbo.Tmp_TKD_BookingMaxMinFareFeeCalc', N'TKD_BookingMaxMinFareFeeCalc', 'OBJECT' 
GO
ALTER TABLE dbo.TKD_BookingMaxMinFareFeeCalc ADD CONSTRAINT
	PK_TKD_BookingMaxMinFareFeeCalc PRIMARY KEY CLUSTERED 
	(
	UkeNo,
	UnkRen,
	SyaSyuRen
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.TKD_BookingMaxMinFareFeeCalc', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TKD_BookingMaxMinFareFeeCalc', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TKD_BookingMaxMinFareFeeCalc', 'Object', 'CONTROL') as Contr_Per 