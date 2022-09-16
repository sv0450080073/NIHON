USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetInfoBus_R]    Script Date: 6/2/2021 4:43:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER   PROCEDURE [dbo].[Pro_GetInfoBus_R]
    (
		@DateBooking VARCHAR(10)
	,	@ListCompany VARCHAR(Max)
	,	@BranchFrom  VARCHAR(10)
	,	@BranchTo    VARCHAR(10)
	,	@BookingTypeList VARCHAR(MAX)
	,	@MihaisyaKbn VARCHAR(255)
	/*,	@BusNumber VARCHAR(255)*/
	,	@Order VARCHAR(Max)
	,	@TenantCdSeq VARCHAR(10)
	,	@ReturnCd      INTEGER OUTPUT         
	,	@ROWCOUNT      INTEGER OUTPUT       
	,	@ReturnMsg     VARCHAR(MAX) OUTPUT     
	,	@eProcedure    VARCHAR(20) OUTPUT     
	,	@eLine         VARCHAR(20) OUTPUT     
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
				,	@OrderConditionRowNumber VARCHAR(MAX) =''
				,	@RowNumberCondition VARCHAR(MAX)=''
				,   @TouNmTmp NVARCHAR(255)  =''	
				,   @TouSKouKNm NVARCHAR(255)  =''	
				,   @dataCurrent_VistLocation  NVARCHAR(255)  ='泊 中'
				,   @dataCurrent_VistLocationCompact NVARCHAR(255)  =''
				 

        SET @ReturnCd   =   0       
        SET @ROWCOUNT   =   0       
        SET @ReturnMsg  =   ' '   
        SET @eProcedure =   ' '     
        SET @eLine      =   ' '     

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
		--SELECT @BookingTypeCondition = CASE
		--									WHEN @BookingTypeFrom ='0' OR @BookingTypeTo ='0' THEN ''
		--									ELSE 'AND YOYAKUSHO.YoyaKbnSeq >='+@BookingTypeFrom+'
		--									  AND YOYAKUSHO.YoyaKbnSeq <='+@BookingTypeTo+' '
		--								END
		SELECT @BookingTypeCondition = ' AND YOYAKUSHO.YoyaKbnSeq IN (select * from FN_SplitString('+dbo.FP_SetString(@BookingTypeList)+', ''-'')) '

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
	
		SELECT @OrderCondition =		CASE
											WHEN @Order='1' 
												THEN ' ORDER BY EIGYOSHO.EigyoCd ASC,
															   HAISHA.SyuKoYmd ASC,
															   HAISHA.SyuKoTime ASC, 
															   SYARYO.SyaRyoCd ASC,								
															   HAISHA.UkeNo ASC,								
															   HAISHA.UnkRen ASC,								
															   HAISHA.TeiDanNo ASC,								
															   HAISHA.BunkRen ASC '	
											WHEN @Order='2' 
												THEN ' ORDER BY EIGYOSHO.EigyoCd ASC,
															   HAISHA.SyuKoYmd ASC,
															   HAISHA.SyuKoTime ASC,
															   HAISHA.TeiDanNo ASC,
															   HAISHA.UkeNo ASC,
															   HAISHA.UnkRen ASC,
															   HAISHA.BunkRen ASC '	
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
																SYAINCD ASC ,
																HAISHA.SyuKoYmd ASC,									
																HAISHA.SyuKoTime ASC,									
																HAISHA.UkeNo ASC,									
																HAISHA.TeiDanNo ASC,									
																HAISHA.UnkRen ASC,									
																HAISHA.BunkRen ASC '	
										ELSE ' '
								END
		IF(@Order ='5')
			BEGIN
				SELECT @OrderConditionRowNumber = ' ORDER BY EIGYOSHO.EigyoCd ASC,
													ISNULL((SELECT Top 1 SyainCdSeq
													FROM TKD_Haiin
													WHERE UkeNo = HAISHA.UkeNo
													AND UnkRen = HAISHA.UnkRen
													AND TeiDanNo = HAISHA.TeiDanNo
													AND BunkRen = HAISHA.BunkRen
													AND SiyoKbn = 1
													ORDER BY SyainCdSeq ASC),999999999 ) ASC ,
													HAISHA.SyuKoYmd ASC,
													HAISHA.SyuKoTime ASC,
													HAISHA.UkeNo ASC, 
													HAISHA.TeiDanNo ASC, 
													HAISHA.UnkRen ASC, 
													HAISHA.BunkRen ASC '
			END
		ELSE
			BEGIN
				SELECT @OrderConditionRowNumber += @OrderCondition;
			END
		/*CHECK ROW NUMBER */
		SELECT @RowNumberCondition =' ROW_NUMBER() OVER ( PARTITION BY EIGYOSHO.EigyoCdSeq '+@OrderConditionRowNumber+') AS row_Num '
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
		,	ISNULL(HAISHA.HaisBinNm,'''')
		,	ISNULL(HAISHA.SyuKoYmd,'''') 
		,	ISNULL(HAISHA.SyuKoTime,'''')  AS HAI_SyuKoTime
		,	ISNULL(HAISHA.HaiSYmd,'''') AS HAI_HaiSYmd 
		,	ISNULL(HAISHA.HaiSTime,'''') AS HAI_HaiSTime 
		,	ISNULL(HAISHA.TouYmd,'''') AS  HAI_TouYmd
		,	ISNULL(HAISHA.TouChTime,'''') AS HAI_TouChTime 
		,	ISNULL(HAISHA.TouKouKCdSeq,0) 
		/*,	ISNULL(HAISHA.TouSKouKNm,'''') */
	   ,  CASE  WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.TouYmd))=0
								THEN HAISHA.TouSKouKNm 
						ELSE ''''
					END  AS HAISHA_TouSKouKNm
		,	ISNULL(HAISHA.TouBinNm,'''')
		,	ISNULL(HAISHA.KikYmd,'''') 
		,	ISNULL(HAISHA.KikTime,'''') AS HAI_KikTime 
		,	ISNULL(HAISHA.HaiSNm,'''') 
		,	ISNULL(HAISHA.DanTaNm2,'''') 
		,	ISNULL(SUBSTRING(HAISHA.IkNm,1,20),'''') 
		,	ISNULL(HAISHA.TouNm,'''') 
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
		/*,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) 
						 AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1=1, ISNULL(SUBSTRING(HAISHA.HaiSNm,1,20), ''''), ''泊中'')
					ELSE ''''
				END AS VistLocation */
		,  CASE  WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.TouYmd))>0
							THEN ''泊 中''
					 WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1 =1
							THEN HAISHA.HaiSNm 
				ELSE
					CASE WHEN HAISHA.HaiSYmd= HAISHA.SyuKoYmd
								THEN 
									(SELECT TOP 1 TehNm
									FROM TKD_Tehai
									WHERE Ukeno =  HAISHA.UkeNo  
									AND UnkRen = HAISHA.UnkRen
									AND TeiDanNo=HAISHA.TeiDanNo
									AND BunkRen =HAISHA.BunkRen
									AND Nittei =(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1-1)
									AND TomKbn =1
									AND SiyoKbn = 1
									AND TehaiCdSeq = 1
									ORDER BY TehRen)	
			
					ELSE
							CASE
								WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1 =2
									THEN 
										(SELECT TOP 1 TehNm
										FROM TKD_Tehai
										WHERE Ukeno = HAISHA.UkeNo  
										AND UnkRen = HAISHA.UnkRen
										AND TeiDanNo=HAISHA.TeiDanNo
										AND BunkRen =HAISHA.BunkRen
										AND Nittei =(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1-1)
										AND TomKbn =2
										AND SiyoKbn = 1
										AND TehaiCdSeq = 1
										ORDER BY TehRen )			
							WHEN ( CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1 < CONVERT(INT,CONVERT(DATETIME, HAISHA.KikYmd)-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1)
								THEN 
									(SELECT TOP 1 TehNm
									FROM TKD_Tehai
									WHERE Ukeno = HAISHA.UkeNo  
									AND UnkRen = HAISHA.UnkRen
									AND TeiDanNo=HAISHA.TeiDanNo
									AND BunkRen =HAISHA.BunkRen
									AND Nittei =(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1-2)
									AND TomKbn =1
									AND SiyoKbn = 1
									AND TehaiCdSeq = 1
									ORDER BY TehRen)
						ELSE   ''泊 中''	
					END 
				END												
			END  AS VistLocation 
		/*,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1=1, ISNULL(HAISHA.HaiSKouKNm, ''''),'''')
					ELSE ''''
				END AS VistLocationCompact */
		,   CASE WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.TouYmd))>0
						THEN ''''
				 WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1 =1
						THEN HAISHA.HaiSKouKNm 	
				ELSE ''''
			END  AS VistLocationCompact
		,	 SUBSTRING(UNKOBI.DanTaNm,1,20) AS GroupName				
		,	(SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.TouYmd))=0,HAISHA.TouChTime,''''))) AS HAISTouChTimeMain 

       /* ,	(SELECT (IIF(CONVERT(INT,CONVERT(DATETIME,'+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME,HAISHA.TouYmd))=0,HAISHA.TouNm,''''))) AS TouNmMain */
	   	,	 CASE WHEN  CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.TouYmd))=0
							THEN HAISHA.TouNm
						WHEN  HAISHA.HaiSYmd=HAISHA.SyuKoYmd
							THEN 
								CASE WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1 <> CONVERT(INT,CONVERT(DATETIME, HAISHA.KikYmd)-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1	
										THEN (SELECT TOP 1 TehNm
											  FROM TKD_Tehai
											WHERE Ukeno =  HAISHA.UkeNo  
											AND UnkRen = HAISHA.UnkRen
											AND TeiDanNo=HAISHA.TeiDanNo
											AND BunkRen =HAISHA.BunkRen
											AND Nittei =(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1)
											AND TomKbn =1
											AND SiyoKbn = 1
											AND TehaiCdSeq = 1
											ORDER BY TehRen)	
								END 
						ELSE 
							CASE WHEN CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1=1
									THEN (SELECT TOP 1 TehNm
											FROM TKD_Tehai
											WHERE Ukeno =  HAISHA.UkeNo  
											AND UnkRen = HAISHA.UnkRen
											AND TeiDanNo=HAISHA.TeiDanNo
											AND BunkRen =HAISHA.BunkRen
											AND Nittei =(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1)
											AND TomKbn =2
											AND SiyoKbn = 1
											AND TehaiCdSeq = 1
											ORDER BY TehRen)	
							ELSE
								CASE WHEN   (CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1) < (CONVERT(INT,CONVERT(DATETIME, HAISHA.KikYmd)-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1) -1
										THEN  (SELECT TOP 1 TehNm
													FROM TKD_Tehai
													WHERE Ukeno =  HAISHA.UkeNo  
													AND UnkRen = HAISHA.UnkRen
													AND TeiDanNo=HAISHA.TeiDanNo
													AND BunkRen =HAISHA.BunkRen
													AND Nittei =(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+')-CONVERT(DATETIME, HAISHA.SyuKoYmd))+1-1)
													AND TomKbn =1
													AND SiyoKbn = 1
													AND TehaiCdSeq = 1
													ORDER BY TehRen)	
								ELSE ''''
						END 
					END
				END AS HAISHA_TouNmMain
		,	ISNULL((SELECT Top 1 SyainCdSeq FROM TKD_Haiin WHERE UkeNo = HAISHA.UkeNo																				
                                           AND UnkRen = HAISHA.UnkRen																				
                                           AND TeiDanNo = HAISHA.TeiDanNo																				
                                           AND BunkRen = HAISHA.BunkRen																				
                                           AND SiyoKbn = 1																				
                                           ORDER BY SyainCdSeq ASC ),999999999) AS SYAINCD
	/*	,	CASE
					WHEN CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') BETWEEN CONVERT(DATETIME, HAISHA.HaiSYmd) 
						 AND CONVERT(DATETIME, HAISHA.TouYmd) 
						THEN IIF(CONVERT(INT,CONVERT(DATETIME, '+dbo.FP_SetString(@DateBooking)+') - CONVERT(DATETIME, HAISHA.HaiSYmd))+1=1, ISNULL(SUBSTRING(HAISHA.HaiSNm,21,20), ''''), ''泊中'')
					ELSE ''''
				END AS VistLocation02 */
		, '''' AS VistLocation02
		,   SUBSTRING(HAISHA.DanTaNm2,1,20) AS GroupName02
		,	'+ @RowNumberCondition +'
		FROM TKD_Haisha AS HAISHA
		LEFT JOIN TKD_Unkobi AS UNKOBI ON HAISHA.UkeNo = UNKOBI.UkeNo
		LEFT JOIN VPM_SyaRyo AS SYARYO ON HAISHA.HaiSSryCdSeq = SYARYO.SyaRyoCdSeq
		LEFT JOIN VPM_HenSya AS HENSYA ON SYARYO.SyaRyoCdSeq = HENSYA.SyaRyoCdSeq
		LEFT JOIN VPM_Eigyos AS EIGYOSHO ON HENSYA.EigyoCdSeq = EIGYOSHO.EigyoCdSeq
		LEFT JOIN VPM_Compny AS COMPANY ON COMPANY.CompanyCdSeq = EIGYOSHO.CompanyCdSeq AND COMPANY.TenantCdSeq = '+ @TenantCdSeq + '
		LEFT JOIN TKD_Yyksho AS YOYAKUSHO ON HAISHA.ukeno = YOYAKUSHO.UkeNo
		LEFT JOIN TKD_YykSyu AS YYKSYU 
				ON YYKSYU.UkeNo = HAISHA.UkeNo
				AND YYKSYU.UnkRen = HAISHA.UnkRen
				AND YYKSYU.SyaSyuRen = HAISHA.SyaSyuRen
		LEFT JOIN VPM_SyaSyu AS SYASYU 
				ON SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
		LEFT JOIN VPM_SyaSyu AS SYASHYU1 
				ON SYASHYU1.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq 
		WHERE HAISHA.SiyoKbn = 1
		AND UNKOBI.SiyoKbn = 1
		AND YOYAKUSHO.SiyoKbn = 1
		AND YOYAKUSHO.YoyaSyu = 1
		AND YYKSYU.SiyoKbn = 1 
		AND HAISHA.SyuKoYmd <='+dbo.FP_SetString(@DateBooking)+''	+ '
		AND HAISHA.KikYmd >= '+dbo.FP_SetString(@DateBooking)+''	+ '
        AND HAISHA.KSKbn <> 1 '+ '
		AND ((YYKSYU.SyaSyuCdSeq <> 0 and SYASYU.TenantCdSeq = '+@TenantCdSeq + ') or YYKSYU.SyaSyuCdSeq = 0)
		AND YOYAKUSHO.TenantCdSeq = '+@TenantCdSeq + '      
		AND SYASHYU1.TenantCdSeq = ' + @TenantCdSeq +
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
								,	HAISHA_HaisBinNm      NVARCHAR(255) 
								,	HAISHA_SyuKoYmd        CHAR(10)   																			
								,	HAISHA_SyuKoTime       CHAR(4)    																			
								,	HAISHA_HaiSYmd         CHAR(10)      																	
								,	HAISHA_HaiSTime        CHAR(4)      															
								,	HAISHA_TouYmd          CHAR(10)    															
								,	HAISHA_TouChTime       CHAR(4)   																				
								,	HAISHA_TouKouKCdSeq    INT																				
 　　							,	HAISHA_TouSKouKNm      NVARCHAR(255)    	
								,	HAISHA_TouBinNm		   NVARCHAR(255) 
								,	HAISHA_KikYmd          CHAR(10)
								,	HAISHA_KikTime         CHAR(255)   																		
								,	HAISHA_HaiSNm          NVARCHAR(255)    																					
								,	HAISHA_DanTaNm2        NVARCHAR(255) 																						
								,	HAISHA_IkNm            NVARCHAR(255)          																				
								,	HAISHA_TouNm           NVARCHAR(255)      																							
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
								,	SYAINCD INT
								,	VistLocation02 NVARCHAR(255)
								,	GroupName02 NVARCHAR(255)
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


