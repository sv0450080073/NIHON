USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetMFutTu_R]    Script Date: 2021/05/17 8:10:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE OR ALTER   PROCEDURE [dbo].[Pro_GetMFutTu_R]
    (        
        @UkeNo VARCHAR(15),
        @Unkren smallint,
        @BunkRen smallint,
		@TeiDanNo smallint,
		@Mode tinyint
    )
AS
    BEGIN TRY     
        DECLARE  @tb_Temp TABLE (
                                FUTTUM_HasYmd  char(8),
								FUTTUM_UnkRen smallint,
								FUTTUM_FutTumKbn tinyint,
								FUTTUM_FutTumRen smallint,
								FUTTUM_FutTumNm nvarchar(50),
								FUTTUM_SeisanNm nvarchar(50),
								FUTTUM_FutTumCdSeq int,
								MFUTTU_Suryo smallint,
								MFUTTU_TeiDanNo smallint,
								MFUTTU_BunkRen smallint
                            )
        INSERT INTO @tb_Temp 
                    SELECT FUTTUM.HasYmd,																						
						   FUTTUM.UnkRen,																						
						   FUTTUM.FutTumKbn,																						
							   FUTTUM.FutTumRen,																						
						   FUTTUM.FutTumNm,																						
							   FUTTUM.SeisanNm,																						
							   FUTTUM.FutTumCdSeq,																						
							   MFUTTU.Suryo,																						
							   MFUTTU.TeiDanNo,																						
							   MFUTTU.BunkRen																						
					FROM TKD_MFutTu AS MFUTTU																						
					LEFT JOIN TKD_FutTum AS FUTTUM																						
						   ON FUTTUM.UkeNo = MFUTTU.UkeNo																						
							  AND FUTTUM.UnkRen = MFUTTU.UnkRen																						
							  AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn																						
							  AND FUTTUM.FutTumRen = MFUTTU.FutTumRen																						
					WHERE MFUTTU.UkeNo =  @UkeNo          --3/　運行指示・乗務記録情報取得で取得した受付番号			
					AND MFUTTU.UnkRen=@Unkren
					AND MFUTTU.TeiDanNo=@TeiDanNo
					AND MFUTTU.BunkRen=@BunkRen
					  AND MFUTTU.SiyoKbn = 1																						
					  AND FUTTUM.SiyoKbn = 1
					  AND FUTTUM.FutTumKbn=@Mode
        SELECT  * FROM @tb_Temp
    --SELECT @strSQL
    END TRY
    -- エラー処理
    BEGIN CATCH
    END CATCH
    RETURN

GO


