USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pk_dWorkHourCustomGr_R]    Script Date: 08/24/2020 10:59:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   Pk_dWorkHourCustomGr_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Work hour custom group data List
-- Date			:   2020/08/19
-- Author		:   N.N.T.AN
-- Description	:   Get work hour custom group data list with conditions
------------------------------------------------------------
CREATE  or ALTER  PROCEDURE [dbo].[Pk_dWorkHourCustomGr_R]
		(
		--Parameter
			@GroupId			INT = NULL,					--Group id
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date
			@FromDate4W			VARCHAR(50),				--From Date Get 4 Week
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
			SELECT TKD_Kikyuj01.SyainCdSeq AS SyainCdSeq --社員コード
 ,
       SUM(datediff(HOUR, TKD_Kikyuj01.KinKyuSDateTime, TKD_Kikyuj01.KinKyuEDateTime)) AS WorkHour -- 拘束時間
 ,
       '4WK' AS TYPE
FROM
  (SELECT SyainCdSeq,
          CASE
              WHEN KinKyuSYmd < @FromDate4W THEN @FromDate4W + ' 00:00'
              ELSE STUFF(STUFF(KinKyuSYmd, 5, 0, '/'), 8, 0, '/') + ' ' + STUFF(KinKyuSTime, 3, 0, ':')
          END AS KinKyuSDateTime,
          CASE
              WHEN KinKyuSYmd > @ToDate THEN @ToDate + ' 23:59'
              ELSE STUFF(STUFF(KinKyuEYmd, 5, 0, '/'), 8, 0, '/') + ' ' + STUFF(KinKyuETime, 3, 0, ':')
          END AS KinKyuEDateTime
   FROM TKD_Kikyuj
   WHERE SiyoKbn = 1
     AND KinKyuEYmd >= @FromDate
     AND KinKyuSYmd <= @ToDate ) AS TKD_Kikyuj01 --社員情報取得

JOIN TKD_SchCusGrpMem ON TKD_SchCusGrpMem.SyainCdSeq = TKD_Kikyuj01.SyainCdSeq
AND TKD_SchCusGrpMem.CusGrpSeq = @GroupId GROUP BY TKD_Kikyuj01.SyainCdSeq
UNION
SELECT TKD_Kikyuj01.SyainCdSeq AS SyainCdSeq --社員コード
 ,
       SUM(datediff(HOUR, TKD_Kikyuj01.KinKyuSDateTime, TKD_Kikyuj01.KinKyuEDateTime)) AS WorkHour -- 拘束時間
 ,
       '1WK' AS TYPE
FROM
  (SELECT SyainCdSeq,
          CASE
              WHEN KinKyuSYmd < @FromDate THEN @FromDate + ' 00:00'
              ELSE STUFF(STUFF(KinKyuSYmd, 5, 0, '/'), 8, 0, '/') + ' ' + STUFF(KinKyuSTime, 3, 0, ':')
          END AS KinKyuSDateTime,
          CASE
              WHEN KinKyuSYmd > @ToDate THEN @ToDate + ' 23:59'
              ELSE STUFF(STUFF(KinKyuEYmd, 5, 0, '/'), 8, 0, '/') + ' ' + STUFF(KinKyuETime, 3, 0, ':')
          END AS KinKyuEDateTime
   FROM TKD_Kikyuj
   WHERE SiyoKbn = 1
     AND KinKyuEYmd >= @FromDate
     AND KinKyuSYmd <= @ToDate ) AS TKD_Kikyuj01 --社員情報取得

JOIN TKD_SchCusGrpMem ON TKD_SchCusGrpMem.SyainCdSeq = TKD_Kikyuj01.SyainCdSeq
AND TKD_SchCusGrpMem.CusGrpSeq = @GroupId GROUP BY TKD_Kikyuj01.SyainCdSeq
						
	
	UNION

				SELECT																																					
			    TKD_Haiin.SyainCdSeq as SyainCdSeq,																																					
			    SUM (CAST(TKD_Shabni.KoskuTime AS int)) as WorkHour -- 拘束時間																																					
			,																																					
			    '4WK' AS Type -- 4週間の拘束時間																																					
			FROM																																					
			    TKD_Haiin --車輛別日報取得																																					
			    JOIN TKD_Shabni ON TKD_Shabni.UkeNo = TKD_Haiin.UkeNo																																					
			    AND TKD_Shabni.UnkRen = TKD_Haiin.UnkRen																																					
			    AND TKD_Shabni.TeiDanNo = TKD_Haiin.TeiDanNo																																					
			    AND TKD_Shabni.BunkRen = TKD_Haiin.BunkRen																																					
			    AND TKD_Shabni.SiyoKbn = 1 --カスタムグループ情報取得																																					
			    JOIN TKD_SchCusGrpMem ON TKD_SchCusGrpMem.SyainCdSeq = TKD_Haiin.SyainCdSeq																																					
			    AND TKD_SchCusGrpMem.CusGrpSeq =  @GroupId																																				
			WHERE																																					
			    TKD_Haiin.SiyoKbn = 1																																					
			    AND TKD_Shabni.UnkYmd BETWEEN @FromDate4W																																				
			    AND @ToDate																																					
			GROUP BY																																					
			    TKD_Haiin.SyainCdSeq																																					
			UNION																																					
			SELECT																																					
			    TKD_Haiin.SyainCdSeq as SyainCdSeq,																																					
			    SUM (CAST(TKD_Shabni.KoskuTime AS int)) as WorkHour -- 拘束時間																																					
			,																																					
			    '1WK' AS Type -- 1週間の拘束時間																																					
			FROM																																					
			    TKD_Haiin --車輛別日報取得																																					
			    JOIN TKD_Shabni ON TKD_Shabni.UkeNo = TKD_Haiin.UkeNo																																					
			    AND TKD_Shabni.UnkRen = TKD_Haiin.UnkRen																																					
			    AND TKD_Shabni.TeiDanNo = TKD_Haiin.TeiDanNo																																					
			    AND TKD_Shabni.BunkRen = TKD_Haiin.BunkRen																																					
			    AND TKD_Shabni.SiyoKbn = 1 --カスタムグループ情報取得																																					
			    JOIN TKD_SchCusGrpMem ON TKD_SchCusGrpMem.SyainCdSeq = TKD_Haiin.SyainCdSeq																																					
			    AND TKD_SchCusGrpMem.CusGrpSeq =  @GroupId																																					
			WHERE																																					
			    TKD_Haiin.SiyoKbn = 1																																					
			    AND TKD_Shabni.UnkYmd BETWEEN @FromDate																																				
			    AND @ToDate																																					
			GROUP BY																																					
			    TKD_Haiin.SyainCdSeq																																																																																																																					

		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN