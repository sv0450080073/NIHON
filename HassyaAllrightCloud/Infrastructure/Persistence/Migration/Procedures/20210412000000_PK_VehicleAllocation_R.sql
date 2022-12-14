USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_VehicleAllocation_R]    Script Date: 10/20/2020 10:21:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_VehicleAllocation_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Vehicle Allocation Data
-- Date			:   2020/10/20
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].PK_VehicleAllocation_R
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
		SELECT TKD_Haisha.*
            ,ISNULL(eTKD_Yyksho.KaknKais, 0) AS KaknKais -- 確認回数
            ,ISNULL(eTKD_Yyksho.KaktYmd, ' ') AS KaktYmd -- 確定年月日
            ,ISNULL(eTKD_Unkobi01.DanTaNm, ' ') AS DanTaNm -- 団体名
            ,ISNULL(eVPM_Tokisk01.TokuiCd, 0) AS YouCdSeq_TokuiCd -- 傭車先コード
            ,ISNULL(eVPM_Tokisk01.RyakuNm, ' ') AS YouCdSeq_RyakuNm -- 傭車先略名
            ,ISNULL(eVPM_TokiSt01.SitenCd, 0) AS YouSitCdSeq_SitenCd -- 傭車支店コード
            ,ISNULL(eVPM_TokiSt01.RyakuNm, ' ') AS YouSitCdSeq_RyakuNm -- 傭車先支店略名
            ,ISNULL(eVPM_Eigyos01.EigyoCd, 0) AS SyuEigCdSeq_EigyoCd -- 出庫営業所コード
            ,ISNULL(eVPM_Eigyos01.RyakuNm, ' ') AS SyuEigCdSeq_RyakuNm -- 出庫営業所略名
            ,ISNULL(eVPM_Eigyos02.EigyoCd, 0) AS KikSEigCdSeq_EigyoCd -- 帰庫営業所コード
            ,ISNULL(eVPM_Eigyos02.RyakuNm, ' ') AS KikSEigCdSeq_RyakuNm -- 帰庫営業所略名
            ,ISNULL(eVPM_Eigyos03.EigyoCd, 0) AS HaiSSryCdSeq_EigyoCd -- 配車営業所コード
            ,ISNULL(eVPM_Eigyos03.RyakuNm, ' ') AS HaiSSryCdSeq_RyakuNm -- 配車営業所略名
            ,ISNULL(eVPM_Eigyos04.EigyoCd, 0) AS KSSyaRSeq_EigyoCd -- 仮車営業所コード
            ,ISNULL(eVPM_Eigyos04.RyakuNm, ' ') AS KSSyaRSeq_RyakuNm -- 仮車営業所略名
            ,ISNULL(eVPM_SyaRyo01.SyaRyoCd, 0) AS HaiSSyaRCdSeq_SyaRyoCd -- 配車車輌コード
            ,ISNULL(eVPM_SyaRyo01.SyaRyoNm, ' ') AS HaiSSyaRCdSeq_SyaRyoNm -- 配車車号
            ,ISNULL(eVPM_SyaRyo01.SyainCdSeq, 0) AS HaiSSyaRCdSeq_SyainCdSeq -- 配車車輌_社員コードＳＥＱ
            ,ISNULL(eVPM_SyaRyo02.SyaRyoCd, 0) AS KSSyaRCdSeq_SyaRyoCd -- 仮車車輌コード
            ,ISNULL(eVPM_SyaRyo02.KariSyaRyoNm, ' ') AS KSSyaRCdSeq_KariSyaRyoNm -- 仮車号
            ,ISNULL(eVPM_SyaRyo02.SyainCdSeq, 0) AS KSSyaRCdSeq_SyainCdSeq -- 仮車車輌_社員コードＳＥＱ
            ,ISNULL(eVPM_SyaSyu01.SyaSyuCd, 0) AS HaiSSyaSCdSeq_SyaSyuCd -- 配車車種コード
            ,ISNULL(eVPM_SyaSyu01.SyaSyuNm, ' ') AS HaiSSyaSCdSeq_SyaSyuNm -- 配車車種名
            ,ISNULL(eVPM_SyaSyu02.SyaSyuCd, 0) AS KSSyaSCdSeq_SyaSyuCd -- 仮車車種コード
            ,ISNULL(eVPM_SyaSyu02.SyaSyuNm, ' ') AS KSSyaSCdSeq_SyaSyuNm -- 仮車車種名
            ,ISNULL(eVPM_Haichi01.BunruiCdSeq, 0) AS HaiCh_BunruiCdSeq -- 配車地分類コードＳＥＱ
            ,ISNULL(eVPM_Haichi01.HaiSCd, 0) AS HaiCh_HaiSCd -- 配車地コード
            ,ISNULL(eVPM_Koutu01.BunruiCdSeq, 0) AS HaiKo_BunruiCdSeq -- 配車地交通機関分類コードＳＥＱ
            ,ISNULL(eVPM_Koutu01.KoukCd, 0) AS HaiKo_KouKCd -- 配車地交通機関コード
            ,ISNULL(eVPM_CodeKb01.CodeKbn, ' ') AS HaiKo_BunruiCd -- 配車地交通機関分類コード
            ,ISNULL(eVPM_CodeKb01.RyakuNm, ' ') AS HaiKo_BunruiRyakuNm -- 配車地交通機関分類略名
            ,ISNULL(eVPM_Bin01.BinCd, 0) AS HaiB_BinCd -- 配車地便コード
            ,ISNULL(eVPM_Haichi02.BunruiCdSeq, 0) AS TouCh_BunruiCdSeq -- 到着地分類コードＳＥＱ
            ,ISNULL(eVPM_Haichi02.HaiSCd, 0) AS TouCh_HaiSCd -- 到着地コード
            ,ISNULL(eVPM_Koutu02.BunruiCdSeq, 0) AS TouKo_BunruiCdSeq -- 到着地交通機関分類コードＳＥＱ
            ,ISNULL(eVPM_Koutu02.KouKCd, 0) AS TouKo_KouKCd -- 到着地交通機関コード
            ,ISNULL(eVPM_CodeKb02.CodeKbn, ' ') AS TouKo_BunruiCd -- 到着地交通機関分類コード
            ,ISNULL(eVPM_CodeKb02.RyakuNm, ' ') AS TouKo_BunruiRyakuNm -- 到着地交通機関分類略名
            ,ISNULL(eVPM_Bin02.BinCd, 0) AS TouB_BinCd -- 到着地便コード
            ,ISNULL(eVPM_CodeKb03.RyakuNm, ' ') AS OthJinKbn1_RyakuNm -- その他人員区分１略名
            ,ISNULL(eVPM_CodeKb04.RyakuNm, ' ') AS OthJinKbn2_RyakuNm -- その他人員区分２略名
            ,ISNULL(eVPM_CodeKb05.RyakuNm, ' ') AS YouKataKbn_RyakuNm -- 傭車型区分略名
            ,ISNULL(eVPM_CodeKb06.RyakuNm, ' ') AS UkeJyKbn_RyakuNm -- 受付条件区分略名
            ,ISNULL(eVPM_Tokisk02.RyakuNm, ' ') AS TokuiSeq_RyakuNm -- 得意先略名
            ,ISNULL(eVPM_TokiSt02.RyakuNm, ' ') AS SitenCdSeq_RyakuNm -- 得意先支店略名
            ,ISNULL(eTKD_Unkobi01.HaiSYmd, ' ') AS UHaiSYmd -- 運行配車日
            ,ISNULL(eTKD_Unkobi01.HaiSTime, ' ') AS UHaiSTime -- 運行配車時間
            ,ISNULL(eTKD_Unkobi01.TouYmd, ' ') AS UTouYmd -- 運行到着日
            ,ISNULL(eTKD_Unkobi01.TouChTime, ' ') AS UTouChTime -- 運行到着時間
            ,ISNULL(eTKD_Unkobi01.ZenHaFlg, ' ') AS UZenHaFlg -- 運行前泊フラグ
            ,ISNULL(eTKD_Unkobi01.KhakFlg, ' ') AS UKhakFlg -- 運行後泊フラグ
            ,ISNULL(eVPM_Basyo01.BasyoKenCdSeq, 0) AS Ik_BasyoKenCdSeq -- 行き先県コードＳＥＱ
            ,ISNULL(eVPM_Basyo01.BasyoMapCd, 0) AS Ik_BasyoMapCd -- 行き先マップコード
            ,ISNULL(eVPM_Gyosya01.GyosyaCd, 0) AS YouGyosyaCdSeq_GyosyaCd -- 傭車先業者コード
            ,ISNULL(eVPM_Gyosya01.GyosyaNm, ' ') AS YouGyosyaCdSeq_GyosyaNm -- 傭車先業者名
            ,ISNULL(eTKD_Yousha01.UkeNo, ' ') AS You_UkeNo -- 傭車テーブル．受付番号
            ,ISNULL(eTKD_Yousha01.UnkRen, 0) AS You_UnkRen -- 傭車テーブル．運行日連番
            ,ISNULL(eTKD_Yousha01.YouTblSeq, 0) AS You_YouTblSeq -- 傭車テーブル．傭車テーブルＳＥＱ
            ,ISNULL(eTKD_Yousha01.HenKai, 0) AS You_HenKai -- 傭車テーブル．変更回数
            ,ISNULL(eTKD_Yousha01.YouCdSeq, 0) AS You_YouCdSeq -- 傭車テーブル．傭車先コードＳＥＱ
            ,ISNULL(eTKD_Yousha01.YouSitCdSeq, 0) AS You_YouSitCdSeq -- 傭車テーブル．傭車先支店コードＳＥＱ
            ,ISNULL(eTKD_Yousha01.HasYmd, ' ') AS You_HasYmd -- 傭車テーブル．発生年月日
            ,ISNULL(eTKD_Yousha01.SihYotYmd, ' ') AS You_SihYotYmd -- 傭車テーブル．支払予定年月日
            ,ISNULL(eTKD_Yousha01.SihYm, ' ') AS You_SihYm -- 傭車テーブル．支払年月
            ,ISNULL(eTKD_Yousha01.SyaRyoUnc, 0) AS You_SyaRyoUnc -- 傭車テーブル．運賃
            ,ISNULL(eTKD_Yousha01.ZeiKbn, 0) AS You_ZeiKbn -- 傭車テーブル．税区分
            ,ISNULL(eTKD_Yousha01.Zeiritsu, 0) AS You_Zeiritsu -- 傭車テーブル．消費税率
            ,ISNULL(eTKD_Yousha01.SyaRyoSyo, 0) AS You_SyaRyoSyo -- 傭車テーブル．消費税額
            ,ISNULL(eTKD_Yousha01.TesuRitu, 0) AS You_TesuRitu -- 傭車テーブル．手数料率
            ,ISNULL(eTKD_Yousha01.SyaRyoTes, 0) AS You_SyaRyoTes -- 傭車テーブル．手数料額
            ,ISNULL(eTKD_Yousha01.JitaFlg, 0) AS You_JitaFlg -- 傭車テーブル．自他社フラグ
            ,ISNULL(eTKD_Yousha01.CompanyCdSeq, 0) AS You_CompanyCdSeq -- 傭車テーブル．会社コードＳＥＱ
            ,ISNULL(eTKD_Yousha01.SihKbn, 0) AS You_SihKbn -- 傭車テーブル．支払区分
            ,ISNULL(eTKD_Yousha01.SiyoKbn, 0) AS You_SiyoKbn -- 傭車テーブル．使用区分
            ,ISNULL(eTKD_Yousha01.UpdYmd, ' ') AS You_UpdYmd -- 傭車テーブル．最終更新年月日
            ,ISNULL(eTKD_Yousha01.UpdTime, ' ') AS You_UpdTime -- 傭車テーブル．最終更新時間
            ,ISNULL(eTKD_Yousha01.UpdSyainCd, 0) AS You_UpdSyainCd -- 傭車テーブル．最終更新社員コードＳＥＱ
            ,ISNULL(eTKD_Yousha01.UpdPrgID, ' ') AS You_UpdPrgID -- 傭車テーブル．最終更新プログラムＩＤ
            ,ISNULL(eVPM_SyaSyu01.KataKbn, 0) AS HaiSSyaSCdSeq_KataKbn -- 配車型区分
            ,ISNULL(eVPM_SyaSyu02.KataKbn, 0) AS KSSyaSCdSeq_KataKbn -- 仮車型区分
            ,ISNULL(eVPM_SyaSyu03.KataKbn, 0) AS MiKSSyaSCdSeq_KataKbn -- 未仮型区分
            ,ISNULL(eVPM_Compny01.CompanyCd, 0) AS UkeEigCdSeq_CompanyCd -- 受付営業所会社コード
            ,ISNULL(eVPM_Compny02.CompanyCd, 0) AS HaiSSryCdSeq_CompanyCd -- 配車車輌会社コード
            ,ISNULL(eVPM_Compny03.CompanyCd, 0) AS KSSyaRSeq_CompanyCd -- 仮車車輌会社コード
            ,ISNULL(eVPM_Compny04.CompanyCd, 0) AS SyuEigCdSeq_CompanyCd -- 出庫営業所会社コード
            ,ISNULL(eTKD_Unkobi01.DrvJin, 0) AS TotalDrvJin
            ,ISNULL(eTKD_Unkobi01.GuiSu, 0) AS TotalGuiSu
        FROM (
                SELECT TKD_Haisha.UkeNo AS UkeNo
                        ,TKD_Haisha.UnkRen AS UnkRen
                FROM TKD_Haisha
                JOIN TKD_Yyksho ON TKD_Yyksho.UkeNo = TKD_Haisha.UkeNo
                LEFT JOIN VPM_YoyKbn ON VPM_YoyKbn.YoyaKbnSeq = TKD_Yyksho.YoyaKbnSeq
                WHERE TKD_Yyksho.UkeNo = @UkeNo
                        AND TKD_Yyksho.YoyaSyu = 1
                        AND TKD_Yyksho.SiyoKbn = 1
                        AND TKD_Haisha.SiyoKbn = 1
                GROUP BY TKD_Haisha.UkeNo
                        ,TKD_Haisha.UnkRen
                ) AS eTKD_Haisha
                ,TKD_Haisha
        -- 配車テーブルの受付番号、運行日連番より取得(運行日テーブル)
        LEFT JOIN TKD_Unkobi AS eTKD_Unkobi01 ON TKD_Haisha.UkeNo = eTKD_Unkobi01.UkeNo
                AND TKD_Haisha.UnkRen = eTKD_Unkobi01.UnkRen
                AND eTKD_Unkobi01.SiyoKbn = 1
        -- 配車テーブルの受付番号、運行日連番、傭車テーブルＳＥＱより取得(傭車テーブル)
        LEFT JOIN TKD_Yousha AS eTKD_Yousha01 ON TKD_Haisha.UkeNo = eTKD_Yousha01.UkeNo
                AND TKD_Haisha.UnkRen = eTKD_Yousha01.UnkRen
                AND TKD_Haisha.YouTblSeq = eTKD_Yousha01.YouTblSeq
                AND eTKD_Yousha01.SiyoKbn = 1
        -- 傭車テーブルの傭車先コードＳＥＱより取得(得意先マスタ)
        --（配車テーブルの配車年月日を基準日とする）
        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk01 ON eTKD_Yousha01.YouCdSeq = eVPM_Tokisk01.TokuiSeq
                AND TKD_Haisha.HaiSYmd BETWEEN eVPM_Tokisk01.SiyoStaYmd AND eVPM_Tokisk01.SiyoEndYmd
                AND eVPM_Tokisk01.TenantCdSeq = @TenantCdSeq
        -- 傭車テーブルの傭車先コードＳＥＱ、傭車先支店コードＳＥＱより取得(得意先支店マスタ)
        --（配車テーブルの配車年月日を基準日とする）
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt01 ON eTKD_Yousha01.YouCdSeq = eVPM_TokiSt01.TokuiSeq
                AND eTKD_Yousha01.YouSitCdSeq = eVPM_TokiSt01.SitenCdSeq
                AND TKD_Haisha.HaiSYmd BETWEEN eVPM_TokiSt01.SiyoStaYmd AND eVPM_TokiSt01.SiyoEndYmd
        -- 傭車テーブルの傭車先ＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(業者マスタ)
        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya01 ON eVPM_Tokisk01.GyosyaCdSeq = eVPM_Gyosya01.GyosyaCdSeq
        -- 配車テーブルの出庫営業所コードＳＥＱより取得(営業所マスタ)
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos01 ON TKD_Haisha.SyuEigCdSeq = eVPM_Eigyos01.EigyoCdSeq
        -- 配車テーブルの帰庫営業所コードＳＥＱより取得(営業所マスタ)
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos02 ON TKD_Haisha.KikEigSeq = eVPM_Eigyos02.EigyoCdSeq
        -- 配車テーブルの配車車輌コードＳＥＱより車輌編成マスタから取得した
        -- 営業所コードＳＥＱより取得(営業所マスタ)
        -- （配車テーブルの配車年月日を基準日とする）
        LEFT JOIN VPM_HenSya AS eVPM_HenSya01 ON TKD_Haisha.HaiSSryCdSeq = eVPM_HenSya01.SyaRyoCdSeq
                AND TKD_Haisha.HaiSYmd BETWEEN eVPM_HenSya01.StaYmd AND eVPM_HenSya01.EndYmd
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos03 ON eVPM_HenSya01.EigyoCdSeq = eVPM_Eigyos03.EigyoCdSeq
        -- 配車テーブルの仮車車輌コードＳＥＱより車輌編成マスタから取得した
        -- 営業所コードＳＥＱより取得(営業所マスタ)
        -- （配車テーブルの配車年月日を基準日とする）
        LEFT JOIN VPM_HenSya AS eVPM_HenSya02 ON TKD_Haisha.KSSyaRSeq = eVPM_HenSya02.SyaRyoCdSeq
                AND TKD_Haisha.HaiSYmd BETWEEN eVPM_HenSya02.StaYmd AND eVPM_HenSya02.EndYmd
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos04 ON eVPM_HenSya02.EigyoCdSeq = eVPM_Eigyos04.EigyoCdSeq
        -- 配車テーブルの配車車輌コードＳＥＱより取得(車輌マスタ)
        LEFT JOIN VPM_SyaRyo AS eVPM_SyaRyo01 ON TKD_Haisha.HaiSSryCdSeq = eVPM_SyaRyo01.SyaRyoCdSeq
        -- 配車テーブルの仮車車輌コードＳＥＱより取得(車輌マスタ)
        LEFT JOIN VPM_SyaRyo AS eVPM_SyaRyo02 ON TKD_Haisha.KSSyaRSeq = eVPM_SyaRyo02.SyaRyoCdSeq
        -- 配車テーブルの配車車輌コードＳＥＱより車輌マスタから取得した
        -- 車種コードＳＥＱより取得(車種マスタ)
        LEFT JOIN VPM_SyaSyu AS eVPM_SyaSyu01 ON eVPM_SyaRyo01.SyaSyuCdSeq = eVPM_SyaSyu01.SyaSyuCdSeq
                AND eVPM_SyaSyu01.TenantCdSeq = @TenantCdSeq
        -- 配車テーブルの仮車車輌コードＳＥＱより車輌マスタから取得した
        -- 車種コードＳＥＱより取得(車種マスタ)
        LEFT JOIN VPM_SyaSyu AS eVPM_SyaSyu02 ON eVPM_SyaRyo02.SyaSyuCdSeq = eVPM_SyaSyu02.SyaSyuCdSeq
                AND eVPM_SyaSyu02.TenantCdSeq = @TenantCdSeq
        LEFT JOIN TKD_YykSyu AS eVPM_SyaSyu03 ON TKD_Haisha.UkeNo = eVPM_SyaSyu03.UkeNo
                AND TKD_Haisha.UnkRen = eVPM_SyaSyu03.UnkRen
                AND TKD_Haisha.SyaSyuRen = eVPM_SyaSyu03.SyaSyuRen
        -- 配車テーブルの配車地コードＳＥＱより取得(配車地マスタ)
        LEFT JOIN VPM_Haichi AS eVPM_Haichi01 ON TKD_Haisha.HaiSCdSeq = eVPM_Haichi01.HaiSCdSeq
        -- 配車テーブルの配車地交通機関コードＳＥＱより取得(交通機関マスタ)
        LEFT JOIN VPM_Koutu AS eVPM_Koutu01 ON TKD_Haisha.HaiSKouKCdSeq = eVPM_Koutu01.KoukCdSeq
        -- 交通機関マスタの分類コードＳＥＱより取得(コード区分マスタ)
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb01 ON eVPM_Koutu01.BunruiCdSeq = eVPM_CodeKb01.CodeKbnSeq
        -- 配車テーブルの配車地交通機関コードＳＥＱ、配車地便コードＳＥＱより取得(便マスタ)
        LEFT JOIN VPM_Bin AS eVPM_Bin01 ON TKD_Haisha.HaiSBinCdSeq = eVPM_Bin01.BinCdSeq
                AND TKD_Haisha.HaiSYmd BETWEEN eVPM_Bin01.SiyoStaYmd AND eVPM_Bin01.SiyoEndYmd
        -- 配車テーブルの到着地コードＳＥＱより取得(配車地マスタ)
        LEFT JOIN VPM_Haichi AS eVPM_Haichi02 ON TKD_Haisha.TouCdSeq = eVPM_Haichi02.HaiSCdSeq
        -- 配車テーブルの到着地交通機関コードＳＥＱより取得(交通機関マスタ)
        LEFT JOIN VPM_Koutu AS eVPM_Koutu02 ON TKD_Haisha.TouKouKCdSeq = eVPM_Koutu02.KoukCdSeq
        -- 交通機関マスタの分類コードＳＥＱより取得(コード区分マスタ)
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb02 ON eVPM_Koutu02.BunruiCdSeq = eVPM_CodeKb02.CodeKbnSeq
        --                              '       AND     eVPM_CodeKb02.SiyoKbn                                           =       1                                                               
        -- 配車テーブルの到着地交通機関コードＳＥＱ、到着地便コードＳＥＱより取得(便マスタ)
        LEFT JOIN VPM_Bin AS eVPM_Bin02 ON TKD_Haisha.TouBinCdSeq = eVPM_Bin02.BinCdSeq
                AND TKD_Haisha.HaiSYmd BETWEEN eVPM_Bin02.SiyoStaYmd AND eVPM_Bin02.SiyoEndYmd
        -- 配車テーブルの行き先県コードＳＥＱより取得(場所マスタ)
        LEFT JOIN VPM_Basyo AS eVPM_Basyo01 ON TKD_Haisha.IkMapCdSeq = eVPM_Basyo01.BasyoMapCdSeq
        -- 配車テーブルのその他人員区分コード１より取得(OTHJINKBN)(コード区分マスタ)
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb03 ON eVPM_CodeKb03.CodeKbn = dbo.FP_RpZero(2, TKD_Haisha.OthJinKbn1)
                AND eVPM_CodeKb03.CodeSyu = 'OTHJINKBN'
                AND eVPM_CodeKb03.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'OTHJINKBN'
                                AND KanriKbn <> 1
                        )
        -- 配車テーブルのその他人員区分コード２より取得(OTHJINKBN)(コード区分マスタ)
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb04 ON eVPM_CodeKb04.CodeKbn = dbo.FP_RpZero(2, TKD_Haisha.OthJinKbn2)
                AND eVPM_CodeKb04.CodeSyu = 'OTHJINKBN'
                AND eVPM_CodeKb04.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'OTHJINKBN'
                                AND KanriKbn <> 1
                        )
        -- 配車テーブルの傭車型区分より取得(KATAKBN)(コード区分マスタ)
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb05 ON eVPM_CodeKb05.CodeSyu = 'KATAKBN'
                AND CONVERT(VARCHAR(10), TKD_Haisha.YouKataKbn) = eVPM_CodeKb05.CodeKbn
                AND eVPM_CodeKb05.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'KATAKBN'
                                AND KanriKbn <> 1
                        )
        -- 配車テーブルの受付条件区分コードより取得(UKEJYKBNCD)(コード区分マスタ)
        LEFT JOIN VPM_CodeKb AS eVPM_CodeKb06 ON eVPM_CodeKb06.CodeKbn = dbo.FP_RpZero(2, TKD_Haisha.UkeJyKbnCd)
                AND eVPM_CodeKb06.CodeSyu = 'UKEJYKBNCD'
                AND eVPM_CodeKb06.TenantCdSeq = (
                        SELECT CASE WHEN COUNT(*) > 0 THEN @TenantCdSeq ELSE 0 END AS TenantCdSeq
                        FROM VPM_CodeSy
                        WHERE CodeSyu = 'UKEJYKBNCD'
                                AND KanriKbn <> 1
                        )
        -- 配車テーブルの受付番号より取得(予約書テーブル)
        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Yyksho.UkeNo = TKD_Haisha.UkeNo
        -- 予約書テーブルの得意先ＳＥＱより取得(請求年月日が使用開始/終了年月日の範囲内)(得意先マスタ)
        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk02 ON eTKD_Yyksho.TokuiSeq = eVPM_Tokisk02.TokuiSeq
                AND eTKD_Yyksho.SeiTaiYmd BETWEEN eVPM_Tokisk02.SiyoStaYmd AND eVPM_Tokisk02.SiyoEndYmd
                AND eVPM_Tokisk02.TenantCdSeq = @TenantCdSeq
        -- 予約書テーブルの支店コードＳＥＱより取得(請求年月日が使用開始/終了年月日の範囲内)(得意先支店マスタ)
        LEFT JOIN VPM_TokiSt AS eVPM_TokiSt02 ON eTKD_Yyksho.TokuiSeq = eVPM_TokiSt02.TokuiSeq
                AND eTKD_Yyksho.SitenCdSeq = eVPM_TokiSt02.SitenCdSeq
                AND eTKD_Yyksho.SeiTaiYmd BETWEEN eVPM_TokiSt02.SiyoStaYmd AND eVPM_TokiSt02.SiyoEndYmd
        -- 予約書テーブルの受付営業所コードＳＥＱより取得(営業所マスタ)
        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos05 ON eTKD_Yyksho.UkeEigCdSeq = eVPM_Eigyos05.EigyoCdSeq
        -- 予約書テーブルの受付営業所コードＳＥＱより営業所マスタから取得した
        -- 会社コードＳＥＱより取得(会社マスタ)
        LEFT JOIN VPM_Compny AS eVPM_Compny01 ON eVPM_Eigyos05.CompanyCdSeq = eVPM_Compny01.CompanyCdSeq
                AND eVPM_Compny01.TenantCdSeq = @TenantCdSeq
        -- 配車テーブルの配車車輌コードＳＥＱより車輌編成マスタから取得した
        -- 営業所コードＳＥＱを使用し営業所マスタから取得した会社コードＳＥＱより取得(会社マスタ)
        LEFT JOIN VPM_Compny AS eVPM_Compny02 ON eVPM_Eigyos03.CompanyCdSeq = eVPM_Compny02.CompanyCdSeq
                AND eVPM_Compny02.TenantCdSeq = @TenantCdSeq
        -- 配車テーブルの仮車車輌コードＳＥＱより車輌編成マスタから取得した
        -- 営業所コードＳＥＱを使用し営業所マスタから取得した会社コードＳＥＱより取得(会社マスタ)
        LEFT JOIN VPM_Compny AS eVPM_Compny03 ON eVPM_Eigyos04.CompanyCdSeq = eVPM_Compny03.CompanyCdSeq
                AND eVPM_Compny03.TenantCdSeq = @TenantCdSeq
        -- 配車テーブルの出庫営業所コードＳＥＱより営業所マスタから取得した
        -- 会社コードＳＥＱより取得(会社マスタ)
        LEFT JOIN VPM_Compny AS eVPM_Compny04 ON eVPM_Eigyos01.CompanyCdSeq = eVPM_Compny04.CompanyCdSeq
                AND eVPM_Compny04.TenantCdSeq = @TenantCdSeq
        WHERE TKD_Haisha.UkeNo = eTKD_Haisha.UkeNo
                AND TKD_Haisha.UnkRen = eTKD_Haisha.UnkRen
                AND TKD_Haisha.SiyoKbn = 1
        ORDER BY TKD_Haisha.UkeNo
                ,TKD_Haisha.UnkRen
                ,TKD_Haisha.GoSyaJyn
                ,TKD_Haisha.BunKSyuJyn

    SET	@ROWCOUNT	=	@@ROWCOUNT
	END
RETURN																													
