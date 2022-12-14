USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetHaiinCustomGroup_R]    Script Date: 02/25/2021 4:55:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dGetHaiinCustomGroup_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get data from haiin table for custom group 
-- Date			:   2020/12/15
-- Author		:   N.N.T.AN
-- Description	:   Get data from haiin table for custom group with conditions
------------------------------------------------------------
CREATE OR ALTER            PROCEDURE [dbo].[PK_dGetHaiinCustomGroup_R]
		(
		--Parameter
			@EmployeeId			INT,
			@TennantCdSeq		INT,
			@GroupId			INT,
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		
SELECT
    ISNULL(TKD_Yyksho.UkeNo, 0) AS UkeNo --受付番号
    ,ISNULL(TKD_Yyksho.UkeCd, 0) AS UkeCd --受付コード
    ,ISNULL(CAST(TKD_Haisha.UnkRen AS int), 0) AS UnkRen --運行日連番
    ,ISNULL(CAST(TKD_Haisha.TeiDanNo AS int), 0) AS TeiDanNo --悌団番号
    ,ISNULL(CAST(TKD_Haisha.BunkRen AS int), 0) AS BunkRen --分割連番
    ,ISNULL(TKD_Haiin.HaiInRen, 0) AS HaiInRen --配員連番
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
    JOIN TKD_SchCusGrpMem ON TKD_SchCusGrpMem.SyainCdSeq = TKD_Haiin.SyainCdSeq --運行日取得
    JOIN TKD_Unkobi ON TKD_Unkobi.UkeNo = TKD_Haisha.UkeNo
    AND TKD_Unkobi.UnkRen = TKD_Haisha.UnkRen
    AND TKD_Unkobi.SiyoKbn = 1
    JOIN TKD_Yyksho ON TKD_Yyksho.UkeNo = TKD_Haisha.UkeNo
    AND TKD_Yyksho.TenantCdSeq = @TennantCdSeq
    AND TKD_Yyksho.SiyoKbn = 1
    LEFT JOIN VPM_SyaRyo ON VPM_SyaRyo.SyaRyoCdSeq = TKD_Haisha.HaiSSryCdSeq
    LEFT JOIN VPM_SyaSyu ON VPM_SyaSyu.SyaSyuCdSeq = VPM_SyaRyo.SyaSyuCdSeq
    AND VPM_SyaSyu.TenantCdSeq = @TennantCdSeq    
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
    AND TKD_Haiin.SyainCdSeq = @EmployeeId
WHERE
    TKD_Haiin.SiyoKbn = 1
    AND TKD_Haisha.TouYmd >= @FromDate
    AND TKD_Haisha.SyuKoYmd <= @ToDate
    AND TKD_SchCusGrpMem.CusGrpSeq = @GroupId



		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN