USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dVehicleDailyReportHaisha_R]    Script Date: 4/28/2021 10:48:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAllrightCloud
-- Module-Name	:   HassyaAllrightCloud
-- SP-ID		:   PK_dVehicleDailyReportHaisha_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get vehicle daily report haisha for edit
-- Date			:   2020/08/11
-- Author		:   P.M.NHAT
-- Description	:   Get vehicle daily report haisha for edit with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dVehicleDailyReportHaisha_R]
	-- Add the parameters for the stored procedure here
		@UkeNo			nchar(15)	
	,	@UnkRen			smallint	
	,	@TeiDanNo		smallint			
	,	@BunkRen		smallint
AS
BEGIN
	SELECT TKD_Haisha.UkeNo
        ,TKD_Haisha.UnkRen
        ,TKD_Haisha.SyaSyuRen
        ,TKD_Haisha.TeiDanNo
        ,JT_Shabni.BunkRen
        ,JT_BookingMaxMin.FeeMaxAmount
        ,JT_BookingMaxMin.FeeMinAmount
        ,JT_BookingMaxMin.TransportationPlaceCodeSeq
        ,JT_BookingMaxMin.KataKbn
        ,JT_FeeRule.BigVehicalMaxUnitPriceforHour AS BigVehicalMaxUnitPriceforHour
        ,JT_FeeRule.BigVehicalMinUnitPriceforHour AS BigVehicalMinUnitPriceforHour
        ,JT_FeeRule.BigVehicalMaxUnitPriceforKm AS BigVehicalMaxUnitPriceforKm
        ,JT_FeeRule.BigVehicalMinUnitPriceforKm AS BigVehicalMinUnitPriceforKm
        ,JT_FeeRule.MedVehicalMaxUnitPriceforHour AS MedVehicalMaxUnitPriceforHour
        ,JT_FeeRule.MedVehicalMinUnitPriceforHour AS MedVehicalMinUnitPriceforHour
        ,JT_FeeRule.MedVehicalMaxUnitPriceforKm AS MedVehicalMaxUnitPriceforKm
        ,JT_FeeRule.MedVehicalMinUnitPriceforKm AS MedVehicalMinUnitPriceforKm
        ,JT_FeeRule.SmallVehicalMaxUnitPriceforHour AS SmallVehicalMaxUnitPriceforHour
        ,JT_FeeRule.SmallVehicalMinUnitPriceforHour AS SmallVehicalMinUnitPriceforHour
        ,JT_FeeRule.SmallVehicalMaxUnitPriceforKm AS SmallVehicalMaxUnitPriceforKm
        ,JT_FeeRule.SmallVehicalMinUnitPriceforKm AS SmallVehicalMinUnitPriceforKm,
		TKD_Haisha.UpdYmd,
		TKD_Haisha.UpdTime
	FROM TKD_Haisha
	LEFT JOIN TKD_Shabni AS JT_Shabni ON TKD_Haisha.UkeNo = JT_Shabni.UkeNo
			AND TKD_Haisha.UnkRen = JT_Shabni.UnkRen
			AND TKD_Haisha.TeiDanNo = JT_Shabni.TeiDanNo
			AND TKD_Haisha.BunkRen = JT_Shabni.BunkRen
	LEFT JOIN TKD_BookingMaxMinFareFeeCalc AS JT_BookingMaxMin ON TKD_Haisha.UkeNo = JT_BookingMaxMin.UkeNo
			AND TKD_Haisha.UnkRen = JT_BookingMaxMin.UnkRen
			AND TKD_Haisha.SyaSyuRen = JT_BookingMaxMin.SyaSyuRen
	LEFT JOIN VPM_TransportationFeeRule AS JT_FeeRule ON JT_FeeRule.TransportationPlaceCodeSeq = JT_BookingMaxMin.TransportationPlaceCodeSeq
	WHERE TKD_Haisha.UkeNo = @UkeNo
			AND TKD_Haisha.UnkRen = @UnkRen
			AND TKD_Haisha.TeiDanNo = @TeiDanNo
			AND TKD_Haisha.BunkRen = @BunkRen
END
