USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dKikyui_R]    Script Date: 12/07/2020 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dKikyui_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from Kikyui table
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get data from Kikyui table with conditions
------------------------------------------------------------
CREATE OR ALTER    PROCEDURE [dbo].[PK_dGetKikyuj_R]
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
    ISNULL(TKD_Kikyuj.KinKyuTblCdSeq, 0) AS KinKyuTblCdSeq   
   ,ISNULL(VPM_CodeKb.CodeKbnNm, '') AS TukiLabel --付きラベル
   ,ISNULL(VPM_KinKyu.KinKyuNm, '') AS Title --タイトル
   ,ISNULL(TKD_Kikyuj.KinKyuSYmd, '') AS KinKyuSYmd --勤務休日開始年月日
   ,ISNULL(TKD_Kikyuj.KinKyuSTime, '') AS KinKyuSTime --勤務休日開始時間
   ,ISNULL(TKD_Kikyuj.KinKyuEYmd, '') AS KinKyuEYmd --勤務休日終了年月日
   ,ISNULL(TKD_Kikyuj.KinKyuETime, '') AS KinKyuETime --勤務休日終了時間
   ,ISNULL(TKD_Kikyuj.BikoNm, '') AS BikoNm --備考
   ,ISNULL(TKD_Kikyuj.SyainCdSeq, 0) AS SyainCdSeq --社員コードSEQ
   ,ISNULL(VPM_Syain.SyainCd, '') AS SyainCd --社員コード
   ,ISNULL(VPM_Syain.SyainNm, '') AS SyainNm --社員名
   ,ISNULL(VPM_KinKyu.KinKyuKbn, 0) AS KinKyuKbn -- 勤務休日種別区分
   ,ISNULL(VPM_CodeKb.CodeKbnNm, '') AS KinKyuKbnNm --勤務休日種別区分名
   ,ISNULL(VPM_KinKyu.KinKyuNm, '') AS KinKyuNm --勤務休日種別名
   ,ISNULL(VPM_KinKyu.KinKyuCdSeq, 0) AS KinKyuCdSeq -- 勤務休日種別コードＳＥＱ
   ,ISNULL(eTKD_HaiinMail.SchReadKbn, 0) AS SchReadKbn --既読区分
FROM
    TKD_Kikyuj --社員情報取得
    JOIN VPM_Syain ON VPM_Syain.SyainCdSeq = TKD_Kikyuj.SyainCdSeq --勤務休日種別取得
    LEFT JOIN VPM_KinKyu ON VPM_KinKyu.KinKyuCdSeq = TKD_Kikyuj.KinKyuCdSeq
    AND VPM_KinKyu.SiyoKbn = 1 --勤務休日種別区分取得
	AND VPM_KinKyu.TenantCdSeq = @TenantCdSeq
    LEFT JOIN VPM_CodeKb ON VPM_CodeKb.CodeKbn = VPM_KinKyu.KinKyuKbn
    AND VPM_CodeKb.CodeSyu = 'KINKYUKBN'
    AND VPM_CodeKb.SiyoKbn = 1
    AND VPM_CodeKb.TenantCdSeq = (
        SELECT
            CASE
                WHEN COUNT(*) > 0 THEN @TenantCdSeq
                ELSE 0
            END AS TenantCdSeq
        FROM
            VPM_CodeSy
        WHERE
            CodeSyu = 'KINKYUKBN'
            AND KanriKbn <> 1
    )
    LEFT JOIN (
        SELECT
            DISTINCT SyainCdSeq,
            KinKyuTblCdSeq,
            SchReadKbn
        FROM
            TKD_HaiinMail
        WHERE
            UkeNo = 0
            AND UnkRen = 0
            AND TeiDanNo = 0
            AND BunkRen = 0
            AND HaiInRen = 0
            AND SchReadKbn = 1 --既読
    ) AS eTKD_HaiinMail ON eTKD_HaiinMail.SyainCdSeq = TKD_Kikyuj.SyainCdSeq
    AND eTKD_HaiinMail.KinKyuTblCdSeq = TKD_Kikyuj.KinKyuTblCdSeq
WHERE
    TKD_Kikyuj.SiyoKbn = 1
    AND TKD_Kikyuj.KinKyuEYmd >= @FromDate
    AND TKD_Kikyuj.KinKyuSYmd <= @ToDate
	AND TKD_Kikyuj.SyainCdSeq = @EmployeeId

		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN