USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_CancelList_R]    Script Date: 11/19/2020 10:05:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Pro_CancelList_R] 
	@bookingKeys Tblt_BookingKeyType READONLY,
	@tenantId int = 1 
AS
BEGIN
SELECT  Yo.UkeCd																												
       ,Un.UnkRen																											
       ,Yo.YoyaKbnSeq
	   ,YoyaKbn.YoyaKbn AS YoyaCode
       ,YoyaKbn.YoyaKbnNm																												
       ,Yo.UkeYmd																											
       ,Toku.RyakuNm AS TokuRyakuNm																										
       ,Toku.TokuiCd TokuTokuiCd																																																						
       ,Yo.TokuiTanNm AS TokuiTanStaff																																																							
       ,Toshi.SitenCd AS ToshiSitenCd
	   ,Toshi.SitenNm AS ToshiSitenNm
       ,Toshi.RyakuNm AS ToshiRyakuNm																																																							
       ,Gyosa.GyosyaCd AS GyosyaCd
	   ,Gyosa.GyosyaNm  AS GyosyaNm
	   ,Gosa.GyosyaCd AS GosaCd
	   ,Gosa.GyosyaNm  AS GosaNm
	   ,Toki.TokuiCd  AS TokiTokuiCd
	   ,Toki.TokuiNm AS TokiTokuiNm
       ,Toki.RyakuNm AS TokiRyakuNm		
	   ,Toshiten.SitenCd AS ToshitenCd
	   ,Toshiten.SitenNm AS ToshitenNm
       ,Toshiten.RyakuNm AS ToshitenRyakuNm																												
       ,Yo.CanYmd AS CancelYmd	
	   ,Shain.SyainCd AS CanTanSyainCd
       ,Shain.SyainNm AS CanTanSyainNm	
	   ,Egos.EigyoCd AS EigyoCd
       ,Egos.EigyoNm AS CanTanEigyoRyakuNm
	   ,Egos.RyakuNm AS EgosRyakuNm
       ,Yo.YoyaNm AS YoyaNm																												
       ,Yo.CanRiy AS CanRiy																												
       ,Un.DrvJin AS DrvJin																												
       ,Un.GuiSu AS GuiSu		
	   ,Un.HaiIKbn AS HaiIKbn
       ,Yo.KaktYmd AS KaktYmd
	   ,Yo.KaknKais AS KaknKais
	   ,Un.HaiSKbn AS HaiSkbn
	   ,'キャンセル' AS FixedText																												
       ,Yo.CanUnc AS CancelFee																												
       ,Yo.CanSyoG AS CancelTax	
	   ,Yo.CanZKbn AS CanZCodeKbn
	   ,Yo.CanSyoR AS CanSyoR
       ,Un.HaiSYmd AS HaiSYmd																												
       ,Un.HaiSTime AS HaiSTime																												
　　　  ,Un.HaiSNm AS HaiSNm																												
       ,Un.TouYmd AS TouYmd																												
       ,Un.TouChTime AS TouChTime																												
　　　  ,Un.TouNm AS TouNm																												
       ,Un.DanTaNm AS DanTaNm																												
       ,Un.IkNm AS IkNm																												
       ,Un.KanJNm AS KanJNm	
	   ,Sashu.SyaSyuCd AS SyaSyuCd
       ,ISNULL(Sashu.SyaSyuNm, '指定なし') AS SyaSyuNm	
	   ,Shu.KataKbn AS KataCode
       ,Shu.SyaSyuDai AS SyaSyuDai	
	   ,Shu.UnitBusPrice AS UnitBusPrice
	   ,Un.JyoSyaJin AS JyoSyaJin
	   ,Un.PlusJin AS PlusJin
       ,Yo.UntKin AS UntKin	
	   ,Yo.ZeiKbn AS ZeiCodeKbn
	   ,Yo.Zeiritsu AS Zeiritsu
       ,Yo.ZeiRui AS ZeiRui	
	   ,Yo.TesuRitu AS TesuRitu
       ,Yo.TesuRyoG AS TesuRyoG
	   ,Yo.CanRit AS CanRit
	   ,Ei.EigyoCd AS UkeEigyoCd
	   ,Ei.EigyoNm AS UkeEigyoNm
       ,Ei.RyakuNm AS UkeEigyoRyakuNm	
	   ,Sain.SyainCd AS EigTanSyainCd
       ,Sain.SyainNm AS EigTanSyainNm		
	   ,Syain.SyainCd AS InputTanSyainCd
       ,Syain.SyainNm AS InputTanSyainNm
	   ,Toku.SiyoStaYmd  AS TokuSiyoStaYmd 
	   ,Toku.SiyoEndYmd AS TokuSiyoEndYmd
	   ,Toshi.SiyoStaYmd AS ToshiSiyoStaYmd 
	   ,Toshi.SiyoEndYmd AS ToshiSiyoEndYmd
 ,(SELECT Top 1 TKD_SeiPrS.SeiHatYmd                 																												
              FROM TKD_SeiMei 																												
         LEFT JOIN TKD_SeiPrS																												
                ON TKD_SeiPrS.SeiOutSeq = TKD_SeiMei.SeiOutSeq																												
               AND TKD_SeiPrS.SiyoKbn = 1																												
         WHERE TKD_SeiMei.SiyoKbn = 1																												
           AND TKD_SeiMei.UkeNo = Yo.UkeNo																												
         ORDER BY SeiHatYmd DESC)  AS SeiHatYmd	
FROM @bookingKeys as b
INNER JOIN TKD_Unkobi AS Un	
	   ON b.UkeNo = Un.UkeNo
LEFT JOIN TKD_Yyksho AS Yo																												
       ON Yo.UkeNo=Un.UkeNo																												
LEFT JOIN TKD_YykSyu AS Shu																												
       ON Shu.UkeNo=Un.UkeNo 																												
INNER JOIN VPM_Tokisk AS Toku																												
       ON Yo.TokuiSeq=Toku.TokuiSeq 																												
INNER JOIN VPM_TokiSt AS Toshi　																												
      ON Yo.SitenCdSeq=Toshi.SeiSitenCdSeq And Yo.TokuiSeq = Toshi.TokuiSeq																												
INNER JOIN VPM_Tokisk AS Toki																												
       ON Yo.SirCdSeq=Toki.TokuiSeq 																												
INNER JOIN VPM_TokiSt AS Toshiten　																												
       ON Yo.SirSitenCdSeq=Toshiten.SitenCdSeq and Yo.SirCdSeq = Toshiten.TokuiSeq																												
LEFT JOIN VPM_Gyosya AS Gyosa																												
       ON Gyosa.GyosyaCdSeq=Toku.GyosyaCdSeq
LEFT JOIN VPM_Gyosya AS Gosa																												
       ON Gosa.GyosyaCdSeq=Toki.GyosyaCdSeq
LEFT JOIN VPM_Eigyos AS Ei																												
       ON Yo.UkeEigCdSeq=Ei.EigyoCdSeq																												
LEFT JOIN VPM_Compny AS Company																												
       ON Company.EigyoCdSeq=Ei.EigyoCdSeq																												
       AND Company.TenantCdSeq=@tenantId																											
LEFT JOIN VPM_Syain AS Shain																												
       ON Yo.CanTanSeq=Shain.SyainCdSeq																												
LEFT JOIN VPM_Syain AS Sain																												
       ON Yo.EigTanCdSeq=Sain.SyainCdSeq																												
LEFT JOIN VPM_KyoSHe AS KyoShe																												
       ON Yo.CanTanSeq=KyoShe.SyainCdSeq																												
LEFT JOIN VPM_Eigyos AS Egos																												
       ON KyoShe.EigyoCdSeq=Egos.EigyoCdSeq																												
LEFT JOIN VPM_Syain AS Syain																												
       ON Yo.InTanCdSeq=Syain.SyainCdSeq																												
LEFT JOIN VPM_SyaSyu AS Sashu																												
       ON Shu.SyaSyuCdSeq=Sashu.SyaSyuCdSeq																												
LEFT JOIN VPM_YoyKbn AS YoyaKbn																												
       ON YoyaKbn.YoyaKbnSeq = Yo.YoyaKbnSeq
ORDER BY b.UkeNo, b.UnKren
END
GO


