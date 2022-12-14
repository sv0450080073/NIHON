USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSchYoteiCustomGroupFb_R]    Script Date: 02/26/2021 3:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dSchYoteiCustomGroupFb_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from schyotei table for custom group feedback
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get data from schyotei table for custom group feedback with conditions
------------------------------------------------------------
CREATE OR ALTER              PROCEDURE [dbo].[PK_dSchYoteiCustomGroupFb_R]
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
    LEFT JOIN TKD_SchCusGrpMem AS TKD_SchCusGrpMem01 ON TKD_SchCusGrpMem01.SyainCdSeq = TKD_SchYotei.SyainCdSeq --参加者のカスタムグループの取得
    LEFT JOIN TKD_SchCusGrpMem AS TKD_SchCusGrpMem02 ON TKD_SchCusGrpMem02.SyainCdSeq = TKD_SchYotKSya.SyainCdSeq --関係者のカスタムグループの取得
WHERE
    TKD_SchYotei.SiyoKbn = 1
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
        TKD_SchCusGrpMem01.CusGrpSeq = @GroupId
        OR TKD_SchCusGrpMem02.CusGrpSeq = @GroupId
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