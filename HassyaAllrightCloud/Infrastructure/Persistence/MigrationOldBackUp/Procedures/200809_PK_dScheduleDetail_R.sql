USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dScheduleDetail_R]    Script Date: 09/08/2020 9:21:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dScheduleDetail_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Schedule detail data List
-- Date			:   2020/09/08
-- Author		:   N.N.T.AN
-- Description	:   Get schedule data detail list with conditions
------------------------------------------------------------
CREATE or ALTER     PROCEDURE [dbo].[PK_dScheduleDetail_R]
		(
		--Parameter
			@ScheduleId			INT = NULL,					--Schedule id
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		SELECT																																								
		 TKD_SchYotei.YoteiSeq AS YoteiSeq -- 予定コードSEQ																																								
		,VPM_Syain01.SyainCd AS SyainCd -- 社員コード																																								
		,VPM_Syain01.SyainNm AS SyainNm -- 社員名																																								
		,CONCAT(VPM_Compny01.RyakuNm, '　', VPM_Eigyos01.RyakuNm) AS EigyoNm -- 営業所																																								
		,TKD_SchYotei.UpdYmd AS UpdYmd -- 更新年月日																																								
		,TKD_SchYotei.UpdTime AS UpdTime -- 更新時間																																								
		,VPM_CodeKb01.CodeKbnNm AS YoteiTypeNm -- 申請種別																																								
		,VPM_KinKyu.KinKyuNm AS KinKyuNm -- 休日種別																																								
		,TKD_SchYotei.Title AS Title -- タイトル																																								
		,TKD_SchYotei.TukiLabKbn AS TukiLabKbn -- 付きラベル																																								
		,TKD_SchYotei.YoteiBiko AS YoteiBiko -- 詳細																																								
		,TKD_SchYotei.YoteiSYmd AS YoteiSYmd -- 予定開始年月日																																								
		,TKD_SchYotei.YoteiSTime AS YoteiSTime -- 予定開始時間																																								
		,TKD_SchYotei.YoteiEYmd AS YoteiEYmd -- 予定終了年月日																																								
		,TKD_SchYotei.YoteiETime AS YoteiETime -- 予定終了時間																																								
		,TKD_SchYotei.AllDayKbn AS AllDayKbn -- 終日区分																																								
		,TKD_SchYotei.YoteiShoKbn AS YoteiShoKbn -- 承認区分																																								
		,TKD_SchYotei.ShoRejBiko AS ShoRejBiko -- 承認者の備考																																								
		,VPM_Syain02.SyainCd AS ShoSyainCd -- 承認者の社員コード																																								
		,VPM_Syain02.SyainNm AS ShoSyainNm -- 承認者の社員名																																								
		,TKD_SchYotei.ShoUpdYmd AS ShoUpdYmd -- 承認年月日																																								
		,TKD_SchYotei.ShoUpdTime AS ShoUpdTime -- 承認時間																																								
		FROM																																								
		    TKD_SchYotei --申請者情報の取得																																								
		    LEFT JOIN VPM_Syain AS VPM_Syain01 ON VPM_Syain01.SyainCdSeq = TKD_SchYotei.SyainCdSeq --申請者の営業所の取得																																								
		    LEFT JOIN VPM_KyoSHe AS VPM_KyoSHe01 ON VPM_KyoSHe01.SyainCdSeq = TKD_SchYotei.SyainCdSeq																																								
		    AND (																																								
		        CAST(VPM_KyoSHe01.StaYmd AS DATE) <= CAST(GETDATE() AS DATE)																																								
		    )																																								
		    AND (																																								
		        CAST(VPM_KyoSHe01.EndYmd AS DATE) >= CAST(GETDATE() AS DATE)																																								
		    )																																								
		    LEFT JOIN VPM_Eigyos AS VPM_Eigyos01 ON VPM_Eigyos01.EigyoCdSeq = VPM_KyoSHe01.EigyoCdSeq																																								
		    AND VPM_Eigyos01.SiyoKbn = 1																																								
		    LEFT JOIN VPM_Compny AS VPM_Compny01 ON VPM_Compny01.CompanyCdSeq = VPM_Eigyos01.CompanyCdSeq																																								
		    AND VPM_Compny01.SiyoKbn = 1 --予定種別の取得																																								
		    LEFT JOIN VPM_CodeKb AS VPM_CodeKb01 ON VPM_CodeKb01.CodeKbnSeq = TKD_SchYotei.YoteiType																																								
		    AND VPM_CodeKb01.SiyoKbn = 1 --休日種別の取得																																								
		    LEFT JOIN VPM_KinKyu ON VPM_KinKyu.KinKyuCdSeq = TKD_SchYotei.KinKyuCdSeq																																								
		    AND VPM_KinKyu.SiyoKbn = 1 --承認者情報の取得																																								
		    LEFT JOIN VPM_Syain AS VPM_Syain02 ON VPM_Syain02.SyainCdSeq = TKD_SchYotei.ShoSyainCdSeq																																								
		WHERE																																								
		    TKD_SchYotei.YoteiSeq = @ScheduleId																																					
			
		
		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN