USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSchedule_R]    Script Date: 09/08/2020 9:21:17 AM ******/
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
-- Date			:   2020/09/08
-- Author		:   N.N.T.AN
-- Description	:   Get schedule data list with conditions
------------------------------------------------------------
CREATE or ALTER     PROCEDURE [dbo].[PK_dSchedule_R]
		(
		--Parameter
			@GroupId			INT = NULL,					--Group id
			@Query				VARCHAR(300) = '',				--Query Condition
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
	DECLARE @strSql varchar(max)
BEGIN
	set @strSql = 'SELECT TKD_SchYotei.YoteiSeq AS YoteiSeq ,
       VPM_Syain01.SyainCd AS SyainCd ,
       VPM_Syain01.SyainNm AS SyainNm ,
       CONCAT(VPM_Compny01.RyakuNm, '' '', VPM_Eigyos01.RyakuNm) AS EigyoNm ,
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
       TKD_SchYotei.ShoUpdTime AS ShoUpdTime
FROM TKD_SchYotei
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
LEFT JOIN VPM_Syain AS VPM_Syain02 ON VPM_Syain02.SyainCdSeq = TKD_SchYotei.ShoSyainCdSeq
WHERE TKD_SchYotei.SiyoKbn = 1
  AND VPM_CodeKb01.CodeKbn = 1'
			SET @strSql = @strSql + 'AND VPM_Compny01.TenantCdSeq = ' + CAST(@GroupId AS varchar) + @query
			EXEC(@strSql)																																
																											
		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN