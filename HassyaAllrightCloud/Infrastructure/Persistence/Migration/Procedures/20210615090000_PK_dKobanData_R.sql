USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dKobanData_R]    Script Date: 03/22/2021 3:50:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dKobanData_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get koban data
-- Date			:   2021/03/22
-- Author		:   N.N.T.AN
-- Description	:   Get koban data with conditions
------------------------------------------------------------
CREATE OR ALTER   PROCEDURE [dbo].[PK_dKobanData_R]
		(
		--Parameter
			@TargetYmd		VARCHAR(50),
			@CompanyCd		INT,				--From Date
			@EigyoCdStr		INT,
			@EigyoCdEnd		INT,
			@SyainCdStr		VARCHAR(50) = '',
			@SyainCdEnd		VARCHAR(50) = '',
			@SyokumuCdStr	SMALLINT,
			@SyokumuCdEnd	SMALLINT,
			@SyuJun			VARCHAR(50)= '',
			@TenantCdSeq	INT,
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
		
		SELECT ISNULL(MainTable.SyainCdSeq, 0) AS SyainCdSeq
        ,ISNULL(VPM_Syain.SyainCd, '') AS SyainCd
        ,ISNULL(VPM_Syain.SyainNm, '') AS SyainNm
        ,ISNULL(VPM_Compny.CompanyCd, '') AS CompanyCd
        ,ISNULL(JM_SyainEigyos.EigyoCd, '') AS SyainEigyoCd
        ,ISNULL(JM_SyainEigyos.RyakuNm, '') AS SyainEigyoNm
        ,ISNULL(MainTable.SyainTenkoNo, '9999999999') AS SyainTenkoNo
        ,ISNULL(VPM_SyaRyo.SyaRyoCd, '') AS SyaRyoCd
        ,ISNULL(VPM_SyaSyu.SyaSyuCd, '') AS SyaSyuCd
        ,ISNULL(JM_SyaRyoEigyos.EigyoCd, '') AS SyaRyoEigyoCd
        ,ISNULL(JM_SyaRyoEigyos.RyakuNm, '') AS SyaRyoEigyoNm
        ,ISNULL(VPM_HenSya.TenkoNo, '9999999999') AS SyaRyoTenkoNo
        ,ISNULL(VPM_Syokum.SyokumuNm, '') AS SyokumuNm
        ,ISNULL(VPM_Syokum.SyokumuCd, '') AS SyokumuCd
        ,(CASE WHEN ISNUMERIC(MainTable.SyainTenkoNo) = 1 THEN (CASE WHEN CAST(MainTable.SyainTenkoNo AS DECIMAL) < 5000 THEN 2 ELSE 1 END) ELSE 1 END) AS GyomuKbn
        ,(CASE ISNULL(VPM_SyaSyu.SyaSyuCd, 0) WHEN 7 THEN '001' WHEN 11 THEN '002' WHEN 9 THEN '003' WHEN 6 THEN '004' WHEN 1 THEN '005' ELSE '999' END) AS SyaSyuSort
        --１日目
        ,ISNULL(JM_Day01KinKyu.KinKyuCd, '') AS Day01KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day01KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day01KinKyu.KinKyuNm, '') END) AS Day01KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day01KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day01SelectKbn
        --２日目
        ,ISNULL(JM_Day02KinKyu.KinKyuCd, '') AS Day02KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day02KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day02KinKyu.KinKyuNm, '') END) AS Day02KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day02KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day02SelectKbn
        --３日目
        ,ISNULL(JM_Day03KinKyu.KinKyuCd, '') AS Day03KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day03KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day03KinKyu.KinKyuNm, '') END) AS Day03KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day03KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day03SelectKbn
        --４日目
        ,ISNULL(JM_Day04KinKyu.KinKyuCd, '') AS Day04KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day04KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day04KinKyu.KinKyuNm, '') END) AS Day04KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day04KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day04SelectKbn
        --５日目
        ,ISNULL(JM_Day05KinKyu.KinKyuCd, '') AS Day05KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day05KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day05KinKyu.KinKyuNm, '') END) AS Day05KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day05KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day05SelectKbn
        --６日目
        ,ISNULL(JM_Day06KinKyu.KinKyuCd, '') AS Day06KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day06KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day06KinKyu.KinKyuNm, '') END) AS Day06KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day06KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day06SelectKbn
        --７日目
        ,ISNULL(JM_Day07KinKyu.KinKyuCd, '') AS Day07KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day07KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day07KinKyu.KinKyuNm, '') END) AS Day07KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day07KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day07SelectKbn
        --８日目
        ,ISNULL(JM_Day08KinKyu.KinKyuCd, '') AS Day08KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day08KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day08KinKyu.KinKyuNm, '') END) AS Day08KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day08KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day08SelectKbn
        --９日目
        ,ISNULL(JM_Day09KinKyu.KinKyuCd, '') AS Day09KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day09KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day09KinKyu.KinKyuNm, '') END) AS Day09KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day09KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day09SelectKbn
        --１０日目
        ,ISNULL(JM_Day10KinKyu.KinKyuCd, '') AS Day10KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day10KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day10KinKyu.KinKyuNm, '') END) AS Day10KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day10KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day10SelectKbn
        --１１日目
        ,ISNULL(JM_Day11KinKyu.KinKyuCd, '') AS Day11KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day11KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day11KinKyu.KinKyuNm, '') END) AS Day11KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day11KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day11SelectKbn
        --１２日目
        ,ISNULL(JM_Day12KinKyu.KinKyuCd, '') AS Day12KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day12KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day12KinKyu.KinKyuNm, '') END) AS Day12KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day12KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day12SelectKbn
        --１３日目
        ,ISNULL(JM_Day13KinKyu.KinKyuCd, '') AS Day13KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day13KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day13KinKyu.KinKyuNm, '') END) AS Day13KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day13KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day13SelectKbn
        --１４日目
        ,ISNULL(JM_Day14KinKyu.KinKyuCd, '') AS Day14KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day14KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day14KinKyu.KinKyuNm, '') END) AS Day14KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day14KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day14SelectKbn
        --１５日目
        ,ISNULL(JM_Day15KinKyu.KinKyuCd, '') AS Day15KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day15KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day15KinKyu.KinKyuNm, '') END) AS Day15KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day15KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day15SelectKbn
        --１６日目
        ,ISNULL(JM_Day16KinKyu.KinKyuCd, '') AS Day16KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day16KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day16KinKyu.KinKyuNm, '') END) AS Day16KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day16KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day16SelectKbn
        --１７日目
        ,ISNULL(JM_Day17KinKyu.KinKyuCd, '') AS Day17KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day17KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day17KinKyu.KinKyuNm, '') END) AS Day17KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day17KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day17SelectKbn
        --１８日目
        ,ISNULL(JM_Day18KinKyu.KinKyuCd, '') AS Day18KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day18KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day18KinKyu.KinKyuNm, '') END) AS Day18KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day18KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day18SelectKbn
        --１９日目
        ,ISNULL(JM_Day19KinKyu.KinKyuCd, '') AS Day19KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day19KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day19KinKyu.KinKyuNm, '') END) AS Day19KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day19KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day19SelectKbn
        --２０日目
        ,ISNULL(JM_Day20KinKyu.KinKyuCd, '') AS Day20KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day20KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day20KinKyu.KinKyuNm, '') END) AS Day20KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day20KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day20SelectKbn
        --２１日目
        ,ISNULL(JM_Day21KinKyu.KinKyuCd, '') AS Day21KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day21KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day21KinKyu.KinKyuNm, '') END) AS Day21KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day21KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day21SelectKbn
        --２２日目
        ,ISNULL(JM_Day22KinKyu.KinKyuCd, '') AS Day22KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day22KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day22KinKyu.KinKyuNm, '') END) AS Day22KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day22KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day22SelectKbn
        --２３日目
        ,ISNULL(JM_Day23KinKyu.KinKyuCd, '') AS Day23KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day23KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day23KinKyu.KinKyuNm, '') END) AS Day23KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day23KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day23SelectKbn
        --２４日目
        ,ISNULL(JM_Day24KinKyu.KinKyuCd, '') AS Day24KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day24KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day24KinKyu.KinKyuNm, '') END) AS Day24KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day24KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day24SelectKbn
        --２５日目
        ,ISNULL(JM_Day25KinKyu.KinKyuCd, '') AS Day25KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day25KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day25KinKyu.KinKyuNm, '') END) AS Day25KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day25KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day25SelectKbn
        --２６日目
        ,ISNULL(JM_Day26KinKyu.KinKyuCd, '') AS Day26KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day26KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day26KinKyu.KinKyuNm, '') END) AS Day26KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day26KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day26SelectKbn
        --２７日目
        ,ISNULL(JM_Day27KinKyu.KinKyuCd, '') AS Day27KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day27KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day27KinKyu.KinKyuNm, '') END) AS Day27KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day27KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day27SelectKbn
        --２８日目
        ,ISNULL(JM_Day28KinKyu.KinKyuCd, '') AS Day28KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day28KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day28KinKyu.KinKyuNm, '') END) AS Day28KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day28KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day28SelectKbn
        --２９日目
        ,ISNULL(JM_Day29KinKyu.KinKyuCd, '') AS Day29KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day29KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day29KinKyu.KinKyuNm, '') END) AS Day29KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day29KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day29SelectKbn
        --３０日目
        ,ISNULL(JM_Day30KinKyu.KinKyuCd, '') AS Day30KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day30KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day30KinKyu.KinKyuNm, '') END) AS Day30KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day30KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day30SelectKbn
        --３１日目
        ,ISNULL(JM_Day31KinKyu.KinKyuCd, '') AS Day31KinKyuCd
        ,(CASE WHEN ISNULL(MainTable.Day31KinKyuCnt, 0) > 1 THEN '＊＊' ELSE ISNULL(JM_Day31KinKyu.KinKyuNm, '') END) AS Day31KinKyuNm
        ,(CASE WHEN ISNULL(MainTable.Day31KadouCnt, 0) > 0 THEN 1 ELSE 0 END) AS Day31SelectKbn
        ,ISNULL(MainTable.Day01CalenKbn, 0) AS Day01CalenKbn
        ,ISNULL(MainTable.Day02CalenKbn, 0) AS Day02CalenKbn
        ,ISNULL(MainTable.Day03CalenKbn, 0) AS Day03CalenKbn
        ,ISNULL(MainTable.Day04CalenKbn, 0) AS Day04CalenKbn
        ,ISNULL(MainTable.Day05CalenKbn, 0) AS Day05CalenKbn
        ,ISNULL(MainTable.Day06CalenKbn, 0) AS Day06CalenKbn
        ,ISNULL(MainTable.Day07CalenKbn, 0) AS Day07CalenKbn
        ,ISNULL(MainTable.Day08CalenKbn, 0) AS Day08CalenKbn
        ,ISNULL(MainTable.Day09CalenKbn, 0) AS Day09CalenKbn
        ,ISNULL(MainTable.Day10CalenKbn, 0) AS Day10CalenKbn
        ,ISNULL(MainTable.Day11CalenKbn, 0) AS Day11CalenKbn
        ,ISNULL(MainTable.Day12CalenKbn, 0) AS Day12CalenKbn
        ,ISNULL(MainTable.Day13CalenKbn, 0) AS Day13CalenKbn
        ,ISNULL(MainTable.Day14CalenKbn, 0) AS Day14CalenKbn
        ,ISNULL(MainTable.Day15CalenKbn, 0) AS Day15CalenKbn
        ,ISNULL(MainTable.Day16CalenKbn, 0) AS Day16CalenKbn
        ,ISNULL(MainTable.Day17CalenKbn, 0) AS Day17CalenKbn
        ,ISNULL(MainTable.Day18CalenKbn, 0) AS Day18CalenKbn
        ,ISNULL(MainTable.Day19CalenKbn, 0) AS Day19CalenKbn
        ,ISNULL(MainTable.Day20CalenKbn, 0) AS Day20CalenKbn
        ,ISNULL(MainTable.Day21CalenKbn, 0) AS Day21CalenKbn
        ,ISNULL(MainTable.Day22CalenKbn, 0) AS Day22CalenKbn
        ,ISNULL(MainTable.Day23CalenKbn, 0) AS Day23CalenKbn
        ,ISNULL(MainTable.Day24CalenKbn, 0) AS Day24CalenKbn
        ,ISNULL(MainTable.Day25CalenKbn, 0) AS Day25CalenKbn
        ,ISNULL(MainTable.Day26CalenKbn, 0) AS Day26CalenKbn
        ,ISNULL(MainTable.Day27CalenKbn, 0) AS Day27CalenKbn
        ,ISNULL(MainTable.Day28CalenKbn, 0) AS Day28CalenKbn
        ,ISNULL(MainTable.Day29CalenKbn, 0) AS Day29CalenKbn
        ,ISNULL(MainTable.Day30CalenKbn, 0) AS Day30CalenKbn
        ,ISNULL(MainTable.Day31CalenKbn, 0) AS Day31CalenKbn
FROM (
        SELECT ISNULL(VPM_KyoSHe.SyainCdSeq, 0) AS SyainCdSeq
                ,MIN(ISNULL(VPM_KyoSHe.EigyoCdSeq, 0)) AS EigyoCdSeq
                ,(
                        SELECT (MIN(VPM_HenSya.SyaRyoCdSeq))
                        FROM VPM_SyaRyo
                        LEFT JOIN VPM_HenSya ON VPM_SyaRyo.SyaRyoCdSeq = VPM_HenSya.SyaRyoCdSeq
                                AND '' + @TargetYmd + '' BETWEEN VPM_HenSya.StaYmd AND VPM_HenSya.EndYmd
                        WHERE VPM_KyoSHe.SyainCdSeq = VPM_SyaRyo.SyainCdSeq
                        ) AS SyaRyoCdSeq
                ,MAX(ISNULL(VPM_KyoSHe.TenkoNo, '9999999999')) AS SyainTenkoNo
                --１日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + @TargetYmd + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day01KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + @TargetYmd + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day01KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + @TargetYmd + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day01KadouCnt
                --２日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 1, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day02KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 1, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day02KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 1, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day02KadouCnt
                --３日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 2, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day03KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 2, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day03KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 2, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day03KadouCnt
                --４日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 3, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day04KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 3, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day04KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 3, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day04KadouCnt
                --５日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 4, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day05KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 4, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day05KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 4, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day05KadouCnt
                --６日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 5, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day06KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 5, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day06KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 5, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day06KadouCnt
                --７日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 6, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day07KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 6, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day07KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 6, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day07KadouCnt
                --８日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 7, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day08KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 7, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day08KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 7, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day08KadouCnt
                --９日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 8, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day09KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 8, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day09KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 8, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day09KadouCnt
                --１０日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 9, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day10KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 9, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day10KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 9, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day10KadouCnt
                --１１日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 10, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day11KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 10, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day11KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 10, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day11KadouCnt
                --１２日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 11, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day12KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 11, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day12KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 11, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day12KadouCnt
                --１３日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 12, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day13KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 12, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day13KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 12, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day13KadouCnt
                --１４日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 13, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day14KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 13, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day14KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 13, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day14KadouCnt
                --１５日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 14, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day15KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 14, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day15KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 14, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day15KadouCnt
                --１６日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 15, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day16KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 15, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day16KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 15, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day16KadouCnt
                --１７日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 16, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day17KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 16, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day17KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 16, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day17KadouCnt
                --１８日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 17, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day18KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 17, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day18KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 17, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day18KadouCnt
                --１９日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 18, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day19KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 18, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day19KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 18, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day19KadouCnt
                --２０日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 19, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day20KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 19, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day20KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 19, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day20KadouCnt
                --２１日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 20, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day21KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 20, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day21KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 20, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day21KadouCnt
                --２２日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 21, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day22KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 21, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day22KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 21, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day22KadouCnt
                --２３日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 22, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day23KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 22, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day23KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 22, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day23KadouCnt
                --２４日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 23, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day24KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 23, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day24KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 23, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day24KadouCnt
                --２５日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 24, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day25KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 24, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day25KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 24, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day25KadouCnt
                --２６日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 25, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day26KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 25, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day26KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 25, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day26KadouCnt
                --２７日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 26, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day27KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 26, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day27KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 26, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day27KadouCnt
                --２８日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 27, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day28KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 27, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day28KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 27, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day28KadouCnt
                --２９日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 28, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day29KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 28, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day29KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 28, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day29KadouCnt
                --３０日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 29, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day30KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 29, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day30KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 29, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day30KadouCnt
                --３１日目
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 30, @TargetYmd), 112) + '' THEN ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) ELSE 0 END) AS Day31KinKyuCdSeq
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 30, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Kikyuj.KinKyuCdSeq, 0) > 0 THEN 1 ELSE 0 END) AS Day31KinKyuCnt
                ,SUM(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 30, @TargetYmd), 112) + ''
                                        AND ISNULL(TKD_Yyksho.UkeNo, '0') <> '0' THEN 1 ELSE 0 END) AS Day31KadouCnt
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + @TargetYmd + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day01CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 1, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day02CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 2, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day03CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 3, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day04CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 4, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day05CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 5, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day06CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 6, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day07CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 7, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day08CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 8, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day09CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 9, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day10CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 10, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day11CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 11, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day12CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 12, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day13CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 13, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day14CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 14, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day15CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 15, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day16CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 16, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day17CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 17, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day18CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 18, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day19CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 19, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day20CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 20, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day21CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 21, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day22CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 22, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day23CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 23, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day24CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 24, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day25CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 25, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day26CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 26, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day27CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 27, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day28CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 28, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day29CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 29, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day30CalenKbn
                ,MAX(CASE WHEN TKD_Koban.UnkYmd = '' + convert(VARCHAR, DATEADD(day, 30, @TargetYmd), 112) + '' THEN ISNULL(VPM_Calend.CalenKbn, 0) ELSE 0 END) AS Day31CalenKbn
        FROM VPM_KyoSHe
        LEFT JOIN TKD_Koban ON TKD_Koban.SyainCdSeq = VPM_KyoSHe.SyainCdSeq
                AND TKD_Koban.UnkYmd BETWEEN '' + @TargetYmd + '' AND '' + convert(VARCHAR, DATEADD(day, 30, @TargetYmd), 112) + ''
                AND TKD_Koban.SiyoKbn = 1
        LEFT JOIN TKD_Yyksho ON TKD_Yyksho.UkeNo = TKD_Koban.UkeNo
                AND TKD_Yyksho.Yoyasyu = 1
                AND TKD_Yyksho.SiyoKbn = 1
				AND TKD_Yyksho.TenantCdSeq = @TenantCdSeq
        LEFT JOIN TKD_Kikyuj ON TKD_Koban.KinKyuTblCdSeq = TKD_Kikyuj.KinKyuTblCdSeq
                AND TKD_Koban.UnkYmd BETWEEN TKD_Kikyuj.KinKyuSYmd AND TKD_Kikyuj.KinKyuEYmd
                AND TKD_Kikyuj.SiyoKbn = 1
        LEFT JOIN VPM_Eigyos ON VPM_KyoSHe.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
                AND VPM_Eigyos.SiyoKbn = 1
        LEFT JOIN VPM_Calend ON VPM_Eigyos.CompanyCdSeq = VPM_Calend.CompanyCdSeq
                AND TKD_Koban.UnkYmd = VPM_Calend.CalenYmd
                AND VPM_Calend.CalenSyu = 1
                AND VPM_Calend.CalenYmd BETWEEN '' + @TargetYmd + '' AND '' + convert(VARCHAR, DATEADD(day, 30, @TargetYmd), 112) + ''
        WHERE '' + @TargetYmd + '' BETWEEN VPM_KyoSHe.StaYmd AND VPM_KyoSHe.EndYmd
        GROUP BY VPM_KyoSHe.SyainCdSeq
        ) AS MainTable
LEFT JOIN VPM_Syain ON MainTable.SyainCdSeq = VPM_Syain.SyainCdSeq
LEFT JOIN VPM_KyoSHe ON MainTable.SyainCdSeq = VPM_KyoSHe.SyainCdSeq
        AND '' + @TargetYmd + '' BETWEEN VPM_KyoSHe.StaYmd AND VPM_KyoSHe.EndYmd
LEFT JOIN VPM_Syokum ON VPM_KyoSHe.SyokumuCdSeq = VPM_Syokum.SyokumuCdSeq
AND VPM_Syokum.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_Eigyos AS JM_SyainEigyos ON MainTable.EigyoCdSeq = JM_SyainEigyos.EigyoCdSeq
        AND JM_SyainEigyos.SiyoKbn = 1
LEFT JOIN VPM_Compny ON JM_SyainEigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
        AND VPM_Compny.SiyoKbn = 1
LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
        AND VPM_Compny.SiyoKbn = 1
LEFT JOIN VPM_HenSya ON MainTable.SyaRyoCdSeq = VPM_HenSya.SyaRyoCdSeq
        AND '' + @TargetYmd + '' BETWEEN VPM_HenSya.StaYmd AND VPM_HenSya.EndYmd
LEFT JOIN VPM_SyaRyo ON VPM_HenSya.SyaRyoCdSeq = VPM_SyaRyo.SyaRyoCdSeq
LEFT JOIN VPM_Eigyos AS JM_SyaRyoEigyos ON VPM_HenSya.EigyoCdSeq = JM_SyaRyoEigyos.EigyoCdSeq
        AND JM_SyaRyoEigyos.SiyoKbn = 1
LEFT JOIN VPM_SyaSyu ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
        AND VPM_SyaSyu.SiyoKbn = 1
		AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day01KinKyu ON MainTable.Day01KinKyuCdSeq = JM_Day01KinKyu.KinKyuCdSeq
        AND JM_Day01KinKyu.SiyoKbn = 1
        AND JM_Day01KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day02KinKyu ON MainTable.Day02KinKyuCdSeq = JM_Day02KinKyu.KinKyuCdSeq
        AND JM_Day02KinKyu.SiyoKbn = 1
        AND JM_Day02KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day03KinKyu ON MainTable.Day03KinKyuCdSeq = JM_Day03KinKyu.KinKyuCdSeq
        AND JM_Day03KinKyu.SiyoKbn = 1
        AND JM_Day03KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day04KinKyu ON MainTable.Day04KinKyuCdSeq = JM_Day04KinKyu.KinKyuCdSeq
        AND JM_Day04KinKyu.SiyoKbn = 1
        AND JM_Day04KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day05KinKyu ON MainTable.Day05KinKyuCdSeq = JM_Day05KinKyu.KinKyuCdSeq
        AND JM_Day05KinKyu.SiyoKbn = 1
        AND JM_Day05KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day06KinKyu ON MainTable.Day06KinKyuCdSeq = JM_Day06KinKyu.KinKyuCdSeq
        AND JM_Day06KinKyu.SiyoKbn = 1
        AND JM_Day06KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day07KinKyu ON MainTable.Day07KinKyuCdSeq = JM_Day07KinKyu.KinKyuCdSeq
        AND JM_Day07KinKyu.SiyoKbn = 1
        AND JM_Day07KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day08KinKyu ON MainTable.Day08KinKyuCdSeq = JM_Day08KinKyu.KinKyuCdSeq
        AND JM_Day08KinKyu.SiyoKbn = 1
        AND JM_Day08KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day09KinKyu ON MainTable.Day09KinKyuCdSeq = JM_Day09KinKyu.KinKyuCdSeq
        AND JM_Day09KinKyu.SiyoKbn = 1
        AND JM_Day09KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day10KinKyu ON MainTable.Day10KinKyuCdSeq = JM_Day10KinKyu.KinKyuCdSeq
        AND JM_Day10KinKyu.SiyoKbn = 1
        AND JM_Day10KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day11KinKyu ON MainTable.Day11KinKyuCdSeq = JM_Day11KinKyu.KinKyuCdSeq
        AND JM_Day11KinKyu.SiyoKbn = 1
        AND JM_Day11KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day12KinKyu ON MainTable.Day12KinKyuCdSeq = JM_Day12KinKyu.KinKyuCdSeq
        AND JM_Day12KinKyu.SiyoKbn = 1
        AND JM_Day12KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day13KinKyu ON MainTable.Day13KinKyuCdSeq = JM_Day13KinKyu.KinKyuCdSeq
        AND JM_Day13KinKyu.SiyoKbn = 1
        AND JM_Day13KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day14KinKyu ON MainTable.Day14KinKyuCdSeq = JM_Day14KinKyu.KinKyuCdSeq
        AND JM_Day14KinKyu.SiyoKbn = 1
        AND JM_Day14KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day15KinKyu ON MainTable.Day15KinKyuCdSeq = JM_Day15KinKyu.KinKyuCdSeq
        AND JM_Day15KinKyu.SiyoKbn = 1
        AND JM_Day15KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day16KinKyu ON MainTable.Day16KinKyuCdSeq = JM_Day16KinKyu.KinKyuCdSeq
        AND JM_Day16KinKyu.SiyoKbn = 1
        AND JM_Day16KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day17KinKyu ON MainTable.Day17KinKyuCdSeq = JM_Day17KinKyu.KinKyuCdSeq
        AND JM_Day17KinKyu.SiyoKbn = 1
        AND JM_Day17KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day18KinKyu ON MainTable.Day18KinKyuCdSeq = JM_Day18KinKyu.KinKyuCdSeq
        AND JM_Day18KinKyu.SiyoKbn = 1
        AND JM_Day18KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day19KinKyu ON MainTable.Day19KinKyuCdSeq = JM_Day19KinKyu.KinKyuCdSeq
        AND JM_Day19KinKyu.SiyoKbn = 1
        AND JM_Day19KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day20KinKyu ON MainTable.Day20KinKyuCdSeq = JM_Day20KinKyu.KinKyuCdSeq
        AND JM_Day20KinKyu.SiyoKbn = 1
        AND JM_Day20KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day21KinKyu ON MainTable.Day21KinKyuCdSeq = JM_Day21KinKyu.KinKyuCdSeq
        AND JM_Day21KinKyu.SiyoKbn = 1
        AND JM_Day21KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day22KinKyu ON MainTable.Day22KinKyuCdSeq = JM_Day22KinKyu.KinKyuCdSeq
        AND JM_Day22KinKyu.SiyoKbn = 1
        AND JM_Day22KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day23KinKyu ON MainTable.Day23KinKyuCdSeq = JM_Day23KinKyu.KinKyuCdSeq
        AND JM_Day23KinKyu.SiyoKbn = 1
        AND JM_Day23KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day24KinKyu ON MainTable.Day24KinKyuCdSeq = JM_Day24KinKyu.KinKyuCdSeq
        AND JM_Day24KinKyu.SiyoKbn = 1
        AND JM_Day24KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day25KinKyu ON MainTable.Day25KinKyuCdSeq = JM_Day25KinKyu.KinKyuCdSeq
        AND JM_Day25KinKyu.SiyoKbn = 1
        AND JM_Day25KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day26KinKyu ON MainTable.Day26KinKyuCdSeq = JM_Day26KinKyu.KinKyuCdSeq
        AND JM_Day26KinKyu.SiyoKbn = 1
        AND JM_Day26KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day27KinKyu ON MainTable.Day27KinKyuCdSeq = JM_Day27KinKyu.KinKyuCdSeq
        AND JM_Day27KinKyu.SiyoKbn = 1
        AND JM_Day27KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day28KinKyu ON MainTable.Day28KinKyuCdSeq = JM_Day28KinKyu.KinKyuCdSeq
        AND JM_Day28KinKyu.SiyoKbn = 1
        AND JM_Day28KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day29KinKyu ON MainTable.Day29KinKyuCdSeq = JM_Day29KinKyu.KinKyuCdSeq
        AND JM_Day29KinKyu.SiyoKbn = 1
        AND JM_Day29KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day30KinKyu ON MainTable.Day30KinKyuCdSeq = JM_Day30KinKyu.KinKyuCdSeq
        AND JM_Day30KinKyu.SiyoKbn = 1
        AND JM_Day30KinKyu.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_KinKyu AS JM_Day31KinKyu ON MainTable.Day31KinKyuCdSeq = JM_Day31KinKyu.KinKyuCdSeq
        AND JM_Day31KinKyu.SiyoKbn = 1
        AND JM_Day31KinKyu.TenantCdSeq = @TenantCdSeq
WHERE VPM_Tenant.TenantCdSeq = @TenantCdSeq
        AND (@CompanyCd = 0 OR VPM_Compny.CompanyCd = @CompanyCd) -- IF @CompanyCd <> '' : Add this condition
        AND (@EigyoCdStr = 0 OR JM_SyainEigyos.EigyoCd >= @EigyoCdStr) -- IF @EigCdStr <> '' : Add this condition
        AND (@EigyoCdEnd = 0 OR JM_SyainEigyos.EigyoCd <= @EigyoCdEnd) -- IF @EigCdEnd <> '' : Add this condition
        AND (@SyainCdStr = '' OR VPM_Syain.SyainCd >= @SyainCdStr) -- IF @KinSyainCdStr <> '' : Add this condition
        AND (@SyainCdEnd = '' OR VPM_Syain.SyainCd <= @SyainCdEnd) -- IF @KinSyainCdEnd <> '' : Add this condition
        AND (@SyokumuCdStr = 0 OR VPM_Syokum.SyokumuCd >= @SyokumuCdStr) -- IF @SyokumuCdStr <> '' : Add this condition
        AND (@SyokumuCdEnd = 0 OR VPM_Syokum.SyokumuCd <= @SyokumuCdEnd) -- IF @SyokumuCdEnd <> '' : Add this condition

ORDER BY 
CASE WHEN @SyuJun = '車輌点呼順' THEN ISNULL(VPM_HenSya.TenkoNo, '9999999999') END,
CASE WHEN @SyuJun = '車輌点呼順' THEN ISNULL(JM_SyaRyoEigyos.EigyoCd, '') END,
CASE WHEN @SyuJun = '車輌点呼順' THEN ISNULL(VPM_SyaRyo.SyaRyoCd, '') END,
CASE WHEN @SyuJun = '車輌点呼順' THEN ISNULL(JM_SyainEigyos.EigyoCd, '') END,
CASE WHEN @SyuJun = '車輌点呼順' THEN ISNULL(VPM_Syain.SyainCd, '') END,

CASE WHEN @SyuJun = '社員点呼順' THEN ISNULL(MainTable.SyainTenkoNo, '9999999999') END,
CASE WHEN @SyuJun = '社員点呼順' THEN ISNULL(JM_SyainEigyos.EigyoCd, '') END,
CASE WHEN @SyuJun = '社員点呼順' THEN ISNULL(VPM_Syain.SyainCd, '') END,

CASE WHEN @SyuJun = '社員コード順' THEN ISNULL(VPM_Syain.SyainCd, '') END,

CASE WHEN @SyuJun = '営業所コード順' THEN ISNULL(JM_SyainEigyos.EigyoCd, '') END,
CASE WHEN @SyuJun = '営業所コード順' THEN ISNULL(VPM_Syain.SyainCd, '') END

SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN