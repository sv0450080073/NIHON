USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dAcquisitionData_R]    Script Date: 2021/06/10 10:23:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dAcquisitionData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data bill check list
-- Date			:   2020/08/24
-- Author		:   N.T.Lan.Anh
-- Description	:   Acquisition of vehicle data for billing checklist
------------------------------------------------------------
CREATE OR ALTER    PROCEDURE [dbo].[PK_dAcquisitionData_R]
	(
	-- Parameter
		    @TenantCdSeq int        
		,   @ListUkeNo  varchar(Max)
	 	-- Output
		,	@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS 
BEGIN
SELECT eTKD_YykSyu01.UkeNo,
	ISNULL(eVPM_SyaSyu03.SyaSyuNm, '') AS SyaSyuCd_SyaSyuNm,
	ISNULL(eVPM_CodeKb04.CodeKbnNm, '') AS KataKbn_CodeKbnNm,
	ISNULL(eVPM_CodeKb04.RyakuNm, '') AS KataKbn_RyakuNm 
FROM TKD_YykSyu AS eTKD_YykSyu01
INNER JOIN (
	SELECT eTKD_Haisha00.UkeNo,
		eTKD_Haisha00.UnkRen,
		eTKD_Haisha00.SyaSyuRen,
		eTKD_Haisha00.SiyoKbn
	FROM TKD_Yyksho AS eTKD_Yyksho00
	INNER JOIN TKD_Haisha AS eTKD_Haisha00
		ON eTKD_Haisha00.UkeNo = eTKD_Yyksho00.UkeNo
		AND eTKD_Haisha00.SiyoKbn = eTKD_Yyksho00.SiyoKbn
		AND eTKD_Haisha00.SiyoKbn = 1
	-- LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn00							-- 2021/06/03 DEL
		-- ON eVPM_YoyKbn00.YoyaKbnSeq = eTKD_Yyksho00.YoyaKbnSeq		-- 2021/06/03 DEL
	LEFT JOIN VPM_Eigyos AS eVPM_Eigyos00
		ON eVPM_Eigyos00.EigyoCdSeq = eTKD_Haisha00.SyuEigCdSeq
	WHERE eTKD_Yyksho00.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
	GROUP BY eTKD_Haisha00.UkeNo,
		eTKD_Haisha00.UnkRen,
		eTKD_Haisha00.SyaSyuRen,
		eTKD_Haisha00.SiyoKbn) AS eTKD_Haisha02
	ON eTKD_Haisha02.UkeNo = eTKD_YykSyu01.UkeNo
	AND eTKD_Haisha02.UnkRen = eTKD_YykSyu01.UnkRen
	AND eTKD_Haisha02.SyaSyuRen = eTKD_YykSyu01.SyaSyuRen
	AND eTKD_Haisha02.SiyoKbn = eTKD_YykSyu01.SiyoKbn
LEFT JOIN VPM_SyaSyu AS eVPM_SyaSyu03
	ON eVPM_SyaSyu03.SyaSyuCdSeq = eTKD_YykSyu01.SyaSyuCdSeq
	AND eVPM_SyaSyu03.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq	-- 2021/06/03 ADD
LEFT JOIN VPM_CodeKb AS eVPM_CodeKb04
	ON eVPM_CodeKb04.CodeSyu = 'KATAKBN'
	AND	eVPM_CodeKb04.CodeKbn = CONVERT(VARCHAR(10),eTKD_YykSyu01.KataKbn)
	AND eVPM_CodeKb04.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
WHERE (@ListUkeNo IS NULL OR (eTKD_YykSyu01.UkeNo IN (SELECT value FROM STRING_SPLIT(@ListUkeNo, ',')))) -- 表示している各行のUkeno

OPTION (RECOMPILE)
SET	@ROWCOUNT	=	@@ROWCOUNT
END
