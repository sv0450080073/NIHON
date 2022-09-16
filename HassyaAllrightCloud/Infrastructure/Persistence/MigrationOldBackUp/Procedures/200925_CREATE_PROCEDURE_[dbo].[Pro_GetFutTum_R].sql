USE [HOC_Kashikiri_New]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetFutTum_R]    Script Date: 9/25/2020 9:03:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Pro_GetFutTum_R]
    (		
		@ListBooking VARCHAR(MAX),		
		--@TenantCdSeq		varchar(2),
		@FutTumKbn VARCHAR(2)
    )
AS
	BEGIN TRY 	
        DECLARE @strSQL NVARCHAR(MAX)=' '	
		DECLARE @BookingCondition NVARCHAR(MAX)='',
				@FutTumKbnCondition VARCHAR(MAX) =''
		SELECT @BookingCondition = ' AND FUTTUM.UkeNo IN (SELECT * FROM dbo.FN_SplitString('+dbo.FP_SetString(@ListBooking)+',''-''))'		
		SET @strSQL = @strSQL + 
		'SELECT ISNULL(FUTTUM.UkeNo,'''')        																				
		,ISNULL(FUTTUM.UnkRen,0)     																			
		,ISNULL(FUTTUM.FutTumKbn,0)    																				
		,ISNULL(FUTTUM.FutTumRen,0)   																		
		,ISNULL(FUTTUM.Nittei,0)       																			
		,ISNULL(FUTTUM.FutTumCdSeq,0)  																				
		,ISNULL(FUTTUM.FutTumNm,'''')     																	
		,ISNULL(FUTTUM.SeisanCdSeq,0)  																			
		,ISNULL(FUTTUM.SeisanNm,'''')     																			
		,FORMAT(FUTTUM.Suryo,''##,###'')       																			
		,ISNULL(CONCAT(FORMAT(FUTTUM.TanKa,''##,###''),'' â~''),'''')      																		
		,ISNULL(CONCAT(FORMAT(FUTTUM.UriGakKin,''##,###''),'' â~''),'''')  																		
		,ISNULL(FORMAT(CAST(FUTTUM.HasYmd AS date),''MM/dd''),'''')      																				
		FROM TKD_FutTum AS FUTTUM																					
		WHERE FUTTUM.SiyoKbn = 1																		
		'+@BookingCondition+'	
		AND FUTTUM.FutTumKbn ='+ISNULL(@FutTumKbn,'0')+'  
		ORDER BY FUTTUM.UkeNo ASC																					
        ,FUTTUM.UnkRen ASC																					
		,FUTTUM.FutTumKbn ASC																			
		,FUTTUM.FutTumRen ASC																			
		,FUTTUM.Nittei ASC																			
		,FUTTUM.HasYmd ASC'
	DECLARE  @tb_Temp TABLE (
							 FUTTUM_UkeNo NVARCHAR(15)
							, FUTTUM_UnkRen  VARCHAR(10)
							, FUTTUM_FutTumKbn VARCHAR(10)
							, FUTTUM_FutTumRen     VARCHAR(10)
							, FUTTUM_Nittei NVARCHAR(10)
							, FUTTUM_FutTumCdSeq NVARCHAR(10)		
							, FUTTUM_FutTumNm NVARCHAR(255)
							, FUTTUM_SeisanCdSeq     VARCHAR(10)
							, FUTTUM_SeisanNm NVARCHAR(255)
							, FUTTUM_Suryo NVARCHAR(255)		
							, FUTTUM_TanKa     VARCHAR(255)
							, FUTTUM_UriGakKin NVARCHAR(255)
							, FUTTUM_HasYmd NVARCHAR(10)	
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


