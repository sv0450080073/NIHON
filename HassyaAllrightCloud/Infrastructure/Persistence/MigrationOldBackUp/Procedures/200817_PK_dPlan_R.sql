USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dPlan_R]    Script Date: 09/14/2020 2:25:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   Pk_dPlan_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Plan data List
-- Date			:   2020/08/17
-- Author		:   N.N.T.AN
-- Description	:   Get plan data list with conditions
------------------------------------------------------------
CREATE OR ALTER       PROCEDURE [dbo].[PK_dPlan_R]
		(
		--Parameter
			@GroupId			INT = NULL,					--Group id
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
SELECT DISTINCT TKD_SchYotei.YoteiSeq,
                VPM_CodeKb01.RyakuNm --予定種別
 ,
                TKD_SchYotei.TukiLabKbn --付きラベル区分
 ,
                TKD_SchYotei.Title --タイトル
 ,
                TKD_SchYotei.YoteiSYmd --予定開始年月日
 ,
                TKD_SchYotei.YoteiSTime --予定開始時間
 ,
                TKD_SchYotei.YoteiEYmd --予定終了年月日
 ,
                TKD_SchYotei.YoteiETime --予定終了時間
 ,
                TKD_SchYotei.AllDayKbn --終日区分
 ,
                TKD_SchYotei.KuriRule --繰り返しルール
 ,
                TKD_SchYotei.KuriReg --繰り返し例外
 ,
                TKD_SchYotei.GaiKkKbn --外部公開区分
 ,
                TKD_SchYotei.SyainCdSeq ,
                VPM_Syain01.SyainCd --作成者の社員コード
 ,
                VPM_Syain01.SyainNm --作成者の社員名
 ,
                TKD_SchYotei.YoteiBiko --予定備考

FROM TKD_SchYotei --予定種別取得

LEFT JOIN VPM_CodeKb AS VPM_CodeKb01 ON TKD_SchYotei.YoteiType = VPM_CodeKb01.CodeKbnSeq --社員情報取得

JOIN VPM_Syain AS VPM_Syain01 ON VPM_Syain01.SyainCdSeq = TKD_SchYotei.SyainCdSeq --作成者の営業所取得

LEFT JOIN VPM_KyoSHe AS VPM_KyoSHe01 ON VPM_KyoSHe01.SyainCdSeq = TKD_SchYotei.SyainCdSeq
AND (CAST(VPM_KyoSHe01.StaYmd AS DATE) <= CAST(GETDATE() AS DATE))
AND (CAST(VPM_KyoSHe01.EndYmd AS DATE) >= CAST(GETDATE() AS DATE) )--参加者の営業所取得

LEFT JOIN TKD_SchYotKSya AS TKD_SchYotKSya02 ON TKD_SchYotKSya02.YoteiSeq = TKD_SchYotei.YoteiSeq
LEFT JOIN VPM_KyoSHe AS VPM_KyoSHe02 ON VPM_KyoSHe02.SyainCdSeq = TKD_SchYotKSya02.SyainCdSeq
AND (CAST(VPM_KyoSHe02.StaYmd AS DATE) <= CAST(GETDATE() AS DATE))
AND (CAST(VPM_KyoSHe02.EndYmd AS DATE) >= CAST(GETDATE() AS DATE))
WHERE TKD_SchYotei.SiyoKbn = 1
  AND TKD_SchYotei.YoteiShoKbn = 1 --未承認の休暇申請のみ

  AND (TKD_SchYotei.YoteiEYmd >= @FromDate
       AND TKD_SchYotei.YoteiSYmd <= @ToDate
       OR TKD_SchYotei.KuriEndYmd >= @FromDate
       AND TKD_SchYotei.YoteiSYmd <= @ToDate) 
  AND (VPM_KyoSHe01.EigyoCdSeq = @GroupId
       OR VPM_KyoSHe02.EigyoCdSeq = @GroupId)

		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN