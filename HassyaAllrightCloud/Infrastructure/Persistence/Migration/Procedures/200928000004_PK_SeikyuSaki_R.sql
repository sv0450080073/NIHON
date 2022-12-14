--USE [HOC_Kashikiri]
--GO
/****** Object:  StoredProcedure [dbo].[PK_dSeikyuSaki_R]    Script Date: 2020/09/21 16:22:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
--	System-Name	:   新新発車オーライシステム
--	Module-Name	:   貸切モジュール
--	SP-ID		:   PK_SeikyuSaki_R
--	DB-Name		:   請求先リスト
--	Name		:   データ取得
--	Date		:   2020/09/08
--	Author		:   nhhkieuanh
--	Descriotion	:   請求先リストのSelect処理
------------------------------------------------------------
--	Update		:
--	Comment		:
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dSeikyuSaki_R]
--パラメータ
	(
		@TenantCdSeq				INT				-- テナントコード
	)
AS
BEGIN
	SELECT VPM_Gyosya.GyosyaCd,
		   VPM_Tokisk.TokuiCd,
		   VPM_Tokisk.RyakuNm,
		   VPM_TokiSt.SitenCd, 
		   VPM_TokiSt.SitenNm,
		   CONCAT(FORMAT(VPM_Tokisk.TokuiCd, '0000'), ' : ', VPM_Tokisk.RyakuNm, ' ',
				  FORMAT(VPM_TokiSt.SitenCd, '0000'), ' : ', VPM_TokiSt.SitenNm) AS Name
	FROM VPM_Tokisk
	INNER JOIN VPM_TokiSt
		   ON VPM_TokiSt.TokuiSeq = VPM_Tokisk.TokuiSeq
		   AND (CAST(VPM_TokiSt.SiyoStaYmd AS DATE) <= CAST(GETDATE() AS DATE))
	AND (CAST(VPM_TokiSt.SiyoEndYmd AS DATE) >= CAST(GETDATE() AS DATE))
	INNER JOIN VPM_Gyosya
		   ON VPM_Tokisk.GyosyaCdSeq = VPM_Gyosya.GyosyaCdSeq
		   AND VPM_Gyosya.SiyoKbn = 1
	WHERE (CAST(VPM_Tokisk.SiyoStaYmd AS DATE) <= CAST(GETDATE() AS DATE)) AND
		  (CAST(VPM_Tokisk.SiyoEndYmd AS DATE) >= CAST(GETDATE() AS DATE))
		   AND VPM_Tokisk.TenantCdSeq = @TenantCdSeq -- ログインしたユーザーのTenantCdSeq
	ORDER BY VPM_Gyosya.GyosyaCd, VPM_Tokisk.TokuiCd, VPM_TokiSt.SitenCd

END
