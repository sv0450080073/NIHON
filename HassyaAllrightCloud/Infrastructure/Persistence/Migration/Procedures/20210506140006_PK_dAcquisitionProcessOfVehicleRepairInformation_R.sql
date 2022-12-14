USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dAcquisitionProcessOfVehicleRepairInformation_R]    Script Date: 5/17/2021 11:16:32 AM ******/
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
SELECT ISNULL(SHURI.ShuriTblSeq, 0) as ShuriTblSeq --AS '修理Seq'
        ,ISNULL(SHURI.SyaRyoCdSeq, 0) as SyaRyoCdSeq --AS '車両Seq'
        ,SHURI.ShuriSYmd --AS '修理開始年月日'
        ,SHURI.ShuriSTime --AS '修理開始時間'
        ,SHURI.ShuriEYmd --AS '修理終了年月日'
        ,SHURI.ShuriETime --AS '修理終了時間'
        ,ISNULL(SYASYU.KataKbn, 0) as KataKbn --AS '型区分'
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
