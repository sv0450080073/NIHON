USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[Pro_GetNumberOfBusUnAsignedList_R]    Script Date: 6/11/2021 10:24:41 AM ******/
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
CREATE OR ALTER PROCEDURE [dbo].[Pro_GetNumberOfBusUnAsignedList_R]
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
		@SyaSyuCdFrom int,
		@SyaSyuCdTo int,
		@YouKataKbn  int ,
		@BasyoMapCdFrom varchar(20),
		@BasyoMapCdTo varchar(20),
		@ModeCondition varchar(20), 
		@NumberDayLoop int ,
		@TenantCdSeq  int
    )
AS
begin
	--   Parse date string to datetime 
	Declare @DateHS DATE = convert ( datetime, @DateHaiSha , 111 )
	DECLARE @DateHSStartTpm DATE = @DateHS, @DateHSEndTpm DATE = DATEADD(day,@NumberDayLoop,@DateHS);
	DECLARE  @StrStartDate varchar(8) =convert(varchar,@DateHSStartTpm, 112);
	DECLARE  @StrEndDate varchar(8) =convert(varchar,@DateHSEndTpm, 112);
	CREATE TABLE #Tb_NumberVehicle_UnAsign(STT INT , KataKbn INT, NumberOfVehicle INT )
	CREATE TABLE #Tb_Tmp( KataKbn INT, SyuKoYmd char(8),KikYmd char(8)   )
IF(@ModeCondition=1) --UNASIGN BUS
	BEGIN 
	SELECT YYSYU.KataKbn
			, HAISHA.SyuKoYmd
			, HAISHA.KikYmd 
			, 0 AS NumberOfVehicle 
			,  YYKSHO.UkeEigCdSeq 
			, Eigyos.CompanyCdSeq 
			, YYSYU.SyaSyuCdSeq AS YYSYU_SyaSyuCdSeq
	FROM TKD_Haisha AS HAISHA																																
		LEFT JOIN TKD_Yyksho AS YYKSHO																																
		ON YYKSHO.UkeNo = HAISHA.UkeNo																																
		AND YYKSHO.SiyoKbn = 1	
		AND YYKSHO.TenantCdSeq = @TenantCdSeq
		LEFT JOIN TKD_YykSyu AS YYSYU																																
		ON YYSYU.UkeNo = HAISHA.UkeNo																																
		AND YYSYU.UnkRen = HAISHA.UnkRen																															
		AND YYSYU.SyaSyuRen = HAISHA.SyaSyuRen																															
		AND YYSYU.SiyoKbn = 1	
		LEFT JOIN TKD_Unkobi AS UNKOBI	  
		ON UNKOBI.UkeNo = HAISHA.UkeNo
		AND UNKOBI.UnkRen = HAISHA.UnkRen
		LEFT JOIN VPM_Eigyos AS Eigyos																																
		--ON Eigyos.EigyoCdSeq = HAISHA.SyuEigCdSeq
		ON Eigyos.EigyoCdSeq = YYKSHO.UkeEigCdSeq
		LEFT JOIN VPM_Compny AS COMPANY   
		ON COMPANY.CompanyCdSeq = Eigyos.CompanyCdSeq
		AND COMPANY.TenantCdSeq = @TenantCdSeq
		LEFT JOIN VPM_Syain AS Syain01																																
		ON Syain01.SyainCdSeq=YYKSHO.EigTanCdSeq																																
		LEFT JOIN VPM_Syain AS Syain02																																
		ON Syain02.SyainCdSeq=YYKSHO.InTanCdSeq	
		LEFT JOIN VPM_Basyo AS BASHO
		ON BASHO.BasyoMapCdSeq = UNKOBI.IkMapCdSeq
		AND BASHO.TenantCdSeq = @TenantCdSeq
		--LEFT JOIN VPM_SyaSyu AS Syasyu																																
		--ON Syasyu.SyaSyuCdSeq=YYSYU.SyaSyuCdSeq																																
		WHERE HAISHA.SiyoKbn = 1																																
		AND HAISHA.SyuKoYmd <= @StrEndDate       																															
		AND HAISHA.KikYmd >=   @StrStartDate      																																
		AND HAISHA.HaiSSryCdSeq = 0            																															
		AND HAISHA.YouTblSeq = 0               																															
		AND YYKSHO.YoyaSyu = 1                
		AND COMPANY.CompanyCdSeq =CASE WHEN @CompanyCdSeq =0 THEN  COMPANY.CompanyCdSeq ELSE  @CompanyCdSeq   END 
		AND ((@BookingTypeList != '' AND YYKSHO.YoyaKbnSeq IN (select * from dbo.FN_SplitString(@BookingTypeList, '-'))) OR @BookingTypeList = '')
		AND Eigyos.EigyoCd >=CASE WHEN @EigyoCdFrom =0 THEN  Eigyos.EigyoCd ELSE  @EigyoCdFrom   END 
		AND Eigyos.EigyoCd <=CASE WHEN @EigyoCdTo =0 THEN  Eigyos.EigyoCd ELSE  @EigyoCdTo   END 
		AND Syain01.SyainCd >=CASE WHEN @SyainCdFrom ='0' THEN  Syain01.SyainCd ELSE  @SyainCdFrom   END 
		AND Syain01.SyainCd <=CASE WHEN @SyainCdTo='0' THEN  Syain01.SyainCd ELSE  @SyainCdTo   END 
		AND Syain02.SyainCd >=CASE WHEN @PersonInputFrom ='0' THEN  Syain02.SyainCd ELSE  @PersonInputFrom   END 
		AND Syain02.SyainCd <=CASE WHEN @PersonInputTo='0' THEN  Syain02.SyainCd ELSE  @PersonInputTo   END
		AND ((@BasyoMapCdFrom !='0' AND BASHO.BasyoMapCd<=@BasyoMapCdFrom) OR @BasyoMapCdFrom='0')
		AND ((@BasyoMapCdTo !='0' AND BASHO.BasyoMapCd>=@BasyoMapCdTo) OR @BasyoMapCdTo='0')
	END 	
ELSE IF(@ModeCondition =2) --HIRING
	BEGIN 
	SELECT HAISHA.YouKataKbn
			, HAISHA.SyuKoYmd
			, HAISHA.KikYmd  
			, YYKSHO.UkeEigCdSeq 
			, Eigyos.CompanyCdSeq	
			, HAISHA.UkeNo 
			, HAISHA.UnkRen 
			, HAISHA.SyaSyuRen
	FROM TKD_Haisha AS HAISHA																																
		LEFT JOIN TKD_Yyksho AS YYKSHO																																
		ON YYKSHO.UkeNo = HAISHA.UkeNo																																
		AND YYKSHO.SiyoKbn = 1	
		AND YYKSHO.TenantCdSeq =@TenantCdSeq
		LEFT JOIN TKD_YykSyu AS YYSYU												
		ON YYSYU.UkeNo = HAISHA.UkeNo												
		AND YYSYU.UnkRen = HAISHA.UnkRen											
		AND YYSYU.SyaSyuRen = HAISHA.SyaSyuRen											
		AND YYSYU.SiyoKbn = 1											
		LEFT JOIN TKD_Unkobi AS UNKOBI           															
　　　　	ON UNKOBI.UkeNo = HAISHA.UkeNo      														
		AND UNKOBI.UnkRen = HAISHA.UnkRen  		
		LEFT JOIN VPM_Eigyos AS Eigyos																																
		ON Eigyos.EigyoCdSeq = YYKSHO.UkeEigCdSeq
		LEFT JOIN VPM_Compny AS COMPANY														
　　　　	ON COMPANY.CompanyCdSeq = Eigyos.CompanyCdSeq														
		AND COMPANY.TenantCdSeq =@TenantCdSeq													
		LEFT JOIN VPM_Syain AS Syain01																																
		ON Syain01.SyainCdSeq=YYKSHO.EigTanCdSeq																																
		LEFT JOIN VPM_Syain AS Syain02																																
		ON Syain02.SyainCdSeq=YYKSHO.InTanCdSeq																																
		LEFT JOIN VPM_Basyo AS BASHO												
 		ON BASHO.BasyoMapCdSeq = UNKOBI.IkMapCdSeq	
		AND BASHO.TenantCdSeq = @TenantCdSeq
		WHERE HAISHA.SiyoKbn = 1																																
		AND HAISHA.SyuKoYmd <= @StrEndDate       																																
		AND HAISHA.KikYmd >=   @StrStartDate       																																																												
		AND HAISHA.HaiSSryCdSeq = 0            																															
		AND HAISHA.YouTblSeq  <>0              																															
		AND YYKSHO.YoyaSyu = 1                	
		AND COMPANY.CompanyCdSeq = CASE WHEN @CompanyCdSeq =0 THEN  COMPANY.CompanyCdSeq ELSE  @CompanyCdSeq   END  
		AND HAISHA.YouKataKbn =CASE WHEN @YouKataKbn =0 THEN  HAISHA.YouKataKbn ELSE  @YouKataKbn   END  																													
		AND ((@BookingTypeList != '' AND YYKSHO.YoyaKbnSeq IN (select * from dbo.FN_SplitString(@BookingTypeList, '-'))) OR @BookingTypeList = '')
		AND Eigyos.EigyoCd >=CASE WHEN @EigyoCdFrom =0 THEN  Eigyos.EigyoCd ELSE  @EigyoCdFrom   END 
		AND Eigyos.EigyoCd <=CASE WHEN @EigyoCdTo =0 THEN  Eigyos.EigyoCd ELSE  @EigyoCdTo   END 
		AND Syain01.SyainCd >=CASE WHEN @SyainCdFrom ='0' THEN  Syain01.SyainCd ELSE  @SyainCdFrom   END 
		AND Syain01.SyainCd <=CASE WHEN @SyainCdTo='0' THEN  Syain01.SyainCd ELSE  @SyainCdTo   END 
		AND Syain02.SyainCd >=CASE WHEN @PersonInputFrom ='0' THEN  Syain02.SyainCd ELSE  @PersonInputFrom   END 
		AND Syain02.SyainCd <=CASE WHEN @PersonInputTo='0' THEN  Syain02.SyainCd ELSE  @PersonInputTo   END
		AND ((@BasyoMapCdFrom !='0' AND BASHO.BasyoMapCd<=@BasyoMapCdFrom) OR @BasyoMapCdFrom='0')
		AND ((@BasyoMapCdTo !='0' AND BASHO.BasyoMapCd>=@BasyoMapCdTo) OR @BasyoMapCdTo='0')
	END		
END
