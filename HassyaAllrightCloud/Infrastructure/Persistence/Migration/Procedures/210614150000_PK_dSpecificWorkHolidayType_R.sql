USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dSpecificWorkHolidayType_R]    Script Date: 04/20/2021 11:46:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dSpecificWorkHolidayType_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   WorkHolidayType
-- Date			:   2021/04/20
-- Author		:   An Nguyen 
-- Description	:   Get specific work holiday Data For AttendanceReport
CREATE OR ALTER     PROCEDURE [dbo].[PK_dSpecificWorkHolidayType_R]
	@CompanyCdSeq		int,
	@TenantCdSeq		int
AS
BEGIN
	SELECT TKM_JisKin.JisKinKyuCd,
     VPM_KinKyu.KinKyuCdSeq,
     VPM_KinKyu.KinKyuCd,
     VPM_KinKyu.KinKyuNm,
     VPM_CodeKb.CodeKbnNm
FROM TKM_JisKin
LEFT JOIN VPM_KinKyu
     ON TKM_JisKin.KinKyuCdSeq = VPM_KinKyu.KinKyuCdSeq
     AND VPM_Kinkyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_CodeSy
ON VPM_CodeSy.CodeSyu = 'KINKYUKBN'
LEFT JOIN VPM_CodeKb
     ON VPM_KinKyu.KinKyuKbn = VPM_CodeKb.CodeKbn
     AND VPM_CodeKb.SiyoKbn = 1
     AND VPM_CodeKb.CodeSyu = VPM_CodeSy.CodeSyu
     AND ((VPM_CodeSy.KanriKbn = 1 AND VPM_CodeKb.TenantCdSeq = 0) OR (VPM_CodeSy.KanriKbn != 1 AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq)) -- ログインユーザーのTenantCdSeq
WHERE TKM_JisKin.CompanyCdSeq = @CompanyCdSeq -- 「会社」のCompanyCdSeq
ORDER BY TKM_JisKin.JisKinKyuCd,
     VPM_KinKyu.KinKyuCd
END
