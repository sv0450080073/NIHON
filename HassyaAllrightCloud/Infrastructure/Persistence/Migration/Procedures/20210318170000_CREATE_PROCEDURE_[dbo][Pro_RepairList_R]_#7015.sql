USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_RepairList_R]    Script Date: 3/18/2021 4:56:31 PM ******/
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
CREATE PROCEDURE [dbo].[Pro_RepairList_R]
    (
	    @CompanyList nvarchar(max),
		@DateRepairFrom  char(8),
		@DateRepairTo char(8),
		@EigyoCdFrom int,
		@EigyoCdTo int ,
		@SyaRyoCdFrom int,
		@SyaRyoCdTo int,
		@RepairCdFrom int,
		@RepairCdTo int,		
		@OrderParam smallint,		
		@TenantCdSeq  int
    )
AS
begin
	DECLARE @CompanyTbl TABLE (CompanyID VARCHAR(10));
	INSERT INTO @CompanyTbl (CompanyID)
    SELECT value FROM STRING_SPLIT(@CompanyList,'-') WHERE value <> '';
      SELECT 
	   ROW_NUMBER() OVER (ORDER BY
                        
                       CASE @OrderParam
                                        WHEN 1
                                                        THEN concat(HENSYA.EigyoCdSeq,', ',SYARYO.SyaRyoCd,', ',SHURI.ShuriSYmd,', ',SHURI.ShuriSTime)
                        END ASC
                      , CASE @OrderParam
                                        WHEN 2
                                                        THEN concat(SHURI.ShuriSYmd,', ',SHURI.ShuriSTime)
                        END ASC
                      , CASE @OrderParam
                                        WHEN 3
                                                        THEN concat(SHURI.ShuriEYmd,', ',SHURI.ShuriETime)
                        END ASC)                                                                              AS RowNum,
		    SHURI.ShuriSYmd																		
           ,SHURI.ShuriSTime 																			
           ,SHURI.ShuriEYmd 																			
           ,SHURI.ShuriETime 																			
		   ,SHURI.BikoNm   																
		   ,SYARYO.SyaRyoCdSeq  																	
           ,SYARYO.SyaRyoCd  																		
           ,SYARYO.SyaRyoNm 																		
           ,SYARYO.KariSyaRyoNm 																		
		   ,EIGYOS.EigyoCdSeq  																	
           ,EIGYOS.EigyoCd  																			
           ,EIGYOS.EigyoNm 																			
           ,EIGYOS.RyakuNm  																		
 　　　　,REPAIR.RepairCdSeq																			
           ,REPAIR.RepairCd   																			
           ,REPAIR.RepairNm 																			
           ,REPAIR.RepairRyakuNm 																			
　　　　　,SHURI.BikoNm   																		          																			
	FROM TKD_Shuri AS SHURI																																						
	LEFT JOIN VPM_SyaRyo AS SYARYO 																			
	ON SYARYO.SyaRyoCdSeq = SHURI.SyaRyoCdSeq																																						
	LEFT JOIN VPM_HenSya AS HENSYA 																			
	ON  HENSYA.SyaRyoCdSeq = SYARYO.SyaRyoCdSeq																			
	AND SHURI.ShuriSYmd >= HENSYA.StaYmd																			
	AND SHURI.ShuriSYmd <= HENSYA.EndYmd																																					
	LEFT JOIN VPM_Eigyos AS EIGYOS 																			
	ON EIGYOS.EigyoCdSeq = HENSYA.EigyoCdSeq																																						
	LEFT JOIN VPM_Compny AS COMPANY																			
	ON COMPANY.CompanyCdSeq = EIGYOS.CompanyCdSeq																			
	AND COMPANY.TenantCdSeq = @TenantCdSeq    																																						
	LEFT JOIN VPM_Repair AS REPAIR 																			
	ON REPAIR.RepairCdSeq = SHURI.ShuriCdSeq																			
	AND REPAIR.TenantCdSeq =@TenantCdSeq      																																					
	WHERE 
	COMPANY.CompanyCdSeq IN  (CASE WHEN (SELECT COUNT(*) FROM @CompanyTbl) = 0 THEN (SELECT COMPANY.CompanyCd) -- input empty => select all
									 WHEN (SELECT COUNT(*) FROM @CompanyTbl) > 0 THEN (SELECT CompanyID FROM @CompanyTbl where COMPANY.CompanyCdSeq= CompanyID) 
								END)
	AND SHURI.ShuriSYmd >= @DateRepairFrom 																			
	AND SHURI.ShuriSYmd <= @DateRepairTo　
	AND EIGYOS.EigyoCd >=  CASE WHEN @EigyoCdFrom =0 or @EigyoCdTo = 0 THEN EIGYOS.EigyoCd ELSE @EigyoCdFrom END   
	--EIGYOS.EigyoCd >=  @EigyoCdFrom　																		
	--AND EIGYOS.EigyoCd <=  @EigyoCdTo　　			
	AND EIGYOS.EigyoCd <=  CASE WHEN @EigyoCdFrom =0 or @EigyoCdTo = 0 THEN EIGYOS.EigyoCd ELSE @EigyoCdTo END   
	--AND SYARYO.SyaRyoCd >= @SyaRyoCdFrom																	
	--AND SYARYO.SyaRyoCd <= @SyaRyoCdTo　		
	AND SYARYO.SyaRyoCd >=  CASE WHEN @SyaRyoCdFrom =0 or @SyaRyoCdTo = 0 THEN SYARYO.SyaRyoCd ELSE @SyaRyoCdFrom END   
	AND SYARYO.SyaRyoCd <=  CASE WHEN @SyaRyoCdFrom =0 or @SyaRyoCdTo = 0 THEN SYARYO.SyaRyoCd ELSE @SyaRyoCdTo END   
	--AND REPAIR.RepairCd >= @RepairCdFrom　																		
	--AND REPAIR.RepairCd <= @RepairCdTo　 
	AND REPAIR.RepairCd >=  CASE WHEN @RepairCdFrom =0 or @RepairCdTo = 0 THEN REPAIR.RepairCd ELSE @RepairCdFrom END   
	AND REPAIR.RepairCd <=  CASE WHEN @RepairCdFrom =0 or @RepairCdTo = 0 THEN REPAIR.RepairCd ELSE @RepairCdTo END 
	AND SHURI.SiyoKbn = 1																			
--①画面での出力順＝車輌順		
    ORDER BY
                        
                       CASE @OrderParam
                                        WHEN 1
                                                        THEN concat(HENSYA.EigyoCdSeq,', ',SYARYO.SyaRyoCd,', ',SHURI.ShuriSYmd,', ',SHURI.ShuriSTime)
                        END ASC
                      , CASE @OrderParam
                                        WHEN 2
                                                        THEN concat(SHURI.ShuriSYmd,', ',SHURI.ShuriSTime)
                        END ASC
                      , CASE @OrderParam
                                        WHEN 3
                                                        THEN concat(SHURI.ShuriEYmd,', ',SHURI.ShuriETime)
                        END ASC
    END 



GO


