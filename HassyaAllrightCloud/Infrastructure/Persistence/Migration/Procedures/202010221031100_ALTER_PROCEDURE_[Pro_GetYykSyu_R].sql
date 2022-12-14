USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_GetYykSyu_R]    Script Date: 22/10/2020 10:27:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER   PROCEDURE [dbo].[Pro_GetYykSyu_R]
    (		
		@ListBooking VARCHAR(MAX),
		@ListUnkRen VARCHAR(MAX),
		@TenantCdSeq VARCHAR(2)		
    )
AS
	BEGIN TRY 	
        DECLARE @strSQL NVARCHAR(MAX)='
		DECLARE @Temp TABLE(UkeNo nchar(15), UnkRen smallint)
		insert into @Temp(UkeNo, UnkRen)
					select ukeNoList.splitdata, unkRenList.splitdata
					from (SELECT ROW_NUMBER() OVER (ORDER BY (select null)) row_num, * FROM dbo.FN_SplitString('+dbo.FP_SetString(@ListBooking)+',''-'')) ukeNoList
					join (SELECT ROW_NUMBER() OVER (ORDER BY (select null)) row_num, * FROM dbo.FN_SplitString('+dbo.FP_SetString(@ListUnkRen)+',''-'')) unkRenList
					on ukeNoList.row_num = unkRenList.row_num'
		SET @strSQL = @strSQL + ' ' +
		'SELECT  ISNULL(YYKSYU.UkeNo,'''')                    																						
				 ,ISNULL(YYKSYU.UnkRen,0)                    																							
				 ,ISNULL(YYKSYU.SyaSyuRen,0)                																								
				 ,ISNULL(YYKSYU.SyaSyuCdSeq,0)              																						
				,ISNULL(CONCAT(YYKSYU.SyaSyuDai,''台''),'''')    																						
				,ISNULL(SYASYU.SyaSyuNm,'''')                 																					
				,ISNULL(YYKSYU.SyaSyuTan,0)                 																						
				,ISNULL(YYKSYU.SyaRyoUnc,0)               																					
		FROM TKD_YykSyu AS YYKSYU																								
		LEFT JOIN VPM_SyaSyu AS SYASYU																								
			ON SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq																								
			AND SYASYU.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+'
		JOIN @Temp TEMP on YYKSYU.UkeNo=TEMP.UkeNo and YYKSYU.UnkRen=TEMP.UnkRen
		WHERE YYKSYU.SiyoKbn = 1
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
	-- エラー処理
    BEGIN CATCH		
    END CATCH	
    RETURN
