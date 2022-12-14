USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSchYoteiGroupFb_R]    Script Date: 12/15/2020 3:01:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dSchYoteiGroupFb_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from schyotei table for group feedback
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get data from schyotei table for group feedback with conditions
------------------------------------------------------------
CREATE OR ALTER          PROCEDURE [dbo].[PK_dSchYoteiGroupFb_R]
		(
		--Parameter
			@EmployeeId			INT,
			@GroupId			INT,
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		
		SELECT
    TKD_SchYotei.YoteiSeq, --予定SEQ
    ISNULL(TKD_SchYotKSyaFb.YoteiSYmd, '') AS YoteiSYmd, --予定開始年月日
    ISNULL(TKD_SchYotKSyaFb.YoteiSTime, '') AS YoteiSTime, --予定開始時間
    ISNULL(TKD_SchYotKSyaFb.KuriKbn, 1) AS KuriKbn, --繰り返し区分
    (
        SELECT
            eTKD_SchYotKSya01.SyainCdSeq AS SyainCdSeq,
            eVPM_Syain01.SyainNm AS SyainNm,
            ISNULL(
                eTKD_SchYotKSyaFb01.AcceptKbn,
                ISNULL(eTKD_SchYotKSyaFb02.AcceptKbn, 0)
            ) AS AcceptKbn
        FROM
            TKD_SchYotKSya AS eTKD_SchYotKSya01
            LEFT JOIN VPM_Syain AS eVPM_Syain01 ON eVPM_Syain01.SyainCdSeq = eTKD_SchYotKSya01.SyainCdSeq
            LEFT JOIN TKD_SchYotKSyaFb AS eTKD_SchYotKSyaFb01 ON eTKD_SchYotKSyaFb01.YoteiSeq = eTKD_SchYotKSya01.YoteiSeq
            AND eTKD_SchYotKSyaFb01.SyainCdSeq = eTKD_SchYotKSya01.SyainCdSeq
            AND eTKD_SchYotKSyaFb01.YoteiSYmd = TKD_SchYotKSyaFb.YoteiSYmd
            AND eTKD_SchYotKSyaFb01.YoteiSTime = TKD_SchYotKSyaFb.YoteiSTime
            AND eTKD_SchYotKSyaFb01.KuriKbn = TKD_SchYotKSyaFb.KuriKbn
            LEFT JOIN TKD_SchYotKSyaFb AS eTKD_SchYotKSyaFb02 ON eTKD_SchYotKSyaFb02.YoteiSeq = eTKD_SchYotKSya01.YoteiSeq
            AND eTKD_SchYotKSyaFb02.SyainCdSeq = eTKD_SchYotKSya01.SyainCdSeq
            AND eTKD_SchYotKSyaFb02.KuriKbn = 1
        WHERE
            eTKD_SchYotKSya01.YoteiSeq = TKD_SchYotei.YoteiSeq FOR JSON PATH
    ) AS EventParticipant --予定参加者
FROM
    TKD_SchYotKSya
    LEFT JOIN TKD_SchYotei ON TKD_SchYotKSya.YoteiSeq = TKD_SchYotei.YoteiSeq
    LEFT JOIN TKD_SchYotKSyaFb ON TKD_SchYotKSyaFb.YoteiSeq = TKD_SchYotei.YoteiSeq
    AND (
        TKD_SchYotKSyaFb.KuriKbn = 1
        OR(
            TKD_SchYotKSyaFb.KuriKbn = 2
            AND TKD_SchYotKSyaFb.YoteiSYmd BETWEEN @FromDate
            AND  @ToDate
        )
    )
    LEFT JOIN VPM_Syain ON VPM_Syain.SyainCdSeq = TKD_SchYotKSyaFb.SyainCdSeq
    LEFT JOIN VPM_KyoSHe AS VPM_KyoSHe01 ON VPM_KyoSHe01.SyainCdSeq = TKD_SchYotei.SyainCdSeq
    AND (
        CAST(VPM_KyoSHe01.StaYmd AS DATE) <= CAST(GETDATE() AS DATE)
    )
    AND (
        CAST(VPM_KyoSHe01.EndYmd AS DATE) >= CAST(GETDATE() AS DATE)
    )
    LEFT JOIN VPM_KyoSHe AS VPM_KyoSHe02 ON VPM_KyoSHe02.SyainCdSeq = TKD_SchYotKSya.SyainCdSeq
    AND (
        CAST(VPM_KyoSHe02.StaYmd AS DATE) <= CAST(GETDATE() AS DATE)
    )
    AND (
        CAST(VPM_KyoSHe02.EndYmd AS DATE) >= CAST(GETDATE() AS DATE)
    )
WHERE
    TKD_SchYotei.SiyoKbn = 1
	AND TKD_SchYotei.CalendarSeq = 0
    AND (
       (
          TKD_SchYotei.YoteiShoKbn IN (1, 3)
          AND TKD_SchYotei.SyainCdSeq = @EmployeeId
       )
       OR (
          TKD_SchYotei.YoteiShoKbn = 1
          AND TKD_SchYotei.SyainCdSeq <>  @EmployeeId
       )
    )

    AND (
        (
            TKD_SchYotei.YoteiEYmd >= @FromDate
            AND TKD_SchYotei.YoteiSYmd <= @ToDate
        )
        OR (
            TKD_SchYotei.KuriEndYmd >= @FromDate
            AND TKD_SchYotei.YoteiSYmd <= @ToDate
        )
    )
    AND (
        VPM_KyoSHe01.EigyoCdSeq = @GroupId
        OR VPM_KyoSHe02.EigyoCdSeq = @GroupId
    )
GROUP BY
    TKD_SchYotei.YoteiSeq,
    TKD_SchYotKSyaFb.YoteiSYmd,
    TKD_SchYotKSyaFb.YoteiSTime,
    TKD_SchYotKSyaFb.KuriKbn
ORDER BY
    TKD_SchYotei.YoteiSeq,
    TKD_SchYotKSyaFb.KuriKbn,
    TKD_SchYotKSyaFb.YoteiSYmd,
    TKD_SchYotKSyaFb.YoteiSTime



		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN