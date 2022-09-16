-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dTransportDailyReports_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Transport Daily Report List
-- Date			:   2020/08/26
-- Author		:   P.M.Nhat
-- Description	:   Get transport daily report list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_dTransportDailyReports_R] 
	-- Add the parameters for the stored procedure here
	@OutStei		int,			-- 出力区分				
	@UnkYmd			varchar(8),		-- 運行年月日				
	@CompanyCd		int,			-- 会社				
	@StaEigyoCd		int,			-- 車輌営業所から				
	@EndEigyoCd		int,			-- 車輌営業所まで				
	@SyuKbn			int				-- 集計区分				
AS
BEGIN
	SELECT eTKD_Haisha.UkeNo AS UkeNo -- 受付番号																														
        ,eTKD_Haisha.UnkRen AS UnkRen -- 運行連番																														
        ,eTKD_Haisha.TeiDanNo AS TeiDanNo -- 悌団番号																														
        ,eTKD_Haisha.BunkRen AS BunkRen -- 分割連番																														
        ,eVPM_Eigyos.EigyoCd AS EigyoCd																														
        ,eVPM_Eigyos.EigyoNm AS EigyoNm																														
        ,eVPM_Eigyos.RyakuNm AS EigyoRyakuNm																														
        ,(																														
                CASE 																														
                        WHEN eTKD_Haisha.SyuKoYmd = eTKD_Haisha.KikYmd																														
                                THEN '日帰'																														
                        ELSE '宿泊'																														
                        END																														
                ) AS Hihaku																														
        ,ISNULL(eVPM_SyaRyo01.SyaRyoCd, '') AS SyaRyoCd																														
        ,ISNULL(eVPM_SyaRyo01.SyaRyoNm, '') AS SyaRyoNm																														
        ,ISNULL(eVPM_SyaRyo01.KariSyaRyoNm, '') AS KariSyaRyoNm																														
        ,ISNULL(eVPM_SyaRyo01.TeiCnt, 0) AS TeiCnt																														
        ,ISNULL(Hai_Nenryo1.CodeKbn, '') AS Nenryo1Cd																														
        ,ISNULL(Hai_Nenryo1.CodeKbnNm, '') AS Nenryo1Nm																														
        ,ISNULL(Hai_Nenryo1.RyakuNm, '') AS Nenryo1RyakuNm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.Nenryo1, 0)) AS Nenryo1																														
        ,ISNULL(Hai_Nenryo2.CodeKbn, '') AS Nenryo2Cd																														
        ,ISNULL(Hai_Nenryo2.CodeKbnNm, '') AS Nenryo2Nm																														
        ,ISNULL(Hai_Nenryo2.RyakuNm, '') AS Nenryo2RyakuNm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.Nenryo2, 0)) AS Nenryo2																														
        ,ISNULL(Hai_Nenryo3.CodeKbn, '') AS Nenryo3Cd																														
        ,ISNULL(Hai_Nenryo3.CodeKbnNm, '') AS Nenryo3Nm																														
        ,ISNULL(Hai_Nenryo3.RyakuNm, '') AS Nenryo3RyakuNm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.Nenryo3, 0)) AS Nenryo3																														
        ,ISNULL(eVPM_CodeKb02.RyakuNm, '') AS YoyaKbnNm																														
        ,ISNULL(eTKD_Unkobi.DanTaNm, '') AS DanTaNm																														
        ,ISNULL(eTKD_Haisha.DanTaNm2, '') AS DanTaNm2																														
        ,ISNULL(eTKD_Haisha.IkNm, '') AS IkNm																														
        ,ISNULL(eVPM_Gyosya.GyosyaCd, 0) AS GyosyaCd																														
        ,ISNULL(eVPM_Tokisk.TokuiCd, 0) AS TokuiCd																														
        ,ISNULL(eVPM_Tokist.sitenCd, 0) AS SitenCd																														
        ,ISNULL(eVPM_Tokist.TesKbn, 0) AS TesKbn																														
        ,ISNULL(eVPM_CodeKb01.RyakuNm, '') AS GyosyaNm																														
        ,ISNULL(eVPM_Tokisk.RyakuNm, '') AS Tokui_RyakuNm																														
        ,ISNULL(eVPM_Tokist.RyakuNm, '') AS Siten_RyakuNm																														
        ,ISNULL(eVPM_Tokisk.TokuiNm, '') AS TokuiNm																														
        ,ISNULL(eVPM_Tokist.SitenNm, '') AS SitenNm																														
        ,eTKD_Haisha.HaiSYmd AS HaiSYmd																														
        ,eTKD_Haisha.HaiSTime AS HaiSTime																														
        ,eTKD_Haisha.TouYmd AS TouYmd																														
        ,eTKD_Haisha.TouChTime AS TouChTime																														
        ,eTKD_Haisha.SyuKoYmd AS SyuKoYmd																														
        ,eTKD_Haisha.SyuKoTime AS SyuKoTime																														
        ,eTKD_Haisha.KikYmd AS KikYmd																														
        ,eTKD_Haisha.KikTime AS KikTime																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.StMeter, 0)) AS StMeter																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.EndMeter, 0)) AS EndMeter																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.JisaIPKm, 0)) AS Total_JisaIPKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.JisaKSKm, 0)) AS Total_JisaKSKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.KisoIPkm, 0)) AS Total_KisoIPKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.KisoKOkm, 0)) AS Total_KisoKSKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.OhtKm, 0)) AS Total_OthKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.TotalKm, 0)) AS Total_TotalKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum02.JisaIPKm, 0)) AS MonthTotal_JisaIPKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum02.JisaKSKm, 0)) AS MonthTotal_JisaKSKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum02.KisoIPkm, 0)) AS MonthTotal_KisoIPKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum02.KisoKOkm, 0)) AS MonthTotal_KisoKSKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum02.OhtKm, 0)) AS MonthTotal_OthKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum02.TotalKm, 0)) AS MonthTotal_TotalKm																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.JyoSyaJin, 0)) AS JyoSyaJin																														
        ,CONVERT(VARCHAR, ISNULL(YV_ShabniHaishaSum01.PlusJin, 0)) AS PlusJin																														
        ,eTKD_Haisha.SyaRyoUnc + eTKD_Haisha.SyaRyoSyo AS SyaRyoUnc																														
        ,eTKD_YYksho.Zeiritsu AS Zeiritsu																														
        ,eTKD_Haisha.SyaRyoSyo AS SyaRyoSyo																														
        ,eTKD_YYksho.TesuRitu AS TesuRitu																														
        ,eTKD_Haisha.SyaRyoTes AS SyaRyoTes																														
        ,ISNULL(eVPM_Syokum01.SyokumuKbn, 0) AS SyokumuKbn1																														
        ,ISNULL(eVPM_Syokum01.SyokumuNm, '') AS SyokumuNm1																														
        ,ISNULL(eVPM_Syain01.SyainCd, '') AS SyainCd1																														
        ,ISNULL(eVPM_Syain01.SyainNm, '') AS SyainNm1																														
        ,ISNULL(eVPM_Syain01.KariSyainNm, '') AS SyainKariNm1																														
        ,ISNULL(eVPM_Syokum02.SyokumuKbn, 0) AS SyokumuKbn2																														
        ,ISNULL(eVPM_Syokum02.SyokumuNm, '') AS SyokumuNm2																														
        ,ISNULL(eVPM_Syain02.SyainCd, '') AS SyainCd2																														
        ,ISNULL(eVPM_Syain02.SyainNm, '') AS SyainNm2																														
        ,ISNULL(eVPM_Syain02.KariSyainNm, '') AS SyainKariNm2																														
        ,ISNULL(eVPM_Syokum03.SyokumuKbn, 0) AS SyokumuKbn3																														
        ,ISNULL(eVPM_Syokum03.SyokumuNm, '') AS SyokumuNm3																														
        ,ISNULL(eVPM_Syain03.SyainCd, '') AS SyainCd3																														
        ,ISNULL(eVPM_Syain03.SyainNm, '') AS SyainNm3																														
        ,ISNULL(eVPM_Syain03.KariSyainNm, '') AS SyainKariNm3																														
        ,ISNULL(eVPM_Syokum04.SyokumuKbn, 0) AS SyokumuKbn4																														
        ,ISNULL(eVPM_Syokum04.SyokumuNm, '') AS SyokumuNm4																														
        ,ISNULL(eVPM_Syain04.SyainCd, '') AS SyainCd4																														
        ,ISNULL(eVPM_Syain04.SyainNm, '') AS SyainNm4																														
        ,ISNULL(eVPM_Syain04.KariSyainNm, '') AS SyainKariNm4																														
        ,ISNULL(eVPM_Syokum05.SyokumuKbn, 0) AS SyokumuKbn5																														
        ,ISNULL(eVPM_Syokum05.SyokumuNm, '') AS SyokumuNm5																														
        ,ISNULL(eVPM_Syain05.SyainCd, '') AS SyainCd5																														
        ,ISNULL(eVPM_Syain05.SyainNm, '') AS SyainNm5																														
        ,ISNULL(eVPM_Syain05.KariSyainNm, '') AS SyainKariNm5																														
	FROM TKD_Haisha AS eTKD_Haisha																														
	-- 予約書 																														
	LEFT JOIN TKD_YYksho AS eTKD_YYksho ON eTKD_YYksho.UkeNo = eTKD_Haisha.UkeNo																														
	-- 予約区分																														
	LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eVPM_YoyKbn.YoyaKbnSeq = eTKD_YYksho.YoyaKbnSeq																														
	-- 運行日 																														
	LEFT JOIN TKD_Unkobi AS eTKD_Unkobi ON eTKD_Unkobi.UkeNo = eTKD_Haisha.UkeNo																														
			AND eTKD_Unkobi.UnkRen = eTKD_Haisha.UnkRen																														
	-- 配車車輌 																														
	LEFT JOIN VPM_SyaRyo AS eVPM_SyaRyo01 ON eVPM_SyaRyo01.SyaRyoCdSeq = eTKD_Haisha.HaisSryCdSeq																														
	-- 車輌マスタの車輌コードＳＥＱより取得（車輌編成マスタ）																														
	LEFT JOIN VPM_HenSya AS eVPM_HenSya01 ON eVPM_SyaRyo01.SyaRyoCdSeq = eVPM_HenSya01.SyaRyoCdSeq																														
			AND eTKD_Haisha.SyukoYmd >= eVPM_HenSya01.StaYmd																														
			AND eTKD_Haisha.SyukoYmd <= eVPM_HenSya01.EndYmd																														
	-- 車輌編成マスタの営業所コードＳＥＱより取得（営業所マスタ）																														
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya01.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq																														
	-- 営業所マスタの会社コードＳＥＱより取得（会社マスタ）																														
	LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
	-- 会社マスタのテナントコードＳＥＱより取得（テナントマスタ）																														
	LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eVPM_Compny.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
	-- 予約区分名 																														
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02 ON eVPM_CodeKb02.CodeSyu = 'YOYAKBN'																														
			AND eVPM_CodeKb02.CodeKbn = CONVERT(TINYINT, eTKD_YykSho.YoyaKbnSeq)																														
			AND eVPM_CodeKb02.SiyoKbn = 1																														
	-- 運送区分名 																														
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb03 ON eVPM_CodeKb03.CodeSyu = 'UNSOUKBN'																														
			AND eVPM_CodeKb03.CodeKbn = CONVERT(TINYINT, eVPM_YoyKbn.UnsouKbn)																														
			AND eVPM_CodeKb03.SiyoKbn = 1																														
	-- 得意先 																														
	LEFT JOIN VPM_Tokisk AS eVPM_Tokisk ON eTKD_YYksho.TokuiSeq = eVPM_Tokisk.TokuiSeq																														
			AND eTKD_YYksho.UkeYmd >= eVPM_Tokisk.SiyoStaYmd																														
			AND eTKD_YYksho.UkeYmd <= eVPM_Tokisk.SiyoEndYmd																														
	-- 得意先支店 																														
	LEFT JOIN VPM_Tokist AS eVPM_Tokist ON eTKD_YYksho.TokuiSeq = eVPM_Tokist.TokuiSeq																														
			AND eTKD_YYksho.SitenCdSeq = eVPM_Tokist.SitenCdSeq																														
			AND eTKD_YYksho.UkeYmd >= eVPM_Tokist.SiyoStaYmd																														
			AND eTKD_YYksho.UkeYmd <= eVPM_Tokist.SiyoEndYmd																														
	-- 業者区分 																														
	LEFT JOIN VPM_Gyosya AS eVPM_Gyosya ON eVPM_Gyosya.GyosyaCdSeq = eVPM_Tokisk.GyosyaCdSeq																														
	LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeSyu = 'GYOSYAKBN'																														
			AND eVPM_CodeKb01.CodeKbn = CONVERT(TINYINT, eVPM_Gyosya.GyosyaKbn)																														
			AND eVPM_CodeKb01.SiyoKbn = 1																														
	-- 運行全日キロ数・燃料 																														
	LEFT JOIN (																														
			SELECT eTKD_Shabni.UkeNo																														
					,eTKD_Shabni.UnkRen																														
					,eTKD_Shabni.TeiDanNo																														
					,eTKD_Shabni.BunkRen																														
					,SUM(eTKD_Shabni.JisaIPKm) AS JisaIPKm																														
					,SUM(eTKD_Shabni.JisaKSKm) AS JisaKSKm																														
					,SUM(eTKD_Shabni.KisoIPkm) AS KisoIPkm																														
					,SUM(eTKD_Shabni.KisoKOKm) AS KisoKOKm																														
					,SUM(eTKD_Shabni.OthKm) AS OhtKm																														
					,SUM(eTKD_Shabni.JisaIPKm + eTKD_Shabni.JisaKSKm + eTKD_Shabni.KisoIPkm + eTKD_Shabni.KisoKOKm + eTKD_Shabni.OthKm) AS TotalKm																														
					,SUM(eTKD_Shabni.Nenryo1) AS Nenryo1																														
					,SUM(eTKD_Shabni.Nenryo2) AS Nenryo2																														
					,SUM(eTKD_Shabni.Nenryo3) AS Nenryo3																														
					,MAX(eTKD_Shabni.JyoSyaJin) AS JyoSyaJin																														
					,MAX(eTKD_Shabni.PlusJin) AS PlusJin																														
					,Min(eTKD_Shabni.StMeter) AS StMeter																														
					,Max(eTKD_Shabni.EndMeter) AS EndMeter																														
			FROM TKD_Shabni AS eTKD_Shabni																														
			GROUP BY eTKD_Shabni.UkeNo																														
					,eTKD_Shabni.UnkRen																														
					,eTKD_Shabni.TeiDanNo																														
					,eTKD_Shabni.BunkRen																														
			) AS YV_ShabniHaishaSum01 ON eTKD_Haisha.UkeNo = YV_ShabniHaishaSum01.UkeNo																														
			AND eTKD_Haisha.UnkRen = YV_ShabniHaishaSum01.UnkRen																														
			AND eTKD_Haisha.TeiDanNo = YV_ShabniHaishaSum01.TeiDanNo																														
			AND eTKD_Haisha.BunkRen = YV_ShabniHaishaSum01.BunkRen																														
	-- 当月内キロ数 																														
	LEFT JOIN (																														
			SELECT eTKD_Shabni.UkeNo																														
					,eTKD_Shabni.UnkRen																														
					,eTKD_Shabni.TeiDanNo																														
					,eTKD_Shabni.BunkRen																														
					,SUM(eTKD_Shabni.JisaIPKm) AS JisaIPKm																														
					,SUM(eTKD_Shabni.JisaKSKm) AS JisaKSKm																														
					,SUM(eTKD_Shabni.KisoIPkm) AS KisoIPkm																														
					,SUM(eTKD_Shabni.KisoKOKm) AS KisoKOKm																														
					,SUM(eTKD_Shabni.OthKm) AS OhtKm																														
					,SUM(eTKD_Shabni.JisaIPKm + eTKD_Shabni.JisaKSKm + eTKD_Shabni.KisoIPkm + eTKD_Shabni.KisoKOKm + eTKD_Shabni.OthKm) AS TotalKm																														
			FROM TKD_Shabni AS eTKD_Shabni																														
			WHERE left(UnkYmd, 6) = left(@UnkYmd, 6)																														
			GROUP BY eTKD_Shabni.UkeNo																														
					,eTKD_Shabni.UnkRen																														
					,eTKD_Shabni.TeiDanNo																														
					,eTKD_Shabni.BunkRen																														
			) AS YV_ShabniHaishaSum02 ON eTKD_Haisha.UkeNo = YV_ShabniHaishaSum02.UkeNo																														
			AND eTKD_Haisha.UnkRen = YV_ShabniHaishaSum02.UnkRen																														
			AND eTKD_Haisha.TeiDanNo = YV_ShabniHaishaSum02.TeiDanNo																														
			AND eTKD_Haisha.BunkRen = YV_ShabniHaishaSum02.BunkRen																														
	-- 配車車輌燃料名 																														
	LEFT JOIN (																														
			SELECT CodeKbnSeq																														
					,CodeKbn																														
					,CodeKbnNm																														
					,RyakuNm																														
			FROM VPM_CodeKb																														
			WHERE SiyoKbn = 1																														
			) AS Hai_Nenryo1 ON eVPM_SyaRyo01.NenryoCd1Seq = Hai_Nenryo1.CodeKbnSeq																														
	LEFT JOIN (																														
			SELECT CodeKbnSeq																														
					,CodeKbn																														
					,CodeKbnNm																														
					,RyakuNm																														
			FROM VPM_CodeKb																														
			WHERE SiyoKbn = 1																														
			) AS Hai_Nenryo2 ON eVPM_SyaRyo01.NenryoCd2Seq = Hai_Nenryo2.CodeKbnSeq																														
	LEFT JOIN (																														
			SELECT CodeKbnSeq																														
					,CodeKbn																														
					,CodeKbnNm																														
					,RyakuNm																														
			FROM VPM_CodeKb																														
			WHERE SiyoKbn = 1																														
			) AS Hai_Nenryo3 ON eVPM_SyaRyo01.NenryoCd3Seq = Hai_Nenryo3.CodeKbnSeq																														
	-- 乗務員View参照 																														
	LEFT JOIN VP_Haisha1 AS wHaiin ON eTKD_Haisha.UkeNo = wHaiin.UkeNo																														
			AND eTKD_Haisha.UnkRen = wHaiin.UnkRen																														
			AND eTKD_Haisha.TeiDanNo = wHaiin.TeiDanNo																														
			AND eTKD_Haisha.BunkRen = wHaiin.BunkRen																														
	-- 乗務員1 																														
	LEFT JOIN VPM_Syain AS eVPM_Syain01 ON wHaiin.SyainCdSeq1 = eVPM_Syain01.SyainCdSeq																														
	LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe01 ON eVPM_Syain01.SyainCdSeq = eVPM_KyoSHe01.SyainCdSeq																														
			AND eVPM_KyoSHe01.StaYmd <= eTKD_Haisha.SyukoYmd																														
			AND eVPM_KyoSHe01.EndYmd >= eTKD_Haisha.SyukoYmd																														
	LEFT JOIN VPM_Syokum AS eVPM_Syokum01 ON eVPM_KyoSHe01.SyokumuCdSeq = eVPM_Syokum01.SyokumuCdSeq																														
	-- 乗務員2 																														
	LEFT JOIN VPM_Syain AS eVPM_Syain02 ON wHaiin.SyainCdSeq2 = eVPM_Syain02.SyainCdSeq																														
	LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe02 ON eVPM_Syain02.SyainCdSeq = eVPM_KyoSHe02.SyainCdSeq																														
			AND eVPM_KyoSHe02.StaYmd <= eTKD_Haisha.SyukoYmd																														
			AND eVPM_KyoSHe02.EndYmd >= eTKD_Haisha.SyukoYmd																														
	LEFT JOIN VPM_Syokum AS eVPM_Syokum02 ON eVPM_KyoSHe02.SyokumuCdSeq = eVPM_Syokum02.SyokumuCdSeq																														
	-- 乗務員3 																														
	LEFT JOIN VPM_Syain AS eVPM_Syain03 ON wHaiin.SyainCdSeq3 = eVPM_Syain03.SyainCdSeq																														
	LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe03 ON eVPM_Syain03.SyainCdSeq = eVPM_KyoSHe03.SyainCdSeq																														
			AND eVPM_KyoSHe03.StaYmd <= eTKD_Haisha.SyukoYmd																														
			AND eVPM_KyoSHe03.EndYmd >= eTKD_Haisha.SyukoYmd																														
	LEFT JOIN VPM_Syokum AS eVPM_Syokum03 ON eVPM_KyoSHe03.SyokumuCdSeq = eVPM_Syokum03.SyokumuCdSeq																														
	-- 乗務員4 																														
	LEFT JOIN VPM_Syain AS eVPM_Syain04 ON wHaiin.SyainCdSeq4 = eVPM_Syain04.SyainCdSeq																														
	LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe04 ON eVPM_Syain04.SyainCdSeq = eVPM_KyoSHe04.SyainCdSeq																														
			AND eVPM_KyoSHe04.StaYmd <= eTKD_Haisha.SyukoYmd																														
			AND eVPM_KyoSHe04.EndYmd >= eTKD_Haisha.SyukoYmd																														
	LEFT JOIN VPM_Syokum AS eVPM_Syokum04 ON eVPM_KyoSHe04.SyokumuCdSeq = eVPM_Syokum04.SyokumuCdSeq																														
	-- 乗務員5 																														
	LEFT JOIN VPM_Syain AS eVPM_Syain05 ON wHaiin.SyainCdSeq5 = eVPM_Syain05.SyainCdSeq																														
	LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe05 ON eVPM_Syain05.SyainCdSeq = eVPM_KyoSHe05.SyainCdSeq																														
			AND eVPM_KyoSHe05.StaYmd <= eTKD_Haisha.SyukoYmd																														
			AND eVPM_KyoSHe05.EndYmd >= eTKD_Haisha.SyukoYmd																														
	LEFT JOIN VPM_Syokum AS eVPM_Syokum05 ON eVPM_KyoSHe05.SyokumuCdSeq = eVPM_Syokum05.SyokumuCdSeq																														
	WHERE 1 = 1																														
			AND (@OutStei != 1 OR eTKD_Haisha.SyukoYmd = @UnkYmd) -- IF @OutStei = 1																														
			AND (@OutStei != 2 OR eTKD_Haisha.KikYmd = @UnkYmd) -- IF @OutStei = 2																														
			AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																														
			AND (@StaEigyoCd = 0 OR eVPM_Eigyos.EigyoCd >= @StaEigyoCd)																														
			AND (@EndEigyoCd = 0 OR eVPM_Eigyos.EigyoCd <= @EndEigyoCd)																														
			AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)																														
			AND eTKD_Haisha.SiyoKbn = 1																														
			AND eTKD_Yyksho.YoyaSyu = 1																														
	ORDER BY eVPM_Eigyos.EigyoCd																														
			,eVPM_SyaRyo01.SyaRyoCd																														
			,CASE WHEN @OutStei = 1 THEN eTKD_Haisha.SyukoYmd END -- IF @OutStei = 1
			,CASE WHEN @OutStei = 1 THEN eTKD_Haisha.SyukoTime END -- IF @OutStei = 1																														
			,CASE WHEN @OutStei = 2 THEN eTKD_Haisha.KikYmd END -- IF @OutStei = 2	
			,CASE WHEN @OutStei = 2 THEN eTKD_Haisha.KikTime END -- IF @OutStei = 2										
END
GO
