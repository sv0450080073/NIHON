USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_BusCoordination_R]    Script Date: 2/24/2021 10:28:28 AM ******/
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
ALTER PROCEDURE [dbo].[Pro_BusCoordination_R]
    (
		@CheckDaySearch char(2),
		@DateFrom  char(8),
		@DateTo char(8),
		@BookingTypeList varchar(MAX),
		@CustomerFrom varchar(255),
		@CustomerTo varchar(255),
		@SupplierFrom varchar(255),
		@SupplierTo varchar(255),
		@CustomerFrom01 varchar(255),
		@CustomerTo01 varchar(255),
		@SupplierFrom01 varchar(255),
		@SupplierTo01 varchar(255),
		@SaleBranch varchar(255),
		@BookingFrom Varchar(15),
		@BookingTo varchar(15),
		@Staff varchar(255),
		@PersonInput varchar(255),
		@TenantCdSeq		varchar(2)
	/*,	@ReturnCd      INTEGER OUTPUT          -- リターンコード
	,	@ROWCOUNT      INTEGER OUTPUT          -- 処理件数
	,	@ReturnMsg     VARCHAR(MAX) OUTPUT     -- 処理メッセージ
	,	@eProcedure    VARCHAR(20) OUTPUT      -- エラーオブジェクト
	,	@eLine         VARCHAR(20) OUTPUT      -- エラー行番号*/
    )
AS
	BEGIN TRY 	
        DECLARE @strSQL NVARCHAR(MAX)=' '			  
		DECLARE @CheckDaySearchCondition VARCHAR(255)='',
				@BookingTypeCondition  VARCHAR(255)='',
				@CustomerCondition VARCHAR(MAX)='',
		        @Customer01Condition VARCHAR(MAX)='',
				@SaleBranchCondition VARCHAR(255)='',
				@UkenoCondition VARCHAR(255)='',
			    @StaffCondition VARCHAR(255)='',
			@PersonInputCondition VARCHAR(255)=''
      /* SET @ReturnCd   =   0       -- リターンコード初期化
        SET @ROWCOUNT   =   0       -- 処理件数初期化
        SET @ReturnMsg  =   ' '     -- 処理メッセージ初期化
        SET @eProcedure =   ' '     -- エラーオブジェクト初期化
        SET @eLine      =   ' '     -- エラー行番号初期化*/
		/*CHECK SELECT ALL*/
		SELECT @CheckDaySearchCondition = CASE WHEN @CheckDaySearch ='1' THEN ' AND UNKOBI.HaiSYmd >='+dbo.FP_SetString(@DateFrom)+
																				' '+' AND UNKOBI.HaiSYmd <='+dbo.FP_SetString(@DateTo)
											   WHEN @CheckDaySearch='2' THEN ' AND UNKOBI.TouYmd >='+dbo.FP_SetString(@DateFrom)+
																				' '+' AND UNKOBI.TouYmd <='+dbo.FP_SetString(@DateTo)
												 WHEN @CheckDaySearch='3' THEN ' AND YYKSHO.UkeYmd >='+dbo.FP_SetString(@DateFrom)+
																				' '+' AND YYKSHO.UkeYmd <='+dbo.FP_SetString(@DateTo)
											END
		--SELECT @BookingTypeCondition = CASE WHEN @BookingTypeFrom ='0' OR @BookingTypeTo ='0' THEN ' '
		--									ELSE ' AND YYKSHO.YoyaKbnSeq >='+@BookingTypeFrom+'
		--									  AND YYKSHO.YoyaKbnSeq <='+@BookingTypeTo+' '
		SELECT @BookingTypeCondition = ' AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString('+dbo.FP_SetString(@BookingTypeList)+', ''-'')) '
										
		SELECT @CustomerCondition = ' AND ((' + @CustomerFrom + ' = 0 and ' + @CustomerTo + ' = 0)
									or
									(' + @CustomerFrom + ' <> ' + @CustomerTo + ' and YYKSHO.TokuiSeq = ' + @CustomerFrom + ' and YYKSHO.SitenCdSeq >= ' + @SupplierFrom + ')
									or
									(' + @CustomerFrom + ' <> ' + @CustomerTo + ' and YYKSHO.TokuiSeq = ' + @CustomerTo + ' and YYKSHO.SitenCdSeq <= ' + @SupplierTo + ')
									or
									(' + @CustomerFrom + ' = ' + @CustomerTo + ' and YYKSHO.TokuiSeq = ' + @CustomerFrom + ' and YYKSHO.SitenCdSeq >= ' + @SupplierFrom + ' and YYKSHO.SitenCdSeq <= ' + @SupplierTo + ')
									or
									(' + @CustomerFrom + ' = 0 and ' + @CustomerTo + ' <> 0 and ((YYKSHO.TokuiSeq = ' + @CustomerTo + ' and YYKSHO.SitenCdSeq <= ' + @SupplierTo + ') or YYKSHO.TokuiSeq < ' + @CustomerTo + '))
									or
									(' + @CustomerTo+ ' = 0 and ' + @CustomerFrom + ' <> 0 and ((YYKSHO.TokuiSeq = ' + @CustomerFrom + ' and YYKSHO.SitenCdSeq >= ' + @SupplierFrom + ') or YYKSHO.TokuiSeq > ' + @CustomerFrom + '))
									or
									(YYKSHO.TokuiSeq < ' + @CustomerTo + ' and YYKSHO.TokuiSeq > ' + @CustomerFrom + ')) '
		SELECT @Customer01Condition = 'AND ((' + @CustomerFrom01 + ' = 0 and ' + @CustomerTo01 +  ' = 0)
									or
									(' + @CustomerFrom01 + ' <> ' + @CustomerTo01 +  ' and YYKSHO.SirCdSeq = ' + @CustomerFrom01 + ' and YYKSHO.SirSitenCdSeq >= ' + @SupplierFrom01 +  ')
									or
									(' + @CustomerFrom01 + ' <> ' + @CustomerTo01 +  ' and YYKSHO.SirCdSeq = ' + @CustomerTo01 +  ' and YYKSHO.SirSitenCdSeq <= ' + @SupplierTo01 +  ')
									or
									(' + @CustomerFrom01 + ' = ' + @CustomerTo01 +  ' and YYKSHO.SirCdSeq = ' + @CustomerFrom01 + ' and YYKSHO.SirSitenCdSeq >= ' + @SupplierFrom01 +  ' and YYKSHO.SirSitenCdSeq <= ' + @SupplierTo01 +  ')
									or
									(' + @CustomerFrom01 + ' = 0 and ' + @CustomerTo01 +  ' <> 0 and ((YYKSHO.SirCdSeq = ' + @CustomerTo01 +  ' and YYKSHO.SirSitenCdSeq <= ' + @SupplierTo01 +  ') or YYKSHO.SirCdSeq < ' + @CustomerTo01 +  '))
									or
									(' + @CustomerTo01 +  ' = 0 and ' + @CustomerFrom01 + ' <> 0 and ((YYKSHO.SirCdSeq = ' + @CustomerFrom01 + ' and YYKSHO.SirSitenCdSeq >= ' + @SupplierFrom01 +  ') or YYKSHO.SirCdSeq > ' + @CustomerFrom01 + '))
									or
									(YYKSHO.SirCdSeq < ' + @CustomerTo01 +  ' and YYKSHO.SirCdSeq > ' + @CustomerFrom01 + ')) '
		SELECT @SaleBranchCondition = CASE WHEN @SaleBranch='0' THEN ' '
											ELSE ' AND YYKSHO.UkeEigCdSeq='+@SaleBranch+' '
									END

		SELECT @UkenoCondition = ' AND YYKSHO.UkeCd >='+@BookingFrom+' 
								   AND YYKSHO.UkeCd <='+@BookingTo+''
		SELECT @StaffCondition = CASE WHEN @Staff='0' THEN ' '
											ELSE ' AND YYKSHO.EigTanCdSeq='+@Staff+' '
									END
		SELECT @PersonInputCondition = CASE WHEN @PersonInput='0' THEN ' '
											ELSE ' AND YYKSHO.InTanCdSeq='+@PersonInput+' '
									END
		/*END CHECK SELECT ALL*/

		--IF @CustomerFrom = '0' and @CustomerTo = '0'
		--set @CustomerCondition = ''
		--IF @CustomerFrom01 = '0' and @CustomerTo01 = '0'
		--set @Customer01Condition = ''

		/*SELECT */
		SET @strSQL = @strSQL + 
	'SELECT ISNULL(UNKOBI.UkeNo,'''')               																																	          																															
	  ,ISNULL(FORMAT ( convert(datetime,UNKOBI.HaiSYmd , 112), ''yyyy年MM月dd日（ddd）'', ''ja-JP'' ),'''')
	  ,ISNULL(UNKOBI.HaiSTime,'''')            																															
	  ,ISNULL(UNKOBI.SyuPaTime,'''')
	  ,ISNULL(FORMAT ( convert(datetime,UNKOBI.TouYmd , 112), ''yyyy年MM月dd日（ddd）'', ''ja-JP'' ),'''')  
	  ,ISNULL(SUBSTRING(UNKOBI.DanTaNm,1,34),'''')   AS  UNKOBI_DanTaNm    																													
	  ,ISNULL(UNKOBI.KanjJyus1,'''')            																														
	  ,ISNULL(UNKOBI.KanjJyus2,'''')            																															
	  ,ISNULL(UNKOBI.KanjTel ,'''')             																															
	  ,ISNULL(UNKOBI.KanJNm,'''')            																													
	  ,ISNULL(UNKOBI.HaiSNm,'''')             																															
	  ,ISNULL(UNKOBI.HaiSJyus1,'''')          																											
	  ,ISNULL(UNKOBI.HaiSJyus2,'''')              																															
	  ,ISNULL(UNKOBI.IkNm,'''')                 																												               																															          																																        																																																													
	  ,ISNULL(UNKOBI.HaiSSetTime,'''')        																																																													          																													        																															    																															
	  ,ISNULL(YYKSHO.TokuiTel,'''')           																															
	  ,ISNULL(YYKSHO.TokuiTanNm,'''')          																																                																															               																															         																																
	  ,ISNULL(TOKISK.TokuiNm,'''')             																																
	  ,ISNULL(TOKIST.SitenNm,'''')             																															
	  ,ISNULL(SIRSK.TokuiNm,'''')                																															
	  ,ISNULL(SIRST.SitenNm,'''') 
      ,ISNULL(SJJOKBN1.CodeKbnNm,'''')
	  ,ISNULL(SJJOKBN2.CodeKbnNm,'''')    
	  ,ISNULL(SJJOKBN3.CodeKbnNm,'''') 
	  ,ISNULL(SJJOKBN4.CodeKbnNm,'''')
	  ,ISNULL(SJJOKBN5.CodeKbnNm,'''')
	  ,ISNULL(OTHERJIN1.CodeKbnNm,'''')  AS OTHERJIN1_CodeKbnNm
	  ,ISNULL(UNKOBI.OthJin1,0)  AS UNKOBI_OthJin1
	  ,ISNULL(OTHERJIN2.CodeKbnNm,'''')  AS OTHERJIN2_CodeKbnNm
	  ,ISNULL(UNKOBI.OthJin2,0)    AS UNKOBI_OthJin2 
	  ,ISNULL(UNKOBI.JyoSyaJin,0)  
	  ,ISNULL(UNKOBI.PlusJin,0)  
	  ,ISNULL(UNKOBI.HaiSKouKNm,'''')             											
	  ,ISNULL(UNKOBI.HaisBinNm,'''')            												        											
	  ,ISNULL(UNKOBI.TouSKouKNm,'''')             											
	  ,ISNULL(UNKOBI.TouSBinNm,'''')            										
	  ,ISNULL(UNKOBI.TouSetTime,'''')  
      ,ISNULL(UNKOBI.KikYmd,'''')
	  ,ISNULL(UNKOBI.SyuKoYmd,'''')
	  ,ISNULL(UNKOBI.BikoNm,'''')
	  ,ISNULL(UNKOBI.TouNm,'''')
	  ,ISNULL(UNKOBI.TouJyusyo1,'''')            																															
	  ,ISNULL(UNKOBI.TouJyusyo2,'''')
	  ,ISNULL(UNKOBI.TouChTime,'''')
	  ,ISNULL(YYKSHO.UntKin,0) 
	  ,ISNULL(ZEIKBN.CodeKbnNm,'''')  
	  ,ISNULL(YYKSHO.ZeiRui,0)
	  ,ISNULL(YYKSHO.UntKin + YYKSHO.ZeiRui,0) 
	  ,ISNULL(CONCAT(YYKSHO.TesuRitu,''%''),'''')  
	  ,ISNULL(YYKSHO.TesuRyoG,0)     
	  ,ISNULL((SELECT SUM(ISNULL(SyaRyoSyo,0)) + SUM(ISNULL(SyaRyoTes,0)) FROM TKD_Yousha WHERE UkeNo = UNKOBI.UkeNo AND SiyoKbn = 1),0) 
	  ,ISNULL(UKEJOKEN.CodeKbnNm,'''')
	  ,ISNULL(SUBSTRING(UNKOBI.DanTaNm,35,34),'''')  AS  UNKOBI_DanTaNm2
	  ,ISNULL(FORMAT(YYKSHO.UkeCd,''0000000000''),'''')   
	  ,ISNULL(UNKOBI.UnkRen,'''') 
	FROM TKD_Unkobi    AS UNKOBI																																	
	LEFT JOIN TKD_Yyksho AS YYKSHO																																	
       ON YYKSHO.UkeNo = UNKOBI.UkeNo																																	
	LEFT JOIN VPM_Tokisk AS TOKISK																																	
       ON TOKISK.TokuiSeq  = YYKSHO.TokuiSeq																																	
	  AND TOKISK.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'     																															
	LEFT JOIN VPM_TokiSt AS TOKIST																																	
       ON TOKIST.TokuiSeq   = YYKSHO.TokuiSeq																																	
	  AND TOKIST.SitenCdSeq = YYKSHO.SitenCdSeq	
	  AND TOKIST.SiyoStaYmd <= UNKOBI.HaiSYmd										
	  AND TOKIST.SiyoEndYmd >= UNKOBI.HaiSYmd										
	LEFT JOIN VPM_Tokisk AS SIRSK																																	
       ON SIRSK.TokuiSeq = YYKSHO.SirCdSeq																																	
	  AND SIRSK.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'  																													
	LEFT JOIN VPM_TokiSt AS SIRST																																	
       ON SIRST.TokuiSeq   = YYKSHO.SirCdSeq																																	
	  AND SIRST.SitenCdSeq = YYKSHO.SirSitenCdSeq	
	  AND SIRST.SiyoStaYmd <= UNKOBI.HaiSYmd										
	  AND SIRST.SiyoEndYmd >= UNKOBI.HaiSYmd										
	LEFT JOIN VPM_CodeKb AS ZEIKBN																																	
       ON ZEIKBN.CodeKbn = YYKSHO.ZeiKbn																																	
	  AND ZEIKBN.CodeSyu = ''ZEIKBN''																																
	  AND ZEIKBN.SiyoKbn = 1																																
	  AND ZEIKBN.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'     																																
	LEFT JOIN VPM_CodeKb AS UKEJOKEN																																	
       ON UKEJOKEN.CodeKbn = UNKOBI.UkeJyKbnCd																																	
	  AND UKEJOKEN.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'  																															
	  AND UKEJOKEN.CodeSyu = ''UKEJYKBNCD''																															
	LEFT JOIN VPM_CodeKb AS OTHERJIN1																																	
		ON OTHERJIN1.CodeKbn = UNKOBI.OthJinKbn1																																	
		AND OTHERJIN1.CodeSyu = ''OTHJINKBN''																																
		AND OTHERJIN1.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'        																																
	LEFT JOIN VPM_CodeKb AS OTHERJIN2																																	
		 ON OTHERJIN2.CodeKbn = UNKOBI.OthJinKbn2																																	
		AND OTHERJIN2.CodeSyu = ''OTHJINKBN''																																
		 AND OTHERJIN2.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'  
	LEFT JOIN VPM_CodeKb AS SJJOKBN1																							
		ON SJJOKBN1.CodeKbn = UNKOBI.SijJoKbn1																							
		AND SJJOKBN1.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'                 																						
		AND SJJOKBN1.CodeSyu = ''SIJJOKBN1''																						
	LEFT JOIN VPM_CodeKb AS SJJOKBN2																							
		ON SJJOKBN2.CodeKbn = UNKOBI.SijJoKbn2																							
		AND SJJOKBN2.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'              																					
		AND SJJOKBN2.CodeSyu = ''SIJJOKBN2''																					
	LEFT JOIN VPM_CodeKb AS SJJOKBN3																							
		ON SJJOKBN3.CodeKbn = UNKOBI.SijJoKbn3																							
		AND SJJOKBN3.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'              																					
		AND SJJOKBN3.CodeSyu = ''SIJJOKBN3''																						
	LEFT JOIN VPM_CodeKb AS SJJOKBN4																							
		ON SJJOKBN4.CodeKbn = UNKOBI.SijJoKbn4																							
		AND SJJOKBN4.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'               																						
		AND SJJOKBN4.CodeSyu = ''SIJJOKBN4''																					
	LEFT JOIN VPM_CodeKb AS SJJOKBN5																							
		ON SJJOKBN5.CodeKbn = UNKOBI.SijJoKbn5																							
		AND SJJOKBN5.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'                 																					
		AND SJJOKBN5.CodeSyu = ''SIJJOKBN5''			  
	WHERE UNKOBI.SiyoKbn = 1																														
		AND YYKSHO.YoyaSyu = 1                                  																															
		AND YYKSHO.SiyoKbn = 1 
		AND YYKSHO.TenantCdSeq ='+@TenantCdSeq+
		@CheckDaySearchCondition +
		@BookingTypeCondition + 
		@CustomerCondition+ 
		@Customer01Condition +
		@SaleBranchCondition +
		@UkenoCondition+ 
		@StaffCondition +
		@PersonInputCondition +
     ' ORDER BY UNKOBI.UkeNo ASC'
	DECLARE  @tb_Temp TABLE (
							UNKOBI_Ukeno NVARCHAR(255)
							, UNKOBI_HaiSYmd NVARCHAR(255)
							, UNKOBI_HaiSTime CHAR(4)
							, UNKOBI_SyuPaTime CHAR(4)
							, UNKOBI_TouYmd NVARCHAR(255)
							, UNKOBI_DanTaNm NVARCHAR(255)
							, UNKOBI_KanjJyus1 NVARCHAR(255)
							, UNKOBI_KanjJyus2 NVARCHAR(255)
							,UNKOBI_KanjTel NVARCHAR(255)
							,UNKOBI_KanjNm NVARCHAR(255)
							,UNKOBI_HaiSNm NVARCHAR(255)
							,UNKOBI_HaiSJyus1 NVARCHAR(255)
							,UNKOBI_HaiSJyus2 NVARCHAR(255)
							,UNKOBI_IkNm NVARCHAR(255)
							,UNKOBI_HaiSSetTime  CHAR(4)
							,YYKSHO_TokuiTel NVARCHAR(255)
							,YYKSHO_TokuiTanNm NVARCHAR(255)
							,TOKISK_TokuiNm NVARCHAR(255)
							,TOKIST_SitenNm NVARCHAR(255)
							,SIRSK_TokuiNm NVARCHAR(255)
							,SIRST_SitenNm NVARCHAR(255)
							,SJJOKBN1_CodeKbnNm  NVARCHAR(255)
							,SJJOKBN2_CodeKbnNm  NVARCHAR(255)
						    ,SJJOKBN3_CodeKbnNm  NVARCHAR(255)
							,SJJOKBN4_CodeKbnNm  NVARCHAR(255)
							,SJJOKBN5_CodeKbnNm  NVARCHAR(255)
							,OTHERJIN1_CodeKbnNm NVARCHAR(255)
							,UNKOBI_OthJin1      NVARCHAR(255)
							,OTHERJIN2_CodeKbnNm NVARCHAR(255)
							,UNKOBI_OthJin2  NVARCHAR(255)
							,UNKOBI_JyoSyaJin   NVARCHAR(255)
							,UNKOBI_PlusJin  NVARCHAR(255)
							,UNKOBI_HaiSKouKNm  NVARCHAR(255)           											
							,UNKOBI_HaisBinNm     NVARCHAR(255)       																			  											
							,UNKOBI_TouSKouKNm     NVARCHAR(255)        											
							,UNKOBI_TouSBinNm      NVARCHAR(255)      										
							,UNKOBI_TouSetTime    NVARCHAR(255)
							,UNKOBI_KikYmd NVARCHAR(255)
							,UNKOBI_SyuKoYmd NVARCHAR(255)
							,UNKOBI_BikoNm      NVARCHAR(255)
							,UNKOBI_TouNm NVARCHAR(255)
							,UNKOBI_TouJyusyo1  NVARCHAR(255)
							,UNKOBI_TouJyusyo2   NVARCHAR(255)
							,UNKOBI_TouChTime  NVARCHAR(255)
							,YYKSHO_UntKin  NVARCHAR(255)           											
							,ZEIKBN_CodeKbnNm     NVARCHAR(255)       																			  											
							,YYKSHO_ZeiRui     VARCHAR(255)        											
							,Bus_ZeiKomiKinGak      NVARCHAR(255)      										
							,Bus_TesuRyoRitu    NVARCHAR(255)
							,YYKSHO_TesuRyoG NVARCHAR(255)
							,YouSyaGak NVARCHAR(255)
							,UKEJOKEN_CodeKbnNm NVARCHAR(255)
							,UNKOBI_DanTaNm2 NVARCHAR(255)
							,YYKSHO_UkeCd NVARCHAR(255)
							,UNKOBI_UnkRen VARCHAR(10) 
							)
	INSERT INTO @tb_Temp EXEC (@strSQL)
	SELECT  * FROM @tb_Temp
	--EXEC @strSQL
	--SELECT @strSQL
      /*  SET @ROWCOUNT   =   @@ROWCOUNT*/
    END TRY
	-- エラー処理
    BEGIN CATCH	
   /*  SET @ReturnCd   =   ERROR_NUMBER()
        SET @ReturnMsg  =   ERROR_MESSAGE()
        SET @eProcedure =   ERROR_PROCEDURE()
        SET @eLine      =   ERROR_LINE()*/
    END CATCH	
    RETURN
