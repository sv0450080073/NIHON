USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetDriverAndGuiList_R]    Script Date: 6/2/2021 4:47:26 PM ******/
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
ALTER PROCEDURE [dbo].[Pro_GetDriverAndGuiList_R]
    (
		@Ukeno VARCHAR(15)
	,	@UnkRen VARCHAR(10)
	,	@TeiDanNo VARCHAR(10)
	,	@BunkRen VARCHAR(10)
	,	@HaiShaSyuKoYmd VARCHAR(10)
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
			   ,@strSQL_DR NVARCHAR(MAX)=''

        SET @ReturnCd   =   0     
        SET @ROWCOUNT   =   0       
        SET @ReturnMsg  =   ' '    
        SET @eProcedure =   ' '    
        SET @eLine      =   ' '     
		/*GET SYAIN AND POSITION*/
        SET @strSQL_DR = @strSQL_DR +
		'SELECT 
			ISNULL([1],0)
		,	ISNULL([2],0)
		,	ISNULL([3],0)
		,	ISNULL([4],0)
		,	ISNULL([5],0)
		FROM (
			SELECT
				ISNULL(HAIIN_DRV.SyainCdSeq,0)AS SyainCdSeq
			,	ROW_NUMBER() OVER ( PARTITION BY HAIIN_DRV.UkeNo order by HAIIN_DRV.HaiInRen ) AS row_Num
			FROM TKD_Haiin HAIIN_DRV  
			JOIN TKD_Haisha AS HAISHA 	
				ON HAISHA.UkeNo    = HAIIN_DRV.UkeNo	 
				AND HAISHA.UnkRen = HAIIN_DRV.UnkRen
				AND HAISHA.TeiDanNo = HAIIN_DRV.TeiDanNo
				AND HAISHA.BunkRen = HAIIN_DRV.BunkRen
				WHERE HAISHA.UkeNo  ='+ dbo.FP_SetString(ISNULL(@Ukeno,'0')) +' /*' + ISNULL(@Ukeno,'0') + '*/
				And HAIIN_DRV.SiyoKbn =1
				AND  HAIIN_DRV.BunkRen  ='+ ISNULL(@BunkRen,'0') 	+'
				AND HAIIN_DRV.UnkRen= '+ ISNULL(@UnkRen,'0') 	+	'
				AND HAIIN_DRV.TeiDanNo=' + ISNULL(@TeiDanNo,'0') 	+	'
			) AS tb_SOURCE
		PIVOT
			(MAX(SyainCdSeq)
			FOR row_Num IN ([1],[2],[3],[4],[5])) AS tb_Pivot '

		DECLARE @tb_Haiin TABLE (
									SYAIN01 INT
								,	SYAIN02 INT
								,	SYAIN03 INT
								,	SYAIN04 INT
								,	SYAIN05 INT
								)
		INSERT INTO @tb_Haiin EXEC (@strSQL_DR)
		--select  @strSQL_DR
		/*Result*/
		SELECT 
				ISNULL(SYAIN_DRV01.SyainNm+ ' ( '+SYOKUM.SyokumuNm + ' ) ','') AS Name_Position1  
			,	ISNULL(SYAIN_DRV02.SyainNm+ ' ( '+SYOKUM02.SyokumuNm + ' ) ' ,'')AS Name_Position2   
			,	ISNULL(SYAIN_DRV03.SyainNm+ ' ( '+SYOKUM03.SyokumuNm + ' ) ','') AS Name_Position3   
			,	ISNULL(SYAIN_DRV04.SyainNm+ ' ( '+SYOKUM04.SyokumuNm + ' ) ','') AS Name_Position4  
			,	ISNULL(SYAIN_DRV05.SyainNm+ ' ( '+SYOKUM05.SyokumuNm + ' ) ','') AS Name_Position5  
		FROM  @tb_Haiin HAIIN  
		/*Query01*/	
		LEFT JOIN VPM_KyoSHe AS KYOSHE01 	
			ON KYOSHE01.SyainCdSeq = HAIIN.SYAIN01
			AND KYOSHE01.StaYmd   <= dbo.FP_SetString(@HaiShaSyuKoYmd)																					
			AND KYOSHE01.EndYmd >= dbo.FP_SetString(@HaiShaSyuKoYmd)					
		LEFT JOIN VPM_Syokum AS SYOKUM																								
			ON SYOKUM.SyokumuCdSeq = KYOSHE01.SyokumuCdSeq
			and SYOKUM.TenantCdSeq =@TenantCdSeq
		LEFT JOIN VPM_Syain AS SYAIN_DRV01																								
			ON SYAIN_DRV01.SyainCdSeq = HAIIN.SYAIN01
		/*Query02*/	 
		LEFT JOIN VPM_KyoSHe AS KYOSHE02 	
			ON KYOSHE02.SyainCdSeq = HAIIN.SYAIN02
			AND KYOSHE02.StaYmd <= dbo.FP_SetString(@HaiShaSyuKoYmd)																					
			AND KYOSHE02.EndYmd >= dbo.FP_SetString(@HaiShaSyuKoYmd)					
		LEFT JOIN VPM_Syokum AS SYOKUM02																								
			ON SYOKUM02.SyokumuCdSeq = KYOSHE02.SyokumuCdSeq
			and SYOKUM02.TenantCdSeq =@TenantCdSeq
		LEFT JOIN VPM_Syain AS SYAIN_DRV02																								
			ON SYAIN_DRV02.SyainCdSeq = HAIIN.SYAIN02
		/*Query03*/	 
		LEFT JOIN VPM_KyoSHe AS KYOSHE03 	
			ON KYOSHE03.SyainCdSeq = HAIIN.SYAIN03
			AND KYOSHE03.StaYmd <= dbo.FP_SetString(@HaiShaSyuKoYmd)																					
			AND KYOSHE03.EndYmd >= dbo.FP_SetString(@HaiShaSyuKoYmd)					
		LEFT JOIN VPM_Syokum AS SYOKUM03																								
			ON SYOKUM03.SyokumuCdSeq = KYOSHE03.SyokumuCdSeq			
			and SYOKUM03.TenantCdSeq =@TenantCdSeq
		LEFT JOIN VPM_Syain AS SYAIN_DRV03																								
			ON SYAIN_DRV03.SyainCdSeq = HAIIN.SYAIN03
		/*Query04*/	
		LEFT JOIN VPM_KyoSHe AS KYOSHE04 	
			ON KYOSHE04.SyainCdSeq = HAIIN.SYAIN04
			AND KYOSHE04.StaYmd <= dbo.FP_SetString(@HaiShaSyuKoYmd)																					
			AND KYOSHE04.EndYmd >= dbo.FP_SetString(@HaiShaSyuKoYmd)					
		LEFT JOIN VPM_Syokum AS SYOKUM04																								
			ON SYOKUM04.SyokumuCdSeq = KYOSHE04.SyokumuCdSeq		
			and SYOKUM04.TenantCdSeq =@TenantCdSeq
		LEFT JOIN VPM_Syain AS SYAIN_DRV04																								
			ON SYAIN_DRV04.SyainCdSeq = HAIIN.SYAIN04
		/*Query05*/
		LEFT JOIN VPM_KyoSHe AS KYOSHE05 	
			ON KYOSHE05.SyainCdSeq = HAIIN.SYAIN05
			AND KYOSHE05.StaYmd <= dbo.FP_SetString(@HaiShaSyuKoYmd)																					
			AND KYOSHE05.EndYmd >= dbo.FP_SetString(@HaiShaSyuKoYmd)					
		LEFT JOIN VPM_Syokum AS SYOKUM05																								
			ON SYOKUM05.SyokumuCdSeq = KYOSHE05.SyokumuCdSeq
			and SYOKUM05.TenantCdSeq =@TenantCdSeq
		LEFT JOIN VPM_Syain AS SYAIN_DRV05																								
			ON SYAIN_DRV05.SyainCdSeq = HAIIN.SYAIN05
		WHERE SYOKUM.TenantCdSeq  =ISNULL(@TenantCdSeq,0)
			AND SYOKUM.SiyoKbn = 1																								
			AND SYOKUM.JigyoKbn = 1                             																							
			AND SYOKUM.SyokumuKbn IN (1,2,3,4) 
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


