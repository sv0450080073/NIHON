USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_GetFutTum_R]    Script Date: 22/10/2020 10:27:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Pro_GetFutTum_R]
    (		
		@ListBooking VARCHAR(MAX),
		@ListUnkRen VARCHAR(MAX),
		--@TenantCdSeq		varchar(2),
		@FutTumKbn VARCHAR(2)
    )
AS
	BEGIN TRY 	
		DECLARE @FutTumKbnCondition VARCHAR(MAX) =''
        DECLARE @strSQL NVARCHAR(MAX)='
		DECLARE @Temp TABLE(UkeNo nchar(15), UnkRen smallint)
		insert into @Temp(UkeNo, UnkRen)
					select ukeNoList.splitdata, unkRenList.splitdata
					from (SELECT ROW_NUMBER() OVER (ORDER BY (select null)) row_num, * FROM dbo.FN_SplitString('+dbo.FP_SetString(@ListBooking)+',''-'')) ukeNoList
					join (SELECT ROW_NUMBER() OVER (ORDER BY (select null)) row_num, * FROM dbo.FN_SplitString('+dbo.FP_SetString(@ListUnkRen)+',''-'')) unkRenList
					on ukeNoList.row_num = unkRenList.row_num'	
		SET @strSQL = @strSQL + ' ' +
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
		,ISNULL(CONCAT(FORMAT(FUTTUM.TanKa,''##,###''),'' 円''),'''')
		,ISNULL(CONCAT(FORMAT(FUTTUM.UriGakKin,''##,###''),'' 円''),'''')
		,ISNULL(FORMAT(CAST(FUTTUM.HasYmd AS date),''MM/dd''),'''')
		FROM TKD_FutTum AS FUTTUM
		JOIN @Temp TEMP on FUTTUM.UkeNo=TEMP.UkeNo and FUTTUM.UnkRen=TEMP.UnkRen
		WHERE FUTTUM.SiyoKbn = 1
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
	-- エラー処理
    BEGIN CATCH		
    END CATCH	
    RETURN
