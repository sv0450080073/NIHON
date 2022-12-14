USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dNewKotei_R]    Script Date: 2020/08/10 15:49:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
----------------------------------------------------
-- System-Name	:	新発車オーライシステム
-- Module-Name	:	貸切バスモジュール
-- SP-ID		:	[PK_dNewKotei_R]
-- DB-Name		:	行程テーブル
-- Name			:	データ取得
-- Date			:	2020/08/07
-- Author		:	nhhkieuanh
-- Descriotion	:	行程テーブルのSelect処理
----------------------------------------------------
-- Update		:	
-- Comment		:	
----------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dNewKotei_R]
--パラメータ

	(
		@UkeNo				CHAR(15)					-- 受付番号
	,	@UnkRen				VARCHAR(3)					-- 運行連番
	,   @SyaSyuRen			INTEGER						-- 車種連番
	,	@OutputUnit			VARCHAR(1)					-- 出力単位(1:予約毎 2:予約車種毎)	
	,	@ROWCOUNT			INTEGER			OUTPUT		-- 処理件数
	)
AS

DECLARE @tempTbl TABLE(
	OrderNumber int,
	UkeNo nchar(15),
	Koutei nvarchar(102),
	KoteiYmd nchar(20)
)
DECLARE	@rowCount1 int

IF @OutputUnit = 1 OR (SELECT COUNT(*) FROM TKD_Kotei 
			INNER JOIN TKD_Haisha AS KT_Haisha
			ON TKD_Kotei.UkeNo = KT_Haisha.UkeNo
			AND TKD_Kotei.UnkRen = KT_Haisha.UnkRen	
			AND TKD_Kotei.TeiDanNo = KT_Haisha.TeiDanNo	
			WHERE TKD_Kotei.UkeNo = @UkeNo
				AND TKD_Kotei.UnkRen = @UnkRen
				AND KT_Haisha.SyaSyuRen = @SyaSyuRen
			) = 0
BEGIN

	INSERT INTO	@tempTbl
	SELECT 1, TKD_Kotei.UkeNo, TKD_Kotei.Koutei, SUBSTRING(CONVERT(varchar, (CASE 
					WHEN TKD_Kotei.TomKbn = 2
						THEN DATEADD(DAY, -1, TKD_Unkobi.HaiSYmd)
					WHEN TKD_Kotei.TomKbn = 3
						THEN DATEADD(DAY, 1, TKD_Unkobi.TouYmd)
					ELSE 
						DATEADD(DAY, TKD_Kotei.Nittei - 1, TKD_Unkobi.HaiSYmd)	
				 END), 11), 4, 5) AS KoteiYmd
	FROM TKD_Kotei
	LEFT JOIN TKD_Unkobi
		ON TKD_Kotei.UkeNo = TKD_Unkobi.UkeNo
		AND TKD_Kotei.UnkRen = TKD_Unkobi.UnkRen
	WHERE TKD_Kotei.UkeNo = @UkeNo
		AND TKD_Kotei.UnkRen = @UnkRen
		AND TKD_Kotei.TeiDanNo = 0
		AND TKD_Kotei.SiyoKbn = 1
	ORDER BY KoteiYmd, TKD_Kotei.KouRen
	SET	@ROWCOUNT	=	@@ROWCOUNT
END
ELSE
BEGIN

	INSERT INTO	@tempTbl
	SELECT 1, TKD_Kotei.UkeNo, TKD_Kotei.Koutei, SUBSTRING(CONVERT(varchar, (CASE 
					WHEN TKD_Kotei.TomKbn = 2
						THEN DATEADD(DAY, -1, TKD_Unkobi.HaiSYmd)
					WHEN TKD_Kotei.TomKbn = 3
						THEN DATEADD(DAY, 1, TKD_Unkobi.TouYmd)
					ELSE 
						DATEADD(DAY, TKD_Kotei.Nittei - 1, TKD_Unkobi.HaiSYmd)	
				 END), 11), 4, 5) AS KoteiYmd
	FROM TKD_Kotei
	INNER JOIN TKD_Haisha AS JT_Haisha
		ON TKD_Kotei.UkeNo = JT_Haisha.UkeNo
		AND TKD_Kotei.UnkRen = JT_Haisha.UnkRen	
		AND TKD_Kotei.TeiDanNo = JT_Haisha.TeiDanNo	
	LEFT JOIN TKD_Unkobi
		ON TKD_Kotei.UkeNo = TKD_Unkobi.UkeNo
		AND TKD_Kotei.UnkRen = TKD_Unkobi.UnkRen

	WHERE TKD_Kotei.UkeNo = @UkeNo
		AND TKD_Kotei.UnkRen = @UnkRen
		AND JT_Haisha.SyaSyuRen = @SyaSyuRen
		AND TKD_Kotei.SiyoKbn = 1
	ORDER BY KoteiYmd, TKD_Kotei.KouRen, TKD_Kotei.TeiDanNo
	SET	@ROWCOUNT	=	@@ROWCOUNT
END

SELECT @rowCount1 = COUNT(*) FROM @tempTbl
WHILE @rowCount1 < 12
BEGIN
   INSERT INTO @tempTbl
   VALUES
   (
	   2,
       NULL, -- UkeNo - nchar
       NULL, -- Koutei - nvarchar
	   NULL -- KoteiYmd - char
   )
   SELECT @rowCount1 = COUNT(*) FROM @tempTbl
END

SELECT * FROM @tempTbl tt
ORDER BY OrderNumber, KoteiYmd