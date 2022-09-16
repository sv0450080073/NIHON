USE [HOC_Kashikiri_New]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetInfoBooking_R]    Script Date: 12/31/2020 9:06:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
--  System-Name :   
--  Module-Name :  
--  SP-ID       :   
--  DB-Name     :   
--  Name        : 
--  Date        :   
--  Author      :   
--  Descriotion :   調整量データのSelect処理
----------------------------------------------------
--  Update      :
--  Comment     :
----------------------------------------------------
ALTER PROCEDURE [dbo].[Pro_GetInfoBooking_R]
    (
		@DateBooking VARCHAR(10)
	,	@ListCompany VARCHAR(Max)
	,	@BranchFrom  VARCHAR(10)
	,	@BranchTo    VARCHAR(10)
	,	@BookingTypeList VARCHAR(MAX)
	,	@MihaisyaKbn VARCHAR(255)
	,	@Order VARCHAR(Max)
	,	@TenantCdSeq VARCHAR(10)	
    )
AS
	BEGIN TRY

        DECLARE @strSQL VARCHAR(MAX)=''
		DECLARE @tb_SplitString TABLE (CompanyCdSeq int)
		DECLARE		@CompanyCondition VARCHAR(MAX)=''
				,	@BranchCondition VARCHAR(MAX) =''
				,	@BookingTypeCondition VARCHAR(MAX) =''
				,	@MihaisyaKbnCondition VARCHAR(mAX) = ''
				,	@OrderCondition VARCHAR(MAX) =''
				,	@RowNumberCondition VARCHAR(MAX)=''
		/*Check Param @ListCompany : 0 - CheckAll  */
		--INSERT INTO @tb_SplitString SELECT * FROM dbo.FN_SplitString(@ListCompany,'-')
		SELECT @CompanyCondition = CASE
										WHEN @ListCompany = '0' THEN ''
										ELSE  'AND COMPANY.CompanyCdSeq IN (SELECT * FROM dbo.FN_SplitString('+dbo.FP_SetString(@ListCompany)+',''-''))'
									END
		/*Check Param @BranchFrom,@BranchTo : 0 - CheckAll  */
		SELECT @BranchCondition =	CASE
										WHEN @BranchFrom ='0' OR @BranchTo ='0' THEN ''
										ELSE 'AND HAISHA.SyuEigCdSeq >='+@BranchFrom+'
											  AND HAISHA.SyuEigCdSeq <='+@BranchTo+''
									END
		/*Check Param @BookingTypeFrom,@BookingTypeTo : 0 - CheckAll  */
		--SELECT @BookingTypeCondition = CASE
		--									WHEN @BookingTypeFrom ='0' OR @BookingTypeTo ='0' THEN ''
		--									ELSE 'AND YOYAKUSHO.YoyaKbnSeq >='+@BookingTypeFrom+'
		--									  AND YOYAKUSHO.YoyaKbnSeq <='+@BookingTypeTo+' '
		--								END
		SELECT @BookingTypeCondition = ' AND YOYAKUSHO.YoyaKbnSeq IN (select * from FN_SplitString('+dbo.FP_SetString(@BookingTypeList)+', ''-'')) '

		/*CHECK Param HAISHA.HaiSSryCdSeq*/ 
		SELECT @MihaisyaKbnCondition = CASE 
											WHEN @MihaisyaKbn='未出力'
												THEN ' AND HAISHA.HaiSKbn = 2 '
											WHEN @MihaisyaKbn='出力'
												THEN ' '
											ELSE ''
										END

		/*CHECK Param Order 1: 画面で出力順に「出庫・車両コード順」を指定した場合
							2: 画面で出力順に「出庫・梯団順」を指定した場合	
							3: 画面で出力順に「車両コード順」を指定した場合
							4: 画面で出力順に「車両点呼順」を指定した場合
							5: 画面で出力順に「出庫・乗務員コード順」を指定した場合
							*/
		SELECT @OrderCondition =		CASE
											WHEN @Order='1' 
												THEN ' ORDER BY EIGYOSHO.EigyoCd ASC,
															   SYARYO.SyaRyoCd ASC,
															   HAISHA.SyuKoYmd ASC,								
															   HAISHA.SyuKoTime ASC,								
															   HAISHA.UkeNo ASC,								
															   HAISHA.UnkRen ASC,								
															   HAISHA.TeiDanNo ASC,								
															   HAISHA.BunkRen ASC '	
											WHEN @Order='2' 
												THEN ' ORDER BY EIGYOSHO.EigyoCd ASC,
															   HAISHA.TeiDanNo ASC,
															   HAISHA.UkeNo ASC,
															   HAISHA.UnkRen ASC,
															   HAISHA.BunkRen ASC,
															   HAISHA.SyuKoYmd ASC,
														       HAISHA.SyuKoTime ASC	'
											WHEN  @Order='3'
												 THEN ' ORDER BY EIGYOSHO.EigyoCd ASC,								
																SYARYO.SyaRyoCd ASC,								
																HAISHA.SyuKoYmd ASC,						
																HAISHA.SyuKoTime ASC,						
																HAISHA.UkeNo ASC,						
																HAISHA.UnkRen ASC,						
																HAISHA.TeiDanNo ASC,						
																HAISHA.BunkRen ASC '
											WHEN @Order='4'
												THEN  ' ORDER BY EIGYOSHO.EigyoCd ASC,									
																HENSYA.TenkoNo ASC,								
																HAISHA.UkeNo ASC,									
																HAISHA.UnkRen ASC,									
																HAISHA.BunkRen ASC,									
																HAISHA.SyuKoYmd ASC,									
																HAISHA.SyuKoTime ASC '
										WHEN @Order='5' 
												THEN ' ORDER BY EIGYOSHO.EigyoCd ASC,									
																HAISHA.SyuKoYmd ASC,									
																HAISHA.SyuKoTime ASC,									
																HAISHA.UkeNo ASC,									
																HAISHA.TeiDanNo ASC,									
																HAISHA.UnkRen ASC,									
																HAISHA.BunkRen ASC '	
										ELSE ' '
								END
		/*CHECK ROW NUMBER */
		SELECT @RowNumberCondition =' ROW_NUMBER() OVER ( PARTITION BY EIGYOSHO.EigyoCdSeq '+@OrderCondition+') AS row_Num '
        SET @strSQL = @strSQL+ 
	' SELECT	ISNULL(HAISHA.UkeNo,'''') AS UkeNo '	+
			' ,	ISNULL(HAISHA.UnkRen,0) AS UnkRen '	+
			' ,	ISNULL(HAISHA.BunkRen,0) AS BunkRen '	+
			' ,	ISNULL(COMPANY.CompanyCdSeq,0) AS CompanyCdSeq '	+
			' ,	ISNULL(COMPANY.CompanyCd,0) AS CompanyCd '	+
			' ,	ISNULL(EIGYOSHO.EigyoCdSeq,0) AS EigyoCdSeq '	+
			' ,	ISNULL(EIGYOSHO.EigyoCd,0) AS EigyoCd '	+
			' ,	ISNULL(EIGYOSHO.EigyoNm,'''') AS EigyoNm '	+
			' ,	ISNULL(EIGYOSHO.RyakuNm,'''') AS RyakuNm '	+
			' ,	ISNULL(HAISHA.SyuKoYmd ,'''')AS SyuKoYmd '	+
			' ,	ISNULL(HAISHA.SyuKoTime,'''') AS SyuKoTime '	+
			' ,	ISNULL(HAISHA.HaiSYmd,'''') AS HaiSYmd '	+
			' ,	ISNULL(HAISHA.HaiSTime,'''') AS TouYmd '	+
			' ,	ISNULL(HAISHA.TouYmd,'''') AS TouYmd '	+
			' ,	ISNULL(HAISHA.TouChTime,'''') AS TouChTime '	+
			' ,	ISNULL(HAISHA.KikYmd,'''') AS KikYmd '	+
			' ,	ISNULL(HAISHA.KikTime,'''') AS KikTime '	+
			' ,	ISNULL(HAISHA.GoSya,'''') AS GoSya '	+
			' ,	ISNULL(HAISHA.TeiDanNo,0) AS TeiDanNo '	+
			' ,	ISNULL(HAISHA.IkNm,'''') AS IkNm '	+
			' ,	ISNULL(HAISHA.HaiSNm,'''') AS HaiSNm '	+
			' ,	ISNULL(HAISHA.HaiSKouKCdSeq,0) AS HaiSKouKCdSeq '	+
			' ,	ISNULL(HAISHA.HaiSKouKNm,'''') AS HaiSKouKNm '	+
			' ,	ISNULL(HAISHA.HaiSBinCdSeq,0) AS HaiSBinCdSeq '	+
			' ,	ISNULL(HAISHA.HaiSBinNm,'''') AS HaiSBinNm '	+
			' ,	ISNULL(HAISHA.HaiSSetTime,'''') AS HaiSSetTime '	+
			' ,	ISNULL(HAISHA.TouNm,'''') AS TouNm '	+
			' ,	ISNULL(HAISHA.TouKouKCdSeq,0) AS TouKouKCdSeq '	+
			' ,	ISNULL(HAISHA.TouSKouKNm,'''') AS TouSKouKNm '	+
			' ,	ISNULL(HAISHA.TouBinCdSeq,0) AS TouBinCdSeq '	+
			' ,	ISNULL(HAISHA.TouBinNm,'''') AS TouBinNm '	+
			' ,	ISNULL(HAISHA.TouSetTime,'''') AS TouSetTime '	+
			' ,	ISNULL(UNKOBI.SyuKoTime,'''')AS GioRoiAllBooking '	+
			' ,	ISNULL(UNKOBI.HaiSYmd,'''') AS NgayVeAllBooking '	+
			' ,	ISNULL(UNKOBI.HaiSTime,'''') AS GioVeAllBooking '	+
			' ,	ISNULL(UNKOBI.TouYmd,'''') AS TouYmd '	+
			' ,	ISNULL(UNKOBI.TouChTime,'''') AS TouChTime '	+
			' ,	ISNULL(UNKOBI.KikTime,'''') AS KikTime '	+
			' ,	ISNULL(UNKOBI.DanTaNm,'''') AS DanTaNm	'	+
			' ,	ISNULL(SYASYU.SyaSyuCdSeq,0) AS SyaSyuCdSeq '	+
			' ,	ISNULL(SYASYU.SyaSyuCd,0) AS SyaSyuCd '	+
			' ,	ISNULL(SYASYU.SyaSyuNm ,'''')AS SyaSyuNm '	+
			' ,	ISNULL(SYARYO.SyaRyoCdSeq,0) AS SyaRyoCdSeq '	+
			' ,	ISNULL(SYARYO.SyaRyoCd,0) AS SyaRyoCd '	+
			' ,	ISNULL(SYARYO.SyaRyoNm ,'''')AS SyaRyoNm '	+
			' ,	ISNULL(HENSYA.TenkoNo,'''') AS TenkoNo '	+ 
			' ,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') >= CONVERT(DATETIME, HAISHA.HaiSYmd)
						 AND CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')<=CONVERT(DATETIME, HAISHA.TouYmd)
						THEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.HaiSYmd))+1
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') > CONVERT(DATETIME, HAISHA.HaiSYmd) 
						THEN CONVERT(INT,CONVERT(DATETIME, HAISHA.HaiSYmd) - CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+'))
					ELSE CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.TouYmd))+1
				END AS DayBusRunning '	+
			' ,	CONVERT(INT,CONVERT(DATETIME, HAISHA.TouYmd)-CONVERT(DATETIME, HAISHA.HaiSYmd))+1 AS TotalDayBusRun '	+
			' ,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) 
						 AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1<2, ISNULL(HAISHA.HaiSNm, ''''), ''泊中'')
					ELSE ''''
				END AS VistLocation '	+
			' ,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1<2, ISNULL(HAISHA.HaiSKouKNm, ''''), ''泊中'')
					ELSE ''''
				END AS VistLocationCompact ' +
			' , (SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.SyuKoYmd))=0,HAISHA.SyuKoTime,''''))) AS SyuKoTimeMain ' +
			' , (SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.HaiSYmd))=0,HAISHA.HaiSTime,''''))) AS HaiSTimeMain '+
			' , (SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,ISNULL(HAISHA.KikYmd,'''')))=0,UNKOBI.KikTime,'''')))  AS KikTimeMain '+
			' ,	(SELECT SUM(SyaSyuDai) FROM TKD_YykSyu WHERE UkeNo =HAISHA.UkeNo GROUP BY TKD_YykSyu.UkeNo) AS TotalBus '	+
			' , ' + @RowNumberCondition +''+
		' FROM TKD_Haisha AS HAISHA '	+
		' LEFT JOIN TKD_Yyksho AS YOYAKUSHO ON YOYAKUSHO.UkeNo = HAISHA.UkeNo '	+
		' LEFT JOIN TKD_Unkobi AS UNKOBI ON UNKOBI.UkeNo = HAISHA.UkeNo AND UNKOBI.UnkRen = HAISHA.UnkRen '	+
		' LEFT JOIN VPM_HenSya AS HENSYA ON HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq '	+
		' LEFT JOIN VPM_Eigyos AS EIGYOSHO ON EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq '	+
		' LEFT JOIN VPM_Compny AS COMPANY ON COMPANY.CompanyCdSeq = EIGYOSHO.CompanyCdSeq '	+
		' LEFT JOIN VPM_SyaRyo AS SYARYO ON SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq '	+
		' LEFT JOIN VPM_SyaSyu AS SYASYU ON SYASYU.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq '	+
		' WHERE HAISHA.SiyoKbn = 1 '	+
		'		AND HAISHA.HaiSSryCdSeq <> 0 '	+
		'		AND COMPANY.TenantCdSeq =ISNULL( '+@TenantCdSeq+',0) '	+
		'		AND HAISHA.HaiSYmd <='+dbo.FP_SetString(@DateBooking)+''	+
		'		AND HAISHA.TouYmd >= '+dbo.FP_SetString(@DateBooking) +''	+
				@MihaisyaKbnCondition+ ''	+
				@BookingTypeCondition+ ''	+
				@BranchCondition+ ''	+
				@CompanyCondition+ ''	+
				@OrderCondition
		
		DECLARE @tb_Temp TABLE(   UkeNo VARCHAR(15)
										, UnkRen VARCHAR(10) 
										, BunkRen VARCHAR(10) 
										, CompanyCdSeq VARCHAR(10)
										, CompanyCd VARCHAR(10)
										, EigyoCdSeq VARCHAR(10) 
										, EigyoCd VARCHAR(10)
										, EigyoNm NVARCHAR(255)
										, RyakuNm NVARCHAR(255)
										, SyuKoYmd VARCHAR(8)
										, SyuKoTime VARCHAR(4)
										, HaiSYmd VARCHAR(8)
										, HaiSTime VARCHAR(4)
										, TouYmd VARCHAR(8)
										, TouChTime VARCHAR(4)
										, KikYmd VARCHAR(8)
										, HAISHA_KikTime VARCHAR(4)
										, GoSya VARCHAR(4)
										, TeiDanNo VARCHAR(10)
										, IkNm NVARCHAR(255)
										, HaiSNm NVARCHAR(255)
										, HaiSKouKCdSeq VARCHAR(10)
										, HaiSKouKNm NVARCHAR(255)
										, HaiSBinCdSeq VARCHAR(10)
										, HaiSBinNm NVARCHAR(20)
										, HaiSSetTime NVARCHAR(4)
										, TouNm NVARCHAR(255)
										, TouKouKCdSeq VARCHAR(10)
										, TouSKouKNm NVARCHAR(255)
										, TouBinCdSeq VARCHAR(10)
										, TouBinNm NVARCHAR(255)
										, TouSetTime VARCHAR(4)
										, UNKOBI_SyuKoTime VARCHAR(4)
										, UNKOBI_HaiSYmd VARCHAR(8)
										, UNKOBI_HaiSTime VARCHAR(4)
										, UNKOBI_TouYmd VARCHAR(8)
										, UNKOBI_TouChTime VARCHAR(4)
										, UNKOBI_KikTime VARCHAR(4)
										, UNKOBI_DanTaNm NVARCHAR(100) 
										, SyaSyuCdSeq VARCHAR(10) 
										, SyaSyuCd VARCHAR(10)
										, SyaSyuNm NVARCHAR(255)
										, SyaRyoCdSeq VARCHAR(10) 
										, SyaRyoCd VARCHAR(10)
										, SyaRyoNm NVARCHAR(255)
										, TenkoNo NVARCHAR(255)
										, DayBusRunning VARCHAR(10)
										, TotalDayBusRun VARCHAR(10)
										, VistLocation NVARCHAR(255)
										, VistLocationCompact NVARCHAR(255)
										, SyuKoTimeMain VARCHAR(4)
										, HaiSTimeMain VARCHAR(4)
										, KikTimeMain VARCHAR(4)
										, TotalBus VARCHAR(10)
										, RowNum VARCHAR(10)										
								      ) 		
		INSERT INTO @tb_Temp EXEC(@strSQL)
		SELECT * FROM @tb_Temp
		--select @strSQL       
    END TRY
-- エラー処理
    BEGIN CATCH

    END CATCH

    RETURN
GO

