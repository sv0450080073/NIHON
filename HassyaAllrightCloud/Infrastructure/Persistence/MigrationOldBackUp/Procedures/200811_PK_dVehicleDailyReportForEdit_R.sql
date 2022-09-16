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
-- System-Name	:   HassyaAllrightCloud
-- Module-Name	:   HassyaAllrightCloud
-- SP-ID		:   PK_dVehicleDailyReportsForEdit_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get vehicle daily report list for edit
-- Date			:   2020/08/11
-- Author		:   P.M.NHAT
-- Description	:   Get vehicle daily report list for edit with conditions
-- =============================================
CREATE PROCEDURE [dbo].[PK_dVehicleDailyReportForEdit_R]
(
	-- Parameter
		@UkeNo			nchar(15)	
	,	@UnkRen			smallint	
	,	@TeiDanNo		smallint			
	,	@BunkRen		smallint	
	, 	@TenantCdSeq 	int
)
AS
	BEGIN
		SELECT TKD_Shabni.* 
		,	ISNULL(eTPM_CodeKb01.CodeKbn	,     0)	 AS 	Nenryo1_NenryoCd	
		,	ISNULL(eTPM_CodeKb01.CodeKbnNm	, ' ')		 AS 	Nenryo1_NenryoName	
		,	ISNULL(eTPM_CodeKb01.RyakuNm	, ' ')		 AS 	Nenryo1_NenryoRyak	
		,	ISNULL(eTPM_CodeKb02.CodeKbn	,     0)	 AS 	Nenryo2_NenryoCd	
		,	ISNULL(eTPM_CodeKb02.CodeKbnNm	, ' ')	 AS 	Nenryo2_NenryoName	
		,	ISNULL(eTPM_CodeKb02.RyakuNm	, ' ')	 AS 	Nenryo2_NenryoRyak	
		,	ISNULL(eTPM_CodeKb03.CodeKbn	,     0)	 AS 	Nenryo3_NenryoCd	
		,	ISNULL(eTPM_CodeKb03.CodeKbnNm	, ' ')	 AS 	Nenryo3_NenryoName	
		,	ISNULL(eTPM_CodeKb03.RyakuNm	, ' ')	 AS 	Nenryo3_NenryoRyak	
		,	ISNULL(eTPM_CodeKb04.CodeKbnNm	, ' ')	 AS 	ZeiKbn_CodeKbnNm	
		,	ISNULL(eTPM_CodeKb04.RyakuNm	, ' ')	 AS 	ZeiKbn_RyakuNm		
		,	ISNULL(eTPM_Syain05.SyainCd		, ' ')	 AS 	UpdSyainCd_SyainCd	
		,	ISNULL(eTPM_Syain05.SyainNm		, ' ')	 AS 	UpdSyainCd_SyainNm	
		,	ISNULL(JT_Haisha.HaiSYmd		, ' ')	 AS 	HaiSYmd				
		,	ISNULL(JT_Haisha.TouYmd			, ' ')	 AS 	TouYmd				
		,	ISNULL(JM_SyaRyo.SyaRyoCd		, ' ')	 AS 	SyaRyoCd			
		,	ISNULL(JM_SyaRyo.SyaRyoNm		, ' ')	 AS 	SyaRyoNm			
		,	ISNULL(JT_Haisha.NippoKbn			, ' ')	 AS 	NippoKbn
				
		FROM TKD_Shabni LEFT JOIN 	VPM_CodeKb	 AS 	eTPM_CodeKb01	
		ON	 TKD_Shabni.NenryoCd1Seq					=	eTPM_CodeKb01.CodeKbnSeq	
		AND  eTPM_CodeKb01.TenantCdSeq = @TenantCdSeq

		LEFT JOIN 	VPM_CodeKb	 AS 	eTPM_CodeKb02	
		ON	 TKD_Shabni.NenryoCd2Seq					=	eTPM_CodeKb02.CodeKbnSeq	
		AND  eTPM_CodeKb02.TenantCdSeq = @TenantCdSeq

		LEFT JOIN 	VPM_CodeKb	AS	eTPM_CodeKb03	
		ON	 TKD_Shabni.NenryoCd3Seq					=	eTPM_CodeKb03.CodeKbnSeq	
		AND  eTPM_CodeKb03.TenantCdSeq = @TenantCdSeq

		LEFT JOIN 	VPM_CodeKb	AS	eTPM_CodeKb04	
		ON	 eTPM_CodeKb04.CodeKbn	=	Format(TKD_Shabni.ZeiKbn, '00')
		AND	 eTPM_CodeKb04.CodeSyu	=	'ZEIKBN'			
		AND  eTPM_CodeKb04.TenantCdSeq = @TenantCdSeq		
			
		LEFT JOIN 	VPM_Syain	AS	eTPM_Syain05	
		ON	 TKD_Shabni.UpdSyainCd					=	eTPM_Syain05.SyainCdSeq		

		LEFT JOIN 	TKD_Haisha	AS	JT_Haisha	
		ON	 TKD_Shabni.UkeNo						=	JT_Haisha.UkeNo				
		AND  TKD_Shabni.UnkRen						=	JT_Haisha.UnkRen			
		AND  TKD_Shabni.TeiDanNo					=	JT_Haisha.TeiDanNo			
		AND  TKD_Shabni.BunkRen						=	JT_Haisha.BunkRen		

		LEFT JOIN 	VPM_SyaRyo	AS	JM_SyaRyo	
		ON	 JT_Haisha.HaiSSryCdSeq					=	JM_SyaRyo.SyaRyoCdSeq	

		Where  TKD_Shabni.UkeNo = @UkeNo
		AND  TKD_Shabni.UnkRen = @UnkRen
		AND  TKD_Shabni.TeiDanNo = @TeiDanNo
		AND  TKD_Shabni.BunkRen = @BunkRen																																		
	END
GO
