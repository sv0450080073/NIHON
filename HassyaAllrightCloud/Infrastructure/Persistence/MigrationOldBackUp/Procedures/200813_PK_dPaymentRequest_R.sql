USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dPaymentRequest_R]    Script Date: 9/1/2020 8:00:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	
-- Module-Name	:	
-- SP-ID		:	PK_dPaymentRequest_R
-- DB-Name		:	
-- Name			:	
-- Date			:	2020/08/11
-- Author		:	T.L.DUY
-- Description	:	Select
-- 				:	

CREATE OR ALTER PROCEDURE [dbo].[PK_dPaymentRequest_R]
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
	DECLARE	@wk_SeikYm				VARCHAR	(MAX)				-- 
	DECLARE	@wk_SeiHatYmd			VARCHAR	(MAX)				-- 
	DECLARE	@wk_SeiOutTime			VARCHAR	(MAX)				-- 
	DECLARE	@wk_InTanCdSeq			CHAR	(8)					-- 
	DECLARE	@wk_SeiOutSyKbn			CHAR	(1)					-- 
	DECLARE	@wk_SeiGenFlg			CHAR	(1)					-- 
	DECLARE	@wk_StaUkeNo			CHAR	(15)					-- 
	DECLARE	@wk_EndUkeNo			CHAR	(15)					-- 
	DECLARE	@wk_StaYoyaKbn			CHAR	(3)					-- 
	DECLARE	@wk_EndYoyaKbn			CHAR	(3)					-- 
	DECLARE	@wk_SeiEigCdSeq			CHAR	(8)					-- 
	DECLARE	@wk_SeiSitKbn			CHAR	(1)					-- 
	DECLARE	@wk_StaSeiCdSeq			CHAR	(8)					-- 
	DECLARE	@wk_StaSeiSitCdSeq		CHAR	(8)					-- 
	DECLARE	@wk_EndSeiCdSeq			CHAR	(8)					-- 
	DECLARE	@wk_EndSeiSitCdSeq		CHAR	(8)					-- 
	DECLARE	@wk_SimeD				CHAR	(2)					-- 
	DECLARE	@wk_PrnCpys				CHAR	(2)					-- 
	DECLARE	@wk_PrnCpysTan			CHAR	(1)					-- 
	DECLARE	@wk_TesPrnKbn			CHAR	(1)					-- 
	DECLARE	@wk_SeiFutUncKbn		CHAR	(1)					-- 
	DECLARE	@wk_SeiFutFutKbn		CHAR	(1)					-- 
	DECLARE	@wk_SeiFutTukKbn		CHAR	(1)					-- 
	DECLARE	@wk_SeiFutTehKbn		CHAR	(1)					-- 
	DECLARE	@wk_SeiFutGuiKbn		CHAR	(1)					-- 
	DECLARE	@wk_SeiFutTumKbn		CHAR	(1)					-- 
	DECLARE	@wk_SeiFutCanKbn		CHAR	(1)					-- 
	DECLARE	@wk_ZipCd				VARCHAR	(MAX)				-- 
	DECLARE	@wk_Jyus1				VARCHAR	(MAX)				-- 
	DECLARE	@wk_Jyus2				VARCHAR	(MAX)				-- 
	DECLARE	@wk_TokuiNm				VARCHAR	(MAX)				-- 
	DECLARE	@wk_SitenNm				VARCHAR	(MAX)				-- 
	DECLARE	@wk_SiyoKbn				CHAR	(1)					-- 
	DECLARE	@wk_UpdYmd				VARCHAR	(MAX)				-- 
	DECLARE	@wk_UpdTime				VARCHAR	(MAX)				-- 
	DECLARE	@wk_UpdSyainCd			CHAR	(8)					-- 
	DECLARE	@wk_UpdPrgID			VARCHAR	(MAX)				-- 
	DECLARE @wk_TenantCdSeq			NVARCHAR(10)                 
	DECLARE @wk_StartBillAdd		NVARCHAR(11)        
	DECLARE @wk_EndBillAdd			NVARCHAR(11)        
	DECLARE @wk_InvoiceOutNum		NVARCHAR(10)					
	DECLARE @wk_InvoiceSerNum		NVARCHAR(5)			
	DECLARE @wk_BillingType			NVARCHAR(100)       
	DECLARE @wk_OutDataTable		NVARCHAR(MAX)		    		
	--2015/03/10 M.OHMORI STR
	DECLARE @wk_KuriSyoriKbn		CHAR	(1)					-- 
	--2015/03/10 M.OHMORI END
	DECLARE	@wk_SeiOutSeq			CHAR	(8)

	DECLARE	@wk_SeiUch_InsCount		INT							-- 
	DECLARE	@wk_SeiUch_SelCount		INT							-- 

	DECLARE	@strSQL1				VARCHAR(MAX)
	DECLARE	@strSQL_Seikyu			VARCHAR(MAX)
	DECLARE	@strSQL_SeiMei			VARCHAR(MAX)
	DECLARE	@strSQL_SeiUch			VARCHAR(MAX)

	SET		@strSQL1			=	' '
	SET		@strSQL_Seikyu		=	' '
	SET		@strSQL_SeiMei		=	' '
	SET		@strSQL_SeiUch		=	' '

	-- 
	SET		@wk_InTanCdSeq		=	CONVERT(CHAR(8)	,@InTanCdSeq		)	-- 
	SET		@wk_SeiOutSyKbn		=	CONVERT(CHAR(1)	,@SeiOutSyKbn		)	-- 
	SET		@wk_SeiGenFlg		=	CONVERT(CHAR(1)	,@SeiGenFlg			)	-- 
	SET		@wk_StaUkeNo		=	CONVERT(CHAR(15),@StaUkeNo			)	-- 
	SET		@wk_EndUkeNo		=	CONVERT(CHAR(15),@EndUkeNo			)	-- 
	SET		@wk_StaYoyaKbn		=	CONVERT(CHAR(3)	,@StaYoyaKbn		)	-- 
	SET		@wk_EndYoyaKbn		=	CONVERT(CHAR(3)	,@EndYoyaKbn		)	-- 
	SET		@wk_SeiEigCdSeq		=	CONVERT(CHAR(8)	,@SeiEigCdSeq		)	-- 
	SET		@wk_SeiSitKbn		=	CONVERT(CHAR(1)	,@SeiSitKbn			)	-- 
	SET		@wk_StaSeiCdSeq		=	CONVERT(CHAR(8)	,@StaSeiCdSeq		)	-- 
	SET		@wk_StaSeiSitCdSeq	=	CONVERT(CHAR(8)	,@StaSeiSitCdSeq	)	-- 
	SET		@wk_EndSeiCdSeq		=	CONVERT(CHAR(8)	,@EndSeiCdSeq		)	-- 
	SET		@wk_EndSeiSitCdSeq	=	CONVERT(CHAR(8)	,@EndSeiSitCdSeq	)	-- 
	SET		@wk_SimeD			=	CONVERT(CHAR(2)	,@SimeD				)	-- 
	SET		@wk_PrnCpys			=	CONVERT(CHAR(2)	,@PrnCpys			)	-- 
	SET		@wk_PrnCpysTan		=	CONVERT(CHAR(1)	,@PrnCpysTan		)	-- 
	SET		@wk_TesPrnKbn		=	CONVERT(CHAR(1)	,@TesPrnKbn			)	-- 
	SET		@wk_SeiFutUncKbn	=	CONVERT(CHAR(1)	,@SeiFutUncKbn		)	-- 
	SET		@wk_SeiFutFutKbn	=	CONVERT(CHAR(1)	,@SeiFutFutKbn		)	-- 
	SET		@wk_SeiFutTukKbn	=	CONVERT(CHAR(1)	,@SeiFutTukKbn		)	-- 
	SET		@wk_SeiFutTehKbn	=	CONVERT(CHAR(1)	,@SeiFutTehKbn		)	-- 
	SET		@wk_SeiFutGuiKbn	=	CONVERT(CHAR(1)	,@SeiFutGuiKbn		)	-- 
	SET		@wk_SeiFutTumKbn	=	CONVERT(CHAR(1)	,@SeiFutTumKbn		)	-- 
	SET		@wk_SeiFutCanKbn	=	CONVERT(CHAR(1)	,@SeiFutCanKbn		)	-- 
	SET		@wk_SiyoKbn			=	CONVERT(CHAR(1)	,@SiyoKbn			)	-- 
	SET		@wk_UpdSyainCd		=	CONVERT(CHAR(8)	,@UpdSyainCd		)	-- 
	SET		@wk_KuriSyoriKbn	=	CONVERT(CHAR(1) ,@KuriSyoriKbn		)	-- 
	SET		@wk_SeikYm			=	REPLACE	(@SeikYm		,'''','''''')	-- 
	SET		@wk_SeiHatYmd		=	REPLACE	(@SeiHatYmd		,'''','''''')	-- 
	SET		@wk_SeiOutTime		=	REPLACE	(@SeiOutTime	,'''','''''')	-- 
	SET		@wk_ZipCd			=	REPLACE	(@ZipCd			,'''','''''')	-- 
	SET		@wk_Jyus1			=	REPLACE	(@Jyus1			,'''','''''')	-- 
	SET		@wk_Jyus2			=	REPLACE	(@Jyus2			,'''','''''')	-- 
	SET		@wk_TokuiNm			=	REPLACE	(@TokuiNm		,'''','''''')	-- 
	SET		@wk_SitenNm			=	REPLACE	(@SitenNm		,'''','''''')	-- 
	SET		@wk_UpdYmd			=	REPLACE	(@UpdYmd		,'''','''''')	-- 
	SET		@wk_UpdTime			=	REPLACE	(@UpdTime		,'''','''''')	-- 
	SET		@wk_UpdPrgID		=	REPLACE	(@UpdPrgID		,'''','''''')	-- 
	SET     @wk_TenantCdSeq		=	CONVERT(NVARCHAR(10) ,@TenantCdSeq	)
	
	SET     @wk_StartBillAdd	=	CONVERT(NVARCHAR(11) ,@StartBillAdd	)                 
	SET     @wk_EndBillAdd		=	CONVERT(NVARCHAR(11) ,@EndBillAdd	)                 
	SET     @wk_InvoiceOutNum	=	CONVERT(NVARCHAR(10) ,@InvoiceOutNum	)                 
	SET     @wk_InvoiceSerNum	=	CONVERT(NVARCHAR(5) ,@InvoiceSerNum	)                 
	SET     @wk_BillingType		=	CONVERT(NVARCHAR(100) ,@BillingType	)                 
	SET     @wk_OutDataTable	=	CONVERT(NVARCHAR(MAX) ,@OutDataTable)               



	-- 
	SET		@SeiOutSeq			=	0		-- 
	SET		@ReturnCd			=	0		-- 
	SET		@RowCount			=	0		-- 
	SET		@ReturnMsg			=	' '		-- 
	SET		@eProcedure			=	' '		-- 
	SET		@eLine				=	' '		-- 

	BEGIN TRY
-- ****************************************************************************************************************************************
		INSERT	INTO
				TKD_SeiPrS
		VALUES
			(
	--			SeiOutSeq	IDENTITY
				@SeikYm
			,	@SeiHatYmd
			,	@SeiOutTime
			,	@InTanCdSeq
			,	@SeiOutSyKbn
			,	@SeiGenFlg
			,	@StaUkeNo
			,	@EndUkeNo
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
			,	@ZipCd
			,	@Jyus1
			,	@Jyus2
			,	@TokuiNm
			,	@SitenNm
			,	@SiyoKbn
			,	@UpdYmd
			,	@UpdTime
			,	@UpdSyainCd
			,	@UpdPrgID
			)

		SET		@SeiOutSeq		=	SCOPE_IDENTITY()
		SET		@wk_SeiOutSeq	=	CONVERT(CHAR(8)	,@SeiOutSeq)

		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
			OR	ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************
				SET		@strSQL1			=	@strSQL1
						+ CHAR(13) + CHAR(10) +	'WITH '
									-- CTE QUERY --------------------------------------------------------------------------------------------------------------*
						+ CHAR(13) + CHAR(10) +	'eTKD_Unkobi01   AS '
						+ CHAR(13) + CHAR(10) +	'( '
						+ CHAR(13) + CHAR(10) +	'    SELECT '
						+ CHAR(13) + CHAR(10) +	'            TKD_Unkobi.UkeNo                            AS      UkeNo '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Unkobi.UnkRen                           AS      UnkRen '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Unkobi.HaiSYmd                          AS      HaiSYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Unkobi.TouYmd                           AS      TouYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Unkobi.IkNm							 AS      IkNm '
						-- 2013/11/26 S.Sato INSERT STR --
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Unkobi.DanTaNm							 AS      DanTaNm '
						-- 2013/11/26 S.Sato INSERT END --
						+ CHAR(13) + CHAR(10) +	'        ,   ROW_NUMBER()    OVER    (   PARTITION BY '
						+ CHAR(13) + CHAR(10) +	'                                                                TKD_Unkobi.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                        ORDER BY '
						+ CHAR(13) + CHAR(10) +	'                                                                TKD_Unkobi.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                                        ,       TKD_Unkobi.UnkRen '
						+ CHAR(13) + CHAR(10) +	'                                    )                   AS      RowNumbr '
						+ CHAR(13) + CHAR(10) +	'    FROM '
						+ CHAR(13) + CHAR(10) +	'            TKD_Unkobi '
						+ CHAR(13) + CHAR(10) +	'    WHERE '
						+ CHAR(13) + CHAR(10) +	'            TKD_Unkobi.SiyoKbn                          =       1 '
						+ CHAR(13) + CHAR(10) +	') '
						+ CHAR(13) + CHAR(10) +	', '
									-- CTE QUERY --------------------------------------------------------------------------------------------------------------*
						+ CHAR(13) + CHAR(10) +	'eTKD_SeiMei01   AS '
						+ CHAR(13) + CHAR(10) +	'( '
						+ CHAR(13) + CHAR(10) +	'    SELECT  DISTINCT '
						+ CHAR(13) + CHAR(10) +	'            TKD_SeiMei.UkeNo                            AS      UkeNo '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_SeiMei.MisyuRen                         AS      MisyuRen '
						+ CHAR(13) + CHAR(10) +	'        ,   2                                           AS      SeiSaHKbn '
						+ CHAR(13) + CHAR(10) +	'    FROM '
						+ CHAR(13) + CHAR(10) +	'            TKD_SeiMei '
						+ CHAR(13) + CHAR(10) +	'    WHERE '
						+ CHAR(13) + CHAR(10) +	'            TKD_SeiMei.SiyoKbn                          =       1 '
						+ CHAR(13) + CHAR(10) +	') '
						+ CHAR(13) + CHAR(10) +	', '
									-- CTE QUERY --------------------------------------------------------------------------------------------------------------*
						+ CHAR(13) + CHAR(10) +	'eTKD_Mishum01   AS '
						+ CHAR(13) + CHAR(10) +	'( '
						+ CHAR(13) + CHAR(10) +	'    SELECT '
						+ CHAR(13) + CHAR(10) +	'            TKD_Mishum.UkeNo                            AS      UkeNo '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.MisyuRen                         AS      MisyuRen '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.HenKai                           AS      HenKai '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.SeiFutSyu                        AS      SeiFutSyu '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.UriGakKin                        AS      UriGakKin '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.SyaRyoSyo                        AS      SyaRyoSyo '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.SyaRyoTes                        AS      SyaRyoTes '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.SeiKin                           AS      SeiKin '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.NyuKinRui                        AS      NyuKinRui '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.FutuUnkRen                       AS      FutuUnkRen '
						+ CHAR(13) + CHAR(10) +	'        ,   TKD_Mishum.FutTumRen                        AS      FutTumRen '
						+ CHAR(13) + CHAR(10) +	'        ,   eTKD_Yyksho11.SeikYm                        AS      SeikYm '

				IF		ISNULL(@SeiSitKbn	,1)	=	1
					BEGIN
						SET		@strSQL1			=	@strSQL1
						--  --------------------------------------------------------------------------------------------------------------*
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_Gyosya12.GyosyaCd                      AS      GyosyaCd '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_Tokisk12.TokuiCd                       AS      TokuiCd '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt11.SeiCdSeq                      AS      TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt12.SitenCd                       AS      SitenCd '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt11.SeiSitenCdSeq                 AS      SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt12.SiyoEndYmd                    AS      SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt12.SimeD                         AS      SimeD '
					END

				IF		ISNULL(@SeiSitKbn	,1)	=	2
					BEGIN
						SET		@strSQL1			=	@strSQL1
						--  --------------------------------------------------------------------------------------------------------------*
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_Gyosya11.GyosyaCd                      AS      GyosyaCd '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_Tokisk11.TokuiCd                       AS      TokuiCd '
						+ CHAR(13) + CHAR(10) +	'        ,   eTKD_Yyksho11.TokuiSeq                      AS      TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt11.SitenCd                       AS      SitenCd '
						+ CHAR(13) + CHAR(10) +	'        ,   eTKD_Yyksho11.SitenCdSeq                    AS      SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt11.SiyoEndYmd                    AS      SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   eVPM_TokiSt11.SimeD                         AS      SimeD '
					END

				SET		@strSQL1			=	@strSQL1
						-- 2013/11/26 S.Sato UPDATE STR --
						--+ CHAR(13) + CHAR(10) +	'        ,   eTKD_Yyksho11.YoyaNm                        AS      YoyaNm '
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eTKD_Unkobi11.DanTaNm    ,eTKD_Yyksho11.YoyaNm) '
						+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_Unkobi12.DanTaNm    ,eTKD_Yyksho11.YoyaNm) '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      YoyaNm '
						-- 2013/11/26 S.Sato UPDATE END --
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eTKD_Yyksho11.SeiTaiYmd  ,       '' '') '
						+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_FutTum11.HasYmd     ,       '' '') '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      HasYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) '
						--+ CHAR(13) + CHAR(10) +	'                THEN    '''' '
						+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eTKD_Unkobi11.IkNm       ,       '' '') '
						+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_FutTum11.FutTumNm   ,       '' '') '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      FutTumNm '
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    0 '
						+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_FutTum11.Suryo      ,       0) '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      Suryo '
						
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    0 '
						+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_FutTum11.TanKa      ,       0) '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      TanKa '
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eTKD_Unkobi11.HaiSYmd    ,       '' '') '
--						+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_Unkobi12.HaiSYmd    ,       '' '') '
						+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      HaiSYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eTKD_Unkobi11.TouYmd     ,       '' '') '
--						+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_Unkobi12.TouYmd     ,       '' '') '
						+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      TouYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   eTKD_Yyksho11.YoyaKbnSeq                    AS      YoyaKbnSeq '
						+ CHAR(13) + CHAR(10) +	'        ,   CONCAT(CASE WHEN VPM_YoyaKbnSort11.PriorityNum IS NULL THEN ''99'' ELSE FORMAT(VPM_YoyaKbnSort11.PriorityNum, ''00'') END '
						+ CHAR(13) + CHAR(10) +	'        ,   FORMAT(eVPM_YoyKbn11.YoyaKbn,''00''))     AS YoyaKbnSort '
						--+ CHAR(13) + CHAR(10) +	'        ,   eVPM_YoyKbn11.YoyaKbn                       AS      YoyaKbn'
						+ CHAR(13) + CHAR(10) +	'        ,   eTKD_Yyksho11.SeiEigCdSeq                   AS      SeiEigCdSeq '

						+ CHAR(13) + CHAR(10) +	'        ,   eTKD_Yyksho11.SeiTaiYmd                     AS      SeiTaiYmd '
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    1 '
						+ CHAR(13) + CHAR(10) +	'                ELSE    2 '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      SeiFutSyu_Sort '
						-- 2013/11/26 S.Sato INSERT STR --
						+ CHAR(13) + CHAR(10) +	'        ,   CASE '
						+ CHAR(13) + CHAR(10) +	'                WHEN    TKD_Mishum.SeiFutSyu            IN      (1,7) '
						+ CHAR(13) + CHAR(10) +	'                THEN    '' '' '
						+ CHAR(13) + CHAR(10) +	'                ELSE    SUBSTRING(ISNULL(eTKD_FutTum11.ExpItem	,'' '')	,1,3) '
						+ CHAR(13) + CHAR(10) +	'            END                                         AS      FutTum_Sort '
						-- 2013/11/26 S.Sato INSERT END --
						--2019/09/18 M.OHMORI STR
						+ CHAR(13) + CHAR(10) + '		 ,	CASE '
						+ CHAR(13) + CHAR(10) + '				WHEN	TKD_Mishum.SeiFutSyu			 IN		(1,7)	'
						+ CHAR(13) + CHAR(10) + '				THEN	eTKD_Yyksho11.Zeiritsu'
						+ CHAR(13) + CHAR(10) + '				ELSE	eTKD_FutTum11.Zeiritsu'
						+ CHAR(13) + CHAR(10) + '			END											 AS		Zeiritsu'

						--2019/09/18 M.OHMORI END
						+ CHAR(13) + CHAR(10) +	'    FROM '
						+ CHAR(13) + CHAR(10) +	'            TKD_Mishum '
						+ CHAR(13) + CHAR(10) +	'            JOIN        TKD_Yyksho      AS      eTKD_Yyksho11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_Mishum.UkeNo                =       eTKD_Yyksho11.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Mishum.SiyoKbn              =       1 '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho11.SiyoKbn           =       1 '
						+ CHAR(13) + CHAR(10) +	'                                        AND (   (   TKD_Mishum.SeiFutSyu        =       7 '
						+ CHAR(13) + CHAR(10) +	'                                                AND eTKD_Yyksho11.YoyaSyu       =       2 '
						+ CHAR(13) + CHAR(10) +	'                                                ) '
						+ CHAR(13) + CHAR(10) +	'                                            OR  (   TKD_Mishum.SeiFutSyu        <>      7 '
						+ CHAR(13) + CHAR(10) +	'                                                AND eTKD_Yyksho11.YoyaSyu       =       1 '
						+ CHAR(13) + CHAR(10) +	'                                                ) '
						+ CHAR(13) + CHAR(10) +	'                                            ) '
--						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho11.SeikYm            =       '	+	@wk_SeikYm
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_YoyKbn      AS      eVPM_YoyKbn11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_Yyksho11.YoyaKbnSeq        =       eVPM_YoyKbn11.YoyaKbnSeq '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_YoyaKbnSort AS      VPM_YoyaKbnSort11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_YoyKbn11.YoyaKbnSeq       = VPM_YoyaKbnSort11.YoyaKbnSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND    VPM_YoyaKbnSort11.TenantCdSeq      = ' + @wk_TenantCdSeq 
						+ CHAR(13) + CHAR(10) +	'            INNER JOIN   VPM_Tokisk     AS      eVPM_Tokisk11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_Tokisk11.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk11.SiyoStaYmd '
						+ CHAR(13) + CHAR(10) +	'                                                                                AND     eVPM_Tokisk11.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Tokisk11.GyosyaCdSeq       =       eVPM_Gyosya11.GyosyaCdSeq '
						+ CHAR(13) + CHAR(10) +	'             JOIN   VPM_TokiSt          AS      eVPM_TokiSt11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_Yyksho11.TokuiSeq          =       eVPM_TokiSt11.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho11.SitenCdSeq        =       eVPM_TokiSt11.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt11.SiyoStaYmd '
						+ CHAR(13) + CHAR(10) +	'                                                                                AND     eVPM_TokiSt11.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Tokisk      AS      eVPM_Tokisk12 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_Tokisk12.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_Tokisk12.SiyoStaYmd '
						+ CHAR(13) + CHAR(10) +	'                                                                                AND     eVPM_Tokisk12.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Gyosya      AS      eVPM_Gyosya12 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Tokisk12.GyosyaCdSeq       =       eVPM_Gyosya12.GyosyaCdSeq '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho11.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd '
						+ CHAR(13) + CHAR(10) +	'                                                                                AND     eVPM_TokiSt12.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   TKD_FutTum      AS      eTKD_FutTum11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_Mishum.UkeNo                =       eTKD_FutTum11.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Mishum.FutuUnkRen           =       eTKD_FutTum11.UnkRen '
						+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Mishum.FutTumRen            =       eTKD_FutTum11.FutTumRen '
						+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Mishum.SeiFutSyu            <>      1 '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_FutTum11.SiyoKbn           =       1 '
						+ CHAR(13) + CHAR(10) +	'                                        AND (   (   TKD_Mishum.SeiFutSyu        =       6 '
						+ CHAR(13) + CHAR(10) +	'                                                AND eTKD_FutTum11.FutTumKbn     =       2 '
						+ CHAR(13) + CHAR(10) +	'                                                ) '
						+ CHAR(13) + CHAR(10) +	'                                            OR  (   TKD_Mishum.SeiFutSyu        <>      6 '
						+ CHAR(13) + CHAR(10) +	'                                                AND eTKD_FutTum11.FutTumKbn     =       1 '
						+ CHAR(13) + CHAR(10) +	'                                                ) '
						+ CHAR(13) + CHAR(10) +	'                                            ) '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   eTKD_Unkobi01   AS      eTKD_Unkobi11 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_Mishum.UkeNo                =       eTKD_Unkobi11.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Unkobi11.RowNumbr          =       1 '
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   TKD_Unkobi		 AS      eTKD_Unkobi12 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_FutTum11.UkeNo             =       eTKD_Unkobi12.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_FutTum11.UnkRen			 =       eTKD_Unkobi12.UnkRen '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Unkobi12.SiyoKbn           =       1 WHERE '
				IF		ISNULL(@SeiOutSyKbn	,1)	=	1
					BEGIN
					IF TRIM(@wk_SeikYm) <> ''
					BEGIN
					SET		@strSQL1		=	@strSQL1	+ ' eTKD_Yyksho11.SeikYm = ''' + @wk_SeikYm + ''' AND '
					END

					IF TRIM(@wk_StaUkeNo) <> ''
					BEGIN
					SET		@strSQL1		=	@strSQL1 +	'TKD_Mishum.UkeNo >= ''' + @wk_StaUkeNo + ''' AND '
					END
					
					IF TRIM(@wk_EndUkeNo) <> ''
					BEGIN
					SET		@strSQL1		=	@strSQL1 +	'TKD_Mishum.UkeNo <= ''' + @wk_EndUkeNo + ''' AND '
					END

					IF TRIM(@wk_StaYoyaKbn) <> '0'
					BEGIN
					SET		@strSQL1		=	@strSQL1 + 'eVPM_YoyKbn11.YoyaKbn >= ' + @wk_StaYoyaKbn + ' AND '
					END

					IF TRIM(@wk_EndYoyaKbn) <> '0'
					BEGIN
					SET		@strSQL1		=	@strSQL1 + 'eVPM_YoyKbn11.YoyaKbn <= ' + @wk_EndYoyaKbn + ' AND '
					END

					IF TRIM(@wk_SeiEigCdSeq) <> '0'
					BEGIN
					SET		@strSQL1		=	@strSQL1 + 'eTKD_Yyksho11.SeiEigCdSeq = ' + @wk_SeiEigCdSeq + ' AND '
					END

					IF TRIM(@wk_StartBillAdd) <> ''
					BEGIN
					SET		@strSQL1		=	@strSQL1 + '''' + COALESCE(@wk_StartBillAdd, '') + ''' <= CASE WHEN ISNULL(' + @wk_SeiSitKbn + ' ,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk12.TokuiCd,''0000''),FORMAT(eVPM_TokiSt12.SitenCd,''0000''))'
						+ ' ELSE CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk11.TokuiCd,''0000''),FORMAT(eVPM_TokiSt11.SitenCd,''0000'')) END AND '
					END

					IF TRIM(@wk_EndBillAdd) <> ''
					BEGIN
					SET		@strSQL1		=	@strSQL1 + '''' + COALESCE(@wk_EndBillAdd, '') + ''' >= CASE WHEN ISNULL(' + @wk_SeiSitKbn + ' ,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk12.TokuiCd,''0000''),FORMAT(eVPM_TokiSt12.SitenCd,''0000''))'
						+ ' ELSE CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk11.TokuiCd,''0000''),FORMAT(eVPM_TokiSt11.SitenCd,''0000'')) END AND '
					END

					IF TRIM(@wk_SimeD) <> '0'
					BEGIN
					SET		@strSQL1		=	@strSQL1 + ' eVPM_TokiSt12.SimeD = ' + @wk_SimeD
					END

					IF right(@strSQL1, 4) = 'AND '
					BEGIN
					SET		@strSQL1		=	@strSQL1 + ' 1 = 1 '
					END

						--SET		@strSQL1		=	@strSQL1	+ ' eTKD_Yyksho11.SeikYm = ''' + @wk_SeikYm + ''' AND '
						--+ 'TKD_Mishum.UkeNo >= ''' + @wk_StaUkeNo + ''' AND '
						--+ 'TKD_Mishum.UkeNo <= ''' + @wk_EndUkeNo + ''' AND '
						--+ 'eVPM_YoyKbn11.YoyaKbn >= ' + @wk_StaYoyaKbn + ' AND '
						--+ 'eVPM_YoyKbn11.YoyaKbn <= ' + @wk_EndYoyaKbn + ' AND '
						--+ 'eTKD_Yyksho11.SeiEigCdSeq = ' + @wk_SeiEigCdSeq + ' AND '''
						--+ COALESCE(@wk_StartBillAdd, '') + ''' <= CASE WHEN ISNULL(' + @wk_SeiSitKbn + ' ,1) = 1 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk12.TokuiCd,''0000''),FORMAT(eVPM_TokiSt12.SitenCd,''0000''))'
						--+ ' ELSE CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk11.TokuiCd,''0000''),FORMAT(eVPM_TokiSt11.SitenCd,''0000'')) END AND '''
						--+ COALESCE(@wk_EndBillAdd, '') + ''' >= CASE WHEN ISNULL(' + @wk_SeiSitKbn + ' ,1) = 2 THEN CONCAT(FORMAT(eVPM_Gyosya12.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk12.TokuiCd,''0000''),FORMAT(eVPM_TokiSt12.SitenCd,''0000''))'
						--+ ' ELSE CONCAT(FORMAT(eVPM_Gyosya11.GyosyaCd,''000'') ,FORMAT(eVPM_TokiSk11.TokuiCd,''0000''),FORMAT(eVPM_TokiSt11.SitenCd,''0000'')) END AND'
						--+ '	eVPM_TokiSt12.SimeD = ' + @wk_SimeD
						
					END
				
						---DELETE START 2016/07/19 MATSUOKA-----
						---
						----ADD START	2010/01/25	H.Takamiya
						--SET		@strSQL1		=	@strSQL1	+' and TKD_Mishum.SeiKin - TKD_Mishum.NyuKinRui <> 0'  
						----ADD END	2010/01/25	H.Takamiya			
						---DELETE E N D 2016/07/19 MATSUOKA-----					
				IF		ISNULL(@SeiOutSyKbn	,1)	=	2
					BEGIN
					IF TRIM(@wk_StaUkeNo) <> ''
					BEGIN
					SET		@strSQL1		=	@strSQL1 +	' TKD_Mishum.UkeNo >= ''' + @wk_StaUkeNo + ''' AND '
					END
					
					IF TRIM(@wk_EndUkeNo) <> ''
					BEGIN
					SET		@strSQL1		=	@strSQL1 +	' TKD_Mishum.UkeNo <= ''' + @wk_EndUkeNo + ''' AND '
					END

					IF TRIM(@wk_SeiEigCdSeq) <> '0'
					BEGIN
					SET		@strSQL1		=	@strSQL1 + ' eTKD_Yyksho11.SeiEigCdSeq = ' + @wk_SeiEigCdSeq + ' AND '
					END

					SET		@strSQL1		=	@strSQL1 + ' 1 = 1 '
						--SET		@strSQL1		=	@strSQL1	+	'TKD_Mishum.UkeNo BETWEEN ''' + @wk_StaUkeNo + ''' AND ''' + @wk_EndUkeNo + ''' AND eTKD_Yyksho11.SeiEigCdSeq = ' + @wk_SeiEigCdSeq
					END
				IF		ISNULL(@SeiOutSyKbn	,1)	=	3
					BEGIN
					IF TRIM(@wk_OutDataTable) <> ''
					BEGIN
						SET		@strSQL1		=	@strSQL1	+ 'CONCAT(TKD_Mishum.UkeNo, TKD_Mishum.SeiFutSyu, TKD_Mishum.FutuUnkRen, TKD_Mishum.FutTumRen)
						IN (Select * from  STRING_SPLIT(''' + @wk_OutDataTable + ''', '',''))'
					END
					END

				SET		@strSQL1				=	@strSQL1
						+ CHAR(13) + CHAR(10) +	') '
						+ CHAR(13) + CHAR(10) +	', '
									-- CTE QUERY --------------------------------------------------------------------------------------------------------------*
						+ CHAR(13) + CHAR(10) +	'eTKD_Mishum02    AS '
						+ CHAR(13) + CHAR(10) +	'( '
						+ CHAR(13) + CHAR(10) +	'    SELECT '
						+ CHAR(13) + CHAR(10) +	'        DENSE_RANK()    OVER    (   ORDER BY '
--						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum01.SeikYm '
						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum01.GyosyaCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.TokuiCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SitenCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'                                )                   AS      SeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   DENSE_RANK()    OVER    (   PARTITION BY '
--						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum01.SeikYm '
						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum01.GyosyaCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.TokuiCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SitenCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'                                    ORDER BY '
--						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum01.SeikYm '
						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum01.GyosyaCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.TokuiCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SitenCd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SeiTaiYmd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SeiFutSyu_Sort '
						-- 2013/11/26 S.Sato INSERT STR --
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.FutTum_Sort '
						-- 2013/11/26 S.Sato INSERT END --
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.HasYmd '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.SeiFutSyu '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum01.MisyuRen '
						+ CHAR(13) + CHAR(10) +	'                                )                   AS      SeiMeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum01.* '
						+ CHAR(13) + CHAR(10) +	'FROM '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum01 '
						+ CHAR(13) + CHAR(10) +	') '
						
						--2014/08/08 M.OHMORI STR
						SET		@strSQL1		=	@strSQL1	
						+ CHAR(13) + CHAR(10) + ' ,'
						+ CHAR(13) + CHAR(10) + ' eTKD_Kuri01 AS '
						+ CHAR(13) + CHAR(10) + '('
						+ CHAR(13) + CHAR(10) + ' SELECT '
						+ CHAR(13) + CHAR(10) + '	 ISNULL(eTKD_Kuri.SeinKbn,0) AS SeinKbn'
						+ CHAR(13) + CHAR(10) + '	,ISNULL(eTKD_Kuri.YoyaKbn,0) AS YoyaKbn'
						+ CHAR(13) + CHAR(10) + '	,ISNULL(eTKD_Kuri.SyoriYm,'''') AS SyoriYm'
						+ CHAR(13) + CHAR(10) + '	,ISNULL(eTKD_Kuri.SyoEigyoSeq,0) AS SyoEigyoSeq'
						+ CHAR(13) + CHAR(10) + '	,ISNULL(eTKD_Kuri.SyoTokuiSeq,0) AS SyoTokuiSeq'
						+ CHAR(13) + CHAR(10) + '	,ISNULL(eTKD_Kuri.SyoSitenSeq,0) AS SyoSitenSeq'
						+ CHAR(13) + CHAR(10) + '	,SUM(ISNULL(eTKD_Kuri.KuriKin,0)) AS KuriKin'
						+ CHAR(13) + CHAR(10) + ' FROM TKD_Kuri eTKD_Kuri'												
						+ CHAR(13) + CHAR(10) + ' WHERE eTKD_Kuri.SyoEigyoSeq =' + @wk_SeiEigCdSeq + ' AND eTKD_Kuri.SeinKbn = 2 AND eTKD_Kuri.YoyaKbn = 0 AND eTKD_Kuri.SiyoKbn = 1 AND eTKD_Kuri.SyoriYm = ''' + @wk_SeikYm + ''''

				IF		TRIM(@wk_BillingType) <> ''
					BEGIN
						SET		@strSQL1		=	@strSQL1	+	' AND eTKD_Kuri.SeiFutSyu IN(' +  @wk_BillingType + ')'
					END						
						
						set @strSQL1 = @strSQl1			
						+ CHAR(13) + CHAR(10) + ' Group By eTKD_Kuri.SeinKbn,eTKD_Kuri.YoyaKbn,eTKD_Kuri.SyoriYm,eTKD_Kuri.SyoEigyoSeq,eTKD_Kuri.SyoTokuiSeq'
						+ CHAR(13) + CHAR(10) + ',eTKD_Kuri.SyoSitenSeq'
						+ CHAR(13) + CHAR(10) + ') '
				
				SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + ','
						+ CHAR(13) + CHAR(10) +	' eTKD_Nyukin01 AS '
						+ CHAR(13) + CHAR(10) + '('
						+ CHAR(13) + CHAR(10) + ' SELECT '
						+ CHAR(13) + CHAR(10) + ' eVPM_TokiSt12.TokuiSeq								AS		TokuiSeq'
						+ CHAR(13) + CHAR(10) + ',eVPM_TokiSt12.SitenCdSeq								AS		SitenCdSeq'

--						+ CHAR(13) + CHAR(10) + ' eVPM_TokiSt12.TokuiSeq								AS		TokuiSeq'
--						+ CHAR(13) + CHAR(10) + ',eVPM_TokiSt12.SitenCdSeq								AS		SitenCdSeq'

				SET	@strSQL1 = @strSQL1
						+ CHAR(13) + CHAR(10) + ',SUM(eTKD_Nyshmi.Kesg + eTKD_Nyshmi.FurKesG) AS NyukinRui'
						---INSERT START 2016/07/19 MATSUOKA-----
						+ CHAR(13) + CHAR(10) + ',eTKD_Nyshmi.UkeNo '
						+ CHAR(13) + CHAR(10) + ',eTKD_Nyshmi.FutTumRen '
						+ CHAR(13) + CHAR(10) + ',eTKD_Nyshmi.SeiFutSyu '
						+ CHAR(13) + CHAR(10) + ',CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,''00'')) AS YoyaKbnSort '
						---INSERT E N D 2016/07/19 MATSUOKA-----
						+ CHAR(13) + CHAR(10) + ' FROM TKD_NyuSih eTKD_NyuSih'
						+ CHAR(13) + CHAR(10) + '			Left Join TKD_NyShmi		AS		eTKD_NyShmi'
						+ CHAR(13) + CHAR(10) + '										ON		eTKD_NyShmi.NyuSihTblSeq=eTKD_NyuSih.NyuSihTblSeq'
						+ CHAR(13) + CHAR(10) + '										AND		eTKD_NyShmi.SiyoKbn			=1'
						--INSERT STR  2017/07/11 A.Mizobuchi---
						+ CHAR(13) + CHAR(10) + '										and		eTKD_NyuSih.NyuSihKbn = 1 and eTKD_NyShmi.NyuSihKbn = 1'
						--INSERT END  2017/07/11 A.Mizobuchi---
						+ CHAR(13) + CHAR(10) + '			Left Join TKD_Yyksho		AS		eTKD_Yyksho'
						+ CHAR(13) + CHAR(10) + '										ON		eTKD_Yyksho.UkeNo			= eTKD_NyShmi.UkeNo'
						+ CHAR(13) + CHAR(10) + '										AND		eTKD_Yyksho.SiyoKbn			= 1'
									--  --------------------------------------------------------------------------------------------------------------*
				--2015/6/26 K.Kodama UPDATE STR 		
						--+ CHAR(13) + CHAR(10) +	'           LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt11 '
						--+ CHAR(13) + CHAR(10) +	'                                       ON      eTKD_Yyksho.TokuiSeq        =       eVPM_TokiSt11.TokuiSeq '
						--+ CHAR(13) + CHAR(10) +	'                                       AND     eTKD_Yyksho.SitenCdSeq      =       eVPM_TokiSt11.SitenCdSeq '
						--+ CHAR(13) + CHAR(10) +	'                                       AND     eTKD_Yyksho.SeiTaiYmd      BETWEEN  eVPM_TokiSt11.SiyoStaYmd '
						--+ CHAR(13) + CHAR(10) +	'                                                                           AND     eVPM_TokiSt11.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) +	'           INNER JOIN   VPM_TokiSt     AS      eVPM_TokiSt11 '
						+ CHAR(13) + CHAR(10) +	'                                       ON      eTKD_Yyksho.TokuiSeq        =       eVPM_TokiSt11.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                       AND     eTKD_Yyksho.SitenCdSeq      =       eVPM_TokiSt11.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'                                       AND     eTKD_Yyksho.SeiTaiYmd      BETWEEN  eVPM_TokiSt11.SiyoStaYmd '
						+ CHAR(13) + CHAR(10) +	'                                                                           AND     eVPM_TokiSt11.SiyoEndYmd '				
				--2015/6/26 K.Kodama UPDATE END
									--  --------------------------------------------------------------------------------------------------------------*
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt12 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_TokiSt11.SeiCdSeq          =       eVPM_TokiSt12.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eVPM_TokiSt11.SeiSitenCdSeq     =       eVPM_TokiSt12.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Yyksho.SeiTaiYmd         BETWEEN eVPM_TokiSt12.SiyoStaYmd '
						+ CHAR(13) + CHAR(10) + '																				AND		eVPM_TokiSt12.SiyoEndYmd '
						+ CHAR(13) + CHAR(10) + '	         LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn13 ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn13.YoyaKbnSeq '
						+ CHAR(13) + CHAR(10) + '            LEFT JOIN VPM_YoyaKbnSort AS VPM_YoyaKbnSort14 ON eVPM_YoyKbn13.YoyaKbnSeq = VPM_YoyaKbnSort14.YoyaKbnSeq AND VPM_YoyaKbnSort14.TenantCdSeq = ' + @wk_TenantCdSeq
						+ CHAR(13) + CHAR(10) + ' WHERE (eTKD_Yyksho.SeikYm <= ''' + @wk_SeikYm +        																																										
												''' AND eTKD_NyuSih.NyuSihYmd BETWEEN (CASE WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,( ''' + @wk_SeikYm + ''')) + ''01'' 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR,''' + @wk_SeikYm + ''' + RIGHT(''00'' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '''')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,(''' + @wk_SeikYm + ''' + RIGHT(''00'' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '''')), 2))))), 112) 																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,(''' + @wk_SeikYm + ''' )) + ''01''),112) END END ) 																																										
												AND (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,DATEADD(MONTH,1,CONVERT(VARCHAR,(''' + @wk_SeikYm + ''')) + ''01'')-1,112) 																																										
												ELSE 																																										
												CASE WHEN ISDATE(CONVERT(VARCHAR, ''' + @wk_SeikYm + ''' + RIGHT(''00'' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '''')), 2))) = 1 
												THEN CONVERT(VARCHAR, ''' + @wk_SeikYm + ''' + RIGHT(''00'' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '''')), 2)) 																																										
												ELSE CONVERT(VARCHAR, DATEADD(DAY, - 1, DATEADD(MONTH, 1, CONVERT(DATE, ''' + @wk_SeikYm + ''' + ''01''))), 112)  END END ))

												OR (eTKD_Yyksho.SeikYm = ''' + @wk_SeikYm + 																																										
												''' AND eTKD_NyuSih.NyuSihYmd < (CASE 																																										
												WHEN eVPM_TokiSt12.SimeD = 31 THEN CONVERT(VARCHAR,( ''' + @wk_SeikYm + ''')) + ''01'' 																																										
												ELSE CASE 																																										
												WHEN ISDATE(CONVERT(VARCHAR, ''' + @wk_SeikYm + ''' + RIGHT(''00'' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '''')), 2))) = 1 
												THEN CONVERT(VARCHAR, DATEADD(DAY, 1, DATEADD(MONTH, - 1,CONVERT(DATE,( ''' + @wk_SeikYm + ''' + RIGHT(''00'' + CONVERT(VARCHAR, ISNULL(eVPM_TokiSt12.SimeD, '''')), 2))))), 112)																																										
												ELSE CONVERT(VARCHAR,DATEADD(DAY,-1,CONVERT(VARCHAR,( ''' + @wk_SeikYm + ''')) + ''01''),112) END END ))
												AND eTKD_Nyusih.NyuSihKbn = 1 AND eTKD_Nyusih.SiyoKbn = 1'
				IF	TRIM(@wk_BillingType) <> ''
					BEGIN
						SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + ' AND eTKD_NyShmi.SeiFutSyu IN(' + @wk_BillingType + ')'
					END
				IF	TRIM(@wk_StaUkeNo) <> ''
					BEGIN
						SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + ' AND eTKD_Yyksho.UkeNo >= ''' + @wk_StaUkeNo + ''''
					END
				IF	TRIM(@wk_EndUkeNo) <> ''
					BEGIN
						SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + ' AND eTKD_Yyksho.UkeNo <= ''' + @wk_EndUkeNo + ''''
					END
				IF	TRIM(@wk_StaYoyaKbn) <> '0'
					BEGIN
						SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + ' AND eVPM_YoyKbn13.YoyaKbn >= ' + @wk_StaYoyaKbn
					END
				IF	TRIM(@wk_EndYoyaKbn) <> '0'
					BEGIN
						SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + ' AND eVPM_YoyKbn13.YoyaKbn <= ' + @wk_EndYoyaKbn
					END
				IF		ISNULL(@SeiSitKbn	,1) = 1
					BEGIN
						SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + '	Group By eVPM_TokiSt12.TokuiSeq,eVPM_TokiSt12.SitenCdSeq,
						eTKD_Nyshmi.UkeNo,eTKD_Nyshmi.SeiFutSyu,eTKD_Nyshmi.FutTumRen,
						CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,''00''))'
					END
				IF		ISNULL(@SeiSitKbn	,1) =2
					BEGIN
						SET		@strSQL1		=	@strSQL1
						+ CHAR(13) + CHAR(10) + '	GROUP BY eVPM_TokiSt11.TokuiSeq,eVPM_TokiSt11.SitenCdSeq,eTKD_Nyshmi.UkeNo,eTKD_Nyshmi.SeiFutSyu,eTKD_Nyshmi.FutTumRen,
						CONCAT(CASE WHEN VPM_YoyaKbnSort14.PriorityNum IS NULL THEN 1 ELSE 0 END, FORMAT(eVPM_YoyKbn13.YoyaKbn,''00''))'
					END
					
					SET			@strSQL1		=	@strSQL1
						---INSERT E N D 2016/07/19 MATSUOKA-----
						+ CHAR(13) + CHAR(10) + ') ' 
						--2014/08/08 M.OHMORI END

-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************
				SET		@strSQL_Seikyu	=	@strSQL_Seikyu
						+ CHAR(13) + CHAR(10) +	'INSERT  INTO '
						+ CHAR(13) + CHAR(10) +	'        TKD_Seikyu '
						+ CHAR(13) + CHAR(10) +	'SELECT '
						+ CHAR(13) + CHAR(10) +	'        '	+	@wk_SeiOutSeq	+				'    AS      SeiOutSeq '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiRen                        AS      SeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.TokuiSeq                      AS      TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SitenCdSeq                    AS      SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SiyoEndYmd                    AS      SiyoEndYmd '
--						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeikYm                        AS      SeikYm '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_SeikYm		+	''''	+	'  AS      SeikYm '
						--2014/08/08 M.OHMORI STR
						--+ CHAR(13) + CHAR(10) +	'    ,   0                                           AS      ZenKurG '
						+ CHAR(13) + CHAR(10) +	'    ,   isnull(eTKD_Kuri01.KuriKin,0)		         AS      ZenKurG '
						--2014/08/08 M.OHMORI END
						+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.UriGakKin)                AS      KonUriG '
						+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SyaRyoSyo)                AS      KonSyoG '
						+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SyaRyoTes)                AS      KonTesG '
						--2014/08/08 M.OHMORI STR
						--+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.NyuKinRui)                AS      KonNyuG '
						if @wk_SeiOutSyKbn = 1
						begin 
							--2015/03/10 M.OHMORI STR
							--set @strSQl_Seikyu = @strSQL_Seikyu
							--	+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_Nyukin01.NyuKinRui,0)   AS      KonNyuG '
							if @wk_kurisyorikbn = 1
								begin
									set @strSQl_Seikyu = @strSQL_Seikyu
									  ---UPDATE START 2016/07/19 MATSUOKA-----
										+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_Nyukin02.NyuKinRui,0)   AS      KonNyuG '
									  --+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_Nyukin01.NyuKinRui,0)   AS      KonNyuG '
									  ---UPDATE E N D 2016/07/19 MATSUOKA-----
								END
							if @wk_kurisyorikbn = 2
								begin
									set @strSQl_Seikyu = @strSQL_Seikyu
										+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.NyuKinRui)        AS      KonNyuG '
								end
							--2015/03/10 M.OHMORI END
					    end
					
					if @wk_SeiOutSyKbn = 2 or @wk_SeiOutSyKbn = 3
						Begin 
							set @strSQL_Seikyu = @strSQL_Seikyu
								+ CHAR(13) + CHAR(10) + '	,	SUM(eTKD_Mishum02.NyuKinRui)		 AS		 KouNyuG '
						End
					
						SET @strSQL_Seikyu = @strSQl_Seikyu
						--+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SeiKin) '
						--+ CHAR(13) + CHAR(10) +	'    -   SUM(eTKD_Mishum02.NyuKinRui)                AS      KonSeiG '
						--2014/08/11 M.OHMORI STR
						--+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SeiKin)					 AS      KonSeiG'
												
						---UPDATE START 2016/07/19 MATSUOKA-----
						if @wk_SeiOutSyKbn = 1
						BEGIN 
							IF @wk_kurisyorikbn = 1
								BEGIN   ---[]&[]
									SET @strSQl_Seikyu = @strSQL_Seikyu
									+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SeiKin) '
									+ CHAR(13) + CHAR(10) +	'    -   ISNULL(eTKD_Nyukin02.NyuKinRui,0)           AS      KonSeiG '
								END
							IF @wk_kurisyorikbn = 2
								BEGIN
									SET @strSQl_Seikyu = @strSQL_Seikyu
									+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SeiKin) '
									+ CHAR(13) + CHAR(10) +	'    -   SUM(eTKD_Mishum02.NyuKinRui)                AS      KonSeiG '
								END							
					    END					
						IF @wk_SeiOutSyKbn = 2 OR @wk_SeiOutSyKbn = 3
						BEGIN 
							SET @strSQL_Seikyu = @strSQL_Seikyu
							+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SeiKin) '
							+ CHAR(13) + CHAR(10) +	'    -   SUM(eTKD_Mishum02.NyuKinRui)                AS      KonSeiG '
						END
						SET @strSQL_Seikyu = @strSQl_Seikyu
						--+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.SeiKin) '
						--+ CHAR(13) + CHAR(10) +	'    -   SUM(eTKD_Mishum02.NyuKinRui)                AS      KonSeiG '
						---UPDATE E N D 2016/07/19 MATSUOKA-----
						
						--2014/08/08 M.OHMORI END
--						+ CHAR(13) + CHAR(10) +	'    ,   SUM(eTKD_Mishum02.UriGakKin) '
--						+ CHAR(13) + CHAR(10) +	'    +   SUM(eTKD_Mishum02.SyaRyoSyo) '
--						+ CHAR(13) + CHAR(10) +	'    +   SUM(eTKD_Mishum02.SyaRyoTes) '
--						+ CHAR(13) + CHAR(10) +	'    -   SUM(eTKD_Mishum02.NyuKinRui)                AS      KonSeiG '
						+ CHAR(13) + CHAR(10) +	'    ,   1                                           AS      SiyoKbn '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdYmd		+	''''	+	'  AS      UpdYmd '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdTime		+	''''	+	'  AS      UpdTime '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+				@wk_UpdSyainCd				+	'  AS      UpdSyainCd '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdPrgID	+	''''	+	'  AS      UpdPrgID '
						+ CHAR(13) + CHAR(10) +	'FROM '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum02 '
						--2014/08/08 M.OHMORI STR
						+ CHAR(13) + CHAR(10) + ' Left JOIn eTKD_Kuri01'
						+ CHAR(13) + CHAR(10) + ' ON eTKD_Kuri01.SyoTokuiSeq = eTKD_Mishum02.TokuiSeq'
						+ CHAR(13) + CHAR(10) + ' AND eTKD_Kuri01.SyoSitenSeq = eTKD_Mishum02.SitenCdSeq'

						---UPDATE START 2016/07/19 MATSUOKA-----
						+ CHAR(13) + CHAR(10) + ' LEFT JOIN  '
						+ CHAR(13) + CHAR(10) + '		(SELECT '
						+ CHAR(13) + CHAR(10) + '			 eTKD_Nyukin01.TokuiSeq AS TokuiSeq '
					    + CHAR(13) + CHAR(10) + '			,eTKD_Nyukin01.SitenCdSeq AS SitenCdSeq '
					    + CHAR(13) + CHAR(10) + '			,SUM(eTKD_Nyukin01.NyukinRui) AS NyukinRui '
						+ CHAR(13) + CHAR(10) + '		 FROM	eTKD_Nyukin01 '
						+ CHAR(13) + CHAR(10) + '		 GROUP  BY TokuiSeq,SitenCdSeq)  AS eTKD_Nyukin02 '
						+ CHAR(13) + CHAR(10) + '	ON eTKD_Nyukin02.TokuiSeq = eTKD_Mishum02.TokuiSeq'
						+ CHAR(13) + CHAR(10) + '	AND eTKD_Nyukin02.SitenCdSeq = eTKD_Mishum02.SitenCdSeq '
						--+ CHAR(13) + CHAR(10) + ' Left JOIN eTKD_Nyukin01'
						--+ CHAR(13) + CHAR(10) + ' ON eTKD_Nyukin01.TokuiSeq = eTKD_Mishum02.TokuiSeq'
						--+ CHAR(13) + CHAR(10) + ' AND eTKD_Nyukin01.SitenCdSeq = eTKD_Mishum02.SitenCdSeq '
						---UPDATE E N D 2016/07/19 MATSUOKA-----

						--2014/08/08 M.OHMORI END
						+ CHAR(13) + CHAR(10) +	'GROUP BY '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum02.SeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SiyoEndYmd '
--						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeikYm '
						--2014/08/08 M.OHMORI STR
						+ CHAR(13) + CHAR(10) + '	 ,	 eTKD_Kuri01.KuriKin '
						---UPDATE START 2016/07/19 MATSUOKA-----
						+ CHAR(13) + CHAR(10) + '	 ,	 eTKD_Nyukin02.NyukinRui '
						--+ CHAR(13) + CHAR(10) + '	 ,	 eTKD_Nyukin01.NyukinRui '
						---UPDATE E N D 2016/07/19 MATSUOKA-----
						--2014/08/08 M.OHMORI END
						+ CHAR(13) + CHAR(10) +	'ORDER BY '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum02.SeiRen '

-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************
				SET		@strSQL_SeiMei	=	@strSQL_SeiMei
						+ CHAR(13) + CHAR(10) +	'INSERT  INTO '
						+ CHAR(13) + CHAR(10) +	'        TKD_SeiMei '
						+ CHAR(13) + CHAR(10) +	'SELECT '
						+ CHAR(13) + CHAR(10) +	'        '	+	@wk_SeiOutSeq	+				'    AS      SeiOutSeq '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiRen                        AS      SeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiMeiRen                     AS      SeiMeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.UkeNo                         AS      UkeNo '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.MisyuRen                      AS      MisyuRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.UriGakKin                     AS      UriGakKin '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SyaRyoSyo                     AS      SyaRyoSyo '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SyaRyoTes                     AS      SyaRyoTes '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiKin                        AS      SeiKin '
						
						---UPDATE START 2016/07/19 MATSUOKA-----
						IF @wk_kurisyorikbn = 1
							BEGIN   ---[]&[]
								SET @strSQL_SeiMei	=	@strSQL_SeiMei
								+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_Nyukin01.NyuKinRui,0)		    AS      NyuKinRui '
							END
						IF @wk_kurisyorikbn = 2
							BEGIN
								SET @strSQL_SeiMei	=	@strSQL_SeiMei
								+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.NyuKinRui                     AS      NyuKinRui '
							END							
						SET		@strSQL_SeiMei	=	@strSQL_SeiMei
						--+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.NyuKinRui                     AS      NyuKinRui '
						---UPDATE E N D 2016/07/19 MATSUOKA-----
						
						+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_SeiMei11.SeiSaHKbn  ,1)         AS      SeiSaHKbn '
						--2019/11/05 M.OHMORI STR
						+ CHAR(13) + CHAR(10) + '	 ,	 ISNULL(eTKD_Mishum02.Zeiritsu, 0)			 AS	Zeiritsu'
						--2019/11/05 M.OHMORI END
						+ CHAR(13) + CHAR(10) +	'    ,   1                                           AS      SiyoKbn '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdYmd		+	''''	+	'  AS      UpdYmd '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdTime		+	''''	+	'  AS      UpdTime '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+				@wk_UpdSyainCd				+	'  AS      UpdSyainCd '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdPrgID	+	''''	+	'  AS      UpdPrgID '
						--2019/11/05 M.OHMORI STR
						----2019/09/18 M.OHMORI STR
						--+ CHAR(13) + CHAR(10) + '	 ,	 eTKD_Mishum02.Zeiritsu							AS	Zeiritsu'
						----2019/09/18 M.OHMORI END
						--2019/11/05 M.OHMORI END
						+ CHAR(13) + CHAR(10) +	'FROM '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum02 '
						+ CHAR(13) + CHAR(10) +	'        LEFT JOIN   eTKD_SeiMei01   AS      eTKD_SeiMei11 '
						+ CHAR(13) + CHAR(10) +	'                                    ON      eTKD_Mishum02.UkeNo         =       eTKD_SeiMei11.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                    AND     eTKD_Mishum02.MisyuRen      =       eTKD_SeiMei11.MisyuRen '
						---INSERT START 2016/07/19 MATSUOKA-----
						+ CHAR(13) + CHAR(10) +	'       LEFT JOIN eTKD_Nyukin01 '
						+ CHAR(13) + CHAR(10) +	'       	ON	eTKD_Nyukin01.TokuiSeq     =    eTKD_Mishum02.TokuiSeq '
						+ CHAR(13) + CHAR(10) +	'       	AND	eTKD_Nyukin01.SitenCdSeq   =    eTKD_Mishum02.SitenCdSeq '
						+ CHAR(13) + CHAR(10) +	'       	AND	eTKD_Nyukin01.UkeNo        =    eTKD_Mishum02.UkeNo '
						+ CHAR(13) + CHAR(10) +	'       	AND	eTKD_Nyukin01.SeiFutSyu    =    eTKD_Mishum02.SeiFutSyu '
						+ CHAR(13) + CHAR(10) +	'       	AND eTKD_Nyukin01.FutTumRen    =    eTKD_Mishum02.FutTumRen '
						---INSERT E N D 2016/07/19 MATSUOKA-----
	
						+ CHAR(13) + CHAR(10) +	'ORDER BY '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum02.SeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiMeiRen '

-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************
				SET		@strSQL_SeiUch	=	@strSQL_SeiUch
						+ CHAR(13) + CHAR(10) +	'INSERT  INTO '
						+ CHAR(13) + CHAR(10) +	'        TKD_SeiUch '
						+ CHAR(13) + CHAR(10) +	'SELECT '
						+ CHAR(13) + CHAR(10) +	'        '	+	@wk_SeiOutSeq	+				'    AS      SeiOutSeq '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiRen                        AS      SeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiMeiRen                     AS      SeiMeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   DENSE_RANK()    OVER    (   PARTITION BY '
						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum02.SeiRen '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum02.SeiMeiRen '
						+ CHAR(13) + CHAR(10) +	'                                    ORDER BY '
						+ CHAR(13) + CHAR(10) +	'                                                            eTKD_Mishum02.SeiRen '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       eTKD_Mishum02.SeiMeiRen '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       ISNULL(eTKD_YykSyu12.UnkRen,0) '
						+ CHAR(13) + CHAR(10) +	'                                                    ,       ISNULL(eTKD_YykSyu12.SyaSyuRen,0) '
						+ CHAR(13) + CHAR(10) +	'                                )                   AS      SeiUchRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.HasYmd                        AS      HasYmd '
						--2013/12/24 S.SATO UPDATE STR --
						--+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.YoyaNm                        AS      YoyaNm '
						--+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.FutTumNm                      AS      FutTumNm '
						--+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.HaiSYmd                       AS      HaiSYmd '
						--+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.TouYmd                        AS      TouYmd '
						+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_Unkobi13.DantaNm  ,eTKD_Mishum02.YoyaNm	) AS      YoyaNm '
						+ CHAR(13) + CHAR(10) +	'    ,   CASE '
						+ CHAR(13) + CHAR(10) +	'            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1,7) '
						+ CHAR(13) + CHAR(10) +	'            THEN    ISNULL(eTKD_Unkobi13.IkNm		 ,eTKD_Mishum02.FutTumNm	) '
						+ CHAR(13) + CHAR(10) +	'            ELSE    ISNULL(eTKD_Mishum02.FutTumNm   ,       '' '') '
						+ CHAR(13) + CHAR(10) +	'        END													  AS      FutTumNm '
						+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_Unkobi13.HaiSYmd  ,eTKD_Mishum02.HaiSYmd	) AS      HaiSYmd '
						+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_Unkobi13.TouYmd   ,eTKD_Mishum02.TouYmd	) AS      TouYmd '
						--2013/12/24 S.SATO UPDATE END --
						+ CHAR(13) + CHAR(10) +	'    ,   CASE '
						+ CHAR(13) + CHAR(10) +	'            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1) '
						+ CHAR(13) + CHAR(10) +	'            THEN    ISNULL(eTKD_YykSyu12.SyaSyuDai  ,       0) '
						+ CHAR(13) + CHAR(10) +	'            ELSE    eTKD_Mishum02.Suryo '
						+ CHAR(13) + CHAR(10) +	'        END                                         AS      Suryo '

						

						
						+ CHAR(13) + CHAR(10) +	'    ,   CASE '
						+ CHAR(13) + CHAR(10) +	'            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1) '
						+ CHAR(13) + CHAR(10) +	'            THEN    ISNULL(eTKD_YykSyu12.SyaSyuTan  ,       0) '
						+ CHAR(13) + CHAR(10) +	'            ELSE    eTKD_Mishum02.TanKa '
						+ CHAR(13) + CHAR(10) +	'        END                                         AS      TanKa '


						--2018/1/22 T.Fukao STA ADD-------------------------------------------------------------
						+ CHAR(13) + CHAR(10) +	'    ,   CASE '
						+ CHAR(13) + CHAR(10) +	'            WHEN    eTKD_Mishum02.SeiFutSyu         IN      (1) '
						+ CHAR(13) + CHAR(10) +	'            THEN    ISNULL( eVPM_SyaSyu12.SyaSyuNm  ,       '''') '
						+ CHAR(13) + CHAR(10) +	'            ELSE    ''''  '
						+ CHAR(13) + CHAR(10) +	'        END                                         AS      SyaSyuNm '
						--2018/1/22 T.Fukao STA ADD-------------------------------------------------------------

						+ CHAR(13) + CHAR(10) +	'    ,   1                                           AS      SiyoKbn '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdYmd		+	''''	+	'  AS      UpdYmd '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdTime		+	''''	+	'  AS      UpdTime '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+				@wk_UpdSyainCd				+	'  AS      UpdSyainCd '
						+ CHAR(13) + CHAR(10) +	'    ,   '	+	''''	+	@wk_UpdPrgID	+	''''	+	'  AS      UpdPrgID '
						+ CHAR(13) + CHAR(10) +	'FROM '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum02 '
						+ CHAR(13) + CHAR(10) +	'        LEFT JOIN   TKD_YykSyu      AS      eTKD_YykSyu12 '
						+ CHAR(13) + CHAR(10) +	'                                    ON      eTKD_Mishum02.UkeNo         =       eTKD_YykSyu12.UkeNo '
--						+ CHAR(13) + CHAR(10) +	'                                    AND     eTKD_Mishum02.FutuUnkRen    =       eTKD_YykSyu12.UnkRen '
						+ CHAR(13) + CHAR(10) +	'                                    AND     eTKD_YykSyu12.SiyoKbn       =       1 '
						+ CHAR(13) + CHAR(10) +	'                                    AND     eTKD_Mishum02.SeiFutSyu     =       1 '
						--2013/12/24 S.SATO INSERT STR --
						+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   TKD_Unkobi      AS      eTKD_Unkobi13 '
						+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_YykSyu12.UkeNo     =       eTKD_Unkobi13.UkeNo '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_YykSyu12.UnkRen	 =       eTKD_Unkobi13.UnkRen '
						+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Unkobi13.SiyoKbn   =       1 '
						--2013/12/24 S.SATO INSERT END --

							--2018/1/22 T.Fukao INSERT STA--
				        + CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_SyaSyu      AS      eVPM_SyaSyu12 '
				        + CHAR(13) + CHAR(10) +	'                                        ON      eVPM_SyaSyu12.SyaSyuCdSeq             =       eTKD_YykSyu12.SyaSyuCdSeq '
				        + CHAR(13) + CHAR(10) +	'                                        AND     eVPM_SyaSyu12.SiyoKbn        =       1 '		
				           --2018/1/22 T.Fukao INSERT END--

						
						+ CHAR(13) + CHAR(10) +	'ORDER BY '
						+ CHAR(13) + CHAR(10) +	'        eTKD_Mishum02.SeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   eTKD_Mishum02.SeiMeiRen '
						+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_YykSyu12.UnkRen,0) '
						+ CHAR(13) + CHAR(10) +	'    ,   ISNULL(eTKD_YykSyu12.SyaSyuRen,0) '

--					SELECT	(@strSQL1	+	@strSQL_Seikyu)	AS	Seikyu
--					SELECT	(@strSQL1	+	@strSQL_SeiMei)	AS	SeiMei
--					SELECT	(@strSQL1	+	@strSQL_SeiUch)	AS	SeiUch

--	DBG	---------------------------------------------------------------------------------------------------------
--					DECLARE	@strSQL_work	VARCHAR(MAX)

--					SET		@strSQL_work	=	@strSQL1	+	@strSQL_Seikyu
--					EXEC	DBG_HoLog
--							@UpdTableId		=	N'PK_bSeikys_R',
--							@LogMsg			=	@strSQL_work

--					SET		@strSQL_work	=	@strSQL1	+	@strSQL_SeiMei
--					EXEC	DBG_HoLog
--							@UpdTableId		=	N'PK_bSeikys_R',
--							@LogMsg			=	@strSQL_work

--					SET		@strSQL_work	=	@strSQL1	+	@strSQL_SeiUch
--					EXEC	DBG_HoLog
--							@UpdTableId		=	N'PK_bSeikys_R',
--							@LogMsg			=	@strSQL_work
--	DBG	---------------------------------------------------------------------------------------------------------
					EXEC	(@strSQL1	+	@strSQL_Seikyu)
					EXEC	(@strSQL1	+	@strSQL_SeiMei)
					EXEC	(@strSQL1	+	@strSQL_SeiUch)
					SET		@wk_SeiUch_InsCount		=	@@RowCount
			END

-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************

		SET		@strSQL1	=	' '
		SET		@strSQL1	=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'WITH '
				+ CHAR(13) + CHAR(10) +	'eTKD_SeiUch01   AS '
				+ CHAR(13) + CHAR(10) +	'( '
				+ CHAR(13) + CHAR(10) +	'    SELECT '
				+ CHAR(13) + CHAR(10) +	'            TKD_SeiUch.SeiOutSeq    AS      SeiOutSeq '
				+ CHAR(13) + CHAR(10) +	'        ,   TKD_SeiUch.SeiRen       AS      SeiRen '
				+ CHAR(13) + CHAR(10) +	'        ,   COUNT(*)                AS      MeisaiKensu '
				+ CHAR(13) + CHAR(10) +	'    FROM '
				+ CHAR(13) + CHAR(10) +	'            TKD_SeiUch '

		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
			OR	ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
				SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'    WHERE '
				+ CHAR(13) + CHAR(10) +	'            TKD_SeiUch.SeiOutSeq        =       '	+	@wk_SeiOutSeq
			END

		SET		@strSQL1	=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'    GROUP BY '
				+ CHAR(13) + CHAR(10) +	'            TKD_SeiUch.SeiOutSeq '
				+ CHAR(13) + CHAR(10) +	'        ,   TKD_SeiUch.SeiRen '
				+ CHAR(13) + CHAR(10) +	') '
				+ CHAR(13) + CHAR(10) +	', '
				--2011/12/01 M.OHMORI STA
				+ CHAR(13) + CHAR(10) + 'eTKD_SeiMei02 AS '
				+ CHAR(13) + CHAR(10) + '( '
				+ CHAR(13) + CHAR(10) + '	SELECT '
				+ CHAR(13) + CHAR(10) + '			ROW_NUMBER() OVER (PARTITION BY '
				+ CHAR(13) + CHAR(10) + '			TKD_Seimei.SeiOutSeq'
				+ CHAR(13) + CHAR(10) + '			ORDER BY '
				+ CHAR(13) + CHAR(10) + '				CASE WHEN JT_Yyksho.Yoyasyu=1'
				+ CHAR(13) + CHAR(10) + '				THEN JT_YykSho.SEITAIYMD'
				+ CHAR(13) + CHAR(10) + '				ELSE JT_YykSho.CanYMD'
				+ CHAR(13) + CHAR(10) + '				END) AS ROWNUM'
				+ CHAR(13) + CHAR(10) + '		,	TKD_SEIMEI.SEIOUTSEQ'
				+ CHAR(13) + CHAR(10) + '		,	(CASE WHEN JT_YykSho.Yoyasyu=1'
				+ CHAR(13) + CHAR(10) + '			THEN JT_YykSho.SeitaiYMD'
				+ CHAR(13) + CHAR(10) + '			ELSE JT_YykSho.CanYMD'
				+ CHAR(13) + CHAR(10) + '			END) AS SeiTaiYmd '
				+ CHAR(13) + CHAR(10) + '	FROM TKD_SeiMei'
				+ CHAR(13) + CHAR(10) + '	LEFT JOIN TKD_YykSho AS JT_YykSho'
				+ CHAR(13) + CHAR(10) + '	ON JT_YykSho.UkeNo=TKD_SeiMei.UkeNo'
				+ CHAR(13) + CHAR(10) + '	AND JT_YykSho.SiyoKbn=1'
				+ CHAR(13) + CHAR(10) + '	WHERE TKD_SeiMei.SeiOutSeq=' + @WK_SEIOUTSEQ
				+ CHAR(13) + CHAR(10) + ')'
				+ CHAR(13) + CHAR(10) + ','
				+ CHAR(13) + CHAR(10) + 'eVPM_CodeKb01 AS (SELECT CodeKbn, CodeKbnNm FROM VPM_CodeKb WHERE CodeSyu = ''YOKINSYU'' AND SiyoKbn = 1 AND TenantCdSeq = (
				SELECT CASE WHEN COUNT(*) = 0 THEN 0 ELSE ' + @wk_TenantCdSeq + ' END AS TenantCdSeq FROM VPM_CodeKb WHERE VPM_CodeKb.CodeSyu = ''YOKINSYU'' AND VPM_CodeKb.SiyoKbn = 1 AND
				VPM_CodeKb.TenantCdSeq = ' + @wk_TenantCdSeq + ')),'
				+ CHAR(13) + CHAR(10) +	'eTKD_Seikyu01   AS '
				+ CHAR(13) + CHAR(10) +	'( '
				+ CHAR(13) + CHAR(10) +	'    SELECT '
				+ CHAR(13) + CHAR(10) +	'            TKD_Seikyu.* '

		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
--			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
				SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_TokiSt11.ZipCd      ,'' ''  )   AS      ZipCd '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_TokiSt11.Jyus1      ,'' ''  )   AS      Jyus1 '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_TokiSt11.Jyus2      ,'' ''  )   AS      Jyus2 '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Tokisk11.TokuiNm    ,'' ''  )   AS      TokuiNm '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_TokiSt11.SitenNm    ,'' ''  )   AS      SitenNm '
			END

		IF		ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
				SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'        ,   '	+	''''	+	@wk_ZipCd	+	''''	+	'   AS      ZipCd '
				+ CHAR(13) + CHAR(10) +	'        ,   '	+	''''	+	@wk_Jyus1	+	''''	+	'   AS      Jyus1 '
				+ CHAR(13) + CHAR(10) +	'        ,   '	+	''''	+	@wk_Jyus2	+	''''	+	'   AS      Jyus2 '
				+ CHAR(13) + CHAR(10) +	'        ,   '	+	''''	+	@wk_TokuiNm	+	''''	+	'   AS      TokuiNm '
				+ CHAR(13) + CHAR(10) +	'        ,   '	+	''''	+	@wk_SitenNm	+	''''	+	'   AS      SitenNm '
			END

		IF		ISNULL(@SeiOutSyKbn	,1)	=	4
			BEGIN
				SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    ISNULL(eTKD_SeiPrS11.SeiOutSyKbn    ,0      )   =   1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_TokiSt11.ZipCd          ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiPrS11.ZipCd          ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      ZipCd '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    ISNULL(eTKD_SeiPrS11.SeiOutSyKbn    ,0      )   =   1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_TokiSt11.Jyus1          ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiPrS11.Jyus1          ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      Jyus1 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    ISNULL(eTKD_SeiPrS11.SeiOutSyKbn    ,0      )   =   1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_TokiSt11.Jyus2          ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiPrS11.Jyus2          ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      Jyus2 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    ISNULL(eTKD_SeiPrS11.SeiOutSyKbn    ,0      )   =   1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_Tokisk11.TokuiNm        ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiPrS11.TokuiNm        ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      TokuiNm '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    ISNULL(eTKD_SeiPrS11.SeiOutSyKbn    ,0      )   =   1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_TokiSt11.SitenNm        ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiPrS11.SitenNm        ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      SitenNm '
			END

				SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Eigyos11.ZipCd      ,'' ''  )   AS      SeiEigZipCd '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Eigyos11.Jyus1      ,'' ''  )   AS      SeiEigJyus1 '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Eigyos11.Jyus2      ,'' ''  )   AS      SeiEigJyus2 '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Eigyos11.EigyoNm    ,'' ''  )   AS      SeiEigEigyoNm '
				--2019/10/25 T.Matsubara STA 
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Tokist11.TokuiTanNm ,'' ''  )   AS      TokuiTanNm '
				--2019/10/25 T.Matsubara END
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Eigyos11.TelNo      ,'' ''  )   AS      SeiEigTelNo '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Eigyos11.FaxNo      ,'' ''  )   AS      SeiEigFaxNo '
--				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_SeiCom11.SeiCom1    ,'' ''  )   AS      SeiCom1 '
--				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_SeiCom11.SeiCom2    ,'' ''  )   AS      SeiCom2 '
--				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_SeiCom11.SeiCom3    ,'' ''  )   AS      SeiCom3 '
--				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_SeiCom11.SeiCom4    ,'' ''  )   AS      SeiCom4 '
--				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_SeiCom11.SeiCom5    ,'' ''  )   AS      SeiCom5 '
--				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_SeiCom11.SeiCom6    ,'' ''  )   AS      SeiCom6 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_Bank11.BankNm       ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      BankNm1 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_BankSt11.BankSitNm  ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      BankSitNm1 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_CodeKb11.CodeKbnNm  ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      YokinSyuNm1 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn1          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_Eigyos11.KouzaNo1   ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      KouzaNo1 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_Bank12.BankNm       ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      BankNm2 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_BankSt12.BankSitNm  ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      BankSitNm2 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_CodeKb12.CodeKbnNm  ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      YokinSyuNm2 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn2          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_Eigyos11.KouzaNo2   ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      KouzaNo2 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_Bank13.BankNm       ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      BankNm3 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_BankSt13.BankSitNm  ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      BankSitNm3 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_CodeKb13.CodeKbnNm  ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      YokinSyuNm3 '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eVPM_TokEig11.BankKbn3          IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    ISNULL(eVPM_Eigyos11.KouzaNo3   ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'                ELSE    '' '' '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      KouzaNo3 '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Eigyos11.KouzaMeigi ,'' ''  )   AS      KouzaMeigi '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eTKD_SeiUch11.MeisaiKensu,0      )   AS      MeisaiKensu '
						--	2008/08/20 ADD START ---
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eTKD_SeiPrS11.SeiHatYmd  ,'' ''  )   AS      SeiHatYmd '
				+ CHAR(13) + CHAR(10) +	'        ,   ISNULL(eVPM_Compny11.CompanyNm  ,'' ''  )   AS      SeiEigCompanyNm '
						--	2008/08/20 ADD E N D ---
				+ CHAR(13) + CHAR(10) +	'    FROM '
				+ CHAR(13) + CHAR(10) +	'            TKD_Seikyu '
						--	2008/08/20 ADD START ---
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   TKD_SeiPrS      AS      eTKD_SeiPrS11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_Seikyu.SeiOutSeq            =       eTKD_SeiPrS11.SeiOutSeq '
						--	2008/08/20 ADD E N D ---
				--2011/12/01 M.OHMORI STA
				--+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Tokisk      AS      eVPM_Tokisk11 '
				--+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_Seikyu.TokuiSeq             =       eVPM_Tokisk11.TokuiSeq '
				--+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Seikyu.SiyoEndYmd           BETWEEN eVPM_Tokisk11.SiyoStaYmd '
				--+ CHAR(13) + CHAR(10) +	'                                                                                AND     eVPM_Tokisk11.SiyoEndYmd '
				+ CHAR(13) + CHAR(10) + '			 LEFT JOIN eTKD_SeiMei02	AS		eTKD_SeiMei12'
				+ CHAR(13) + CHAR(10) + '										ON		TKD_Seikyu.SeiOutSeq=eTKD_SeiMei12.SeiOutSeq'
				+ CHAR(13) + CHAR(10) + '										AND		eTKD_SeiMei12.RowNum=1'
				+ CHAR(13) + CHAR(10) + '			 LEFT JOIN VPM_Tokisk		AS      eVPM_Tokisk11'
				+ CHAR(13) + CHAR(10) + '										ON		TKD_Seikyu.TokuiSeq				=	eVPM_Tokisk11.TokuiSeq'
				--UPDATE Sta 2017-02-15 K.Hayashi --
				--+ CHAR(13) + CHAR(10) + '										AND		eTKD_SeiMei12.SeiTaiYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd'
				+ CHAR(13) + CHAR(10) +	'                                      AND     TKD_Seikyu.SiyoEndYmd           BETWEEN eVPM_Tokisk11.SiyoStaYmd '				
				--UPDATE End 2017-02-15 K.Hayashi --
				+ CHAR(13) + CHAR(10) + '																				AND eVPM_Tokisk11.SiyoEndYmd'
				--2011/12/01 M.OHMORI END
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_TokiSt      AS      eVPM_TokiSt11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_Seikyu.TokuiSeq             =       eVPM_TokiSt11.TokuiSeq '
				+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Seikyu.SitenCdSeq           =       eVPM_TokiSt11.SitenCdSeq '
				+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Seikyu.SiyoEndYmd           BETWEEN eVPM_TokiSt11.SiyoStaYmd '
				+ CHAR(13) + CHAR(10) +	'                                                                                AND     eVPM_TokiSt11.SiyoEndYmd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Eigyos      AS      eVPM_Eigyos11 '
--				+ CHAR(13) + CHAR(10) +	'                                        ON      ' + @wk_SeiEigCdSeq + '         =       eVPM_Eigyos11.EigyoCdSeq '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_SeiPrS11.SeiEigCdSeq       =       eVPM_Eigyos11.EigyoCdSeq '
						--	2008/08/20 ADD START ---
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Compny      AS      eVPM_Compny11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Eigyos11.CompanyCdSeq      =       eVPM_Compny11.CompanyCdSeq '
						--	2008/08/20 ADD E N D ---
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_TokEig      AS      eVPM_TokEig11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_Seikyu.TokuiSeq             =       eVPM_TokEig11.TokuiSeq '
				+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Seikyu.SitenCdSeq           =       eVPM_TokEig11.SitenCdSeq '
				--Update START	2010/01/27 H.Takamiya	
				--+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_SeiPrS11.SeiEigCdSeq       =       eVPM_Eigyos11.EigyoCdSeq '
				+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_SeiPrS11.SeiEigCdSeq       =       eVPM_TokEig11.EigyoCdSeq '
				--Update E N D	2010/01/27 H.Takamiya	
				+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Seikyu.SiyoEndYmd           =       eVPM_TokEig11.SiyoEndYmd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Bank        AS      eVPM_Bank11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Eigyos11.BankCd1           =       eVPM_Bank11.BankCd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Bank        AS      eVPM_Bank12 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Eigyos11.BankCd2           =       eVPM_Bank12.BankCd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_Bank        AS      eVPM_Bank13 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Eigyos11.BankCd3           =       eVPM_Bank13.BankCd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_BankSt      AS      eVPM_BankSt11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Eigyos11.BankCd1           =       eVPM_BankSt11.BankCd '
				+ CHAR(13) + CHAR(10) +	'                                        AND     eVPM_Eigyos11.BankSitCd1        =       eVPM_BankSt11.BankSitCd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_BankSt      AS      eVPM_BankSt12 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Eigyos11.BankCd2           =       eVPM_BankSt12.BankCd '
				+ CHAR(13) + CHAR(10) +	'                                        AND     eVPM_Eigyos11.BankSitCd2        =       eVPM_BankSt12.BankSitCd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_BankSt      AS      eVPM_BankSt13 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eVPM_Eigyos11.BankCd3           =       eVPM_BankSt13.BankCd '
				+ CHAR(13) + CHAR(10) +	'                                        AND     eVPM_Eigyos11.BankSitCd3        =       eVPM_BankSt13.BankSitCd '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   eVPM_CodeKb01		 AS      eVPM_CodeKb11 '
				--+ CHAR(13) + CHAR(10) +	'                                        On      eVPM_CodeKb11.CodeCodeKbn       =       eVPM_Eigyos11.YokinSyu1 '
				+ CHAR(13) + CHAR(10) +	'                                        On     eVPM_Eigyos11.YokinSyu1         =       eVPM_CodeKb11.CodeKbn '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   eVPM_CodeKb01      AS      eVPM_CodeKb12 '
				--+ CHAR(13) + CHAR(10) +	'                                        On      eVPM_CodeKb12.CodeSyu           =       ''YOKINSYU'' '
				+ CHAR(13) + CHAR(10) +	'                                        On     eVPM_Eigyos11.YokinSyu2         =       eVPM_CodeKb12.CodeKbn '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   eVPM_CodeKb01      AS      eVPM_CodeKb13 '
				--+ CHAR(13) + CHAR(10) +	'                                        On      eVPM_CodeKb13.CodeSyu           =       ''YOKINSYU'' '
				+ CHAR(13) + CHAR(10) +	'                                        On     eVPM_Eigyos11.YokinSyu3         =       eVPM_CodeKb13.CodeKbn '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   eTKD_SeiUch01   AS      eTKD_SeiUch11 '
				+ CHAR(13) + CHAR(10) +	'                                        On      TKD_Seikyu.SeiOutSeq            =       eTKD_SeiUch11.SeiOutSeq '
				+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_Seikyu.SeiRen               =       eTKD_SeiUch11.SeiRen '
--				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   VPM_SeiCom      AS      eVPM_SeiCom11 '
--				+ CHAR(13) + CHAR(10) +	'                                        On      eVPM_SeiCom11.SeiComCd          =       1 '

		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
			OR	ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
				SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'    WHERE '
				+ CHAR(13) + CHAR(10) +	'            TKD_Seikyu.SeiOutSeq        =       '	+	@wk_SeiOutSeq
			END

		IF		ISNULL(@SeiOutSyKbn	,1)	=	4
			BEGIN
				SET		@strSQL1		=	@strSQL1	+	' WHERE TKD_Seikyu.SeiOutSeq = ' +  @wk_InvoiceOutNum + ' AND TKD_Seikyu.SeiRen = ' + @wk_InvoiceSerNum
			END
		SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	') '
				+ CHAR(13) + CHAR(10) +	'SELECT '
				+ CHAR(13) + CHAR(10) +	'        eTKD_Seikyu01.* '
				+ CHAR(13) + CHAR(10) +	'FROM '
				+ CHAR(13) + CHAR(10) +	'        eTKD_Seikyu01 '

--	DBG	---------------------------------------------------------------------------------------------------------
--		EXEC	DBG_HoLog
--				@UpdTableId		=	N'PK_bSeikys_R',
--				@LogMsg			=	@strSQL1
--	DBG	---------------------------------------------------------------------------------------------------------
		EXEC	(@strSQL1)

-- ****************************************************************************************************************************************
-- 
-- ****************************************************************************************************************************************
		SET		@strSQL1	=	' '
		SET		@strSQL1	=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'WITH '
				+ CHAR(13) + CHAR(10) +	'eTKD_SeiUch01   AS '
				+ CHAR(13) + CHAR(10) +	'( '
				+ CHAR(13) + CHAR(10) +	'    SELECT '
				+ CHAR(13) + CHAR(10) +	'            TKD_SeiUch.* '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                AND     TKD_SeiUch.SeiUchRen            <>      1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    0 '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiMei11.UriGakKin  ,0      ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      UriGakKin '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                AND     TKD_SeiUch.SeiUchRen            <>      1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    0 '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiMei11.SyaRyoSyo  ,0      ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      SyaRyoSyo '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                AND     TKD_SeiUch.SeiUchRen            <>      1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    0 '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiMei11.SyaRyoTes  ,0      ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      SyaRyoTes '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                AND     TKD_SeiUch.SeiUchRen            <>      1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    0 '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiMei11.SeiKin     ,0      ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      SeiKin '
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                AND     TKD_SeiUch.SeiUchRen            <>      1 '
				+ CHAR(13) + CHAR(10) +	'                THEN    0 '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_SeiMei11.NyuKinRui  ,0      ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      NyuKinRui '
				-- 2013/11/26 S.Sato INSERT STR --
				+ CHAR(13) + CHAR(10) +	'        ,   CASE '
				+ CHAR(13) + CHAR(10) +	'                WHEN    eTKD_Mishum11.SeiFutSyu         IN      (1) '
				+ CHAR(13) + CHAR(10) +	'                THEN    '' '' '
				+ CHAR(13) + CHAR(10) +	'                ELSE    ISNULL(eTKD_FutTum11.BikoNm     ,'' ''  ) '
				+ CHAR(13) + CHAR(10) +	'            END                                         AS      BikoNm '
				-- 2013/11/26 S.Sato INSERT END --
						--	2008/08/20 ADD START ---
				+ CHAR(13) + CHAR(10) +	'         ,   ISNULL(eTKD_SeiMei11.SeiSaHKbn  ,0      )  AS      SeiSaHKbn '
						--	2008/08/20 ADD E N D ---
				--ADD START 2010/01/29 H.Takamiya---
				+ CHAR(13) + CHAR(10) +	'         ,   ISNULL(eTKD_SeiMei11.UkeNo  ,0      )  AS      UkeNo '
				--ADD End	2010/01/29 H.Takamiya ---
				
				--INSERT START 2017/05/15 A.Nishizawa
				+ CHAR(13) + CHAR(10) +	'         ,   ISNULL(eTKD_FutTum11.IriRyoNm ,'' ''   )  AS      IriRyoNm '
				+ CHAR(13) + CHAR(10) +	'         ,   ISNULL(eTKD_FutTum11.DeRyoNm ,'' ''   )   AS      DeRyoNm '
				--INSERT E N D 2017/05/15 A.Nishizawa

				--INSERT START 2017/09/25 T.Fukao
				+ CHAR(13) + CHAR(10) +	'         ,   ISNULL(eTKD_Mishum11.SeiFutSyu ,0   )  AS      SeiFutSyu '
				--INSERT E N D 2017/09/25 T.Fukao
				--2018/12/14 S.Furuya Add Start
                + CHAR(13) + CHAR(10) +	'         ,    ISNULL(eVPM_Futai11.FutaiCd ,0) AS FutaiCd '
				--2018/12/14 S.Furuya Add End
				--2019/09/24 M.OHMORI STR
				+ CHAR(13) + CHAR(10) + '		  ,		CASE '
				+ CHAR(13) + CHAR(10) + '					WHEN eTKD_Mishum11.SeiFutSyu	IN			(1)'
				+ CHAR(13) + CHAR(10) + '					AND		TKD_SeiUch.SeiUchRen	<>			1'
				+ CHAR(13) + CHAR(10) + '					THEN	0'
				+ CHAR(13) + CHAR(10) + '					ELSE	ISNULL(eTKD_SeiMei11.Zeiritsu,0		)'
				+ CHAR(13) + CHAR(10) + '				END  AS Zeiritsu'
				--2019/09/24 M.OHMORI END
				+ CHAR(13) + CHAR(10) +	'    FROM '
				+ CHAR(13) + CHAR(10) +	'            TKD_SeiUch '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   TKD_SeiMei      AS      eTKD_SeiMei11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      TKD_SeiUch.SeiOutSeq            =       eTKD_SeiMei11.SeiOutSeq '
				+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_SeiUch.SeiRen               =       eTKD_SeiMei11.SeiRen '
				+ CHAR(13) + CHAR(10) +	'                                        AND     TKD_SeiUch.SeiMeiRen            =       eTKD_SeiMei11.SeiMeiRen '
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   TKD_Mishum      AS      eTKD_Mishum11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_SeiMei11.UkeNo             =       eTKD_Mishum11.UkeNo '
				+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_SeiMei11.MisyuRen          =       eTKD_Mishum11.MisyuRen '
				-- 2013/11/26 S.Sato INSERT STR --
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN   TKD_FutTum      AS      eTKD_FutTum11 '
				+ CHAR(13) + CHAR(10) +	'                                        ON      eTKD_Mishum11.UkeNo             =       eTKD_FutTum11.UkeNo '
				+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Mishum11.FutuUnkRen        =       eTKD_FutTum11.UnkRen '
				+ CHAR(13) + CHAR(10) +	'										 AND CASE '
				+ CHAR(13) + CHAR(10) +	'										 WHEN    eTKD_Mishum11.SeiFutSyu         =       6 '
				+ CHAR(13) + CHAR(10) +	'										 THEN    2 '
				+ CHAR(13) + CHAR(10) +	'										 ELSE    1 '
				+ CHAR(13) + CHAR(10) +	'										 END									 =       eTKD_FutTum11.FutTumKbn '
				+ CHAR(13) + CHAR(10) +	'                                        AND     eTKD_Mishum11.FutTumRen         =       eTKD_FutTum11.FutTumRen '
				-- 2013/11/26 S.Sato INSERT END --
				--2018/12/14 S.Furuya Add Start
				+ CHAR(13) + CHAR(10) +	'            LEFT JOIN  VPM_Futai AS eVPM_Futai11 '
				+ CHAR(13) + CHAR(10) +	'										 ON   eVPM_Futai11.FutaiCdSeq  =  eTKD_FutTum11.FutTumCdSeq '
				+ CHAR(13) + CHAR(10) +	'										 AND  eVPM_Futai11.SiyoKbn     =  1 '
				--2018/12/14 S.Furuya Add End
			

		IF		ISNULL(@SeiOutSyKbn	,1)	=	1
			OR	ISNULL(@SeiOutSyKbn	,1)	=	2
			OR	ISNULL(@SeiOutSyKbn	,1)	=	3
			BEGIN
				SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'    WHERE '
				+ CHAR(13) + CHAR(10) +	'            TKD_SeiUch.SeiOutSeq        =       '	+	@wk_SeiOutSeq
			END

		IF		ISNULL(@SeiOutSyKbn	,1)	=	4
			BEGIN
				SET		@strSQL1		=	@strSQL1	+	' WHERE TKD_SeiUch.SeiOutSeq = ' + @wk_InvoiceOutNum +	' AND  TKD_SeiUch.SeiRen = ' + @wk_InvoiceSerNum
			END
		SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	') '
				+ CHAR(13) + CHAR(10) +	'SELECT '
				+ CHAR(13) + CHAR(10) +	'            eTKD_SeiUch01.* '
				+ CHAR(13) + CHAR(10) +	'FROM '
				+ CHAR(13) + CHAR(10) +	'            eTKD_SeiUch01 '

		-- 2013/11/26 S.Sato INSERT STR --
		SET		@strSQL1		=	@strSQL1
				+ CHAR(13) + CHAR(10) +	'ORDER BY '
				+ CHAR(13) + CHAR(10) +	'	eTKD_SeiUch01.SeiOutSeq '
				+ CHAR(13) + CHAR(10) +	',	eTKD_SeiUch01.SeiRen '
				+ CHAR(13) + CHAR(10) +	',	eTKD_SeiUch01.SeiMeiRen '
				+ CHAR(13) + CHAR(10) +	',	eTKD_SeiUch01.SeiUchRen '
		-- 2013/11/26 S.Sato INSERT END --

--	DBG	---------------------------------------------------------------------------------------------------------
--		EXEC	DBG_HoLog
--				@UpdTableId		=	N'PK_bSeikys_R',
--				@LogMsg			=	@strSQL1
--	DBG	---------------------------------------------------------------------------------------------------------
		EXEC	(@strSQL1)
		SET		@wk_SeiUch_SelCount		=	@@RowCount


		IF		ISNULL(@SeiOutSyKbn	,1)	=	4
			BEGIN
				SELECT	@RowCount	=	@wk_SeiUch_SelCount
			END
		ELSE
			BEGIN
				SELECT	@RowCount	=	@wk_SeiUch_InsCount
			END

	END		TRY

	BEGIN	CATCH
		SET	@ReturnCd	=	ERROR_NUMBER()
		SET	@ReturnMsg	=	ERROR_MESSAGE()
		SET	@eProcedure	=	ERROR_PROCEDURE()
		SET	@eLine		=	ERROR_LINE()
	END		CATCH
	RETURN
