USE [HOC_Kashikiri_New]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetKouteiAndTeHai_R]    Script Date: 9/25/2020 9:04:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Pro_GetKouteiAndTeHai_R]   
	@Ukeno NCHAR(15),
	@SyuKoYmd CHAR(10),
	@KikYmd CHAR(10),
	@TeiDanNo SMALLINT
AS   
		CREATE TABLE #Datetmp(RowID  INT IDENTITY(1,1),datehaisha DATE,TomKbn TINYINT, Nittei SMALLINT)
		DECLARE @CheckAddSyuKoYmd INT =0, 
				@CheckAddKiKYmd INT =0
		DECLARE @SyuKodate  DATE
				,@Kikdate  DATE
				,@dtStart DATE
				,@dtpara DATE
				,@i INT
				,@row_number INT
				,@totalRecords INT
				,@totalRecordstmp INT 
		DECLARE @TomKbn TINYINT
				, @Nittei SMALLINT
				,@date DATE
		DECLARE @datehaisha DATE,
				@Koutei NVARCHAR(102)
				,@TehNm NVARCHAR(102)
				,@TehTel CHAR(14)
				,@TehRen INT
		DECLARE @CountRecordKotei  INT=0
			   ,@CountRecordTehai  INT=0
		DECLARE @strSQL NVARCHAR(MAX)=''
			   ,@Condition VARCHAR(20)=''
			   
		SET @SyuKodate=CONVERT(DATETIME, @SyuKoYmd, 111)
		SET @Kikdate=CONVERT(DATETIME, @KikYmd, 111)
		SET @dtStart=@SyuKodate
		SET @i=1
		WHILE @SyuKodate <= @Kikdate    --20200920 < 20200923
			BEGIN
				IF (SELECT UNKOBI.ZenHaFlg 
					FROM TKD_Unkobi AS UNKOBI 
					WHERE UNKOBI.UkeNo=@Ukeno)=1
					AND (
					SELECT DATEADD(DAY,-1,CONVERT(DATETIME, HaiSYmd, 111)) 
					FROM TKD_Unkobi AS UNKOBI 
					WHERE UNKOBI.UkeNo=@Ukeno ) = CONVERT(DATETIME, @SyuKodate, 111)
					BEGIN
						INSERT INTO #Datetmp VALUES (@SyuKodate,2,1)
						SELECT @SyuKodate = DATEADD(DAY,1,@SyuKodate)
					END
				ELSE  IF (SELECT UNKOBI.KhakFlg 
						  FROM TKD_Unkobi AS UNKOBI 
						  WHERE UNKOBI.UkeNo=@Ukeno)=1 
						  AND ( 
						  SELECT DATEADD(DAY,1,CONVERT(DATETIME, TouYmd, 111)) 
						  FROM TKD_Unkobi AS UNKOBI 
						  WHERE UNKOBI.UkeNo=@Ukeno ) = CONVERT(DATETIME, @SyuKodate, 111)
					BEGIN
						INSERT INTO #Datetmp values (@SyuKodate,3,1)
						SELECT @SyuKodate = DATEADD(DAY,1,@SyuKodate)
					END
				ELSE																																																																																																																																																																																																																																																																																
					BEGIN
						INSERT INTO #Datetmp values (@SyuKodate,1,@i)
						SELECT @SyuKodate = DATEADD(DAY,1,@SyuKodate)
						SET @i=@i+1
					END
			END
		SELECT @totalRecords = COUNT(*) FROM #Datetmp --4
		CREATE TABLE #tblKotei(
								datehaisha DATE
								,Koutei NVARCHAR(102)
								, KouRen INT)
		CREATE TABLE #tblTehai(
								datehaisha DATE
								,TehNm NVARCHAR(102)
								,TehTel CHAR(14)
								,TehRen INT
								,RowID  INT IDENTITY(1,1))
		CREATE TABLE #tblTehaitmp(
								datehaisha DATE
								,TehNm NVARCHAR(102)
								,TehTel CHAR(14)
								,TehRen INT
								,RowID  INT IDENTITY(1,1))
		SET @i=1
		WHILE (@i <= @totalRecords)
			BEGIN
				SELECT  @TomKbn=TomKbn,
						@Nittei=Nittei,
						@date=datehaisha
				FROM #Datetmp 
				WHERE RowID=@i
				/*Koutei*/
				INSERT INTO #tblKotei (datehaisha, Koutei,KouRen) 
				SELECT @date
					   ,Koutei
					   ,ROW_NUMBER() OVER(ORDER BY KouRen ASC)
				FROM TKD_Kotei AS KOTEI
				WHERE KOTEI.UkeNo = @Ukeno　　　　				
						AND KOTEI.UnkRen = 1 
						AND KOTEI.TeiDanNo = 0  
						AND KOTEI.BunkRen = 0         	
						AND KOTEI.Nittei=@Nittei
						AND KOTEI.TomKbn=@TomKbn
						AND KOTEI.SiyoKbn = 1
						ORDER BY Nittei,KouRen
				/**TEHAI*/		
				INSERT INTO #tblTehai (datehaisha ,TehNm ,TehTel,TehRen) 
				SELECT @date, TehNm,TehTel,ROW_NUMBER() OVER(ORDER BY TehRen ASC)
				FROM TKD_Tehai AS TEIHAI
				WHERE TEIHAI.UkeNo = @Ukeno　　　　					
						AND TEIHAI.UnkRen = 1          					
						AND TEIHAI.TeiDanNo = 0        				
						AND TEIHAI.BunkRen = 0         	
						AND TEIHAI.Nittei=@Nittei
						AND TEIHAI.SiyoKbn = 1
						AND TEIHAI.TomKbn=@TomKbn
	
				SET @i=@i+1
			END 
		SET @i=1
		SELECT @totalRecordstmp = COUNT(*) FROM #tblTehai
		WHILE (@i <= @totalRecordstmp)
			BEGIN
				SELECT  @date=datehaisha
						,@TehNm=TehNm
						,@TehTel=TehTel
						,@TehRen=TehRen
				FROM #tblTehai
				WHERE RowID=@i
				INSERT INTO #tblTehai values (@date,@TehNm,'',@TehRen*2-1)
				INSERT INTO #tblTehai values (@date,@TehTel,'',@TehRen*2)
				DELETE  FROM #tblTehai WHERE RowID=@i
				SET @i=@i+1
			END
		SET @i=1
		INSERT INTO #tblTehaitmp (datehaisha ,TehNm ,TehTel,TehRen)
		SELECT tbl.datehaisha ,tbl.TehNm ,tbl.TehTel,tbl.TehRen 
		FROM #tblTehai AS tbl 
		ORDER BY tbl.RowID
		DECLARE @tblsubreport TABLE(DateKotei DATE,Koutei NVARCHAR(102),TehNm NVARCHAR(102),TehTel CHAR(14),DateShow NVARCHAR(20))
		WHILE (@i <= @totalRecords)
			BEGIN
				SELECT @date=datehaisha
				FROM #Datetmp
				WHERE RowID=@i
				IF(SELECT COUNT(*) FROM #tblKotei WHERE datehaisha=@date)=0 AND (SELECT COUNT(*) FROM #tblTehaitmp WHERE datehaisha=@date)=0
					BEGIN
						SET @i=@i+1
					END
				ELSE
					BEGIN
						SELECT @CountRecordKotei = COUNT(*) FROM #tblKotei WHERE datehaisha=@date
						SELECT @CountRecordTehai = COUNT(*) FROM #tblTehaitmp WHERE datehaisha=@date
						SET @Condition = IIF(@CountRecordKotei>=@CountRecordTehai,'LEFT','RIGHT')
						SET @strSQL=''
						SET @strSQL = @strSQL + 
							'SELECT ISNULL(Kotei.datehaisha,Tehai.datehaisha) ,'	+
							'	 ISNULL(Kotei.Koutei,''''),'	+
							'    ISNULL(Tehai.TehNm,''''),'	+
							'	 ISNULL(Tehai.TehTel,''''), '	+ 
							'	 FORMAT(ISNULL(Kotei.datehaisha,Tehai.datehaisha), ''MM/dd'')  '+     
							'FROM #tblKotei AS Kotei '+ @Condition + ' JOIN #tblTehaitmp AS Tehai   
								ON Kotei.datehaisha=Tehai.datehaisha AND Kotei.KouRen=Tehai.TehRen  
							 WHERE Kotei.datehaisha='''+CONVERT(VARCHAR, @date, 23)+''' or Tehai.datehaisha='''+CONVERT(VARCHAR, @date, 23)+''''
						INSERT INTO @tblsubreport EXEC(@strSQL)
						SET @i=@i+1
					END
			END
		SELECT * FROM @tblsubreport
GO


