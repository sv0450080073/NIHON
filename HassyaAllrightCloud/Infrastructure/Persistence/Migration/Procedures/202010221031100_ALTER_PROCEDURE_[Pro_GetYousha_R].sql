USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_GetYousha_R]    Script Date: 22/10/2020 10:27:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Pro_GetYousha_R]
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
		'SELECT ISNULL(YOUSHA.UkeNo,'''')         																	
			,ISNULL(YOUSHA.UnkRen,0)         																		
			,ISNULL(YOUSHA.YouCdSeq,0)      															
			,ISNULL(YOUSHA.YouSitCdSeq,0)    																	
			,COUNT(*)             																
			,ISNULL(CONCAT(TOKISK.RyakuNm, TOKIST.RyakuNm),'''')    																	
		FROM TKD_Yousha AS YOUSHA																		
		LEFT JOIN VPM_Tokisk AS TOKISK																		
			ON TOKISK.TokuiSeq = YOUSHA.YouCdSeq																		
			AND TOKISK.TenantCdSeq ='+ISNULL(@TenantCdSeq,'0')+' 																
		LEFT JOIN VPM_TokiSt AS TOKIST																		
			ON TOKIST.TokuiSeq = YOUSHA.YouCdSeq																		
			AND TOKIST.SitenCdSeq = YOUSHA.YouSitCdSeq
		JOIN @Temp TEMP on YOUSHA.UkeNo=TEMP.UkeNo and YOUSHA.UnkRen=TEMP.UnkRen
		WHERE YOUSHA.SiyoKbn = 1
		GROUP BY YOUSHA.UkeNo																		
				,YOUSHA.UnkRen																		
				,YOUSHA.YouCdSeq																
				,YOUSHA.YouSitCdSeq																	
				,TOKISK.RyakuNm																
				,TOKIST.RyakuNm	'	
	DECLARE  @tb_Temp TABLE (
							 YOUSHA_UkeNo VARCHAR(15)
							, YOUSHA_UnkRen  VARCHAR(10)
							, YOUSHA_YouCdSeq VARCHAR(10)
							, YOUSHA_YouSitCdSeq     VARCHAR(10)
							, YOUSHA_Count NVARCHAR(10)
							, YOUSHA_Nm NVARCHAR(255)									
							)
	INSERT INTO @tb_Temp EXEC (@strSQL)
	SELECT  * FROM @tb_Temp
	--SELECT @strSQL   
    END TRY
    BEGIN CATCH		
    END CATCH	
    RETURN
