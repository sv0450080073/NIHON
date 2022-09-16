--USE [HOC_Kashikiri]
--GO

/****** Object:  UserDefinedTableType [dbo].[Tblt_StatusConfirmType]    Script Date: 10/8/2020 1:22:43 PM ******/
CREATE TYPE [dbo].[Tblt_StatusConfirmType] AS TABLE(
	[UkeNo] [nvarchar](15) NOT NULL,
	[UnKren] [smallint] NOT NULL
)
GO

/****** Object:  StoredProcedure [dbo].[Pro_StatusConfirmation_R]    Script Date: 10/8/2020 1:23:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[Pro_StatusConfirmation_R]
		@selectedList Tblt_StatusConfirmType READONLY,
		@tenantId int = 1
AS
BEGIN
SELECT
unkobi.UkeNo, CONVERT(varchar,unkobi.UnkRen) as UnkRen,
tokisk.RyakuNm AS TokuiRyakuNm, tokiSt.RyakuNm AS SitenRyakuNm, tokiSt.FaxNo AS SitenFaxNo, 
unkobi.HaiSYmd AS UnkoYmdStr, unkobi.HaiSTime AS HaiSTime, unkobi.TouYmd AS UnkoYmdEnd, unkobi.TouChTime AS TouChTime,
unkobi.DanTaNm AS DanTaNm, yyksho.YoyaNm AS YoyakuNm,
unkobi.IkNm AS IkNm,
unkobi.HaiSNm AS HaiSNm,
unkobi.TouNm AS TouNm,
IsNull(syasyu.SyaSyuNm, '指定なし') AS SyaSyuNm, KataKbn.RyakuNm AS KataKbn, yyksyu.SyaSyuDai AS SyaSyuDai,
CONVERT(varchar,yyksho.UntKin) AS UntKin, CONVERT(varchar, yyksho.ZeiRui) AS ZeiRui, CONVERT(varchar, yyksho.Zeiritsu) AS Zeiritsu, CONVERT(varchar, yyksho.TesuRitu) AS TesuRitu, CONVERT(varchar, yyksho.TesuRyoG) AS TesuRyoG,
CONVERT(varchar, yyksho.GuitKin) AS GuitKin, CONVERT(varchar, yyksho.FeeGuider) AS FeeGuider, CONVERT(varchar, yyksho.TaxGuider) AS TaxGuider, 
unkobi.JyoSyaJin AS JyoSyaJin, unkobi.PlusJin AS PlusJin,
yyksho.GuiWNin AS GuiWNin,
yyksho.KaktYmd AS KaktYmd,
SaikFlg, DaiSuFlg, KingFlg, NitteiFlg,
unkobi.BikoNm AS BikoNm,
company.CompanyNm AS CompanyName,
eigyos.EigyoNm AS EigyosRyakuNm,
eigtan.SyainNm AS EigSyain,
eigyos.TelNo AS TEL, eigyos.FaxNo AS FAX,
CONVERT(varchar, (ISNULL(yyksho.UntKin, 0) + ISNULL(yyksho.ZeiRui, 0) + ISNULL(yyksho.TesuRyoG, 0) + ISNULL(yyksho.GuitKin, 0) + ISNULL(yyksho.TaxGuider, 0) + ISNULL(yyksho.FeeGuider, 0)))  AS TotalAmount,
--fileds for CSV report
yyksho.UkeCd as UkeCd, 
yoykbn.YoyaKbn as YoyaKbn, yoykbn.YoyaKbnNm as YoyaKbnNm,
yyksho.UkeYmd as UkeYmd,
tokisk.TokuiCd as TokiskTokuiCd, tokisk.RyakuNm as TokiskRyakuNm, tokiSt.SitenCd as TokiStSitenCd, tokiSt.RyakuNm as TokiStRyakuNm,
yyksho.TokuiTanNm as TokuiTanNm,
sirTokisk.TokuiCd as SirTokiskTokuiCd, sirTokisk.RyakuNm as SirTokiskRyakuNm, sirTokiSt.SitenCd as SirTokiStSitenCd, sirTokiSt.RyakuNm as SirTokiStRyakuNm,
eigyos.EigyoCd as EigyoCd, eigyos.RyakuNm  as EigyoRyakuNm,
unkobi.KanJNm as KanJNm,
syasyu.SyaSyuCd as SyaSyuCd,
KataKbn.CodeKbn as CodeKbn, KataKbn.CodeKbnNm as CodeKbnNm,
yyksho.KaknKais as KaknKais, 
IsNull(kaknin.KaknNin, 0) as KaknNin,
IsNull(kaknin.SaikFlg, 0) as SaikFlg,
IsNull(kaknin.DaiSuFlg, 0) as DaiSuFlg,
IsNull(kaknin.KingFlg, 0) as KingFlg,
IsNull(kaknin.NitteiFlg, 0) as NitteiFlg,
intan.SyainCd as IntanSyainCd,
intan.SyainNm as IntanSyainNm,
eigtan.SyainCd as EigtanSyainCd,
eigtan.SyainNm as EigtanSyainNm,
unkobi.GuiSu as GuiSu,
yyksho.FeeGuider as UnitGuiderFee
FROM TKD_YykSyu yyksyu
JOIN TKD_Yyksho yyksho ON yyksho.UkeNo = yyksyu.UkeNo
JOIN TKD_Unkobi unkobi ON yyksho.UkeNo = unkobi.UkeNo
LEFT JOIN
	(SELECT kak.UkeNo, kk.KaknYmd, kk.KaknNin, kk.KaknAit, kak.SaikFlg, kak.DaiSuFlg, kak.KingFlg, kak.NitteiFlg, IsNull(kak.CountKak, 0) AS CountKak
	FROM (SELECT Count(k.KaknRen) AS CountKak, Max(k.KaknRen) AS KaknRen, Max(k.KaknAit) AS KaknAit, Max(k.SaikFlg) AS SaikFlg, Max(k.DaiSuFlg) AS DaiSuFlg, Max(k.KingFlg) AS KingFlg, Max(k.NitteiFlg) AS NitteiFlg, k.UkeNo
		FROM TKD_Kaknin k group by k.UkeNo) kak
	INNER JOIN TKD_Kaknin kk ON kak.UkeNo = kk.UkeNo and kak.KaknRen = kk.KaknRen) kaknin ON kaknin.UkeNo = yyksho.UkeNo
LEFT JOIN VPM_SyaSyu syasyu ON yyksyu.SyaSyuCdSeq = syasyu.SyaSyuCdSeq
LEFT JOIN VPM_Syain eigtan ON yyksho.EigTanCdSeq = eigtan.SyainCdSeq
LEFT JOIN VPM_Syain intan ON yyksho.InTanCdSeq = intan.SyainCdSeq
LEFT JOIN VPM_Eigyos eigyos ON yyksho.UkeEigCdSeq = eigyos.EigyoCdSeq
LEFT JOIN VPM_Compny company ON yyksho.UkeEigCdSeq = company.CompanyCdSeq
LEFT JOIN VPM_YoyKbn yoykbn ON yyksho.YoyaKbnSeq = yoykbn.YoyaKbnSeq
LEFT JOIN VPM_Tokisk tokisk ON yyksho.TokuiSeq = tokisk.TokuiSeq
LEFT JOIN VPM_TokiSt tokiSt ON yyksho.TokuiSeq = tokiSt.TokuiSeq and yyksho.SitenCdSeq = tokiSt.SitenCdSeq
LEFT JOIN VPM_Tokisk sirTokisk ON yyksho.SirCdSeq = sirTokisk.TokuiSeq
LEFT JOIN VPM_TokiSt sirTokiSt ON yyksho.SirCdSeq = sirTokiSt.TokuiSeq and yyksho.SirSitenCdSeq = sirTokiSt.SitenCdSeq
LEFT JOIN (SELECT RyakuNm, CodeKbn, CodeKbnNm FROM VPM_CodeKb WHERE CodeSyu='KATAKBN' AND TenantCdSeq= 0) AS KataKbn ON KataKbn.CodeKbn = yykSyu.KataKbn
WHERE EXISTS (SELECT * FROM @selectedList s WHERE unkobi.UkeNo = s.UkeNo AND unkobi.UnkRen = s.UnKren)
END
GO


