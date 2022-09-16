USE [HOC_Kashikiri_New]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetInfoBus_R]    Script Date: 9/25/2020 1:45:58 PM ******/
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
ALTER PROCEDURE [dbo].[Pro_GetInfoBus_R]
    (
		@DateBooking VARCHAR(10)
	,	@ListCompany VARCHAR(Max)
	,	@BranchFrom  VARCHAR(10)
	,	@BranchTo    VARCHAR(10)
	,	@BookingTypeFrom VARCHAR(10)
	,	@BookingTypeTo VARCHAR(10)
	,	@MihaisyaKbn VARCHAR(255)
	/*,	@BusNumber VARCHAR(255)*/
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
				,	@BusNumberCondition VARCHAR(255) = ''
				,	@OrderCondition VARCHAR(MAX) =''
				,	@RowNumberCondition VARCHAR(MAX)=''

        SET @ReturnCd   =   0       -- リターンコード初期化
        SET @ROWCOUNT   =   0       -- 処理件数初期化
        SET @ReturnMsg  =   ' '     -- 処理メッセージ初期化
        SET @eProcedure =   ' '     -- エラーオブジェクト初期化
        SET @eLine      =   ' '     -- エラー行番号初期化

		/*Check Param @ListCompany : 0 - CheckAll  */
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
											  AND YOYAKUSHO.YoyaKbnSeq <='+@BookingTypeTo+' '
										END		
		/*CHECK Param HAISHA.HaiSSryCdSeq*/ 
		SELECT @MihaisyaKbnCondition = CASE 
											WHEN @MihaisyaKbn='未出力'
												THEN ' AND HAISHA.HaiSSryCdSeq <> 0 '
											WHEN @MihaisyaKbn='出力'
												THEN ' '
											ELSE ''
										END
		/*CHECK Param HAISHA.HaiSSryCdSeq*/ -------------------------CONFIRM -----
		/*SELECT @BusNumber = CASE 
											WHEN @MihaisyaKbn='未出力'
												THEN ' AND HAISHA.HaiSKbn = 2 '
											WHEN @MihaisyaKbn='出力'
												THEN ' '
											ELSE ''
										END*/
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
		'	SELECT 
			ISNULL(EIGYOSHO.EigyoCdSeq,0)
		,	FORMAT(ISNULL(EIGYOSHO.EigyoCd, 0), ''00000'')
		,	ISNULL(EIGYOSHO.RyakuNm,'''') 
		,	ISNULL(EIGYOSHO.EigyoNm,'''') 
		,	ISNULL(SYASYU.SyaSyuNm,'''') 
		,	ISNULL(SYASHYU1.SyaSyuNm,'''') 
		,	ISNULL(SYARYO.SyaRyoNm,'''') 
		,	ISNULL(HAISHA.GoSya,'''') 
		,	ISNULL(HAISHA.UkeNo,'''') 
		,	ISNULL(HAISHA.UnkRen,0) 
		,	ISNULL(HAISHA.SyaSyuRen,0) 
		,	FORMAT(ISNULL(HAISHA.TeiDanNo,0), ''00'')
		,	ISNULL(HAISHA.BunkRen,0) 
		,	ISNULL(HAISHA.HaiSKouKCdSeq,0) 
		,	ISNULL(HAISHA.HaiSKouKNm,'''') 
		,	ISNULL(HAISHA.SyuKoYmd,'''') 
		,	ISNULL(HAISHA.SyuKoTime,'''')  AS HAI_SyuKoTime
		,	ISNULL(HAISHA.HaiSYmd,'''') AS HAI_HaiSYmd 
		,	ISNULL(HAISHA.HaiSTime,'''') AS HAI_HaiSTime 
		,	ISNULL(HAISHA.TouYmd,'''') AS  HAI_TouYmd
		,	ISNULL(HAISHA.TouChTime,'''') AS HAI_TouChTime 
		,	ISNULL(HAISHA.TouKouKCdSeq,0) 
		,	ISNULL(HAISHA.TouSKouKNm,'''') 
		,	ISNULL(HAISHA.KikYmd,'''') 
		,	ISNULL(HAISHA.KikTime,'''') AS HAI_KikTime 
		,	ISNULL(HAISHA.HaiSNm,'''') 
		,   ISNULL(HAISHA.HaisBinNm,'''' ) 
		,	ISNULL(HAISHA.DanTaNm2,'''') 
		,	ISNULL(HAISHA.IkNm,'''') 
		,	ISNULL(HAISHA.TouNm,'''') 
		,   ISNULL(HAISHA.TouBinNm,'''')  
		,	ISNULL(UNKOBI.SyuKoTime,'''') AS UN_SyuKoTime 
		,	ISNULL(UNKOBI.HaiSYmd,'''') AS UN_HaiSYmd 
		,	ISNULL(UNKOBI.HaiSTime,'''') AS UN_HaiSTime 
		,	ISNULL(UNKOBI.TouYmd,'''') AS UN_TouYmd
		,	ISNULL(UNKOBI.TouChTime,'''') AS UN_TouChTime
		,	ISNULL(UNKOBI.KikTime,'''') AS UN_KikTime 
		,	ISNULL( UNKOBI.DanTaNm,'''') AS UN_DanTaNm
		,	ISNULL(SYASYU.SyaSyuCdSeq,0) 
	    ,	(SELECT SUM(ISNULL(SyaSyuDai,0)) FROM TKD_YykSyu WHERE UkeNo =HAISHA.UkeNo AND SiyoKbn = 1 GROUP BY TKD_YykSyu.UkeNo) AS TotalBus 
	    ,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') >= CONVERT(DATETIME, HAISHA.SyuKoYmd)
						 AND CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')<=CONVERT(DATETIME, HAISHA.KikYmd)
						THEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') > CONVERT(DATETIME, HAISHA.SyuKoYmd) 
						THEN CONVERT(INT,CONVERT(DATETIME, HAISHA.SyuKoYmd) - CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+'))
					ELSE CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.KikYmd))+1
				END AS DayBusRunning 	
		,	CONVERT(INT,CONVERT(DATETIME, HAISHA.KikYmd)-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1 AS TotalDayBusRun  
		,	(SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.SyuKoYmd))=0,HAISHA.SyuKoTime,''''))) AS SyuKoTimeMain 
		,	(SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.HaiSYmd))=0,HAISHA.HaiSTime,''''))) AS HaiSTimeMain 
		,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) 
						 AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1=1, ISNULL(HAISHA.HaiSNm, ''''), ''泊中'')
					ELSE ''''
				END AS VistLocation 
		,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1=1, ISNULL(HAISHA.HaiSKouKNm, ''''),'''')
					ELSE ''''
				END AS VistLocationCompact 
		,	UNKOBI.DanTaNm + '' ''+ HAISHA.DanTaNm2 AS GroupName				
		,	(SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.TouYmd))=0,HAISHA.TouChTime,''''))) AS HAISTouChTimeMain 
        ,	(SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.TouYmd))=0,HAISHA.TouNm,''''))) AS TouNmMain 
		,	'+ @RowNumberCondition +'
		FROM TKD_Haisha AS HAISHA
		LEFT JOIN TKD_Yyksho AS YOYAKUSHO ON YOYAKUSHO.UkeNo = HAISHA.UkeNo
		LEFT JOIN TKD_Unkobi AS UNKOBI  ON UNKOBI.UkeNo = HAISHA.UkeNo
			AND UNKOBI.UnkRen = HAISHA.UnkRen
		LEFT JOIN VPM_HenSya AS HENSYA ON HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
		LEFT JOIN TKD_YykSyu AS YYKSYU ON YYKSYU.UkeNo = HAISHA.UkeNo
			AND YYKSYU.UnkRen = HAISHA.UnkRen
			AND YYKSYU.SyaSyuRen = HAISHA.SyaSyuRen
		LEFT JOIN VPM_SyaSyu AS SYASYU ON SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
		LEFT JOIN VPM_SyaRyo AS SYARYO ON SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
		LEFT JOIN VPM_SyaSyu AS SYASHYU1 ON SYASHYU1.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq
		LEFT JOIN VPM_Eigyos AS EIGYOSHO ON EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
		LEFT JOIN VPM_Compny AS COMPANY ON COMPANY.CompanyCdSeq = EIGYOSHO.CompanyCdSeq 
		WHERE HAISHA.SiyoKbn = 1
		AND YYKSYU.SiyoKbn = 1 
		AND HAISHA.SyuKoYmd <='+dbo.FP_SetString(@DateBooking)+''	+ '
		AND HAISHA.KikYmd >= '+dbo.FP_SetString(@DateBooking)+''	+ '
        AND HAISHA.KSKbn <> 1 '+ '
        AND COMPANY.TenantCdSeq =ISNULL( '+@TenantCdSeq+',0) ' +
		@MihaisyaKbnCondition+ ''	+
		/*+@BusNumber+''+*/
		@BookingTypeCondition+ '' +
		@BranchCondition+ ''	+
		@CompanyCondition+ ''	+
		@OrderCondition   +''  
		
		DECLARE @tb_Temp TABLE(		EIGYOS_EigyoCdSeq	INT  
								,	EIGYOS_EigyoCd		VARCHAR(255)
								,	EIGYOS_RyakuNm		NVARCHAR(255)
								,	EIGYOS_EigyoNm		NVARCHAR(255)
								,	SYASYU_SyaSyuNm		NVARCHAR(255)
								,	SYASHYU1_SyaSyuNm	NVARCHAR(255)
								,	SYARYO_SyaRyoNm		NVARCHAR(255)        																						
								,	HAISHA_GoSya		NVARCHAR(255)             																							
								,	HAISHA_UkeNo		VARCHAR(20)             																						
								,	HAISHA_UnkRen		SMALLINT           																				
								,	HAISHA_SyaSyuRen	SMALLINT         																					
								,	HAISHA_TeiDanNo		   VARCHAR(4)    																		
								,	HAISHA_BunkRen		   SMALLINT   																				
								,	HAISHA_HaiSKouKCdSeq   INT																						
								,	HAISHA_HaiSKouKNm      NVARCHAR(255)     																								
								,	HAISHA_SyuKoYmd        CHAR(10)   																			
								,	HAISHA_SyuKoTime       CHAR(4)    																			
								,	HAISHA_HaiSYmd         CHAR(10)      																	
								,	HAISHA_HaiSTime        CHAR(4)      															
								,	HAISHA_TouYmd          CHAR(10)    															
								,	HAISHA_TouChTime       CHAR(4)   																				
								,	HAISHA_TouKouKCdSeq    INT																				
 　　							,	HAISHA_TouSKouKNm      NVARCHAR(255)    																							
								,	HAISHA_KikYmd          CHAR(10)
								,	HAISHA_KikTime         CHAR(255)   																		
								,	HAISHA_HaiSNm          NVARCHAR(255) 
								,   HAISHA_HaisBinNm       NVARCHAR(255)
								,	HAISHA_DanTaNm2        NVARCHAR(255) 																						
								,	HAISHA_IkNm            NVARCHAR(255)          																				
								,	HAISHA_TouNm           NVARCHAR(255) 
								,   HAISHA_TouBinNm        NVARCHAR(255)
								,	UNKOBI_SyuKoTime       CHAR(4)																			
								,	UNKOBI_HaiSYmd         CHAR(10)																		
								,	UNKOBI_HaiSTime        CHAR(4)																	
								,	UNKOBI_TouYmd          CHAR(10)																				
								,	UNKOBI_TouChTime       CHAR(4)																		
								,	UNKOBI_KikTime         CHAR(4)																				
								,	UNKOBI_DanTaNm         VARCHAR(255) 																			
								,	SYASYU_SyaSyuCdSeq     INT
								,	TotalBus INT
								,	DayBusRunning INT
								,	TotalDayBusRun INT
								,	SyuKoTimeMain CHAR(4)
								,	HaiSTimeMain CHAR(4)
								,	VistLocation NVARCHAR(255)
								,	VistLocationCompact  NVARCHAR(255)
								,	GroupName NVARCHAR(255)
								,	HAISTouChTimeMain CHAR(4)
								,	HAISHA_TouNmMain NVARCHAR(255)
								,	Row_Num INT
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


