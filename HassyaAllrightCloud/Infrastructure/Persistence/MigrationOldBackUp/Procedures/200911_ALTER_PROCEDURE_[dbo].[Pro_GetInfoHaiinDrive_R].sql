USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetInfoHaiinDrive_R]    Script Date: 9/11/2020 9:28:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Pro_GetInfoHaiinDrive_R]
    (	
		@Ukeno VARCHAR(20)
	,	@UnkRen VARCHAR(10)
	,	@TeiDanNo VARCHAR(10)
	,	@BunkRen VARCHAR(10)
	,	@HaiShaSyuKoYmd VARCHAR(10)
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
			   ,@strSQL_DR VARCHAR(MAX)=''
			   ,@strSQL_GUI VARCHAR(MAX)=''
			   ,@Condition VARCHAR(20)=''
		DECLARE @CountRecordHaiinDR  INT=0
			   ,@CountRecordHaiinGUI  INT=0

        SET @ReturnCd   =   0       -- リターンコード初期化
        SET @ROWCOUNT   =   0       -- 処理件数初期化
        SET @ReturnMsg  =   ' '     -- 処理メッセージ初期化
        SET @eProcedure =   ' '     -- エラーオブジェクト初期化
        SET @eLine      =   ' '     -- エラー行番号初期化
		/*GET DRIVER*/
        SET @strSQL_DR = @strSQL_DR + 
		' SELECT	ISNULL(HAIIN_DR.UkeNo, '''') AS UkeNo '	+
				' ,	ISNULL(HAIIN_DR.SyainCdSeq, 0) AS SyainCdSeq '	+
				' ,	ISNULL(HAIIN_DR.TeiDanNo, 0) AS TeiDanNo '	+
				' ,	ISNULL(KYOSHE_DR.SyokumuCdSeq, 0) AS SyokumuCdSeq '	+ 
				' ,	ISNULL(SYAIN.SyainCd, '''') AS SyainCd '	+
				' ,	ISNULL(SYAIN.SyainNm, '''') AS SyainNm '	+
		' FROM TKD_Haiin HAIIN_DR '	+
		' LEFT JOIN VPM_KyoSHe KYOSHE_DR ON HAIIN_DR.SyainCdSeq = KYOSHE_DR.SyainCdSeq '	+
		' AND KYOSHE_DR.StaYmd <='+dbo.FP_SetString(@HaiShaSyuKoYmd)+''	+
		' AND KYOSHE_DR.EndYmd >='+dbo.FP_SetString(@HaiShaSyuKoYmd)+''	+
		' LEFT JOIN VPM_Syokum AS SYOKUM ON SYOKUM.SyokumuCdSeq = KYOSHE_DR.SyokumuCdSeq '	+
		' AND SYOKUM.TenantCdSeq ='+ ISNULL(@TenantCdSeq,'0') 	+
		' JOIN VPM_Syain SYAIN ON SYAIN.SyainCdSeq= HAIIN_DR.SyainCdSeq '	+ 
		' WHERE HAIIN_DR.UkeNo ='+ ISNULL(@Ukeno,'0') 	+
		'		AND HAIIN_DR.BunkRen ='+ ISNULL(@BunkRen,'0') 	+
		'		AND HAIIN_DR.UnkRen= '+ ISNULL(@UnkRen,'0') 	+	
		'		AND HAIIN_DR.TeiDanNo=' + ISNULL(@TeiDanNo,'0') 	+	
		'		AND HAIIN_DR.SiyoKbn = 1 '	+	
		'		AND SYOKUM.SyokumuKbn IN (1,2) '	+
		'  ORDER BY HAIIN_DR.HaiInRen ASC '
		/*GET GUI*/
		SET @strSQL_GUI = @strSQL_GUI+ 
		' SELECT	ISNULL(HAIIN_GUI.UkeNo, '''') AS UkeNo '	+
				' ,	ISNULL(HAIIN_GUI.SyainCdSeq, 0) AS SyainCdSeq '	+
				' ,	ISNULL(HAIIN_GUI.TeiDanNo, 0) AS TeiDanNo '	+
				' ,	ISNULL(KYOSHE_GUI.SyokumuCdSeq, 0) AS SyokumuCdSeq '	+ 
				' ,	ISNULL(SYAIN.SyainCd, '''') AS SyainCd '	+
				' ,	ISNULL(SYAIN.SyainNm, '''') AS SyainNm '	+
		' FROM TKD_Haiin HAIIN_GUI '	+
		' LEFT JOIN VPM_KyoSHe KYOSHE_GUI ON HAIIN_GUI.SyainCdSeq = KYOSHE_GUI.SyainCdSeq '	+
		' AND KYOSHE_GUI.StaYmd <='+dbo.FP_SetString(@HaiShaSyuKoYmd)+''	+
		' AND KYOSHE_GUI.EndYmd >='+dbo.FP_SetString(@HaiShaSyuKoYmd)+''	+
		' LEFT JOIN VPM_Syokum AS SYOKUM ON SYOKUM.SyokumuCdSeq = KYOSHE_GUI.SyokumuCdSeq '	+
		' AND SYOKUM.TenantCdSeq ='+ ISNULL(@TenantCdSeq,'0') 	+
		' JOIN VPM_Syain SYAIN ON SYAIN.SyainCdSeq= HAIIN_GUI.SyainCdSeq '	+ 
		' WHERE HAIIN_GUI.UkeNo ='+ ISNULL(@Ukeno,'0') 	+
		'		AND HAIIN_GUI.BunkRen ='+ ISNULL(@BunkRen,'0') 	+
		'		AND HAIIN_GUI.UnkRen= '+ ISNULL(@UnkRen,'0') 	+	
		'		AND HAIIN_GUI.TeiDanNo=' + ISNULL(@TeiDanNo,'0') 	+	
		'		AND HAIIN_GUI.SiyoKbn = 1 '	+	
		'		AND SYOKUM.SyokumuKbn IN (3,4) '	+
		' ORDER BY HAIIN_GUI.HaiInRen ASC '

		IF OBJECT_ID('tempdb.dbo.##tb_HaiinDRTemp', 'U') IS  NULL
			BEGIN
				CREATE TABLE ##tb_HaiinDRTemp ( 
								  rowIndex int IDENTITY(1,1)
								, UkeNo VARCHAR(15)
								, SyainCdSeq INT
								, TeiDanNo SMALLINT
								, SyokumuCdSeq INT
								, SyainCd CHAR(10)
								, SyainNm NVARCHAR(20)
							  )
  
			END
		IF OBJECT_ID('tempdb.dbo.##tb_HaiinGUITemp', 'U') IS  NULL
			BEGIN
				CREATE TABLE ##tb_HaiinGUITemp (
								  rowIndex int IDENTITY(1,1)
								, UkeNo VARCHAR(15)
								, SyainCdSeq INT
								, TeiDanNo SMALLINT
								, SyokumuCdSeq INT
								, SyainCd CHAR(10)
								, SyainNm NVARCHAR(20)
							  )							  
			END
		--INSERT @tb_HaiinDRTemp
	    INSERT INTO ##tb_HaiinDRTemp EXEC(@strSQL_DR)
		--INSERT INTO @tb_HaiinGUITemp
		INSERT INTO ##tb_HaiinGUITemp EXEC(@strSQL_GUI)
		--CHECK JOIN Table
		SELECT @CountRecordHaiinDR = COUNT(*) FROM ##tb_HaiinDRTemp
		SELECT @CountRecordHaiinGUI = COUNT(*) FROM ##tb_HaiinGUITemp

		SET @Condition = IIF(@CountRecordHaiinDR>=@CountRecordHaiinGUI,'LEFT','RIGHT')
		SET @strSQL=''
		SET @strSQL = @strSQL + 
		'SELECT ISNULL(HAIIN_DR.SyainCd,0) AS DR_SyainCd , '	+
		'		ISNULL(HAIIN_DR.SyainNm,'''') AS DR_SyainNm , '	+
		'		ISNULL(HAIIN_GUI.SyainCd,0) AS GUI_SyainCd , '	+
		'		ISNULL(HAIIN_GUI.SyainNm,'''') AS GUI_SyainNm , '	+ 
		'		ISNULL(HAIIN_DR.TeiDanNo,0) AS TeiDanNo '	+   
		'FROM ##tb_HaiinDRTemp HAIIN_DR '+ @Condition + ' JOIN ##tb_HaiinGUITemp HAIIN_GUI    
		ON HAIIN_DR.rowIndex=HAIIN_GUI.rowIndex'

		DECLARE @tb_HAIIN TABLE (	DR_SyainCd CHAR(255)
								  ,	DR_SyainNm NVARCHAR(255)
								  ,	GUI_SyainCd CHAR(255)
								  , GUI_SyainNm NVARCHAR(255)
								  ,	TeiDanNo SMALLINT	
								)
		INSERT INTO @tb_HAIIN EXEC(@strSQL)
		DELETE FROM ##tb_HaiinDRTemp
		DELETE FROM ##tb_HaiinGUITemp
		SELECT * FROM @tb_HAIIN		
		--SELECT @strSQL_DR
        SET @ROWCOUNT   =   @@ROWCOUNT
    END TRY

-- エラー処理
    BEGIN CATCH
		DROP TABLE ##tb_HaiinDRTemp;
		DROP TABLE ##tb_HaiinGUITemp;
	
        SET @ReturnCd   =   ERROR_NUMBER()
        SET @ReturnMsg  =   ERROR_MESSAGE()
        SET @eProcedure =   ERROR_PROCEDURE()
        SET @eLine      =   ERROR_LINE()		
    END CATCH	
    RETURN
GO


