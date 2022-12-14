USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetUnkobiFile_R]    Script Date: 12/08/2020 1:21:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dHaiin_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from UnkobiFile table
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get data from UnkobiFile table with conditions
------------------------------------------------------------
CREATE OR ALTER      PROCEDURE [dbo].[PK_dGetUnkobiFile_R]
		(
		--Parameter
			@TenantCdSeq		INT,
			@EmployeeId			INT,
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		
		SELECT
    TKD_UnkobiFile.UkeNo AS UkeNo --受付番号
    ,CAST(TKD_UnkobiFile.UnkRen as int) AS UnkRen --運行日連番
    ,TKD_UnkobiFile.FileNo AS FileNo --ファイル番号
    ,'' AS FileNm --ファイル名
    ,'' AS FileLink --ファイルリンク
FROM
    TKD_UnkobiFile
    JOIN TKD_Haisha ON TKD_Haisha.UkeNo = TKD_UnkobiFile.UkeNo
    AND TKD_Haisha.UnkRen = TKD_UnkobiFile.UnkRen
    AND TKD_Haisha.SiyoKbn = 1
    JOIN TKD_Haiin ON TKD_Haiin.UkeNo = TKD_Haisha.UkeNo
    AND TKD_Haiin.UnkRen = TKD_Haisha.UnkRen
    AND TKD_Haiin.TeiDanNo = TKD_Haisha.TeiDanNo
    AND TKD_Haiin.BunkRen = TKD_Haisha.BunkRen
    AND TKD_Haiin.SiyoKbn = 1
WHERE
    TKD_UnkobiFile.TenantCdSeq = @TenantCdSeq
    AND TKD_UnkobiFile.SiyoKbn = 1
    AND TKD_Haisha.TouYmd >= @FromDate
    AND TKD_Haisha.SyuKoYmd <= @ToDate
    AND TKD_Haiin.SyainCdSeq = @EmployeeId
GROUP BY
    TKD_UnkobiFile.UkeNo,
    TKD_UnkobiFile.UnkRen,
    TKD_UnkobiFile.FileNo



		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN