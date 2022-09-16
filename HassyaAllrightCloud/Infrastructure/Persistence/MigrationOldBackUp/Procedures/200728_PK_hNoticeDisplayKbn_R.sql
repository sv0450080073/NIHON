USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PP_mCompany_R]    Script Date: 2020/07/24 17:18:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   新発車オーライシステムクラウド
-- Module-Name	:   貸切バスモジュール
-- SP-ID		:   PK_hNoticeDisplayKbn_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   データ取得
-- Date			:   2020/07/28
-- Author		:   kieuanhnhh 
-- Descriotion	:   指定条件からデータ取得
------------------------------------------------------------

CREATE PROCEDURE [dbo].[PK_hNoticeDisplayKbn_R]
--パラメータ
	(
		@SiyoKbn	    INTEGER					    -- ログインしたユーザーのSiyoKbn
	)
AS
SELECT CodeKbn, RyakuNm
FROM VPM_CodeKb
WHERE CodeSyu = 'NoticeDisplayKbn'
AND SiyoKbn = @SiyoKbn