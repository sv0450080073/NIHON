USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dPaymentRequestPre_R]    Script Date: 2020/12/09 9:44:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	
-- Module-Name	:	
-- SP-ID		:	PK_dPaymentRequestPreview_R
-- DB-Name		:	
-- Name			:	
-- Date			:	2020/09/12
-- Author		:	T.L.DUY
-- Description	:	Select
-- 				:	

CREATE OR ALTER PROCEDURE [dbo].[PK_dPaymentRequestPreview_R]
--
	(
		@SeikYm						CHAR	(6)	        
	,	@SeiHatYmd					CHAR	(8)			
	,	@SeiOutTime					CHAR	(4)			
	,	@InTanCdSeq					INT					
	,	@SeiOutSyKbn				TINYINT								--  - OutputInst (1,2,3)
	,	@SeiGenFlg					TINYINT				
	,	@StaUkeNo					NCHAR(15)							
	,	@EndUkeNo					NCHAR(15)						
	,	@StaYoyaKbn					TINYINT				
	,	@EndYoyaKbn					TINYINT				
	,	@SeiEigCdSeq				INT					
	,	@SeiSitKbn					TINYINT								--  - BillingAddress
	,	@StaSeiCdSeq				INT					
	,	@StaSeiSitCdSeq				INT					
	,	@EndSeiCdSeq				INT					
	,	@EndSeiSitCdSeq				INT					
	,	@SimeD						TINYINT				
	,	@PrnCpys					TINYINT				
	,	@PrnCpysTan					TINYINT				
	,	@TesPrnKbn					TINYINT				
	,	@SeiFutUncKbn				TINYINT				
	,	@SeiFutFutKbn				TINYINT				
	,	@SeiFutTukKbn				TINYINT				
	,	@SeiFutTehKbn				TINYINT				
	,	@SeiFutGuiKbn				TINYINT				
	,	@SeiFutTumKbn				TINYINT				
	,	@SeiFutCanKbn				TINYINT				
	,	@ZipCd						CHAR	(12)		
	,	@Jyus1						VARCHAR	(60)		
	,	@Jyus2						VARCHAR	(60)		
	,	@TokuiNm					VARCHAR	(60)		
	,	@SitenNm					VARCHAR	(60)		
	,	@SiyoKbn					TINYINT				
	,	@UpdYmd						CHAR	(8)			
	,	@UpdTime					CHAR	(6)			
	,	@UpdSyainCd					INT					
	,	@UpdPrgID					CHAR	(10)		
	,	@KuriSyoriKbn				TINYINT				
	,   @TenantCdSeq                INT								 -- TenantCdSeq
	,   @StartBillAdd               NVARCHAR(11)					 -- StartBillAdd
	,   @EndBillAdd					NVARCHAR(11)					 -- EndBillAdd
	,   @InvoiceOutNum				INT								 -- InvoiceOutNum
	,   @InvoiceSerNum				SMALLINT						 -- InvoiceSerNum
	,   @BillingType				NVARCHAR(100)					 -- BillingType
	,   @OutDataTable				NVARCHAR(MAX)					 -- OutDataTable
	
--2014/08/08 M.OHMORI END
	,	@SeiOutSeq					INTEGER			OUTPUT		-- 
	,	@ReturnCd					INTEGER			OUTPUT		-- 
	,	@RowCount					INTEGER			OUTPUT		-- 
	,	@ReturnMsg					VARCHAR(MAX)	OUTPUT		-- 
	,	@eProcedure					VARCHAR(20)		OUTPUT		-- 
	,	@eLine						VARCHAR(20)		OUTPUT		-- 
	)
AS           
	-- 
		DECLARE @Temp_TKD_SeiPrS TABLE
	(
		SeiOutSeq int,
		SeikYm char(6),
		SeiHatYmd char(8),
		SeiOutTime char(4),
		InTanCdSeq int,
		SeiOutSyKbn tinyint,
		SeiGenFlg tinyint,
		StaUkeNo nchar(15),
		EndUkeNo nchar(15),
		StaYoyaKbn tinyint,
		EndYoyaKbn tinyint,
		SeiEigCdSeq int,
		SeiSitKbn tinyint,
		StaSeiCdSeq int,
		StaSeiSitCdSeq int,
		EndSeiCdSeq int,
		EndSeiSitCdSeq int,
		SimeD tinyint,
		PrnCpys tinyint,
		PrnCpysTan tinyint,
		TesPrnKbn tinyint,
		SeiFutUncKbn tinyint,
		SeiFutFutKbn tinyint,
		SeiFutTukKbn tinyint,
		SeiFutTehKbn tinyint,
		SeiFutGuiKbn tinyint,
		SeiFutTumKbn tinyint,
		SeiFutCanKbn tinyint,
		ZipCd char(12),
		Jyus1 nvarchar(60),
		Jyus2 nvarchar(60),
		TokuiNm nvarchar(60),
		SitenNm nvarchar(60),
		SiyoKbn tinyint,
		UpdYmd char(8),
		UpdTime char(6),
		UpdSyainCd int,
		UpdPrgID char(10),
		INDEX IX1 CLUSTERED(SeiOutSeq)
	);

	DECLARE @Temp_TKD_Seikyu TABLE
	(
		SeiOutSeq int,
		SeiRen smallint,
		TokuiSeq int,
		SitenCdSeq int,
		SiyoEndYmd char(8),
		SeikYm char(6),
		ZenKurG int,
		KonUriG int,
		KonSyoG int,
		KonTesG int,
		KonNyuG int,
		KonSeiG int,
		SiyoKbn tinyint,
		UpdYmd char(8),
		UpdTime char(6),
		UpdSyainCd int,
		UpdPrgID char(10),
		INDEX IX1 CLUSTERED(SeiOutSeq,SeiRen)
	);

	DECLARE @Temp_TKD_SeiMei TABLE
	(
		SeiOutSeq int,
		SeiRen smallint,
		SeiMeiRen smallint,
		UkeNo nchar(15),
		MisyuRen smallint,
		UriGakKin int,
		SyaRyoSyo int,
		SyaRyoTes int,
		SeiKin int,
		NyuKinRui numeric(9, 0),
		SeiSaHKbn tinyint,
		Zeiritsu numeric(3, 1),
		SiyoKbn tinyint,
		UpdYmd char(8),
		UpdTime char(6),
		UpdSyainCd int,
		UpdPrgID char(10),		
		INDEX IX1 CLUSTERED(SeiOutSeq,SeiRen,SeiMeiRen),
		INDEX IX2 NONCLUSTERED(UkeNo)
	);

	DECLARE @Temp_TKD_SeiUch TABLE
	(
		SeiOutSeq int,
		SeiRen smallint,
		SeiMeiRen smallint,
		SeiUchRen smallint,
		HasYmd char(8),
		YoyaNm nvarchar(100),
		FutTumNm nvarchar(50),
		HaiSYmd char(8),
		TouYmd char(8),
		Suryo smallint,
		TanKa int,
		SyaSyuNm nvarchar(12),
		SiyoKbn tinyint,
		UpdYmd char(8),
		UpdTime char(6),
		UpdSyainCd int,
		UpdPrgID char(10),
		INDEX IX1 CLUSTERED(SeiOutSeq,SeiRen,SeiMeiRen,SeiUchRen)
	);

	SET		@SeiOutSeq			=	0		-- 
	SET		@ReturnCd			=	0		-- 
	SET		@RowCount			=	0		-- 
	SET		@ReturnMsg			=	' '		-- 
	SET		@eProcedure			=	' '		-- 
	SET		@eLine				=	' '		-- 

	BEGIN
-- ****************************************************************************************************************************************
		INSERT	INTO
				@Temp_TKD_SeiPrS
		VALUES
			(
				0,
				ISNULL(@SeikYm, '')
			,	ISNULL(@SeiHatYmd, '')
			,	ISNULL(@SeiOutTime, '')
			,	@InTanCdSeq
			,	@SeiOutSyKbn
			,	@SeiGenFlg
			,	ISNULL(@StaUkeNo, '')
			,	ISNULL(@EndUkeNo, '')
			,	@StaYoyaKbn
			,	@EndYoyaKbn
			,	@SeiEigCdSeq
			,	@SeiSitKbn
			,	@StaSeiCdSeq
			,	@StaSeiSitCdSeq
			,	@EndSeiCdSeq
			,	@EndSeiSitCdSeq
			,	@SimeD
			,	@PrnCpys
			,	@PrnCpysTan
			,	@TesPrnKbn
			,	@SeiFutUncKbn
			,	@SeiFutFutKbn
			,	@SeiFutTukKbn
			,	@SeiFutTehKbn
			,	@SeiFutGuiKbn
			,	@SeiFutTumKbn
			,	@SeiFutCanKbn
			,	ISNULL(@ZipCd, '')
			,	ISNULL(@Jyus1, '')
			,	ISNULL(@Jyus2, '')
			,	ISNULL(@TokuiNm, '')
			,	ISNULL(@SitenNm, '')
			,	@SiyoKbn
			,	ISNULL(@UpdYmd, '')
			,	ISNULL(@UpdTime, '')
			,	@UpdSyainCd
			,	ISNULL(@UpdPrgID, '')
			)

		SET		@SeiOutSeq		=	0

		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
			OR	ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
WITH 
eTKD_Unkobi01   AS 
( 
    SELECT 
            TKD_Unkobi.UkeNo                            AS      UkeNo 
       ,   TKD_Unkobi.UnkRen                           AS      UnkRen 
       ,   TKD_Unkobi.HaiSYmd                          AS      HaiSYmd 
       ,   TKD_Unkobi.TouYmd                           AS      TouYmd 
       ,   TKD_Unkobi.IkNm							 AS      IkNm 
       ,   TKD_Unkobi.DanTaNm							 AS      DanTaNm 
       ,   ROW_NUMBER()    OVER    (   PARTITION BY 
                                                                TKD_Unkobi.UkeNo 
                                        ORDER BY 
                                                                TKD_Unkobi.UkeNo 
                                                       ,       TKD_Unkobi.UnkRen 
                                    )                   AS      RowNumbr 
    FROM 
            TKD_Unkobi 
    WHERE 
            TKD_Unkobi.SiyoKbn                          =       1 
) 
, 
eTKD_SeiMei01   AS 
( 
    SELECT  DISTINCT 
            TKD_SeiMei.UkeNo                            AS      UkeNo 
       ,   TKD_SeiMei.MisyuRen                         AS      MisyuRen 
       ,   2                                           AS      SeiSaHKbn 
    FROM 
            TKD_SeiMei 
    WHERE 
            TKD_SeiMei.SiyoKbn                          =       1 
) 
, 
eTKD_Mishum01   AS 
( 
    SELECT 
            TKD_Mishum.UkeNo                            AS      UkeNo 
       ,   TKD_Mishum.MisyuRen                         AS      MisyuRen 
       ,   TKD_Mishum.HenKai                           AS      HenKai 
       ,   TKD_Mishum.SeiFutSyu                        AS      SeiFutSyu 
       ,   TKD_Mishum.UriGakKin                        AS      UriGakKin 
       ,   TKD_Mishum.SyaRyoSyo                        AS      SyaRyoSyo 
       ,   TKD_Mishum.SyaRyoTes                        AS      SyaRyoTes 
       ,   TKD_Mishum.SeiKin                           AS      SeiKin 
       ,   TKD_Mishum.NyuKinRui                        AS      NyuKinRui 
       ,   TKD_Mishum.FutuUnkRen                       AS      FutuUnkRen 
       ,   TKD_Mishum.FutTumRen                        AS      FutTumRen 
       ,   eTKD_Yyksho11.SeikYm                        AS      SeikYm
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_Gyosya12.GyosyaCd 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_Gyosya11.GyosyaCd END						AS GyosyaCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_Tokisk12.TokuiCd
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_Tokisk11.TokuiCd END                      AS TokuiCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt11.SeiCdSeq 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eTKD_Yyksho11.TokuiSeq END                     AS TokuiSeq 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SitenCd 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SitenCd END                      AS SitenCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt11.SeiSitenCdSeq 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eTKD_Yyksho11.SitenCdSeq END					AS SitenCdSeq 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SiyoEndYmd
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SiyoEndYmd END                   AS SiyoEndYmd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SimeD
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SimeD END                        AS SimeD 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.DanTaNm   ,eTKD_Yyksho11.YoyaNm) 
                ELSE    ISNULL(eTKD_Unkobi12.DanTaNm   ,eTKD_Yyksho11.YoyaNm) 
            END                                         AS      YoyaNm 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Yyksho11.SeiTaiYmd ,       ' ') 
                ELSE    ISNULL(eTKD_FutTum11.HasYmd    ,       ' ') 
            END                                         AS      HasYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.IkNm      ,       ' ') 
                ELSE    ISNULL(eTKD_FutTum11.FutTumNm  ,       ' ') 
            END                                         AS      FutTumNm 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) 
                THEN    0 
                ELSE    ISNULL(eTKD_FutTum11.Suryo     ,       0) 
            END                                         AS      Suryo 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) 
                THEN    0 
                ELSE    ISNULL(eTKD_FutTum11.TanKa     ,       0) 
            END                                         AS      TanKa 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.HaiSYmd   ,       ' ') 
                ELSE    ' ' 
            END                                         AS      HaiSYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.TouYmd    ,       ' ') 
                ELSE    ' ' 
            END                                         AS      TouYmd 
       ,   eTKD_Yyksho11.YoyaKbnSeq                    AS      YoyaKbnSeq 
       ,   CONCAT(CASE WHEN VPM_YoyaKbnSort11.PriorityNum IS NULL THEN '99' ELSE FORMAT(VPM_YoyaKbnSort11.PriorityNum, '00') END 
       ,   FORMAT(eVPM_YoyKbn11.YoyaKbn,'00'))     AS YoyaKbnSort 
       ,   eTKD_Yyksho11.SeiEigCdSeq                   AS      SeiEigCdSeq 
       ,   eTKD_Yyksho11.SeiTaiYmd                     AS      SeiTaiYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    1 
                ELSE    2 
            END                                         AS      SeiFutSyu_Sort 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ' ' 
                ELSE    SUBSTRING(ISNULL(eTKD_FutTum11.ExpItem	,' ')	,1,3) 
            END                                         AS      FutTum_Sort 
		,	CASE 
				WHEN	TKD_Mishum.SeiFutSyu			 IN		(1,7)	
				THEN	eTKD_Yyksho11.Zeiritsu
				ELSE	eTKD_FutTum11.Zeiritsu
			END											 AS		Zeiritsu
    FROM 
            TKD_Mishum 
            JOIN        TKD_Yyksho      AS      eTKD_Yyksho11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_Yyksho11.UkeNo 
                                        AND     TKD_Mishum.SiyoKbn              =       1 
                                        AND     eTKD_Yyksho11.SiyoKbn           =       1 
                                        AND (   (   TKD_Mishum.SeiFutSyu        =       7 
                                                AND eTKD_Yyksho11.YoyaSyu       =       2 
                                                ) 
                                            OR  (   TKD_Mishum.SeiFutSyu        <>      7 
                                                AND eTKD_Yyksho11.YoyaSyu       =       1 
                                                ) 
                                            ) 
            LEFT JOIN   VPM_YoyKbn      AS      eVPM_YoyKbn11 
                                        ON      eTKD_Yyksho11.YoyaKbnSeq        =       eVPM_YoyKbn11.YoyaKbnSeq 
            LEFT JOIN   VPM_YoyaKbnSort AS      VPM_YoyaKbnSort11 
                                        ON      eVPM_YoyKbn11.YoyaKbnSeq       = VPM_YoyaKbnSort11.YoyaKbnSeq 
                                        AND    VPM_YoyaKbnSort11.TenantCdSeq      = @TenantCdSeq
            INNER JOIN   VPM_Tokisk     AS      eVPM_Tokisk11 
                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_Tokisk11.TokuiSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk11.SiyoStaYmd 
                                                                                AND     eVPM_Tokisk11.SiyoEndYmd 
            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya11 
                                        ON      eVPM_Tokisk11.GyosyaCdSeq       =       eVPM_Gyosya11.GyosyaCdSeq 
             JOIN   VPM_TokiSt          AS      eVPM_TokiSt11 
                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_TokiSt11.TokuiSeq 
                                        AND     eTKD_Yyksho11.SitenCdSeq        =       eVPM_TokiSt11.SitenCdSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt11.SiyoStaYmd 
                                                                                AND     eVPM_TokiSt11.SiyoEndYmd 
            LEFT JOIN   VPM_Tokisk      AS      eVPM_Tokisk12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_Tokisk12.TokuiSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk12.SiyoStaYmd 
                                                                                AND     eVPM_Tokisk12.SiyoEndYmd 
            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya12 
                                        ON      eVPM_Tokisk12.GyosyaCdSeq       =       eVPM_Gyosya12.GyosyaCdSeq 
            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq 
                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd 
                                                                                AND     eVPM_TokiSt12.SiyoEndYmd 
            LEFT JOIN   TKD_FutTum      AS      eTKD_FutTum11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_FutTum11.UkeNo 
                                        AND     TKD_Mishum.FutuUnkRen           =       eTKD_FutTum11.UnkRen 
                                        AND     TKD_Mishum.FutTumRen            =       eTKD_FutTum11.FutTumRen 
                                        AND     TKD_Mishum.SeiFutSyu            <>      1 
                                        AND     eTKD_FutTum11.SiyoKbn           =       1 
                                        AND (   (   TKD_Mishum.SeiFutSyu        =       6 
                                                AND eTKD_FutTum11.FutTumKbn     =       2 
                                                ) 
                                            OR  (   TKD_Mishum.SeiFutSyu        <>      6 
                                                AND eTKD_FutTum11.FutTumKbn     =       1 
                                                ) 
                                            ) 
            LEFT JOIN   eTKD_Unkobi01   AS      eTKD_Unkobi11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_Unkobi11.UkeNo 
                                        AND     eTKD_Unkobi11.RowNumbr          =       1 
            LEFT JOIN   TKD_Unkobi		 AS      eTKD_Unkobi12 
                                        ON      eTKD_FutTum11.UkeNo             =       eTKD_Unkobi12.UkeNo 
                                        AND     eTKD_FutTum11.UnkRen			 =       eTKD_Unkobi12.UnkRen 
                                        AND     eTKD_Unkobi12.SiyoKbn           =       1 
WHERE (ISNULL(@SeiOutSyKbn,1) = 1 
AND (@SeikYm = '' OR eTKD_Yyksho11.SeikYm = @SeikYm)
AND (@StaUkeNo = '' OR TKD_Mishum.UkeNo >= @StaUkeNo)
AND (@EndUkeNo = '' OR TKD_Mishum.UkeNo <= @EndUkeNo)
AND (@StaYoyaKbn = 0 OR eVPM_YoyKbn11.YoyaKbn >= @StaYoyaKbn)
AND (@EndYoyaKbn = 0 OR eVPM_YoyKbn11.YoyaKbn <= @EndYoyaKbn)
AND (@SeiEigCdSeq = 0 OR eTKD_Yyksho11.SeiEigCdSeq = @SeiEigCdSeq)
AND (@StartBillAdd = '' OR COALESCE(@StartBillAdd, '') <= CASE WHEN ISNULL(@SeiSitKbn,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,'000'),FORMAT(eVPM_TokiSk12.TokuiCd,'0000'),FORMAT(eVPM_TokiSt12.SitenCd,'0000'))
						WHEN ISNULL(@SeiSitKbn,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,'000'),FORMAT(eVPM_TokiSk11.TokuiCd,'0000'),FORMAT(eVPM_TokiSt11.SitenCd,'0000')) END)
AND (@EndBillAdd = '' OR COALESCE(@EndBillAdd, '') >= CASE WHEN ISNULL(@SeiSitKbn,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,'000'),FORMAT(eVPM_TokiSk12.TokuiCd,'0000'),FORMAT(eVPM_TokiSt12.SitenCd,'0000'))
						WHEN ISNULL(@SeiSitKbn,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,'000'),FORMAT(eVPM_TokiSk11.TokuiCd,'0000'),FORMAT(eVPM_TokiSt11.SitenCd,'0000')) END)
AND (@SimeD = 0 OR eVPM_TokiSt12.SimeD = @SimeD))
						
OR (ISNULL(@SeiOutSyKbn,1)	= 2 
AND (@StaUkeNo = '' OR TKD_Mishum.UkeNo >= @StaUkeNo)
AND (@EndUkeNo = '' OR TKD_Mishum.UkeNo <= @EndUkeNo)
AND (@SeiEigCdSeq = 0 OR eTKD_Yyksho11.SeiEigCdSeq = @SeiEigCdSeq)) 
OR (ISNULL(@SeiOutSyKbn,1)	= 3 AND (@OutDataTable = '' OR CONCAT(TKD_Mishum.UkeNo, TKD_Mishum.SeiFutSyu, TKD_Mishum.FutuUnkRen, TKD_Mishum.FutTumRen)
						IN (Select * from  STRING_SPLIT(@OutDataTable, ','))))) 
, 
eTKD_Mishum02    AS 
( 
    SELECT 
        DENSE_RANK()    OVER    (   ORDER BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                )                   AS      SeiRen 
   ,   DENSE_RANK()    OVER    (   PARTITION BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                    ORDER BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                                   ,       eTKD_Mishum01.SeiTaiYmd 
                                                   ,       eTKD_Mishum01.UkeNo 
                                                   ,       eTKD_Mishum01.SeiFutSyu_Sort 
                                                   ,       eTKD_Mishum01.FutTum_Sort 
                                                   ,       eTKD_Mishum01.HasYmd 
                                                   ,       eTKD_Mishum01.SeiFutSyu 
                                                   ,       eTKD_Mishum01.MisyuRen 
                                )                   AS      SeiMeiRen 
   ,   eTKD_Mishum01.* 
FROM 
        eTKD_Mishum01 
) 
,
 eTKD_Kuri01 AS 
(
 SELECT 
	 ISNULL(eTKD_Kuri.SeinKbn,0) AS SeinKbn
	,ISNULL(eTKD_Kuri.YoyaKbn,0) AS YoyaKbn
	,ISNULL(eTKD_Kuri.SyoriYm,'') AS SyoriYm
	,ISNULL(eTKD_Kuri.SyoEigyoSeq,0) AS SyoEigyoSeq
	,ISNULL(eTKD_Kuri.SyoTokuiSeq,0) AS SyoTokuiSeq
	,ISNULL(eTKD_Kuri.SyoSitenSeq,0) AS SyoSitenSeq
	,SUM(ISNULL(eTKD_Kuri.KuriKin,0)) AS KuriKin
 FROM TKD_Kuri eTKD_Kuri
 WHERE eTKD_Kuri.SyoEigyoSeq = @SeiEigCdSeq 
 AND eTKD_Kuri.SeinKbn = 2 
 AND eTKD_Kuri.YoyaKbn = 0 
 AND eTKD_Kuri.SiyoKbn = 1 
 AND eTKD_Kuri.SyoriYm = @SeikYm 
 AND (@BillingType = '' OR eTKD_Kuri.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
 Group By eTKD_Kuri.SeinKbn,eTKD_Kuri.YoyaKbn,eTKD_Kuri.SyoriYm,eTKD_Kuri.SyoEigyoSeq,eTKD_Kuri.SyoTokuiSeq
,eTKD_Kuri.SyoSitenSeq
) 
,
 eTKD_Nyukin01 AS 
(
 SELECT DISTINCT
 eVPM_TokiSt12.TokuiSeq								AS		TokuiSeq
,eVPM_TokiSt12.SitenCdSeq								AS		SitenCdSeq
,SUM(eTKD_Nyshmi.Kesg + eTKD_Nyshmi.FurKesG) AS NyukinRui
,eTKD_Nyshmi.UkeNo 
,eTKD_Nyshmi.FutTumRen 
,eTKD_Nyshmi.SeiFutSyu 
,CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,'00')) AS YoyaKbnSort 
 FROM TKD_NyuSih eTKD_NyuSih
			Left Join TKD_NyShmi		AS		eTKD_NyShmi
										ON		eTKD_NyShmi.NyuSihTblSeq=eTKD_NyuSih.NyuSihTblSeq
										AND		eTKD_NyShmi.SiyoKbn			=1
										and		eTKD_NyuSih.NyuSihKbn = 1 and eTKD_NyShmi.NyuSihKbn = 1
			Left Join TKD_Yyksho		AS		eTKD_Yyksho
										ON		eTKD_Yyksho.UkeNo			= eTKD_NyShmi.UkeNo
										AND		eTKD_Yyksho.SiyoKbn			= 1
           INNER JOIN   VPM_TokiSt     AS      eVPM_TokiSt11 
                                       ON      eTKD_Yyksho.TokuiSeq        =       eVPM_TokiSt11.TokuiSeq 
                                       AND     eTKD_Yyksho.SitenCdSeq      =       eVPM_TokiSt11.SitenCdSeq 
                                       AND     eTKD_Yyksho.SeiTaiYmd      BETWEEN  eVPM_TokiSt11.SiyoStaYmd 
                                                                           AND     eVPM_TokiSt11.SiyoEndYmd 
            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq 
                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq 
                                        AND     eTKD_Yyksho.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd 
																				AND		eVPM_TokiSt12.SiyoEndYmd 
	         LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn13 ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn13.YoyaKbnSeq 
            LEFT JOIN VPM_YoyaKbnSort AS VPM_YoyaKbnSort14 ON eVPM_YoyKbn13.YoyaKbnSeq = VPM_YoyaKbnSort14.YoyaKbnSeq AND VPM_YoyaKbnSort14.TenantCdSeq = @TenantCdSeq
 WHERE (eTKD_Yyksho.SeikYm = '' OR ((eTKD_Yyksho.SeikYm <= @SeikYm AND eTKD_NyuSih.NyuSihYmd BETWEEN (CASE WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,(@SeikYm)) + '01' 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR,@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,(@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))))), 112) 																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,(@SeikYm)) + '01'),112) END END ) 																																										
												AND (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,DATEADD(MONTH,1,CONVERT(VARCHAR,(@SeikYm)) + '01')-1,112) 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2)) 																																										
												ELSE CONVERT(VARCHAR, DATEADD(DAY, - 1, DATEADD(MONTH, 1, CONVERT(DATE, @SeikYm + '01'))), 112)  END END ))

												OR (eTKD_Yyksho.SeikYm = @SeikYm AND eTKD_NyuSih.NyuSihYmd < (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,(@SeikYm)) + '01' 																																										
												ELSE CASE 																																										
												WHEN ISDATE(CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,(@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))))), 112)																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,(@SeikYm)) + '01'),112) END END ))
												AND eTKD_Nyusih.NyuSihKbn = 1 AND eTKD_Nyusih.SiyoKbn = 1))
 AND (@BillingType = '' OR eTKD_NyShmi.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
 AND (@StaUkeNo = '' OR eTKD_Yyksho.UkeNo >= @StaUkeNo)
 AND (@EndUkeNo = '' OR eTKD_Yyksho.UkeNo <= @EndUkeNo)
 AND (@StaYoyaKbn = 0 OR eVPM_YoyKbn13.YoyaKbn >= @StaYoyaKbn)
 AND (@EndYoyaKbn = 0 OR eVPM_YoyKbn13.YoyaKbn <= @EndYoyaKbn)
Group By eVPM_TokiSt12.TokuiSeq,
eVPM_TokiSt12.SitenCdSeq,
eTKD_Nyshmi.UkeNo,
eTKD_Nyshmi.SeiFutSyu,
eTKD_Nyshmi.FutTumRen,
 CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,'00'))
)
INSERT  INTO 
        @Temp_TKD_Seikyu 
SELECT 
        @SeiOutSeq									AS      SeiOutSeq 
    ,   ISNULL(eTKD_Mishum02.SeiRen, 0)             AS      SeiRen 
    ,   ISNULL(eTKD_Mishum02.TokuiSeq, 0)           AS      TokuiSeq 
    ,   ISNULL(eTKD_Mishum02.SitenCdSeq, 0)         AS      SitenCdSeq 
    ,   ISNULL(eTKD_Mishum02.SiyoEndYmd, '')        AS      SiyoEndYmd 
    ,   ISNULL(@SeikYm, '')							AS      SeikYm 
    ,   ISNULL(eTKD_Kuri01.KuriKin,0)		        AS      ZenKurG 
    ,   SUM(eTKD_Mishum02.UriGakKin)                AS      KonUriG 
    ,   SUM(eTKD_Mishum02.SyaRyoSyo)                AS      KonSyoG 
    ,   SUM(eTKD_Mishum02.SyaRyoTes)                AS      KonTesG
	
	,	CASE WHEN @SeiOutSyKbn = 1 THEN (CASE WHEN @KuriSyoriKbn = 1 THEN ISNULL(eTKD_Nyukin02.NyuKinRui,0) 
											  WHEN @KuriSyoriKbn = 2 THEN SUM(eTKD_Mishum02.NyuKinRui) END)
			 WHEN (@SeiOutSyKbn = 2 or @SeiOutSyKbn = 3) THEN SUM(eTKD_Mishum02.NyuKinRui) END AS KouNyuG 
   ,   CASE WHEN @SeiOutSyKbn = 1 THEN (CASE WHEN @KuriSyoriKbn = 1 THEN SUM(eTKD_Mishum02.SeiKin) - ISNULL(eTKD_Nyukin02.NyuKinRui,0) 
											  WHEN @KuriSyoriKbn = 2 THEN SUM(eTKD_Mishum02.SeiKin) - SUM(eTKD_Mishum02.NyuKinRui) END)
			 WHEN (@SeiOutSyKbn = 2 or @SeiOutSyKbn = 3) THEN SUM(eTKD_Mishum02.SeiKin) - SUM(eTKD_Mishum02.NyuKinRui) END AS KonSeiG 
   ,   1                                           AS      SiyoKbn 
   ,   @UpdYmd										AS      UpdYmd 
   ,   @UpdTime									AS      UpdTime 
   ,   @UpdSyainCd									AS      UpdSyainCd 
   ,   @UpdPrgID									AS      UpdPrgID 
FROM 
        eTKD_Mishum02 
 Left JOIn eTKD_Kuri01
 ON eTKD_Kuri01.SyoTokuiSeq = eTKD_Mishum02.TokuiSeq
 AND eTKD_Kuri01.SyoSitenSeq = eTKD_Mishum02.SitenCdSeq
 LEFT JOIN  
		(SELECT 
			 eTKD_Nyukin01.TokuiSeq AS TokuiSeq 
			,eTKD_Nyukin01.SitenCdSeq AS SitenCdSeq 
			,SUM(eTKD_Nyukin01.NyukinRui) AS NyukinRui 
		 FROM	eTKD_Nyukin01 
		 GROUP  BY TokuiSeq,SitenCdSeq)  AS eTKD_Nyukin02 
	ON eTKD_Nyukin02.TokuiSeq = eTKD_Mishum02.TokuiSeq
	AND eTKD_Nyukin02.SitenCdSeq = eTKD_Mishum02.SitenCdSeq 
GROUP BY 
        eTKD_Mishum02.SeiRen 
   ,   eTKD_Mishum02.TokuiSeq 
   ,   eTKD_Mishum02.SitenCdSeq 
   ,   eTKD_Mishum02.SiyoEndYmd 
	,	 eTKD_Kuri01.KuriKin 
	,	 eTKD_Nyukin02.NyukinRui 
ORDER BY 
        eTKD_Mishum02.SeiRen
END		


------ TKD_SeiMei
		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
			OR	ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
WITH 
eTKD_Unkobi01   AS 
( 
    SELECT 
            TKD_Unkobi.UkeNo                            AS      UkeNo 
       ,   TKD_Unkobi.UnkRen                           AS      UnkRen 
       ,   TKD_Unkobi.HaiSYmd                          AS      HaiSYmd 
       ,   TKD_Unkobi.TouYmd                           AS      TouYmd 
       ,   TKD_Unkobi.IkNm							 AS      IkNm 
       ,   TKD_Unkobi.DanTaNm							 AS      DanTaNm 
       ,   ROW_NUMBER()    OVER    (   PARTITION BY 
                                                                TKD_Unkobi.UkeNo 
                                        ORDER BY 
                                                                TKD_Unkobi.UkeNo 
                                                       ,       TKD_Unkobi.UnkRen 
                                    )                   AS      RowNumbr 
    FROM 
            TKD_Unkobi 
    WHERE 
            TKD_Unkobi.SiyoKbn                          =       1 
) 
, 
eTKD_SeiMei01   AS 
( 
    SELECT  DISTINCT 
            TKD_SeiMei.UkeNo                            AS      UkeNo 
       ,   TKD_SeiMei.MisyuRen                         AS      MisyuRen 
       ,   2                                           AS      SeiSaHKbn 
    FROM 
            TKD_SeiMei 
    WHERE 
            TKD_SeiMei.SiyoKbn                          =       1 
) 
, 
eTKD_Mishum01   AS 
( 
    SELECT 
            TKD_Mishum.UkeNo                            AS      UkeNo 
       ,   TKD_Mishum.MisyuRen                         AS      MisyuRen 
       ,   TKD_Mishum.HenKai                           AS      HenKai 
       ,   TKD_Mishum.SeiFutSyu                        AS      SeiFutSyu 
       ,   TKD_Mishum.UriGakKin                        AS      UriGakKin 
       ,   TKD_Mishum.SyaRyoSyo                        AS      SyaRyoSyo 
       ,   TKD_Mishum.SyaRyoTes                        AS      SyaRyoTes 
       ,   TKD_Mishum.SeiKin                           AS      SeiKin 
       ,   TKD_Mishum.NyuKinRui                        AS      NyuKinRui 
       ,   TKD_Mishum.FutuUnkRen                       AS      FutuUnkRen 
       ,   TKD_Mishum.FutTumRen                        AS      FutTumRen 
       ,   eTKD_Yyksho11.SeikYm                        AS      SeikYm
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_Gyosya12.GyosyaCd 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_Gyosya11.GyosyaCd END						AS GyosyaCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_Tokisk12.TokuiCd
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_Tokisk11.TokuiCd END                      AS TokuiCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt11.SeiCdSeq 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eTKD_Yyksho11.TokuiSeq END                     AS TokuiSeq 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SitenCd 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SitenCd END                      AS SitenCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt11.SeiSitenCdSeq 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eTKD_Yyksho11.SitenCdSeq END					AS SitenCdSeq 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SiyoEndYmd
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SiyoEndYmd END                   AS SiyoEndYmd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SimeD
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SimeD END                        AS SimeD 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.DanTaNm   ,eTKD_Yyksho11.YoyaNm) 
                ELSE    ISNULL(eTKD_Unkobi12.DanTaNm   ,eTKD_Yyksho11.YoyaNm) 
            END                                         AS      YoyaNm 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Yyksho11.SeiTaiYmd ,       ' ') 
                ELSE    ISNULL(eTKD_FutTum11.HasYmd    ,       ' ') 
            END                                         AS      HasYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.IkNm      ,       ' ') 
                ELSE    ISNULL(eTKD_FutTum11.FutTumNm  ,       ' ') 
            END                                         AS      FutTumNm 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) 
                THEN    0 
                ELSE    ISNULL(eTKD_FutTum11.Suryo     ,       0) 
            END                                         AS      Suryo 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) 
                THEN    0 
                ELSE    ISNULL(eTKD_FutTum11.TanKa     ,       0) 
            END                                         AS      TanKa 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.HaiSYmd   ,       ' ') 
                ELSE    ' ' 
            END                                         AS      HaiSYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.TouYmd    ,       ' ') 
                ELSE    ' ' 
            END                                         AS      TouYmd 
       ,   eTKD_Yyksho11.YoyaKbnSeq                    AS      YoyaKbnSeq 
       ,   CONCAT(CASE WHEN VPM_YoyaKbnSort11.PriorityNum IS NULL THEN '99' ELSE FORMAT(VPM_YoyaKbnSort11.PriorityNum, '00') END 
       ,   FORMAT(eVPM_YoyKbn11.YoyaKbn,'00'))     AS YoyaKbnSort 
       ,   eTKD_Yyksho11.SeiEigCdSeq                   AS      SeiEigCdSeq 
       ,   eTKD_Yyksho11.SeiTaiYmd                     AS      SeiTaiYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    1 
                ELSE    2 
            END                                         AS      SeiFutSyu_Sort 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ' ' 
                ELSE    SUBSTRING(ISNULL(eTKD_FutTum11.ExpItem	,' ')	,1,3) 
            END                                         AS      FutTum_Sort 
		,	CASE 
				WHEN	TKD_Mishum.SeiFutSyu			 IN		(1,7)	
				THEN	eTKD_Yyksho11.Zeiritsu
				ELSE	eTKD_FutTum11.Zeiritsu
			END											 AS		Zeiritsu
    FROM 
            TKD_Mishum 
            JOIN        TKD_Yyksho      AS      eTKD_Yyksho11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_Yyksho11.UkeNo 
                                        AND     TKD_Mishum.SiyoKbn              =       1 
                                        AND     eTKD_Yyksho11.SiyoKbn           =       1 
                                        AND (   (   TKD_Mishum.SeiFutSyu        =       7 
                                                AND eTKD_Yyksho11.YoyaSyu       =       2 
                                                ) 
                                            OR  (   TKD_Mishum.SeiFutSyu        <>      7 
                                                AND eTKD_Yyksho11.YoyaSyu       =       1 
                                                ) 
                                            ) 
            LEFT JOIN   VPM_YoyKbn      AS      eVPM_YoyKbn11 
                                        ON      eTKD_Yyksho11.YoyaKbnSeq        =       eVPM_YoyKbn11.YoyaKbnSeq 
            LEFT JOIN   VPM_YoyaKbnSort AS      VPM_YoyaKbnSort11 
                                        ON      eVPM_YoyKbn11.YoyaKbnSeq       = VPM_YoyaKbnSort11.YoyaKbnSeq 
                                        AND    VPM_YoyaKbnSort11.TenantCdSeq      = @TenantCdSeq
            INNER JOIN   VPM_Tokisk     AS      eVPM_Tokisk11 
                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_Tokisk11.TokuiSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk11.SiyoStaYmd 
                                                                                AND     eVPM_Tokisk11.SiyoEndYmd 
            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya11 
                                        ON      eVPM_Tokisk11.GyosyaCdSeq       =       eVPM_Gyosya11.GyosyaCdSeq 
             JOIN   VPM_TokiSt          AS      eVPM_TokiSt11 
                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_TokiSt11.TokuiSeq 
                                        AND     eTKD_Yyksho11.SitenCdSeq        =       eVPM_TokiSt11.SitenCdSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt11.SiyoStaYmd 
                                                                                AND     eVPM_TokiSt11.SiyoEndYmd 
            LEFT JOIN   VPM_Tokisk      AS      eVPM_Tokisk12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_Tokisk12.TokuiSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk12.SiyoStaYmd 
                                                                                AND     eVPM_Tokisk12.SiyoEndYmd 
            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya12 
                                        ON      eVPM_Tokisk12.GyosyaCdSeq       =       eVPM_Gyosya12.GyosyaCdSeq 
            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq 
                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd 
                                                                                AND     eVPM_TokiSt12.SiyoEndYmd 
            LEFT JOIN   TKD_FutTum      AS      eTKD_FutTum11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_FutTum11.UkeNo 
                                        AND     TKD_Mishum.FutuUnkRen           =       eTKD_FutTum11.UnkRen 
                                        AND     TKD_Mishum.FutTumRen            =       eTKD_FutTum11.FutTumRen 
                                        AND     TKD_Mishum.SeiFutSyu            <>      1 
                                        AND     eTKD_FutTum11.SiyoKbn           =       1 
                                        AND (   (   TKD_Mishum.SeiFutSyu        =       6 
                                                AND eTKD_FutTum11.FutTumKbn     =       2 
                                                ) 
                                            OR  (   TKD_Mishum.SeiFutSyu        <>      6 
                                                AND eTKD_FutTum11.FutTumKbn     =       1 
                                                ) 
                                            ) 
            LEFT JOIN   eTKD_Unkobi01   AS      eTKD_Unkobi11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_Unkobi11.UkeNo 
                                        AND     eTKD_Unkobi11.RowNumbr          =       1 
            LEFT JOIN   TKD_Unkobi		 AS      eTKD_Unkobi12 
                                        ON      eTKD_FutTum11.UkeNo             =       eTKD_Unkobi12.UkeNo 
                                        AND     eTKD_FutTum11.UnkRen			 =       eTKD_Unkobi12.UnkRen 
                                        AND     eTKD_Unkobi12.SiyoKbn           =       1 
WHERE (ISNULL(@SeiOutSyKbn,1) = 1 
AND (@SeikYm = '' OR eTKD_Yyksho11.SeikYm = @SeikYm)
AND (@StaUkeNo = '' OR TKD_Mishum.UkeNo >= @StaUkeNo)
AND (@EndUkeNo = '' OR TKD_Mishum.UkeNo <= @EndUkeNo)
AND (@StaYoyaKbn = 0 OR eVPM_YoyKbn11.YoyaKbn >= @StaYoyaKbn)
AND (@EndYoyaKbn = 0 OR eVPM_YoyKbn11.YoyaKbn <= @EndYoyaKbn)
AND (@SeiEigCdSeq = 0 OR eTKD_Yyksho11.SeiEigCdSeq = @SeiEigCdSeq)
AND (@StartBillAdd = '' OR COALESCE(@StartBillAdd, '') <= CASE WHEN ISNULL(@SeiSitKbn,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,'000'),FORMAT(eVPM_TokiSk12.TokuiCd,'0000'),FORMAT(eVPM_TokiSt12.SitenCd,'0000'))
						WHEN ISNULL(@SeiSitKbn,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,'000'),FORMAT(eVPM_TokiSk11.TokuiCd,'0000'),FORMAT(eVPM_TokiSt11.SitenCd,'0000')) END)
AND (@EndBillAdd = '' OR COALESCE(@EndBillAdd, '') >= CASE WHEN ISNULL(@SeiSitKbn,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,'000'),FORMAT(eVPM_TokiSk12.TokuiCd,'0000'),FORMAT(eVPM_TokiSt12.SitenCd,'0000'))
						WHEN ISNULL(@SeiSitKbn,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,'000'),FORMAT(eVPM_TokiSk11.TokuiCd,'0000'),FORMAT(eVPM_TokiSt11.SitenCd,'0000')) END)
AND (@SimeD = 0 OR eVPM_TokiSt12.SimeD = @SimeD))
						
OR (ISNULL(@SeiOutSyKbn,1)	= 2 
AND (@StaUkeNo = '' OR TKD_Mishum.UkeNo >= @StaUkeNo)
AND (@EndUkeNo = '' OR TKD_Mishum.UkeNo <= @EndUkeNo)
AND (@SeiEigCdSeq = 0 OR eTKD_Yyksho11.SeiEigCdSeq = @SeiEigCdSeq)) 
OR (ISNULL(@SeiOutSyKbn,1)	= 3 AND (@OutDataTable = '' OR CONCAT(TKD_Mishum.UkeNo, TKD_Mishum.SeiFutSyu, TKD_Mishum.FutuUnkRen, TKD_Mishum.FutTumRen)
						IN (Select * from  STRING_SPLIT(@OutDataTable, ','))))) 
, 
eTKD_Mishum02    AS 
( 
    SELECT 
        DENSE_RANK()    OVER    (   ORDER BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                )                   AS      SeiRen 
   ,   DENSE_RANK()    OVER    (   PARTITION BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                    ORDER BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                                   ,       eTKD_Mishum01.SeiTaiYmd 
                                                   ,       eTKD_Mishum01.UkeNo 
                                                   ,       eTKD_Mishum01.SeiFutSyu_Sort 
                                                   ,       eTKD_Mishum01.FutTum_Sort 
                                                   ,       eTKD_Mishum01.HasYmd 
                                                   ,       eTKD_Mishum01.SeiFutSyu 
                                                   ,       eTKD_Mishum01.MisyuRen 
                                )                   AS      SeiMeiRen 
   ,   eTKD_Mishum01.* 
FROM 
        eTKD_Mishum01 
) 
,
 eTKD_Kuri01 AS 
(
 SELECT 
	 ISNULL(eTKD_Kuri.SeinKbn,0) AS SeinKbn
	,ISNULL(eTKD_Kuri.YoyaKbn,0) AS YoyaKbn
	,ISNULL(eTKD_Kuri.SyoriYm,'') AS SyoriYm
	,ISNULL(eTKD_Kuri.SyoEigyoSeq,0) AS SyoEigyoSeq
	,ISNULL(eTKD_Kuri.SyoTokuiSeq,0) AS SyoTokuiSeq
	,ISNULL(eTKD_Kuri.SyoSitenSeq,0) AS SyoSitenSeq
	,SUM(ISNULL(eTKD_Kuri.KuriKin,0)) AS KuriKin
 FROM TKD_Kuri eTKD_Kuri
 WHERE eTKD_Kuri.SyoEigyoSeq = @SeiEigCdSeq 
 AND eTKD_Kuri.SeinKbn = 2 
 AND eTKD_Kuri.YoyaKbn = 0 
 AND eTKD_Kuri.SiyoKbn = 1 
 AND eTKD_Kuri.SyoriYm = @SeikYm 
 AND (@BillingType = '' OR eTKD_Kuri.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
 Group By eTKD_Kuri.SeinKbn,eTKD_Kuri.YoyaKbn,eTKD_Kuri.SyoriYm,eTKD_Kuri.SyoEigyoSeq,eTKD_Kuri.SyoTokuiSeq
,eTKD_Kuri.SyoSitenSeq
) 
,
 eTKD_Nyukin01 AS 
(
 SELECT 
 eVPM_TokiSt12.TokuiSeq								AS		TokuiSeq
,eVPM_TokiSt12.SitenCdSeq								AS		SitenCdSeq
,SUM(eTKD_Nyshmi.Kesg + eTKD_Nyshmi.FurKesG) AS NyukinRui
,eTKD_Nyshmi.UkeNo 
,eTKD_Nyshmi.FutTumRen 
,eTKD_Nyshmi.SeiFutSyu 
,CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,'00')) AS YoyaKbnSort 
 FROM TKD_NyuSih eTKD_NyuSih
			Left Join TKD_NyShmi		AS		eTKD_NyShmi
										ON		eTKD_NyShmi.NyuSihTblSeq=eTKD_NyuSih.NyuSihTblSeq
										AND		eTKD_NyShmi.SiyoKbn			=1
										and		eTKD_NyuSih.NyuSihKbn = 1 and eTKD_NyShmi.NyuSihKbn = 1
			Left Join TKD_Yyksho		AS		eTKD_Yyksho
										ON		eTKD_Yyksho.UkeNo			= eTKD_NyShmi.UkeNo
										AND		eTKD_Yyksho.SiyoKbn			= 1
           INNER JOIN   VPM_TokiSt     AS      eVPM_TokiSt11 
                                       ON      eTKD_Yyksho.TokuiSeq        =       eVPM_TokiSt11.TokuiSeq 
                                       AND     eTKD_Yyksho.SitenCdSeq      =       eVPM_TokiSt11.SitenCdSeq 
                                       AND     eTKD_Yyksho.SeiTaiYmd      BETWEEN  eVPM_TokiSt11.SiyoStaYmd 
                                                                           AND     eVPM_TokiSt11.SiyoEndYmd 
            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq 
                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq 
                                        AND     eTKD_Yyksho.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd 
																				AND		eVPM_TokiSt12.SiyoEndYmd 
	         LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn13 ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn13.YoyaKbnSeq 
            LEFT JOIN VPM_YoyaKbnSort AS VPM_YoyaKbnSort14 ON eVPM_YoyKbn13.YoyaKbnSeq = VPM_YoyaKbnSort14.YoyaKbnSeq AND VPM_YoyaKbnSort14.TenantCdSeq = @TenantCdSeq
 WHERE (eTKD_Yyksho.SeikYm = '' OR ((eTKD_Yyksho.SeikYm <= @SeikYm AND eTKD_NyuSih.NyuSihYmd BETWEEN (CASE WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,(@SeikYm)) + '01' 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR,@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,(@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))))), 112) 																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,(@SeikYm)) + '01'),112) END END ) 																																										
												AND (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,DATEADD(MONTH,1,CONVERT(VARCHAR,(@SeikYm)) + '01')-1,112) 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2)) 																																										
												ELSE CONVERT(VARCHAR, DATEADD(DAY, - 1, DATEADD(MONTH, 1, CONVERT(DATE, @SeikYm + '01'))), 112)  END END ))

												OR (eTKD_Yyksho.SeikYm = @SeikYm AND eTKD_NyuSih.NyuSihYmd < (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,(@SeikYm)) + '01' 																																										
												ELSE CASE 																																										
												WHEN ISDATE(CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,(@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))))), 112)																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,(@SeikYm)) + '01'),112) END END ))
												AND eTKD_Nyusih.NyuSihKbn = 1 AND eTKD_Nyusih.SiyoKbn = 1))
 AND (@BillingType = '' OR eTKD_NyShmi.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
 AND (@StaUkeNo = '' OR eTKD_Yyksho.UkeNo >= @StaUkeNo)
 AND (@EndUkeNo = '' OR eTKD_Yyksho.UkeNo <= @EndUkeNo)
 AND (@StaYoyaKbn = 0 OR eVPM_YoyKbn13.YoyaKbn >= @StaYoyaKbn)
 AND (@EndYoyaKbn = 0 OR eVPM_YoyKbn13.YoyaKbn <= @EndYoyaKbn)
Group By eVPM_TokiSt12.TokuiSeq,
eVPM_TokiSt12.SitenCdSeq,
eTKD_Nyshmi.UkeNo,
eTKD_Nyshmi.SeiFutSyu,
eTKD_Nyshmi.FutTumRen,
 CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,'00'))
)  
INSERT  INTO 
        @Temp_TKD_SeiMei 
SELECT 
        @SeiOutSeq									AS      SeiOutSeq 
    ,   ISNULL(eTKD_Mishum02.SeiRen, 0)                        AS      SeiRen 
    ,   ISNULL(eTKD_Mishum02.SeiMeiRen, 0)                     AS      SeiMeiRen 
    ,   ISNULL(eTKD_Mishum02.UkeNo, '')                         AS      UkeNo 
    ,   eTKD_Mishum02.MisyuRen                      AS      MisyuRen 
    ,   eTKD_Mishum02.UriGakKin                     AS      UriGakKin 
    ,   eTKD_Mishum02.SyaRyoSyo                     AS      SyaRyoSyo 
    ,   eTKD_Mishum02.SyaRyoTes                     AS      SyaRyoTes 
    ,   eTKD_Mishum02.SeiKin                        AS      SeiKin 
    ,   CASE WHEN @KuriSyoriKbn = 1 THEN ISNULL(eTKD_Nyukin01.NyuKinRui,0)
			 WHEN @KuriSyoriKbn = 2 THEN eTKD_Mishum02.NyuKinRui END AS NyuKinRui 
    ,   ISNULL(eTKD_SeiMei11.SeiSaHKbn  ,1)         AS      SeiSaHKbn 
	,	ISNULL(eTKD_Mishum02.Zeiritsu, 0)			AS		Zeiritsu
    ,   1                                           AS      SiyoKbn 
    ,   @UpdYmd										AS      UpdYmd 
    ,   @UpdTime									AS      UpdTime 
    ,   @UpdSyainCd									AS      UpdSyainCd 
    ,   @UpdPrgID									AS      UpdPrgID 
FROM 
        eTKD_Mishum02 
        LEFT JOIN   eTKD_SeiMei01   AS      eTKD_SeiMei11 
                                    ON      eTKD_Mishum02.UkeNo         =       eTKD_SeiMei11.UkeNo 
                                    AND     eTKD_Mishum02.MisyuRen      =       eTKD_SeiMei11.MisyuRen 
       LEFT JOIN eTKD_Nyukin01 
       	ON	eTKD_Nyukin01.TokuiSeq     =    eTKD_Mishum02.TokuiSeq 
       	AND	eTKD_Nyukin01.SitenCdSeq   =    eTKD_Mishum02.SitenCdSeq 
       	AND	eTKD_Nyukin01.UkeNo        =    eTKD_Mishum02.UkeNo 
       	AND	eTKD_Nyukin01.SeiFutSyu    =    eTKD_Mishum02.SeiFutSyu 
       	AND eTKD_Nyukin01.FutTumRen    =    eTKD_Mishum02.FutTumRen 
ORDER BY 
        eTKD_Mishum02.SeiRen 
   ,   eTKD_Mishum02.SeiMeiRen
END	


--------------------- TKD_SeiUch
		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
			OR	ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN 
WITH 
eTKD_Unkobi01   AS 
( 
    SELECT 
            TKD_Unkobi.UkeNo                            AS      UkeNo 
       ,   TKD_Unkobi.UnkRen                           AS      UnkRen 
       ,   TKD_Unkobi.HaiSYmd                          AS      HaiSYmd 
       ,   TKD_Unkobi.TouYmd                           AS      TouYmd 
       ,   TKD_Unkobi.IkNm							 AS      IkNm 
       ,   TKD_Unkobi.DanTaNm							 AS      DanTaNm 
       ,   ROW_NUMBER()    OVER    (   PARTITION BY 
                                                                TKD_Unkobi.UkeNo 
                                        ORDER BY 
                                                                TKD_Unkobi.UkeNo 
                                                       ,       TKD_Unkobi.UnkRen 
                                    )                   AS      RowNumbr 
    FROM 
            TKD_Unkobi 
    WHERE 
            TKD_Unkobi.SiyoKbn                          =       1 
) 
, 
eTKD_SeiMei01   AS 
( 
    SELECT  DISTINCT 
            TKD_SeiMei.UkeNo                            AS      UkeNo 
       ,   TKD_SeiMei.MisyuRen                         AS      MisyuRen 
       ,   2                                           AS      SeiSaHKbn 
    FROM 
            TKD_SeiMei 
    WHERE 
            TKD_SeiMei.SiyoKbn                          =       1 
) 
, 
eTKD_Mishum01   AS 
( 
    SELECT 
            TKD_Mishum.UkeNo                            AS      UkeNo 
       ,   TKD_Mishum.MisyuRen                         AS      MisyuRen 
       ,   TKD_Mishum.HenKai                           AS      HenKai 
       ,   TKD_Mishum.SeiFutSyu                        AS      SeiFutSyu 
       ,   TKD_Mishum.UriGakKin                        AS      UriGakKin 
       ,   TKD_Mishum.SyaRyoSyo                        AS      SyaRyoSyo 
       ,   TKD_Mishum.SyaRyoTes                        AS      SyaRyoTes 
       ,   TKD_Mishum.SeiKin                           AS      SeiKin 
       ,   TKD_Mishum.NyuKinRui                        AS      NyuKinRui 
       ,   TKD_Mishum.FutuUnkRen                       AS      FutuUnkRen 
       ,   TKD_Mishum.FutTumRen                        AS      FutTumRen 
       ,   eTKD_Yyksho11.SeikYm                        AS      SeikYm
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_Gyosya12.GyosyaCd 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_Gyosya11.GyosyaCd END						AS GyosyaCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_Tokisk12.TokuiCd
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_Tokisk11.TokuiCd END                      AS TokuiCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt11.SeiCdSeq 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eTKD_Yyksho11.TokuiSeq END                     AS TokuiSeq 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SitenCd 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SitenCd END                      AS SitenCd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt11.SeiSitenCdSeq 
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eTKD_Yyksho11.SitenCdSeq END					AS SitenCdSeq 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SiyoEndYmd
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SiyoEndYmd END                   AS SiyoEndYmd 
       ,   CASE WHEN ISNULL(@SeiSitKbn	,1)	= 1 THEN eVPM_TokiSt12.SimeD
				 WHEN ISNULL(@SeiSitKbn	,1)	= 2 THEN eVPM_TokiSt11.SimeD END                        AS SimeD 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.DanTaNm   ,eTKD_Yyksho11.YoyaNm) 
                ELSE    ISNULL(eTKD_Unkobi12.DanTaNm   ,eTKD_Yyksho11.YoyaNm) 
            END                                         AS      YoyaNm 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Yyksho11.SeiTaiYmd ,       ' ') 
                ELSE    ISNULL(eTKD_FutTum11.HasYmd    ,       ' ') 
            END                                         AS      HasYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.IkNm      ,       ' ') 
                ELSE    ISNULL(eTKD_FutTum11.FutTumNm  ,       ' ') 
            END                                         AS      FutTumNm 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) 
                THEN    0 
                ELSE    ISNULL(eTKD_FutTum11.Suryo     ,       0) 
            END                                         AS      Suryo 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) 
                THEN    0 
                ELSE    ISNULL(eTKD_FutTum11.TanKa     ,       0) 
            END                                         AS      TanKa 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.HaiSYmd   ,       ' ') 
                ELSE    ' ' 
            END                                         AS      HaiSYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ISNULL(eTKD_Unkobi11.TouYmd    ,       ' ') 
                ELSE    ' ' 
            END                                         AS      TouYmd 
       ,   eTKD_Yyksho11.YoyaKbnSeq                    AS      YoyaKbnSeq 
       ,   CONCAT(CASE WHEN VPM_YoyaKbnSort11.PriorityNum IS NULL THEN '99' ELSE FORMAT(VPM_YoyaKbnSort11.PriorityNum, '00') END 
       ,   FORMAT(eVPM_YoyKbn11.YoyaKbn,'00'))     AS YoyaKbnSort 
       ,   eTKD_Yyksho11.SeiEigCdSeq                   AS      SeiEigCdSeq 
       ,   eTKD_Yyksho11.SeiTaiYmd                     AS      SeiTaiYmd 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    1 
                ELSE    2 
            END                                         AS      SeiFutSyu_Sort 
       ,   CASE 
                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) 
                THEN    ' ' 
                ELSE    SUBSTRING(ISNULL(eTKD_FutTum11.ExpItem	,' ')	,1,3) 
            END                                         AS      FutTum_Sort 
		,	CASE 
				WHEN	TKD_Mishum.SeiFutSyu			 IN		(1,7)	
				THEN	eTKD_Yyksho11.Zeiritsu
				ELSE	eTKD_FutTum11.Zeiritsu
			END											 AS		Zeiritsu
    FROM 
            TKD_Mishum 
            JOIN        TKD_Yyksho      AS      eTKD_Yyksho11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_Yyksho11.UkeNo 
                                        AND     TKD_Mishum.SiyoKbn              =       1 
                                        AND     eTKD_Yyksho11.SiyoKbn           =       1 
                                        AND (   (   TKD_Mishum.SeiFutSyu        =       7 
                                                AND eTKD_Yyksho11.YoyaSyu       =       2 
                                                ) 
                                            OR  (   TKD_Mishum.SeiFutSyu        <>      7 
                                                AND eTKD_Yyksho11.YoyaSyu       =       1 
                                                ) 
                                            ) 
            LEFT JOIN   VPM_YoyKbn      AS      eVPM_YoyKbn11 
                                        ON      eTKD_Yyksho11.YoyaKbnSeq        =       eVPM_YoyKbn11.YoyaKbnSeq 
            LEFT JOIN   VPM_YoyaKbnSort AS      VPM_YoyaKbnSort11 
                                        ON      eVPM_YoyKbn11.YoyaKbnSeq       = VPM_YoyaKbnSort11.YoyaKbnSeq 
                                        AND    VPM_YoyaKbnSort11.TenantCdSeq      = @TenantCdSeq
            INNER JOIN   VPM_Tokisk     AS      eVPM_Tokisk11 
                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_Tokisk11.TokuiSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk11.SiyoStaYmd 
                                                                                AND     eVPM_Tokisk11.SiyoEndYmd 
            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya11 
                                        ON      eVPM_Tokisk11.GyosyaCdSeq       =       eVPM_Gyosya11.GyosyaCdSeq 
             JOIN   VPM_TokiSt          AS      eVPM_TokiSt11 
                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_TokiSt11.TokuiSeq 
                                        AND     eTKD_Yyksho11.SitenCdSeq        =       eVPM_TokiSt11.SitenCdSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt11.SiyoStaYmd 
                                                                                AND     eVPM_TokiSt11.SiyoEndYmd 
            LEFT JOIN   VPM_Tokisk      AS      eVPM_Tokisk12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_Tokisk12.TokuiSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk12.SiyoStaYmd 
                                                                                AND     eVPM_Tokisk12.SiyoEndYmd 
            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya12 
                                        ON      eVPM_Tokisk12.GyosyaCdSeq       =       eVPM_Gyosya12.GyosyaCdSeq 
            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq 
                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq 
                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd 
                                                                                AND     eVPM_TokiSt12.SiyoEndYmd 
            LEFT JOIN   TKD_FutTum      AS      eTKD_FutTum11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_FutTum11.UkeNo 
                                        AND     TKD_Mishum.FutuUnkRen           =       eTKD_FutTum11.UnkRen 
                                        AND     TKD_Mishum.FutTumRen            =       eTKD_FutTum11.FutTumRen 
                                        AND     TKD_Mishum.SeiFutSyu            <>      1 
                                        AND     eTKD_FutTum11.SiyoKbn           =       1 
                                        AND (   (   TKD_Mishum.SeiFutSyu        =       6 
                                                AND eTKD_FutTum11.FutTumKbn     =       2 
                                                ) 
                                            OR  (   TKD_Mishum.SeiFutSyu        <>      6 
                                                AND eTKD_FutTum11.FutTumKbn     =       1 
                                                ) 
                                            ) 
            LEFT JOIN   eTKD_Unkobi01   AS      eTKD_Unkobi11 
                                        ON      TKD_Mishum.UkeNo                =       eTKD_Unkobi11.UkeNo 
                                        AND     eTKD_Unkobi11.RowNumbr          =       1 
            LEFT JOIN   TKD_Unkobi		 AS      eTKD_Unkobi12 
                                        ON      eTKD_FutTum11.UkeNo             =       eTKD_Unkobi12.UkeNo 
                                        AND     eTKD_FutTum11.UnkRen			 =       eTKD_Unkobi12.UnkRen 
                                        AND     eTKD_Unkobi12.SiyoKbn           =       1 
WHERE (ISNULL(@SeiOutSyKbn,1) = 1 
AND (@SeikYm = '' OR eTKD_Yyksho11.SeikYm = @SeikYm)
AND (@StaUkeNo = '' OR TKD_Mishum.UkeNo >= @StaUkeNo)
AND (@EndUkeNo = '' OR TKD_Mishum.UkeNo <= @EndUkeNo)
AND (@StaYoyaKbn = 0 OR eVPM_YoyKbn11.YoyaKbn >= @StaYoyaKbn)
AND (@EndYoyaKbn = 0 OR eVPM_YoyKbn11.YoyaKbn <= @EndYoyaKbn)
AND (@SeiEigCdSeq = 0 OR eTKD_Yyksho11.SeiEigCdSeq = @SeiEigCdSeq)
AND (@StartBillAdd = '' OR COALESCE(@StartBillAdd, '') <= CASE WHEN ISNULL(@SeiSitKbn,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,'000'),FORMAT(eVPM_TokiSk12.TokuiCd,'0000'),FORMAT(eVPM_TokiSt12.SitenCd,'0000'))
						WHEN ISNULL(@SeiSitKbn,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,'000'),FORMAT(eVPM_TokiSk11.TokuiCd,'0000'),FORMAT(eVPM_TokiSt11.SitenCd,'0000')) END)
AND (@EndBillAdd = '' OR COALESCE(@EndBillAdd, '') >= CASE WHEN ISNULL(@SeiSitKbn,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,'000'),FORMAT(eVPM_TokiSk12.TokuiCd,'0000'),FORMAT(eVPM_TokiSt12.SitenCd,'0000'))
						WHEN ISNULL(@SeiSitKbn,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,'000'),FORMAT(eVPM_TokiSk11.TokuiCd,'0000'),FORMAT(eVPM_TokiSt11.SitenCd,'0000')) END)
AND (@SimeD = 0 OR eVPM_TokiSt12.SimeD = @SimeD))
						
OR (ISNULL(@SeiOutSyKbn,1)	= 2 
AND (@StaUkeNo = '' OR TKD_Mishum.UkeNo >= @StaUkeNo)
AND (@EndUkeNo = '' OR TKD_Mishum.UkeNo <= @EndUkeNo)
AND (@SeiEigCdSeq = 0 OR eTKD_Yyksho11.SeiEigCdSeq = @SeiEigCdSeq)) 
OR (ISNULL(@SeiOutSyKbn,1)	= 3 AND (@OutDataTable = '' OR CONCAT(TKD_Mishum.UkeNo, TKD_Mishum.SeiFutSyu, TKD_Mishum.FutuUnkRen, TKD_Mishum.FutTumRen)
						IN (Select * from  STRING_SPLIT(@OutDataTable, ','))))) 
, 
eTKD_Mishum02    AS 
( 
    SELECT 
        DENSE_RANK()    OVER    (   ORDER BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                )                   AS      SeiRen 
   ,   DENSE_RANK()    OVER    (   PARTITION BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                    ORDER BY 
                                                            eTKD_Mishum01.GyosyaCd 
                                                   ,       eTKD_Mishum01.TokuiCd 
                                                   ,       eTKD_Mishum01.TokuiSeq 
                                                   ,       eTKD_Mishum01.SitenCd 
                                                   ,       eTKD_Mishum01.SitenCdSeq 
                                                   ,       eTKD_Mishum01.SiyoEndYmd 
                                                   ,       eTKD_Mishum01.SeiTaiYmd 
                                                   ,       eTKD_Mishum01.UkeNo 
                                                   ,       eTKD_Mishum01.SeiFutSyu_Sort 
                                                   ,       eTKD_Mishum01.FutTum_Sort 
                                                   ,       eTKD_Mishum01.HasYmd 
                                                   ,       eTKD_Mishum01.SeiFutSyu 
                                                   ,       eTKD_Mishum01.MisyuRen 
                                )                   AS      SeiMeiRen 
   ,   eTKD_Mishum01.* 
FROM 
        eTKD_Mishum01 
) 
,
 eTKD_Kuri01 AS 
(
 SELECT 
	 ISNULL(eTKD_Kuri.SeinKbn,0) AS SeinKbn
	,ISNULL(eTKD_Kuri.YoyaKbn,0) AS YoyaKbn
	,ISNULL(eTKD_Kuri.SyoriYm,'') AS SyoriYm
	,ISNULL(eTKD_Kuri.SyoEigyoSeq,0) AS SyoEigyoSeq
	,ISNULL(eTKD_Kuri.SyoTokuiSeq,0) AS SyoTokuiSeq
	,ISNULL(eTKD_Kuri.SyoSitenSeq,0) AS SyoSitenSeq
	,SUM(ISNULL(eTKD_Kuri.KuriKin,0)) AS KuriKin
 FROM TKD_Kuri eTKD_Kuri
 WHERE eTKD_Kuri.SyoEigyoSeq = @SeiEigCdSeq 
 AND eTKD_Kuri.SeinKbn = 2 
 AND eTKD_Kuri.YoyaKbn = 0 
 AND eTKD_Kuri.SiyoKbn = 1 
 AND eTKD_Kuri.SyoriYm = @SeikYm 
 AND (@BillingType = '' OR eTKD_Kuri.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
 Group By eTKD_Kuri.SeinKbn,eTKD_Kuri.YoyaKbn,eTKD_Kuri.SyoriYm,eTKD_Kuri.SyoEigyoSeq,eTKD_Kuri.SyoTokuiSeq
,eTKD_Kuri.SyoSitenSeq
) 
,
 eTKD_Nyukin01 AS 
(
 SELECT 
 eVPM_TokiSt12.TokuiSeq								AS		TokuiSeq
,eVPM_TokiSt12.SitenCdSeq								AS		SitenCdSeq
,SUM(eTKD_Nyshmi.Kesg + eTKD_Nyshmi.FurKesG) AS NyukinRui
,eTKD_Nyshmi.UkeNo 
,eTKD_Nyshmi.FutTumRen 
,eTKD_Nyshmi.SeiFutSyu 
,CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,'00')) AS YoyaKbnSort 
 FROM TKD_NyuSih eTKD_NyuSih
			Left Join TKD_NyShmi		AS		eTKD_NyShmi
										ON		eTKD_NyShmi.NyuSihTblSeq=eTKD_NyuSih.NyuSihTblSeq
										AND		eTKD_NyShmi.SiyoKbn			=1
										and		eTKD_NyuSih.NyuSihKbn = 1 and eTKD_NyShmi.NyuSihKbn = 1
			Left Join TKD_Yyksho		AS		eTKD_Yyksho
										ON		eTKD_Yyksho.UkeNo			= eTKD_NyShmi.UkeNo
										AND		eTKD_Yyksho.SiyoKbn			= 1
           INNER JOIN   VPM_TokiSt     AS      eVPM_TokiSt11 
                                       ON      eTKD_Yyksho.TokuiSeq        =       eVPM_TokiSt11.TokuiSeq 
                                       AND     eTKD_Yyksho.SitenCdSeq      =       eVPM_TokiSt11.SitenCdSeq 
                                       AND     eTKD_Yyksho.SeiTaiYmd      BETWEEN  eVPM_TokiSt11.SiyoStaYmd 
                                                                           AND     eVPM_TokiSt11.SiyoEndYmd 
            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 
                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq 
                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq 
                                        AND     eTKD_Yyksho.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd 
																				AND		eVPM_TokiSt12.SiyoEndYmd 
	         LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn13 ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn13.YoyaKbnSeq 
            LEFT JOIN VPM_YoyaKbnSort AS VPM_YoyaKbnSort14 ON eVPM_YoyKbn13.YoyaKbnSeq = VPM_YoyaKbnSort14.YoyaKbnSeq AND VPM_YoyaKbnSort14.TenantCdSeq = @TenantCdSeq
 WHERE (eTKD_Yyksho.SeikYm = '' OR ((eTKD_Yyksho.SeikYm <= @SeikYm AND eTKD_NyuSih.NyuSihYmd BETWEEN (CASE WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,(@SeikYm)) + '01' 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR,@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,(@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))))), 112) 																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,(@SeikYm)) + '01'),112) END END ) 																																										
												AND (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,DATEADD(MONTH,1,CONVERT(VARCHAR,(@SeikYm)) + '01')-1,112) 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2)) 																																										
												ELSE CONVERT(VARCHAR, DATEADD(DAY, - 1, DATEADD(MONTH, 1, CONVERT(DATE, @SeikYm + '01'))), 112)  END END ))

												OR (eTKD_Yyksho.SeikYm = @SeikYm AND eTKD_NyuSih.NyuSihYmd < (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,(@SeikYm)) + '01' 																																										
												ELSE CASE 																																										
												WHEN ISDATE(CONVERT(VARCHAR, @SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,(@SeikYm + RIGHT('00' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '')), 2))))), 112)																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,(@SeikYm)) + '01'),112) END END ))
												AND eTKD_Nyusih.NyuSihKbn = 1 AND eTKD_Nyusih.SiyoKbn = 1))
 AND (@BillingType = '' OR eTKD_NyShmi.SeiFutSyu IN (SELECT value FROM STRING_SPLIT(@BillingType, ',')))
 AND (@StaUkeNo = '' OR eTKD_Yyksho.UkeNo >= @StaUkeNo)
 AND (@EndUkeNo = '' OR eTKD_Yyksho.UkeNo <= @EndUkeNo)
 AND (@StaYoyaKbn = 0 OR eVPM_YoyKbn13.YoyaKbn >= @StaYoyaKbn)
 AND (@EndYoyaKbn = 0 OR eVPM_YoyKbn13.YoyaKbn <= @EndYoyaKbn)
Group By eVPM_TokiSt12.TokuiSeq,
eVPM_TokiSt12.SitenCdSeq,
eTKD_Nyshmi.UkeNo,
eTKD_Nyshmi.SeiFutSyu,
eTKD_Nyshmi.FutTumRen,
 CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,'00'))
)  
INSERT  INTO 
        @Temp_TKD_SeiUch 
SELECT 
        @SeiOutSeq									AS      SeiOutSeq 
    ,   ISNULL(eTKD_Mishum02.SeiRen, 0)                        AS      SeiRen 
    ,   ISNULL(eTKD_Mishum02.SeiMeiRen, 0)                     AS      SeiMeiRen 
    ,   DENSE_RANK()    OVER    (   PARTITION BY 
                                                            eTKD_Mishum02.SeiRen 
                                                    ,       eTKD_Mishum02.SeiMeiRen 
                                    ORDER BY 
                                                            eTKD_Mishum02.SeiRen 
                                                    ,       eTKD_Mishum02.SeiMeiRen 
                                                    ,       ISNULL(eTKD_YykSyu12.UnkRen,0) 
                                                    ,       ISNULL(eTKD_YykSyu12.SyaSyuRen,0) 
                                )                   AS      SeiUchRen 
    ,   ISNULL(eTKD_Mishum02.HasYmd, '')                        AS      HasYmd 
    ,   ISNULL(eTKD_Unkobi13.DantaNm  ,eTKD_Mishum02.YoyaNm	) AS      YoyaNm 
    ,   CASE 
            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1,7) 
            THEN    ISNULL(eTKD_Unkobi13.IkNm		 ,eTKD_Mishum02.FutTumNm	) 
            ELSE    ISNULL(eTKD_Mishum02.FutTumNm   ,       ' ') 
        END													  AS      FutTumNm 
    ,   ISNULL(eTKD_Unkobi13.HaiSYmd  ,eTKD_Mishum02.HaiSYmd	) AS      HaiSYmd 
    ,   ISNULL(eTKD_Unkobi13.TouYmd   ,eTKD_Mishum02.TouYmd	) AS      TouYmd 
    ,   CASE 
            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1) 
            THEN    ISNULL(eTKD_YykSyu12.SyaSyuDai  ,       0) 
            ELSE    eTKD_Mishum02.Suryo 
        END                                         AS      Suryo 
    ,   CASE 
            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1) 
            THEN    ISNULL(eTKD_YykSyu12.SyaSyuTan  ,       0) 
            ELSE    eTKD_Mishum02.TanKa 
        END                                         AS      TanKa 
    ,   CASE 
            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1) 
            THEN    ISNULL( eVPM_SyaSyu12.SyaSyuNm  ,       '') 
            ELSE    ''  
        END                                         AS      SyaSyuNm 
    ,   1                                           AS      SiyoKbn 
    ,   @UpdYmd										AS      UpdYmd 
    ,   @UpdTime									AS      UpdTime 
    ,   @UpdSyainCd									AS      UpdSyainCd 
    ,   @UpdPrgID									AS      UpdPrgID 
FROM 
        eTKD_Mishum02 
        LEFT JOIN   TKD_YykSyu      AS      eTKD_YykSyu12 
                                    ON      eTKD_Mishum02.UkeNo         =       eTKD_YykSyu12.UkeNo 
                                    AND     eTKD_YykSyu12.SiyoKbn       =       1 
                                    AND     eTKD_Mishum02.SeiFutSyu     =       1 
            LEFT JOIN   TKD_Unkobi      AS      eTKD_Unkobi13 
                                        ON      eTKD_YykSyu12.UkeNo     =       eTKD_Unkobi13.UkeNo 
                                        AND     eTKD_YykSyu12.UnkRen	 =       eTKD_Unkobi13.UnkRen 
                                        AND     eTKD_Unkobi13.SiyoKbn   =       1 
            LEFT JOIN   VPM_SyaSyu      AS      eVPM_SyaSyu12 
                                        ON      eVPM_SyaSyu12.SyaSyuCdSeq             =       eTKD_YykSyu12.SyaSyuCdSeq 
                                        AND     eVPM_SyaSyu12.SiyoKbn        =       1 
ORDER BY 
        eTKD_Mishum02.SeiRen 
   ,   eTKD_Mishum02.SeiMeiRen 
   ,   ISNULL(eTKD_YykSyu12.UnkRen,0) 
   ,   ISNULL(eTKD_YykSyu12.SyaSyuRen,0) 
END
-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************
BEGIN 
WITH 
eTKD_SeiUch01   AS 
( 
    SELECT 
            TKD_SeiUch.SeiOutSeq    AS      SeiOutSeq 
       ,   TKD_SeiUch.SeiRen       AS      SeiRen 
       ,   COUNT(*)                AS      MeisaiKensu 
    FROM 
            @Temp_TKD_SeiUch AS TKD_SeiUch
    WHERE (ISNULL(@SeiOutSyKbn,1) = 1 AND TKD_SeiUch.SeiOutSeq = @SeiOutSeq)
	OR (ISNULL(@SeiOutSyKbn,1) = 2 AND TKD_SeiUch.SeiOutSeq = @SeiOutSeq) 
	OR (ISNULL(@SeiOutSyKbn,1) = 3 AND TKD_SeiUch.SeiOutSeq = @SeiOutSeq)     
    GROUP BY 
            TKD_SeiUch.SeiOutSeq 
       ,   TKD_SeiUch.SeiRen 
) 
, 
eTKD_SeiMei02 AS 
( 
	SELECT 
			ROW_NUMBER() OVER (PARTITION BY 
			TKD_SeiMei.SeiOutSeq
			ORDER BY 
				CASE WHEN JT_Yyksho.Yoyasyu=1
				THEN JT_YykSho.SEITAIYMD
				ELSE JT_YykSho.CanYMD
				END) AS ROWNUM
		,	TKD_SeiMei.SeiOutSeq
		,	(CASE WHEN JT_YykSho.Yoyasyu=1
			THEN JT_YykSho.SeitaiYMD
			ELSE JT_YykSho.CanYMD
			END) AS SeiTaiYmd 
	FROM @Temp_TKD_SeiMei AS TKD_SeiMei
	LEFT JOIN TKD_YykSho AS JT_YykSho
	ON JT_YykSho.UkeNo=TKD_SeiMei.UkeNo
	AND JT_YykSho.SiyoKbn=1
	WHERE TKD_SeiMei.SeiOutSeq = @SeiOutSeq     
)
,
eVPM_CodeKb01 AS (SELECT CodeKbn, CodeKbnNm FROM VPM_CodeKb WHERE CodeSyu = 'YOKINSYU' AND SiyoKbn = 1 AND TenantCdSeq = (
				SELECT CASE WHEN COUNT(*) = 0 THEN 0 ELSE @TenantCdSeq END AS TenantCdSeq FROM VPM_CodeKb WHERE VPM_CodeKb.CodeSyu = 'YOKINSYU' AND VPM_CodeKb.SiyoKbn = 1 AND
				VPM_CodeKb.TenantCdSeq = @TenantCdSeq)),
eTKD_Seikyu01   AS 
( 
    SELECT 
            TKD_SeiPrS.* 
       ,   CASE WHEN ISNULL(@SeiOutSyKbn,1) = 1 THEN ISNULL(eVPM_TokiSt11.ZipCd,' ')  
				 WHEN ISNULL(@SeiOutSyKbn,1) = 2 OR ISNULL(@SeiOutSyKbn,1) = 3 THEN @ZipCd
				 WHEN ISNULL(@SeiOutSyKbn,1) = 4 THEN 
				 (CASE WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn,0) = 1 THEN ISNULL(eVPM_TokiSt11.ZipCd,' ')
				 ELSE ISNULL(eTKD_SeiPrS11.ZipCd,' ')END) END AS ZipCd
				 
       ,   CASE WHEN ISNULL(@SeiOutSyKbn,1) = 1 THEN ISNULL(eVPM_TokiSt11.Jyus1,' ') 
				 WHEN ISNULL(@SeiOutSyKbn,1) = 2 OR ISNULL(@SeiOutSyKbn,1) = 3 THEN @Jyus1
				 WHEN ISNULL(@SeiOutSyKbn,1) = 4 THEN 
				 (CASE WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn,0) = 1 THEN ISNULL(eVPM_TokiSt11.Jyus1,' ')
				 ELSE ISNULL(eTKD_SeiPrS11.Jyus1,' ')END) END AS Jyus1
				 
       ,   CASE WHEN ISNULL(@SeiOutSyKbn,1) = 1 THEN ISNULL(eVPM_TokiSt11.Jyus2,' ')  
				 WHEN ISNULL(@SeiOutSyKbn,1) = 2 OR ISNULL(@SeiOutSyKbn,1) = 3 THEN @Jyus2
				 WHEN ISNULL(@SeiOutSyKbn,1) = 4 THEN 
				 (CASE WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn,0) = 1 THEN ISNULL(eVPM_TokiSt11.Jyus2,' ')
				 ELSE ISNULL(eTKD_SeiPrS11.Jyus2,' ')END) END AS Jyus2
				 
       ,   CASE WHEN ISNULL(@SeiOutSyKbn,1) = 1 THEN ISNULL(eVPM_Tokisk11.TokuiNm,' ')  
				 WHEN ISNULL(@SeiOutSyKbn,1) = 2 OR ISNULL(@SeiOutSyKbn,1) = 3 THEN @TokuiNm
				 WHEN ISNULL(@SeiOutSyKbn,1) = 4 THEN 
				 (CASE WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn,0) = 1 THEN ISNULL(eVPM_Tokisk11.TokuiNm,' ')
				 ELSE ISNULL(eTKD_SeiPrS11.TokuiNm,' ')END) END AS TokuiNm 

       ,   CASE WHEN ISNULL(@SeiOutSyKbn,1) = 1 THEN ISNULL(eVPM_TokiSt11.SitenNm,' ')  
				 WHEN ISNULL(@SeiOutSyKbn,1) = 2 OR ISNULL(@SeiOutSyKbn,1) = 3 THEN @SitenNm
				 WHEN ISNULL(@SeiOutSyKbn,1) = 4 THEN 
				 (CASE WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn,0) = 1 THEN ISNULL(eVPM_TokiSt11.SitenNm,' ')
				 ELSE ISNULL(eTKD_SeiPrS11.SitenNm,' ')END) END AS SitenNm 
       ,   ISNULL(eVPM_Eigyos11.ZipCd     ,' '  )   AS      SeiEigZipCd 
       ,   ISNULL(eVPM_Eigyos11.Jyus1     ,' '  )   AS      SeiEigJyus1 
       ,   ISNULL(eVPM_Eigyos11.Jyus2     ,' '  )   AS      SeiEigJyus2 
       ,   ISNULL(eVPM_Eigyos11.EigyoNm   ,' '  )   AS      SeiEigEigyoNm 
       ,   ISNULL(eVPM_Tokist11.TokuiTanNm,' '  )   AS      TokuiTanNm 
       ,   ISNULL(eVPM_Eigyos11.TelNo     ,' '  )   AS      SeiEigTelNo 
       ,   ISNULL(eVPM_Eigyos11.FaxNo     ,' '  )   AS      SeiEigFaxNo 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) 
                THEN    ISNULL(eVPM_Bank11.BankNm      ,' '  ) 
                ELSE    ' ' 
            END                                         AS      BankNm1 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) 
                THEN    ISNULL(eVPM_BankSt11.BankSitNm ,' '  ) 
                ELSE    ' ' 
            END                                         AS      BankSitNm1 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) 
                THEN    ISNULL(eVPM_CodeKb11.CodeKbnNm ,' '  ) 
                ELSE    ' ' 
            END                                         AS      YokinSyuNm1 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) 
                THEN    ISNULL(eVPM_Eigyos11.KouzaNo1  ,' '  ) 
                ELSE    ' ' 
            END                                         AS      KouzaNo1 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) 
                THEN    ISNULL(eVPM_Bank12.BankNm      ,' '  ) 
                ELSE    ' ' 
            END                                         AS      BankNm2 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) 
                THEN    ISNULL(eVPM_BankSt12.BankSitNm ,' '  ) 
                ELSE    ' ' 
            END                                         AS      BankSitNm2 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) 
                THEN    ISNULL(eVPM_CodeKb12.CodeKbnNm ,' '  ) 
                ELSE    ' ' 
            END                                         AS      YokinSyuNm2 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) 
                THEN    ISNULL(eVPM_Eigyos11.KouzaNo2  ,' '  ) 
                ELSE    ' ' 
            END                                         AS      KouzaNo2 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) 
                THEN    ISNULL(eVPM_Bank13.BankNm      ,' '  ) 
                ELSE    ' ' 
            END                                         AS      BankNm3 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) 
                THEN    ISNULL(eVPM_BankSt13.BankSitNm ,' '  ) 
                ELSE    ' ' 
            END                                         AS      BankSitNm3 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) 
                THEN    ISNULL(eVPM_CodeKb13.CodeKbnNm ,' '  ) 
                ELSE    ' ' 
            END                                         AS      YokinSyuNm3 
       ,   CASE 
                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) 
                THEN    ISNULL(eVPM_Eigyos11.KouzaNo3  ,' '  ) 
                ELSE    ' ' 
            END                                         AS      KouzaNo3 
       ,   ISNULL(eVPM_Eigyos11.KouzaMeigi,' '  )   AS      KouzaMeigi 
       ,   ISNULL(eTKD_SeiUch11.MeisaiKensu,0      )   AS      MeisaiKensu 
       ,   ISNULL(eTKD_SeiPrS11.SeiHatYmd ,' '  )   AS      SeiHatYmd 
       ,   ISNULL(eVPM_Compny11.CompanyNm ,' '  )   AS      SeiEigCompanyNm 
    FROM 
            @Temp_TKD_Seikyu AS  TKD_SeiPrS
            LEFT JOIN   @Temp_TKD_SeiPrS      AS      eTKD_SeiPrS11 
                                        ON      TKD_SeiPrS.SeiOutSeq            =       eTKD_SeiPrS11.SeiOutSeq 
			 LEFT JOIN eTKD_SeiMei02	AS		eTKD_SeiMei12
										ON		TKD_SeiPrS.SeiOutSeq=eTKD_SeiMei12.SeiOutSeq
										AND		eTKD_SeiMei12.RowNum=1
			 LEFT JOIN VPM_Tokisk		AS      eVPM_Tokisk11
										ON		TKD_SeiPrS.TokuiSeq				=	eVPM_Tokisk11.TokuiSeq
                                      AND     TKD_SeiPrS.SiyoEndYmd           BETWEEN eVPM_Tokisk11.SiyoStaYmd 
																				AND eVPM_Tokisk11.SiyoEndYmd
            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt11 
                                        ON      TKD_SeiPrS.TokuiSeq             =       eVPM_TokiSt11.TokuiSeq 
                                        AND     TKD_SeiPrS.SitenCdSeq           =       eVPM_TokiSt11.SitenCdSeq 
                                        AND     TKD_SeiPrS.SiyoEndYmd           BETWEEN eVPM_TokiSt11.SiyoStaYmd 
                                                                                AND     eVPM_TokiSt11.SiyoEndYmd 
            LEFT JOIN   VPM_Eigyos      AS      eVPM_Eigyos11 
                                        ON      eTKD_SeiPrS11.SeiEigCdSeq       =       eVPM_Eigyos11.EigyoCdSeq 
            LEFT JOIN   VPM_Compny      AS      eVPM_Compny11 
                                        ON      eVPM_Eigyos11.CompanyCdSeq      =       eVPM_Compny11.CompanyCdSeq 
            LEFT JOIN   VPM_TokEig      AS      eVPM_TokEig11 
                                        ON      TKD_SeiPrS.TokuiSeq             =       eVPM_TokEig11.TokuiSeq 
                                        AND     TKD_SeiPrS.SitenCdSeq           =       eVPM_TokEig11.SitenCdSeq 
                                        AND     eTKD_SeiPrS11.SeiEigCdSeq       =       eVPM_TokEig11.EigyoCdSeq 
                                        AND     TKD_SeiPrS.SiyoEndYmd           =       eVPM_TokEig11.SiyoEndYmd 
            LEFT JOIN   VPM_Bank        AS      eVPM_Bank11 
                                        ON      eVPM_Eigyos11.BankCd1           =       eVPM_Bank11.BankCd 
            LEFT JOIN   VPM_Bank        AS      eVPM_Bank12 
                                        ON      eVPM_Eigyos11.BankCd2           =       eVPM_Bank12.BankCd 
            LEFT JOIN   VPM_Bank        AS      eVPM_Bank13 
                                        ON      eVPM_Eigyos11.BankCd3           =       eVPM_Bank13.BankCd 
            LEFT JOIN   VPM_BankSt      AS      eVPM_BankSt11 
                                        ON      eVPM_Eigyos11.BankCd1           =       eVPM_BankSt11.BankCd 
                                        AND     eVPM_Eigyos11.BankSitCd1        =       eVPM_BankSt11.BankSitCd 
            LEFT JOIN   VPM_BankSt      AS      eVPM_BankSt12 
                                        ON      eVPM_Eigyos11.BankCd2           =       eVPM_BankSt12.BankCd 
                                        AND     eVPM_Eigyos11.BankSitCd2        =       eVPM_BankSt12.BankSitCd 
            LEFT JOIN   VPM_BankSt      AS      eVPM_BankSt13 
                                        ON      eVPM_Eigyos11.BankCd3           =       eVPM_BankSt13.BankCd 
                                        AND     eVPM_Eigyos11.BankSitCd3        =       eVPM_BankSt13.BankSitCd 
            LEFT JOIN   eVPM_CodeKb01		 AS      eVPM_CodeKb11 
                                        On     eVPM_Eigyos11.YokinSyu1         =       eVPM_CodeKb11.CodeKbn 
            LEFT JOIN   eVPM_CodeKb01      AS      eVPM_CodeKb12 
                                        On     eVPM_Eigyos11.YokinSyu2         =       eVPM_CodeKb12.CodeKbn 
            LEFT JOIN   eVPM_CodeKb01      AS      eVPM_CodeKb13 
                                        On     eVPM_Eigyos11.YokinSyu3         =       eVPM_CodeKb13.CodeKbn 
            LEFT JOIN   eTKD_SeiUch01   AS      eTKD_SeiUch11 
                                        On      TKD_SeiPrS.SeiOutSeq            =       eTKD_SeiUch11.SeiOutSeq 
                                        AND     TKD_SeiPrS.SeiRen               =       eTKD_SeiUch11.SeiRen 
    WHERE (ISNULL(@SeiOutSyKbn,1) = 1 AND TKD_SeiPrS.SeiOutSeq = @SeiOutSeq)
	OR (ISNULL(@SeiOutSyKbn,1) = 2 AND TKD_SeiPrS.SeiOutSeq = @SeiOutSeq) 
	OR (ISNULL(@SeiOutSyKbn,1) = 3 AND TKD_SeiPrS.SeiOutSeq = @SeiOutSeq)
	OR(ISNULL(@SeiOutSyKbn,1) = 4 AND  TKD_SeiPrS.SeiOutSeq = @InvoiceOutNum AND TKD_SeiPrS.SeiRen = @InvoiceSerNum)
) 
SELECT 
        eTKD_Seikyu01.* 
FROM 
        eTKD_Seikyu01
END		
-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************

BEGIN 
WITH 
eTKD_SeiUch01 AS 
( 
    SELECT 
            TKD_SeiUch.* 
       ,   CASE 
                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) 
                AND     TKD_SeiUch.SeiUchRen            <>      1 
                THEN    0 
                ELSE    ISNULL(eTKD_SeiMei11.UriGakKin ,0      ) 
            END                                         AS      UriGakKin 
       ,   CASE 
                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) 
                AND     TKD_SeiUch.SeiUchRen            <>      1 
                THEN    0 
                ELSE    ISNULL(eTKD_SeiMei11.SyaRyoSyo ,0      ) 
            END                                         AS      SyaRyoSyo 
       ,   CASE 
                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) 
                AND     TKD_SeiUch.SeiUchRen            <>      1 
                THEN    0 
                ELSE    ISNULL(eTKD_SeiMei11.SyaRyoTes ,0      ) 
            END                                         AS      SyaRyoTes 
       ,   CASE 
                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) 
                AND     TKD_SeiUch.SeiUchRen            <>      1 
                THEN    0 
                ELSE    ISNULL(eTKD_SeiMei11.SeiKin    ,0      ) 
            END                                         AS      SeiKin 
       ,   CASE 
                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) 
                AND     TKD_SeiUch.SeiUchRen            <>      1 
                THEN    0 
                ELSE    ISNULL(eTKD_SeiMei11.NyuKinRui ,0      ) 
            END                                         AS      NyuKinRui 
       ,   CASE 
                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) 
                THEN    ' ' 
                ELSE    ISNULL(eTKD_FutTum11.BikoNm    ,' '  ) 
            END                                         AS      BikoNm 
        ,   ISNULL(eTKD_SeiMei11.SeiSaHKbn ,0      )  AS      SeiSaHKbn 
        ,   ISNULL(eTKD_SeiMei11.UkeNo ,0      )  AS      UkeNo 
        ,   ISNULL(eTKD_FutTum11.IriRyoNm,' '   )  AS      IriRyoNm 
        ,   ISNULL(eTKD_FutTum11.DeRyoNm,' '   )   AS      DeRyoNm 
        ,   ISNULL(eTKD_Mishum11.SeiFutSyu,0   )  AS      SeiFutSyu 
        ,    ISNULL(eVPM_Futai11.FutaiCd,0) AS FutaiCd 
		 ,		CASE 
					WHEN eTKD_Mishum11.SeiFutSyu	IN			(1)
					AND		TKD_SeiUch.SeiUchRen	<>			1
					THEN	0
					ELSE	ISNULL(eTKD_SeiMei11.Zeiritsu,0		)
				END  AS Zeiritsu
    FROM 
            @Temp_TKD_SeiUch AS TKD_SeiUch
            LEFT JOIN   @Temp_TKD_SeiMei      AS      eTKD_SeiMei11 
                                        ON      TKD_SeiUch.SeiOutSeq            =       eTKD_SeiMei11.SeiOutSeq 
                                        AND     TKD_SeiUch.SeiRen               =       eTKD_SeiMei11.SeiRen 
                                        AND     TKD_SeiUch.SeiMeiRen            =       eTKD_SeiMei11.SeiMeiRen 
            LEFT JOIN   TKD_Mishum      AS      eTKD_Mishum11 
                                        ON      eTKD_SeiMei11.UkeNo             =       eTKD_Mishum11.UkeNo 
                                        AND     eTKD_SeiMei11.MisyuRen          =       eTKD_Mishum11.MisyuRen 
            LEFT JOIN   TKD_FutTum      AS      eTKD_FutTum11 
                                        ON      eTKD_Mishum11.UkeNo             =       eTKD_FutTum11.UkeNo 
                                        AND     eTKD_Mishum11.FutuUnkRen        =       eTKD_FutTum11.UnkRen 
										 AND CASE 
										 WHEN    eTKD_Mishum11.SeiFutSyu         =       6 
										 THEN    2 
										 ELSE    1 
										 END									 =       eTKD_FutTum11.FutTumKbn 
                                        AND     eTKD_Mishum11.FutTumRen         =       eTKD_FutTum11.FutTumRen 
            LEFT JOIN  VPM_Futai AS eVPM_Futai11 
										 ON   eVPM_Futai11.FutaiCdSeq  =  eTKD_FutTum11.FutTumCdSeq 
										 AND  eVPM_Futai11.SiyoKbn     =  1 
    WHERE (ISNULL(@SeiOutSyKbn,1) = 1 AND TKD_SeiUch.SeiOutSeq = @SeiOutSeq)
	OR (ISNULL(@SeiOutSyKbn,1) = 2 AND TKD_SeiUch.SeiOutSeq = @SeiOutSeq) 
	OR (ISNULL(@SeiOutSyKbn,1) = 3 AND TKD_SeiUch.SeiOutSeq = @SeiOutSeq)
	OR(ISNULL(@SeiOutSyKbn,1) = 4 AND  TKD_SeiUch.SeiOutSeq = @InvoiceOutNum AND TKD_SeiUch.SeiRen = @InvoiceSerNum)   
) 
SELECT 
            eTKD_SeiUch01.* 
FROM 
            eTKD_SeiUch01 
ORDER BY 
	eTKD_SeiUch01.SeiOutSeq 
,	eTKD_SeiUch01.SeiRen 
,	eTKD_SeiUch01.SeiMeiRen 
,	eTKD_SeiUch01.SeiUchRen
END

SET @RowCount	=	@@RowCount
SET	@ReturnCd	=	ERROR_NUMBER()
SET	@ReturnMsg	=	ERROR_MESSAGE()
SET	@eProcedure	=	ERROR_PROCEDURE()
SET	@eLine		=	ERROR_LINE()
END
RETURN