USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dWorkHolidayType_R]    Script Date: 04/15/2021 10:06:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   AttendanceReport
-- SP-ID		:   PK_dWorkHolidayType_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   WorkHolidayType
-- Date			:   2020/10/28
-- Author		:   Tra Nguyen 
-- Description	:   Get work holiday Data For AttendanceReport
CREATE OR ALTER   PROCEDURE [dbo].[PK_dWorkHolidayType_R]
	@CompanyCdSeq		int,
    @TenantCdSeq        int
AS
BEGIN
	SELECT VPM_KinKyu.KinKyuCdSeq,
     VPM_KinKyu.KinKyuCd,
     VPM_KinKyu.KinKyuNm,
     VPM_CodeKb.CodeKbnNm
FROM VPM_KinKyu
LEFT JOIN VPM_CodeSy
     ON VPM_CodeSy.CodeSyu = 'KINKYUKBN'
LEFT JOIN VPM_CodeKb
     ON VPM_KinKyu.KinKyuKbn = VPM_CodeKb.CodeKbn
     AND VPM_CodeKb.SiyoKbn = 1
     AND VPM_CodeKb.CodeSyu = 'KINKYUKBN'
     AND ((VPM_CodeSy.KanriKbn = 1 AND VPM_CodeKb.TenantCdSeq = 0) OR (VPM_CodeSy.KanriKbn != 1 AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq)) -- ログインユーザーのTenantCdSeq
WHERE VPM_KinKyu.SiyoKbn = 1
     AND VPM_KinKyu.KinKyuCdSeq NOT IN (SELECT TKM_JisKin.KinKyuCdSeq FROM TKM_JisKin WHERE TKM_JisKin.CompanyCdSeq = @CompanyCdSeq)  -- 「会社」のCompanyCdSeq
     AND VPM_Kinkyu.TenantCdSeq = @TenantCdSeq
ORDER BY VPM_KinKyu.KinKyuCd
END
