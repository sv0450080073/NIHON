USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetBusType_R]    Script Date: 2021/04/27 11:02:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Pro_GetBusType_R]
    (		
		@ListBooking VARCHAR(MAX),		
		@TenantCdSeq VARCHAR(2)	
    )
AS
	BEGIN TRY 	
        DECLARE @strSQL NVARCHAR(MAX)=' '	
		DECLARE @BookingCondition NVARCHAR(MAX)=''
		SELECT @BookingCondition = ' AND YYKSYU.UkeNo IN (SELECT * FROM dbo.FN_SplitString('+dbo.FP_SetString(@ListBooking)+',''-''))'		
		SET @strSQL = @strSQL + 
		'SELECT  ISNULL(YYKSYU.UkeNo,'''')                 																						
       ,ISNULL(YYKSYU.UnkRen,0)                   																						
       ,ISNULL(YYKSYU.SyaSyuRen,0)              																					
	   ,ISNULL(YYKSYU.SyaSyuCdSeq,0)              																						
	   ,ISNULL(CONCAT(YYKSYU.SyaSyuDai,''‘ä''),'''') as   YYKSYU_SyaSyuDai 																						
	   ,ISNULL(SYASYU.SyaSyuNm,''Žw’è‚È‚µ'')                  																				
	   ,ISNULL(YYKSYU.SyaSyuTan,0)               																					
	   ,ISNULL(YYKSYU.SyaRyoUnc,0)                 																				
		FROM TKD_YykSyu AS YYKSYU																							
		LEFT JOIN VPM_SyaSyu AS SYASYU																							
       ON SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq																							
	  AND SYASYU.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'  																					
		WHERE YYKSYU.SiyoKbn = 1 '
		+ @BookingCondition	+'																				
		ORDER BY YYKSYU.UkeNo '	
	DECLARE  @tb_Temp TABLE (
							YYKSYU_Ukeno NVARCHAR(15)
							, YYKSYU_UnkRen NVARCHAR(255)
							, YYKSYU_SyaSyuRen NVARCHAR(255)
							, YYKSYU_SyaSyuCdSeq NVARCHAR(255)
							, YYKSYU_SyaSyuDai NVARCHAR(255)
							, SYASYU_SyaSyuNm NVARCHAR(255)
							, YYKSYU_SyaSyuTan NVARCHAR(255)
							, YYKSYU_SyaRyoUnc NVARCHAR(255)					
							)
	INSERT INTO @tb_Temp EXEC (@strSQL)
	SELECT  * FROM @tb_Temp
	--SELECT @strSQL
    END TRY
    BEGIN CATCH			
    END CATCH	
    RETURN
GO


