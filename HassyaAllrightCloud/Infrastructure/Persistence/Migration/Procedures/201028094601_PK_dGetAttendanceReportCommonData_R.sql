USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetAttendanceReportCommonData_R]    Script Date: 2020/10/28 09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dGetAttendanceReportCommonData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetAttendanceReportCommonData
-- Date			:   2020/10/28
-- Author		:   Tra Nguyen 
-- Description	:   Get CommonData For AttendanceReport
CREATE OR ALTER PROCEDURE PK_dGetAttendanceReportCommonData_R
	@ProcessingDate varchar(8),
	@EigyoCdFrom		int,
	@EigyoCdTo		int,
	@CompanyCdSeq	int
AS
BEGIN
	SELECT ISNULL(eVPM_Syain01.SyainCdSeq, '') AS SyainSyainCdSeq,
		 ISNULL(eVPM_Syain01.SyainCd, '') AS SyainSyainCd,
		 ISNULL(eVPM_Syain01.SyainNm, '') AS SyainSyainNm,
		 ISNULL(VPM_Syokum.SyokumuKbn, 0) AS SyokumSyokumuKbn,
		 ISNULL(VPM_Eigyos.EigyoCdSeq, 0) AS EigyosEigyoCdSeq,
		 ISNULL(VPM_Eigyos.EigyoCd, 0) AS EigyosEigyoCd,
		 ISNULL(VPM_Eigyos.EigyoNm, '') AS EigyosEigyoNm,
		 ISNULL(VPM_Eigyos.RyakuNm, '') AS EigyosRyakuNm
	FROM VPM_KyoSHe
	LEFT JOIN VPM_Syain AS eVPM_Syain01
		 ON VPM_KyoSHe.SyainCdSeq = eVPM_Syain01.SyainCdSeq
	LEFT JOIN VPM_Syokum
		 ON VPM_KyoSHe.SyokumuCdSeq = VPM_Syokum.SyokumuCdSeq
	LEFT JOIN VPM_Eigyos
		 ON VPM_KyoSHe.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
	LEFT JOIN VPM_Compny
		 ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq

	WHERE VPM_Compny.CompanyCdSeq = @CompanyCdSeq --�u��Ёv�R���{�{�b�N�X�̒l��CompanyCdSeq
		 AND VPM_Syokum.SyokumuKbn BETWEEN 1 AND 4
		 AND VPM_KyoSHe.StaYmd <= @ProcessingDate								--�^�s�N���� yyyyMMdd
		 AND VPM_KyoSHe.EndYmd >= @ProcessingDate								--�^�s�N���� yyyyMMdd
		 AND (@EigyoCdFrom = 0 OR VPM_Eigyos.EigyoCd >= @EigyoCdFrom)			--�z�ԉc�Ə��R�[�h�@�J�n��EigyoCd
		 AND (@EigyoCdTo = 0 OR VPM_Eigyos.EigyoCd <= @EigyoCdTo)				--�z�ԉc�Ə��R�[�h�@�I����EigyoCd
	ORDER BY EigyosEigyoCd, EigyosEigyoCdSeq, SyainSyainCd
END
GO
