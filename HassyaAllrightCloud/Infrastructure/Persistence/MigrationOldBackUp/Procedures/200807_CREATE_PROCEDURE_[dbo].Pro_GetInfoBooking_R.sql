USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetInfoBooking_R]    Script Date: 8/7/2020 7:51:27 AM ******/
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
--  Descriotion :  
----------------------------------------------------
--  Update      :
--  Comment     :
----------------------------------------------------
CREATE PROCEDURE [dbo].[Pro_GetInfoBooking_R]
    (
		@DateBooking VARCHAR(10)
	,	@ListCompany VARCHAR(Max)
	,	@BranchFrom  VARCHAR(10)
	,	@BranchTo    VARCHAR(10)
	,	@BookingTypeFrom VARCHAR(10)
	,	@BookingTypeTo VARCHAR(10)
	,	@MihaisyaKbn VARCHAR(255)
	,	@Order VARCHAR(Max)
	,	@TenantCdSeq VARCHAR(10)
	,	@ReturnCd      INTEGER OUTPUT          -- リターンコード
	,	@ROWCOUNT      INTEGER OUTPUT          -- 処理件数
	,	@ReturnMsg     VARCHAR(MAX) OUTPUT     -- 処理メッセージ
	,	@eProcedure    VARCHAR(20) OUTPUT      -- エラーオブジェクト
	,	@eLine         VARCHAR(20) OUTPUT      -- エラー行番号
    )
AS
	BEGIN TRY

        DECLARE @strSQL VARCHAR(MAX)=''
		DECLARE @tb_SplitString TABLE (CompanyCdSeq int)
		DECLARE		@CompanyCondition VARCHAR(255)=''
				,	@BranchCondition VARCHAR(255) =''
				,	@BookingTypeCondition VARCHAR(255) =''
				,	@MihaisyaKbnCondition VARCHAR(255) = ''
				,	@OrderCondition VARCHAR(MAX) =''

        SET @ReturnCd   =   0       -- リターンコード初期化
        SET @ROWCOUNT   =   0       -- 処理件数初期化
        SET @ReturnMsg  =   ' '     -- 処理メッセージ初期化
        SET @eProcedure =   ' '     -- エラーオブジェクト初期化
        SET @eLine      =   ' '     -- エラー行番号初期化

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
		SELECT @BookingTypeCondition = CASE
											WHEN @BookingTypeFrom ='0' OR @BookingTypeTo ='0' THEN ''
											ELSE 'AND YOYAKUSHO.YoyaKbnSeq >='+@BookingTypeFrom+'
											  AND YOYAKUSHO.YoyaKbnSeq <='+@BookingTypeTo+''
										END		
		/*CHECK Param HAISHA.HaiSSryCdSeq*/ 
		SELECT @MihaisyaKbnCondition = CASE 
											WHEN @MihaisyaKbn='未出力'
												THEN ' AND HAISHA.HaiSSryCdSeq <> 0'
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
																HAISHA.SyuKoTime ASC'
										WHEN @Order='5' 
												THEN ' ORDER BY EIGYOSHO.EigyoCd ASC,									
																HAISHA.SyuKoYmd ASC,									
																HAISHA.SyuKoTime ASC,									
																HAISHA.UkeNo ASC,									
																HAISHA.TeiDanNo ASC,									
																HAISHA.UnkRen ASC,									
																HAISHA.BunkRen ASC'	
										ELSE ' '
								END
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
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1=1, ISNULL(HAISHA.HaiSNm, ''''), ''泊中'')
					ELSE ''''
				END AS VistLocation '	+
			' ,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1=1, ISNULL(HAISHA.HaiSKouKNm, ''''), ''泊中'')
					ELSE ''''
				END AS VistLocationCompact ' +
			' , (SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.SyuKoYmd))=0,HAISHA.SyuKoTime,''''))) AS SyuKoTimeMain ' +
			' , (SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.HaiSYmd))=0,HAISHA.HaiSTime,''''))) AS HaiSTimeMain '+
			' , (SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,ISNULL(HAISHA.KikYmd,'''')))=0,UNKOBI.KikTime,'''')))  AS KikTimeMain '+
			' ,	(SELECT SUM(SyaSyuDai) FROM LKD_YykSyu WHERE UkeNo =HAISHA.UkeNo GROUP BY LKD_YykSyu.UkeNo) AS TotalBus '	+
			' , ROW_NUMBER() OVER ( PARTITION BY EIGYOSHO.EigyoCdSeq ORDER BY EIGYOSHO.EigyoCd) row_Num '	+
		' FROM TKD_Haisha AS HAISHA '	+
		' LEFT JOIN TKD_Yyksho AS YOYAKUSHO ON YOYAKUSHO.UkeNo = HAISHA.UkeNo '	+
		' LEFT JOIN TKD_Unkobi AS UNKOBI ON UNKOBI.UkeNo = HAISHA.UkeNo AND UNKOBI.UnkRen = HAISHA.UnkRen '	+
		' LEFT JOIN VPM_HenSya AS HENSYA ON HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq '	+
		' LEFT JOIN VPM_Eigyos AS EIGYOSHO ON EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq '	+
		' LEFT JOIN VPM_Compny AS COMPANY ON COMPANY.CompanyCdSeq = EIGYOSHO.CompanyCdSeq '	+
		' LEFT JOIN VPM_SyaRyo AS SYARYO ON SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq '	+
		' LEFT JOIN VPM_SyaSyu AS SYASYU ON SYASYU.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq '	+
		' WHERE HAISHA.SiyoKbn = 1 '	+
		'		AND COMPANY.TenantCdSeq =ISNULL( '+@TenantCdSeq+',0) '	+
		'		AND HAISHA.HaiSYmd <='+dbo.FP_SetString(@DateBooking)+''	+
		'		AND HAISHA.TouYmd >= '+dbo.FP_SetString(@DateBooking) +''	+
				@MihaisyaKbnCondition+ ''	+
				@BookingTypeCondition+ ''	+
				@BranchCondition+ ''	+
				@CompanyCondition+ ''	+
				@OrderCondition
		
		DECLARE @tb_Temp TABLE(   UkeNo VARCHAR(15)
										, UnkRen SMALLINT 
										, BunkRen SMALLINT 
										, CompanyCdSeq INT
										, CompanyCd INT
										, EigyoCdSeq INT 
										, EigyoCd INT
										, EigyoNm VARCHAR(50)
										, RyakuNm VARCHAR(10)
										, SyuKoYmd CHAR(8)
										, SyuKoTime CHAR(4)
										, HaiSYmd CHAR(8)
										, HaiSTime CHAR(4)
										, TouYmd CHAR(8)
										, TouChTime CHAR(4)
										, KikYmd CHAR(8)
										, HAISHA_KikTime CHAR(4)
										, GoSya CHAR(4)
										, TeiDanNo SMALLINT
										, IkNm VARCHAR(50)
										, HaiSNm VARCHAR(50)
										, HaiSKouKCdSeq INT
										, HaiSKouKNm CHAR(20)
										, HaiSBinCdSeq INT
										, HaiSBinNm CHAR(20)
										, HaiSSetTime CHAR(4)
										, TouNm VARCHAR(50)
										, TouKouKCdSeq INT
										, TouSKouKNm CHAR(20)
										, TouBinCdSeq INT
										, TouBinNm CHAR(20)
										, TouSetTime CHAR(4)
										, UNKOBI_SyuKoTime CHAR(4)
										, UNKOBI_HaiSYmd CHAR(8)
										, UNKOBI_HaiSTime CHAR(4)
										, UNKOBI_TouYmd CHAR(8)
										, UNKOBI_TouChTime CHAR(4)
										, UNKOBI_KikTime CHAR(4)
										, UNKOBI_DanTaNm VARCHAR(100) 
										, SyaSyuCdSeq INT 
										, SyaSyuCd SMALLINT
										, SyaSyuNm VARCHAR(12)
										, SyaRyoCdSeq INT 
										, SyaRyoCd INT
										, SyaRyoNm NVARCHAR(10)
										, TenkoNo NVARCHAR(10)
										, DayBusRunning INT
										, TotalDayBusRun INT
										, VistLocation NVARCHAR(255)
										, VistLocationCompact NVARCHAR(255)
										, SyuKoTimeMain CHAR(4)
										, HaiSTimeMain CHAR(4)
										, KikTimeMain CHAR(4)
										, TotalBus INT
										, RowNum INT
										
								      ) 		
		INSERT INTO @tb_Temp EXEC(@strSQL)
		SELECT * FROM @tb_Temp
		--select @strSQL       
        SET @ROWCOUNT   =   @@ROWCOUNT
    END TRY
-- エラー処理
    BEGIN CATCH
        SET @ReturnCd   =   ERROR_NUMBER()
        SET @ReturnMsg  =   ERROR_MESSAGE()
        SET @eProcedure =   ERROR_PROCEDURE()
        SET @eLine      =   ERROR_LINE()
    END CATCH

    RETURN
GO


