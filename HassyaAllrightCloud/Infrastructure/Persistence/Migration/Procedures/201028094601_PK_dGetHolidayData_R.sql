USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetHolidayData_R]    Script Date: 2020/10/28 09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dGetHolidayData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetHolidayData
-- Date			:   2020/10/28
-- Author		:   Tra Nguyen 
-- Description	:   Get Holiday Data For AttendanceReport
CREATE OR ALTER PROCEDURE PK_dGetHolidayData_R
	@ProcessingDate		varchar(8),
	@EigyoCdFrom		int,
	@EigyoCdTo			int,
	@CompanyCdSeq		int
AS
BEGIN
	SELECT eTKD_Kikyuj01.SyainCdSeq,
     eTKD_Kikyuj01.KinKyuCdSeq,
     ISNULL(eVPM_KinKyu01.KinKyuCd, '') AS KinKyuCdKinKyuCd,
     ISNULL(eVPM_KinKyu01.KinKyuNm, '') AS KinKyuCdKinKyuNm,
     ISNULL(eVPM_KinKyu01.RyakuNm, '') AS KinKyuCdRyakuNm,
     ISNULL(eVPM_KinKyu01.KinKyuKbn, 0) AS KinKyuCdKinKyuKbn
FROM TKD_Kikyuj AS eTKD_Kikyuj01
INNER JOIN (
     SELECT eTKD_Koban00.KinKyuTblCdSeq,
          MIN(eTKD_Koban00.FuriYmd) AS Min_FuriYmd
     FROM TKD_Koban AS eTKD_Koban00
     WHERE eTKD_Koban00.UnkYmd = @ProcessingDate -- 運行年月日
     GROUP BY eTKD_Koban00.KinKyuTblCdSeq) AS eTKD_Koban01
     ON eTKD_Koban01.KinKyuTblCdSeq = eTKD_Kikyuj01.KinKyuTblCdSeq
LEFT JOIN VPM_KinKyu AS eVPM_KinKyu01
     ON eVPM_KinKyu01.KinKyuCdSeq = eTKD_Kikyuj01.KinKyuCdSeq
LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe03
     ON eVPM_KyoSHe03.SyainCdSeq = eTKD_Kikyuj01.SyainCdSeq
LEFT JOIN VPM_Eigyos AS eVPM_Eigyos04
     ON eVPM_Eigyos04.EigyoCdSeq = eVPM_KyoSHe03.EigyoCdSeq
LEFT JOIN VPM_Compny AS eVPM_Compny05
     ON eVPM_Compny05.CompanyCdSeq = eVPM_Eigyos04.CompanyCdSeq
WHERE eTKD_Kikyuj01.SiyoKbn = 1
     AND eVPM_Compny05.CompanyCdSeq = @CompanyCdSeq								--「会社」コンボボックスの値のCompanyCdSeq
     AND (@EigyoCdFrom = 0 OR eVPM_Eigyos04.EigyoCd >= @EigyoCdFrom)			--配車営業所コード　開始のEigyoCd
     AND (@EigyoCdTo = 0 OR eVPM_Eigyos04.EigyoCd <= @EigyoCdTo)				--配車営業所コード　終了のEigyoCd
END
GO
