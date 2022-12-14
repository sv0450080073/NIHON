USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSchedule_R]    Script Date: 02/02/2021 11:45:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   Pk_dSchedule_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Schedule data List
-- Date			:   2020/10/27
-- Author		:   N.N.T.AN
-- Description	:   Get schedule data list with conditions
------------------------------------------------------------
CREATE OR ALTER         PROCEDURE [dbo].[PK_dSchedule_R]
		(
		--Parameter
			@EigyoCdSeq			INT,
			@YoteiSYmd			VARCHAR(max) = '',
			@YoteiEYmd			VARCHAR(max) = '',
			@SyainCdSeq			INT,
			@YoteiShoKbn		VARCHAR(max) = '',
			@CusGrpSeq			INT,
			@Skip				INT,
			@Take				INT,
			@TenantCdSeq		int,
		--Output
			@ROWCOUNT			INT OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
	
		SELECT TKD_SchYotei.YoteiSeq AS YoteiSeq ,
       VPM_Syain01.SyainCd AS SyainCd ,
       VPM_Syain01.SyainNm AS SyainNm ,
       CONCAT(VPM_Compny01.RyakuNm, ' ', VPM_Eigyos01.RyakuNm) AS EigyoNm ,
       VPM_CodeKb01.CodeKbnNm AS YoteiTypeNm ,
       VPM_KinKyu.KinKyuNm AS KinKyuNm ,
       TKD_SchYotei.TukiLabKbn AS TukiLabKbn ,
       TKD_SchYotei.Title AS Title ,
       TKD_SchYotei.YoteiSYmd AS YoteiSYmd ,
       TKD_SchYotei.YoteiSTime AS YoteiSTime ,
       TKD_SchYotei.YoteiEYmd AS YoteiEYmd ,
       TKD_SchYotei.YoteiETime AS YoteiETime ,
       TKD_SchYotei.YoteiShoKbn AS YoteiShoKbn ,
       VPM_Syain02.SyainCd AS ShoSyainCd ,
       VPM_Syain02.SyainNm AS ShoSyainNm ,
       TKD_SchYotei.ShoUpdYmd AS ShoUpdYmd ,
       TKD_SchYotei.ShoUpdTime AS ShoUpdTime,
	   TKD_SchYotei.AllDayKbn AS AllDayKbn,
	   TKD_SchYotei.UpdYmd
into #TempTable FROM TKD_SchYotei
LEFT JOIN VPM_Syain AS VPM_Syain01 ON VPM_Syain01.SyainCdSeq = TKD_SchYotei.SyainCdSeq
LEFT JOIN VPM_KyoSHe AS VPM_KyoSHe01 ON VPM_KyoSHe01.SyainCdSeq = TKD_SchYotei.SyainCdSeq
AND (CAST(VPM_KyoSHe01.StaYmd AS DATE) <= CAST(GETDATE() AS DATE))
AND (CAST(VPM_KyoSHe01.EndYmd AS DATE) >= CAST(GETDATE() AS DATE))
LEFT JOIN VPM_Eigyos AS VPM_Eigyos01 ON VPM_Eigyos01.EigyoCdSeq = VPM_KyoSHe01.EigyoCdSeq
AND VPM_Eigyos01.SiyoKbn = 1
LEFT JOIN VPM_Compny AS VPM_Compny01 ON VPM_Compny01.CompanyCdSeq = VPM_Eigyos01.CompanyCdSeq
AND VPM_Compny01.SiyoKbn = 1
LEFT JOIN VPM_CodeKb AS VPM_CodeKb01 ON VPM_CodeKb01.CodeKbnSeq = TKD_SchYotei.YoteiType
AND VPM_CodeKb01.SiyoKbn = 1
LEFT JOIN VPM_KinKyu ON VPM_KinKyu.KinKyuCdSeq = TKD_SchYotei.KinKyuCdSeq
AND VPM_KinKyu.SiyoKbn = 1
AND VPM_KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_Syain AS VPM_Syain02 ON VPM_Syain02.SyainCdSeq = TKD_SchYotei.ShoSyainCdSeq
WHERE TKD_SchYotei.SiyoKbn = 1
  AND VPM_CodeKb01.CodeKbn = 1
  AND (@TenantCdSeq = 0 OR (VPM_Compny01.TenantCdSeq = @TenantCdSeq))
  AND (@EigyoCdSeq = 0 OR (VPM_Eigyos01.EigyoCdSeq = @EigyoCdSeq))
  AND (@YoteiSYmd = '' OR (TKD_SchYotei.YoteiEYmd >= @YoteiSYmd))
  AND (@YoteiEYmd = '' OR (TKD_SchYotei.YoteiSYmd <= @YoteiEYmd))
  AND (@SyainCdSeq = 0 OR (VPM_Syain01.SyainCdSeq = @SyainCdSeq))
  AND (@YoteiShoKbn = 0 OR (TKD_SchYotei.YoteiShoKbn = @YoteiShoKbn))
  AND (@CusGrpSeq = 0 OR (VPM_Syain01.SyainCdSeq IN (SELECT TKD_SchCusGrpMem.SyainCdSeq FROM TKD_SchCusGrpMem WHERE TKD_SchCusGrpMem.CusGrpSeq = @CusGrpSeq)))

		SELECT	@ROWCOUNT	= CAST(ISNULL(COUNT(*), 0) AS INT) FROM #TempTable
		SELECT * from #TempTable 
	ORDER BY #TempTable.UpdYmd																										

	OFFSET @Skip ROWS FETCH FIRST @Take ROWS only																		

END
RETURN