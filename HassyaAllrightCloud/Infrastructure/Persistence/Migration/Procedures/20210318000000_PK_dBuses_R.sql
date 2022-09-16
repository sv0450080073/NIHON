USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dBuses_R]    Script Date: 3/18/2021 9:35:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAllrightCloud
-- Module-Name	:   HassyaAllrightCloud
-- SP-ID		:   PK_dBuses_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Buses List
-- Date			:   2020/08/05
-- Author		:   P.M.NHAT
-- Description	:   Get buses list with conditions for search
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[PK_dBuses_R]
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
	,	@TenantCdSeq		int
AS
BEGIN
	SELECT DISTINCT TOP 1000 ISNULL(JM_SyaRyo.SyaRyoCdSeq, 0) AS SyaRyoCdSeq
        ,ISNULL(JM_SyaRyo.SyaRyoCd, 0) AS SyaRyoCd
        ,ISNULL(JM_SyaRyo.SyaRyoNm, 0) AS SyaRyoNm
        ,ISNULL(JM_SyaSyu.SyaSyuNm, '') AS SyaSyuNm
        ,ISNULL(JM_KataKbn.RyakuNm, '') AS KataKbnRyakuNm
        ,ISNULL(JM_NinkaKbn.RyakuNm, '') AS NinkaKbnRyakuNm
        ,ISNULL(JM_Eigyos.RyakuNm, '') AS EigyoRyakuNm
	FROM TKD_Haisha
	LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Haisha.UkeNo = JT_Yyksho.UkeNo
			AND JT_Yyksho.SiyoKbn = 1
			AND JT_Yyksho.TenantCdSeq = @TenantCdSeq
			AND JT_Yyksho.YoyaSyu = 1
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
   WHERE 1 = 1																							
		 AND JT_Yyksho.YoyaSyu = 1																							
		 AND TKD_Haisha.YouTblSeq = 0																							
		 AND TKD_Haisha.HaiSSryCdSeq <> 0																							
		 AND (
			(@NipoYmdStr = '' AND @NipoYmdEnd = '')
			OR (@NipoYmdStr <> '' AND @NipoYmdEnd <> '' 
			AND ((TKD_Shabni.UnkYmd IS NULL 
				AND (TKD_Haisha.SyuKoYmd BETWEEN @NipoYmdStr AND @NipoYmdEnd OR TKD_Haisha.KikYmd BETWEEN @NipoYmdStr AND @NipoYmdEnd
				OR @NipoYmdStr BETWEEN TKD_Haisha.SyuKoYmd AND TKD_Haisha.KikYmd OR @NipoYmdEnd BETWEEN TKD_Haisha.SyuKoYmd AND TKD_Haisha.KikYmd)) 
				OR TKD_Shabni.UnkYmd BETWEEN @NipoYmdStr AND @NipoYmdEnd))
			OR (@NipoYmdStr <> '' AND @NipoYmdEnd = '' AND ((TKD_Shabni.UnkYmd IS NULL AND TKD_Haisha.SyuKoYmd >= @NipoYmdStr) OR TKD_Shabni.UnkYmd >= @NipoYmdStr))
			OR (@NipoYmdEnd <> '' AND @NipoYmdStr = '' AND ((TKD_Shabni.UnkYmd IS NULL AND TKD_Haisha.KikYmd <= @NipoYmdEnd) OR TKD_Shabni.UnkYmd <= @NipoYmdEnd))
		 )																																			
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
	ORDER BY SyaRyoCd																							 																					
END
