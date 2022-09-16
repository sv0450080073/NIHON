USE [shinhassha_edited]
GO

/****** Object:  View [dbo].[VP_Haisha1]    Script Date: 2020/08/04 16:31:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VP_Haisha1]
AS
 SELECT
   TKD_Haisha.UkeNo
 , TKD_Haisha.UnkRen
 , TKD_Haisha.TeiDanNo
 , TKD_Haisha.BunkRen
 , TOP1_Haiin.SyainCdSeq AS SyainCdSeq1
 , TOP1_Haiin.HaiInRen AS HaiInRen1
 , TOP2_Haiin.SyainCdSeq AS SyainCdSeq2
 , TOP2_Haiin.HaiInRen AS HaiInRen2
 , TOP3_Haiin.SyainCdSeq AS SyainCdSeq3
 , TOP3_Haiin.HaiInRen AS HaiInRen3
 , TOP4_Haiin.SyainCdSeq AS SyainCdSeq4
 , TOP4_Haiin.HaiInRen AS HaiInRen4
 , TOP5_Haiin.SyainCdSeq AS SyainCdSeq5
 , TOP5_Haiin.HaiInRen AS HaiInRen5
 FROM	TKD_Haisha
	LEFT JOIN
		(
		 SELECT
		   MT_Haiin.UkeNo
		 , MT_Haiin.UnkRen
		 , MT_Haiin.TeiDanNo
		 , MT_Haiin.BunkRen
		 , MT_Haiin.HaiInRen
		 , MT_Haiin.SyainCdSeq
		 FROM
			(
			 SELECT
			   TKD_Haiin.UkeNo
			 , TKD_Haiin.UnkRen
			 , TKD_Haiin.TeiDanNo
			 , TKD_Haiin.BunkRen
 			 , TKD_Haiin.HaiInRen
			 , RANK() OVER
 				(
 				 PARTITION BY
 				   TKD_Haiin.UkeNo
 				 , TKD_Haiin.UnkRen
 				 , TKD_Haiin.TeiDanNo
 				 , TKD_Haiin.BunkRen
 				 ORDER BY
 				   TKD_Haiin.HaiInRen ASC
 				) AS RANK_Haiin
			 , TKD_Haiin.SyainCdSeq
			 FROM	TKD_Haiin
			 WHERE TKD_Haiin.SiyoKbn = 1	
			 ) AS MT_Haiin
		 WHERE MT_Haiin.RANK_Haiin = 1
		 ) AS TOP1_Haiin
	ON	TKD_Haisha.UkeNo = TOP1_Haiin.UkeNo
	AND	TKD_Haisha.UnkRen = TOP1_Haiin.UnkRen
	AND	TKD_Haisha.TeiDanNo = TOP1_Haiin.TeiDanNo
	AND	TKD_Haisha.BunkRen = TOP1_Haiin.BunkRen
	LEFT JOIN
		(
		 SELECT
		   MT_Haiin.UkeNo
		 , MT_Haiin.UnkRen
		 , MT_Haiin.TeiDanNo
		 , MT_Haiin.BunkRen
		 , MT_Haiin.HaiInRen
		 , MT_Haiin.SyainCdSeq
		 FROM
			(
			 SELECT
			   TKD_Haiin.UkeNo
			 , TKD_Haiin.UnkRen
			 , TKD_Haiin.TeiDanNo
			 , TKD_Haiin.BunkRen
 			 , TKD_Haiin.HaiInRen
			 , RANK() OVER
 				(
 				 PARTITION BY
 				   TKD_Haiin.UkeNo
 				 , TKD_Haiin.UnkRen
 				 , TKD_Haiin.TeiDanNo
 				 , TKD_Haiin.BunkRen
 				 ORDER BY
 				   TKD_Haiin.HaiInRen ASC
 				) AS RANK_Haiin
			 , TKD_Haiin.SyainCdSeq
			 FROM	TKD_Haiin
			 WHERE TKD_Haiin.SiyoKbn = 1	
			 ) AS MT_Haiin
		 WHERE MT_Haiin.RANK_Haiin = 2
		 ) AS TOP2_Haiin
	ON	TKD_Haisha.UkeNo = TOP2_Haiin.UkeNo
	AND	TKD_Haisha.UnkRen = TOP2_Haiin.UnkRen
	AND	TKD_Haisha.TeiDanNo = TOP2_Haiin.TeiDanNo
	AND	TKD_Haisha.BunkRen = TOP2_Haiin.BunkRen
	LEFT JOIN
		(
		 SELECT
		   MT_Haiin.UkeNo
		 , MT_Haiin.UnkRen
		 , MT_Haiin.TeiDanNo
		 , MT_Haiin.BunkRen
		 , MT_Haiin.HaiInRen
		 , MT_Haiin.SyainCdSeq
		 FROM
			(
			 SELECT
			   TKD_Haiin.UkeNo
			 , TKD_Haiin.UnkRen
			 , TKD_Haiin.TeiDanNo
			 , TKD_Haiin.BunkRen
 			 , TKD_Haiin.HaiInRen
			 , RANK() OVER
 				(
 				 PARTITION BY
 				   TKD_Haiin.UkeNo
 				 , TKD_Haiin.UnkRen
 				 , TKD_Haiin.TeiDanNo
 				 , TKD_Haiin.BunkRen
 				 ORDER BY
 				   TKD_Haiin.HaiInRen ASC
 				) AS RANK_Haiin
			 , TKD_Haiin.SyainCdSeq
			 FROM	TKD_Haiin
			 WHERE TKD_Haiin.SiyoKbn = 1	
			 ) AS MT_Haiin
		 WHERE MT_Haiin.RANK_Haiin = 3
		 ) AS TOP3_Haiin
	ON	TKD_Haisha.UkeNo = TOP3_Haiin.UkeNo
	AND	TKD_Haisha.UnkRen = TOP3_Haiin.UnkRen
	AND	TKD_Haisha.TeiDanNo = TOP3_Haiin.TeiDanNo
	AND	TKD_Haisha.BunkRen = TOP3_Haiin.BunkRen
	LEFT JOIN
		(
		 SELECT
		   MT_Haiin.UkeNo
		 , MT_Haiin.UnkRen
		 , MT_Haiin.TeiDanNo
		 , MT_Haiin.BunkRen
		 , MT_Haiin.HaiInRen
		 , MT_Haiin.SyainCdSeq
		 FROM
			(
			 SELECT
			   TKD_Haiin.UkeNo
			 , TKD_Haiin.UnkRen
			 , TKD_Haiin.TeiDanNo
			 , TKD_Haiin.BunkRen
 			 , TKD_Haiin.HaiInRen
			 , RANK() OVER
 				(
 				 PARTITION BY
 				   TKD_Haiin.UkeNo
 				 , TKD_Haiin.UnkRen
 				 , TKD_Haiin.TeiDanNo
 				 , TKD_Haiin.BunkRen
 				 ORDER BY
 				   TKD_Haiin.HaiInRen ASC
 				) AS RANK_Haiin
			 , TKD_Haiin.SyainCdSeq
			 FROM	TKD_Haiin
			 WHERE TKD_Haiin.SiyoKbn = 1	
			 ) AS MT_Haiin
		 WHERE MT_Haiin.RANK_Haiin = 4
		 ) AS TOP4_Haiin
	ON	TKD_Haisha.UkeNo = TOP4_Haiin.UkeNo
	AND	TKD_Haisha.UnkRen = TOP4_Haiin.UnkRen
	AND	TKD_Haisha.TeiDanNo = TOP4_Haiin.TeiDanNo
	AND	TKD_Haisha.BunkRen = TOP4_Haiin.BunkRen
	LEFT JOIN
		(
		 SELECT
		   MT_Haiin.UkeNo
		 , MT_Haiin.UnkRen
		 , MT_Haiin.TeiDanNo
		 , MT_Haiin.BunkRen
		 , MT_Haiin.HaiInRen
		 , MT_Haiin.SyainCdSeq
		 FROM
			(
			 SELECT
			   TKD_Haiin.UkeNo
			 , TKD_Haiin.UnkRen
			 , TKD_Haiin.TeiDanNo
			 , TKD_Haiin.BunkRen
 			 , TKD_Haiin.HaiInRen
			 , RANK() OVER
 				(
 				 PARTITION BY
 				   TKD_Haiin.UkeNo
 				 , TKD_Haiin.UnkRen
 				 , TKD_Haiin.TeiDanNo
 				 , TKD_Haiin.BunkRen
 				 ORDER BY
 				   TKD_Haiin.HaiInRen ASC
 				) AS RANK_Haiin
			 , TKD_Haiin.SyainCdSeq
			 FROM	TKD_Haiin
			 WHERE TKD_Haiin.SiyoKbn = 1	
			 ) AS MT_Haiin
		 WHERE MT_Haiin.RANK_Haiin = 5
		 ) AS TOP5_Haiin
	ON	TKD_Haisha.UkeNo = TOP5_Haiin.UkeNo
	AND	TKD_Haisha.UnkRen = TOP5_Haiin.UnkRen
	AND	TKD_Haisha.TeiDanNo = TOP5_Haiin.TeiDanNo
	AND	TKD_Haisha.BunkRen = TOP5_Haiin.BunkRen
 WHERE	TOP1_Haiin.HaiInRen IS NOT NULL

GO

