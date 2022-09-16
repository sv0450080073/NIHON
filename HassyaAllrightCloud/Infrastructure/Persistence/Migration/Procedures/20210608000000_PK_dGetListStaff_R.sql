USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetListStaff_R]    Script Date: 2020/10/28 09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dGetListStaff_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetListStaff
-- Date			:   2020/10/28
-- Author		:   Tra Nguyen 
-- Description	:   Get List Staff Code For AttendanceReport
CREATE OR ALTER PROCEDURE PK_dGetListStaff_R
	@ProcessingDate		varchar(8),
	@EigyoCdFrom		int,
	@EigyoCdTo			int,
	@YoyaKbnSortFrom		int,
	@YoyaKbnSortTo			int,
	@CompanyCdSeq		int,
	@TenantCdSeq		int
AS
BEGIN
	SELECT ISNULL(eTKD_Haiin43.SyainCdSeq, 0) AS SyainCdSeq
	FROM TKD_Haisha AS eTKD_Haisha05
	INNER JOIN TKD_Yyksho AS eTKD_Yyksho01
			ON eTKD_Yyksho01.UkeNo = eTKD_Haisha05.UkeNo
			AND eTKD_Yyksho01.SiyoKbn = 1
	INNER JOIN TKD_Unkobi AS eTKD_Unkobi02
			ON eTKD_Unkobi02.SiyoKbn = 1
			AND eTKD_Unkobi02.UkeNo = eTKD_Haisha05.UkeNo
			AND eTKD_Unkobi02.UnkRen = eTKD_Haisha05.UnkRen
	INNER JOIN TKD_YykSyu AS eTKD_YykSyu03
			ON eTKD_YykSyu03.SiyoKbn = 1
			AND eTKD_YykSyu03.UkeNo = eTKD_Haisha05.UkeNo
			AND eTKD_YykSyu03.UnkRen = eTKD_Haisha05.UnkRen
			AND eTKD_YykSyu03.SyaSyuRen = eTKD_Haisha05.SyaSyuRen
	LEFT JOIN TKD_Haiin AS eTKD_Haiin43 ON eTKD_Haiin43.UkeNo = eTKD_Haisha05.UkeNo
			AND eTKD_Haiin43.UnkRen = eTKD_Haisha05.UnkRen
			AND eTKD_Haiin43.TeiDanNo = eTKD_Haisha05.TeiDanNo
			AND eTKD_Haiin43.BunkRen = eTKD_Haisha05.BunkRen
			AND eTKD_Haiin43.SiyoKbn = 1
	LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn01
			ON eTKD_Yyksho01.YoyaKbnSeq = eVPM_YoyKbn01.YoyaKbnSeq
			AND eVPM_YoyKbn01.TenantCdSeq = @TenantCdSeq
	LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe44
			ON eVPM_KyoSHe44.SyainCdSeq = eTKD_Haiin43.SyainCdSeq
			AND eTKD_Haisha05.HaiSYmd BETWEEN eVPM_KyoSHe44.StaYmd AND eVPM_KyoSHe44.EndYmd
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos48
			ON eVPM_Eigyos48.EigyoCdSeq = eVPM_KyoSHe44.EigyoCdSeq
	LEFT JOIN VPM_Compny AS eVPM_Compny49
			ON eVPM_Compny49.CompanyCdSeq = eVPM_Eigyos48.CompanyCdSeq
	WHERE eTKD_Haisha05.SiyoKbn = 1
			AND eVPM_Compny49.CompanyCdSeq = @CompanyCdSeq
			AND eTKD_Haisha05.SyuKoYmd >= @ProcessingDate
			AND eTKD_Haisha05.KikYmd <= @ProcessingDate
			AND eTKD_Yyksho01.YoyaSyu = 1
			AND (@EigyoCdFrom = 0 OR eVPM_Eigyos48.EigyoCd >= @EigyoCdFrom)				--配車営業所コード　開始のEigyoCd
			AND (@EigyoCdTo = 0 OR eVPM_Eigyos48.EigyoCd <= @EigyoCdTo)				--配車営業所コード　終了のEigyoCd
			AND (@YoyaKbnSortFrom = 0 OR eVPM_YoyKbn01.YoyaKbn >= @YoyaKbnSortFrom)				--予約区分　開始のYoyaKbn
			AND (@YoyaKbnSortTo = 0 OR eVPM_YoyKbn01.YoyaKbn <= @YoyaKbnSortTo)				--予約区分　終了のYoyaKbn
END
GO
