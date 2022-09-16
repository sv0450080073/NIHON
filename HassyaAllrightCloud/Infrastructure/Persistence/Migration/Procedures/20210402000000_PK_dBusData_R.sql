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
-- Author:		Tra Nguyen Lam 
-- Create date: 2021/04/02
-- Description:	Get Bus Data
-- =============================================
CREATE OR ALTER PROCEDURE PK_dBusData_R
	@From			char(8),
	@To				char(8),
	@TenantCdSeq	int,
	@Eigyos			varchar(max),
	@Compny			varchar(max)
AS
BEGIN
	SELECT VPM_SyaRyo.SyaRyoCdSeq,
     VPM_SyaSyu.KataKbn,
     VPM_SyaRyo.SyaRyoNm,
     VPM_HenSya.StaYmd,
     VPM_HenSya.EndYmd,
     VPM_SyaRyo.NinkaKbn,
     VPM_SyaSyu.SyaSyuCdSeq,
     VPM_SyaSyu.SyaSyuNm
FROM VPM_Eigyos
INNER JOIN VPM_Compny
     ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
INNER JOIN VPM_HenSya
     ON VPM_Eigyos.EigyoCdSeq = VPM_HenSya.EigyoCdSeq
INNER JOIN VPM_SyaRyo
     ON VPM_HenSya.SyaRyoCdSeq = VPM_SyaRyo.SyaRyoCdSeq
INNER JOIN VPM_SyaSyu
     ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
     AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN TKD_LockTable
     ON VPM_Compny.TenantCdSeq = TKD_LockTable.TenantCdSeq
     AND VPM_Eigyos.EigyoCdSeq = TKD_LockTable.EigyoCdSeq
WHERE VPM_Eigyos.SiyoKbn = 1
     AND VPM_Compny.TenantCdSeq = @TenantCdSeq
     AND VPM_HenSya.StaYmd <= @To	
     AND VPM_HenSya.EndYmd >= @From
     AND VPM_SyaRyo.NinkaKbn != 7
	 AND ((@Eigyos = '' AND (@Compny = '' OR VPM_Compny.CompanyCdSeq IN (SELECT * FROM STRING_SPLIT (@Compny , ',' )))) OR VPM_Eigyos.EigyoCdSeq IN (SELECT * FROM STRING_SPLIT (@Eigyos , ',' )))
ORDER BY VPM_SyaSyu.KataKbn,
     VPM_SyaSyu.SyaSyuCdSeq,
     VPM_HenSya.EigyoCdSeq,
     VPM_SyaSyu.SyaSyuCd,
     VPM_SyaRyo.SyaRyoCdSeq
END
GO
