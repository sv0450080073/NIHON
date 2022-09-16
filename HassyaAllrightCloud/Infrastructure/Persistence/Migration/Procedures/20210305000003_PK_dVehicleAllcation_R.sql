USE [HOC_Kashikiri]
GO
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
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dVehicleAllcation_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get VehicleAllocation List
-- Date			:   2021/02/18
-- Author		:   P.M.Nhat
-- Description	:   Get VehicleAllocation list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dVehicleAllcation_R
	@StartYmd varchar(8),
	@EndYmd varchar(8),
	@SyaRyoCdSeq int,
	@TenantCdSeq int
AS
BEGIN
	SELECT TKD_Yyksho.UkeNo,
		 TKD_Yyksho.UkeCd,
		 TKD_Haisha.HaiSSryCdSeq,
		 TKD_Unkobi.DanTaNm,
		 TKD_Haisha.KSKbn,
		 TKD_Haisha.SyuKoYmd,
		 TKD_Haisha.SyuKoTime,
		 TKD_Haisha.KikYmd,
		 TKD_Haisha.KikTime,
		 TKD_Yyksho.TokuiSeq,
		 TKD_Yyksho.SitenCdSeq,
		 VPM_SyaRyo.NinkaKbn,
		 SyaSyu1.KataKbn,
		 VPM_Tokisk.RyakuNm AS TokiskNm,
		 VPM_TokiSt.RyakuNm AS TokiStNm,
		 TKD_Unkobi.BikoNm
	FROM TKD_Yyksho
	LEFT JOIN TKD_Unkobi
		 ON TKD_Yyksho.UkeNo = TKD_Unkobi.UkeNo
	LEFT JOIN TKD_Haisha
		 ON TKD_Unkobi.UkeNo = TKD_Haisha.UkeNo
		 AND TKD_Unkobi.UnkRen = TKD_Haisha.UnkRen
	LEFT JOIN TKD_YykSyu
		 ON TKD_Haisha.UkeNo = TKD_YykSyu.UkeNo
		 AND TKD_Haisha.UnkRen = TKD_YykSyu.UnkRen
		 AND TKD_Haisha.SyaSyuRen = TKD_YykSyu.SyaSyuRen
	LEFT JOIN VPM_Eigyos
		 ON TKD_Yyksho.UkeEigCdSeq = VPM_Eigyos.EigyoCdSeq
	LEFT JOIN VPM_Tokisk
		 ON TKD_Yyksho.TokuiSeq = VPM_Tokisk.TokuiSeq
	LEFT JOIN VPM_TokiSt
		 ON TKD_Yyksho.TokuiSeq = VPM_TokiSt.TokuiSeq
		 AND TKD_Yyksho.SitenCdSeq = VPM_TokiSt.SitenCdSeq
	LEFT JOIN VPM_SyaSyu SyaSyu0
		 ON TKD_YykSyu.SyaSyuCdSeq = SyaSyu0.SyaSyuCdSeq
	LEFT JOIN TKD_Yousha
		 ON TKD_Haisha.UkeNo = TKD_Yousha.UkeNo
		 AND TKD_Haisha.UnkRen = TKD_Yousha.UnkRen
		 AND TKD_Haisha.YouTblSeq = TKD_Yousha.YouTblSeq
		 AND TKD_Yousha.SiyoKbn = 1
	LEFT JOIN VPM_SyaRyo
		 ON TKD_Haisha.HaiSSryCdSeq = VPM_SyaRyo.SyaRyoCdSeq
	LEFT JOIN VPM_SyaSyu SyaSyu1
		 ON VPM_SyaRyo.SyaSyuCdSeq = SyaSyu1.SyaSyuCdSeq
		 AND SyaSyu1.TenantCdSeq = @TenantCdSeq
	WHERE TKD_Haisha.KikYmd >= @StartYmd
		 AND TKD_Haisha.SyuKoYmd <= @EndYmd
		 AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq
		 AND TKD_Yyksho.YoyaSyu = 1
		 AND TKD_Yyksho.SiyoKbn = 1
		 AND TKD_Haisha.SiyoKbn = 1
		 AND TKD_Haisha.HaiSSryCdSeq = @SyaRyoCdSeq
	ORDER BY TKD_Yyksho.UkeNo
END
GO
