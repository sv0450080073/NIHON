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
-- Description:	Get work holiday data
-- =============================================
CREATE OR ALTER PROCEDURE PK_dWorkHolidayData_R
	@From			char(8),
	@To				char(8),
	@TenantCdSeq	int,
	@SyainCdSeq		varchar(max)
AS
BEGIN
	SELECT TKD_Kikyuj.SyainCdSeq, 
     VPM_KyoSHe.BigTypeDrivingFlg,
     VPM_KyoSHe.MediumTypeDrivingFlg,
     VPM_KyoSHe.SmallTypeDrivingFlg,
     TKD_Kikyuj.KinKyuSYmd,
     TKD_Kikyuj.KinKyuEYmd
FROM TKD_Kikyuj
LEFT JOIN VPM_KyoSHe
     ON TKD_Kikyuj.SyainCdSeq = VPM_KyoSHe.SyainCdSeq
WHERE TKD_Kikyuj.SyainCdSeq IN (SELECT * FROM STRING_SPLIT (@SyainCdSeq , ',' ))
     AND TKD_Kikyuj.KinKyuSYmd <= @To
     AND TKD_Kikyuj.KinKyuEYmd >= @From
     AND VPM_KyoSHe.StaYmd <= TKD_Kikyuj.KinKyuSYmd
     AND VPM_KyoSHe.EndYmd >= TKD_Kikyuj.KinKyuEYmd
END
GO
