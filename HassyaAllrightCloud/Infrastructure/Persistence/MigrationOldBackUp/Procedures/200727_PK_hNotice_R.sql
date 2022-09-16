USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PP_mCompany_R]    Script Date: 2020/07/24 17:18:28 ******/
USE [HOC_Kashikiri]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   新発車オーライシステムクラウド
-- Module-Name	:   貸切バスモジュール
-- SP-ID		:   PK_hNotice_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   データ取得
-- Date			:   2020/07/27
-- Author		:   kieuanhnhh 
-- Descriotion	:   指定条件からデータ取得
------------------------------------------------------------

CREATE PROCEDURE [dbo].[PK_hNotice_R]
--パラメータ
	(
		@CompanyCdSeq	    INTEGER					    -- 会社シーケンス
	,	@EigyoCdSeq		    INTEGER					    -- ログインしたユーザーのEigyoCdSeq	
	)
AS
SELECT TOP 50 FORMAT(CONVERT(datetime, n.UpdYmd), 'yy/MM/dd') AS UpdYmd
	, (SUBSTRING(n.UpdTime,1,2) + ':' + SUBSTRING(n.UpdTime,3,2)) AS UpdTime, n.UpdSyainCd, s.SyainNm, n.NoticeCdSeq, n.NoticeContent, n.NoticeDisplayKbn
FROM TKD_Notice AS n 
        INNER JOIN VPM_Syain AS s ON s.SyainCdSeq = n.UpdSyainCd
WHERE n.SiyoKbn = 1
AND n.NoticeDisplayKbn = 1
UNION
SELECT TOP 50 FORMAT(CONVERT(datetime, n.UpdYmd), 'yy/MM/dd') AS UpdYmd
	, (SUBSTRING(n.UpdTime,1,2) + ':' + SUBSTRING(n.UpdTime,3,2)) AS UpdTime, n.UpdSyainCd, s.SyainNm, n.NoticeCdSeq, n.NoticeContent, n.NoticeDisplayKbn
FROM TKD_Notice AS n 
        INNER JOIN VPM_Syain AS s ON s.SyainCdSeq = n.UpdSyainCd
        INNER JOIN VPM_KyoSHe AS k ON s.SyainCdSeq = k.SyainCdSeq
        INNER JOIN VPM_Eigyos AS e ON k.EigyoCdSeq = e.EigyoCdSeq
        INNER JOIN VPM_Compny AS c ON e.CompanyCdSeq = c.CompanyCdSeq
WHERE n.SiyoKbn = 1
AND n.NoticeDisplayKbn = 2
AND c.CompanyCdSeq = 1  /* ログインユーザの会社のCompanyCdSeq */
UNION
SELECT TOP 50 FORMAT(CONVERT(datetime, n.UpdYmd), 'yy/MM/dd') AS UpdYmd
	, (SUBSTRING(n.UpdTime,1,2) + ':' + SUBSTRING(n.UpdTime,3,2)) AS UpdTime, n.UpdSyainCd, s.SyainNm, n.NoticeCdSeq, n.NoticeContent, n.NoticeDisplayKbn
FROM TKD_Notice AS n 
        INNER JOIN VPM_Syain AS s ON s.SyainCdSeq = n.UpdSyainCd
        INNER JOIN VPM_KyoSHe AS k ON s.SyainCdSeq = k.SyainCdSeq
        INNER JOIN VPM_Eigyos AS e ON k.EigyoCdSeq = e.EigyoCdSeq
        INNER JOIN VPM_Compny AS c ON e.CompanyCdSeq = c.CompanyCdSeq
WHERE n.SiyoKbn = 1
AND n.NoticeDisplayKbn = 3
AND c.CompanyCdSeq = @CompanyCdSeq  /* ログインユーザの会社のCompanyCdSeq */
AND e.EigyoCdSeq = @EigyoCdSeq  /* ログインユーザの営業所のEigyoCdSeq */
ORDER BY UpdYmd, UpdTime DESC