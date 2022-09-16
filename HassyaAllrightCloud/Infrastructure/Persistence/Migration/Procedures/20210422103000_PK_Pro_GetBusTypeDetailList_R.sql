USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetBusTypeDetailList_R]    Script Date: 4/22/2021 10:34:36 AM ******/
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
--  Update      :
--  Comment     :
----------------------------------------------------
CREATE OR ALTER    PROCEDURE [dbo].[Pro_GetBusTypeDetailList_R]
    (
		@DateHaiSha  char(8),
		@BookingTypeList varchar(max),
		@CompanyCdSeq int,
		@EigyoCdFrom int ,
		@EigyoCdTo int ,
		@SyainCdFrom varchar(20),
		@SyainCdTo varchar(20),
		@PersonInputFrom varchar(20),
		@PersonInputTo varchar(20),
		@KataKbn int ,
		@SyaSyuCdFrom int,
		@SyaSyuCdTo int,
		@BasyoMapCdFrom varchar(20),
		@BasyoMapCdTo varchar(20),
		@NumberDayLoop int ,
		@TenantCdSeq  int
    )
AS
begin
	--   Parse date string to datetime 
	Declare @DateHS DATE = convert ( datetime, @DateHaiSha , 111 )
	DECLARE @DateTpm DATE = @DateHS;
	DECLARE @DateHSStartTpm DATE = @DateHS, @DateHSEndTpm DATE = DATEADD(day,@NumberDayLoop,@DateHS);
	DECLARE  @STT INT = 1;
	DECLARE  @StrStartDate varchar(8) =convert(varchar,@DateHSStartTpm, 112);
	DECLARE  @StrEndDate varchar(8) =convert(varchar,@DateHSEndTpm, 112);
	SELECT SYASYU.SyaSyuCdSeq
			,SYASYU.SyaSyuNm
			,HAISHA.SyuKoYmd 
			,HAISHA.KikYmd 
			,SYASYU.KataKbn 
			,Compny.CompanyCdSeq
			,Eigyos.EigyoCdSeq
			,Eigyos.EigyoCd
			,HAISHA.DrvJin
			,HAISHA.GuiSu 
			, HAISHA.KSKbn
			, HAISHA.HaiSKbn
			, HAISHA.HaiSSryCdSeq
			, HAISHA.YouTblSeq
			, UNKOBI.UnkoJKbn as UN_UnkoJKbn
			, UNKOBI.SyukoYmd as UN_SyukoYmd
			, UNKOBI.KikYmd as UN_KikYmd
			, UNKOBI.HaiSYmd as UN_HaiSYmd
			, UNKOBI.TouYmd as UN_TouYmd
			,0 as NumberOfVehicle 
	FROM TKD_Haisha AS HAISHA																															
		LEFT JOIN TKD_Yyksho AS YYKSHO																															
		ON YYKSHO.UkeNo = HAISHA.UkeNo																															
		AND YYKSHO.SiyoKbn = 1	
		AND YYKSHO.TenantCdSeq = @TenantCdSeq
		LEFT JOIN TKD_Unkobi AS UNKOBI
		ON UNKOBI.UkeNo = HAISHA.UkeNo
		AND UNKOBI.UnkRen = HAISHA.UnkRen
		LEFT JOIN VPM_SyaRyo AS SYARYO																															
		ON SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq																															
		LEFT JOIN VPM_SyaSyu AS SYASYU																															
		ON SYASYU.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq																															
		AND SYASYU.TenantCdSeq = @TenantCdSeq																														
		LEFT JOIN VPM_Eigyos AS Eigyos																															
      -- ON Eigyos.EigyoCdSeq=YYKSHO.UkeEigCdSeq
		ON Eigyos.EigyoCdSeq　=　HAISHA.SyuEigCdSeq
		LEFT JOIN VPM_Syain AS Syain01																															
		ON YYKSHO.EigTanCdSeq=Syain01.SyainCdSeq																															
		LEFT JOIN VPM_Syain AS Syain02																															
		ON YYKSHO.InTanCdSeq=Syain02.SyainCdSeq																															
		LEFT JOIN TKD_YykSyu AS YYKSHU																															
		ON YYKSHU.UkeNo=HAISHA.UkeNo																															
		AND YYKSHU.UnkRen=HAISHA.UnkRen																														
		AND YYKSHU.SiyoKbn=1	
		AND YYKSHU.SyaSyuRen= HAISHA.SyaSyuRen  
		LEFT JOIN VPM_Compny AS Compny																															
		ON Compny.CompanyCdSeq=Eigyos.CompanyCdSeq
		AND Compny.TenantCdSeq = @TenantCdSeq
		AND Compny.SiyoKbn=1  
		LEFT JOIN VPM_Basyo AS BASHO
		ON BASHO.BasyoMapCdSeq = UNKOBI.IkMapCdSeq
	WHERE HAISHA.SiyoKbn = 1																															
			AND HAISHA.HaiSSryCdSeq <> 0            																															
			AND YYKSHO.YoyaSyu = 1                 																																																														
			AND ((@BookingTypeList != '' AND YYKSHO.YoyaKbnSeq IN (select * from dbo.FN_SplitString(@BookingTypeList, '-'))) OR @BookingTypeList = '')
			AND Compny.CompanyCdSeq=CASE WHEN @CompanyCdSeq =0 THEN  Compny.CompanyCdSeq ELSE  @CompanyCdSeq   END   																															
			AND Eigyos.EigyoCd >=CASE WHEN @EigyoCdFrom =0 THEN  Eigyos.EigyoCd ELSE  @EigyoCdFrom   END 
			AND Eigyos.EigyoCd <=CASE WHEN @EigyoCdTo =0 THEN  Eigyos.EigyoCd ELSE  @EigyoCdTo   END 
			AND Syain01.SyainCd >=CASE WHEN @SyainCdFrom ='0' THEN  Syain01.SyainCd ELSE  @SyainCdFrom   END 
			AND Syain01.SyainCd <=CASE WHEN @SyainCdTo='0' THEN  Syain01.SyainCd ELSE  @SyainCdTo   END 
			AND Syain02.SyainCd >=CASE WHEN @PersonInputFrom ='0' THEN  Syain02.SyainCd ELSE  @PersonInputFrom   END 
			AND Syain02.SyainCd <=CASE WHEN @PersonInputTo='0' THEN  Syain02.SyainCd ELSE  @PersonInputTo   END																														
			AND SYASYU.KataKbn=CASE WHEN @KataKbn=0 THEN  SYASYU.KataKbn ELSE  @KataKbn   END
			AND ((@BasyoMapCdFrom !='0' AND BASHO.BasyoMapCd<=@BasyoMapCdFrom) OR @BasyoMapCdFrom='0')
			AND ((@BasyoMapCdTo !='0' AND BASHO.BasyoMapCd>=@BasyoMapCdTo) OR @BasyoMapCdTo='0')
			AND HAISHA.SyuKoYmd <= @StrEndDate   
			AND HAISHA.KikYmd >=   @StrStartDate
	ORDER BY SYASYU.KataKbn
	END

GO


