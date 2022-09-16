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
-- Description:	Get Employee Data
-- =============================================
CREATE OR ALTER PROCEDURE PK_dEmployeeData_R
	@From			char(8),
	@To				char(8),
	@Eigyos			varchar(max),
	@TenantCdSeq	int
AS
BEGIN
	SELECT VPM_KyoSHe.SyainCdSeq,
     VPM_KyoSHe.BigTypeDrivingFlg,
     VPM_KyoSHe.MediumTypeDrivingFlg,
     VPM_KyoSHe.SmallTypeDrivingFlg,
     VPM_KyoSHe.StaYmd,
     VPM_KyoSHe.EndYmd
FROM VPM_KyoSHe
LEFT JOIN VPM_Syokum
     ON VPM_KyoSHe.SyokumuCdSeq = VPM_Syokum.SyokumuCdSeq
     AND VPM_Syokum.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_Eigyos
     ON VPM_KyoSHe.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
WHERE VPM_Syokum.SyokumuKbn IN (1, 2)
     AND VPM_KyoSHe.EigyoCdSeq IN (SELECT * FROM STRING_SPLIT (@Eigyos , ',' ))
     AND VPM_KyoSHe.StaYmd <= @To
     AND VPM_KyoSHe.EndYmd >= @From
END
GO
