USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_SpMnYykReportCSV_R]    Script Date: 2021/04/07 14:34:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------
-- System-Name	:	新発車オーライシステムクラウド
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	PK_SpMnYykReportCSV_R
-- DB-Name		:	予約書テーブル、他
-- Name			:	SuperMenu予約編運行日データ取得処理
-- Date			:	2021/04/06 
-- Author		:	ntLanAnh
-- Descriotion	:	予約書テーブルその他のSelect処理
-- 				:	予約書に紐付けられる他テーブル情報も取得する
---------------------------------------------------

----------------------------------------------------

CREATE OR ALTER     PROCEDURE [dbo].[PK_SpMnYykReportCSV_R]
	@TenantCdSeq int,
    @CompanyCdSeq int,
	@StartDispatchDate nvarchar(8),
	@EndDispatchDate nvarchar(8),
	@StartArrivalDate nvarchar(8),
	@EndArrivalDate nvarchar(8),
	@StartReservationDate nvarchar(8),
	@EndReservationDate nvarchar(8),
	@StartReceiptNumber char(15),
	@EndReceiptNumber char(15),
	@StartReservationClassification int,
	@EndReservationClassification int,
	@StartServicePerson nvarchar(10),
	@EndServicePerson nvarchar(10),
	@StartRegistrationOffice int,
	@EndRegistrationOffice int,
	@StartInputPerson nvarchar(10),
	@EndInputPerson nvarchar(10),
	@StartCustomer nvarchar(11),
	@EndCustomer nvarchar(11),
	@StartSupplier nvarchar(11),
	@EndSupplier nvarchar(11),
	@StartGroupClassification nvarchar(10),
	@EndGroupClassification nvarchar(10),
	@StartCustomerTypeClassification int,
	@EndCustomerTypeClassification int,
	@StartDestination nvarchar(10),
	@EndDestination nvarchar(10),
	@StartDispatchPlace nvarchar(10),
	@EndDispatchPlace nvarchar(10),
	@StartOccurrencePlace nvarchar(10),
	@EndOccurrencePlace nvarchar(10),
	@StartArea nvarchar(10),
	@EndArea nvarchar(10),
	@StartReceiptCondition nvarchar(10),
	@EndReceiptCondition nvarchar(10),
	@StartCarType int,
	@EndCarType int,
	@StartCarTypePrice int,
	@EndCarTypePrice int,
	@DantaNm nvarchar(100),
	@MaxMinSetting int,
	@ReservationStatus int
AS
BEGIN
	DECLARE @LocTenantCdSeq int
	SET @LocTenantCdSeq = @TenantCdSeq

    DECLARE @LocCompanyCdSeq int
	SET @LocCompanyCdSeq = @CompanyCdSeq

	DECLARE @LocStartDispatchDate nvarchar(8)
	SET @LocStartDispatchDate = @StartDispatchDate

	DECLARE @LocEndDispatchDate nvarchar(8)
	SET @LocEndDispatchDate = @EndDispatchDate

	DECLARE @LocStartArrivalDate nvarchar(8)
	SET @LocStartArrivalDate = @StartArrivalDate

	DECLARE @LocEndArrivalDate nvarchar(8)
	SET @LocEndArrivalDate = @EndArrivalDate

	DECLARE @LocStartReservationDate nvarchar(8)
	SET @LocStartReservationDate = @StartReservationDate

	DECLARE @LocEndReservationDate nvarchar(8)
	SET @LocEndReservationDate = @EndReservationDate

	DECLARE @LocStartReceiptNumber char(15)
	SET @LocStartReceiptNumber = @StartReceiptNumber

	DECLARE @LocEndReceiptNumber char(15)
	SET @LocEndReceiptNumber = @EndReceiptNumber

	DECLARE @LocStartReservationClassification int
	SET @LocStartReservationClassification = @StartReservationClassification

	DECLARE @LocEndReservationClassification int
	SET @LocEndReservationClassification = @EndReservationClassification

	DECLARE @LocStartServicePerson nvarchar(10)
	SET @LocStartServicePerson = @StartServicePerson

	DECLARE @LocEndServicePerson nvarchar(10)
	SET @LocEndServicePerson = @EndServicePerson

	DECLARE @LocStartRegistrationOffice int
	SET @LocStartRegistrationOffice = @StartRegistrationOffice

	DECLARE @LocEndRegistrationOffice int
	SET @LocEndRegistrationOffice = @EndRegistrationOffice

	DECLARE @LocStartInputPerson nvarchar(10)
	SET @LocStartInputPerson = @StartInputPerson

	DECLARE @LocEndInputPerson nvarchar(10)
	SET @LocEndInputPerson = @EndInputPerson

	DECLARE @LocStartCustomer nvarchar(11)
	SET @LocStartCustomer = @StartCustomer

	DECLARE @LocEndCustomer nvarchar(11)
	SET @LocEndCustomer = @EndCustomer

	DECLARE @LocStartSupplier nvarchar(11)
	SET @LocStartSupplier = @StartSupplier

	DECLARE @LocEndSupplier nvarchar(11)
	SET @LocEndSupplier = @EndSupplier

	DECLARE @LocStartGroupClassification nvarchar(10)
	SET @LocStartGroupClassification = @StartGroupClassification

	DECLARE @LocEndGroupClassification nvarchar(10)
	SET @LocEndGroupClassification = @EndGroupClassification

	DECLARE @LocStartCustomerTypeClassification int
	SET @LocStartCustomerTypeClassification = @StartCustomerTypeClassification

	DECLARE @LocEndCustomerTypeClassification int
	SET @LocEndCustomerTypeClassification = @EndCustomerTypeClassification

	DECLARE @LocStartDestination nvarchar(10)
	SET @LocStartDestination = @StartDestination

	DECLARE @LocEndDestination nvarchar(10)
	SET @LocEndDestination = @EndDestination

	DECLARE @LocStartDispatchPlace nvarchar(10)
	SET @LocStartDispatchPlace = @StartDispatchPlace

	DECLARE @LocEndDispatchPlace nvarchar(10)
	SET @LocEndDispatchPlace = @EndDispatchPlace

	DECLARE @LocStartOccurrencePlace nvarchar(10)
	SET @LocStartOccurrencePlace = @StartOccurrencePlace

	DECLARE @LocEndOccurrencePlace nvarchar(10)
	SET @LocEndOccurrencePlace = @EndOccurrencePlace

	DECLARE @LocStartArea nvarchar(10)
	SET @LocStartArea = @StartArea

	DECLARE @LocEndArea nvarchar(10)
	SET @LocEndArea = @EndArea

	DECLARE @LocStartReceiptCondition nvarchar(10)
	SET @LocStartReceiptCondition = @StartReceiptCondition

	DECLARE @LocEndReceiptCondition nvarchar(10)
	SET @LocEndReceiptCondition = @EndReceiptCondition

	DECLARE @LocStartCarType int
	SET @LocStartCarType = @StartCarType

	DECLARE @LocEndCarType int
	SET @LocEndCarType = @EndCarType

	DECLARE @LocStartCarTypePrice int
	SET @LocStartCarTypePrice = @StartCarTypePrice

	DECLARE @LocEndCarTypePrice int
	SET @LocEndCarTypePrice = @EndCarTypePrice

	DECLARE @LocDantaNm nvarchar(100)
	SET @LocDantaNm = @DantaNm

	DECLARE @LocMaxMinSetting int
	SET @LocMaxMinSetting = @MaxMinSetting

	DECLARE @LocReservationStatus int
	SET @LocReservationStatus = @ReservationStatus

	BEGIN
	WITH																			
-- ＜取得＞付帯積込品情報を集計
eTKD_FutTum00	AS																
(																				
	SELECT																		
			eTKD_FutTum01.UkeNo				AS		UkeNo						
		,	5								AS		FutGuiKbn							-- 付帯料金区分（5：ガイド料、0：ガイド料以外）
		,	eTKD_FutTum01.Unkren			AS		Unkren						
		,	SUM(eTKD_FutTum01.UriGakKin)	AS		UriGakKin_S							-- 売上額集計値
		,	SUM(eTKD_FutTum01.SyaRyoSyo)	AS		SyaRyoSyo_S							-- 消費税額集計値
		,	SUM(eTKD_FutTum01.SyaRyoTes)	AS		SyaRyoTes_S							-- 手数料額集計値
		,	MIN(eTKD_Yyksyu01.SyaSyuRen)	AS		kSyaSyuRen	              			--SyasyuRenの最小値
	FROM																		
			TKD_FutTum						AS		eTKD_FutTum01				
			JOIN		VPM_Futai			AS		eVPM_Futai01				
											ON		eTKD_FutTum01.FutTumCdSeq	=		eVPM_Futai01.FutaiCdSeq	
												AND	eVPM_Futai01.FutGuiKbn		=		5							-- 付帯料金区分＝ガイド料
												AND eVPM_Futai01.TenantCdSeq	=		@LocTenantCdSeq
			LEFT JOIN		(	          		                            	
					 		SELECT ukeno ,Unkren ,min(SyaSyuRen) as SyaSyuRen FROM TKD_YykSyu    
					 		    WHERE TKD_YykSyu.SiyoKbn =1                     
					 		    group by UkeNo ,UnkRen                                 
					         )              AS      eTKD_YykSyu01               
				                	        ON      eTKD_FutTum01.UkeNo         =       eTKD_YykSyu01.UkeNo	    
				                	          AND   eTKD_FutTum01.UnkRen        =       eTKD_YykSyu01.UnkRen    
	WHERE																	
			eTKD_FutTum01.FutTumKbn			=		1						
		AND	eTKD_FutTum01.SiyoKbn			=		1						
	GROUP BY																
			eTKD_FutTum01.UkeNo 											
		,	eTKD_FutTum01.Unkren											
	UNION ALL																
	SELECT																	
			eTKD_FutTum02.UkeNo				AS		UkeNo					
		,	0								AS		FutGuiKbn						-- 付帯料金区分（5：ガイド料、0：ガイド料以外）
		,	eTKD_FutTum02.Unkren			AS		Unkren	                     
		,	SUM(eTKD_FutTum02.UriGakKin)	AS		UriGakKin_S						-- 売上額集計値
		,	SUM(eTKD_FutTum02.SyaRyoSyo)	AS		SyaRyoSyo_S						-- 消費税額集計値
		,	SUM(eTKD_FutTum02.SyaRyoTes)	AS		SyaRyoTes_S						-- 手数料額集計値
		,	MIN(eTKD_Yyksyu01.SyaSyuRen)	AS		kSyaSyuRen	              		--SyasyuRenの最小値	
	FROM																	
			TKD_FutTum						AS		eTKD_FutTum02			
			JOIN		VPM_Futai			AS		eVPM_Futai02			
											ON		eTKD_FutTum02.FutTumCdSeq	=		eVPM_Futai02.FutaiCdSeq
												AND	eVPM_Futai02.FutGuiKbn		<>		5							-- 付帯料金区分＝ガイド料以外
												AND eVPM_Futai02.TenantCdSeq	=		@LocTenantCdSeq
			LEFT JOIN		(	          		                            
					 		SELECT ukeno ,UnkRen ,min(SyaSyuRen) as SyaSyuRen FROM TKD_YykSyu    
					 		    WHERE TKD_YykSyu.SiyoKbn =1                     
					 		    group by UkeNo ,UnkRen                                  
					         )              AS      eTKD_YykSyu01               
				                	        ON      eTKD_FutTum02.UkeNo         =       eTKD_YykSyu01.UkeNo	    
				                	          AND   eTKD_FutTum02.UnkRen        =       eTKD_YykSyu01.UnkRen
	WHERE																	
			eTKD_FutTum02.FutTumKbn			=		1						
		AND	eTKD_FutTum02.SiyoKbn			=		1						
	GROUP BY																
			eTKD_FutTum02.UkeNo												
		,	eTKD_FutTum02.Unkren											
)																			
,																			
eTKD_YFutTu10	AS															
(																			
	SELECT																	
			TKD_YFutTu.UkeNo				AS		UkeNo					
		,	TKD_YFutTu.UnkRen				AS		UnkRen					
		,	TKD_YFutTu.FutTumKbn			AS		FutTumKbn				
		,	TKD_YFutTu.FutTumCdSeq			AS		FutTumCdSeq				
		,	TKD_YFutTu.HaseiKin				AS		HaseiKin				
		,	TKD_YFutTu.SyaRyoSyo			AS		SyaRyoSyo				
		,	TKD_YFutTu.SyaRyoTes			AS		SyaRyoTes				
		,	ISNULL(TKD_Yousha.JitaFlg	,1)	AS		JitaFlg					
		,	CASE															
				WHEN	TKD_YFutTu.FutTumKbn			=	2				
				THEN	0													
				WHEN	TKD_YFutTu.FutTumKbn			=	1				
					AND	ISNULL(VPM_Futai.FutGuiKbn	,0)	=	5				
				THEN	5													
				ELSE	0													
			END								AS		SeiFutSyu				
	FROM																	
			TKD_YFutTu														
			LEFT JOIN	TKD_Yousha			ON		TKD_YFutTu.UkeNo			=	TKD_Yousha.UkeNo	
												AND	TKD_YFutTu.UnkRen			=	TKD_Yousha.UnkRen	
												AND	TKD_YFutTu.YouTblSeq		=	TKD_Yousha.YouTblSeq
												AND	TKD_Yousha.SiyoKbn			=	1					
			LEFT JOIN	VPM_Futai			ON		TKD_YFutTu.FutTumCdSeq		=	VPM_Futai.FutaiCdSeq
											AND		VPM_Futai.TenantCdSeq		=	@LocTenantCdSeq
	WHERE																	
			TKD_YFutTu.SiyoKbn				=		1						
)																			
,																			
eTKD_YFutTu00	AS															
(																			
	SELECT																	
			eTKD_YFutTu10.UkeNo				AS		UkeNo					
		,	eTKD_YFutTu10.SeiFutSyu			AS		SeiFutSyu				
		,	SUM(eTKD_YFutTu10.HaseiKin	)	AS		HaseiKin_S				
		,	SUM(eTKD_YFutTu10.SyaRyoSyo	)	AS		SyaRyoSyo_S				
		,	SUM(eTKD_YFutTu10.SyaRyoTes	)	AS		SyaRyoTes_S				
	FROM																	
			eTKD_YFutTu10													
	WHERE																	
			eTKD_YFutTu10.JitaFlg			=		0						
	GROUP BY																
			eTKD_YFutTu10.UkeNo												
		,	eTKD_YFutTu10.SeiFutSyu											
)																			
,																			
eTKD_Haisha10	AS															
(																			
	SELECT																	
			TKD_Haisha.UkeNo							AS		UkeNo		
		,	TKD_Haisha.UnkRen							AS		UnkRen		
		,	TKD_Haisha.SyaSyuRen						AS		SyaSyuRen	

		,	TKD_Haisha.SyaRyoUnc						AS		SyaRyoUnc	
		,	TKD_Haisha.SyaRyoSyo						AS		SyaRyoSyo	
		,	TKD_Haisha.SyaRyoTes						AS		SyaRyoTes	
		,	TKD_Haisha.YoushaUnc						AS		YoushaUnc	
		,	TKD_Haisha.YoushaSyo						AS		YoushaSyo	
		,	TKD_Haisha.YoushaTes						AS		YoushaTes	
		,	CASE															
				WHEN	TKD_Haisha.YouTblSeq			=		0			--	１：自社
				THEN	1													
				WHEN	TKD_Haisha.YouTblSeq			<>		0			--	１：自社
					AND	ISNULL(TKD_Yousha.JitaFlg,1)	=		1			
				THEN	1													
				ELSE														--	０：他社
						0													
			END											AS		JitaFlg		
	FROM																	
			TKD_Haisha														
			LEFT JOIN	TKD_Yousha	ON	TKD_Haisha.UkeNo		=	TKD_Yousha.UkeNo	
									AND	TKD_Haisha.UnkRen		=	TKD_Yousha.UnkRen	
									AND	TKD_Haisha.YouTblSeq	=	TKD_Yousha.YouTblSeq
									AND	TKD_Yousha.SiyoKbn		=	1		
	WHERE																	
			TKD_Haisha.SiyoKbn		=	1									
)																			
,																			
eTKD_Haisha20	AS															
(																			
	SELECT																	
			eTKD_Haisha10.UkeNo							AS		UkeNo		
		,	eTKD_Haisha10.UnkRen						AS		UnkRen		
		,	eTKD_Haisha10.SyaSyuRen						AS		SyaSyuRen	
		,	eTKD_Haisha10.SyaRyoUnc						AS		SyaRyoUnc	
		,	eTKD_Haisha10.SyaRyoSyo						AS		SyaRyoSyo	
		,	eTKD_Haisha10.SyaRyoTes						AS		SyaRyoTes	
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	1						
				THEN	eTKD_Haisha10.SyaRyoUnc								
				ELSE	0													
			END											AS		JiSyaRyoUnc	
																			
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	1						
				THEN	eTKD_Haisha10.SyaRyoSyo								
				ELSE	0													
			END											AS		JiSyaRyoSyo	
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	1						
				THEN	eTKD_Haisha10.SyaRyoTes								
				ELSE	0													
			END											AS		JiSyaRyoTes	
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	0						
				THEN	eTKD_Haisha10.YoushaUnc								
				ELSE	0													
			END											AS		YoushaUnc	
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	0						
				THEN	eTKD_Haisha10.YoushaSyo								
				ELSE	0													
			END											AS		YoushaSyo	
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	0						
				THEN	eTKD_Haisha10.YoushaTes								
				ELSE	0													
			END											AS		YoushaTes	
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	0						
				THEN	1													
				ELSE	0													
			END											AS		YoushaDai	
		,	eTKD_Haisha10.JitaFlg						AS		JitaFlg		
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	0						
				THEN	1													
				ELSE	0													
			END											AS		YouExist	
		,	CASE															
				WHEN	eTKD_Haisha10.JitaFlg	=	1						
				THEN	1													
				ELSE	0													
			END											AS		JiShaExist	
	FROM																	
			eTKD_Haisha10													
)																			
,																			
eTKD_Haisha01	AS															
(																			
	SELECT																	
			eTKD_Haisha20.UkeNo							AS		UkeNo		
		,	eTKD_Haisha20.UnkRen						AS		UnkRen		
		,	eTKD_Haisha20.SyaSyuRen						AS		SyaSyuRen	
		,	SUM(eTKD_Haisha20.SyaRyoUnc		)			AS		SyaRyoUnc	
		,	SUM(eTKD_Haisha20.SyaRyoSyo		)			AS		SyaRyoSyo	
		,	SUM(eTKD_Haisha20.SyaRyoTes		)			AS		SyaRyoTes	
		,	SUM(eTKD_Haisha20.JiSyaRyoUnc	)			AS		JiSyaRyoUnc	
		,	SUM(eTKD_Haisha20.JiSyaRyoSyo	)			AS		JiSyaRyoSyo	
		,	SUM(eTKD_Haisha20.JiSyaRyoTes	)			AS		JiSyaRyoTes	
		,	SUM(eTKD_Haisha20.YoushaUnc		)			AS		YoushaUnc	
		,	SUM(eTKD_Haisha20.YoushaSyo		)			AS		YoushaSyo	
		,	SUM(eTKD_Haisha20.YoushaTes		)			AS		YoushaTes	
		,	SUM(eTKD_Haisha20.YoushaDai		)			AS		YoushaDai	
		,	MAX(eTKD_Haisha20.YouExist		)			AS		YouExist	
		,	MAX(eTKD_Haisha20.JiShaExist	)			AS		JiShaExist	
	FROM																	
			eTKD_Haisha20													
	GROUP BY																
			eTKD_Haisha20.UkeNo												
		,	eTKD_Haisha20.UnkRen											
		,	eTKD_Haisha20.SyaSyuRen											
)																			
,																
eTKD_Kaknin10	AS												
(																
	SELECT														
			eTKD_Kaknin01.UkeNo					AS		UkeNo	
		,	ISNULL(eTKD_Kaknin02.KingFlg,0)		AS		KingFlg	
	FROM														
		(														
			SELECT												
					TKD_Kaknin.UkeNo							
			FROM												
					TKD_Kaknin									
			WHERE												
					TKD_Kaknin.SiyoKbn		=		1			
			GROUP BY											
					TKD_Kaknin.UkeNo							
		)							AS	eTKD_Kaknin01			
			LEFT JOIN	TKD_Kaknin	AS	eTKD_Kaknin02									
									ON	eTKD_Kaknin01.UkeNo		=	eTKD_Kaknin02.UkeNo	
									AND	eTKD_Kaknin02.SiyoKbn	=	1					
									AND	eTKD_Kaknin02.KingFlg	=	1					

									GROUP BY eTKD_Kaknin01.UkeNo,		
											 eTKD_Kaknin02.KingFlg				
)																						
,																			
eTKD_Biko01 AS																
(																			
	SELECT																	
			TKD_Biko.UkeNo								AS		UkeNo		
		,	TKD_Biko.BikoTblSeq							AS		BikoTblSeq	
		,	TKD_Biko.BikoRen							AS		BikoRen		
		,	TKD_Biko.BikoNm								AS		BikoNm		
	FROM																	
			TKD_Biko														
	WHERE																	
			TKD_Biko.BikTblKbn		=	1									
		AND	TKD_Biko.SiyoKbn		=	1									
	GROUP BY																
			TKD_Biko.UkeNo													
		,	TKD_Biko.BikoTblSeq												
		,	TKD_Biko.BikoRen												
		,	TKD_Biko.BikoNm													
),

--＜取得＞車種情報(eTKD_YykSyu02)
eTKD_YykSyu02	AS	(																
	SELECT 																					
			eTKD_YykSyu01.UkeNo																
		,	eTKD_YykSyu01.UnkRen															
		,	eTKD_YykSyu01.SyaSyuRen															
		,	eTKD_YykSyu01.SyaSyuCdSeq														
		,	eVPM_SyaSyu01.SyaSyuCd															
		,	eVPM_SyaSyu01.SyaSyuNm															
		,	eTKD_YykSyu01.KataKbn															
		,	eVPM_CodeKb12.RyakuNm															
		,	eTKD_YykSyu01.SyaSyuDai															
	FROM																					
			TKD_YykSyu				AS		eTKD_YykSyu01									
			-- ＜検索＞(eVPM_SyaSyu01)
			LEFT JOIN	VPM_SyaSyu	AS		eVPM_SyaSyu01																
									ON		eTKD_YykSyu01.SyaSyuCdSeq					=		eVPM_SyaSyu01.SyaSyuCdSeq
									AND		eVPM_SyaSyu01.TenantCdSeq					=		@LocTenantCdSeq
			-- ＜検索＞(eVPM_CodeKb12)
			LEFT JOIN	VPM_CodeKb	AS		eVPM_CodeKb12																
									ON		eVPM_CodeKb12.CodeSyu						=		'KATAKBN'				
										AND	CONVERT(VARCHAR(10),eTKD_YykSyu01.KataKbn)	=		eVPM_CodeKb12.CodeKbn
										AND eVPM_CodeKb12.TenantCdSeq					=		(
																									SELECT CASE 
																											WHEN COUNT( * ) = 0 THEN 0 ELSE @LocTenantCdSeq
																											END AS TenantCdSeq
																									FROM VPM_CodeKb
																									WHERE VPM_CodeKb.CodeSyu = 'KATAKBN'
																											AND VPM_CodeKb.SiyoKbn = 1
																											AND VPM_CodeKb.TenantCdSeq = @LocTenantCdSeq
																									)
	WHERE
			eTKD_YykSyu01.SiyoKbn	=		1	
			AND (@LocStartCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd >= @LocStartCarType) -- 車種　開始
			AND (@LocEndCarType IS NULL OR eVPM_SyaSyu01.SyaSyuCd <= @LocEndCarType) -- 車種　開始
			AND (@LocStartCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan >= @LocStartCarTypePrice) -- 車種　開始
			AND (@LocEndCarTypePrice IS NULL OR eTKD_YykSyu01.SyaSyuTan <= @LocEndCarTypePrice) -- 車種　開始
)


SELECT
		eTKD_Yyksho01.UkeNo								AS		UkeNo					-- 受付番号
	,	ISNULL(eTKD_Unkobi01.UnkRen			,0		)	AS		UnkRen					-- 運行日連番
	,	ISNULL(eTKD_YykSyu01.SyaSyuRen		,0		)	AS		SyaSyuRen				-- 車種連番
	,	ISNULL(eVPM_YoyKbn01.YoyaKbn		,0		)	AS		YoyaKbn					-- 予約区分
	,	ISNULL(eVPM_YoyKbn01.YoyaKbnNm		,' '	)		AS		YoyaKbnNm				-- 予約区分名
	,	ISNULL(eTKD_Yyksho01.UkeYmd			,' '	)				AS		UkeYmd					-- 受付年月日
	,	ISNULL(eTKD_Yyksho01.KaknKais		,0	)					AS		KaknKais				-- 確認総回数
	,	ISNULL(eTKD_Yyksho01.KaktYmd		,' '	)					AS		KaktYmd					-- 確定年月日
	,	ISNULL(eVPM_Gyosya01.GyosyaCd		,0		)	AS		TokGyosyaCd				-- 得意先業者コード
	,	ISNULL(eVPM_Tokisk01.TokuiCd		,0		)	AS		TokCd					-- 得意先コード
	,	ISNULL(eVPM_TokiSt01.SitenCd		,0		)	AS		SitenCd					-- 得意先支店コード
	,	ISNULL(eVPM_Gyosya01.GyosyaNm		,' '	)		AS		TokGyosyaNm				-- 得意先業者コード名
	,	ISNULL(eVPM_Tokisk01.TokuiNm		,' '	)		AS		TokNm					-- 得意先名
	,	ISNULL(eVPM_TokiSt01.SitenNm		,' '	)		AS		SitenNm					-- 得意先支店名
	,	ISNULL(eVPM_Tokisk01.RyakuNm		,' '	)		AS		TokRyakuNm				-- 得意先略名
	,	ISNULL(eVPM_TokiSt01.RyakuNm		,' '	)		AS		SitenRyakuNm			-- 得意先支店略名
	,	ISNULL(eTKD_Yyksho01.TokuiTanNm		,' '	)			AS		TokuiTanNm				-- 得意先担当者名
	,	ISNULL(eTKD_Yyksho01.TokuiTel		,' '	)		AS		TokuiTel				-- 得意先電話番号
   ,	ISNULL(eTKD_Yyksho01.TokuiFax		,' '	)		AS      TokuiFax				-- 得意先ＦＡＸ番号
   ,	ISNULL(eTKD_Yyksho01.TokuiMail		,' '	)		AS      TokuiMail				-- 得意先メールアドレス
	,	ISNULL(eTKD_Unkobi01.DanTaNm		,' '	)		AS		DanTaNm					-- 団体名
	,	ISNULL(eTKD_Unkobi01.KanJNm			,' '	)		AS		KanJNm					-- 幹事氏名
	,	ISNULL(eTKD_Unkobi01.KanjJyus1		,' '	)		AS		KanjJyus1				-- 幹事住所１
   ,	ISNULL(eTKD_Unkobi01.KANjJyus2		,' '	)		AS      KanjJyus2				-- 幹事住所２
	,	ISNULL(eTKD_Unkobi01.KanjTel		,' '	)		AS		KanjTel					-- 幹事ＴＥＬ
	,	ISNULL(eTKD_Unkobi01.KanjFax		,' '	)		AS		KanjFax					-- 幹事ＦＡＸ
	,	ISNULL(eTKD_Unkobi01.KanjKeiNo		,' '	)		AS		KanjKeiNo				-- 幹事携帯番号
	,	ISNULL(eTKD_Unkobi01.KanjMail		,' '	)		AS		KanjMail				-- 幹事メールアドレス
	,	ISNULL(eTKD_Unkobi01.KanDMHFlg		,' '	)		AS		KanDMHFlg				-- 幹事ＤＭ発行フラグ
	,	ISNULL(eTKD_Unkobi01.IkNm			,' '	)		AS		IkNm					-- 行き先名
	,	ISNULL(eTKD_Unkobi01.HaiSYmd		,' '	)		AS		HaiSYmd					-- 配車日
	,	ISNULL(eTKD_Unkobi01.HaiSTime		,' '	)		AS		HaiSTime				-- 配車時間
	,	ISNULL(eVPM_CodeKb07.CodeKbn		,' '	)		AS		HaiSBunCd				-- 配車地分類コード
	,	ISNULL(eVPM_CodeKb07.CodeKbnNm		,' '	)		AS		HaiSBunNm				-- 配車地分類名
	,	ISNULL(eVPM_CodeKb07.RyakuNm		,' '	)		AS		HaiSBunRyakuNm			-- 配車地分類略名
	,	ISNULL(eVPM_Haichi01.HaiSCd			,' '	)		AS		HaiSCd					-- 配車地コード
	,	ISNULL(eTKD_Unkobi01.HaiSNm			,' '	)		AS		HaiSNm					-- 配車地名
	,	ISNULL(eTKD_Unkobi01.TouYmd			,' '	)		AS		TouYmd					-- 到着日
	,	ISNULL(eTKD_Unkobi01.TouChTime		,' '	)		AS		TouChTime				-- 到着時間
	,	ISNULL(eVPM_CodeKb08.CodeKbn		,' '	)		AS		TouChaBunCd				-- 到着地分類コード
	,	ISNULL(eVPM_CodeKb08.CodeKbnNm		,' '	)		AS		TouChaBunNm				-- 到着地分類名
	,	ISNULL(eVPM_CodeKb08.RyakuNm		,' '	)		AS		TouChaBunRyakuNm		-- 到着地分類略名
	,	ISNULL(eVPM_Haichi02.HaiSCd			,' '	)		AS		TouChaCd				-- 到着地コード
	,	ISNULL(eTKD_Unkobi01.TouNm			,' '	)		AS		TouNm					-- 到着地名
	,	ISNULL(eTKD_Unkobi01.SyuPaTime		,' '	)		AS		SyuPaTime				-- 出発時間
	,	ISNULL(eTKD_Unkobi01.DrvJin			,0	)		AS		DrvJin					-- 運転手数
	,	ISNULL(eTKD_Unkobi01.GuiSu			,0	)		AS		GuiSu					-- ガイド数
	,	ISNULL(eTKD_YykSyu01.SyaSyuCd		,0	)		AS		SyaSyuCd				-- 車種コード
	,	ISNULL(eTKD_YykSyu01.SyaSyuNm		,' '	)		AS		SyaSyuNm				-- 車種名
	,	ISNULL(eTKD_YykSyu01.KataKbn		,0	)		AS		KataKbn					-- 型区分
	,	ISNULL(eTKD_YykSyu01.RyakuNm		,' '	)		AS		KataKbnRyakuNm			-- 型区分略名
	,	ISNULL(eTKD_YykSyu01.SyaSyuDai		,0	)		AS		SyaSyuDai				-- 台数
	,   ISNULL(eTKD_Unkobi01.UnitPriceIndex ,0  )       AS      UnitPriceIndex			-- 指数
	,	ISNULL(eTKD_Haisha01.SyaRyoUnc		,0	)		AS		SyaRyoUnc				-- 運賃売上額
	,	ISNULL(eTKD_Haisha01.SyaRyoSyo		,0	)		AS		SyaRyoSyo				-- 運賃消費税額
	,	ISNULL(eTKD_Haisha01.SyaRyoTes		,0	)		AS		SyaRyoTes				-- 運賃手数料
	,	ISNULL(eTKD_FutTum01.UriGakKin_S	,0	)		AS		Gui_UriGakKin_S			-- ガイド料売上額
	,	ISNULL(eTKD_FutTum01.SyaRyoSyo_S	,0	)		AS		Gui_SyaRyoSyo_S			-- ガイド料消費税額
	,	ISNULL(eTKD_FutTum01.SyaRyoTes_S	,0	)		AS		Gui_SyaRyoTes_S			-- ガイド料手数料
	,	ISNULL(eTKD_FutTum02.UriGakKin_S	,0	)		AS		Oth_UriGakKin_S			-- その他付帯売上額
	,	ISNULL(eTKD_FutTum02.SyaRyoSyo_S	,0	)		AS		Oth_SyaRyoSyo_S			-- その他付帯消費税額
	,	ISNULL(eTKD_FutTum02.SyaRyoTes_S	,0	)		AS		Oth_SyaRyoTes_S			-- その他付帯手数料
	,	ISNULL(eTKD_Haisha01.YoushaDai		,0	)		AS		YouDai					-- 傭車台数
	,	ISNULL(eTKD_Haisha01.YoushaUnc		,0	)		AS		YoushaUnc				-- 傭車運賃額
	,	ISNULL(eTKD_Haisha01.YoushaSyo		,0	)		AS		YoushaSyo				-- 傭車消費税額
	,	ISNULL(eTKD_Haisha01.YoushaTes		,0	)		AS		YoushaTes				-- 傭車手数料
	,	ISNULL(eTKD_YFutTu01.HaseiKin_S		,0	)		AS		YGui_HaseiKin_S			-- 傭車ガイド料発生額
	,	ISNULL(eTKD_YFutTu01.SyaRyoSyo_S	,0	)		AS		YGui_SyaRyoSyo_S		-- 傭車ガイド料消費税
	,	ISNULL(eTKD_YFutTu01.SyaRyoTes_S	,0	)		AS		YGui_SyaRyoTes_S		-- 傭車ガイド料手数料額
	,	ISNULL(eTKD_YFutTu02.HaseiKin_S		,0	)		AS		YOth_HaseiKin_S			-- 傭車その他付帯発生額
	,	ISNULL(eTKD_YFutTu02.SyaRyoSyo_S	,0	)		AS		YOth_SyaRyoSyo_S		-- 傭車その他付帯消費税
	,	ISNULL(eTKD_YFutTu02.SyaRyoTes_S	,0	)		AS		YOth_SyaRyoTes_S		-- 傭車その他付帯手数料額
	,	ISNULL(eTKD_Unkobi01.JyoSyaJin		,0	)		AS		JyoSyaJin				-- 乗車人員
	,	ISNULL(eTKD_Unkobi01.PlusJin		,0	)		AS		PlusJin					-- 乗車プラス人員
	,	ISNULL(eVPM_CodeKb05.CodeKbn		,' '	)		AS		SeiKyuKbn				-- 請求区分
	,	ISNULL(eVPM_CodeKb05.RyakuNm		,' '	)		AS		SeiKyuKbnRyakuNm		-- 請求区分略名
	,	ISNULL(eTKD_Yyksho01.SeiTaiYmd		,' '	)					AS		SeiTaiYmd				-- 請求対象年月日
	,	ISNULL(eVPM_Eigyos01.EigyoCd		,0	)		AS		UkeEigCd				-- 受付営業所コード
	,	ISNULL(eVPM_Eigyos01.EigyoNm		,' '	)		AS		UkeEigNm				-- 受付営業所名
	,	ISNULL(eVPM_Eigyos01.RyakuNm		,' '	)		AS		UkeEigRyakuNm			-- 受付営業所略名
	,	ISNULL(eVPM_Syain01.SyainCd			,' '	)		AS		EigTanSyainCd			-- 営業担当者社員コード
	,	ISNULL(eVPM_Syain01.SyainNm			,' '	)		AS		EigTanSyainNm			-- 営業担当者社員名
	,	ISNULL(eVPM_Syain02.SyainCd			,' '	)		AS		InputTanSyainCd			-- 入力担当者社員コード
	,	ISNULL(eVPM_Syain02.SyainNm			,' '	)		AS		InputTanSyainNm			-- 入力担当者社員名
	,	ISNULL(eVPM_CodeKb09.CodeKbnNm		,' '	)		AS		HasKenNm				-- 発生地県名
	,	ISNULL(eVPM_Basyo02.BasyoMapCd		,' '	)		AS		HasMapCd				-- 発生地コード
	,	ISNULL(eTKD_Unkobi01.HasNm			,' '	)		AS		HasNm					-- 発生地名
	,	ISNULL(eVPM_CodeKb10.CodeKbnNm		,' '	)		AS		AreaKenNm				-- エリア県名
	,	ISNULL(eVPM_Basyo03.BasyoMapCd		,' '	)		AS		AreaMapCd				-- エリアコード
	,	ISNULL(eTKD_Unkobi01.AreaNm			,' '	)		AS		AreaNm					-- エリア名
	,	ISNULL(eVPM_Gyosya03.GyosyaNm		,' '  )		AS		SirGyosyaNm				-- 仕入業者名
	,	ISNULL(eVPM_Gyosya03.GyosyaCd		,0  )		AS		SirGyosyaCd				-- 仕入先業者コード
	,	ISNULL(eVPM_Tokisk03.TokuiCd		,0  )		AS		SirCd					-- 仕入先コード
	,	ISNULL(eVPM_Tokisk03.TokuiNm		,' '  )		AS		SirNm					-- 仕入先名
	,	ISNULL(eVPM_Tokisk03.RyakuNm		,' '  )		AS		SirRyakuNm				-- 仕入先略名
	,	ISNULL(eVPM_TokiSt03.SitenCd		,0  )		AS		SirSitenCd				-- 仕入先支店コード
	,	ISNULL(eVPM_TokiSt03.SitenNm		,' '  )		AS		SirSitenNm				-- 仕入先支店名
	,	ISNULL(eVPM_TokiSt03.RyakuNm		,' '  )		AS		SirSitenRyakuNm			-- 仕入先支店略名
	,	ISNULL(eTKD_Unkobi01.UkeJyKbnCd		,0  )				AS		UkeJyKbn				-- 受付条件
	,	ISNULL(eTKD_Unkobi01.UnkoJKbn		,0  )					AS		UnkoJKbn				-- 運行条件
	,	ISNULL(eTKD_Unkobi01.SijJoKbn1		,0  )					AS		SijJoKbn1				-- 指示条件1
	,	ISNULL(eTKD_Unkobi01.SijJoKbn2		,0  )					AS		SijJoKbn2				-- 指示条件2
	,	ISNULL(eTKD_Unkobi01.SijJoKbn3		,0  )					AS		SijJoKbn3				-- 指示条件3
	,	ISNULL(eTKD_Unkobi01.SijJoKbn4		,0  )					AS		SijJoKbn4				-- 指示条件4
	,	ISNULL(eTKD_Unkobi01.SijJoKbn5		,0  )					AS		SijJoKbn5				-- 指示条件5
	,	ISNULL(eVPM_CodeKb11.CodeKbn		,' '  )					AS		DantaiCd				--団体区分コード
	,	ISNULL(eVPM_CodeKb11.CodeKbnNm		,' '  )					AS		DantaiCdNm				--団体区分名
	,	ISNULL(eVPM_JyoKya01.JyoKyakuCd		,0  )				AS		JyoKyakuCd				--乗客区分コード
	,	ISNULL(eVPM_JyoKya01.JyoKyakuNm		,' '  )				AS		JyoKyakuNm				--乗客区分名
	,	ISNULL(eTKD_Biko01.BikoNm			,' '  )					AS		BikoNm					--備考
	,	ISNULL(eTKD_Yyksho01.Zeiritsu		,0  )		AS		Zeiritsu				--消費税率
	,	ISNULL(eTKD_Yyksho01.UkeEigCdSeq	,0  )					AS	UkeEigCdSeq	    			-- 受付営業CdSeq
	,	ISNULL(eTKD_Kariei.KSEigSeq	,eTKD_Yyksho01.UkeEigCdSeq)		    
														AS	KariEigCdSeq				-- 仮車営業所コードSEQ
	,   ISNULL(eVPM_Eigyos00.EigyoCd		,' ')		AS  KariEigCd					-- 仮車営業所コード
	,   ISNULL(eVPM_Eigyos00.EigyoNm        ,' ')		AS  KariEigNm					-- 仮車営業所名
	,	ISNULL(eTKD_Unkobi01.ZenHaFlg		,0 )		AS	ZenHaFlg		    		-- 前泊フラグ
	,	ISNULL(eTKD_Unkobi01.KhakFlg		,0 )		AS	KhakFlg			    		-- 後泊フラグ

	,	ISNULL(eTKD_YykReport.AllSokoTime	,' '  )		AS		YRep_AllSokoTime		--総走行時間
	,	ISNULL(eTKD_YykReport.CheckTime		,' '  )		AS		YRep_CheckTime			--点検時間
	,	ISNULL(eTKD_YykReport.AdjustTime	,' '  )		AS		YRep_AdjustTime			--調整時間
	,	ISNULL(eTKD_YykReport.ShinSoTime	,' '  )		AS		YRep_ShinSoTime			--深夜早朝時間
	,	ISNULL(eTKD_YykReport.AllSokoKm		,0  )				AS		YRep_AllSokoKm			--総走行キロ
	,	ISNULL(eTKD_YykReport.JiSaTime		,' '  )		AS		YRep_JiSaTime			--実車時間
	,	ISNULL(eTKD_YykReport.JiSaKm		,0  )					AS		YRep_JiSaKm				--実車キロ
	,	ISNULL(eTKD_YykReport.WaribikiKbn	,0  )					AS		YRep_WaribikiKbn		--割引区分
	,	ISNULL(eTKD_YykReport.ChangeFlg		,0  )				AS		YRep_ChangeFlg			--交替運転者配置料金フラグ
	,	ISNULL(eTKD_YykReport.ChangeKoskTime	,' '  )				AS		YRep_ChangeKoskTime		--交替運転者拘束時間
	,	ISNULL(eTKD_YykReport.ChangeShinTime	,' '  )				AS		YRep_ChangeShinTime		--交替運転者深夜時間
	,	ISNULL(eTKD_YykReport.ChangeSokoKm		,0  )				AS		YRep_ChangeSokoKm		--交替運転者走行キロ
	,	ISNULL(eTKD_YykReport.SpecialFlg		,0  )				AS		YRep_SpecialFlg			--特殊車両料金フラグ
    ,   ISNULL(CONVERT(INTEGER,SUBSTRING(eTKD_UnkobiExp01.ExpItem,9,5))		,0)			AS UExp_SouTotalKm     -- 総走行距離
    ,   ISNULL(CONVERT(INTEGER,SUBSTRING(eTKD_UnkobiExp01.ExpItem,14,5))	,0)			AS UExp_JituKm         -- 実車走行距離
    ,   ISNULL(SUBSTRING(eTKD_UnkobiExp01.ExpItem,19,5)						,'00000')	AS UExp_SumTime        -- 総走行時間
    ,   ISNULL(SUBSTRING(eTKD_UnkobiExp01.ExpItem,24,5)						,'00000')	AS UExp_JituTime       -- 実車走行時間
    ,   ISNULL(SUBSTRING(eTKD_UnkobiExp01.ExpItem,29,5)						,'00000')	AS UExp_ShinSoTime     -- 深夜早朝時間
    ,   ISNULL(SUBSTRING(eTKD_UnkobiExp01.ExpItem,34,1)						,0)			AS UExp_ChangeFlg      -- 交替運転者配置料金フラグ
    ,   ISNULL(SUBSTRING(eTKD_UnkobiExp01.ExpItem,35,1)						,0)			AS UExp_SpecialFlg     -- 特殊車両料金フラグ
    ,	ISNULL(SUBSTRING(eTKD_UnkobiExp01.ExpItem,78,1)						,0)			AS UExp_YearContractFlg --年間契約フラグ
	,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.FareMaxAmount				,0)			AS FareMaxAmount		-- 上限運賃
	,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.FareMinAmount				,0)			AS FareMinAmount		-- 下限運賃
	,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.FeeMaxAmount					,0)			AS FeeMaxAmount			-- 上限料金
	,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.FeeMinAmount					,0)			AS FeeMinAmount			-- 下限料金
	,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMaxAmount			,0)			AS UnitPriceMaxAmount	-- 上限金額
	,	ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMinAmount			,0)			AS UnitPriceMinAmount	-- 下限金額
FROM																									
		TKD_Yyksho					AS		eTKD_Yyksho01												
		LEFT JOIN	eTKD_Kaknin10	ON		eTKD_Yyksho01.UkeNo			=		eTKD_Kaknin10.UkeNo		
		-- ＜検索/取得＞（eTKD_Unkobi01）
		LEFT JOIN	TKD_Unkobi		AS		eTKD_Unkobi01												
									ON		eTKD_Yyksho01.UkeNo			=		eTKD_Unkobi01.UkeNo		
										AND	eTKD_Unkobi01.SiyoKbn		=		1						
		-- ＜検索/取得＞条件設定の車種コード、車種別単価、型区分に該当する予約車種情報を取得する(eTKD_YykSyu01)
		-- サブクエリ内で抽出した結果で運行日情報の検索を行うためJOIN
		JOIN	eTKD_YykSyu02		AS		eTKD_YykSyu01												
									ON		eTKD_Unkobi01.UkeNo			=		eTKD_YykSyu01.UkeNo		
										AND	eTKD_Unkobi01.UnkRen		=		eTKD_YykSyu01.UnkRen	

		LEFT JOIN	eTKD_Haisha01	ON		eTKD_YykSyu01.UkeNo			=		eTKD_Haisha01.UkeNo		
										AND	eTKD_YykSyu01.UnkRen		=		eTKD_Haisha01.UnkRen	
										AND	eTKD_YykSyu01.SyaSyuRen		=		eTKD_Haisha01.SyaSyuRen	

		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、予約書単位で“付帯料金区分：ガイド料”に該当する額を集計(eTKD_FutTum01)
		LEFT JOIN	eTKD_FutTum00	AS		eTKD_FutTum01												
									ON		eTKD_Unkobi01.UkeNo			=		eTKD_FutTum01.UkeNo		
										AND	eTKD_FutTum01.FutGuiKbn		=		5								-- 付帯料金区分＝ガイド料
										AND eTKD_YykSyu01.SyaSyuRen     =       eTKD_FutTum01.kSyaSyuRen		-- SyaSyuRenの最小値の物を取得
										AND eTKD_YykSyu01.UnkRen        =       eTKD_FutTum01.UnkRen     		-- 
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、予約書単位で“付帯料金区分：ガイド料”に該当しない額を集計(eTKD_FutTum02)
		LEFT JOIN	eTKD_FutTum00	AS		eTKD_FutTum02												
									ON		eTKD_Unkobi01.UkeNo			=		eTKD_FutTum02.UkeNo		
										AND eTKD_FutTum02.FutGuiKbn		=		0								-- 付帯料金区分＝ガイド料以外
										AND eTKD_YykSyu01.SyaSyuRen     =       eTKD_FutTum02.kSyaSyuRen		-- SyaSyuRenの最小値の物を取得
										AND eTKD_YykSyu01.UnkRen        =       eTKD_FutTum02.UnkRen    		-- 
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、予約書単位で“支払付帯種別：ガイド料”に該当する額を集計(eTKD_YFutTu01)
		LEFT JOIN	eTKD_YFutTu00	AS		eTKD_YFutTu01												
									ON		eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu01.UkeNo		
										AND	eTKD_YFutTu01.SeiFutSyu		=		5								-- 請求付帯種別＝ガイド料
		-- ＜取得＞“付帯積込品区分：付帯”に該当する付帯情報にて、予約書単位で“支払付帯種別：付帯”に該当する額を集計(eTKD_YFutTu02)
		LEFT JOIN	eTKD_YFutTu00	AS		eTKD_YFutTu02												
									ON		eTKD_Unkobi01.UkeNo			=		eTKD_YFutTu02.UkeNo		
										AND eTKD_YFutTu02.SeiFutSyu		=		0								-- 請求付帯種別＝ガイド料以外
		-- ＜検索/取得＞予約書テーブル．得意先コードＳＥＱより得意先情報を取得(eVPM_Tokisk01)
		LEFT JOIN	VPM_Tokisk		AS		eVPM_Tokisk01												
									ON		eTKD_Yyksho01.TokuiSeq		=		eVPM_Tokisk01.TokuiSeq	
										AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk01.SiyoStaYmd
																		AND		eVPM_Tokisk01.SiyoEndYmd
										AND eTKD_Yyksho01.TenantCdSeq	=		eVPM_Tokisk01.TenantCdSeq
		-- ＜検索/取得＞予約書テーブルの得意先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya01)
		LEFT JOIN	VPM_Gyosya		AS		eVPM_Gyosya01												
									ON		eVPM_Tokisk01.GyosyaCdSeq	=		eVPM_Gyosya01.GyosyaCdSeq
									AND     eVPM_Gyosya01.TenantCdSeq   =       eVPM_Tokisk01.TenantCdSeq -- 2021/05/24 ADD
										AND	eVPM_Gyosya01.SiyoKbn		=		1						
		-- ＜検索/取得＞予約書テーブル．支店コードＳＥＱより得意先情報を取得(eVPM_TokiSt01)
		LEFT JOIN	VPM_TokiSt		AS		eVPM_TokiSt01												
									ON		eTKD_Yyksho01.TokuiSeq		=		eVPM_TokiSt01.TokuiSeq	
										AND eTKD_Yyksho01.SitenCdSeq	=		eVPM_TokiSt01.SitenCdSeq
										AND eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt01.SiyoStaYmd
																		AND		eVPM_TokiSt01.SiyoEndYmd
		-- ＜検索/取得＞得意先支店マスタ．請求先コードＳＥＱより請求先情報を取得(eVPM_Tokisk02)
		LEFT JOIN	VPM_Tokisk		AS		eVPM_Tokisk02												
									ON		eVPM_TokiSt01.SeiCdSeq		=		eVPM_Tokisk02.TokuiSeq	
										AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk02.SiyoStaYmd
																		AND		eVPM_Tokisk02.SiyoEndYmd
										AND eTKD_Yyksho01.TenantCdSeq	=		eVPM_Tokisk02.TenantCdSeq
		-- ＜検索＞予約書テーブルの請求先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya02)
		LEFT JOIN	VPM_Gyosya		AS		eVPM_Gyosya02												
									ON		eVPM_Tokisk02.GyosyaCdSeq	=		eVPM_Gyosya02.GyosyaCdSeq
									AND     eVPM_Gyosya02.TenantCdSeq   =       eVPM_Tokisk02.TenantCdSeq -- 2021/05/24 ADD
										AND	eVPM_Gyosya02.SiyoKbn		=		1						
		-- ＜検索/取得＞得意先支店マスタ．請求先コードＳＥＱ/請求先支店コードＳＥＱより請求先情報を取得(eVPM_TokiSt02)
		LEFT JOIN	VPM_TokiSt		AS		eVPM_TokiSt02												
									ON		eVPM_TokiSt01.SeiCdSeq		=		eVPM_TokiSt02.TokuiSeq	
										AND	eVPM_TokiSt01.SeiSitenCdSeq	=		eVPM_TokiSt02.SitenCdSeq
										AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt02.SiyoStaYmd
																		AND		eVPM_TokiSt02.SiyoEndYmd
		-- ＜検索/取得＞(eVPM_Eigyos01)
		LEFT JOIN	VPM_Eigyos		AS		eVPM_Eigyos01												
									ON		eTKD_Yyksho01.UkeEigCdSeq	=		eVPM_Eigyos01.EigyoCdSeq
										AND	eVPM_Eigyos01.SiyoKbn		=		1						
		-- ＜検索/取得＞(eVPM_Compny01)
		LEFT JOIN	VPM_Compny		AS		eVPM_Compny01												
									ON		eVPM_Eigyos01.CompanyCdSeq	=		eVPM_Compny01.CompanyCdSeq
										AND	eVPM_Compny01.SiyoKbn		=		1						
		-- ＜検索/取得＞(eVPM_Syain01)
		LEFT JOIN	VPM_Syain		AS		eVPM_Syain01												
									ON		eTKD_Yyksho01.EigTanCdSeq	=		eVPM_Syain01.SyainCdSeq	
		-- ＜検索/取得＞(eVPM_Syain02)
		LEFT JOIN	VPM_Syain		AS		eVPM_Syain02												
									ON		eTKD_Yyksho01.InTanCdSeq	=		eVPM_Syain02.SyainCdSeq	
		-- ＜検索/取得＞(eVPM_YoyKbn01)
		LEFT JOIN	VPM_YoyKbn		AS		eVPM_YoyKbn01												
									ON		eTKD_Yyksho01.YoyaKbnSeq	=		eVPM_YoyKbn01.YoyaKbnSeq
									AND eVPM_YoyKbn01.TenantCdSeq = @LocTenantCdSeq -- 	2021/05/24 ADD
		--LEFT JOIN   VPM_YoyaKbnSort AS  eVPM_YoyaKbnSort01
		--							ON	eVPM_YoyaKbnSort01.YoyaKbnSeq	=	eVPM_YoyKbn01.YoyaKbnSeq
		--							AND	eVPM_YoyaKbnSort01.TenantCdSeq	=	@LocTenantCdSeq

		-- ＜検索＞中間日時期間に該当する車輌別日報情報を取得する(eTKD_Shabni01)
		-- サブクエリ内で抽出した結果で運行日情報の検索を行うためJOIN
		JOIN																									
			(																									
				SELECT																							
						eTKD_Haisha01.UkeNo																		
					,	eTKD_Haisha01.UnkRen 																	
				FROM																							
						TKD_Haisha				AS		eTKD_Haisha01											
						LEFT JOIN	TKD_Shabni	AS		eTKD_Shabni01											
												ON		eTKD_Haisha01.UkeNo			=		eTKD_Shabni01.UkeNo	
													AND	eTKD_Haisha01.UnkRen		=		eTKD_Shabni01.UnkRen
													AND	eTKD_Haisha01.TeiDanNo		=		eTKD_Shabni01.TeiDanNo
													AND	eTKD_Haisha01.BunkRen		=		eTKD_Shabni01.BunkRen
													AND eTKD_Shabni01.SiyoKbn		=		1 					
						-- ＜検索/取得＞車輌別日報テーブル．運行年月日の曜日情報を取得する(eVPM_Calend03)
						LEFT JOIN	VPM_Calend	AS		eVPM_Calend03											
												ON		eVPM_Calend03.CompanyCdSeq	=		1--+	@wk_UsrCompanyCdSeq	+					-- ログインユーザの会社コードＳＥＱ
													AND	eVPM_Calend03.CalenSyu		=		1																		-- 貸切用の曜日
													AND	eTKD_Shabni01.UnkYmd		=		eVPM_Calend03.CalenYmd								-- 車輌別日報テーブル．運行年月日
				GROUP BY																						
						eTKD_Haisha01.UkeNo																		
				,		eTKD_Haisha01.UnkRen																	
			)						AS		eTKD_Shabni01														
									ON		eTKD_Unkobi01.UkeNo			=		eTKD_Shabni01.UkeNo				
										AND	eTKD_Unkobi01.UnkRen		=		eTKD_Shabni01.UnkRen			


		-- ＜検索＞運行日テーブル．配車年月日の曜日情報を取得する(eVPM_Calend01)
		LEFT JOIN	VPM_Calend		AS		eVPM_Calend01																	
									ON		eVPM_Calend01.CompanyCdSeq	=		1--+	@wk_UsrCompanyCdSeq	+					-- ログインユーザの会社コードＳＥＱ
										AND	eVPM_Calend01.CalenSyu		=		1													-- 貸切用の曜日
										AND	eTKD_Unkobi01.HaiSYmd		=		eVPM_Calend01.CalenYM + eVPM_Calend01.CalenRen 		-- 運行日テーブル．配車年月日
										AND	eTKD_Unkobi01.HaiSYmd		=		eVPM_Calend01.CalenYmd								-- 運行日テーブル．配車年月日
		-- ＜検索＞運行日テーブル．到着年月日の曜日情報を取得する(eVPM_Calend02)
		LEFT JOIN	VPM_Calend		AS		eVPM_Calend02																	
									ON		eVPM_Calend02.CompanyCdSeq	=		1--+	@wk_UsrCompanyCdSeq	+				-- ログインユーザの会社コードＳＥＱ
										AND	eVPM_Calend02.CalenSyu		=		1													-- 貸切用の曜日
										AND	eTKD_Unkobi01.TouYmd		=		eVPM_Calend02.CalenYM + eVPM_Calend02.CalenRen 		-- 運行日テーブル．到着年月日
										AND	eTKD_Unkobi01.TouYmd		=		eVPM_Calend02.CalenYmd								-- 運行日テーブル．到着年月日
		-- ＜検索＞予約書テーブルの仕入先コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_Tokisk03)
		LEFT JOIN	VPM_Tokisk		AS		eVPM_Tokisk03														
									ON		eTKD_Yyksho01.SirCdSeq		=		eVPM_Tokisk03.TokuiSeq			
										AND	eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_Tokisk03.SiyoStaYmd		
																		AND		eVPM_Tokisk03.SiyoEndYmd
										AND eTKD_Yyksho01.TenantCdSeq	=		eVPM_Tokisk03.TenantCdSeq
		-- ＜検索＞予約書テーブルの仕入先コードＳＥＱ（得意先マスタ）の業者コードＳＥＱより取得(eVPM_Gyosya03)
		LEFT JOIN	VPM_Gyosya		AS		eVPM_Gyosya03														
									ON		eVPM_Tokisk03.GyosyaCdSeq	=		eVPM_Gyosya03.GyosyaCdSeq
									AND eVPM_Gyosya03.TenantCdSeq = eVPM_Tokisk03.TenantCdSeq -- 2021/05/24 ADD
										AND	eVPM_Gyosya03.SiyoKbn		=		1								
		-- ＜検索＞予約書テーブルの仕入先支店コードＳＥＱより取得(受付年月日が使用開始/終了年月日の範囲内)(eVPM_TokiSt03)
		LEFT JOIN	VPM_TokiSt		AS		eVPM_TokiSt03														
									ON		eTKD_Yyksho01.SirCdSeq		=		eVPM_TokiSt03.TokuiSeq			
										AND eTKD_Yyksho01.SirSitenCdSeq	=		eVPM_TokiSt03.SitenCdSeq		
										AND eTKD_Yyksho01.SeiTaiYmd		BETWEEN	eVPM_TokiSt03.SiyoStaYmd		
																		AND		eVPM_TokiSt03.SiyoEndYmd		
		-- ＜検索/取得＞予約書テーブルの請求区分ＳＥＱより取得(eVPM_CodeKb05)
		LEFT JOIN	VPM_CodeKb		AS		eVPM_CodeKb05														
									ON		eTKD_Yyksho01.SeiKyuKbnSeq	=		eVPM_CodeKb05.CodeKbnSeq		
		-- ＜検索＞運行日テーブルの行き先マップコードＳＥＱより取得(eVPM_Basyo01)
		LEFT JOIN	VPM_Basyo		AS		eVPM_Basyo01														
									ON		eTKD_Unkobi01.IkMapCdSeq	=		eVPM_Basyo01.BasyoMapCdSeq
									AND		eVPM_Basyo01.TenantCdSeq	=		@LocTenantCdSeq
										AND	eVPM_Basyo01.SiyoKbn		=		1								
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb06)
		LEFT JOIN	VPM_CodeKb		AS		eVPM_CodeKb06														
									ON		eVPM_Basyo01.BasyoKenCdSeq	=		eVPM_CodeKb06.CodeKbnSeq		
										AND	eVPM_CodeKb06.SiyoKbn		=		1								
		-- ＜検索/取得＞運行日テーブルの配車地コードＳＥＱより取得(eVPM_Haichi01)
		LEFT JOIN	VPM_Haichi		AS		eVPM_Haichi01														
									ON		eTKD_Unkobi01.HaiSCdSeq		=		eVPM_Haichi01.HaiSCdSeq
									AND		eVPM_Haichi01.TenantCdSeq	=		@LocTenantCdSeq
										AND	eVPM_Haichi01.SiyoKbn		=		1								
		-- ＜検索/取得＞配車地マスタの分類コードＳＥＱより取得(eVPM_CodeKb07)
		LEFT JOIN	VPM_CodeKb		AS		eVPM_CodeKb07														
									ON		eVPM_Haichi01.BunruiCdSeq	=		eVPM_CodeKb07.CodeKbnSeq		
										AND	eVPM_CodeKb07.SiyoKbn		=		1								
		-- ＜検索/取得＞運行日テーブルの到着地コードＳＥＱより取得(eVPM_Haichi02)
		LEFT JOIN	VPM_Haichi		AS		eVPM_Haichi02														
									ON		eTKD_Unkobi01.TouCdSeq		=		eVPM_Haichi02.HaiSCdSeq
									AND		eVPM_Haichi02.TenantCdSeq	=		@LocTenantCdSeq
										AND	eVPM_Haichi02.SiyoKbn		=		1								
		-- ＜検索/取得＞配車地マスタの分類コードＳＥＱより取得(eVPM_CodeKb08)
		LEFT JOIN	VPM_CodeKb		AS		eVPM_CodeKb08														
									ON		eVPM_Haichi02.BunruiCdSeq	=		eVPM_CodeKb08.CodeKbnSeq		
										AND	eVPM_CodeKb08.SiyoKbn		=		1								
		-- ＜検索＞運行日テーブルの発生地マップコードＳＥＱより取得(eVPM_Basyo02)
		LEFT JOIN	VPM_Basyo		AS		eVPM_Basyo02														
									ON		eTKD_Unkobi01.HasMapCdSeq	=		eVPM_Basyo02.BasyoMapCdSeq
									AND		eVPM_Basyo02.TenantCdSeq	=		@LocTenantCdSeq
										AND	eVPM_Basyo02.SiyoKbn		=		1								
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb09)
		LEFT JOIN	VPM_CodeKb		AS		eVPM_CodeKb09														
									ON		eVPM_Basyo02.BasyoKenCdSeq	=		eVPM_CodeKb09.CodeKbnSeq		
										AND	eVPM_CodeKb09.SiyoKbn		=		1								
		-- ＜検索＞運行日テーブルのエリアマップコードＳＥＱより取得(eVPM_Basyo03)
		LEFT JOIN	VPM_Basyo		AS		eVPM_Basyo03														
									ON		eTKD_Unkobi01.AreaMapSeq	=		eVPM_Basyo03.BasyoMapCdSeq
									AND		eVPM_Basyo03.TenantCdSeq	=		@LocTenantCdSeq
										AND	eVPM_Basyo03.SiyoKbn		=		1								
		-- ＜検索＞場所マスタの県コードＳＥＱより取得(eVPM_CodeKb10)
		LEFT JOIN	VPM_CodeKb		AS		eVPM_CodeKb10														
									ON		eVPM_Basyo03.BasyoKenCdSeq	=		eVPM_CodeKb10.CodeKbnSeq		
		-- ＜検索＞運行日テーブルの乗客区分コードＳＥＱより取得(eVPM_JyoKya01)
		LEFT JOIN	VPM_JyoKya		AS		eVPM_JyoKya01														
									ON		eTKD_Unkobi01.JyoKyakuCdSeq	=		eVPM_JyoKya01.JyoKyakuCdSeq		
		-- ＜検索＞乗客区分マスタの団体区分コードＳＥＱより取得(eVPM_CodeKb11)
		LEFT JOIN	VPM_CodeKb		AS		eVPM_CodeKb11														
									ON		eVPM_JyoKya01.DantaiCdSeq	=		eVPM_CodeKb11.CodeKbnSeq		
										AND	eVPM_CodeKb11.SiyoKbn		=		1								
		---- ＜検索＞予約書テーブルの貸切ツアーコードＳＥＱより取得(eVPM_Katour01)
		--LEFT JOIN	VPM_Katour		AS		eVPM_Katour01														
		--							ON		eTKD_Yyksho01.KasTourCdSeq	=		eVPM_Katour01.KasTourCdSeq		
		LEFT JOIN	eTKD_Biko01																					
									ON		eTKD_Biko01.UkeNo			=		eTKD_Yyksho01.UkeNo				
									AND		eTKD_Biko01.BikoTblSeq		=		eTKD_Yyksho01.BikoTblSeq		
									AND		eTKD_Biko01.BikoRen			=										
											(SELECT MIN(BikoRen) FROM eTKD_Biko01								
									WHERE	eTKD_Biko01.UkeNo			=		eTKD_Yyksho01.UkeNo				
									AND		eTKD_Biko01.BikoTblSeq		=		eTKD_Yyksho01.BikoTblSeq)		
		LEFT JOIN	TKD_UnkobiExp  AS  eTKD_UnkobiExp01															
                                   ON	eTKD_Unkobi01.UkeNo		=		eTKD_UnkobiExp01.UkeNo					
		                            AND   	eTKD_Unkobi01.UnkRen	=		eTKD_UnkobiExp01.UnkRen				
									AND	    eTKD_Unkobi01.SiyoKbn   =		1			     					
		LEFT JOIN	TKD_YykReport AS eTKD_YykReport																
									ON		eTKD_YykSyu01.UkeNo			=		eTKD_YykReport.UkeNo		        
									AND		eTKD_Unkobi01.UnkRen		=		eTKD_YykReport.UnkRen			
									AND	 eTKD_YykSyu01.SyaSyuRen		=		eTKD_YykReport.SyaSyuRen		
	    LEFT JOIN								
	    (	SELECT								
				UkeNo,							
		        UnkRen,						
		        SyaSyuRen,						
				KSEigSeq						
			FROM								
		        TKD_Kariei						
			WHERE								
		        TKD_Kariei.SiyoKbn = 1		) AS eTKD_Kariei ON			
									eTKD_YykSyu01.UkeNo	=	 eTKD_Kariei.UkeNo AND				
									eTKD_YykSyu01.UnkRen =	 eTKD_Kariei.UnkRen					
									AND eTKD_YykSyu01.SyaSyuRen  = eTKD_Kariei.SyaSyuRen		
		LEFT JOIN	VPM_Eigyos	AS	eVPM_Eigyos00 																
							 ON eTKD_Kariei.KSEigSeq = eVPM_Eigyos00.EigyoCdSeq									
							 AND eVPM_Eigyos00.SiyoKbn = 1	

		-- ＜検索＞予約上限下限運賃料金計算テーブル (eTKD_BookingMaxMinFareFeeCalc01)
		LEFT JOIN	TKD_BookingMaxMinFareFeeCalc		AS	eTKD_BookingMaxMinFareFeeCalc01	
														ON	eTKD_BookingMaxMinFareFeeCalc01.UkeNo	=	eTKD_Unkobi01.UkeNo
														AND	eTKD_BookingMaxMinFareFeeCalc01.UnkRen	=	eTKD_Unkobi01.UnkRen
														AND eTKD_BookingMaxMinFareFeeCalc01.SyaSyuRen	=	eTKD_YykSyu01.SyaSyuRen
WHERE eTKD_Yyksho01.YoyaSyu IN (1, 3)
		    AND eTKD_Yyksho01.TenantCdSeq = @LocTenantCdSeq 
            AND (@LocStartDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd >= @LocStartDispatchDate) -- 配車日
            AND (@LocEndDispatchDate IS NULL OR eTKD_Unkobi01.HaiSYmd <= @LocEndDispatchDate) -- 配車日
            AND (@LocStartArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd >= @LocStartArrivalDate) -- 到着日
            AND (@LocEndArrivalDate IS NULL OR eTKD_Unkobi01.TouYmd <= @LocEndArrivalDate) -- 到着日
            AND (@LocStartReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd >= @LocStartReservationDate) -- 予約日
            AND (@LocEndReservationDate IS NULL OR eTKD_Yyksho01.UkeYmd <= @LocEndReservationDate) -- 予約日
            AND (@LocStartReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo >= @LocStartReceiptNumber) -- 予約番号
            AND (@LocEndReceiptNumber IS NULL OR eTKD_Yyksho01.UkeNo <= @LocEndReceiptNumber) -- 予約番号
            AND (@LocStartReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn >= @LocStartReservationClassification) -- 予約区分
            AND (@LocEndReservationClassification IS NULL OR eVPM_YoyKbn01.YoyaKbn <= @LocEndReservationClassification) -- 予約区分
            AND (@LocStartServicePerson IS NULL OR eVPM_Syain01.SyainCd >= @LocStartServicePerson) -- 営業担当者
            AND (@LocEndServicePerson IS NULL OR eVPM_Syain01.SyainCd <= @LocEndServicePerson) -- 営業担当者
            AND (@LocStartRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd >= @LocStartRegistrationOffice) -- 営業所
            AND (@LocEndRegistrationOffice IS NULL OR eVPM_Eigyos01.EigyoCd <= @LocEndRegistrationOffice) -- 営業所
            AND (@LocStartInputPerson IS NULL OR eVPM_Syain02.SyainCd >= @LocStartInputPerson) -- 入力担当者
            AND (@LocEndInputPerson IS NULL OR eVPM_Syain02.SyainCd <= @LocEndInputPerson) -- 入力担当者
            AND (@LocStartCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') >= @LocStartCustomer) -- 得意先コード
            AND (@LocEndCustomer IS NULL OR FORMAT(eVPM_Gyosya01.GyosyaCd, '000') + FORMAT(eVPM_Tokisk01.TokuiCd, '0000') + FORMAT(eVPM_TokiSt01.SitenCd, '0000') <= @LocEndCustomer) -- 得意先コード
            AND (@LocStartSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') >= @LocStartSupplier) -- 仕入先コード
            AND (@LocEndSupplier IS NULL OR FORMAT(eVPM_Gyosya03.GyosyaCd, '000') + FORMAT(eVPM_Tokisk03.TokuiCd, '0000') + FORMAT(eVPM_TokiSt03.SitenCd, '0000') <= @LocEndSupplier) -- 仕入先コード
            AND (@LocStartGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn >= @LocStartGroupClassification) -- 団体区分
            AND (@LocEndGroupClassification IS NULL OR eVPM_CodeKb11.CodeKbn <= @LocEndGroupClassification) -- 団体区分
            AND (@LocStartCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd >= @LocStartCustomerTypeClassification) -- 乗客コード
            AND (@LocEndCustomerTypeClassification IS NULL OR eVPM_JyoKya01.JyoKyakuCd <= @LocEndCustomerTypeClassification) -- 乗客コード
            AND (@LocStartDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd >= @LocStartDestination) -- 行先
            AND (@LocEndDestination IS NULL OR eVPM_CodeKb06.CodeKbn + eVPM_Basyo01.BasyoMapCd <= @LocEndDestination) -- 行先
            AND (@LocStartDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd >= @LocStartDispatchPlace) -- 配車地
            AND (@LocEndDispatchPlace IS NULL OR eVPM_CodeKb07.CodeKbn + eVPM_Haichi01.HaiSCd <= @LocEndDispatchPlace) -- 配車地
            AND (@LocStartOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd >= @LocStartOccurrencePlace) -- 発生地
            AND (@LocEndOccurrencePlace IS NULL OR eVPM_CodeKb09.CodeKbn + eVPM_Basyo02.BasyoMapCd <= @LocEndOccurrencePlace) -- 発生地
            AND (@LocStartArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd >= @LocStartArea) -- エリア
            AND (@LocEndArea IS NULL OR eVPM_CodeKb10.CodeKbn + eVPM_Basyo03.BasyoMapCd <= @LocEndArea) -- エリア
            AND (@LocStartReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd >= @LocStartReceiptCondition) -- 受付条件
            AND (@LocEndReceiptCondition IS NULL OR eTKD_Unkobi01.UkeJyKbnCd <= @LocEndReceiptCondition) -- 受付条件
            AND (@LocDantaNm IS NULL OR eTKD_Unkobi01.DantaNm LIKE CONCAT('%', @LocDantaNm, '%')) -- 団体名 P.M.Nhat add condition 2020/09/04
			AND (@LocMaxMinSetting IS NULL OR @LocMaxMinSetting <> 0 OR ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMaxAmount, 0) <> 0 OR ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMinAmount, 0) <> 0)
			AND (@LocMaxMinSetting IS NULL OR @LocMaxMinSetting <> 1 OR (ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMaxAmount, 0) = 0 AND ISNULL(eTKD_BookingMaxMinFareFeeCalc01.UnitPriceMinAmount, 0) = 0))
			AND (@LocReservationStatus IS NULL OR @LocReservationStatus <> 0 OR eTKD_Yyksho01.YoyaSyu = 1)
			AND (@LocReservationStatus IS NULL OR @LocReservationStatus <> 1 OR eTKD_Yyksho01.YoyaSyu = 3)
        ORDER BY UkeNo, UnkRen
		OPTION (RECOMPILE)
	END
END
