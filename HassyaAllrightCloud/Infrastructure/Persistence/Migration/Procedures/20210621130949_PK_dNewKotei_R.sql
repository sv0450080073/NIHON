USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dNewKotei_R]    Script Date: 2021/06/21 11:22:24 ******/
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
CREATE OR ALTER   PROCEDURE [dbo].[PK_dNewKotei_R]
--パラメータ

	(
		@UkeNo				CHAR(15)					-- 受付番号
	,	@UnkRen				VARCHAR(3)					-- 運行連番
	,   @TeiDanNo			INTEGER						-- 梯団番号
	,	@OutputUnit			VARCHAR(1)					-- 出力単位(1:予約毎 2:予約車種毎)	
	,	@ROWCOUNT			INTEGER			OUTPUT		-- 処理件数
	)
AS

DECLARE @tempTbl TABLE(
	OrderNumber int,
	UkeNo nchar(15),
	Koutei nvarchar(102),
	KoteiYmd nchar(20),
	Nittei int,
	UnkRen int,
	NmTel nchar(150)
)
DECLARE	@rowCount1 int

IF @OutputUnit = 1 OR (SELECT COUNT(*) FROM TKD_Kotei 
			INNER JOIN TKD_Haisha AS KT_Haisha
			ON TKD_Kotei.UkeNo = KT_Haisha.UkeNo
			AND TKD_Kotei.UnkRen = KT_Haisha.UnkRen	
			AND TKD_Kotei.TeiDanNo = KT_Haisha.TeiDanNo	
			WHERE TKD_Kotei.UkeNo = @UkeNo
				AND TKD_Kotei.UnkRen = @UnkRen
				AND KT_Haisha.TeiDanNo = @TeiDanNo
			) = 0
BEGIN
	WITH Yoyaku AS (SELECT 1 AS Number, TKD_Kotei.UkeNo, 
					(case when row_number() over (partition by TKD_Kotei.Koutei order by (select NULL)) = 1
						 then TKD_Kotei.Koutei
					end) AS Koutei,
					(case when row_number() over (partition by 
						(SUBSTRING(CONVERT(varchar, (CASE 
						WHEN TKD_Kotei.TomKbn = 2
							THEN DATEADD(DAY, -1, TKD_Unkobi.HaiSYmd)
						WHEN TKD_Kotei.TomKbn = 3
							THEN DATEADD(DAY, 1, TKD_Unkobi.TouYmd)
						ELSE 
							DATEADD(DAY, TKD_Kotei.Nittei - 1, TKD_Unkobi.HaiSYmd)	
						END), 11), 4, 5)) order by (select NULL)) = 1
					then 
						(SUBSTRING(CONVERT(varchar, (CASE 
						WHEN TKD_Kotei.TomKbn = 2
							THEN DATEADD(DAY, -1, TKD_Unkobi.HaiSYmd)
						WHEN TKD_Kotei.TomKbn = 3
							THEN DATEADD(DAY, 1, TKD_Unkobi.TouYmd)
						ELSE 
							DATEADD(DAY, TKD_Kotei.Nittei - 1, TKD_Unkobi.HaiSYmd)	
						END), 11), 4, 5))
					end) AS KoteiYmd,
				  TKD_Tehai.Nittei, TKD_Tehai.UnkRen, NmTel
	FROM TKD_Kotei
	LEFT JOIN TKD_Unkobi
		ON TKD_Kotei.UkeNo = TKD_Unkobi.UkeNo
		AND TKD_Kotei.UnkRen = TKD_Unkobi.UnkRen
	LEFT JOIN TKD_Tehai
		ON TKD_Tehai.Nittei = TKD_Kotei.Nittei
        AND TKD_Tehai.UkeNo = TKD_Kotei.UkeNo
        AND TKD_Tehai.UnkRen = TKD_Kotei.UnkRen
	CROSS APPLY 
	(
		VALUES
			(TKD_Tehai.TehNm),
			(TKD_Tehai.TehTel)
	) TehNmTel (NmTel)
	WHERE TKD_Kotei.UkeNo = @UkeNo
		AND TKD_Kotei.UnkRen = @UnkRen
		AND TKD_Kotei.TeiDanNo = 0
		AND (TKD_Tehai.TeiDanNo IS NULL OR TKD_Tehai.TeiDanNo = 0)
		AND TKD_Kotei.SiyoKbn = 1
	--ORDER BY TKD_Kotei.KouRen
	)

	INSERT INTO	@tempTbl
	SELECT * FROM Yoyaku
	WHERE ((Koutei IS NOT NULL) OR (NmTel IS NOT NULL))
END
ELSE
BEGIN
	
	WITH Syasyu AS (SELECT 1 AS Number, TKD_Kotei.UkeNo, 
					(case when row_number() over (partition by TKD_Kotei.Koutei order by (select NULL)) = 1
						 then TKD_Kotei.Koutei
					end) AS Koutei, 
					(case when row_number() over (partition by 
						(SUBSTRING(CONVERT(varchar, (CASE 
						WHEN TKD_Kotei.TomKbn = 2
							THEN DATEADD(DAY, -1, TKD_Unkobi.HaiSYmd)
						WHEN TKD_Kotei.TomKbn = 3
							THEN DATEADD(DAY, 1, TKD_Unkobi.TouYmd)
						ELSE 
							DATEADD(DAY, TKD_Kotei.Nittei - 1, TKD_Unkobi.HaiSYmd)	
						END), 11), 4, 5)) order by (select NULL)) = 1
					then 
						(SUBSTRING(CONVERT(varchar, (CASE 
						WHEN TKD_Kotei.TomKbn = 2
							THEN DATEADD(DAY, -1, TKD_Unkobi.HaiSYmd)
						WHEN TKD_Kotei.TomKbn = 3
							THEN DATEADD(DAY, 1, TKD_Unkobi.TouYmd)
						ELSE 
							DATEADD(DAY, TKD_Kotei.Nittei - 1, TKD_Unkobi.HaiSYmd)	
						END), 11), 4, 5))
					end) AS KoteiYmd,
				 TKD_Tehai.Nittei, TKD_Tehai.UnkRen, NmTel
	FROM TKD_Kotei
	INNER JOIN TKD_Haisha AS JT_Haisha
		ON TKD_Kotei.UkeNo = JT_Haisha.UkeNo
		AND TKD_Kotei.UnkRen = JT_Haisha.UnkRen	
		AND TKD_Kotei.TeiDanNo = JT_Haisha.TeiDanNo	
	LEFT JOIN TKD_Unkobi
		ON TKD_Kotei.UkeNo = TKD_Unkobi.UkeNo
		AND TKD_Kotei.UnkRen = TKD_Unkobi.UnkRen
	LEFT JOIN TKD_Tehai
		ON TKD_Tehai.Nittei = TKD_Kotei.Nittei
        AND TKD_Tehai.UkeNo = TKD_Kotei.UkeNo
        AND TKD_Tehai.UnkRen = TKD_Kotei.UnkRen
		AND TKD_Tehai.TeiDanNo = TKD_Kotei.TeiDanNo
	CROSS APPLY 
	(
		VALUES
			(TKD_Tehai.TehNm),
			(TKD_Tehai.TehTel)
	) TehNmTel (NmTel)
	WHERE TKD_Kotei.UkeNo = @UkeNo
		AND TKD_Kotei.UnkRen = @UnkRen
		AND JT_Haisha.TeiDanNo = @TeiDanNo
		AND TKD_Kotei.TeiDanNo = @TeiDanNo
		--AND (TKD_Tehai.TeiDanNo IS NULL OR TKD_Tehai.TeiDanNo = @TeiDanNo)
		AND TKD_Kotei.SiyoKbn = 1
	--ORDER BY TKD_Kotei.KouRen, TKD_Tehai.TehRen
	)

	INSERT INTO	@tempTbl
	SELECT * FROM Syasyu
	WHERE ((Koutei IS NOT NULL) OR (NmTel IS NOT NULL))
END

SELECT @rowCount1 = COUNT(*) FROM @tempTbl
WHILE ((@rowCount1 % 12 > 0) OR (@rowCount1 = 0 OR @rowCount1 < 12))
BEGIN
   INSERT INTO @tempTbl
   VALUES
   (
	   2,
       NULL, -- UkeNo - nchar
       NULL, -- Koutei - nvarchar
	   NULL, -- KoteiYmd - char
	   NULL, -- Nittei - int
	   NULL,  -- UnkRen - int
	   NULL  -- NmTel - nchar
   )
   SELECT @rowCount1 = COUNT(*) FROM @tempTbl
END

SELECT * FROM @tempTbl tt

ORDER BY OrderNumber
