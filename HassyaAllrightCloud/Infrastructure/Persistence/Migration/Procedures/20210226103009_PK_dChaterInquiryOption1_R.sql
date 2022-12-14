USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dChaterInquiryOption1_R]    Script Date: 3/1/2021 1:47:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetChaterInquiryAsync
-- Date			:   2020/10/20
-- Author		:   T.L.DUY
-- Description	:   Get ChaterInquiry data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dChaterInquiryOption1_R]
		(
		--Parameter
			@TenantCdSeq			INT,
			@UkeNo					NVARCHAR(15),				
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
WITH eTKD_Haisha AS (
     SELECT TKD_Haisha.UkeNo AS UkeNo,
          TKD_Haisha.UnkRen AS UnkRen,
          TKD_Haisha.YouTblSeq AS YouTblSeq,
          COUNT(*) AS Daisu
     FROM TKD_Haisha
     WHERE TKD_Haisha.SiyoKbn = 1
     GROUP BY TKD_Haisha.UkeNo,
          TKD_Haisha.UnkRen,
          TKD_Haisha.YouTblSeq
     )

SELECT ISNULL(eVPM_Gyosya04.GyosyaCd, 0) AS GyosyaCd,
     ISNULL(eVPM_Tokisk02.TokuiCd, 0) AS TokuiCd,
     ISNULL(eVPM_TokiSt03.SitenCd, 0) AS SitenCd,
     ISNULL(eVPM_Tokisk02.RyakuNm, '') AS TokuiRyak,
     ISNULL(eVPM_TokiSt03.RyakuNm, '') AS SitenRyak,
     ISNULL(eTKD_Haisha06.Daisu, 0) AS SyaSyuDai,
     ISNULL(eTKD_Mihrim.HaseiKin, 0) AS HaseiKin,
     ISNULL(eTKD_Mihrim.SyaRyoSyo, 0) AS SyaRyoSyo,
     ISNULL(eTKD_Mihrim.SyaRyoTes, 0) AS SyaRyoTes,
     ISNULL(eTKD_Yousha01.Zeiritsu, 0) AS Zeiritsu,
     ISNULL(eTKD_Yousha01.TesuRitu, 0) AS TesuRitu,
     ISNULL(eTKD_Mihrim.YoushaGak, 0) AS YoushaGak,
     ISNULL(eTKD_Mihrim.SihRaiRui, 0) AS SihRaiRui
FROM TKD_Mihrim AS eTKD_Mihrim
INNER JOIN TKD_Yyksho
     ON eTKD_Mihrim.UkeNo = TKD_Yyksho.UkeNo
     AND TKD_Yyksho.YoyaSyu = 1
LEFT JOIN TKD_Yousha AS eTKD_Yousha01
     ON eTKD_Mihrim.UkeNo = eTKD_Yousha01.UkeNo
     AND eTKD_Mihrim.UnkRen = eTKD_Yousha01.UnkRen
     AND eTKD_Mihrim.YouTblSeq = eTKD_Yousha01.YouTblSeq
     AND eTKD_Yousha01.SiyoKbn = 1
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02
     ON eTKD_Yousha01.YouCdSeq = eVPM_Tokisk02.TokuiSeq
     AND eTKD_Yousha01.HasYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
     AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq -- ログインユーザーのTenantCdSeq
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya04
     ON eVPM_Tokisk02.GyosyaCdSeq = eVPM_Gyosya04.GyosyaCdSeq
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt03
     ON eTKD_Yousha01.YouCdSeq = eVPM_TokiSt03.TokuiSeq
     AND eTKD_Yousha01.YouSitCdSeq = eVPM_TokiSt03.SitenCdSeq
     AND eTKD_Yousha01.HasYmd BETWEEN eVPM_TokiSt03.SiyoStaYmd AND eVPM_TokiSt03.SiyoEndYmd
LEFT JOIN eTKD_Haisha AS eTKD_Haisha06
     ON eTKD_Mihrim.UkeNo = eTKD_Haisha06.UkeNo
     AND eTKD_Mihrim.UnkRen = eTKD_Haisha06.UnkRen
     AND eTKD_Mihrim.YouTblSeq = eTKD_Haisha06.YouTblSeq
WHERE eTKD_Mihrim.UkeNo = @UkeNo -- 「Ukeno」パラメタ
     AND eTKD_Mihrim.SihFutSyu = 1
     AND eTKD_Mihrim.SiyoKbn = 1
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN
