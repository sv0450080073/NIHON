USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetFutTum_R]    Script Date: 2021/04/27 11:12:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Pro_GetFutTum_R]
    (        
        @ListBooking VARCHAR(MAX),
        @ListUnkRen VARCHAR(MAX),
        --@TenantCdSeq        varchar(2),
        @FutTumKbn VARCHAR(2)
    )
AS
    BEGIN TRY     
        DECLARE @Temp TABLE(UkeNo nchar(15), UnkRen smallint)
        insert into @Temp(UkeNo, UnkRen)
                    select ukeNoList.splitdata, unkRenList.splitdata
                    from (SELECT ROW_NUMBER() OVER (ORDER BY (select null)) row_num, * FROM dbo.FN_SplitString(@ListBooking,'-')) ukeNoList
                    join (SELECT ROW_NUMBER() OVER (ORDER BY (select null)) row_num, * FROM dbo.FN_SplitString(@ListUnkRen,'-')) unkRenList
                    on ukeNoList.row_num = unkRenList.row_num
        --Select * from @Temp

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
        INSERT INTO @tb_Temp 
                    SELECT ISNULL(FUTTUM.UkeNo,'')
                    ,ISNULL(FUTTUM.UnkRen,0)
                    ,ISNULL(FUTTUM.FutTumKbn,0)
                    ,ISNULL(FUTTUM.FutTumRen,0)
                    ,ISNULL(FUTTUM.Nittei,0)
                    ,ISNULL(FUTTUM.FutTumCdSeq,0)
                    ,ISNULL(FUTTUM.FutTumNm,'')
                    ,ISNULL(FUTTUM.SeisanCdSeq,0)
                    ,ISNULL(FUTTUM.SeisanNm,'')
                    ,FORMAT(FUTTUM.Suryo,'##,###')
                    ,ISNULL(CONCAT(FORMAT(FUTTUM.TanKa,'##,###'),' â~'),'')
                    ,ISNULL(CONCAT(FORMAT(FUTTUM.UriGakKin,'##,###'),' â~'),'')
                    ,ISNULL(FORMAT(CAST(FUTTUM.HasYmd AS date),'MM/dd'),'''')
                    FROM TKD_FutTum AS FUTTUM
                    JOIN @Temp TEMP on FUTTUM.UkeNo=TEMP.UkeNo and FUTTUM.UnkRen=TEMP.UnkRen
                    WHERE FUTTUM.SiyoKbn = 1
                    AND FUTTUM.FutTumKbn = ISNULL(@FutTumKbn,'0')
                    ORDER BY FUTTUM.UkeNo ASC
                    ,FUTTUM.UnkRen ASC
                    ,FUTTUM.FutTumKbn ASC
                    ,FUTTUM.FutTumRen ASC
                    ,FUTTUM.Nittei ASC
                    ,FUTTUM.HasYmd ASC
        SELECT  * FROM @tb_Temp
    --SELECT @strSQL
    END TRY
    -- ÉGÉâÅ[èàóù
    BEGIN CATCH
    END CATCH
    RETURN
GO


