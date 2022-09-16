USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetBusTypeList_R]    Script Date: 5/26/2021 3:09:52 PM ******/
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
ALTER       PROCEDURE [dbo].[Pro_GetBusTypeList_R]
    (
	    @CompanyCdSeq int,
		@EigyoCdFrom int ,
		@EigyoCdTo int ,
		@DateHenSyaFrom  char(8),
		@DateHenSyaTo char(8),
		@SyaSyuCdFrom int,
		@SyaSyuCdTo int ,
		@KataKbn int,
		@TenantCdSeq  int,
		@TenantCdSeqByCodeSyu int
    )
AS
begin
	SELECT SYU.SyaSyuCdSeq																															
       ,SYU.SyaSyuNm　
	   ,SYU.KataKbn
	   , Ei.EigyoCdSeq
	   , Ei.EigyoCd
	   , Ei.EigyoNm
	   , Ei.RyakuNm AS Ei_RyakuNm
	   , COM.CompanyCdSeq
	   , COM.CompanyNm 
	   , COM.RyakuNm AS COM_RyakuNm
	FROM VPM_SyaRyo AS RYO
		LEFT JOIN VPM_SyaSyu AS SYU
		ON SYU.SyaSyuCdSeq = RYO.SyaSyuCdSeq
		AND SYU.SiyoKbn = 1
		AND SYU.TenantCdSeq = @TenantCdSeq
		LEFT JOIN VPM_CodeKb   AS  CodeKb01   																															
        On  SYU.KataKbn=CodeKb01.CodeKbn 																															
        AND  CodeKb01.CodeSyu   ='KATAKBN'　																															
        AND  CodeKb01.SiyoKbn=1																															
        AND CodeKb01.TenantCdSeq=@TenantCdSeqByCodeSyu		
		LEFT JOIN VPM_HenSya AS HEN
		ON HEN.SyaRyoCdSeq = RYO.SyaRyoCdSeq
		AND HEN.StaYmd <=@DateHenSyaFrom
		AND HEN.EndYmd >= @DateHenSyaTo
		LEFT JOIN VPM_Eigyos AS EI
		ON EI.EigyoCdSeq = HEN.EigyoCdSeq
		AND EI.SiyoKbn = 1
		LEFT JOIN VPM_Compny AS COM
		ON COM.CompanyCdSeq = EI.CompanyCdSeq
		AND COM.SiyoKbn = 1
		AND COM.TenantCdSeq = @TenantCdSeq
	WHERE SYU.SiyoKbn=1																														
			AND SYU.SyaSyuCd >= CASE WHEN @SyaSyuCdFrom = 0 THEN SYU.SyaSyuCd ELSE @SyaSyuCdFrom END 
			AND SYU.SyaSyuCd <= CASE WHEN @SyaSyuCdTo = 0 THEN SYU.SyaSyuCd ELSE @SyaSyuCdTo END 																														
			AND SYU.TenantCdSeq=@TenantCdSeq																															
			AND SYU.KataKbn=CASE WHEN @KataKbn=0 THEN  SYU.KataKbn ELSE  @KataKbn   END 	 																														
			AND COM.CompanyCdSeq =CASE WHEN @CompanyCdSeq = 0 THEN COM.CompanyCdSeq ELSE @CompanyCdSeq END 	
			AND EI.EigyoCd >=CASE WHEN @EigyoCdFrom =0 THEN  EI.EigyoCd ELSE  @EigyoCdFrom   END 
			AND EI.EigyoCd <=CASE WHEN @EigyoCdTo =0 THEN  EI.EigyoCd ELSE  @EigyoCdTo   END 
	ORDER by SYU.SyaSyuCd
end
GO


