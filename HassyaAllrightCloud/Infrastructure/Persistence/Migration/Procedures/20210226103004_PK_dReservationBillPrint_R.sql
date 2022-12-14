USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dReservationBillPrint_R]    Script Date: 2/26/2021 10:59:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dReservation_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetReservationAsync
-- Date			:   2020/08/10
-- Author		:   T.L.DUY
-- Description	:   Get reservation data with conditions
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dReservationBillPrint_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,				-- TenantCdSeq
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	-- Processing
BEGIN
	SELECT																												
	CONCAT(																												
	CASE																												
	WHEN VPM_YoyaKbnSort.PriorityNum IS NULL THEN '99'																												
	ELSE FORMAT(VPM_YoyaKbnSort.PriorityNum, '00')																												
	END,																												
	FORMAT(VPM_YoyKbn.YoyaKbn, '00')																												
	) AS Code,																												
	VPM_YoyKbn.YoyaKbnNm AS Name,
	VPM_YoyKbn.YoyaKbnNm AS CodeText,
	VPM_YoyKbn.YoyaKbn AS CodeNumber
	FROM																												
	VPM_YoyKbn																												
	LEFT JOIN VPM_YoyaKbnSort ON VPM_YoyaKbnSort.YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq																												
	AND VPM_YoyaKbnSort.TenantCdSeq = @TenantCdSeq																												
	WHERE VPM_YoyKbn.SiyoKbn = 1																												
	ORDER BY	 Code
	SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN
