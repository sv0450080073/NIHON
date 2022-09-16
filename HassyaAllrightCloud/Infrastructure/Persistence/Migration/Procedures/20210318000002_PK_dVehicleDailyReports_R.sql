USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dVehicleDailyReports_R]    Script Date: 3/18/2021 9:39:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAllrightCloud
-- Module-Name	:   HassyaAllrightCloud
-- SP-ID		:   PK_dVehicleDailyReports_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get vehicle daily report list
-- Date			:   2020/08/05
-- Author		:   P.M.NHAT
-- Description	:   Get vehicle daily report list with conditions
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[PK_dVehicleDailyReports_R]
(
	-- Parameter
		@NipoYmdStr			char(8)		-- 日程年月日（開始）
	,	@NipoYmdEnd			char(8)		-- 日程年月日（終了）
	,	@EigyoCompnyCd		int			-- 会社コード
	,	@SyaEigyoCdStr		int			-- 車輛営業所（開始）
	,	@SyaEigyoCdEnd		int			-- 車輛営業所（終了）
	,	@SyaRyoCdStr		int			-- 車輛（開始）
	,	@SyaRyoCdEnd		int			-- 車輛（終了）
	,	@UkeNoStr			nchar(15)	-- 受付番号（開始）
	,	@UkeNoEnd			nchar(15)	-- 受付番号（終了）
	,	@YoyaKbnStr			nvarchar(4)	-- 予約区分（開始）
	,	@YoyaKbnEnd			nvarchar(4)	-- 予約区分（終了）
	,	@SyaRyoCdSeq		int
	,	@YmdStr				char(8)		--	
	,	@YmdEnd				char(8)		--
	,	@TenantCdSeq		int
)
AS
	BEGIN
		IF OBJECT_ID(N'tempdb..#Temp') IS NOT NULL
		BEGIN
		DROP TABLE #Temp
		END

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
		INTO #Temp
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

		SELECT *																																			
		FROM																																			
		  (SELECT TOP 1001
				TKD_Shabni.UnkYmd AS UnkYmd ,																																			
				TKD_Shabni.TeiDanNo AS TeiDanNo ,																																			
				TKD_Shabni.BunkRen AS BunkRen																																	
				,ISNULL(TKD_Haisha.UkeNo, 0) AS UkeNo						
				,ISNULL(TKD_Haisha.UnkRen, 0) AS UnkRen						
				,ISNULL(JT_Yyksho.UkeYmd, '') AS UkeYmd						
				,ISNULL(JT_Unkobi.DanTaNm, '') AS DanTaNm						
				,ISNULL(TKD_Haisha.DanTaNm2, '') AS DanTaNm2						
				,ISNULL(TKD_Haisha.IkNm, '') AS IkNm						
				,ISNULL(JM_Gyosya.GyosyaCd, 0) AS GyosyaCd						
				,ISNULL(JM_Tokisk.TokuiCd, 0) AS TokuiCd						
				,ISNULL(JM_TokiSt.SitenCd, 0) AS SitenCd						
				,ISNULL(JM_Gyosya.GyosyaNm, '') AS GyosyaNm						
				,ISNULL(JM_Tokisk.TokuiNm, '') AS TokuiNm						
				,ISNULL(JM_TokiSt.SitenNm, '') AS SitenNm						
				,ISNULL(JM_Tokisk.RyakuNm, '') AS TokuiRyakuNm						
				,ISNULL(JM_TokiSt.RyakuNm, '') AS SitenRyakuNm						
				,ISNULL(JM_YoyKbn.YoyaKbn, 0) AS YoyaKbn						
				,ISNULL(JM_YoyKbn.YoyaKbnNm, '') AS YoyaKbnNm						
				,ISNULL(JM_SyaRyo.SyaRyoCdSeq, 0) AS SyaRyoCdSeq						
				,ISNULL(JM_SyaRyo.SyaRyoCd, 0) AS SyaRyoCd						
				,ISNULL(JM_SyaRyo.SyaryoNm, '') AS SyaryoNm						
				,ISNULL(JM_SyaRyo.KariSyaRyoNm, '') AS KariSyaRyoNm						
				,ISNULL(JM_SyaSyu.SyaSyuCd, '') AS SyaSyuCd						
				,ISNULL(JM_SyaSyu.SyaSyuNm, '') AS SyaSyuNm						
				,ISNULL(JM_SyaSyu.KataKbn, '') AS KataKbn						
				,ISNULL(JM_KataKbn.RyakuNm, '') AS KataKbnRyakuNm						
				,ISNULL(JM_NinkaKbn.RyakuNm, '') AS NinkaKbnRyakuNm						
				,ISNULL(TKD_Haisha.HaiSYmd, '') AS HaiSYmd						
				,ISNULL(TKD_Haisha.TouYmd, '') AS TouYmd						
				,ISNULL(TKD_Haisha.SyukoYmd, '') AS SyukoYmd						
				,ISNULL(TKD_Haisha.SyukoTime, '') AS Haisha_SyukoTime						
				,ISNULL(TKD_Shabni.SyukoTime, '0000') AS Shabni_SyukoTime						
				,ISNULL(TKD_Haisha.KikYmd, '') AS KikYmd						
				,ISNULL(TKD_Haisha.KikTime, '') AS Haisha_KikTime						
				,ISNULL(TKD_Shabni.KikTime, '0000') AS Shabni_KikTime						
				,ISNULL(TKD_Shabni.StMeter, 0.00) AS StMeter						
				,ISNULL(TKD_Shabni.EndMeter, 0.00) AS EndMeter						
				,ISNULL(TKD_Shabni.JisaIPKm, 0.00) AS JisaIPKm						
				,ISNULL(TKD_Shabni.JisaKSKm, 0.00) AS JisaKSKm						
				,ISNULL(TKD_Shabni.KisoIPKm, 0.00) AS KisoIPKm						
				,ISNULL(TKD_Shabni.KisoKOKm, 0.00) AS KisoKOKm						
				,ISNULL(TKD_Shabni.OthKm, 0.00) AS OthKm						
				,ISNULL(JM_Nenryo1.CodeKbn, 0) AS NenryoCd1						
				,ISNULL(JM_Nenryo1.CodeKbnNm, '') AS NenryoNm1						
				,ISNULL(JM_Nenryo1.RyakuNm, '') AS NenryoRyakuNm1						
				,ISNULL(TKD_Shabni.Nenryo1, 0.00) AS Nenryo1						
				,ISNULL(JM_Nenryo2.CodeKbn, 0) AS NenryoCd2						
				,ISNULL(JM_Nenryo2.CodeKbnNm, '') AS NenryoNm2						
				,ISNULL(JM_Nenryo2.RyakuNm, '') AS NenryoRyakuNm2						
				,ISNULL(TKD_Shabni.Nenryo2, 0.00) AS Nenryo2						
				,ISNULL(JM_Nenryo3.CodeKbn, 0) AS NenryoCd3						
				,ISNULL(JM_Nenryo3.CodeKbnNm, '') AS NenryoNm3						
				,ISNULL(JM_Nenryo3.RyakuNm, '') AS NenryoRyakuNm3						
				,ISNULL(TKD_Shabni.Nenryo3, 0.00) AS Nenryo3						
				,ISNULL(TKD_Shabni.JyoSyaJin, 0) AS JyoSyaJin						
				,ISNULL(TKD_Shabni.PlusJin, 0) AS PlusJin						
				,ISNULL(TKD_Shabni.UnkKai, 1) AS UnkKai						
				,ISNULL(JM_Syain1.SyainCd, 0) AS SyainCd1						
				,ISNULL(JM_Syain1.SyainNm, '') AS SyainNm1						
				,ISNULL(JM_Syain1.KariSyainNm, '') AS KariSyainNm1						
				,ISNULL(JM_Syain2.SyainCd, 0) AS SyainCd2						
				,ISNULL(JM_Syain2.SyainNm, '') AS SyainNm2						
				,ISNULL(JM_Syain2.KariSyainNm, '') AS KariSyainNm2						
				,ISNULL(JM_Syain3.SyainCd, 0) AS SyainCd3						
				,ISNULL(JM_Syain3.SyainNm, '') AS SyainNm3						
				,ISNULL(JM_Syain3.KariSyainNm, '') AS KariSyainNm3						
				,ISNULL(JM_Syain4.SyainCd, 0) AS SyainCd4						
				,ISNULL(JM_Syain4.SyainNm, '') AS SyainNm4						
				,ISNULL(JM_Syain4.KariSyainNm, '') AS KariSyainNm4						
				,ISNULL(JM_Syain5.SyainCd, 0) AS SyainCd5						
				,ISNULL(JM_Syain5.SyainNm, '') AS SyainNm5						
				,ISNULL(JM_Syain5.KariSyainNm, '') AS KariSyainNm5						
				,ISNULL(JM_Eigyos.EigyoCdSeq, 0) AS EigyoCdSeq						
				,ISNULL(JM_Eigyos.EigyoCd, 0) AS EigyoCd						
				,ISNULL(JM_Eigyos.RyakuNm, '') AS EigyoRyakuNm																																									
		   FROM TKD_Haisha																																			
		        LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Haisha.UkeNo = JT_Yyksho.UkeNo																	
                AND JT_Yyksho.SiyoKbn = 1																	
                AND JT_Yyksho.TenantCdSeq = @TenantCdSeq																	
				LEFT JOIN TKD_Unkobi AS JT_Unkobi ON TKD_Haisha.UkeNo = JT_Unkobi.UkeNo																	
						AND TKD_Haisha.UnkRen = JT_Unkobi.UnkRen																	
						AND JT_Unkobi.SiyoKbn = 1																	
				LEFT JOIN TKD_Shabni ON TKD_Shabni.UkeNo = TKD_Haisha.UkeNo																	
						AND TKD_Shabni.UnkRen = TKD_Haisha.UnkRen																	
						AND TKD_Shabni.TeiDanNo = TKD_Haisha.TeiDanNo																	
						AND TKD_Shabni.BunkRen = TKD_Haisha.BunkRen																	
						AND TKD_Haisha.SiyoKbn = 1																	
				LEFT JOIN VPM_SyaRyo AS JM_SyaRyo ON TKD_Haisha.HaiSSryCdSeq = JM_SyaRyo.SyaRyoCdSeq																	
				LEFT JOIN VPM_HenSya AS JM_HenSya ON TKD_Haisha.HaiSSryCdSeq = JM_HenSya.SyaRyoCdSeq																	
						AND TKD_Haisha.HaiSYmd >= JM_HenSya.StaYmd																	
						AND TKD_Haisha.HaiSYmd <= JM_HenSya.EndYmd																	
				LEFT JOIN VPM_Eigyos AS JM_Eigyos ON JM_HenSya.EigyoCdSeq = JM_Eigyos.EigyoCdSeq																	
						AND JM_Eigyos.SiyoKbn = 1																	
				LEFT JOIN VPM_Compny AS JM_Compny ON JM_Eigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq																	
						AND JM_Compny.SiyoKbn = 1																	
				LEFT JOIN VPM_Tenant AS JM_Tenant ON JM_Tenant.TenantCdSeq = JM_Compny.TenantCdSeq																	
						AND JM_Tenant.SiyoKbn = 1																	
				LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq																	
						AND JM_YoyKbn.SiyoKbn = 1	
				LEFT JOIN VPM_YoyaKbnSort AS JM_YoyaKbnSort ON JM_YoyKbn.YoyaKbnSeq  =  JM_YoyaKbnSort.YoyaKbnSeq          					
					AND JM_YoyaKbnSort.TenantCdSeq  =  @TenantCdSeq											
				LEFT JOIN VPM_SyaSyu AS JM_SyaSyu ON JM_SyaRyo.SyaSyuCdSeq = JM_SyaSyu.SyaSyuCdSeq																	
				LEFT JOIN (																	
						SELECT CodeKbn																	
								,RyakuNm																	
						FROM VPM_CodeKb																	
						WHERE CodeSyu = 'KATAKBN'																	
								AND SiyoKbn = 1																	
								AND VPM_CodeKb.TenantCdSeq = (																	
										SELECT CASE 																	
														WHEN COUNT(*) = 0																	
																THEN 0																	
														ELSE @TenantCdSeq																	
														END AS TenantCdSeq																	
										FROM VPM_CodeKb																	
										WHERE VPM_CodeKb.CodeSyu = 'KATAKBN'																	
												AND VPM_CodeKb.SiyoKbn = 1																	
												AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
										)																	
						) AS JM_KataKbn ON JM_SyaSyu.KataKbn = CONVERT(TINYINT, JM_KataKbn.CodeKbn)																	
				LEFT JOIN (																	
						SELECT CodeKbn																	
								,RyakuNm																	
						FROM VPM_CodeKb																	
						WHERE CodeSyu = 'NINKAKBN'																	
								AND SiyoKbn = 1																	
								AND VPM_CodeKb.TenantCdSeq = (																	
										SELECT CASE 																	
														WHEN COUNT(*) = 0																	
																THEN 0																	
														ELSE @TenantCdSeq																	
														END AS TenantCdSeq																	
										FROM VPM_CodeKb																	
										WHERE VPM_CodeKb.CodeSyu = 'NINKAKBN'																	
												AND VPM_CodeKb.SiyoKbn = 1																	
												AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
										)																	
						) AS JM_NinkaKbn ON JM_SyaRyo.NinkaKbn = CONVERT(TINYINT, JM_NinkaKbn.CodeKbn)																	
				LEFT JOIN VPM_CodeKb AS JM_Nenryo1 ON JM_SyaRyo.NenryoCd1Seq = JM_Nenryo1.codeKbnSeq																	
						AND JM_Nenryo1.SiyoKbn = 1																	
						AND JM_Nenryo1.TenantCdSeq = (																	
								SELECT CASE 																	
												WHEN COUNT(*) = 0																	
														THEN 0																	
												ELSE @TenantCdSeq																	
												END AS TenantCdSeq																	
								FROM VPM_CodeKb																	
								WHERE VPM_CodeKb.codeKbnSeq = JM_SyaRyo.NenryoCd1Seq																	
										AND VPM_CodeKb.SiyoKbn = 1																	
										AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
								)																	
				LEFT JOIN VPM_CodeKb AS JM_Nenryo2 ON JM_SyaRyo.NenryoCd2Seq = JM_Nenryo2.codeKbnSeq																	
						AND JM_Nenryo2.SiyoKbn = 1																	
						AND JM_Nenryo2.TenantCdSeq = (																	
								SELECT CASE 																	
												WHEN COUNT(*) = 0																	
														THEN 0																	
												ELSE @TenantCdSeq																	
												END AS TenantCdSeq																	
								FROM VPM_CodeKb																	
								WHERE VPM_CodeKb.codeKbnSeq = JM_SyaRyo.NenryoCd2Seq																	
										AND VPM_CodeKb.SiyoKbn = 1																	
										AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
								)																	
				LEFT JOIN VPM_CodeKb AS JM_Nenryo3 ON JM_SyaRyo.NenryoCd3Seq = JM_Nenryo3.codeKbnSeq																	
						AND JM_Nenryo3.SiyoKbn = 1																	
						AND JM_Nenryo3.TenantCdSeq = (																	
								SELECT CASE 																	
												WHEN COUNT(*) = 0																	
														THEN 0																	
												ELSE @TenantCdSeq																	
												END AS TenantCdSeq																	
								FROM VPM_CodeKb																	
								WHERE VPM_CodeKb.codeKbnSeq = JM_SyaRyo.NenryoCd3Seq																	
										AND VPM_CodeKb.SiyoKbn = 1																	
										AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
								)																	
				LEFT JOIN VPM_CodeKb AS JT_Nenryo1 ON TKD_Shabni.NenryoCd1Seq = JT_Nenryo1.codeKbnSeq																	
						AND JT_Nenryo1.SiyoKbn = 1																	
						AND JT_Nenryo1.TenantCdSeq = (																	
								SELECT CASE 																	
												WHEN COUNT(*) = 0																	
														THEN 0																	
												ELSE @TenantCdSeq																	
												END AS TenantCdSeq																	
								FROM VPM_CodeKb																	
								WHERE VPM_CodeKb.codeKbnSeq = TKD_Shabni.NenryoCd1Seq																	
										AND VPM_CodeKb.SiyoKbn = 1																	
										AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
								)																	
				LEFT JOIN VPM_CodeKb AS JT_Nenryo2 ON TKD_Shabni.NenryoCd2Seq = JT_Nenryo2.codeKbnSeq																	
						AND JT_Nenryo2.SiyoKbn = 1																	
						AND JT_Nenryo2.TenantCdSeq = (																	
								SELECT CASE 																	
												WHEN COUNT(*) = 0																	
														THEN 0																	
												ELSE @TenantCdSeq																	
												END AS TenantCdSeq																	
								FROM VPM_CodeKb																	
								WHERE VPM_CodeKb.codeKbnSeq = TKD_Shabni.NenryoCd2Seq																	
										AND VPM_CodeKb.SiyoKbn = 1																	
										AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
								)																	
				LEFT JOIN VPM_CodeKb AS JT_Nenryo3 ON TKD_Shabni.NenryoCd3Seq = JT_Nenryo3.codeKbnSeq																	
						AND JT_Nenryo3.SiyoKbn = 1																	
						AND JT_Nenryo3.TenantCdSeq = (																	
								SELECT CASE 																	
												WHEN COUNT(*) = 0																	
														THEN 0																	
												ELSE @TenantCdSeq																	
												END AS TenantCdSeq																	
								FROM VPM_CodeKb																	
								WHERE VPM_CodeKb.codeKbnSeq = TKD_Shabni.NenryoCd3Seq																	
										AND VPM_CodeKb.SiyoKbn = 1																	
										AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																	
								)																	
				LEFT JOIN VPM_Tokisk AS JM_Tokisk ON JT_Yyksho.TokuiSeq = JM_Tokisk.TokuiSeq																	
						AND JT_Yyksho.SeiTaiYmd >= JM_Tokisk.SiyoStaYmd																	
						AND JT_Yyksho.SeiTaiYmd <= JM_Tokisk.SiyoEndYmd																	
						AND JM_Tokisk.TenantCdSeq = @TenantCdSeq																	
				LEFT JOIN VPM_TokiSt AS JM_TokiSt ON JT_Yyksho.TokuiSeq = JM_TokiSt.TokuiSeq																	
						AND JT_Yyksho.SitenCdSeq = JM_TokiSt.SitenCdSeq																	
						AND JT_Yyksho.SeiTaiYmd >= JM_TokiSt.SiyoStaYmd																	
						AND JT_Yyksho.SeiTaiYmd <= JM_TokiSt.SiyoEndYmd																	
				LEFT JOIN VPM_Gyosya AS JM_Gyosya ON JM_Tokisk.GyosyaCdSeq = JM_Gyosya.GyosyaCdSeq																	
						AND JM_Gyosya.SiyoKbn = 1																	
				LEFT JOIN #Temp AS JW_Haisha ON TKD_Shabni.UkeNo = JW_Haisha.UkeNo																	
						AND TKD_Shabni.UnkRen = JW_Haisha.UnkRen																	
						AND TKD_Shabni.TeiDanNo = JW_Haisha.TeiDanNo																	
						AND TKD_Shabni.BunkRen = JW_Haisha.BunkRen																	
				LEFT JOIN VPM_Syain AS JM_Syain1 ON JW_Haisha.SyainCdSeq1 = JM_Syain1.SyainCdSeq																	
				LEFT JOIN VPM_Syain AS JM_Syain2 ON JW_Haisha.SyainCdSeq2 = JM_Syain2.SyainCdSeq																	
				LEFT JOIN VPM_Syain AS JM_Syain3 ON JW_Haisha.SyainCdSeq3 = JM_Syain3.SyainCdSeq																	
				LEFT JOIN VPM_Syain AS JM_Syain4 ON JW_Haisha.SyainCdSeq4 = JM_Syain4.SyainCdSeq																	
				LEFT JOIN VPM_Syain AS JM_Syain5 ON JW_Haisha.SyainCdSeq5 = JM_Syain5.SyainCdSeq																																														
		   WHERE 1 = 1																																			
			 AND JT_Yyksho.YoyaSyu = 1																																			
			 AND TKD_Haisha.YouTblSeq = 0																																			
			 AND TKD_Haisha.HaiSSryCdSeq <> 0																																			
			 -- AND (
				-- (@NipoYmdStr = '' AND @NipoYmdEnd = '')
				-- OR (@NipoYmdStr <> '' AND @NipoYmdEnd <> '' AND TKD_Shabni.UnkYmd BETWEEN @NipoYmdStr AND @NipoYmdEnd)
				-- OR (@NipoYmdStr <> '' AND @NipoYmdEnd = '' AND TKD_Shabni.UnkYmd >= @NipoYmdStr)
				-- OR (@NipoYmdEnd <> '' AND @NipoYmdStr = '' AND TKD_Shabni.UnkYmd <= @NipoYmdEnd)
			 -- )																																			
			 AND (@EigyoCompnyCd = 0 OR JM_Compny.CompanyCdSeq = @EigyoCompnyCd)																																			
			 AND (
				(@SyaEigyoCdStr = 0 AND @SyaEigyoCdEnd = 0)
				OR (@SyaEigyoCdStr <> 0 AND @SyaEigyoCdEnd <> 0 AND JM_Eigyos.EigyoCd BETWEEN @SyaEigyoCdStr AND @SyaEigyoCdEnd)
				OR (@SyaEigyoCdStr <> 0 AND @SyaEigyoCdEnd = 0 AND JM_Eigyos.EigyoCd >= @SyaEigyoCdStr)
				OR (@SyaEigyoCdEnd <> 0 AND @SyaEigyoCdStr = 0 AND JM_Eigyos.EigyoCd <= @SyaEigyoCdEnd)
			 )																																			
			 AND (
				(@SyaRyoCdStr = 0 AND @SyaRyoCdEnd = 0)
				OR (@SyaRyoCdStr <> 0 AND @SyaRyoCdEnd <> 0 AND JM_SyaRyo.SyaRyoCd BETWEEN @SyaRyoCdStr AND @SyaRyoCdEnd)
				OR (@SyaRyoCdStr <> 0 AND @SyaRyoCdEnd = 0 AND JM_SyaRyo.SyaRyoCd >= @SyaRyoCdStr)
				OR (@SyaRyoCdEnd <> 0 AND @SyaRyoCdEnd = 0 AND JM_SyaRyo.SyaRyoCd <= @SyaRyoCdEnd)
			 )																																		
			 AND (
				(@UkeNoStr = '' AND @UkeNoEnd = '')
				OR (@UkeNoStr <> '' AND @UkeNoEnd <> '' AND SUBSTRING(JT_Unkobi.UkeNo, 6, LEN(JT_Unkobi.UkeNo)) BETWEEN @UkeNoStr AND @UkeNoEnd)
				OR (@UkeNoStr <> '' AND @UkeNoEnd = '' AND SUBSTRING(JT_Unkobi.UkeNo, 6, LEN(JT_Unkobi.UkeNo)) >= @UkeNoStr)
				OR (@UkeNoEnd <> '' AND @UkeNoStr = '' AND SUBSTRING(JT_Unkobi.UkeNo, 6, LEN(JT_Unkobi.UkeNo)) <= @UkeNoEnd)
			 )																																			
			 -- AND (
				-- (@YoyaKbnStr = 0 AND @YoyaKbnEnd = 0)
				-- OR (@YoyaKbnStr <> 0 AND @YoyaKbnEnd <> 0 AND JM_YoyKbn.YoyaKbn BETWEEN @YoyaKbnStr AND @YoyaKbnEnd)
				-- OR (@YoyaKbnStr <> 0 AND @YoyaKbnEnd = 0 AND JM_YoyKbn.YoyaKbn >= @YoyaKbnStr)
				-- OR (@YoyaKbnEnd <> 0 AND @YoyaKbnStr = 0 AND JM_YoyKbn.YoyaKbn <= @YoyaKbnEnd)
			 -- )	
			 AND (@YoyaKbnStr = '' OR CONCAT (							
				CASE 							
					WHEN JM_YoyaKbnSort.PriorityNum IS NULL							
						THEN '99'							
					ELSE FORMAT(JM_YoyaKbnSort.PriorityNum, '00')							
						END							
				,FORMAT(JM_YoyKbn.YoyaKbn, '00')							
				) >= @YoyaKbnStr) --@YoyaKbnStart｛予約区分（開始）「PriorityNum」の値｝                                                        							
			 AND (@YoyaKbnEnd = '' OR CONCAT (							
				CASE 							
					WHEN JM_YoyaKbnSort.PriorityNum IS NULL							
						THEN '99'							
					ELSE FORMAT(JM_YoyaKbnSort.PriorityNum, '00')							
						END							
				,FORMAT(JM_YoyKbn.YoyaKbn, '00')							
				) <= @YoyaKbnEnd) --@YoyaKbnEnd｛予約区分（終了）「PriorityNum」の値｝ 		
			 AND (@SyaRyoCdSeq = 0 OR JM_SyaRyo.SyaRyoCdSeq = @SyaRyoCdSeq)
			 --AND (@YmdStr = '' OR TKD_Shabni.UnkYmd >= @YmdStr)																																		
			 --AND (@YmdEnd = '' OR TKD_Shabni.UnkYmd <= @YmdEnd)
		) AS MAINTABLE																																			
	END
