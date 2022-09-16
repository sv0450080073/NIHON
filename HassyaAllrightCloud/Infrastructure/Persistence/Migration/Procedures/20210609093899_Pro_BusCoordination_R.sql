USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_BusCoordination_R]    Script Date: 2021/05/26 15:04:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



----------------------------------------------------
--  System-Name :   
--  Module-Name :  
--  SP-ID       :   
--  DB-Name     :   
--  Name        : 
--  Date        :   
--  Author      :   
--  Descriotion :   調整量データのSelect処理
----------------------------------------------------
--  Update      : N.T.L.ANH -- 2021/06/09
--  Comment     : Change condition
----------------------------------------------------
CREATE OR ALTER           PROCEDURE [dbo].[Pro_BusCoordination_R]
	(
	-- Parameter
		@CheckDaySearch char(2),
		@DateFrom  char(8),
		@DateTo char(8),
		@YoyakuFrom varchar(11),
		@YoyakuTo varchar(11),
		@CustomerFrom varchar(11),
		@CustomerTo varchar(11),
		@SupplierFrom varchar(11),
		@SupplierTo varchar(11),
		@SaleBranch varchar(255),
		@BookingFrom Varchar(15),
		@BookingTo varchar(15),
		@Staff varchar(255),
		@PersonInput varchar(255),
		@TenantCdSeq		varchar(5),
		@UkenoList varchar(MAX),
	    @FormOutput int
	)
AS 
BEGIN
IF OBJECT_ID(N'tempdb..#TempTable') IS NOT NULL
	BEGIN
	DROP TABLE #TempTable
	END
	-- 運行日テーブル 受付番号毎の最小の運行日連番
	;

SELECT ISNULL(UNKOBI.UkeNo,'') as UNKOBI_Ukeno              																																	          																															
	  ,ISNULL(FORMAT ( convert(datetime,UNKOBI.HaiSYmd , 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ),'') as UNKOBI_HaiSYmd
	  ,ISNULL(UNKOBI.HaiSTime,'') as UNKOBI_HaiSTime           																															
	  ,ISNULL(UNKOBI.SyuPaTime,'') as UNKOBI_SyuPaTime
	  ,ISNULL(FORMAT ( convert(datetime,UNKOBI.TouYmd , 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ),'')  as UNKOBI_TouYmd
	  ,ISNULL(SUBSTRING(UNKOBI.DanTaNm,1,34),'')   AS  UNKOBI_DanTaNm    																													
	  ,ISNULL(UNKOBI.KanjJyus1,'') as UNKOBI_KanjJyus1           																														
	  ,ISNULL(UNKOBI.KanjJyus2,'') as UNKOBI_KanjJyus2           																															
	  ,ISNULL(UNKOBI.KanjTel ,'') as UNKOBI_KanjTel            																															
	  ,ISNULL(UNKOBI.KanJNm,'') as UNKOBI_KanjNm           																													
	  ,ISNULL(UNKOBI.HaiSNm,'') as UNKOBI_HaiSNm             																															
	  ,ISNULL(UNKOBI.HaiSJyus1,'') as UNKOBI_HaiSJyus1          																											
	  ,ISNULL(UNKOBI.HaiSJyus2,'') as UNKOBI_HaiSJyus2              																															
	  ,ISNULL(UNKOBI.IkNm,'') as UNKOBI_IkNm                 																												               																															          																																        																																																													
	  ,ISNULL(UNKOBI.HaiSSetTime,'') as UNKOBI_HaiSSetTime        																																																													          																													        																															    																															
	  ,ISNULL(YYKSHO.TokuiTel,'') as YYKSHO_TokuiTel           																															
	  ,ISNULL(YYKSHO.TokuiTanNm,'') as YYKSHO_TokuiTanNm          																																                																															               																															         																																
	  ,ISNULL(TOKISK.TokuiNm,'') as TOKISK_TokuiNm             																																
	  ,ISNULL(TOKIST.SitenNm,'') as TOKIST_SitenNm             																															
	  ,ISNULL(SIRSK.TokuiNm,'') as SIRSK_TokuiNm                																															
	  ,ISNULL(SIRST.SitenNm,'') as SIRST_SitenNm
      ,ISNULL(SJJOKBN1.CodeKbnNm,'') as SJJOKBN1_CodeKbnNm
	  ,ISNULL(SJJOKBN2.CodeKbnNm,'') as SJJOKBN2_CodeKbnNm   
	  ,ISNULL(SJJOKBN3.CodeKbnNm,'') as SJJOKBN3_CodeKbnNm 
	  ,ISNULL(SJJOKBN4.CodeKbnNm,'') as SJJOKBN4_CodeKbnNm
	  ,ISNULL(SJJOKBN5.CodeKbnNm,'') as SJJOKBN5_CodeKbnNm
	  ,ISNULL(OTHERJIN1.CodeKbnNm,'')  AS OTHERJIN1_CodeKbnNm
	  ,ISNULL(UNKOBI.OthJin1,0)  AS UNKOBI_OthJin1
	  ,ISNULL(OTHERJIN2.CodeKbnNm,'')  AS OTHERJIN2_CodeKbnNm
	  ,ISNULL(UNKOBI.OthJin2,0)    AS UNKOBI_OthJin2 
	  ,ISNULL(UNKOBI.JyoSyaJin,0)  as UNKOBI_JyoSyaJin
	  ,ISNULL(UNKOBI.PlusJin,0)  as UNKOBI_PlusJin
	  ,ISNULL(UNKOBI.HaiSKouKNm,'')  as UNKOBI_HaiSKouKNm            											
	  ,ISNULL(UNKOBI.HaisBinNm,'') as UNKOBI_HaisBinNm            												        											
	  ,ISNULL(UNKOBI.TouSKouKNm,'')  as UNKOBI_TouSKouKNm            											
	  ,ISNULL(UNKOBI.TouSBinNm,'') as UNKOBI_TouSBinNm            										
	  ,ISNULL(UNKOBI.TouSetTime,'') as UNKOBI_TouSetTime  
      ,ISNULL(UNKOBI.KikYmd,'') as UNKOBI_KikYmd
	  ,ISNULL(UNKOBI.SyuKoYmd,'') as UNKOBI_SyuKoYmd
	  ,ISNULL(UNKOBI.BikoNm,'') as UNKOBI_BikoNm
	  ,ISNULL(UNKOBI.TouNm,'') as UNKOBI_TouNm
	  ,ISNULL(UNKOBI.TouJyusyo1,'') as UNKOBI_TouJyusyo1            																															
	  ,ISNULL(UNKOBI.TouJyusyo2,'') as UNKOBI_TouJyusyo2
	  ,ISNULL(UNKOBI.TouChTime,'') as UNKOBI_TouChTime
	  ,ISNULL(YYKSHO.UntKin,0)  as YYKSHO_UntKin
	  ,ISNULL(ZEIKBN.CodeKbnNm,'')  as ZEIKBN_CodeKbnNm 
	  ,ISNULL(YYKSHO.ZeiRui,0) as YYKSHO_ZeiRui
	  ,ISNULL(YYKSHO.UntKin + YYKSHO.ZeiRui,0) as Bus_ZeiKomiKinGak 
	  ,ISNULL(CONCAT(YYKSHO.TesuRitu,'%'),'') as Bus_TesuRyoRitu  
	  ,ISNULL(YYKSHO.TesuRyoG,0) as YYKSHO_TesuRyoG     
	  ,ISNULL((SELECT SUM(ISNULL(SyaRyoSyo,0)) + SUM(ISNULL(SyaRyoUnc,0)) FROM TKD_Yousha WHERE UkeNo = UNKOBI.UkeNo AND SiyoKbn = 1),0) as YouSyaGak 
	  ,ISNULL(UKEJOKEN.CodeKbnNm,'') as UKEJOKEN_CodeKbnNm
	  ,ISNULL(SUBSTRING(UNKOBI.DanTaNm,35,34),'')  AS  UNKOBI_DanTaNm2
	  ,ISNULL(FORMAT(YYKSHO.UkeCd,'0000000000'),'') as YYKSHO_UkeCd  
	  ,ISNULL(UNKOBI.UnkRen,'') as UNKOBI_UnkRen
into #TempTable
FROM
    TKD_Unkobi AS UNKOBI
    LEFT JOIN TKD_Yyksho AS YYKSHO ON YYKSHO.UkeNo = UNKOBI.UkeNo
    LEFT JOIN VPM_Tokisk AS TOKISK ON TOKISK.TokuiSeq = YYKSHO.TokuiSeq
    AND TOKISK.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_Gyosya AS GYOSYA ON GYOSYA.GyosyaCdSeq = TOKISK.GyosyaCdSeq
    AND GYOSYA.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_TokiSt AS TOKIST ON TOKIST.TokuiSeq = YYKSHO.TokuiSeq
    AND TOKIST.SitenCdSeq = YYKSHO.SitenCdSeq
    AND TOKIST.SiyoStaYmd <= UNKOBI.HaiSYmd
    AND TOKIST.SiyoEndYmd >= UNKOBI.HaiSYmd
    LEFT JOIN VPM_Tokisk AS SIRSK ON SIRSK.TokuiSeq = YYKSHO.SirCdSeq
    AND SIRSK.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_Gyosya AS SIRGS ON SIRGS.GyosyaCdSeq = SIRSK.GyosyaCdSeq
    AND SIRGS.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_TokiSt AS SIRST ON SIRST.TokuiSeq = YYKSHO.SirCdSeq
    AND SIRST.SitenCdSeq = YYKSHO.SirSitenCdSeq
    AND SIRST.SiyoStaYmd <= UNKOBI.HaiSYmd
    AND SIRST.SiyoEndYmd >= UNKOBI.HaiSYmd
    LEFT JOIN VPM_CodeKb AS ZEIKBN ON ZEIKBN.CodeKbn = YYKSHO.ZeiKbn
    AND ZEIKBN.CodeSyu = 'ZEIKBN'
    AND ZEIKBN.SiyoKbn = 1
    AND ZEIKBN.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_CodeKb AS UKEJOKEN ON UKEJOKEN.CodeKbn = UNKOBI.UkeJyKbnCd
    AND UKEJOKEN.TenantCdSeq = @TenantCdSeq
    AND UKEJOKEN.CodeSyu = 'UKEJYKBNCD'
    LEFT JOIN VPM_CodeKb AS OTHERJIN1 ON OTHERJIN1.CodeKbn = UNKOBI.OthJinKbn1
    AND OTHERJIN1.CodeSyu = 'OTHJINKBN'
    AND OTHERJIN1.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_CodeKb AS OTHERJIN2 ON OTHERJIN2.CodeKbn = UNKOBI.OthJinKbn2
    AND OTHERJIN2.CodeSyu = 'OTHJINKBN'
    AND OTHERJIN2.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_CodeKb AS SJJOKBN1 ON SJJOKBN1.CodeKbn = UNKOBI.SijJoKbn1
    AND SJJOKBN1.TenantCdSeq = @TenantCdSeq
    AND SJJOKBN1.CodeSyu = 'SIJJOKBN1'
    LEFT JOIN VPM_CodeKb AS SJJOKBN2 ON SJJOKBN2.CodeKbn = UNKOBI.SijJoKbn2
    AND SJJOKBN2.TenantCdSeq = @TenantCdSeq
    AND SJJOKBN2.CodeSyu = 'SIJJOKBN2'
    LEFT JOIN VPM_CodeKb AS SJJOKBN3 ON SJJOKBN3.CodeKbn = UNKOBI.SijJoKbn3
    AND SJJOKBN3.TenantCdSeq = @TenantCdSeq
    AND SJJOKBN3.CodeSyu = 'SIJJOKBN3'
    LEFT JOIN VPM_CodeKb AS SJJOKBN4 ON SJJOKBN4.CodeKbn = UNKOBI.SijJoKbn4
    AND SJJOKBN4.TenantCdSeq = @TenantCdSeq
    AND SJJOKBN4.CodeSyu = 'SIJJOKBN4'
    LEFT JOIN VPM_CodeKb AS SJJOKBN5 ON SJJOKBN5.CodeKbn = UNKOBI.SijJoKbn5
    AND SJJOKBN5.TenantCdSeq = @TenantCdSeq
    AND SJJOKBN5.CodeSyu = 'SIJJOKBN5'
    LEFT JOIN VPM_YoyKbn AS YOYKBN ON YOYKBN.YoyaKbnSeq = YYKSHO.YoyaKbnSeq
    AND YOYKBN.SiyoKbn = 1
    AND YOYKBN.TenantCdSeq = @TenantCdSeq
WHERE YYKSHO.SiyoKbn = 1
    -- 固定設定
    AND YYKSHO.YoyaSyu = 1 --固定値：1（受注）
    AND UNKOBI.SiyoKbn = 1 --固定値：1（使用中）
    AND YYKSHO.TenantCdSeq = @TenantCdSeq
    AND (@UkenoList = '' OR @UkenoList Is NULL OR (concat(UNKOBI.UkeNo,FORMAT(UNKOBI.UnkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))))
    --画面の日付設定に配車日付を指定する場合 (1)
    AND  ((@CheckDaySearch = '1' AND UNKOBI.HaiSYmd >= @DateFrom AND UNKOBI.HaiSYmd <= @DateTo)
	--画面の日付設定に到着日付を指定する場合 (2)
	OR  (@CheckDaySearch = '2' AND UNKOBI.TouYmd >= @DateFrom AND UNKOBI.TouYmd <= @DateTo)
	--画面の日付設定に到着日付を指定する場合 (3)
	OR  (@CheckDaySearch = '3' AND YYKSHO.UkeYmd >= @DateFrom AND YYKSHO.UkeYmd <= @DateTo))
    --画面の予約区分を入力する場合
    AND (@YoyakuFrom = 0 OR  YOYKBN.YoyaKbn >= @YoyakuFrom)  
    AND (@YoyakuTo = 0 OR  YOYKBN.YoyaKbn <= @YoyakuTo)  
    --画面の得意先を入力する場合、
    AND (@CustomerFrom = '0' OR (FORMAT(GYOSYA.GyosyaCd,'000') + FORMAT(TOKISK.TokuiCd,'0000') + FORMAT(TOKIST.SitenCd,'0000')) >= @CustomerFrom)
    AND (@CustomerTo = '0' OR (FORMAT(GYOSYA.GyosyaCd,'000') + FORMAT(TOKISK.TokuiCd,'0000') + FORMAT(TOKIST.SitenCd,'0000')) <= @CustomerTo)
    --画面の仕入先を入力する場合、
    AND (@SupplierFrom = '0' OR (FORMAT(SIRGS.GyosyaCd,'000') + FORMAT(SIRSK.TokuiCd,'0000') + FORMAT(SIRST.SitenCd,'0000')) >= @SupplierFrom)
    AND (@SupplierTo = '0' OR (FORMAT(SIRGS.GyosyaCd,'000') + FORMAT(SIRSK.TokuiCd,'0000') + FORMAT(SIRST.SitenCd,'0000')) <= @SupplierTo)
    --画面の受付番号を入力する場合
    AND (@BookingFrom = '' OR YYKSHO.UkeCd >= @BookingFrom)
    AND (@BookingTo = '' OR YYKSHO.UkeCd <= @BookingTo)
    --画面の受付営業所を入力する場合
    AND ( @SaleBranch='0' OR YYKSHO.UkeEigCdSeq = @SaleBranch)
    --画面の営業所担当者を入力する場合
    AND (@Staff='0' OR YYKSHO.EigTanCdSeq = @Staff)
    --画面の入力担当者を入力する場合
    AND (@PersonInput='0' OR YYKSHO.InTanCdSeq = @PersonInput)
ORDER BY UNKOBI.UkeNo ASC
OPTION (RECOMPILE)
SELECT * from #TempTable 
OPTION (RECOMPILE)
END
