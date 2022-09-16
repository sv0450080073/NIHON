USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_CustomItems]    Script Date: 2021/03/03 10:20:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VPM_CustomItems]
AS
SELECT                      TenantCdSeq, CustomItemsNo, CustomName, Description, FormatKbn, RequiredItemFlg, TextLengh, NumberRangeLimitFlg, NumberRangeOrMore, NumberRangeOrLess, NumberInitialValueFlg, 
                                      NumberInitialValue, NumberUnitFlg, NumberUnit, NumberRoundKbn, NumberFloatFlg, NumberScale, DatetimeRangeLimitFlg, DatetimeRangeLimitStartYmd, DatetimeRangeLimitEndYmd, DatetimeInitialValueFlg, 
                                      DatetimeInitialValueKbn, DateTimeDisplayFormatKbn, TimeRangeLimitFlg, TimeRangeLimitStartTime, TimeRangeLimitEndTime, TimeInitialValueFlg, TimeInitialValueKbn, CodeSyu, UpdYmd, UpdTime, UpdSyainCd, 
                                      UpdPrgID
FROM                          HOC_Master.dbo.TPM_CustomItems
GO