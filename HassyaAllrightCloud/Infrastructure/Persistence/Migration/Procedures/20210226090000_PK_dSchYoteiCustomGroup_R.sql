USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSchYoteiCustomGroup_R]    Script Date: 02/26/2021 3:09:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dSchYoteiCustomGroup_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from schyotei table for custom group
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get data from schyotei table for custom group with conditions
------------------------------------------------------------
CREATE OR ALTER              PROCEDURE [dbo].[PK_dSchYoteiCustomGroup_R]
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
    TKD_SchYotei.YoteiSeq AS YoteiSeq --予定SEQ
	,MIN(TKD_SchYotei.YoteiType) AS YoteiType
    ,MIN(VPM_CodeKb01.CodeKbn) AS YoteiTypeKbn --予定種別区分
    ,MIN(VPM_CodeKb01.RyakuNm) AS YoteiTypeKNm --予定種別区分名
    ,MIN(TKD_SchYotei.TukiLabKbn) AS TukiLabKbn --付きラベル区分
    ,MIN(TKD_SchYotei.Title) AS Title --タイトル
    ,MIN(TKD_SchYotei.YoteiSYmd) AS YoteiSYmd --予定開始年月日
    ,MIN(TKD_SchYotei.YoteiSTime) AS YoteiSTime --予定開始時間
    ,MIN(TKD_SchYotei.YoteiEYmd) AS YoteiEYmd --予定終了年月日
    ,MIN(TKD_SchYotei.YoteiETime) AS YoteiETime --予定終了時間
    ,MIN(TKD_SchYotei.AllDayKbn) AS AllDayKbn --終日区分
    ,MIN(TKD_SchYotei.KuriRule) AS KuriRule --繰り返しルール
    ,MIN(TKD_SchYotei.KuriReg) AS KuriReg --繰り返し例外
    ,MIN(TKD_SchYotei.GaiKkKbn) AS GaiKkKbn --外部公開区分
    ,MIN(TKD_SchYotei.SyainCdSeq) AS SyainCdSeq --作成者の社員コードSEQ
    ,MIN(VPM_Syain01.SyainCd) AS SyainCd --作成者の社員コード
    ,MIN(VPM_Syain01.SyainNm) AS SyainNm --作成者の社員名
    ,MIN(TKD_SchYotei.GaiKkKbn) AS GaiKkKbn -- 外部公開区分
    ,MIN(TKD_SchYotei.YoteiBiko) AS YoteiBiko --予定備考
    ,CASE
        WHEN STRING_AGG(TKD_SchYotKSya01.SyainCdSeq, ',') IS NOT NULL 
             THEN CONCAT(MIN(TKD_SchYotei.SyainCdSeq), ',', STRING_AGG(TKD_SchYotKSya01.SyainCdSeq, ','))
        ELSE CAST(MIN(TKD_SchYotei.SyainCdSeq) AS VARCHAR)
    END AS YotKSya --予定関係者
    ,MIN(TKD_SchYotei.ShoSyainCdSeq) AS ShoSyainCdSeq --承認者の社員コードＳＥＱ
    ,MIN(ISNULL(VPM_Syain02.SyainCd, '')) AS ShoSyainCd --承認者の社員コード
    ,MIN(ISNULL(VPM_Syain02.SyainNm, '')) AS ShoSyainNm --承認者の社員名
    ,MIN(TKD_SchYotei.ShoUpdYmd) AS ShoUpdYmd --承認年月日
    ,MIN(TKD_SchYotei.ShoUpdTime) AS ShoUpdTime --承認時間
    ,MIN(TKD_SchYotei.ShoRejBiko) AS ShoRejBiko --承認者の備考
FROM
    TKD_SchYotei
    LEFT JOIN VPM_CodeKb AS VPM_CodeKb01 ON TKD_SchYotei.YoteiType = VPM_CodeKb01.CodeKbnSeq
    JOIN VPM_Syain AS VPM_Syain01 ON VPM_Syain01.SyainCdSeq = TKD_SchYotei.SyainCdSeq
    LEFT JOIN TKD_SchCusGrpMem AS TKD_SchCusGrpMem01 ON TKD_SchCusGrpMem01.SyainCdSeq = TKD_SchYotei.SyainCdSeq
    LEFT JOIN TKD_SchYotKSya AS TKD_SchYotKSya01 ON TKD_SchYotKSya01.YoteiSeq = TKD_SchYotei.YoteiSeq
    LEFT JOIN TKD_SchCusGrpMem AS TKD_SchCusGrpMem02 ON TKD_SchCusGrpMem01.SyainCdSeq = TKD_SchYotKSya01.SyainCdSeq
    LEFT JOIN VPM_Syain AS VPM_Syain02 ON VPM_Syain02.SyainCdSeq = TKD_SchYotei.ShoSyainCdSeq
    AND TKD_SchYotei.ShoSyainCdSeq IS NOT NULL
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
            AND TKD_SchYotei.SyainCdSeq <> @EmployeeId
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
        TKD_SchCusGrpMem01.CusGrpSeq = @GroupId
        OR TKD_SchCusGrpMem02.CusGrpSeq = @GroupId
    )
GROUP BY
    TKD_SchYotei.YoteiSeq




		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN