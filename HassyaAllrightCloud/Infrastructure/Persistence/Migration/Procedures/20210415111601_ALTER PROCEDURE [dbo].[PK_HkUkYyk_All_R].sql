USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_HkUkYyk_All_R]    Script Date: 2021/04/15 11:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	[PK_HkUkYyk_All_R]
-- DB-Name		:	運行日テーブル、その他
-- Name			:	運送引受書のデータ取得処理
-- Date			:	2021/02/17
-- Author		:	nhhkieuanh
-- Descriotion	:	運行日テーブルその他のSelect処理
-- 				:	運行日紐付けられる他テーブル情報も取得する
---------------------------------------------------
-- Update		:	
-- Comment		:
----------------------------------------------------

ALTER     PROCEDURE [dbo].[PK_HkUkYyk_All_R]
	( 
		@TenantCdSeq		INT		   
	,	@StartDispatchDate	VARCHAR(8)					-- 配車日付開始
	,	@EndDispatchDate	VARCHAR(8)					-- 配車日付終了
	,	@StartArrivalDate	VARCHAR(8)					-- 到着日付開始
	,	@EndArrivalDate		VARCHAR(8)					-- 到着日付終了
	,	@StartReservationDate	VARCHAR(8)				-- 受付日付開始
	,	@EndReservationDate		VARCHAR(8)				-- 受付日付終了		
	,	@YoyaKbnList		VARCHAR(50)					-- 予約区分
	,	@UkeEigCd			VARCHAR(5)					-- 受付営業所コード
	,	@EigSyainCd			VARCHAR(10)					-- 営業社員コード
	,	@InpSyainCd			VARCHAR(10)					-- 入力社員コード	
	,	@TokuiCd			VARCHAR(10)					-- 得意先コード
	,	@SitenCd			VARCHAR(10)					-- 得意先支店コード
	,	@UkeNo				VARCHAR(MAX)					-- 受付番号
	,	@UnkRen				VARCHAR(3)					-- 運行連番
	,	@OutSelect			VARCHAR(1)					-- 出力選択(0:すべて 1:未出力のみ)	
	,	@NenKeiyakuOutFlg	VARCHAR(1)					-- 年間契約出力フラグ(0:出力する 1:出力しない)	
	,	@OutputUnit			VARCHAR(1)					-- 出力単位(1:予約毎 2:予約車種毎)	
	-- Output
	,	@ROWCOUNT			INTEGER OUTPUT				-- 処理件数
	)
AS 
IF @OutputUnit = 1
BEGIN
SELECT TKD_Unkobi.UkeNo																		
        ,TKD_Unkobi.UnkRen																		
        ,TKD_Unkobi.HaiSYmd																		
        ,TKD_Unkobi.TouYmd																		
        ,TKD_Unkobi.DanTaNm																		
        ,TKD_Unkobi.KanjJyus1																		
        ,TKD_Unkobi.KanjJyus2																		
        ,TKD_Unkobi.KanjTel																		
        ,TKD_Unkobi.KanJNm																		
        ,TKD_Unkobi.KanjKeiNo																		
        ,TKD_Unkobi.IkNm																		
        ,TKD_Unkobi.ZenHaFlg																		
        ,TKD_Unkobi.KhakFlg																		
        ,ISNULL(JM_SirTokisk.TokuiNm, '') AS SirTokuiNm																		
        ,ISNULL(JM_SirTokiSt.TokuiTanNm, '') AS SirTokuiTanNm																		
        ,ISNULL(JM_SirTokiSt.SitenNm, '') AS SirSitenNm																		
        ,ISNULL(JM_SirTokiSt.TelNo, '') AS SirTelNo																		
        ,ISNULL(JM_SirTokiSt.FaxNo, '') AS SirFaxNo																		
        ,ISNULL(JM_SirTokiSt.TokuiMail, '') AS SirTokuiMail																		
        ,ISNULL(JM_SirTokiSt.Jyus1, '') AS SirJyus1																		
        ,ISNULL(JM_SirTokiSt.Jyus2, '') AS SirJyus2																		
        ,ISNULL(JM_SirTokiSt.TesuShihKbn, '') AS TesuShihKbn																		
        ,ISNULL(JM_Tokisk.TokuiNm, '') AS TokuiNm																		
        ,ISNULL(JM_TokiSt.SitenNm, '') AS SitenNm																		
        ,ISNULL(JM_TokiSt.Jyus1, '') AS TokiStJyus1																		
        ,ISNULL(JM_TokiSt.Jyus2, '') AS TokiStJyus2																		
        ,ISNULL(Unk_Haisha.Min_SyuKoYmd, '') AS SyuKoYmd																		
        ,ISNULL(Unk_Haisha.Max_KikYmd, '') AS KikYmd																		
        ,ISNULL(Unk_Haisha.Min_SyuKoTime, '') AS SyuKoTime																		
        ,ISNULL(Unk_Haisha.Max_KikTime, '') AS KikTime																		
        ,ISNULL(JM_TokiSt.TokuiMail, '') AS TokiStMail																		
        ,ISNULL(JM_TokiSt.TelNo, '') AS TokuiTel																		
        ,ISNULL(JM_TokiSt.FaxNo, '') AS TokuiFax																		
        ,ISNULL(JM_TokiSt.TokuiTanNm, '') AS TokuiTanNm																		
        ,TKD_Unkobi.HaiSNm																		
        ,TKD_Unkobi.HaiSJyus1																		
        ,TKD_Unkobi.HaiSJyus2																		
        ,ISNULL(JT_UnkobiExp.HaiSKouKNm, '') AS HaiSKoukRyaku																		
        ,ISNULL(JT_UnkobiExp.HaiSBinNm, '') AS HaiSBinNm																		
        ,TKD_Unkobi.HaiSSetTime																		
        ,TKD_Unkobi.HaiSTime																		
        ,TKD_Unkobi.DrvJin																		
        ,TKD_Unkobi.GuiSu																		
        ,ISNULL(JM_EigTan.SyainNm, '') AS EigTanNm																		
        ,TKD_Unkobi.SyuPaTime																		
        ,TKD_Unkobi.TouNm																		
        ,TKD_Unkobi.TouJyusyo1																		
        ,TKD_Unkobi.TouJyusyo2																		
        ,ISNULL(JT_UnkobiExp.TouSKouKNm, '') AS TouChaKoukRyaku																		
        ,ISNULL(JT_UnkobiExp.TouSBinNm, '') AS TouChaBinNm																		
        ,TKD_Unkobi.TouSetTime																		
        ,TKD_Unkobi.TouChTime																		
        ,TKD_Unkobi.IkNm	
        ,ISNULL(JM_SijJoKbn1.RyakuNm, '') AS SijJoKbn1Ryaku																		
        ,ISNULL(JM_SijJoKbn2.RyakuNm, '') AS SijJoKbn2Ryaku																		
        ,ISNULL(JM_SijJoKbn3.RyakuNm, '') AS SijJoKbn3Ryaku																		
        ,ISNULL(JM_SijJoKbn4.RyakuNm, '') AS SijJoKbn4Ryaku																		
        ,ISNULL(JM_SijJoKbn5.RyakuNm, '') AS SijJoKbn5Ryaku																		
        ,ISNULL(JT_YykSyuSumUnc.SumSyuUnc, 0) AS SumSyuUnc																		
        ,ISNULL(JT_HaishaSyaRyo.SumSyaRyoUnc, 0) AS SumSyaRyoUnc																		
        ,ISNULL(JT_HaishaSyaRyo.SumSyaRyoSyo, 0) AS SumSyaRyoSyo																		
        ,ISNULL(JT_HaishaSyaRyo.SumSyaRyoTes, 0) AS SumSyaRyoTes																		
        ,ISNULL(JT_Yyksho.ZeiKbn, 0) AS ZeiKbn																		
        ,ISNULL(JM_ZeiKbn.CodeKbnNm, '') AS ZeiKbnNm																		
        ,ISNULL(JM_ZeiKbn.RyakuNm, '') AS ZeiKbnRyaku																		
        ,ISNULL(JT_Yyksho.Zeiritsu, 0) AS Zeiritsu																		
        ,ISNULL(JT_Yyksho.ZeiRui, 0) AS ZeiRui																		
        ,ISNULL(JT_Yyksho.TesuRitu, 0) AS TesuRitu																		
        ,ISNULL(JT_Yyksho.TesuRyoG, 0) AS TesuRyoG																		
        ,ISNULL(JT_YoushaSumKin.YoushaKin, 0) AS YoushaKin																		
        ,JT_Yyksho.BikoTblSeq																		
        ,ISNULL(eTKD_Biko02.BikoRen, 0) AS BikoRen																		
        ,ISNULL(eTKD_Biko02.BikoNm, '') AS YykBikoNm																		
        ,ISNULL(eTKD_Kariei02.MinKSEigSeq, 0) AS MinKSEigSeq																		
        ,ISNULL(eTKD_Kariei01.CountKSEigSeq, 0) AS CountKSEigSeq																		
        ,ISNULL(eVPM_Eigyos.EigyoNm, '') AS KSEigyoNm																		
        ,ISNULL(eVPM_Eigyos.TelNo, '') AS KSEigyoTelNo																		
        ,ISNULL(eVPM_Eigyos.RyakuNm, '') AS KSEigyoRyakuNm																		
        ,TKD_Unkobi.UkeJyKbnCd																		
        ,ISNULL(JM_UkeJyKbnCd.CodeKbnNm, '') AS UkeJyKbnCdNm																		
        ,ISNULL(JM_UkeJyKbnCd.RyakuNm, '') AS UkeJyKbnCdRyaku																		
        ,TKD_Unkobi.OthJinKbn1																		
        ,ISNULL(JM_OthJinKbn1.CodeKbnNm, '') AS OthJinKbn1Nm																		
        ,ISNULL(JM_OthJinKbn1.RyakuNm, '') AS OthJinKbn1Ryaku																		
        ,TKD_Unkobi.OthJin1																		
        ,TKD_Unkobi.OthJinKbn2																		
        ,ISNULL(JM_OthJinKbn1.CodeKbnNm, '') AS OthJinKbn2Nm																		
        ,ISNULL(JM_OthJinKbn1.RyakuNm, '') AS OthJinKbn2Ryaku																		
        ,TKD_Unkobi.OthJin2																		
        ,TKD_Unkobi.JyoSyaJin																		
        ,TKD_Unkobi.PlusJin																		
        ,ISNULL(JM_YoyKbn.YoyaKbn, 0) AS YoyaKbn																		
        ,ISNULL(JT_Yyksho.UkeYmd, '') AS UkeYmd																		
        ,ISNULL(JM_InTan.SyainCd, 0) AS InTanSyainCd																		
        ,ISNULL(JM_InTan.SyainNm, '') AS InTanSyainNm																		
        ,TKD_Unkobi.DanTaKana AS DanTaKana																		
        ,ISNULL(JM_Gyosya.GyosyaCd, 0) AS GyosyaCd																		
        ,ISNULL(JM_Tokisk.TokuiCd, 0) AS TokuiCd																		
        ,ISNULL(JM_TokiSt.SitenCd, 0) AS SitenCd																		
        ,TKD_Unkobi.BikoTblSeq AS Unkobi_BikoTblSeq																		
        ,JM_UkeEigyos.EigyoNm AS UkeEigyoNm																		
        ,JM_UkeEigyos.Jyus1 AS UkeEigyoJyus1																		
        ,JM_UkeEigyos.Jyus2 AS UkeEigyoJyus2																		
        ,JM_UkeEigyos.TelNo AS UkeEigyoTelNo																		
        ,JM_UkeEigyos.FaxNo AS UkeEigyoFaxNo																		
        ,JM_UkeEigyos.MailAcc AS UkeEigyoMailAdr																		
        ,JM_Compny.CompanyNm AS UkeCompanyNm
		,JM_Compny.BusinessPermitDate AS CompanyBusinessPermitDate
		,JM_Compny.BusinessPermitNumber AS CompanyBusinessPermitNumber
		,JM_Compny.BusinessArea AS CompanyBusinessArea
		,JM_Compny.VoluntaryInsuranceHuman AS CompanyVoluntaryInsuranceHuman
		,JM_Compny.VoluntaryInsuranceObject AS CompanyVoluntaryInsuranceObject
        ,JM_UkeEigyos.ZipCd AS UkeEigyoZipCd																		
        ,ISNULL(JT_FutTum.UnsoJippiFut, 0) AS UnsoJippiFut																		
        ,ISNULL(JT_FutTum.UnsoJippiFutTes, 0) AS UnsoJippiFutTes																		
        ,ISNULL(eTKD_YykReport.AllSokoTime, '00000') AS YRep_AllSokoTime																		
        ,ISNULL(eTKD_YykReport.CheckTime, '00000') AS YRep_CheckTime																		
        ,ISNULL(eTKD_YykReport.AdjustTime, '00000') AS YRep_AdjustTime																		
        ,ISNULL(eTKD_YykReport.ShinSoTime, '00000') AS YRep_ShinSoTime																		
        ,ISNULL(eTKD_YykReport.AllSokoKm, 0) AS YRep_AllSokoKm																		
        ,ISNULL(eTKD_YykReport.JiSaTime, '00000') AS YRep_JiSaTime																		
        ,ISNULL(eTKD_YykReport.JiSaKm, 0) AS YRep_JiSaKm																		
        ,ISNULL(eTKD_YykReport.ChangeFlg, 0) AS YRep_ChangeFlg																		
        ,ISNULL(eTKD_YykReport.ChangeKoskTime, '00000') AS YRep_ChangeKoskTime																		
        ,ISNULL(eTKD_YykReport.ChangeShinTime, '00000') AS YRep_ChangeShinTime																		
        ,ISNULL(eTKD_YykReport.ChangeSokoKm, 0) AS YRep_ChangeSokoKm																		
        ,ISNULL(eTKD_YykReport.SpecialFlg, 0) AS YRep_SpecialFlg																		
        ,ISNULL(eTKD_YykReport.WaribikiKbn, 0) AS YRep_WaribikiKbn																		
        ,ISNULL(CONVERT(INTEGER, SUBSTRING(JT_UnkobiExp.ExpItem, 9, 5)), 0) AS UExp_SouTotalKm																		
        ,ISNULL(CONVERT(INTEGER, SUBSTRING(JT_UnkobiExp.ExpItem, 14, 5)), 0) AS UExp_JituKm																		
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 19, 5), '') AS UExp_SumTime																		
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 24, 5), '') AS UExp_JituTime																		
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 29, 5), '00000') AS UExp_ShinSoTime																		
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 34, 1), '0') AS UExp_ChangeFlg																		
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 35, 1), '0') AS UExp_SpecialFlg																		
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 78, 1), '0') AS UExp_YearContractFlg																		
        ,JT_Yyksho.GuiWNin																		
        ,ISNULL(JT_Yyksho.TokuiTanNm, '') AS YykTokuiTanNm																																	
        ,ISNULL(JT_Yyksho.TokuiTel, '') AS YykTokuiTel																		
        ,ISNULL(JT_Yyksho.TokuiFax, '') AS YykTokuiFax																		
        ,ISNULL(JT_Yyksho.TokuiMail, '') AS YykTokuiMail																		
        ,TKD_Unkobi.KanjFax																		
        ,TKD_Unkobi.KanjKeiNo																		
        ,TKD_Unkobi.KanjMail																		
        ,ISNULL(JT_FutTum_Guide.GuideRyo, 0) AS GuideRyo																		
        ,ISNULL(JT_FutTum_Guide.GuideTes, 0) AS GuideTes																		
        ,ISNULL(JT_FutTum_Guide.GuideFutaiNm, '') AS GuideFutaiNm																		
        ,ISNULL(JT_YykSyuSumKata.SumOogata, 0) AS SumOogata																		
        ,ISNULL(JT_YykSyuSumKata.SumChugata, 0) AS SumChugata																		
        ,ISNULL(JT_YykSyuSumKata.SumKogata, 0) AS SumKogata																		
        ,ISNULL(JT_YykSyuSumBooking.BusGakuwari, 0) AS BusGakuwari																		
        ,ISNULL(JT_YykSyuSumBooking.BusShinSyowari, 0) AS BusShinSyowari																		
        ,ISNULL(JT_YykSyuSumUnc.SumBusPrice, 0) AS SumBusPrice																		
        ,ISNULL(JT_YykSyuSumUnc.SumBusFee, 0) AS SumBusFee																		
        ,ISNULL(JT_YykSyuSumBooking.SumFareMaxAmount, 0) AS SumFareMaxAmount																		
        ,ISNULL(JT_YykSyuSumBooking.SumFareMinAmount, 0) AS SumFareMinAmount																		
        ,ISNULL(JT_YykSyuSumBooking.SumFeeMaxAmount, 0) AS SumFeeMaxAmount																		
        ,ISNULL(JT_YykSyuSumBooking.SumFeeMinAmount, 0) AS SumFeeMinAmount
		,ISNULL(JT_YykSyuSumBooking.TotalKmRunning, 0) AS TotalKmRunning
		,ISNULL(JT_YykSyuSumBooking.JituKmRunning, 0) AS JituKmRunning
		,ISNULL(JT_YykSyuSumBooking.TotalHoursRunning, 0) AS TotalHoursRunning
		,ISNULL(JT_YykSyuSumBooking.TotalMinutesRunning, 0) AS TotalMinutesRunning
		,ISNULL(JT_YykSyuSumBooking.JituHoursRunning, 0) AS JituHoursRunning
		,ISNULL(JT_YykSyuSumBooking.JituMinutesRunning, 0) AS JituMinutesRunning
        ,ISNULL(JT_YykSyuSumBooking.AnnualContractFlag, 0) AS AnnualContractFlag																		
        ,ISNULL(JT_YykSyuSumBooking.MidnightEarlyMorningTimeSum, 0) AS MidnightEarlyMorningTimeSum																		
        ,ISNULL(JT_YykSyuSumBooking.SpecialFlg, 0) AS SpecialFlg																		
        ,ISNULL(JT_YykSyuSumBookingMesai.ChangeDriverFeeFlag, 0) AS ChangeDriverFeeFlag																		
FROM TKD_Unkobi																		
LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Unkobi.UkeNo = JT_Yyksho.UkeNo																		
        AND JT_Yyksho.SiyoKbn = 1																
		AND JT_Yyksho.TenantCdSeq = @TenantCdSeq									  
LEFT JOIN (																		
        SELECT UkeNo																		
                ,bikotblseq																		
                ,MIN(BikoRen) AS BikoRen																		
        FROM TKD_Biko																		
        WHERE TKD_Biko.SiyoKbn = 1																		
        GROUP BY UkeNo																		
                ,bikotblseq																		
        ) AS eTKD_Biko01 ON eTKD_Biko01.UkeNo = JT_Yyksho.UkeNo																		
        AND eTKD_Biko01.bikotblseq = JT_Yyksho.bikotblseq																		
LEFT JOIN TKD_Biko AS eTKD_Biko02 ON eTKD_Biko02.UkeNo = eTKD_Biko01.UkeNo																		
        AND eTKD_Biko02.BikoRen = eTKD_Biko01.BikoRen																		
        AND eTKD_Biko02.bikotblseq = eTKD_Biko01.bikotblseq																		
        AND eTKD_Biko02.SiyoKbn = 1																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,COUNT(UkeNo) AS CountKSEigSeq																		
        FROM TKD_Kariei																		
        WHERE TKD_Kariei.SiyoKbn = 1																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS eTKD_Kariei01 ON eTKD_Kariei01.UkeNo = TKD_Unkobi.UkeNo																		
        AND eTKD_Kariei01.UnkRen = TKD_Unkobi.UnkRen																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,MIN(KSEigSeq) AS MinKSEigSeq																		
        FROM TKD_Kariei																		
        WHERE TKD_Kariei.SiyoKbn = 1																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS eTKD_Kariei02 ON eTKD_Kariei02.UkeNo = eTKD_Kariei01.UkeNo																		
        AND eTKD_Kariei02.UnkRen = eTKD_Kariei01.UnkRen																		
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_Eigyos.EigyoCdSeq = eTKD_Kariei02.MinKSEigSeq																		
        AND eVPM_Eigyos.SiyoKbn = 1																		
LEFT JOIN VPM_Tokisk AS JM_Tokisk ON JT_Yyksho.TokuiSeq = JM_Tokisk.TokuiSeq																		
		AND JM_Tokisk.TenantCdSeq = @TenantCdSeq								  
        AND JT_Yyksho.SeiTaiYmd >= JM_Tokisk.SiyoStaYmd																		
        AND JT_Yyksho.SeiTaiYmd <= JM_Tokisk.SiyoEndYmd																		
LEFT JOIN VPM_TokiSt AS JM_TokiSt ON JT_Yyksho.TokuiSeq = JM_TokiSt.TokuiSeq																		
        AND JT_Yyksho.SitenCdSeq = JM_TokiSt.SitenCdSeq																		
        AND JT_Yyksho.SeiTaiYmd >= JM_TokiSt.SiyoStaYmd																		
        AND JT_Yyksho.SeiTaiYmd <= JM_TokiSt.SiyoEndYmd																		
LEFT JOIN VPM_Gyosya AS JM_Gyosya ON JM_Tokisk.GyosyaCdSeq = JM_Gyosya.GyosyaCdSeq																		
        AND JM_Gyosya.SiyoKbn = 1																		
LEFT JOIN VPM_Tokisk AS JM_SirTokisk ON JT_Yyksho.SirCdSeq = JM_SirTokisk.TokuiSeq																		
		AND JM_SirTokisk.TenantCdSeq = @TenantCdSeq									 
        AND JT_Yyksho.SeiTaiYmd >= JM_SirTokisk.SiyoStaYmd																		
        AND JT_Yyksho.SeiTaiYmd <= JM_SirTokisk.SiyoEndYmd																		
LEFT JOIN VPM_TokiSt AS JM_SirTokiSt ON JT_Yyksho.SirCdSeq = JM_SirTokiSt.TokuiSeq																		
        AND JT_Yyksho.SirSitenCdSeq = JM_SirTokiSt.SitenCdSeq																		
        AND JT_Yyksho.SeiTaiYmd >= JM_SirTokiSt.SiyoStaYmd																		
        AND JT_Yyksho.SeiTaiYmd <= JM_SirTokiSt.SiyoEndYmd																		
LEFT JOIN VPM_Gyosya AS JM_SirGyosya ON JM_SirTokisk.GyosyaCdSeq = JM_SirGyosya.GyosyaCdSeq																		
        AND JM_SirGyosya.SiyoKbn = 1																		
LEFT JOIN TKD_UnkobiExp AS JT_UnkobiExp ON TKD_Unkobi.UkeNo = JT_UnkobiExp.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_UnkobiExp.UnkRen																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'SIJJOKBN1'																		
                AND SiyoKbn = 1			
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN1'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_SijJoKbn1 ON TKD_Unkobi.SijJoKbn1 = CONVERT(TINYINT, JM_SijJoKbn1.CodeKbn)																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'SIJJOKBN2'																		
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN2'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_SijJoKbn2 ON TKD_Unkobi.SijJoKbn2 = CONVERT(TINYINT, JM_SijJoKbn2.CodeKbn)																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'SIJJOKBN3'																		
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN3'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_SijJoKbn3 ON TKD_Unkobi.SijJoKbn3 = CONVERT(TINYINT, JM_SijJoKbn3.CodeKbn)																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'SIJJOKBN4'																		
                AND SiyoKbn = 1							
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN4'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_SijJoKbn4 ON TKD_Unkobi.SijJoKbn4 = CONVERT(TINYINT, JM_SijJoKbn4.CodeKbn)																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'SIJJOKBN5'																		
                AND SiyoKbn = 1							
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN5'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_SijJoKbn5 ON TKD_Unkobi.SijJoKbn5 = CONVERT(TINYINT, JM_SijJoKbn5.CodeKbn)																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,SUM(CAST(SyaRyoUnc AS bigint)) AS SumSyaRyoUnc																		
                ,SUM(CAST(SyaRyoSyo AS bigint)) AS SumSyaRyoSyo																		
                ,SUM(CAST(SyaRyoTes AS bigint)) AS SumSyaRyoTes																		
        FROM TKD_Haisha																		
        WHERE SiyoKbn = 1																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS JT_HaishaSyaRyo ON TKD_Unkobi.UkeNo = JT_HaishaSyaRyo.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_HaishaSyaRyo.UnkRen																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,SUM(CAST(SyaRyoUnc AS bigint)) AS SumSyuUnc																		
                ,SUM(CAST(UnitBusPrice AS bigint) * SyaSyuDai) AS SumBusPrice																		
                ,SUM(CAST(UnitBusFee AS bigint) * SyaSyuDai) AS SumBusFee																		
        FROM TKD_YykSyu																		
        WHERE SiyoKbn = 1																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS JT_YykSyuSumUnc ON TKD_Unkobi.UkeNo = JT_YykSyuSumUnc.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_YykSyuSumUnc.UnkRen																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'ZEIKBN'																		
                AND SiyoKbn = 1				
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'ZEIKBN'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_ZeiKbn ON JT_Yyksho.ZeiKbn = CONVERT(TINYINT, JM_ZeiKbn.CodeKbn)																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,SUM(YoushaKin) AS YoushaKin																		
        FROM (																		
                SELECT UkeNo																		
                        ,UnkRen																		
                        ,ISNULL(TKD_Yousha.SyaRyoUnc, 0) + ISNULL(TKD_Yousha.SyaRyoSyo, 0) AS YoushaKin																		
                FROM TKD_Yousha																		
                WHERE SiyoKbn = 1																		
                ) AS Main																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS JT_YoushaSumKin ON TKD_Unkobi.UkeNo = JT_YoushaSumKin.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_YoushaSumKin.UnkRen																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,SUM(UriGakKin + SyaRyoSyo) AS UnsoJippiFut																		
                ,SUM(SyaRyoTes) AS UnsoJippiFutTes																		
        FROM TKD_FutTum																		
        INNER JOIN VPM_Futai ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq																		
                AND VPM_Futai.SiyoKbn = 1																		
        WHERE TKD_FutTum.SiyoKbn = 1																		
                AND TKD_FutTum.SeisanKbn = 1																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS JT_FutTum ON TKD_Unkobi.UkeNo = JT_FutTum.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_FutTum.UnkRen																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,SUM(UriGakKin + SyaRyoSyo) AS GuideRyo																		
                ,SUM(SyaRyoTes) AS GuideTes																		
                ,MAX(FutaiNm) AS GuideFutaiNm																		
        FROM TKD_FutTum																		
        INNER JOIN VPM_Futai ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq																		
                AND VPM_Futai.SiyoKbn = 1																		
        WHERE TKD_FutTum.SiyoKbn = 1																		
                AND TKD_FutTum.SeisanKbn = 1																		
                AND VPM_Futai.FutGuiKbn = 5																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS JT_FutTum_Guide ON TKD_Unkobi.UkeNo = JT_FutTum_Guide.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_FutTum_Guide.UnkRen																		
LEFT JOIN (																		
        SELECT UkeNo																		
                ,UnkRen																		
                ,Sum(JisaIPKm + JisaKSKm + KisoIPkm + KisoKOKm) AS SouTotalKm																		
                ,SUM(JisaIPKm + JisaKSKm) AS JituKm																		
        FROM TKD_Koteik																		
        WHERE TKD_Koteik.SiyoKbn = 1																		
        GROUP BY UkeNo																		
                ,UnkRen																		
        ) AS JT_Koteik ON TKD_Unkobi.UkeNo = JT_Koteik.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_Koteik.UnkRen																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'UKEJYKBNCD'																		
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'UKEJYKBNCD'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_UkeJyKbnCd ON TKD_Unkobi.UkeJyKbnCd = CONVERT(TINYINT, JM_UkeJyKbnCd.CodeKbn)																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'OTHJINKBN'																		
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'OTHJINKBN'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_OthJinKbn1 ON TKD_Unkobi.OthJinKbn1 = CONVERT(TINYINT, JM_OthJinKbn1.CodeKbn)																		
LEFT JOIN (																		
        SELECT CodeKbn																		
                ,CodeKbnNm																		
                ,RyakuNm																		
        FROM VPM_CodeKb																		
        WHERE CodeSyu = 'OTHJINKBN'																		
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'OTHJINKBN'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        ) AS JM_OthJinKbn2 ON TKD_Unkobi.OthJinKbn2 = CONVERT(TINYINT, JM_OthJinKbn2.CodeKbn)																		
LEFT JOIN (																		
        SELECT TKD_Haisha.UkeNo																		
                ,TKD_Haisha.UnkRen																		
                ,SUBSTRING(MIN(SyuKoYmd + SyuKoTime), 9, 4) AS Min_SyuKoTime																		
                ,SUBSTRING(MAX(KikYmd + KikTime), 9, 4) AS Max_KikTime																		
                ,SUBSTRING(MIN(SyuKoYmd + SyuKoTime), 1, 8) AS Min_SyuKoYmd																		
                ,SUBSTRING(MAX(KikYmd + KikTime), 1, 8) AS Max_KikYmd																																		
        FROM TKD_Haisha																		
        WHERE SiyoKbn = 1																		
        GROUP BY TKD_Haisha.UkeNo																		
                ,TKD_Haisha.UnkRen																		
        ) AS Unk_Haisha ON TKD_Unkobi.UkeNo = Unk_Haisha.UkeNo																		
        AND TKD_Unkobi.UnkRen = Unk_Haisha.UnkRen																		
LEFT JOIN VPM_Eigyos AS JM_UkeEigyos ON JT_Yyksho.UkeEigCdSeq = JM_UkeEigyos.EigyoCdSeq																		
        AND JM_UkeEigyos.SiyoKbn = 1																		
LEFT JOIN VPM_Compny AS JM_Compny ON JM_Compny.CompanyCdSeq = JM_UkeEigyos.CompanyCdSeq																		
LEFT JOIN VPM_Syain AS JM_EigTan ON JT_Yyksho.EigTanCdSeq = JM_EigTan.SyainCdSeq																		
LEFT JOIN VPM_Syain AS JM_InTan ON JT_Yyksho.InTanCdSeq = JM_InTan.SyainCdSeq																		
LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq																		
        AND JT_Yyksho.SiyoKbn = 1																		
		AND JT_Yyksho.TenantCdSeq = @TenantCdSeq								  
LEFT JOIN (																		
        SELECT eTKD_YykReport.UkeNo																		
                ,eTKD_YykReport.UnkRen																		
                ,MAX(AllSokoTime) AS AllSokoTime																		
                ,MAX(CheckTime) AS CheckTime																		
                ,MAX(AdjustTime) AS AdjustTime																		
                ,MAX(ShinSoTime) AS ShinSoTime																		
                ,MAX(AllSokoKm) AS AllSokoKm																		
                ,MAX(JiSaTime) AS JiSaTime																		
                ,MAX(JiSaKm) AS JiSaKm																		
                ,MAX(ChangeFlg) AS ChangeFlg																		
                ,MAX(ChangeKoskTime) AS ChangeKoskTime																		
                ,MAX(ChangeShinTime) AS ChangeShinTime																		
                ,MAX(ChangeSokoKm) AS ChangeSokoKm																		
                ,MAX(SpecialFlg) AS SpecialFlg																		
                ,MAX(TKD_Unkobi.WaribikiKbn) AS WaribikiKbn																		
        FROM TKD_Unkobi																		
        LEFT JOIN TKD_YykSyu AS eTKD_YykSyu ON TKD_Unkobi.UkeNo = eTKD_YykSyu.UkeNo																		
                AND TKD_Unkobi.UnkRen = eTKD_YykSyu.UnkRen																		
        LEFT JOIN TKD_YykReport AS eTKD_YykReport ON eTKD_YykSyu.UkeNo = eTKD_YykReport.UkeNo																		
                AND eTKD_YykSyu.UnkRen = eTKD_YykReport.UnkRen																		
                AND eTKD_YykSyu.SyaSyuRen = eTKD_YykReport.SyaSyuRen																		
        WHERE eTKD_YykSyu.SiyoKbn = 1																		
        GROUP BY eTKD_YykReport.UkeNo																		
                ,eTKD_YykReport.UnkRen																		
        ) AS eTKD_YykReport ON TKD_Unkobi.UkeNo = eTKD_YykReport.UkeNo																		
        AND TKD_Unkobi.UnkRen = eTKD_YykReport.UnkRen																		
LEFT JOIN (																		
        SELECT TKD_YykSyu.UkeNo																		
                ,TKD_YykSyu.UnkRen																		
                ,SUM(CASE 																		
                                WHEN VPM_CodeKb.CodeKbnNm = '大型'																		
                                        THEN TKD_YykSyu.SyaSyuDai																		
                                ELSE NULL																		
                                END) AS SumOogata																		
                ,SUM(CASE 																		
                                WHEN VPM_CodeKb.CodeKbnNm = '中型'																		
                                        THEN TKD_YykSyu.SyaSyuDai																		
                                ELSE NULL																		
                                END) AS SumChugata																		
                ,SUM(CASE 																		
                                WHEN VPM_CodeKb.CodeKbnNm = '小型'																		
                                        THEN TKD_YykSyu.SyaSyuDai																		
                                ELSE NULL																		
                                END) AS SumKogata																		
        FROM TKD_YykSyu																		
        INNER JOIN VPM_CodeKb ON TKD_YykSyu.KataKbn = VPM_CodeKb.CodeKbn																		
                AND VPM_CodeKb.CodeSyu = 'KATAKBN'																		
                AND VPM_CodeKb.SiyoKbn = 1				
                AND VPM_CodeKb.TenantCdSeq = (														
                        SELECT CASE 														
                                        WHEN COUNT(*) = 0														
                                                THEN 0														
                                        ELSE @TenantCdSeq														
                                        END AS TenantCdSeq														
                        FROM VPM_CodeKb														
                        WHERE VPM_CodeKb.CodeSyu = 'KATAKBN'														
                                AND VPM_CodeKb.SiyoKbn = 1														
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq
                        )														
        GROUP BY TKD_YykSyu.UkeNo																		
                ,TKD_YykSyu.UnkRen																		
        ) AS JT_YykSyuSumKata ON TKD_Unkobi.UkeNo = JT_YykSyuSumKata.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_YykSyuSumKata.UnkRen																		
LEFT JOIN (																		
        SELECT TKD_YykSyu.UkeNo																		
                ,TKD_YykSyu.UnkRen																		
                ,COUNT(CASE 																		
                                WHEN TKD_BookingMaxMinFareFeeCalc.WaribikiKbn = 1																		
                                        THEN 1																		
                                ELSE NULL																		
                                END) AS BusShinSyowari																		
                ,COUNT(CASE 																		
                                WHEN TKD_BookingMaxMinFareFeeCalc.WaribikiKbn = 2																		
                                        THEN 1																		
                                ELSE NULL																		
                                END) AS BusGakuwari																		
                ,SUM(CAST(TKD_BookingMaxMinFareFeeCalc.FareMaxAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFareMaxAmount																		
                ,SUM(CAST(TKD_BookingMaxMinFareFeeCalc.FareMinAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFareMinAmount																		
                ,SUM(CAST(TKD_BookingMaxMinFareFeeCalc.FeeMaxAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFeeMaxAmount																		
                ,SUM(CAST(TKD_BookingMaxMinFareFeeCalc.FeeMinAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFeeMinAmount																		
                ,SUM(TKD_BookingMaxMinFareFeeCalc.AnnualContractFlag) AS AnnualContractFlag		
				,SUM(TKD_BookingMaxMinFareFeeCalc.RunningKmCalc) AS TotalKmRunning
				,SUM(TKD_BookingMaxMinFareFeeCalc.ServiceKmSum) AS JituKmRunning
				,SUM(CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.RestraintTimeCalc, 1, 2))) AS TotalHoursRunning
				,SUM(CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.RestraintTimeCalc, 3, 2))) AS TotalMinutesRunning
				,SUM(CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.ServiceTimeSum, 1, 2))) AS JituHoursRunning
				,SUM(CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.ServiceTimeSum, 3, 2))) AS JituMinutesRunning
                ,COUNT(CASE 																		
                                WHEN TKD_BookingMaxMinFareFeeCalc.MidnightEarlyMorningTimeSum <> '0000'																		
                                        THEN 1																		
                                ELSE NULL																		
                                END) AS MidnightEarlyMorningTimeSum																		
                ,SUM(TKD_BookingMaxMinFareFeeCalc.SpecialFlg) AS SpecialFlg																		
        FROM TKD_YykSyu																		
        INNER JOIN TKD_BookingMaxMinFareFeeCalc ON TKD_YykSyu.UkeNo = TKD_BookingMaxMinFareFeeCalc.UkeNo																		
                AND TKD_YykSyu.UnkRen = TKD_BookingMaxMinFareFeeCalc.UnkRen																		
                AND TKD_YykSyu.SyaSyuRen = TKD_BookingMaxMinFareFeeCalc.SyaSyuRen																		
        WHERE SiyoKbn = 1																		
        GROUP BY TKD_YykSyu.UkeNo																		
                ,TKD_YykSyu.UnkRen																		
        ) AS JT_YykSyuSumBooking ON TKD_Unkobi.UkeNo = JT_YykSyuSumBooking.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_YykSyuSumBooking.UnkRen																		
LEFT JOIN (																		
        SELECT TKD_BookingMaxMinFareFeeCalc.UkeNo																		
                ,TKD_BookingMaxMinFareFeeCalc.UnkRen																		
                ,SUM(TKD_BookingMaxMinFareFeeCalcMeisai.ChangeDriverFeeFlag) AS ChangeDriverFeeFlag																		
        FROM TKD_BookingMaxMinFareFeeCalc																		
        INNER JOIN TKD_BookingMaxMinFareFeeCalcMeisai ON TKD_BookingMaxMinFareFeeCalc.UkeNo = TKD_BookingMaxMinFareFeeCalcMeisai.UkeNo																		
                AND TKD_BookingMaxMinFareFeeCalc.UnkRen = TKD_BookingMaxMinFareFeeCalcMeisai.UnkRen																		
                AND TKD_BookingMaxMinFareFeeCalc.SyaSyuRen = TKD_BookingMaxMinFareFeeCalcMeisai.SyaSyuRen																		
        GROUP BY TKD_BookingMaxMinFareFeeCalc.UkeNo																		
                ,TKD_BookingMaxMinFareFeeCalc.UnkRen																		
        ) AS JT_YykSyuSumBookingMesai ON TKD_Unkobi.UkeNo = JT_YykSyuSumBookingMesai.UkeNo																		
        AND TKD_Unkobi.UnkRen = JT_YykSyuSumBookingMesai.UnkRen																		

WHERE JT_Yyksho.YoyaSyu = 1							
	AND TKD_Unkobi.SiyoKbn = 1 						
	-- IF 日付 = 1-配車日付		||-- IF 日付 = 2-到着日付	|| -- IF 日付 = 3-受付日付		
	AND (@StartDispatchDate IS NULL OR @StartDispatchDate = '' OR TKD_Unkobi.HaiSYmd >= @StartDispatchDate)              -- 配車日　開始
	AND (@EndDispatchDate IS NULL OR @EndDispatchDate = '' OR TKD_Unkobi.HaiSYmd <= @EndDispatchDate)                    -- 配車日　終了
	AND (@StartArrivalDate IS NULL OR @StartArrivalDate = '' OR TKD_Unkobi.TouYmd >= @StartArrivalDate)                  -- 到着日　開始
	AND (@EndArrivalDate IS NULL OR @EndArrivalDate = '' OR TKD_Unkobi.TouYmd <= @EndArrivalDate)                        -- 到着日　終了
	AND (@StartReservationDate IS NULL OR @StartReservationDate = '' OR UkeYmd >= @StartReservationDate)                 -- 予約日　開始
	AND (@EndReservationDate IS NULL OR @EndReservationDate = '' OR UkeYmd<= @EndReservationDate)                        -- 予約日　終了
	-- 受付営業所：						
	AND (@UkeEigCd IS NULL OR @UkeEigCd = '' OR JM_UkeEigyos.EigyoCd = @UkeEigCd)
	-- 営業担当者：						
	AND (@EigSyainCd IS NULL OR @EigSyainCd = '' OR JM_EigTan.SyainCd = @EigSyainCd)					
	-- 入力担当者：						
	AND (@InpSyainCd IS NULL  OR @InpSyainCd = '' OR JM_InTan.SyainCd = @InpSyainCd)		
	-- 受付番号：						
	AND (@UkeNo IS NULL OR @UkeNo = '' OR TKD_Unkobi.UkeNo IN (select value from string_split(@UkeNo, ',')))
	-- 予約区分：						
	AND (@YoyaKbnList IS NULL OR @YoyaKbnList = '' OR JM_YoyKbn.YoyaKbn IN (select value from string_split(@YoyaKbnList, ',')))	
	-- 得意先：						
	AND (@TokuiCd IS NULL OR @TokuiCd = '' OR JM_Tokisk.TokuiCd = @TokuiCd)
	AND (@SitenCd IS NULL OR @SitenCd = '' OR JM_TokiSt.SitenCd = @SitenCd)
	-- IF 出力選択 <> 0  (0:すべて 1:未出力のみ)						
	AND	(@OutSelect = 0 OR @OutSelect = '' OR ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem,0,9),'') = '')
	-- IF 年間契約出力フラグ <> 0 (0:出力する 1:出力しない)						
	AND	(@NenKeiyakuOutFlg = 0 OR @NenKeiyakuOutFlg = '' OR ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem,78,1),0) = 0)
	-- 運行日テーブルの連携						
	AND (@UnkRen IS NULL OR @UnkRen = '' OR TKD_Unkobi.UnkRen = @UnkRen)

ORDER BY TKD_Unkobi.UkeNo
SET	@ROWCOUNT	=	@@ROWCOUNT

END
ELSE
BEGIN
    SELECT TKD_Unkobi.UkeNo																	
        ,TKD_Unkobi.UnkRen																	
        ,TKD_Unkobi.HaiSYmd																	
        ,TKD_Unkobi.TouYmd																	
        ,TKD_Unkobi.DanTaNm																	
        ,TKD_Unkobi.KanjJyus1																	
        ,TKD_Unkobi.KanjJyus2																	
        ,TKD_Unkobi.KanjTel																	
        ,TKD_Unkobi.KanJNm																	
        ,TKD_Unkobi.KanjKeiNo																	
        ,TKD_Unkobi.IkNm																	
        ,TKD_Unkobi.ZenHaFlg																	
        ,TKD_Unkobi.KhakFlg																	
        ,ISNULL(JM_SirTokisk.TokuiNm, '') AS SirTokuiNm																	
        ,ISNULL(JM_SirTokiSt.TokuiTanNm, '') AS SirTokuiTanNm																	
        ,ISNULL(JM_SirTokiSt.SitenNm, '') AS SirSitenNm																	
        ,ISNULL(JM_SirTokiSt.TelNo, '') AS SirTelNo																	
        ,ISNULL(JM_SirTokiSt.FaxNo, '') AS SirFaxNo																	
        ,ISNULL(JM_SirTokiSt.TokuiMail, '') AS SirTokuiMail																	
        ,ISNULL(JM_SirTokiSt.Jyus1, '') AS SirJyus1																	
        ,ISNULL(JM_SirTokiSt.Jyus2, '') AS SirJyus2																	
        ,ISNULL(JM_SirTokiSt.TesuShihKbn, '') AS TesuShihKbn																	
        ,ISNULL(JM_Tokisk.TokuiNm, '') AS TokuiNm																	
        ,ISNULL(JM_TokiSt.SitenNm, '') AS SitenNm																	
        ,ISNULL(JM_TokiSt.Jyus1, '') AS TokiStJyus1																	
        ,ISNULL(JM_TokiSt.Jyus2, '') AS TokiStJyus2																	
        ,ISNULL(Unk_Haisha.Min_SyuKoYmd, '') AS SyuKoYmd																	
        ,ISNULL(Unk_Haisha.Max_KikYmd, '') AS KikYmd																	
        ,ISNULL(Unk_Haisha.Min_SyuKoTime, '') AS SyuKoTime																	
        ,ISNULL(Unk_Haisha.Max_KikTime, '') AS KikTime																	
        ,ISNULL(JM_TokiSt.TokuiMail, '') AS TokiStMail																	
        ,ISNULL(JM_TokiSt.TelNo, '') AS TokuiTel																	
        ,ISNULL(JM_TokiSt.FaxNo, '') AS TokuiFax																	
        ,ISNULL(JM_TokiSt.TokuiTanNm, '') AS TokuiTanNm																	
        ,TKD_Unkobi.HaiSNm																	
        ,TKD_Unkobi.HaiSJyus1																	
        ,TKD_Unkobi.HaiSJyus2																	
        ,ISNULL(JT_UnkobiExp.HaiSKouKNm, '') AS HaiSKoukRyaku																	
        ,ISNULL(JT_UnkobiExp.HaiSBinNm, '') AS HaiSBinNm																	
        ,TKD_Unkobi.HaiSSetTime																	
        ,TKD_Unkobi.HaiSTime																	
        ,TKD_Unkobi.DrvJin																	
        ,TKD_Unkobi.GuiSu																	
        ,ISNULL(JM_EigTan.SyainNm, '') AS EigTanNm																	
        ,TKD_Unkobi.SyuPaTime																	
        ,TKD_Unkobi.TouNm																	
        ,TKD_Unkobi.TouJyusyo1																	
        ,TKD_Unkobi.TouJyusyo2																	
        ,ISNULL(JT_UnkobiExp.TouSKouKNm, '') AS TouChaKoukRyaku																	
        ,ISNULL(JT_UnkobiExp.TouSBinNm, '') AS TouChaBinNm																	
        ,TKD_Unkobi.TouSetTime																	
        ,TKD_Unkobi.TouChTime																	
        ,TKD_Unkobi.IkNm																	
        ,ISNULL(JM_SijJoKbn1.RyakuNm, '') AS SijJoKbn1Ryaku																	
        ,ISNULL(JM_SijJoKbn2.RyakuNm, '') AS SijJoKbn2Ryaku																	
        ,ISNULL(JM_SijJoKbn3.RyakuNm, '') AS SijJoKbn3Ryaku																	
        ,ISNULL(JM_SijJoKbn4.RyakuNm, '') AS SijJoKbn4Ryaku																	
        ,ISNULL(JM_SijJoKbn5.RyakuNm, '') AS SijJoKbn5Ryaku																	
        ,ISNULL(JT_Haisha.SumSyaRyoUnc, 0) AS SumSyaRyoUnc																	
        ,ISNULL(JT_Haisha.SumSyaRyoSyo, 0) AS SumSyaRyoSyo																	
        ,ISNULL(JT_Haisha.SumSyaRyoTes, 0) AS SumSyaRyoTes																	
        ,ISNULL(JT_Yyksho.ZeiKbn, 0) AS ZeiKbn																	
        ,ISNULL(JM_ZeiKbn.CodeKbnNm, '') AS ZeiKbnNm																	
        ,ISNULL(JM_ZeiKbn.RyakuNm, '') AS ZeiKbnRyaku																	
        ,ISNULL(JT_Yyksho.Zeiritsu, 0) AS Zeiritsu																	
        ,ISNULL(JT_Yyksho.ZeiRui, 0) AS ZeiRui																	
        ,ISNULL(JT_Yyksho.TesuRitu, 0) AS TesuRitu																	
        ,ISNULL(JT_Yyksho.TesuRyoG, 0) AS TesuRyoG																	
        ,ISNULL(JT_YoushaSumKin.YoushaKin, 0) AS YoushaKin																	
        ,JT_Yyksho.BikoTblSeq																	
        ,ISNULL(eTKD_Biko02.BikoRen, 0) AS BikoRen																	
        ,ISNULL(eTKD_Biko02.BikoNm, '') AS YykBikoNm																	
        ,ISNULL(eTKD_Kariei02.MinKSEigSeq, 0) AS MinKSEigSeq																	
        ,ISNULL(eTKD_Kariei01.CountKSEigSeq, 0) AS CountKSEigSeq																	
        ,ISNULL(eVPM_Eigyos.EigyoNm, '') AS KSEigyoNm																	
        ,ISNULL(eVPM_Eigyos.TelNo, '') AS KSEigyoTelNo																	
        ,ISNULL(eVPM_Eigyos.RyakuNm, '') AS KSEigyoRyakuNm																	
        ,TKD_Unkobi.UkeJyKbnCd																	
        ,ISNULL(JM_UkeJyKbnCd.CodeKbnNm, '') AS UkeJyKbnCdNm																	
        ,ISNULL(JM_UkeJyKbnCd.RyakuNm, '') AS UkeJyKbnCdRyaku																	
        ,TKD_Unkobi.OthJinKbn1																	
        ,ISNULL(JM_OthJinKbn1.CodeKbnNm, '') AS OthJinKbn1Nm																	
        ,ISNULL(JM_OthJinKbn1.RyakuNm, '') AS OthJinKbn1Ryaku																	
        ,TKD_Unkobi.OthJin1																	
        ,TKD_Unkobi.OthJinKbn2																	
        ,ISNULL(JM_OthJinKbn1.CodeKbnNm, '') AS OthJinKbn2Nm																	
        ,ISNULL(JM_OthJinKbn1.RyakuNm, '') AS OthJinKbn2Ryaku																	
        ,TKD_Unkobi.OthJin2																	
        ,TKD_Unkobi.JyoSyaJin																	
        ,TKD_Unkobi.PlusJin																	
        ,ISNULL(JM_YoyKbn.YoyaKbn, 0) AS YoyaKbn																	
        ,ISNULL(JT_Yyksho.UkeYmd, '') AS UkeYmd																	
        ,ISNULL(JM_InTan.SyainCd, 0) AS InTanSyainCd																	
        ,ISNULL(JM_InTan.SyainNm, '') AS InTanSyainNm																	
        ,TKD_Unkobi.DanTaKana AS DanTaKana																	
        ,ISNULL(JM_Gyosya.GyosyaCd, 0) AS GyosyaCd																	
        ,ISNULL(JM_Tokisk.TokuiCd, 0) AS TokuiCd																	
        ,ISNULL(JM_TokiSt.SitenCd, 0) AS SitenCd																	
        ,TKD_Unkobi.BikoTblSeq AS Unkobi_BikoTblSeq																	
        ,JM_UkeEigyos.EigyoNm AS UkeEigyoNm																	
        ,JM_UkeEigyos.Jyus1 AS UkeEigyoJyus1																	
        ,JM_UkeEigyos.Jyus2 AS UkeEigyoJyus2																	
        ,JM_UkeEigyos.TelNo AS UkeEigyoTelNo																	
        ,JM_UkeEigyos.FaxNo AS UkeEigyoFaxNo																	
        ,JM_UkeEigyos.MailAcc AS UkeEigyoMailAdr																	
		,JM_Compny.CompanyNm AS UkeCompanyNm
		,JM_Compny.BusinessPermitDate AS CompanyBusinessPermitDate
		,JM_Compny.BusinessPermitNumber AS CompanyBusinessPermitNumber
		,JM_Compny.BusinessArea AS CompanyBusinessArea
		,JM_Compny.VoluntaryInsuranceHuman AS CompanyVoluntaryInsuranceHuman
		,JM_Compny.VoluntaryInsuranceObject AS CompanyVoluntaryInsuranceObject
        ,JM_UkeEigyos.ZipCd AS UkeEigyoZipCd																	
        ,ISNULL(JT_FutTum.UnsoJippiFut, 0) AS UnsoJippiFut																	
        ,ISNULL(JT_FutTum.UnsoJippiFutTes, 0) AS UnsoJippiFutTes																	
        ,ISNULL(eTKD_YykReport.AllSokoTime, '00000') AS YRep_AllSokoTime																	
        ,ISNULL(eTKD_YykReport.CheckTime, '00000') AS YRep_CheckTime																	
        ,ISNULL(eTKD_YykReport.AdjustTime, '00000') AS YRep_AdjustTime																	
        ,ISNULL(eTKD_YykReport.ShinSoTime, '00000') AS YRep_ShinSoTime																	
        ,ISNULL(eTKD_YykReport.AllSokoKm, 0) AS YRep_AllSokoKm																	
        ,ISNULL(eTKD_YykReport.JiSaTime, '00000') AS YRep_JiSaTime																	
        ,ISNULL(eTKD_YykReport.JiSaKm, 0) AS YRep_JiSaKm																	
        ,ISNULL(eTKD_YykReport.ChangeFlg, 0) AS YRep_ChangeFlg																	
        ,ISNULL(eTKD_YykReport.ChangeKoskTime, '00000') AS YRep_ChangeKoskTime																	
        ,ISNULL(eTKD_YykReport.ChangeShinTime, '00000') AS YRep_ChangeShinTime																	
        ,ISNULL(eTKD_YykReport.ChangeSokoKm, 0) AS YRep_ChangeSokoKm																	
        ,ISNULL(eTKD_YykReport.SpecialFlg, 0) AS YRep_SpecialFlg																	
        ,ISNULL(eTKD_YykReport.WaribikiKbn, 0) AS YRep_WaribikiKbn																	
        ,ISNULL(CONVERT(INTEGER, SUBSTRING(JT_UnkobiExp.ExpItem, 9, 5)), 0) AS UExp_SouTotalKm																	
        ,ISNULL(CONVERT(INTEGER, SUBSTRING(JT_UnkobiExp.ExpItem, 14, 5)), 0) AS UExp_JituKm																	
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 19, 5), '') AS UExp_SumTime																	
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 24, 5), '') AS UExp_JituTime																	
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 29, 5), '00000') AS UExp_ShinSoTime																	
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 34, 1), '0') AS UExp_ChangeFlg																	
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 35, 1), '0') AS UExp_SpecialFlg																	
        ,ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem, 78, 1), '0') AS UExp_YearContractFlg																	
        ,JT_Yyksho.GuiWNin																	
        ,ISNULL(JT_Yyksho.TokuiTanNm, '') AS YykTokuiTanNm																																		
        ,ISNULL(JT_Yyksho.TokuiTel, '') AS YykTokuiTel																	
        ,ISNULL(JT_Yyksho.TokuiFax, '') AS YykTokuiFax																	
        ,ISNULL(JT_Yyksho.TokuiMail, '') AS YykTokuiMail																	
        ,TKD_Unkobi.KanjFax																	
        ,TKD_Unkobi.KanjKeiNo																	
        ,TKD_Unkobi.KanjMail																	
        ,ISNULL(JT_FutTum_Guide.GuideRyo, 0) AS GuideRyo																	
        ,ISNULL(JT_FutTum_Guide.GuideTes, 0) AS GuideTes																	
        ,ISNULL(JT_FutTum_Guide.GuideFutaiNm, '') AS GuideFutaiNm																	
        ,ISNULL(JT_YykSyuSumKata.SumOogata, 0) AS SumOogata																	
        ,ISNULL(JT_YykSyuSumKata.SumChugata, 0) AS SumChugata																	
        ,ISNULL(JT_YykSyuSumKata.SumKogata, 0) AS SumKogata																	
        ,ISNULL(JT_YykSyuSumBooking.BusGakuwari, 0) AS BusGakuwari																	
        ,ISNULL(JT_YykSyuSumBooking.BusShinSyowari, 0) AS BusShinSyowari																	
        ,ISNULL(JT_YykSyuSumUnc.SumBusPrice, 0) AS SumBusPrice																	
        ,ISNULL(JT_YykSyuSumUnc.SumBusFee, 0) AS SumBusFee																	
        ,ISNULL(JT_YykSyuSumBooking.SumFareMaxAmount, 0) AS SumFareMaxAmount																	
        ,ISNULL(JT_YykSyuSumBooking.SumFareMinAmount, 0) AS SumFareMinAmount																	
        ,ISNULL(JT_YykSyuSumBooking.SumFeeMaxAmount, 0) AS SumFeeMaxAmount																	
        ,ISNULL(JT_YykSyuSumBooking.SumFeeMinAmount, 0) AS SumFeeMinAmount			
		,ISNULL(JT_YykSyuSumBooking.TotalKmRunning, 0) AS TotalKmRunning
		,ISNULL(JT_YykSyuSumBooking.JituKmRunning, 0) AS JituKmRunning
		,ISNULL(JT_YykSyuSumBooking.TotalHoursRunning, 0) AS TotalHoursRunning
		,ISNULL(JT_YykSyuSumBooking.TotalMinutesRunning, 0) AS TotalMinutesRunning
		,ISNULL(JT_YykSyuSumBooking.JituHoursRunning, 0) AS JituHoursRunning
		,ISNULL(JT_YykSyuSumBooking.JituMinutesRunning, 0) AS JituMinutesRunning
        ,ISNULL(JT_YykSyuSumBooking.MidnightEarlyMorningTimeSum, 0) AS MidnightEarlyMorningTimeSum																	
        ,ISNULL(JT_YykSyuSumBooking.SpecialFlg, 0) AS SpecialFlg																	
        ,ISNULL(JT_YykSyuSumBooking.AnnualContractFlag, 0) AS AnnualContractFlag																	
        ,ISNULL(JT_YykSyuSumBooking.ChangeDriverFeeFlag, 0) AS ChangeDriverFeeFlag
		,ISNULL(JT_YykSyuSumBooking.SyaSyuRen, 0) AS SyaSyuRen 
FROM TKD_Unkobi																	
LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Unkobi.UkeNo = JT_Yyksho.UkeNo																	
        AND JT_Yyksho.SiyoKbn = 1																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,bikotblseq																	
                ,MIN(BikoRen) AS BikoRen																	
        FROM TKD_Biko																	
        WHERE TKD_Biko.SiyoKbn = 1																	
        GROUP BY UkeNo																	
                ,bikotblseq																	
        ) AS eTKD_Biko01 ON eTKD_Biko01.UkeNo = JT_Yyksho.UkeNo																	
        AND eTKD_Biko01.bikotblseq = JT_Yyksho.bikotblseq																	
LEFT JOIN TKD_Biko AS eTKD_Biko02 ON eTKD_Biko02.UkeNo = eTKD_Biko01.UkeNo																	
        AND eTKD_Biko02.BikoRen = eTKD_Biko01.BikoRen																	
        AND eTKD_Biko02.bikotblseq = eTKD_Biko01.bikotblseq																	
        AND eTKD_Biko02.SiyoKbn = 1																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,COUNT(UkeNo) AS CountKSEigSeq																	
        FROM TKD_Kariei																	
        WHERE TKD_Kariei.SiyoKbn = 1																	
        GROUP BY UkeNo																	
                ,UnkRen																	
        ) AS eTKD_Kariei01 ON eTKD_Kariei01.UkeNo = TKD_Unkobi.UkeNo												
		
        AND eTKD_Kariei01.UnkRen = TKD_Unkobi.UnkRen																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,MIN(KSEigSeq) AS MinKSEigSeq																	
        FROM TKD_Kariei																	
        WHERE TKD_Kariei.SiyoKbn = 1																	
        GROUP BY UkeNo																	
                ,UnkRen																	
        ) AS eTKD_Kariei02 ON eTKD_Kariei02.UkeNo = eTKD_Kariei01.UkeNo																	
        AND eTKD_Kariei02.UnkRen = eTKD_Kariei01.UnkRen																	
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_Eigyos.EigyoCdSeq = eTKD_Kariei02.MinKSEigSeq																	
        AND eVPM_Eigyos.SiyoKbn = 1																	
LEFT JOIN VPM_Tokisk AS JM_Tokisk ON JT_Yyksho.TokuiSeq = JM_Tokisk.TokuiSeq																	
		AND JM_Tokisk.TenantCdSeq = @TenantCdSeq								  
        AND JT_Yyksho.SeiTaiYmd >= JM_Tokisk.SiyoStaYmd																	
        AND JT_Yyksho.SeiTaiYmd <= JM_Tokisk.SiyoEndYmd																	
LEFT JOIN VPM_TokiSt AS JM_TokiSt ON JT_Yyksho.TokuiSeq = JM_TokiSt.TokuiSeq																	
        AND JT_Yyksho.SitenCdSeq = JM_TokiSt.SitenCdSeq																	
        AND JT_Yyksho.SeiTaiYmd >= JM_TokiSt.SiyoStaYmd																	
        AND JT_Yyksho.SeiTaiYmd <= JM_TokiSt.SiyoEndYmd																	
LEFT JOIN VPM_Gyosya AS JM_Gyosya ON JM_Tokisk.GyosyaCdSeq = JM_Gyosya.GyosyaCdSeq																	
        AND JM_Gyosya.SiyoKbn = 1																	
LEFT JOIN VPM_Tokisk AS JM_SirTokisk ON JT_Yyksho.SirCdSeq = JM_SirTokisk.TokuiSeq																	
		AND JM_SirTokisk.TenantCdSeq = @TenantCdSeq									 
        AND JT_Yyksho.SeiTaiYmd >= JM_SirTokisk.SiyoStaYmd																	
        AND JT_Yyksho.SeiTaiYmd <= JM_SirTokisk.SiyoEndYmd																	
LEFT JOIN VPM_TokiSt AS JM_SirTokiSt ON JT_Yyksho.SirCdSeq = JM_SirTokiSt.TokuiSeq																	
        AND JT_Yyksho.SirSitenCdSeq = JM_SirTokiSt.SitenCdSeq																	
        AND JT_Yyksho.SeiTaiYmd >= JM_SirTokiSt.SiyoStaYmd																	
        AND JT_Yyksho.SeiTaiYmd <= JM_SirTokiSt.SiyoEndYmd																	
LEFT JOIN VPM_Gyosya AS JM_SirGyosya ON JM_SirTokisk.GyosyaCdSeq = JM_SirGyosya.GyosyaCdSeq																	
        AND JM_SirGyosya.SiyoKbn = 1																	
LEFT JOIN TKD_UnkobiExp AS JT_UnkobiExp ON TKD_Unkobi.UkeNo = JT_UnkobiExp.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_UnkobiExp.UnkRen																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'SIJJOKBN1'																	
                AND SiyoKbn = 1										
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN1'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
		) AS JM_SijJoKbn1 ON TKD_Unkobi.SijJoKbn1 = CONVERT(TINYINT, JM_SijJoKbn1.CodeKbn)																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'SIJJOKBN2'																	
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN2'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_SijJoKbn2 ON TKD_Unkobi.SijJoKbn2 = CONVERT(TINYINT, JM_SijJoKbn2.CodeKbn)																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'SIJJOKBN3'																	
                AND SiyoKbn = 1							
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN3'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_SijJoKbn3 ON TKD_Unkobi.SijJoKbn3 = CONVERT(TINYINT, JM_SijJoKbn3.CodeKbn)																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'SIJJOKBN4'																	
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN4'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_SijJoKbn4 ON TKD_Unkobi.SijJoKbn4 = CONVERT(TINYINT, JM_SijJoKbn4.CodeKbn)																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'SIJJOKBN5'																	
                AND SiyoKbn = 1							
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'SIJJOKBN5'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_SijJoKbn5 ON TKD_Unkobi.SijJoKbn5 = CONVERT(TINYINT, JM_SijJoKbn5.CodeKbn)																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,SyaSyuRen																	
                ,CAST(UnitBusPrice AS bigint) * SyaSyuDai AS SumBusPrice																	
                ,CAST(UnitBusFee AS bigint) * SyaSyuDai AS SumBusFee																	
        FROM TKD_YykSyu																	
        WHERE SiyoKbn = 1																	
        ) AS JT_YykSyuSumUnc ON TKD_Unkobi.UkeNo = JT_YykSyuSumUnc.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_YykSyuSumUnc.UnkRen																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,SyaSyuRen																	
                ,SUM(CAST(SyaRyoUnc AS bigint)) AS SumSyaRyoUnc																	
                ,SUM(CAST(SyaRyoSyo AS bigint)) AS SumSyaRyoSyo																	
                ,SUM(CAST(SyaRyoTes AS bigint)) AS SumSyaRyoTes																	
        FROM TKD_Haisha																	
        WHERE SiyoKbn = 1																	
        GROUP BY UkeNo																	
                ,UnkRen																	
                ,SyaSyuRen																	
        ) AS JT_Haisha ON TKD_Unkobi.UkeNo = JT_Haisha.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_Haisha.UnkRen																	
        AND JT_YykSyuSumUnc.SyaSyuRen = JT_Haisha.SyaSyuRen																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'ZEIKBN'																	
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'ZEIKBN'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_ZeiKbn ON JT_Yyksho.ZeiKbn = CONVERT(TINYINT, JM_ZeiKbn.CodeKbn)																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,SUM(YoushaKin) AS YoushaKin																	
        FROM (																	
                SELECT UkeNo																	
                        ,UnkRen																	
                        ,ISNULL(TKD_Yousha.SyaRyoUnc, 0) + ISNULL(TKD_Yousha.SyaRyoSyo, 0) AS YoushaKin																	
                FROM TKD_Yousha																	
                WHERE SiyoKbn = 1																	
                ) AS Main																	
        GROUP BY UkeNo																	
                ,UnkRen																	
        ) AS JT_YoushaSumKin ON TKD_Unkobi.UkeNo = JT_YoushaSumKin.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_YoushaSumKin.UnkRen																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,SUM(UriGakKin + SyaRyoSyo) AS UnsoJippiFut																	
                ,SUM(SyaRyoTes) AS UnsoJippiFutTes																	
        FROM TKD_FutTum																	
        INNER JOIN VPM_Futai ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq																	
                AND VPM_Futai.SiyoKbn = 1																	
        WHERE TKD_FutTum.SiyoKbn = 1																	
                AND TKD_FutTum.SeisanKbn = 1																	
        GROUP BY UkeNo																	
                ,UnkRen																	
        ) AS JT_FutTum ON TKD_Unkobi.UkeNo = JT_FutTum.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_FutTum.UnkRen																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,SUM(UriGakKin + SyaRyoSyo) AS GuideRyo																	
                ,SUM(SyaRyoTes) AS GuideTes																	
                ,MAX(FutaiNm) AS GuideFutaiNm																	
        FROM TKD_FutTum																	
        INNER JOIN VPM_Futai ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq																	
                AND VPM_Futai.SiyoKbn = 1																	
        WHERE TKD_FutTum.SiyoKbn = 1																	
                AND TKD_FutTum.SeisanKbn = 1																	
                AND VPM_Futai.FutGuiKbn = 5																	
        GROUP BY UkeNo																	
                ,UnkRen																	
        ) AS JT_FutTum_Guide ON TKD_Unkobi.UkeNo = JT_FutTum_Guide.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_FutTum_Guide.UnkRen																	
LEFT JOIN (																	
        SELECT UkeNo																	
                ,UnkRen																	
                ,Sum(JisaIPKm + JisaKSKm + KisoIPkm + KisoKOKm) AS SouTotalKm																	
                ,SUM(JisaIPKm + JisaKSKm) AS JituKm																	
        FROM TKD_Koteik																	
        WHERE TKD_Koteik.SiyoKbn = 1																	
        GROUP BY UkeNo																	
                ,UnkRen																	
        ) AS JT_Koteik ON TKD_Unkobi.UkeNo = JT_Koteik.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_Koteik.UnkRen																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'UKEJYKBNCD'																	
                AND SiyoKbn = 1							
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'UKEJYKBNCD'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_UkeJyKbnCd ON TKD_Unkobi.UkeJyKbnCd = CONVERT(TINYINT, JM_UkeJyKbnCd.CodeKbn)																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'OTHJINKBN'																	
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'OTHJINKBN'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_OthJinKbn1 ON TKD_Unkobi.OthJinKbn1 = CONVERT(TINYINT, JM_OthJinKbn1.CodeKbn)																	
LEFT JOIN (																	
        SELECT CodeKbn																	
                ,CodeKbnNm																	
                ,RyakuNm																	
        FROM VPM_CodeKb																	
        WHERE CodeSyu = 'OTHJINKBN'																	
                AND SiyoKbn = 1						
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'OTHJINKBN'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=		@TenantCdSeq)
        ) AS JM_OthJinKbn2 ON TKD_Unkobi.OthJinKbn2 = CONVERT(TINYINT, JM_OthJinKbn2.CodeKbn)																	
LEFT JOIN (																	
        SELECT TKD_Haisha.UkeNo																	
                ,TKD_Haisha.UnkRen																	
                ,SUBSTRING(MIN(SyuKoYmd + SyuKoTime), 9, 4) AS Min_SyuKoTime																	
                ,SUBSTRING(MAX(KikYmd + KikTime), 9, 4) AS Max_KikTime																	
                ,SUBSTRING(MIN(SyuKoYmd + SyuKoTime), 1, 8) AS Min_SyuKoYmd																	
                ,SUBSTRING(MAX(KikYmd + KikTime), 1, 8) AS Max_KikYmd																															
        FROM TKD_Haisha																																
        WHERE SiyoKbn = 1																	
        GROUP BY TKD_Haisha.UkeNo																	
                ,TKD_Haisha.UnkRen																	
        ) AS Unk_Haisha ON TKD_Unkobi.UkeNo = Unk_Haisha.UkeNo																	
        AND TKD_Unkobi.UnkRen = Unk_Haisha.UnkRen																	
LEFT JOIN VPM_Eigyos AS JM_UkeEigyos ON JT_Yyksho.UkeEigCdSeq = JM_UkeEigyos.EigyoCdSeq																	
        AND JM_UkeEigyos.SiyoKbn = 1																	
LEFT JOIN VPM_Compny AS JM_Compny ON JM_Compny.CompanyCdSeq = JM_UkeEigyos.CompanyCdSeq																	
LEFT JOIN VPM_Syain AS JM_EigTan ON JT_Yyksho.EigTanCdSeq = JM_EigTan.SyainCdSeq																	
LEFT JOIN VPM_Syain AS JM_InTan ON JT_Yyksho.InTanCdSeq = JM_InTan.SyainCdSeq																	
LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq																	
        AND JT_Yyksho.SiyoKbn = 1																	
LEFT JOIN (																	
        SELECT eTKD_YykReport.UkeNo																	
                ,eTKD_YykReport.UnkRen																	
                ,MAX(AllSokoTime) AS AllSokoTime																	
                ,MAX(CheckTime) AS CheckTime																	
                ,MAX(AdjustTime) AS AdjustTime																	
                ,MAX(ShinSoTime) AS ShinSoTime																	
                ,MAX(AllSokoKm) AS AllSokoKm																	
                ,MAX(JiSaTime) AS JiSaTime																	
                ,MAX(JiSaKm) AS JiSaKm																	
                ,MAX(ChangeFlg) AS ChangeFlg																	
                ,MAX(ChangeKoskTime) AS ChangeKoskTime																	
                ,MAX(ChangeShinTime) AS ChangeShinTime																	
                ,MAX(ChangeSokoKm) AS ChangeSokoKm																	
                ,MAX(SpecialFlg) AS SpecialFlg																	
                ,MAX(TKD_Unkobi.WaribikiKbn) AS WaribikiKbn																	
        FROM TKD_Unkobi																	
        LEFT JOIN TKD_YykSyu AS eTKD_YykSyu ON TKD_Unkobi.UkeNo = eTKD_YykSyu.UkeNo																	
                AND TKD_Unkobi.UnkRen = eTKD_YykSyu.UnkRen																	
        LEFT JOIN TKD_YykReport AS eTKD_YykReport ON eTKD_YykSyu.UkeNo = eTKD_YykReport.UkeNo																	
                AND eTKD_YykSyu.UnkRen = eTKD_YykReport.UnkRen																	
                AND eTKD_YykSyu.SyaSyuRen = eTKD_YykReport.SyaSyuRen																	
        WHERE eTKD_YykSyu.SiyoKbn = 1																	
        GROUP BY eTKD_YykReport.UkeNo																	
                ,eTKD_YykReport.UnkRen																	
        ) AS eTKD_YykReport ON TKD_Unkobi.UkeNo = eTKD_YykReport.UkeNo																	
        AND TKD_Unkobi.UnkRen = eTKD_YykReport.UnkRen																	
LEFT JOIN (																	
        SELECT TKD_YykSyu.UkeNo																	
                ,TKD_YykSyu.UnkRen																	
                ,TKD_YykSyu.SyaSyuRen																	
                ,(																	
                        CASE 																	
                                WHEN VPM_CodeKb.CodeKbnNm = '大型'																	
                                        THEN TKD_YykSyu.SyaSyuDai																	
                                ELSE 0																	
                                END																	
                        ) AS SumOogata																	
                ,(																	
                        CASE 																	
                                WHEN VPM_CodeKb.CodeKbnNm = '中型'																	
                                        THEN TKD_YykSyu.SyaSyuDai																	
                                ELSE 0																	
                                END																	
                        ) AS SumChugata																	
                ,(																	
                        CASE 																	
                                WHEN VPM_CodeKb.CodeKbnNm = '小型'																	
                                        THEN TKD_YykSyu.SyaSyuDai																	
                                ELSE 0																	
                                END																	
                        ) AS SumKogata																	
        FROM TKD_YykSyu																	
        INNER JOIN VPM_CodeKb ON TKD_YykSyu.KataKbn = VPM_CodeKb.CodeKbn																	
                AND VPM_CodeKb.CodeSyu = 'KATAKBN'																	
                AND VPM_CodeKb.SiyoKbn = 1				
                AND VPM_CodeKb.TenantCdSeq = (																			
                        SELECT CASE 																			
                                        WHEN COUNT(*) = 0																			
                                                THEN 0																			
                                        ELSE @TenantCdSeq																			
                                        END AS TenantCdSeq																			
                        FROM VPM_CodeKb																			
                        WHERE VPM_CodeKb.CodeSyu = 'KATAKBN'																			
                                AND VPM_CodeKb.SiyoKbn = 1																			
				AND VPM_CodeKb.TenantCdSeq	=	@TenantCdSeq)
        --WHERE TKD_YykSyu.UkeNo = FORMAT(@TenantCdSeq, '00000') + @UkeNo																	
        ) AS JT_YykSyuSumKata ON TKD_Unkobi.UkeNo = JT_YykSyuSumKata.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_YykSyuSumKata.UnkRen																	
        AND JT_YykSyuSumUnc.SyaSyuRen = JT_YykSyuSumKata.SyaSyuRen																	
LEFT JOIN (																	
        SELECT TKD_YykSyu.UkeNo																	
                ,TKD_YykSyu.UnkRen																	
                ,TKD_YykSyu.SyaSyuRen																	
                ,TKD_YykSyu.KataKbn																	
                ,TKD_YykSyu.SyaSyuDai																	
                ,TKD_BookingMaxMinFareFeeCalc.WaribikiKbn																	
                ,(																	
                        CASE 																	
                                WHEN TKD_BookingMaxMinFareFeeCalc.WaribikiKbn = 1																	
                                        THEN 1																	
                                ELSE 0																	
                                END																	
                        ) AS BusShinSyowari																	
                ,(																	
                        CASE 																	
                                WHEN TKD_BookingMaxMinFareFeeCalc.WaribikiKbn = 2																	
                                        THEN 1																	
                                ELSE 0																	
                                END																	
                        ) AS BusGakuwari																	
                ,(CAST(TKD_BookingMaxMinFareFeeCalc.FareMaxAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFareMaxAmount																	
                ,(CAST(TKD_BookingMaxMinFareFeeCalc.FareMinAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFareMinAmount																	
                ,(CAST(TKD_BookingMaxMinFareFeeCalc.FeeMaxAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFeeMaxAmount																	
                ,(CAST(TKD_BookingMaxMinFareFeeCalc.FeeMinAmount AS bigint) * TKD_YykSyu.SyaSyuDai) AS SumFeeMinAmount	
				,TKD_BookingMaxMinFareFeeCalc.RunningKmCalc AS TotalKmRunning
				,TKD_BookingMaxMinFareFeeCalc.ServiceKmSum AS JituKmRunning
				,CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.RestraintTimeCalc, 1, 2)) AS TotalHoursRunning
				,CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.RestraintTimeCalc, 3, 2)) AS TotalMinutesRunning
				,CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.ServiceTimeSum, 1, 2)) AS JituHoursRunning
				,CONVERT(int, SUBSTRING(TKD_BookingMaxMinFareFeeCalc.ServiceTimeSum, 3, 2)) AS JituMinutesRunning
                ,TKD_BookingMaxMinFareFeeCalc.AnnualContractFlag																	
                ,(																	
                        CASE 																	
                                WHEN TKD_BookingMaxMinFareFeeCalc.MidnightEarlyMorningTimeSum <> '0000'																	
                                        THEN 1																	
                                ELSE 0																	
                                END																	
                        ) AS MidnightEarlyMorningTimeSum																	
                ,TKD_BookingMaxMinFareFeeCalc.SpecialFlg																	
                ,TKD_BookingMaxMinFareFeeCalcMeisai.ChangeDriverFeeFlag																	
        FROM TKD_YykSyu																	
        LEFT JOIN TKD_BookingMaxMinFareFeeCalc ON TKD_YykSyu.UkeNo = TKD_BookingMaxMinFareFeeCalc.UkeNo																	
                AND TKD_YykSyu.UnkRen = TKD_BookingMaxMinFareFeeCalc.UnkRen																	
                AND TKD_YykSyu.SyaSyuRen = TKD_BookingMaxMinFareFeeCalc.SyaSyuRen																	
        LEFT JOIN TKD_BookingMaxMinFareFeeCalcMeisai ON TKD_BookingMaxMinFareFeeCalc.UkeNo = TKD_BookingMaxMinFareFeeCalcMeisai.UkeNo																	
                AND TKD_BookingMaxMinFareFeeCalc.UnkRen = TKD_BookingMaxMinFareFeeCalcMeisai.UnkRen																	
                AND TKD_BookingMaxMinFareFeeCalc.SyaSyuRen = TKD_BookingMaxMinFareFeeCalcMeisai.SyaSyuRen																	
        WHERE SiyoKbn = 1																	
        ) AS JT_YykSyuSumBooking ON TKD_Unkobi.UkeNo = JT_YykSyuSumBooking.UkeNo																	
        AND TKD_Unkobi.UnkRen = JT_YykSyuSumBooking.UnkRen																	
        AND JT_YykSyuSumUnc.SyaSyuRen = JT_YykSyuSumBooking.SyaSyuRen																	

WHERE JT_Yyksho.YoyaSyu = 1							
	AND TKD_Unkobi.SiyoKbn = 1 						
	-- IF 日付 = 1-配車日付		||-- IF 日付 = 2-到着日付	|| -- IF 日付 = 3-受付日付		
	AND (@StartDispatchDate IS NULL OR @StartDispatchDate = '' OR TKD_Unkobi.HaiSYmd >= @StartDispatchDate)                 -- 配車日　開始
	AND (@EndDispatchDate IS NULL OR @EndDispatchDate = '' OR TKD_Unkobi.HaiSYmd <= @EndDispatchDate)                       -- 配車日　終了
	AND (@StartArrivalDate IS NULL OR @StartArrivalDate = '' OR TKD_Unkobi.TouYmd >= @StartArrivalDate)                     -- 到着日　開始
	AND (@EndArrivalDate IS NULL OR @EndArrivalDate = '' OR TKD_Unkobi.TouYmd <= @EndArrivalDate)                           -- 到着日　終了
	AND (@StartReservationDate IS NULL OR @StartReservationDate = '' OR UkeYmd >= @StartReservationDate)                    -- 予約日　開始
	AND (@EndReservationDate IS NULL OR @EndReservationDate = '' OR UkeYmd<= @EndReservationDate)                           -- 予約日　終了
	-- 受付営業所：						
	AND (@UkeEigCd IS NULL OR @UkeEigCd = '' OR JM_UkeEigyos.EigyoCd = @UkeEigCd)
	-- 営業担当者：						
	AND (@EigSyainCd IS NULL OR @EigSyainCd = '' OR JM_EigTan.SyainCd = @EigSyainCd)					
	-- 入力担当者：						
	AND (@InpSyainCd IS NULL OR @InpSyainCd = '' OR JM_InTan.SyainCd = @InpSyainCd)		
	-- 受付番号：						
	AND (@UkeNo IS NULL OR @UkeNo = '' OR TKD_Unkobi.UkeNo IN (select value from string_split(@UkeNo, ',')))
	-- 予約区分：						
	AND (@YoyaKbnList IS NULL OR @YoyaKbnList = '' OR JM_YoyKbn.YoyaKbn IN (select value from string_split(@YoyaKbnList, ',')))	
	-- 得意先：						
	AND (@TokuiCd IS NULL OR @TokuiCd = '' OR JM_Tokisk.TokuiCd = @TokuiCd)
	AND (@SitenCd IS NULL OR @SitenCd = '' OR JM_TokiSt.SitenCd = @SitenCd)
	-- IF 出力選択 <> 0  (0:すべて 1:未出力のみ)						
	AND	(@OutSelect = 0 OR @OutSelect = '' OR ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem,0,9),'') = '')
	-- IF 年間契約出力フラグ <> 0 (0:出力する 1:出力しない)						
	AND	(@NenKeiyakuOutFlg = 0 OR @NenKeiyakuOutFlg = '' OR ISNULL(SUBSTRING(JT_UnkobiExp.ExpItem,78,1),0) = 0)		
	-- 運行日テーブルの連携						
	AND (@UnkRen IS NULL OR @UnkRen = '' OR TKD_Unkobi.UnkRen = @UnkRen)

ORDER BY TKD_Unkobi.UkeNo
SET	@ROWCOUNT	=	@@ROWCOUNT
END