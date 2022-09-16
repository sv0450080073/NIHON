USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetVehicleAllocaltionData_R]    Script Date: 2020/10/28 09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dGetVehicleAllocaltionData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetListVehicleAllocation
-- Date			:   2020/10/28
-- Author		:   Tra Nguyen 
-- Description	:   Get Vehicle Allocation List For AttendanceReport
CREATE OR ALTER PROCEDURE PK_dGetVehicleAllocaltionData_R
	@ProcessingDate varchar(8),
	@EigyoCdFrom		int,
	@EigyoCdTo		int,
	@CompanyCdSeq	int,
	@YoyaKbnSortFrom	int,
	@YoyaKbnSortTo		int,
	@TenantCdSeq	int
AS
BEGIN
	SELECT eTKD_Haisha05.HaiSSryCdSeq,
     eTKD_Haisha05.SyuKoYmd,
     eTKD_Haisha05.SyuKoTime,
     eTKD_Haisha05.KikYmd,
     eTKD_Haisha05.KikTime
	FROM TKD_Haisha AS eTKD_Haisha05
	INNER JOIN TKD_Yyksho AS eTKD_Yyksho01
			ON eTKD_Yyksho01.UkeNo = eTKD_Haisha05.UkeNo
			AND eTKD_Yyksho01.SiyoKbn = 1
			AND eTKD_Yyksho01.YoyaSyu = 1
	INNER JOIN TKD_Unkobi AS eTKD_Unkobi02
			ON eTKD_Unkobi02.SiyoKbn = 1
			AND eTKD_Unkobi02.UkeNo = eTKD_Haisha05.UkeNo
			AND eTKD_Unkobi02.UnkRen = eTKD_Haisha05.UnkRen
	INNER JOIN TKD_YykSyu AS eTKD_YykSyu03
			ON eTKD_YykSyu03.SiyoKbn = 1
			AND eTKD_YykSyu03.UkeNo = eTKD_Haisha05.UkeNo
			AND eTKD_YykSyu03.UnkRen = eTKD_Haisha05.UnkRen
			AND eTKD_YykSyu03.SyaSyuRen = eTKD_Haisha05.SyaSyuRen
	LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
			ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
			AND eVPM_YoyKbn01.TenantCdSeq = @TenantCdSeq
	LEFT JOIN VPM_HenSya AS eVPM_HenSya31
			ON eVPM_HenSya31.SyaRyoCdSeq = eTKD_Haisha05.HaiSSryCdSeq
	AND eTKD_Haisha05.HaiSYmd BETWEEN eVPM_HenSya31.StaYmd AND eVPM_HenSya31.EndYmd
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos32 
			ON eVPM_Eigyos32.EigyoCdSeq = eVPM_HenSya31.EigyoCdSeq
	WHERE eTKD_Haisha05.SiyoKbn = 1
			AND eVPM_Eigyos32.CompanyCdSeq = @CompanyCdSeq	--「会社」コンボボックスの値のCompanyCdSeq
			AND eTKD_Haisha05.SyuKoYmd <= @ProcessingDate -- 運行年月日
			AND eTKD_Haisha05.KikYmd >= @ProcessingDate -- 運行年月日
			AND (@EigyoCdFrom = 0 OR eVPM_Eigyos32.EigyoCd >= @EigyoCdFrom) 		--配車営業所コード　開始のEigyoCd
			AND (@EigyoCdTo = 0 OR eVPM_Eigyos32.EigyoCd <= @EigyoCdTo)			--配車営業所コード　終了のEigyoCd
			AND (@YoyaKbnSortFrom = 0 OR eVPM_YoyKbn01.YoyaKbn >= @YoyaKbnSortFrom)			--予約区分　開始のYoyaKbn
			AND (@YoyaKbnSortTo = 0 OR eVPM_YoyKbn01.YoyaKbn <= @YoyaKbnSortTo)				--予約区分　終了のYoyaKbn
END
GO
