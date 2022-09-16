USE [HOC_Kashikiri_New]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetYykSyu_R]    Script Date: 9/25/2020 9:05:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Pro_GetYykSyu_R]
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
				,ISNULL(CONCAT(YYKSYU.SyaSyuDai,''ë‰''),'''')    																						
				,ISNULL(SYASYU.SyaSyuNm,'''')                 																					
				,ISNULL(YYKSYU.SyaSyuTan,0)                 																						
				,ISNULL(YYKSYU.SyaRyoUnc,0)               																					
		FROM TKD_YykSyu AS YYKSYU																								
		LEFT JOIN VPM_SyaSyu AS SYASYU																								
			ON SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq																								
			AND SYASYU.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'																							
		WHERE YYKSYU.SiyoKbn = 1
			'+@BookingCondition+'
		ORDER BY YYKSYU.UkeNo'
	DECLARE  @tb_Temp TABLE (
							 YYKSYU_UkeNo NVARCHAR(15)
							, YYKSYU_UnkRen  NVARCHAR(10)
							, YYKSYU_SyaSyuRen NVARCHAR(10)
							, YYKSYU_SyaSyuCdSeq     NVARCHAR(10)
							, YYKSYU_SyaSyuDai NVARCHAR(255)
							, SYASYU_SyaSyuNm NVARCHAR(255)		
							, YYKSYU_SyaSyuTan NVARCHAR(255)
							, YYKSYU_SyaRyoUnc     NVARCHAR(255)							
							)
	INSERT INTO @tb_Temp EXEC (@strSQL)
	SELECT  * FROM @tb_Temp
	--SELECT @strSQL
    END TRY
	-- ÉGÉâÅ[èàóù
    BEGIN CATCH		
    END CATCH	
    RETURN
GO


