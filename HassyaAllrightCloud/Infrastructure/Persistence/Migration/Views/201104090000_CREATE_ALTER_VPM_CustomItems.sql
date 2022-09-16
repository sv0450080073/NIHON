USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_CustomItems]    Script Date: 2020/11/04 9:06:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VPM_CustomItems]
AS
SELECT                      TenantCdSeq, CustomItemsNo, CostomName, Description, FormatKbn, RequiredItemFlg, TextLengh, NumberRangeLimitFlg, NumberRangeOrMore, NumberRangeOrLess, NumberInitialValueFlg, 
                                      NumberInitialValue, NumberUnitFlg, NumberUnit, NumberRoundKbn, NumberFloatFlg, NumberScale, DatetimeRangeLimitFlg, DatetimeRangeLimitStartYmd, DatetimeRangeLimitEndYmd, DatetimeInitialValueFlg, 
                                      DatetimeInitialValueKbn, DateTimeDisplayFormatKbn, TimeRangeLimitFlg, TimeRangeLimitStartTime, TimeRangeLimitEndTime, TimeInitialValueFlg, TimeInitialValueKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID
FROM                         HOC_Master.dbo.TPM_CustomItems
GO