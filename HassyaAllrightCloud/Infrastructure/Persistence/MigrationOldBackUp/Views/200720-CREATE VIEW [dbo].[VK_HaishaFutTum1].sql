USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VK_HaishaFutTum1]    Script Date: 2020/07/20 14:50:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: GetTransportationSummary
-- SP-ID				: [VK_HaishaFutTum1]
-- DB-Name				: HOC_Kashikiri
-- Author				: 工房
-- Create date			: 2020/07/20 Sonzero
-- Description			: 車輛別付帯積込品コード別の付帯積込品データを取得する
--						  (付帯積込品明細データの存在しない場合は車輛台数で按分する)
-- =============================================
CREATE OR ALTER VIEW [dbo].[VK_HaishaFutTum1]
AS
	WITH FUTTUM01 AS(
	SELECT
		 Haisha.UkeNo
		,Haisha.Unkren
		,Haisha.TeiDanNo
		,Haisha.BunkRen
		,FutTum.FutTumKbn
		,FutTum.FutTumCdSeq
		,ISNULL(Futai.FutGuiKbn,6) AS FutGuiKbn
		,FutTum.FutTumNm
		,FutTum.Zeiritsu
		,FutTum.TesuRitu
		,FLOOR((FutTum.UriGakKin + FutTum.SyaRyoSyo ) / DaiSu.SumDaiSu) AS ZeiKomi
		,(FutTum.UriGakKin + FutTum.SyaRyoSyo) % DaiSu.SumDaiSu AS ZeiKomiHasu
		,FLOOR((FutTum.UriGakKin ) / DaiSu.SumDaiSu) AS UriGakKin
		,(FutTum.UriGakKin) % DaiSu.SumDaiSu AS UriGakKinHasu
		,FLOOR((FutTum.SyaRyoTes ) / DaiSu.SumDaiSu) AS TesuRyo
		,(FutTum.SyaRyoTes ) % DaiSu.SumDaiSu AS TesuHasu
		,DaiSu.SumDaiSu
		,DENSE_RANK() OVER (Partition By HAISHA.UkeNo,HAISHA.UnkRen
		    				Order By HAISHA.YouTblSeq,HAISHA.TeiDanNo,HAISHA.BunkRen) AS DenseRank
	FROM TKD_Haisha AS Haisha
	INNER JOIN
	(SELECT	 UkeNo
			,UnkRen
			,FutTumKbn
			,FutTumCdSeq
		    ,MAX(FutTumNm) AS FutTumNm
		    ,MAX(Zeiritsu) AS Zeiritsu
		    ,MAX(TesuRitu) AS TesuRitu
			,sum(UriGakKin - ISNULL(MFutTu.MUriGakKin,0)) As UriGakKin
			,sum(SyaRyoSyo - ISNULL(MFutTu.MSyaRyoSyo,0)) As SyaRyoSyo
			,sum(SyaRyoTes - ISNULL(MFutTu.MSyaRyoTes,0)) As SyaRyoTes
	FROM TKD_FutTum 
	LEFT JOIN
			(SELECT
				  UkeNo AS MUkeNo
				 ,UnkRen AS MUnkRen
				 ,FutTumKbn AS MFutTumKbn
				 ,FutTumRen AS MFutTumRen
				 ,SUM(UriGakKin) AS MUriGakKin
				 ,SUM(SyaRyoSyo) AS MSyaRyoSyo
				 ,SUM(SyaRyoTes) AS MSyaRyoTes
				 ,SUM(UriGakKin + SyaRyoSyo) AS MZeiKomiKin
			 FROM TKD_MFutTu
			 WHERE SiyoKbn = 1 AND Suryo > 0 
			 GROUP BY UkeNo,UnkRen,FutTumKbn,FutTumRen
			)AS MFutTu
			ON	MFutTu.MUkeNo = TKD_FutTum.UkeNo 
			AND MFutTu.MUnkRen = TKD_FutTum.UnkRen 
			AND MFutTu.MFutTumKbn = TKD_FutTum.FutTumKbn 
			AND MFutTu.MFutTumRen = TKD_FutTum.FutTumRen 
	WHERE TKD_FutTum.SiyoKbn = 1
	GROUP BY UkeNo,UnkRen,FutTumKbn,FutTumCdSeq
	)AS FutTum
	ON Haisha.UkeNo = FutTum.UkeNo
	AND Haisha.UnkRen = FutTum.UnkRen
	LEFT JOIN VPM_Futai AS Futai
	ON	FutTum.FutTumCdSeq = Futai.FutaiCdSeq
	LEFT JOIN
	(SELECT
	  Haisha.UkeNo
	 ,Haisha.UnkRen
	 ,COUNT(Haisha.UkeNo) AS SumDaiSu
	 FROM TKD_Haisha AS Haisha
	 LEFT JOIN TKD_Yyksho AS Yyksho
	 ON Haisha.UkeNo = Yyksho.UkeNo
	 WHERE Yyksho.YoyaSyu = 1
	 AND   Haisha.SiyoKbn = 1
	 GROUP BY Haisha.UkeNo,Haisha.UnkRen
	) DaiSu
	ON FutTum.UkeNo = DaiSu.UkeNo
	AND FutTum.UnkRen = DaiSu.UnkRen
	WHERE Haisha.SiyoKbn = 1
	)
	,FUTTUM02 AS(
	SELECT
	 FT01.UkeNo
	,FT01.Unkren
	,FT01.TeiDanNo
	,FT01.BunkRen
	,FT01.FutTumKbn
	,FT01.FutTumCdSeq
	,FT01.FutGuiKbn
	,FT01.FutTumNm
	,FT01.Zeiritsu
	,FT01.TesuRitu
	,FT01.ZeiKomi
	,CASE WHEN FT01.DenseRank <= FT01.ZeiKomiHasu THEN 1 ELSE 0 END AS ZeiKomiHasu
	,FT01.UriGakKin
	,CASE WHEN FT01.DenseRank <= FT01.UriGakKinHasu THEN 1 ELSE 0 END AS UriGakKinHasu
	,FT01.TesuRyo
	,CASE WHEN FT01.DenseRank <= FT01.TesuHasu THEN 1 ELSE 0 END AS TesuHasu
    FROM
	FUTTUM01 AS FT01
	)
	,FUTTUM03 AS(
	SELECT
	 FT02.UkeNo
	,FT02.Unkren
	,FT02.TeiDanNo
	,FT02.BunkRen
	,FT02.FutTumKbn
	,FT02.FutTumCdSeq
	,FT02.FutGuiKbn
	,MAX(FT02.FutTumNm) AS FutTumNm
	,MAX(FT02.TesuRitu) AS TesuRitu
	,SUM(FT02.UriGakKin + FT02.UriGakKinHasu) AS UriGakKin
	,SUM((FT02.ZeiKomi + FT02.ZeiKomiHasu) - (FT02.UriGakKin + FT02.UriGakKinHasu)) AS SyaRyoSyo
	,SUM((FT02.TesuRyo + FT02.TesuHasu)) AS SyaRyoTes
    FROM
	FUTTUM02 AS FT02
	GROUP BY
	FT02.UkeNo,FT02.UnkRen,FT02.TeiDanNo,FT02.BunkRen,FT02.FutTumKbn,FT02.FutTumCdSeq,FT02.FutGuiKbn
	)
	,MFUTTU01 AS(
	SELECT
	 MFutTu.UkeNo
	,MFutTu.UnkRen
	,MFutTu.TeiDanNo
	,MFutTu.BunkRen
	,MFutTu.FutTumKbn
	,SUM(MFutTu.UriGakKin) AS UriGakKin
	,SUM(MFutTu.SyaRyoSyo) AS SyaRyoSyo
	,SUM(MFutTu.SyaRyoTes) AS SyaRyoTes
	,FutTum.FutTumCdSeq
	FROM TKD_MFutTu AS MFutTu
	LEFT JOIN TKD_FutTum AS FutTum
	ON  MFutTu.UkeNo = FutTum.UkeNo
	AND MFutTu.UnkRen = FutTum.UnkRen
	AND MFutTu.FutTumKbn = FutTum.FutTumKbn 
	AND MFutTu.FutTumRen = FutTum.FutTumRen
	WHERE MFutTu.Suryo > 0
	AND   MFutTu.SiyoKbn = 1
	GROUP BY
	MFutTu.UkeNo,MFutTu.UnkRen,MFutTu.TeiDanNo,MFutTu.BunkRen,MFutTu.FutTumKbn,FutTum.FutTumCdSeq
	)

	SELECT
	   HAISHA.UkeNo
	  ,HAISHA.UnkRen
	  ,HAISHA.TeiDanNo
	  ,HAISHA.BunkRen
	  ,YYKSHO.SeiTaiYmd
	  ,HAISHA.SyukoYmd
	  ,HAISHA.HaiSYmd
	  ,HAISHA.TouYmd
	  ,HAISHA.KikYmd
	  ,HAISHA.YouTblSeq
	  ,HAISHA.SyuEigCdSeq
	  ,HAISHA.HaiSSryCdSeq
	  ,ISNULL(EIGYOS01.EigyoCdSeq,0) AS HaiSSryEigyoCdSeq
	  ,ISNULL(FUTTUM.FutTumKbn,0) AS FutTumKbn
	  ,ISNULL(FUTTUM.FutTumCdSeq,0) AS FutTumCdSeq
	  ,ISNULL(FUTTUM.FutTumNm,'') AS FutTumNm
	  ,ISNULL(FUTTUM.FutGuiKbn,0) AS FutGuiKbn
	  ,ISNULL(FUTTUM.UriGakKin,0)
	   + ISNULL(MFUTTU.UriGakKin,0) AS UriGakKin
	  ,ISNULL(FUTTUM.SyaRyoSyo,0)
	   + ISNULL(MFUTTU.SyaRyoSyo,0) AS SyaRyoSyo
	  ,ISNULL(FUTTUM.SyaRyoTes,0)
	   + ISNULL(MFUTTU.SyaRyoTes,0) AS SyaRyoTes
      ,ISNULL(FUTTUM.TesuRitu,0) AS TesuRitu 
	FROM
	TKD_Haisha AS HAISHA
	INNER JOIN FUTTUM03 AS FUTTUM
	ON  HAISHA.UkeNo = FUTTUM.UkeNo 
	AND HAISHA.UnkRen = FUTTUM.UnkRen
	AND HAISHA.TeiDanNo = FUTTUM.TeiDanNo
	AND HAISHA.BunkRen = FUTTUM.BunkRen
	LEFT JOIN MFUTTU01 AS MFUTTU
	ON  HAISHA.UkeNo = MFUTTU.UkeNo 
    AND HAISHA.UnkRen = MFUTTU.UnkRen
    AND HAISHA.TeiDanNo = MFUTTU.TeiDanNo
    AND HAISHA.BunkRen = MFUTTU.BunkRen
    AND FUTTUM.FutTumCdSeq = MFUTTU.FutTumCdSeq
	AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
    LEFT JOIN TKD_Yyksho AS YYKSHO
    ON  HAISHA.UkeNo = YYKSHO.UkeNo
    LEFT JOIN VPM_HenSya AS HENSYA01
    ON  HAISHA.HaiSSryCdSeq = HENSYA01.SyaRyoCdSeq
    AND YYKSHO.SeiTaiYmd BETWEEN HENSYA01.StaYmd AND HENSYA01.EndYmd
    LEFT JOIN VPM_Eigyos AS EIGYOS01
    ON  HENSYA01.EigyoCdSeq = EIGYOS01.EigyoCdSeq
    LEFT JOIN VPM_Tenant AS TENANT01
	ON YYKSHO.TenantCdSeq = TENANT01.TenantCdSeq
	AND TENANT01.SiyoKbn = 1
	WHERE
	YYKSHO.YoyaSyu = 1
	AND HAISHA.SiyoKbn = 1

GO