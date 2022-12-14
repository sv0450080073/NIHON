USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dHaiin_R]    Script Date: 12/07/2020 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dHaiin_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from Haiin table
-- Date			:   2020/12/07
-- Author		:   N.N.T.AN
-- Description	:   Get data from Haiin table with conditions
------------------------------------------------------------
CREATE OR ALTER    PROCEDURE [dbo].[PK_dGetHaiin_R]
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
    TKD_Yyksho.UkeNo AS UkeNo --受付番号
    ,TKD_Yyksho.UkeCd AS UkeCd --受付コード
    ,CAST(TKD_Haisha.UnkRen as int) AS UnkRen --運行日連番
    ,CAST(TKD_Haisha.TeiDanNo as int) AS TeiDanNo --悌団番号
    ,CAST(TKD_Haisha.BunkRen as int) AS BunkRen --分割連番
    ,ISNULL(TKD_Haiin.HaiInRen, 0) AS HaiInRen --配員連番a
    ,ISNULL(TKD_Haisha.GoSya, '') AS GoSya --号車
    ,ISNULL(VPM_SyaSyu.SyaSyuNm, '') AS SyaSyuNm --車種名
    ,ISNULL(VPM_SyaRyo.TeiCnt, 0) AS TeiCnt --車両定員
    ,ISNULL(TKD_Unkobi.DanTaNm, '') AS Title -- タイトル
    ,ISNULL(TKD_Haisha.SyuKoYmd, '') AS SyuKoYmd -- 出庫年月日（開始年月日）
    ,ISNULL(TKD_Haisha.SyuKoTime, '') AS SyuKoTime -- 出庫時間（開始時間）
    ,ISNULL(TKD_Haisha.KikYmd, '') AS KikYmd -- 帰庫年月日（終了年月日）
    ,ISNULL(TKD_Haisha.KikTime, '') AS KikTime -- 帰庫時間（終了時間）
    ,ISNULL(TKD_Haiin.SyainCdSeq, 0) AS SyainCdSeq -- 社員SEQ
    ,ISNULL(VPM_Syain.SyainCd, '') AS SyainCd -- 社員コード
    ,ISNULL(VPM_Syain.SyainNm, '') AS SyainNm -- 社員名
    ,ISNULL(TKD_Haisha.HaiSNm, '') AS HaiSNm -- 配車地
    ,ISNULL(TKD_Haisha.HaiSTime, '') AS HaiSTime -- 配車時間
    ,ISNULL(TKD_Haisha.TouNm, '') AS TouNm -- 到着地
    ,ISNULL(TKD_Haisha.TouChTime, '') AS TouChTime -- 到着時間
    ,ISNULL(TKD_Unkobi.DanTaNm, '') AS DanTaNm -- 団体名
    ,ISNULL(TKD_Haisha.IkNm, '') AS IkNm -- 行先名
    ,ISNULL(eTKD_HaiinMail.SchReadKbn, 0) AS SchReadKbn --既読区分
FROM
    TKD_Haiin
    JOIN TKD_Haisha ON TKD_Haisha.UkeNo = TKD_Haiin.UkeNo
    AND TKD_Haisha.UnkRen = TKD_Haiin.UnkRen
    AND TKD_Haisha.TeiDanNo = TKD_Haiin.TeiDanNo
    AND TKD_Haisha.BunkRen = TKD_Haiin.BunkRen
    AND TKD_Haisha.SiyoKbn = 1
    JOIN VPM_Syain ON VPM_Syain.SyainCdSeq = TKD_Haiin.SyainCdSeq 
    JOIN TKD_Unkobi ON TKD_Unkobi.UkeNo = TKD_Haisha.UkeNo
    AND TKD_Unkobi.UnkRen = TKD_Haisha.UnkRen
    AND TKD_Unkobi.SiyoKbn = 1
    JOIN TKD_Yyksho ON TKD_Yyksho.UkeNo = TKD_Haisha.UkeNo
    AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq
    AND TKD_Yyksho.SiyoKbn = 1
    LEFT JOIN VPM_SyaRyo ON VPM_SyaRyo.SyaRyoCdSeq = TKD_Haisha.HaiSSryCdSeq
    LEFT JOIN VPM_SyaSyu ON VPM_SyaSyu.SyaSyuCdSeq = VPM_SyaRyo.SyaSyuCdSeq
    AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq    
    LEFT JOIN (
        SELECT
            DISTINCT UkeNo,
            UnkRen,
            TeiDanNo,
            BunkRen,
            HaiInRen,
            SchReadKbn
        FROM
            TKD_HaiinMail
        WHERE
            SyainCdSeq = 0
            AND KinKyuTblCdSeq = 0
            AND SchReadKbn = 1
    ) AS eTKD_HaiinMail ON TKD_Haiin.UkeNo = eTKD_HaiinMail.UkeNo
    AND TKD_Haiin.UnkRen = eTKD_HaiinMail.UnkRen
    AND TKD_Haiin.TeiDanNo = eTKD_HaiinMail.TeiDanNo
    AND TKD_Haiin.BunkRen = eTKD_HaiinMail.BunkRen
    AND TKD_Haiin.HaiInRen = eTKD_HaiinMail.HaiInRen
WHERE
    TKD_Haiin.SiyoKbn = 1 
    AND TKD_Haisha.TouYmd >= @FromDate
    AND TKD_Haisha.SyuKoYmd <= @ToDate
    AND TKD_Haiin.SyainCdSeq = @EmployeeId


		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN