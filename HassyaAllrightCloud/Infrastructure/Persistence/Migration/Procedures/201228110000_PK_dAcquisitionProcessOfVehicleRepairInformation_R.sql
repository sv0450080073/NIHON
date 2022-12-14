USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetAlertSettingByCode38_R]    Script Date: 1/7/2021 3:19:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetAlertSettingByCode
-- Date			:   2020/12/22
-- Author		:   T.L.DUY
-- Description	:   Get alert setting by code base on sheet excel 3.8
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dAcquisitionProcessOfVehicleRepairInformation_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@StaYmd					CHAR(8),
		@EndYmd					CHAR(8),
		@SharyoSeqList			NVARCHAR(100),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT SHURI.ShuriTblSeq --AS '修理Seq'
        ,SHURI.SyaRyoCdSeq --AS '車両Seq'
        ,SHURI.ShuriSYmd --AS '修理開始年月日'
        ,SHURI.ShuriSTime --AS '修理開始時間'
        ,SHURI.ShuriEYmd --AS '修理終了年月日'
        ,SHURI.ShuriETime --AS '修理終了時間'
        ,SYASYU.KataKbn --AS '型区分'
FROM TKD_Shuri AS SHURI
LEFT JOIN VPM_SyaRyo AS SYARYO ON SYARYO.SyaRyoCdSeq = SHURI.SyaRyoCdSeq
LEFT JOIN VPM_SyaSyu AS SYASYU ON SYASYU.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq
        AND TenantCdSeq = @TenantCdSeq
WHERE SHURI.ShuriSYmd <= @EndYmd 
        AND SHURI.ShuriEYmd >= @StaYmd
        AND SHURI.SyaRyoCdSeq IN (SELECT value FROM STRING_SPLIT(@SharyoSeqList, ','))
        AND SHURI.SiyoKbn = 1

SET	@ROWCOUNT	=	@@ROWCOUNT
END
