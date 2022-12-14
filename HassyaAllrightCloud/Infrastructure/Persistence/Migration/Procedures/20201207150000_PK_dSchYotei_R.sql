USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSchYotei_R]    Script Date: 12/14/2020 3:22:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dSchYotei_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from schyotei table
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get data from schyotei table with conditions
------------------------------------------------------------
CREATE OR ALTER      PROCEDURE [dbo].[PK_dSchYotei_R]
		(
		--Parameter
			@EmployeeId			INT,
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		SELECT
    eTKD_SchYotei01.YoteiSeq AS YoteiSeq,  --予定SEQ
    MIN(eTKD_SchYotei01.YoteiType) AS YoteiType,          --予定種別区分      
    MIN(eVPM_CodeKb01.RyakuNm) AS RyakuNm,              --予定種別名
    MIN(eTKD_SchYotei01.KinKyuCdSeq) AS KinKyuCdSeq,        --勤務休日種別コードＳＥＱ
    MIN(ISNULL(eTKD_SchYotei01.CalendarSeq, 0)) AS CalendarSeq,        --会社カレンダー
    MIN(eTKD_SchYotei01.SyainCdSeq) AS SyainCdSeq,         --作成社員SEQ 
    MIN(ISNULL(eVPM_Syain01.SyainCd, '')) AS SyainCd, --作成社員コード
    MIN(ISNULL(eVPM_Syain01.SyainNm, '')) AS SyainNm, --作成社員名
    MIN(eTKD_SchYotei01.KinKyuTblCdSeq) AS KinKyuTblCdSeq,     --勤務休日テーブルＳＥＱ 
    MIN(eTKD_SchYotei01.Title) AS Title,              --タイトル
    MIN(eTKD_SchYotei01.YoteiSYmd) AS YoteiSYmd,          --開始年月日
    MIN(eTKD_SchYotei01.YoteiSTime) AS YoteiSTime,         --開始時間
    MIN(eTKD_SchYotei01.YoteiEYmd) AS YoteiEYmd,          --終了年月日
    MIN(eTKD_SchYotei01.YoteiETime) AS YoteiETime,         --終了時間
    MIN(eTKD_SchYotei01.TukiLabKbn) AS TukiLabKbn,         --付きラベル区分 
    MIN(eTKD_SchYotei01.AllDayKbn) AS AllDayKbn,          --終日区分
    MIN(eTKD_SchYotei01.KuriKbn) AS KuriKbn,            --繰り返し区分  
    MIN(eTKD_SchYotei01.KuriRule) AS KuriRule,           --繰り返しルール 
    MIN(eTKD_SchYotei01.KuriReg) AS KuriReg,            --繰り返し例外  
    MIN(eTKD_SchYotei01.GaiKkKbn) AS GaiKkKbn ,          --外部公開区分
    MIN(eTKD_SchYotei01.KuriEndYmd) AS KuriEndYmd,         --繰り返し終了年月日
    MIN(eTKD_SchYotei01.YoteiBiko) AS YoteiBiko,          --予定備考
    MIN(eTKD_SchYotei01.YoteiShoKbn) AS YoteiShoKbn,        --予定承認区分  
    MIN(eTKD_SchYotei01.ShoSyainCdSeq) AS ShoSyainCdSeq,      --承認者の社員コードＳＥＱ  
    MIN(ISNULL(eVPM_Syain02.SyainCd, '')) AS ShoSyainCd, --承認者の社員コード
    MIN(ISNULL(eVPM_Syain02.SyainNm, '')) AS ShoSyainNm, --承認者の社員名
    MIN(eTKD_SchYotei01.ShoUpdYmd) AS ShoUpdYmd,          --承認年月日
    MIN(eTKD_SchYotei01.ShoUpdTime) AS ShoUpdTime,         --承認時間
    MIN(eTKD_SchYotei01.ShoRejBiko) AS ShoRejBiko,          --承認者の備考  
    CASE
        WHEN STRING_AGG(eTKD_SchYotKSya01.SyainCdSeq, ',') IS NOT NULL 
             THEN CONCAT(MIN(eTKD_SchYotei01.SyainCdSeq), ',', STRING_AGG(eTKD_SchYotKSya01.SyainCdSeq, ','))
        ELSE CAST(MIN(eTKD_SchYotei01.SyainCdSeq) AS VARCHAR)
    END AS YotKSya --予定関係者

FROM
    TKD_SchYotei AS eTKD_SchYotei01
    LEFT JOIN VPM_Syain AS eVPM_Syain01 ON eVPM_Syain01.SyainCdSeq = eTKD_SchYotei01.SyainCdSeq
    LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeKbnSeq = eTKD_SchYotei01.YoteiType
    LEFT JOIN TKD_SchYotKSya AS eTKD_SchYotKSya01 ON eTKD_SchYotKSya01.YoteiSeq = eTKD_SchYotei01.YoteiSeq
    LEFT JOIN VPM_Syain AS eVPM_Syain02 ON eVPM_Syain02.SyainCdSeq = eTKD_SchYotei01.ShoSyainCdSeq
    AND eTKD_SchYotei01.ShoSyainCdSeq IS NOT NULL
WHERE
    eTKD_SchYotei01.SiyoKbn = 1
    AND eTKD_SchYotei01.YoteiShoKbn <> 2 --未承認または却下の休暇申請
    AND (
        (
            eTKD_SchYotei01.YoteiEYmd >= @FromDate
            AND eTKD_SchYotei01.YoteiSYmd <= @ToDate
        )
        OR (
            eTKD_SchYotei01.KuriEndYmd >= @FromDate
            AND eTKD_SchYotei01.YoteiSYmd <= @ToDate
        )
    )
    AND (
       eTKD_SchYotei01.SyainCdSeq = @EmployeeId
        OR eTKD_SchYotKSya01.SyainCdSeq = @EmployeeId
        OR eTKD_SchYotei01.CalendarSeq <> 0
    )
GROUP BY
    eTKD_SchYotei01.YoteiSeq
		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN