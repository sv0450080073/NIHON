USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dChaterInquiryOtherOption_R]    Script Date: 3/1/2021 1:48:21 PM ******/
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
CREATE OR ALTER PROCEDURE [dbo].[PK_dChaterInquiryOtherOption_R]
		(
		--Parameter
			@TenantCdSeq			INT,
			@SeiFutSyu				TINYINT,
			@FutuUnkRen				SMALLINT,
			@UkeNo					NVARCHAR(15),				
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
SELECT ISNULL(eVPM_Gyosya05.GyosyaCd, 0) AS GyosyaCd,
     ISNULL(eVPM_Tokisk03.TokuiCd, 0) AS TokuiCd,
     ISNULL(eVPM_TokiSt04.SitenCd, 0) AS SitenCd,
     ISNULL(eVPM_Tokisk03.RyakuNm, '') AS TokuiRyak,
     ISNULL(eVPM_TokiSt04.RyakuNm, '') AS SitenRyak,
     ISNULL(eTKD_YFutTu01.Suryo, 0) AS SyaSyuDai,
     ISNULL(eTKD_Mihrim.HaseiKin, 0) AS HaseiKin,
     ISNULL(eTKD_Mihrim.SyaRyoSyo, 0) AS SyaRyoSyo,
     ISNULL(eTKD_Mihrim.SyaRyoTes, 0) AS SyaRyoTes,
     ISNULL(eTKD_YFutTu01.Zeiritsu, 0) AS Zeiritsu,
     ISNULL(eTKD_YFutTu01.TesuRitu, 0) AS TesuRitu,
     ISNULL(eTKD_Mihrim.YoushaGak, 0) AS YoushaGak,
     ISNULL(eTKD_Mihrim.SihRaiRui, 0) AS SihRaiRui
FROM TKD_Mihrim AS eTKD_Mihrim
INNER JOIN TKD_Yyksho
     ON eTKD_Mihrim.UkeNo = TKD_Yyksho.UkeNo
     AND TKD_Yyksho.YoyaSyu = 1
INNER JOIN TKD_YFutTu AS eTKD_YFutTu01
     ON eTKD_Mihrim.UkeNo = eTKD_YFutTu01.UkeNo
     AND eTKD_Mihrim.UnkRen = eTKD_YFutTu01.UnkRen
     AND eTKD_Mihrim.YouTblSeq = eTKD_YFutTu01.YouTblSeq
     AND eTKD_Mihrim.YouFutTumRen = eTKD_YFutTu01.YouFutTumRen
     AND eTKD_YFutTu01.FutTumKbn = CASE WHEN @SeiFutSyu = 6 THEN 2 ELSE 1 END
     AND eTKD_YFutTu01.SiyoKbn = 1
LEFT JOIN TKD_Yousha AS eTKD_Yousha02
     ON eTKD_Mihrim.UkeNo = eTKD_Yousha02.UkeNo
     AND eTKD_Mihrim.UnkRen = eTKD_Yousha02.UnkRen
     AND eTKD_Mihrim.YouTblSeq = eTKD_Yousha02.YouTblSeq
     AND eTKD_Yousha02.SiyoKbn = 1
LEFT JOIN VPM_Tokisk AS eVPM_Tokisk03
     ON eTKD_Yousha02.YouCdSeq = eVPM_Tokisk03.TokuiSeq
     AND eTKD_Yousha02.HasYmd BETWEEN eVPM_Tokisk03.SiyoStaYmd AND eVPM_Tokisk03.SiyoEndYmd
     AND eVPM_Tokisk03.TenantCdSeq = @TenantCdSeq -- ログインユーザーのTenantCdSeq
LEFT JOIN VPM_Gyosya AS eVPM_Gyosya05
     ON eVPM_Tokisk03.GyosyaCdSeq = eVPM_Gyosya05.GyosyaCdSeq
LEFT JOIN VPM_TokiSt AS eVPM_TokiSt04
     ON eTKD_Yousha02.YouCdSeq = eVPM_TokiSt04.TokuiSeq
     AND eTKD_Yousha02.YouSitCdSeq = eVPM_TokiSt04.SitenCdSeq
     AND eTKD_Yousha02.HasYmd BETWEEN eVPM_TokiSt04.SiyoStaYmd AND eVPM_TokiSt04.SiyoEndYmd
WHERE eTKD_Mihrim.UkeNo = @UkeNo -- 「Ukeno」パラメタ
    AND eTKD_Mihrim.SihFutSyu = @SeiFutSyu -- 「SeiFutSyu」パラメタ
    AND eTKD_Mihrim.UnkRen = @FutuUnkRen -- 「FutuUnkRen」パラメタ
    AND eTKD_Mihrim.SiyoKbn = 1
SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN
