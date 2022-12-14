USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dAcquisitionProcessOfWorkHolidayInformation_R]    Script Date: 5/17/2021 10:25:46 AM ******/
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
        ,ISNULL(KOBAN.SyainCdSeq, 0) as SyainCdSeq --AS '交番_社員Seq'
        ,ISNULL(KOBAN.KouBnRen, 0) as KouBnRen --AS '交番_交番連番'
        ,ISNULL(KIKYUJ.KinKyuTblCdSeq, 0) as KinKyuTblCdSeq --AS '勤務休日_勤務休日TblSeq'
        ,ISNULL(KINKYU.KinKyuKbn, 0) as KinKyuKbn --AS '勤務休日種別_種別区分'
        ,ISNULL(KINKYU.KyusyutsuKbn, 0) as KyusyutsuKbn --AS '休出勤'
        ,ISNULL(KYOSHE.BigTypeDrivingFlg, 0) as BigTypeDrivingFlg --AS '大型乗務フラグ'
        ,ISNULL(KYOSHE.MediumTypeDrivingFlg, 0) as MediumTypeDrivingFlg --AS '中型乗務フラグ'
        ,ISNULL(KYOSHE.SmallTypeDrivingFlg, 0) as SmallTypeDrivingFlg --AS '小型乗務フラグ'
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
