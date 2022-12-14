USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetAlertSettingByCode310_R]    Script Date: 1/7/2021 3:21:42 PM ******/
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
-- Description	:   Get alert setting by code base on sheet excel 3.10
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dAcquisitionProcessOfWorkHolidayInformation_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@StaYmd					CHAR(8),
		@EndYmd					CHAR(8),
		@SyainSeqList			NVARCHAR(100),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT KOBAN.UnkYmd --AS '交番_運行年月日'
        ,KOBAN.SyainCdSeq --AS '交番_社員Seq'
        ,KOBAN.KouBnRen --AS '交番_交番連番'
        ,KIKYUJ.KinKyuTblCdSeq --AS '勤務休日_勤務休日TblSeq'
        ,KINKYU.KinKyuKbn --AS '勤務休日種別_種別区分'
        ,KINKYU.KyusyutsuKbn --AS '休出勤'
        ,KYOSHE.BigTypeDrivingFlg --AS '大型乗務フラグ'
        ,KYOSHE.MediumTypeDrivingFlg --AS '中型乗務フラグ'
        ,KYOSHE.SmallTypeDrivingFlg --AS '小型乗務フラグ'
FROM TKD_Koban AS KOBAN
LEFT JOIN TKD_Kikyuj AS KIKYUJ ON KIKYUJ.KinKyuTblCdSeq = KOBAN.KinKyuTblCdSeq
        AND KIKYUJ.SyainCdSeq = KOBAN.SyainCdSeq
        AND KIKYUJ.SiyoKbn = 1
LEFT JOIN VPM_KinKyu AS KINKYU ON KINKYU.KinKyuCdSeq = KIKYUJ.KinKyuCdSeq
LEFT JOIN VPM_KyoSHe AS KYOSHE ON KYOSHE.SyainCdSeq = KOBAN.SyainCdSeq
        AND KYOSHE.StaYmd <= KOBAN.UnkYmd
        AND KYOSHE.EndYmd >= KOBAN.UnkYmd
WHERE KOBAN.UnkYmd >= @StaYmd                        
        AND KOBAN.UnkYmd <= @EndYmd                       
        AND KOBAN.SyainCdSeq IN (SELECT value FROM STRING_SPLIT(@SyainSeqList, ','))
ORDER BY KOBAN.UnkYmd

SET	@ROWCOUNT	=	@@ROWCOUNT
END
