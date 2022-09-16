USE [HOC_Kashikiri]
GO
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
CREATE OR ALTER  PROCEDURE [dbo].[PK_dAcquisitionData_R]
	(
	-- Parameter
		    @TenantCdSeq int        
		,   @ListUkeNo  varchar(Max)
	 	-- Output
		,	@ROWCOUNT				INTEGER OUTPUT		-- 処理件数
	)
AS 
DECLARE @strSQL VARCHAR(MAX)
BEGIN
			SET	@strSQL             =
+   CHAR(13)+CHAR(10)	+	'SELECT eTKD_YykSyu01.UkeNo, '
	+   CHAR(13)+CHAR(10)	+	'ISNULL(eVPM_SyaSyu03.SyaSyuNm, '''') AS SyaSyuCd_SyaSyuNm, '
	+   CHAR(13)+CHAR(10)	+	'ISNULL(eVPM_CodeKb04.CodeKbnNm, '''') AS KataKbn_CodeKbnNm, '
	+   CHAR(13)+CHAR(10)	+	'ISNULL(eVPM_CodeKb04.RyakuNm, '''') AS KataKbn_RyakuNm  '
+   CHAR(13)+CHAR(10)	+	'FROM TKD_YykSyu AS eTKD_YykSyu01 '
+   CHAR(13)+CHAR(10)	+	'INNER JOIN ( '
	+   CHAR(13)+CHAR(10)	+	'SELECT eTKD_Haisha00.UkeNo, '
		+   CHAR(13)+CHAR(10)	+	'eTKD_Haisha00.UnkRen, '
		+   CHAR(13)+CHAR(10)	+	'eTKD_Haisha00.SyaSyuRen, '
		+   CHAR(13)+CHAR(10)	+	'eTKD_Haisha00.SiyoKbn '
	+   CHAR(13)+CHAR(10)	+	'FROM TKD_Yyksho AS eTKD_Yyksho00 '
	+   CHAR(13)+CHAR(10)	+	'INNER JOIN TKD_Haisha AS eTKD_Haisha00 '
		+   CHAR(13)+CHAR(10)	+	'ON eTKD_Haisha00.UkeNo = eTKD_Yyksho00.UkeNo '
		+   CHAR(13)+CHAR(10)	+	'AND eTKD_Haisha00.SiyoKbn = eTKD_Yyksho00.SiyoKbn '
		+   CHAR(13)+CHAR(10)	+	'AND eTKD_Haisha00.SiyoKbn = 1 '
	+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn00 '
		+   CHAR(13)+CHAR(10)	+	'ON eVPM_YoyKbn00.YoyaKbnSeq = eTKD_Yyksho00.YoyaKbnSeq '
	+   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_Eigyos AS eVPM_Eigyos00 '
		+   CHAR(13)+CHAR(10)	+	'ON eVPM_Eigyos00.EigyoCdSeq = eTKD_Haisha00.SyuEigCdSeq '
	+   CHAR(13)+CHAR(10)	+ CONCAT('WHERE eTKD_Yyksho00.TenantCdSeq = ', @TenantCdSeq)
	+   CHAR(13)+CHAR(10)	+	'GROUP BY eTKD_Haisha00.UkeNo, '
		+   CHAR(13)+CHAR(10)	+	'eTKD_Haisha00.UnkRen, '
		+   CHAR(13)+CHAR(10)	+	'eTKD_Haisha00.SyaSyuRen, '
		+   CHAR(13)+CHAR(10)	+	'eTKD_Haisha00.SiyoKbn) AS eTKD_Haisha02 '
	+   CHAR(13)+CHAR(10)	+	'ON eTKD_Haisha02.UkeNo = eTKD_YykSyu01.UkeNo '
	+   CHAR(13)+CHAR(10)	+	'AND eTKD_Haisha02.UnkRen = eTKD_YykSyu01.UnkRen '
	+   CHAR(13)+CHAR(10)	+	'AND eTKD_Haisha02.SyaSyuRen = eTKD_YykSyu01.SyaSyuRen '
	+   CHAR(13)+CHAR(10)	+	'AND eTKD_Haisha02.SiyoKbn = eTKD_YykSyu01.SiyoKbn '
    +   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_SyaSyu AS eVPM_SyaSyu03 '
	+   CHAR(13)+CHAR(10)	+	'ON eVPM_SyaSyu03.SyaSyuCdSeq = eTKD_YykSyu01.SyaSyuCdSeq '
	+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_SyaSyu03.TenantCdSeq = ', @TenantCdSeq)
    +   CHAR(13)+CHAR(10)	+	'LEFT JOIN VPM_CodeKb AS eVPM_CodeKb04 '
	+   CHAR(13)+CHAR(10)	+	'ON eVPM_CodeKb04.CodeSyu = ''KATAKBN'' '
	+   CHAR(13)+CHAR(10)	+	'AND	eVPM_CodeKb04.CodeKbn = CONVERT(VARCHAR(10),eTKD_YykSyu01.KataKbn) '
	+   CHAR(13)+CHAR(10)	+ CONCAT('AND eVPM_CodeKb04.TenantCdSeq = ', @TenantCdSeq)
	
	IF (@ListUkeNo IS NOT NULL)
	BEGIN
		SET @strSQL = @strSQL +
			+   CHAR(13)+CHAR(10)	+ CONCAT('WHERE eTKD_YykSyu01.UkeNo IN ', @ListUkeNo)
	END	
	ELSE
		BEGIN
		SET @strSQL = @strSQL +
			+   CHAR(13)+CHAR(10)	+ 'WHERE 1 = 0 '
	END	
	EXEC(@strSQL)
SET	@ROWCOUNT	=	@@ROWCOUNT
END
