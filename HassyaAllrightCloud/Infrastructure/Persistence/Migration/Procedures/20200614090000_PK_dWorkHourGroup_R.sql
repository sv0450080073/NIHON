USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dWorkHourGroup_R]    Script Date: 12/15/2020 10:06:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dWorkHourGroup_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get work hour data for group 
-- Date			:   2020/12/15
-- Author		:   N.N.T.AN
-- Description	:   Get work hour data for group with conditions
------------------------------------------------------------
CREATE OR ALTER          PROCEDURE [dbo].[PK_dWorkHourGroup_R]
		(
		--Parameter
			@GroupId			INT,
			@Previous27DayToDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date,
			@TenantCdSeq		int,
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		
SELECT
    TKD_Koban.SyainCdSeq AS SyainCdSeq --社員コードＳＥＱ
    ,TKD_Koban.UnkYmd AS UnkYmd --運行年月日
    ,TKD_Koban.SyukinTime AS SyukinTime --出勤年月日
    ,TKD_Koban.TaiknTime AS TaiknTime --退勤年月日
    ,TKD_Koban.KouSTime AS KouSTime --拘束時間
FROM
    TKD_Koban
    JOIN VPM_KyoSHe ON VPM_KyoSHe.SyainCdSeq = TKD_Koban.SyainCdSeq
    AND (
        CAST(VPM_KyoSHe.StaYmd AS DATE) <= CAST(GETDATE() AS DATE)
    )
    AND (
        CAST(VPM_KyoSHe.EndYmd AS DATE) >= CAST(GETDATE() AS DATE)
    )
    LEFT JOIN TKD_Kikyuj ON TKD_Kikyuj.KinKyuTblCdSeq = TKD_Koban.KinKyuTblCdSeq
    AND TKD_Kikyuj.SiyoKbn = 1
    LEFT JOIN VPM_KinKyu ON TKD_Kikyuj.KinKyuCdSeq = VPM_KinKyu.KinKyuCdSeq
    AND VPM_KinKyu.KinKyuKbn = 1
    AND VPM_KinKyu.SiyoKbn = 1
	AND VPM_KinKyu.TenantCdSeq = @TenantCdSeq
WHERE
    (LEN(TRIM(TKD_Koban.UkeNo)) > 0)
    AND TKD_Koban.SyugyoKbn = 1
    AND TKD_Koban.SiyoKbn = 1
    AND TKD_Koban.UnkYmd BETWEEN @Previous27DayToDate 
    AND @ToDate
    AND VPM_KyoSHe.EigyoCdSeq = @GroupId
ORDER BY
    TKD_Koban.SyainCdSeq,
    TKD_Koban.UnkYmd,
    TKD_Koban.SyukinTime

		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN