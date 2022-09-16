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
-- Description:	Get staff data
-- =============================================
CREATE OR ALTER PROCEDURE PK_dStaffData_R
	@From			char(8),
	@To				char(8),
	@TenantCdSeq	int
AS
BEGIN
	SELECT TKD_Haisha.UkeNo,
     TKD_Haisha.UnkRen,
     TKD_Haisha.TeiDanNo,
     TKD_Haisha.BunkRen,
     TKD_Haisha.SyuKoYmd,
     TKD_Haisha.KikYmd,
     TKD_Haisha.DrvJin,
     TKD_YykSyu.KataKbn
FROM TKD_Haisha
INNER JOIN TKD_Yyksho
     ON TKD_Haisha.UkeNo = TKD_Yyksho.UkeNo
INNER JOIN TKD_YykSyu
     ON TKD_Haisha.UkeNo = TKD_YykSyu.UkeNo
     AND TKD_Haisha.UnkRen = TKD_YykSyu.UnkRen
     AND TKD_Haisha.SyaSyuRen = TKD_YykSyu.UnkRen
WHERE TKD_Haisha.KikYmd >= @From
     AND TKD_Haisha.SyuKoYmd <= @To
     AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq
     AND TKD_Yyksho.YoyaSyu = 1
     AND TKD_Yyksho.SiyoKbn = 1
     AND TKD_Haisha.SiyoKbn = 1
END
GO
