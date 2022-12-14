USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dReservation_R]    Script Date: 01/12/2021 10:38:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dReservation_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get reservation List
-- Date			:   2020/08/17
-- Author		:   N.N.T.AN
-- Description	:   Get reservation list with conditions
------------------------------------------------------------
CREATE OR ALTER    PROCEDURE [dbo].[PK_dReservation_R]
		(
		--Parameter
            @TenantCdSeq		INT = NULL,					--Tenant
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		SELECT
    VPM_YoyKbn.YoyaKbnSeq,
    VPM_YoyKbn.YoyaKbnNm AS YoyaKbnNm
FROM
    VPM_YoyKbn
    LEFT JOIN VPM_YoyaKbnSort ON VPM_YoyaKbnSort.YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq
    AND VPM_YoyaKbnSort.TenantCdSeq = @TenantCdSeq
WHERE
    VPM_YoyKbn.SiyoKbn = 1
ORDER BY
    CONCAT(
        CASE
            WHEN VPM_YoyaKbnSort.PriorityNum IS NULL THEN '99'
            ELSE FORMAT(VPM_YoyaKbnSort.PriorityNum, '00')
        END,
        FORMAT(VPM_YoyKbn.YoyaKbn, '00'))

		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN