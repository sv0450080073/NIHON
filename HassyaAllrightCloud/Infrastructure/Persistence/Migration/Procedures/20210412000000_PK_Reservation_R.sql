USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_Reservation_R]    Script Date: 10/20/2020 10:20:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_Reservation_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Reservation Data
-- Date			:   2020/10/20
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].PK_Reservation_R
    (
     -- Parameter
	       @TenantCdSeq           VARCHAR(8)
		,  @UkeNo                 VARCHAR(15)
	 
        -- Output
	    ,  @ROWCOUNT	          INTEGER OUTPUT	   -- 処理件数
    )
AS 
    -- Processing
	BEGIN
		SELECT TKD_Yyksho.*
                ,ISNULL(eVPM_CodeKb01.CodeKbnNm, ' ') AS YoyaSyu_CodeKbnNm -- 予約種別名
                ,ISNULL(eVPM_CodeKb01.RyakuNm, ' ') AS YoyaSyu_RyakuNm -- 予約種別略名
                ,ISNULL(eVPM_YoyKbn02.YoyaKbn, ' ') AS YoyaKbnSeq_YoyaKbn -- 予約区分
                ,ISNULL(eVPM_YoyKbn02.YoyaKbnNm, ' ') AS YoyaKbnSeq_YoyaKbnNm -- 予約区分名
                --,ISNULL(eVPM_Katour03.KasTourCd, ' ') AS KasTourCdSeq_KasTourCd -- 貸切ツアーコード
                --,ISNULL(eVPM_Katour03.KasTourNm, ' ') AS KasTourCdSeq_KasTourNm -- 貸切ツアー名
                --,ISNULL(eVPM_Katour03.RyakuNm, ' ') AS KasTourCdSeq_RyakuNm -- 貸切ツアー略名
                ,ISNULL(eVPM_Eigyos04.EigyoCd, 0) AS UkeEigCdSeq_EigyoCd -- 受付営業所コード
                ,ISNULL(eVPM_Eigyos04.EigyoNm, ' ') AS UkeEigCdSeq_EigyoNm -- 受付営業所名
                ,ISNULL(eVPM_Eigyos04.RyakuNm, ' ') AS UkeEigCdSeq_RyakuNm -- 受付営業所略名
                ,ISNULL(eVPM_Compny05.CompanyCd, 0) AS UkeCompanyCdSeq_CompanyCd -- 受付営業所会社コード
                ,ISNULL(eVPM_Compny05.CompanyNm, ' ') AS UkeCompanyCdSeq_CompanyNm -- 受付営業所会社名
                ,ISNULL(eVPM_Compny05.RyakuNm, ' ') AS UkeCompanyCdSeq_RyakuNm -- 受付営業所会社略名
                ,ISNULL(eVPM_Eigyos06.EigyoCd, 0) AS SeiEigCdSeq_EigyoCd -- 請求営業所コード
                ,ISNULL(eVPM_Eigyos06.EigyoNm, ' ') AS SeiEigCdSeq_EigyoNm -- 請求営業所名
                ,ISNULL(eVPM_Eigyos06.RyakuNm, ' ') AS SeiEigCdSeq_RyakuNm -- 請求営業所略名
                ,ISNULL(eVPM_Compny07.CompanyCd, 0) AS SeiCompanyCdSeq_CompanyCd -- 請求営業所会社コード
                ,ISNULL(eVPM_Compny07.CompanyNm, ' ') AS SeiCompanyCdSeq_CompanyNm -- 請求営業所会社名
                ,ISNULL(eVPM_Compny07.RyakuNm, ' ') AS SeiCompanyCdSeq_RyakuNm -- 請求営業所会社略名
                ,ISNULL(eVPM_Eigyos08.EigyoCd, 0) AS IraEigCdSeq_EigyoCd -- 依頼営業所コード
                ,ISNULL(eVPM_Eigyos08.EigyoNm, ' ') AS IraEigCdSeq_EigyoNm -- 依頼営業所名
                ,ISNULL(eVPM_Eigyos08.RyakuNm, ' ') AS IraEigCdSeq_RyakuNm -- 依頼営業所略名
                ,ISNULL(eVPM_Compny09.CompanyCd, 0) AS IraCompanyCdSeq_CompanyCd -- 依頼営業所会社コード
                ,ISNULL(eVPM_Compny09.CompanyNm, ' ') AS IraCompanyCdSeq_CompanyNm -- 依頼営業所会社名
                ,ISNULL(eVPM_Compny09.RyakuNm, ' ') AS IraCompanyCdSeq_RyakuNm -- 依頼営業所会社略名
                ,ISNULL(eVPM_Syain10.SyainCd, ' ') AS EigTanCdSeq_SyainCd -- 営業担当者コード
                ,ISNULL(eVPM_Syain10.SyainNm, ' ') AS EigTanCdSeq_SyainNm -- 営業担当者名
                ,ISNULL(eVPM_Syain11.SyainCd, ' ') AS InTanCdSeq_SyainCd -- 入力担当者コード
                ,ISNULL(eVPM_Syain11.SyainNm, ' ') AS InTanCdSeq_SyainNm -- 入力担当者名
                ,ISNULL(eVPM_Tokisk12.TokuiCd, 0) AS TokSeq_TokuiCd -- 得意先コード
                ,ISNULL(eVPM_Tokisk12.Kana, ' ') AS TokSeq_Kana -- 得意先かな
                ,ISNULL(eVPM_Tokisk12.TokuiNm, ' ') AS TokSeq_TokuiNm -- 得意先名
                ,ISNULL(eVPM_Tokisk12.RyakuNm, ' ') AS TokSeq_RyakuNm -- 得意先略名
                ,ISNULL(eVPM_Tokisk12.GyosyaCdSeq, 0) AS TokSeq_GyosyaCdSeq -- 得意先業者コードＳＥＱ
                ,ISNULL(eVPM_Gyosya13.GyosyaCd, 0) AS TokGyosyaCdSeq_GyosyaCd -- 得意先業者コード
                ,ISNULL(eVPM_Gyosya13.GyosyaNm, ' ') AS TokGyosyaCdSeq_GyosyaNm -- 得意先業者名
                ,ISNULL(eVPM_Gyosya13.GyosyaKbn, 0) AS TokGyosyaCdSeq_GyosyaKbn -- 得意先業者区分
                ,ISNULL(eVPM_CodeKb14.CodeKbnNm, ' ') AS TokGyosyaKbn_CodeKbnNm -- 得意先業者区分名
                ,ISNULL(eVPM_CodeKb14.RyakuNm, ' ') AS TokGyosyaKbn_RyakuNm -- 得意先業者区分略名
                ,ISNULL(eVPM_TokiSt15.SitenCd, 0) AS SitenCdSeq_SitenCd -- 支店コード
                ,ISNULL(eVPM_TokiSt15.Kana, ' ') AS SitenCdSeq_Kana -- 支店かな
                ,ISNULL(eVPM_TokiSt15.SitenNm, ' ') AS SitenCdSeq_SitenNm -- 支店名
                ,ISNULL(eVPM_TokiSt15.RyakuNm, ' ') AS SitenCdSeq_RyakuNm -- 支店略名
                ,ISNULL(eVPM_TokiSt15.SeiCdSeq, 0) AS SitenCdSeq_SeiCdSeq -- 請求先コードＳＥＱ
                ,ISNULL(eVPM_TokiSt15.SeiSitenCdSeq, 0) AS SitenCdSeq_SeiSitenCdSeq -- 請求先支店コードＳＥＱ
                ,ISNULL(eVPM_TokiSt15.TesuRituFut, 0) AS SitenCdSeq_TesuRituFut -- 手数料率（付帯）
                ,ISNULL(eVPM_TokiSt15.TesuRituGui, 0) AS SitenCdSeq_TesuRituGui -- 手数料率（ガイド料）
                ,ISNULL(eVPM_TokiSt15.TesKbnFut, 0) AS SitenCdSeq_TesKbnFut -- 手数料区分（付帯）
                ,ISNULL(eVPM_TokiSt15.TesKbnGui, 0) AS SitenCdSeq_TesKbnGui -- 手数料区分（ガイド料）
                ,ISNULL(eVPM_Tokisk16.TokuiCd, 0) AS SirCdSeq_TokuiCd -- 仕入先コード
                ,ISNULL(eVPM_Tokisk16.Kana, ' ') AS SirCdSeq_Kana -- 仕入先かな
                ,ISNULL(eVPM_Tokisk16.TokuiNm, ' ') AS SirCdSeq_TokuiNm -- 仕入先名
                ,ISNULL(eVPM_Tokisk16.RyakuNm, ' ') AS SirCdSeq_RyakuNm -- 仕入先略名
                ,ISNULL(eVPM_Tokisk16.GyosyaCdSeq, 0) AS SirCdSeq_GyosyaCdSeq -- 仕入先業者コードＳＥＱ
                ,ISNULL(eVPM_Gyosya17.GyosyaCd, 0) AS SirGyosyaCdSeq_GyosyaCd -- 仕入先業者コード
                ,ISNULL(eVPM_Gyosya17.GyosyaNm, ' ') AS SirGyosyaCdSeq_GyosyaNm -- 仕入先業者名
                ,ISNULL(eVPM_Gyosya17.GyosyaKbn, 0) AS SirGyosyaCdSeq_GyosyaKbn -- 仕入先業者区分
                ,ISNULL(eVPM_CodeKb18.CodeKbnNm, ' ') AS SirGyoSyaKbn_CodeKbnNm -- 仕入先業者区分名
                ,ISNULL(eVPM_CodeKb18.RyakuNm, ' ') AS SirGyoSyaKbn_RyakuNm -- 仕入先業者区分略名
                ,ISNULL(eVPM_TokiSt19.SitenCd, 0) AS SirSitenCdSeq_SitenCd -- 仕入先支店コード
                ,ISNULL(eVPM_TokiSt19.Kana, ' ') AS SirSitenCdSeq_Kana -- 仕入先支店かな
                ,ISNULL(eVPM_TokiSt19.SitenNm, ' ') AS SirSitenCdSeq_SitenNm -- 仕入先支店名
                ,ISNULL(eVPM_TokiSt19.RyakuNm, ' ') AS SirSitenCdSeq_RyakuNm -- 仕入先支店略名
                ,ISNULL(eVPM_CodeKb20.CodeKbnNm, ' ') AS ZeiKbn_CodeKbnNm -- 税区分名
                ,ISNULL(eVPM_CodeKb20.RyakuNm, ' ') AS ZeiKbn_RyakuNm -- 税区分略名
                ,ISNULL(eVPM_CodeKb21.CodeKbnNm, ' ') AS SeiKyuKbnSeq_CodeKbnNm -- 請求区分名
                ,ISNULL(eVPM_CodeKb21.RyakuNm, ' ') AS SeiKyuKbnSeq_RyakuNm -- 請求区分略名
                ,ISNULL(eVPM_CodeKb22.CodeKbnNm, ' ') AS CanZKbn_CodeKbnNm -- キャンセル料税区分名
                ,ISNULL(eVPM_CodeKb22.RyakuNm, ' ') AS CanZKbn_RyakuNm -- キャンセル料税区分略名
                ,ISNULL(eVPM_Syain23.SyainCd, ' ') AS CanTanSeq_SyainCd -- キャンセル担当者コード
                ,ISNULL(eVPM_Syain23.SyainNm, ' ') AS CanTanSeq_SyainNm -- キャンセル担当者名
                ,ISNULL(eVPM_Syain24.SyainCd, ' ') AS CanFuTanSeq_SyainCd -- キャンセル復活担当者コード
                ,ISNULL(eVPM_Syain24.SyainNm, ' ') AS CanFuTanSeq_SyainNm -- キャンセル復活担当者名
                ,ISNULL(eVPM_CodeKb25.CodeKbnNm, ' ') AS KSKbn_CodeKbnNm -- 仮車区分名
                ,ISNULL(eVPM_CodeKb25.RyakuNm, ' ') AS KSKbn_RyakuNm -- 仮車区分略名
                ,ISNULL(eVPM_CodeKb26.CodeKbnNm, ' ') AS KHinKbn_CodeKbnNm -- 仮配員区分名
                ,ISNULL(eVPM_CodeKb26.RyakuNm, ' ') AS KHinKbn_RyakuNm -- 仮配員区分略名
                ,ISNULL(eVPM_CodeKb27.CodeKbnNm, ' ') AS HaiSKbn_CodeKbnNm -- 配車区分名
                ,ISNULL(eVPM_CodeKb27.RyakuNm, ' ') AS HaiSKbn_RyakuNm -- 配車区分略名
                ,ISNULL(eVPM_CodeKb28.CodeKbnNm, ' ') AS HaiIKbn_CodeKbnNm -- 配員区分名
                ,ISNULL(eVPM_CodeKb28.RyakuNm, ' ') AS HaiIKbn_RyakuNm -- 配員区分略名
                ,ISNULL(eVPM_CodeKb29.CodeKbnNm, ' ') AS NippoKbn_CodeKbnNm -- 日報区分名
                ,ISNULL(eVPM_CodeKb29.RyakuNm, ' ') AS NippoKbn_RyakuNm -- 日報区分略名
                ,ISNULL(eVPM_CodeKb30.CodeKbnNm, ' ') AS YouKbn_CodeKbnNm -- 傭車区分名
                ,ISNULL(eVPM_CodeKb30.RyakuNm, ' ') AS YouKbn_RyakuNm -- 傭車区分略名
                ,ISNULL(eVPM_CodeKb31.CodeKbnNm, ' ') AS NyuKinKbn_CodeKbnNm -- 入金区分名
                ,ISNULL(eVPM_CodeKb31.RyakuNm, ' ') AS NyuKinKbn_RyakuNm -- 入金区分略名
                ,ISNULL(eVPM_CodeKb32.CodeKbnNm, ' ') AS NCouKbn_CodeKbnNm -- 入金クーポン区分名
                ,ISNULL(eVPM_CodeKb32.RyakuNm, ' ') AS NCouKbn_RyakuNm -- 入金クーポン区分略名
                ,ISNULL(eVPM_CodeKb33.CodeKbnNm, ' ') AS SihKbn_CodeKbnNm -- 支払区分名
                ,ISNULL(eVPM_CodeKb33.RyakuNm, ' ') AS SihKbn_RyakuNm -- 支払区分略名
                ,ISNULL(eVPM_CodeKb34.CodeKbnNm, ' ') AS SCouKbn_CodeKbnNm -- 支払クーポン区分名
                ,ISNULL(eVPM_CodeKb34.RyakuNm, ' ') AS SCouKbn_RyakuNm -- 支払クーポン区分略名
                ,ISNULL(eVPM_Syain35.SyainCd, ' ') AS UpdSyainCd_SyainCd -- 最終更新社員名
                ,ISNULL(eVPM_Syain35.SyainNm, ' ') AS UpdSyainCd_SyainNm -- 最終更新社員略名
        FROM TKD_Yyksho
        -- 予約書テーブルの予約種別より取得（YOYASYU）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_CodeKb01.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.YoyaSyu)
                AND eVPM_CodeKb01.SiyoKbn = 1
                AND eVPM_CodeKb01.CodeSyu = 'YOYASYU'
                AND eVPM_CodeKb01.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'YOYASYU'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの予約区分ＳＥＱより取得（予約区分マスタ）
        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn02 ON TKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn02.YoyaKbnSeq
        -- 予約書テーブルの貸切ツアーコードＳＥＱより取得（貸切ツアーマスタ）
        --LEFT JOIN VPM_Katour AS eVPM_Katour03 ON TKD_Yyksho.TokuiSeq = eVPM_Katour03.TokuiSeq
        --        AND TKD_Yyksho.SitenCdSeq = eVPM_Katour03.SitenCdSeq
        --        AND TKD_Yyksho.KasTourCdSeq = eVPM_Katour03.KasTourCdSeq
        --        AND eVPM_Katour03.SiyoKbn = 1
        -- 予約書テーブルの受付営業所コードＳＥＱより取得（営業所マスタ）
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos04 ON TKD_Yyksho.UkeEigCdSeq = eVPM_Eigyos04.EigyoCdSeq
                AND eVPM_Eigyos04.SiyoKbn = 1
        -- 受付営業所（営業所マスタ）の会社コードＳＥＱより取得（会社マスタ）
        LEFT JOIN VPM_Compny AS eVPM_Compny05 ON eVPM_Eigyos04.CompanyCdSeq = eVPM_Compny05.CompanyCdSeq
                AND eVPM_Compny05.SiyoKbn = 1
                AND eVPM_Compny05.TenantCdSeq = @TenantCdSeq
        -- 予約書テーブルの請求営業所コードＳＥＱより取得（営業所マスタ）
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos06 ON TKD_Yyksho.SeiEigCdSeq = eVPM_Eigyos06.EigyoCdSeq
                AND eVPM_Eigyos06.SiyoKbn = 1
        -- 請求営業所（営業所マスタ）の会社コードＳＥＱより取得（会社マスタ）
        LEFT JOIN VPM_Compny AS eVPM_Compny07 ON eVPM_Eigyos06.CompanyCdSeq = eVPM_Compny07.CompanyCdSeq
                AND eVPM_Compny07.SiyoKbn = 1
                AND eVPM_Compny07.TenantCdSeq = @TenantCdSeq
        -- 予約書テーブルの依頼営業所コードＳＥＱより取得（営業所マスタ）
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos08 ON TKD_Yyksho.IraEigCdSeq = eVPM_Eigyos08.EigyoCdSeq
                AND eVPM_Eigyos08.SiyoKbn = 1
        -- 依頼営業所（営業所マスタ）の会社コードＳＥＱより取得（会社マスタ）
        LEFT JOIN VPM_Compny AS eVPM_Compny09 ON eVPM_Eigyos08.CompanyCdSeq = eVPM_Compny09.CompanyCdSeq
                AND eVPM_Compny09.SiyoKbn = 1
                AND eVPM_Compny09.TenantCdSeq = @TenantCdSeq
        -- 予約書テーブルの営業担当者コードＳＥＱより取得（社員マスタ）
        LEFT JOIN VPM_Syain AS eVPM_Syain10 ON TKD_Yyksho.EigTanCdSeq = eVPM_Syain10.SyainCdSeq
        -- 予約書テーブルの入力担当者コードＳＥＱより取得（社員マスタ）
        LEFT JOIN VPM_Syain AS eVPM_Syain11 ON TKD_Yyksho.InTanCdSeq = eVPM_Syain11.SyainCdSeq
        -- 予約書テーブルの得意先ＳＥＱより取得（受付年月日が使用開始／終了年月日の範囲内）（得意先マスタ）
        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk12 ON TKD_Yyksho.TokuiSeq = eVPM_Tokisk12.TokuiSeq
                AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk12.SiyoStaYmd AND eVPM_Tokisk12.SiyoEndYmd
                AND eVPM_Tokisk12.TenantCdSeq = @TenantCdSeq
        -- 得意先（得意先マスタ）の業者コードＳＥＱより取得（業者マスタ）
        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya13 ON eVPM_Tokisk12.GyosyaCdSeq = eVPM_Gyosya13.GyosyaCdSeq
                AND eVPM_Gyosya13.SiyoKbn = 1
        -- 得意先業者（業者マスタ）の業者区分より取得（GYOSYAKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb14 ON eVPM_CodeKb14.CodeSyu = 'GYOSYAKBN'
                AND CONVERT(VARCHAR(10), eVPM_Gyosya13.GyoSyaKbn) = eVPM_CodeKb14.CodeKbn
                AND eVPM_CodeKb14.SiyoKbn = 1
                AND eVPM_CodeKb14.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'GYOSYAKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの支店コードＳＥＱより取得（受付年月日が使用開始／終了年月日の範囲内）（得意先支店マスタ）
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt15 ON TKD_Yyksho.TokuiSeq = eVPM_TokiSt15.TokuiSeq
                AND TKD_Yyksho.SitenCdSeq = eVPM_TokiSt15.SitenCdSeq
                AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt15.SiyoStaYmd AND eVPM_TokiSt15.SiyoEndYmd
        -- 予約書テーブルの仕入先コードＳＥＱより取得（受付年月日が使用開始／終了年月日の範囲内）（得意先マスタ）
        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk16 ON TKD_Yyksho.SirCdSeq = eVPM_Tokisk16.TokuiSeq
                AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk16.SiyoStaYmd AND eVPM_Tokisk16.SiyoEndYmd
                AND eVPM_Tokisk16.TenantCdSeq = @TenantCdSeq
        -- 仕入先（得意先マスタ）の業者コードＳＥＱより取得（業者マスタ）
        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya17 ON eVPM_Tokisk16.GyosyaCdSeq = eVPM_Gyosya17.GyosyaCdSeq
                AND eVPM_Gyosya17.SiyoKbn = 1
        -- 仕入先業者（業者マスタ）の業者区分より取得（GYOSYAKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb18 ON eVPM_CodeKb18.CodeSyu = 'GYOSYAKBN'
                AND CONVERT(VARCHAR(10), eVPM_Gyosya17.GyoSyaKbn) = eVPM_CodeKb18.CodeKbn
                AND eVPM_CodeKb18.SiyoKbn = 1
                AND eVPM_CodeKb18.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'GYOSYAKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの仕入先支店コードＳＥＱより取得（受付年月日が使用開始／終了年月日の範囲内）（得意先支店マスタ）
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt19 ON TKD_Yyksho.SirCdSeq = eVPM_TokiSt19.TokuiSeq
                AND TKD_Yyksho.SirSitenCdSeq = eVPM_TokiSt19.SitenCdSeq
                AND TKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt19.SiyoStaYmd AND eVPM_TokiSt19.SiyoEndYmd
        -- 予約書テーブルの税区分より取得（ZEIKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb20 ON eVPM_CodeKb20.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.ZeiKbn)
                AND eVPM_CodeKb20.CodeSyu = 'ZEIKBN'
                AND eVPM_CodeKb20.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'ZEIKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの請求区分ＳＥＱより取得（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb21 ON TKD_Yyksho.SeiKyuKbnSeq = eVPM_CodeKb21.CodeKbnSeq
                AND eVPM_CodeKb21.SiyoKbn = 1
        -- 予約書テーブルのキャンセル料税区分より取得（CANZKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb22 ON eVPM_CodeKb22.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.CanZKbn)
                AND eVPM_CodeKb22.SiyoKbn = 1
                AND eVPM_CodeKb22.CodeSyu = 'CANZKBN'
                AND eVPM_CodeKb22.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'CANZKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルのキャンセル担当者コードＳＥＱより取得（社員マスタ）
        LEFT JOIN VPM_Syain AS eVPM_Syain23 ON TKD_Yyksho.CanTanSeq = eVPM_Syain23.SyainCdSeq
        -- 予約書テーブルのキャンセル復活担当者コードＳＥＱより取得（社員マスタ）
        LEFT JOIN VPM_Syain AS eVPM_Syain24 ON TKD_Yyksho.CanFuTanSeq = eVPM_Syain24.SyainCdSeq
        -- 予約書テーブルの仮車区分より取得（KSKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb25 ON eVPM_CodeKb25.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.KSKbn)
                AND eVPM_CodeKb25.SiyoKbn = 1
                AND eVPM_CodeKb25.CodeSyu = 'KSKBN'
                AND eVPM_CodeKb25.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'KSKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの仮配員区分より取得（KHINKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb26 ON eVPM_CodeKb26.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.KHinKbn)
                AND eVPM_CodeKb26.SiyoKbn = 1
                AND eVPM_CodeKb26.CodeSyu = 'KHINKBN'
                AND eVPM_CodeKb26.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'KHINKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの配車区分より取得（HAISKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb27 ON eVPM_CodeKb27.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.HaiSKbn)
                AND eVPM_CodeKb27.SiyoKbn = 1
                AND eVPM_CodeKb27.CodeSyu = 'HAISKBN'
                AND eVPM_CodeKb27.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'HAISKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの配員区分より取得（HAIIKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb28 ON eVPM_CodeKb28.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.HaiIKbn)
                AND eVPM_CodeKb28.SiyoKbn = 1
                AND eVPM_CodeKb28.CodeSyu = 'HAIIKBN'
                AND eVPM_CodeKb28.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'HAIIKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの日報区分より取得（NIPPOKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb29 ON eVPM_CodeKb29.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.NippoKbn)
                AND eVPM_CodeKb29.SiyoKbn = 1
                AND eVPM_CodeKb29.CodeSyu = 'NIPPOKBN'
                AND eVPM_CodeKb29.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'NIPPOKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの傭車区分より取得（YOUKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb30 ON eVPM_CodeKb30.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.YouKbn)
                AND eVPM_CodeKb30.SiyoKbn = 1
                AND eVPM_CodeKb30.CodeSyu = 'YOUKBN'
                AND eVPM_CodeKb30.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'YOUKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの入金区分より取得（NYUKINKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb31 ON eVPM_CodeKb31.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.NyuKinKbn)
                AND eVPM_CodeKb31.SiyoKbn = 1
                AND eVPM_CodeKb31.CodeSyu = 'NYUKINKBN'
                AND eVPM_CodeKb31.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'NYUKINKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの入金クーポン区分より取得（NCOUKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb32 ON eVPM_CodeKb32.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.NCouKbn)
                AND eVPM_CodeKb32.SiyoKbn = 1
                AND eVPM_CodeKb32.CodeSyu = 'NCOUKBN'
                AND eVPM_CodeKb32.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'NCOUKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの支払区分より取得（SIHKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb33 ON eVPM_CodeKb33.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.SihKbn)
                AND eVPM_CodeKb33.SiyoKbn = 1
                AND eVPM_CodeKb33.CodeSyu = 'SIHKBN'
                AND eVPM_CodeKb33.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'SIHKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの支払クーポン区分より取得（SCOUKBN）（コード区分マスタ）
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb34 ON eVPM_CodeKb34.CodeKbn = dbo.FP_RpZero(2, TKD_Yyksho.SCouKbn)
                AND eVPM_CodeKb34.SiyoKbn = 1
                AND eVPM_CodeKb34.CodeSyu = 'SCOUKBN'
                AND eVPM_CodeKb34.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'SCOUKBN'
                                AND KanriKbn <> 1
                        )
        -- 予約書テーブルの最終更新社員コードＳＥＱより取得（社員マスタ）
        LEFT JOIN VPM_Syain AS eVPM_Syain35 ON TKD_Yyksho.UpdSyainCd = eVPM_Syain35.SyainCdSeq
        WHERE TKD_Yyksho.UkeNo = @UkeNo
                AND TKD_Yyksho.YoyaSyu = 1
                AND TKD_Yyksho.SiyoKbn = 1
                AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq
        ORDER BY TKD_Yyksho.UkeNo

    SET	@ROWCOUNT	=	@@ROWCOUNT
	END
RETURN																													
