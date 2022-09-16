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
-- Description:	Get Bus Allocation
-- =============================================
CREATE OR ALTER PROCEDURE PK_dBusAllocation_R
	@From			char(8),
	@To				char(8),
	@TenantCdSeq	int
AS
BEGIN
	SELECT TKD_Yyksho.UkeNo,
     TKD_Haisha.HaiSSryCdSeq,
     TKD_Haisha.SyuKoYmd,
     TKD_Haisha.KikYmd,
     TKD_Haisha.KSKbn,
     VPM_SyaSyu.SyaSyuCdSeq,
     VPM_SyaSyu.SyaSyuNm,
     VPM_SyaSyu.KataKbn,
     VPM_SyaRyo.NinkaKbn
FROM TKD_Haisha
INNER JOIN TKD_Yyksho
     ON TKD_Haisha.UkeNo = TKD_Yyksho.UkeNo
LEFT JOIN TKD_YykSyu
     ON TKD_Haisha.UkeNo = TKD_YykSyu.UkeNo
     AND TKD_Haisha.UnkRen = TKD_YykSyu.UnkRen
     AND TKD_Haisha.SyaSyuRen = TKD_YykSyu.SyaSyuRen
LEFT JOIN VPM_SyaRyo
     ON TKD_Haisha.HaiSSryCdSeq = VPM_SyaRyo.SyaRyoCdSeq
LEFT JOIN VPM_SyaSyu
     ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
     AND VPM_SyaSyu.TenantCdSeq = TKD_Yyksho.TenantCdSeq -- ログインユーザーのTenantCdSeq
WHERE TKD_Haisha.KikYmd >= @From -- 開始日
     AND TKD_Haisha.SyuKoYmd <= @To -- 終了日　
     AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq -- ログインユーザーのTenantCdSeq
     AND TKD_Yyksho.YoyaSyu = 1
     AND TKD_Yyksho.SiyoKbn = 1
     AND TKD_Haisha.SiyoKbn = 1
     AND VPM_SyaRyo.NinkaKbn != 7
     AND TKD_Haisha.KSKbn = 2
END
GO
