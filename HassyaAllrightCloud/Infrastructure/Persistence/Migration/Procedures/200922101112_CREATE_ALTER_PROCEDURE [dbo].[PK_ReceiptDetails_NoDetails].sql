USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_ReceiptDetails_NoDetails]    Script Date: 9/22/2020 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_ReceiptDetails_NoDetails
-- DB-Name		:   HOC_Kashikiri
-- Name			:   PK_ReceiptDetails_NoDetails
-- Date			:   2020/09/22
-- Author		:   N.T.HIEU
-- Description	:   Get Data Receipt Detail And No Detail
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_ReceiptDetails_NoDetails]
    (
     -- Parameter
         @IssueDate                   VARCHAR(8)      
	 ,   @SyainCdSeq                  VARCHAR(8)  
	 ,   @EigyoCdSeq                  VARCHAR(8)
	 ,   @SeiOutSeqSeiRen             VARCHAR(MAX)
	 ,   @UpdPrgID                    VARCHAR(8)
	 ,   @TenantCdSeq                 VARCHAR(4)  
	 
	   -- Output
	 ,	@ROWCOUNT	                  INTEGER OUTPUT	   -- 処理件数
	 ,	@RyoOutSeqOut	              INTEGER OUTPUT	   -- 処理件数
    )
AS 
	-- Init Value
	Declare @RyoOutTime VARCHAR(4);
	Declare @UpdYmd VARCHAR(8);
	Declare @UpdTime VARCHAR(8);
	Declare @RyoOutSeq INT;

	-- Get Value
	SELECT @RyoOutTime = FORMAT(GETDATE(),'HHmm');
	SELECT @UpdYmd = FORMAT(GETDATE(),'yyyyMMdd');
	SELECT @UpdTime = FORMAT(GETDATE(),'HHmmss');
	
    -- Processing
	BEGIN
		 INSERT INTO TKD_RyoPrS (																																		
        							RyoHatYmd,																																		
        							RyoOutTime,																																		
        							InTanCdSeq,																																		
									RyoEigCdSeq,
									RyoFileId,
        							SiyoKbn,																																		
        							UpdYmd,																																		
        							UpdTime,																																		
        							UpdSyainCd,																																		
        							UpdPrgID																																		
    						    )																																		
		VALUES																																		
                                (																																		
        						    @IssueDate,																																		
        						    @RyoOutTime,																																		
        						    @SyainCdSeq,																																		
								    @EigyoCdSeq,
									'',
        						    1,																																		
        						    @UpdYmd,																																		
        						    @UpdTime,																																		
        						    @SyainCdSeq,																																		
        						    @UpdPrgID																																	
    							)	

		SELECT @RyoOutSeq = SCOPE_IDENTITY();
		INSERT INTO TKD_Ryoshu																																		
						SELECT	    @RyoOutSeq                    AS RyoOutSeq,																																		
    								    DENSE_RANK() OVER (																																		
        							    	ORDER BY																																		
            						    		SeiOutSeq,																																		
            						    		SeiRen																																		
    								    )                             AS RyoRen,																																		
                                    SeiOutSeq                     AS SeiOutSeq,																																		
                                    SeiRen                        AS SeiRen,																																		
                                    1                             AS SiyoKbn,																																		
                                    @UpdYmd                       AS UpdYmd,																																		
                                    @UpdTime                      AS UpdTime,																																		
                                    @SyainCdSeq                   AS UpdSyainCd,																																		
                                    @UpdPrgID                     AS UpdPrgID																																		
						FROM     TKD_Seikyu																																		
		                WHERE	 SiyoKbn = 1 
			                 AND @SeiOutSeqSeiRen LIKE '%' + (CONCAT(FORMAT(SeiOutSeq, '00000000'),FORMAT(SeiRen, '0000')))+ '%'

		--*******************************************************************ヘッダ結果セット出力-START*********************************************************************
		SELECT																																	
    			  TKD_Ryoshu.RyoOutSeq,																																	
    			  TKD_Ryoshu.RyoRen,																																	
    			  eTKD_Seikyu11.SeiOutSeq,																																	
    			  eTKD_Seikyu11.SeiRen,																																	
    			  eTKD_Seikyu11.SeikYm,																																	
    			  eTKD_Seikyu11.ZenKurG,																																	
    			  eTKD_Seikyu11.KonUriG,																																	
    			  eTKD_Seikyu11.KonSyoG,																																	
    			  eTKD_Seikyu11.KonTesG,																																	
    			  eTKD_Seikyu11.KonNyuG,																																	
    			  eTKD_Seikyu11.KonSeiG,																																	
              CASE																																	
                  WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn, 0) = 1 THEN ISNULL(eVPM_TokiSt11.ZipCd, '')																																	
                  ELSE ISNULL(eTKD_SeiPrS11.ZipCd, '')																																	
              END AS ZipCd,																																	
              CASE																																	
                  WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn, 0) = 1 THEN ISNULL(eVPM_TokiSt11.Jyus1, '')																																	
                  ELSE ISNULL(eTKD_SeiPrS11.Jyus1, '')																																	
              END AS Jyus1,																																	
              CASE																																	
                  WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn, 0) = 1 THEN ISNULL(eVPM_TokiSt11.Jyus2, '')																																	
                  ELSE ISNULL(eTKD_SeiPrS11.Jyus2, '')																																	
              END AS Jyus2,																																	
              CASE																																	
                  WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn, 0) = 1 THEN ISNULL(eVPM_Tokisk11.TokuiNm, '')																																	
                  ELSE ISNULL(eTKD_SeiPrS11.TokuiNm, '')																																	
              END AS TokuiNm,																																	
              CASE																																	
                  WHEN ISNULL(eTKD_SeiPrS11.SeiOutSyKbn, 0) = 1 THEN ISNULL(eVPM_TokiSt11.SitenNm, '')																																	
                  ELSE ISNULL(eTKD_SeiPrS11.SitenNm, '')																																	
              END AS SitenNm,																																	
              ISNULL(eVPM_Eigyos11.ZipCd, '') AS SeiEigZipCd,																																	
              ISNULL(eVPM_Eigyos11.Jyus1, '') AS SeiEigJyus1,																																	
              ISNULL(eVPM_Eigyos11.Jyus2, '') AS SeiEigJyus2,																																	
              ISNULL(eVPM_Eigyos11.EigyoNm, '') AS SeiEigEigyoNm,																																	
              ISNULL(eVPM_Tokist11.TokuiTanNm, '') AS TokuiTanNm,																																	
              ISNULL(eVPM_Eigyos11.TelNo, '') AS SeiEigTelNo,																																	
              ISNULL(eVPM_Eigyos11.FaxNo, '') AS SeiEigFaxNo,																																	
              ISNULL(eTKD_SeiUch11.MeisaiKensu, 0) AS MeisaiKensu,																																	
              ISNULL(eTKD_SeiPrS11.SeiHatYmd, '') AS SeiHatYmd,																																	
              ISNULL(eVPM_Compny11.CompanyNm, '') AS SeiEigCompanyNm																																	
		FROM																																	
              TKD_Ryoshu																																	
              LEFT JOIN TKD_RyoPrS AS eTKD_RyoPrS11 ON TKD_Ryoshu.RyoOutSeq = eTKD_RyoPrS11.RyoOutSeq																																	
              JOIN TKD_Seikyu AS eTKD_Seikyu11 ON TKD_Ryoshu.SeiOutSeq = eTKD_Seikyu11.SeiOutSeq																																	
              AND TKD_Ryoshu.SeiRen = eTKD_Seikyu11.SeiRen																																	
              LEFT JOIN TKD_SeiPrS AS eTKD_SeiPrS11 ON eTKD_Seikyu11.SeiOutSeq = eTKD_SeiPrS11.SeiOutSeq																																	
              LEFT JOIN VPM_Tokisk AS eVPM_Tokisk11 ON eTKD_Seikyu11.TokuiSeq = eVPM_Tokisk11.TokuiSeq																																	
              AND eTKD_Seikyu11.SiyoEndYmd BETWEEN eVPM_Tokisk11.SiyoStaYmd																																	
              AND eVPM_Tokisk11.SiyoEndYmd																																	
              AND eVPM_Tokisk11.TenantCdSeq = @TenantCdSeq																																	
              LEFT JOIN VPM_TokiSt AS eVPM_TokiSt11 ON eTKD_Seikyu11.TokuiSeq = eVPM_TokiSt11.TokuiSeq																																	
              AND eTKD_Seikyu11.SitenCdSeq = eVPM_TokiSt11.SitenCdSeq																																	
              AND eTKD_Seikyu11.SiyoEndYmd BETWEEN eVPM_TokiSt11.SiyoStaYmd																																	
              AND eVPM_TokiSt11.SiyoEndYmd																																	
              LEFT JOIN VPM_Eigyos AS eVPM_Eigyos11 ON eTKD_SeiPrS11.SeiEigCdSeq = eVPM_Eigyos11.EigyoCdSeq																																	
              LEFT JOIN VPM_Compny AS eVPM_Compny11 ON eVPM_Eigyos11.CompanyCdSeq = eVPM_Compny11.CompanyCdSeq																																	
              LEFT JOIN (																																	
                  SELECT																																	
                      TKD_SeiUch.SeiOutSeq AS SeiOutSeq,																																	
                      TKD_SeiUch.SeiRen AS SeiRen,																																	
                      COUNT(*) AS MeisaiKensu																																	
                  FROM																																	
                      TKD_SeiUch																																	
                  GROUP BY																																	
                      TKD_SeiUch.SeiOutSeq,																																	
                      TKD_SeiUch.SeiRen																																	
              ) AS eTKD_SeiUch11 On eTKD_Seikyu11.SeiOutSeq = eTKD_SeiUch11.SeiOutSeq																																	
              AND eTKD_Seikyu11.SeiRen = eTKD_SeiUch11.SeiRen																																	
              WHERE		TKD_Ryoshu.RyoOutSeq = @RyoOutSeq																																	
		--*******************************************************************ヘッダ結果セット出力-END***********************************************************************

		--*******************************************************************明細結果セット出力-START***********************************************************************
		SELECT																																	
               TKD_SeiUch.SeiOutSeq,																																	
               TKD_SeiUch.SeiRen,																																	
               TKD_SeiUch.SeiMeiRen,																																	
               TKD_SeiUch.SeiUchRen,																																	
               TKD_SeiUch.HasYmd,																																	
               TKD_SeiUch.YoyaNm,																																	
               TKD_SeiUch.FutTumNm,																																	
               TKD_SeiUch.HaiSYmd,																																	
               TKD_SeiUch.TouYmd,																																	
               TKD_SeiUch.Suryo,																																	
               TKD_SeiUch.TanKa,																																	
               TKD_SeiUch.SyaSyuNm,																																	
               CASE																																	
                   WHEN eTKD_Mishum11.SeiFutSyu IN (1)																																	
                   AND TKD_SeiUch.SeiUchRen <> 1 THEN 0																																	
                   ELSE ISNULL(eTKD_SeiMei11.UriGakKin, 0)																																	
               END AS UriGakKin,																																	
               CASE																																	
                   WHEN eTKD_Mishum11.SeiFutSyu IN (1)																																	
                   AND TKD_SeiUch.SeiUchRen <> 1 THEN 0																																	
                   ELSE ISNULL(eTKD_SeiMei11.SyaRyoSyo, 0)																																	
               END AS SyaRyoSyo,																																	
               CASE																																	
                   WHEN eTKD_Mishum11.SeiFutSyu IN (1)																																	
                   AND TKD_SeiUch.SeiUchRen <> 1 THEN 0																																	
                   ELSE ISNULL(eTKD_SeiMei11.SyaRyoTes, 0)																																	
               END AS SyaRyoTes,																																	
               CASE																																	
                   WHEN eTKD_Mishum11.SeiFutSyu IN (1)																																	
                   AND TKD_SeiUch.SeiUchRen <> 1 THEN 0																																	
                   ELSE ISNULL(eTKD_SeiMei11.SeiKin, 0)																																	
               END AS SeiKin,																																	
               CASE																																	
                   WHEN eTKD_Mishum11.SeiFutSyu IN (1)																																	
                   AND TKD_SeiUch.SeiUchRen <> 1 THEN 0																																	
                   ELSE ISNULL(eTKD_SeiMei11.NyuKinRui, 0)																																	
               END AS NyuKinRui,																																	
               CASE																																	
                   WHEN eTKD_Mishum11.SeiFutSyu IN (1) THEN ''																																	
                   ELSE ISNULL(eTKD_FutTum11.BikoNm, '')																																	
               END AS BikoNm,																																	
               ISNULL(eTKD_SeiMei11.SeiSaHKbn, 0) AS SeiSaHKbn,																																	
               ISNULL(eTKD_SeiMei11.UkeNo, 0) AS UkeNo,																																	
               ISNULL(eTKD_FutTum11.IriRyoNm, '') AS IriRyoNm,																																	
               ISNULL(eTKD_FutTum11.DeRyoNm, '') AS DeRyoNm,																																	
               ISNULL(eTKD_Mishum11.SeiFutSyu, 0) AS SeiFutSyu,																																	
               ISNULL(eVPM_Futai11.FutaiCd, 0) AS FutaiCd,																																	
               CASE																																	
                   WHEN eTKD_Mishum11.SeiFutSyu IN (1)																																	
                   AND TKD_SeiUch.SeiUchRen <> 1 THEN 0																																	
                   ELSE ISNULL(eTKD_SeiMei11.Zeiritsu, 0)																																	
               END AS Zeiritsu																																	
			FROM																																	
                 TKD_SeiUch																																	
                 LEFT JOIN TKD_SeiMei AS eTKD_SeiMei11 ON TKD_SeiUch.SeiOutSeq = eTKD_SeiMei11.SeiOutSeq																																	
                 AND TKD_SeiUch.SeiRen = eTKD_SeiMei11.SeiRen																																	
                 AND TKD_SeiUch.SeiMeiRen = eTKD_SeiMei11.SeiMeiRen																																	
                 LEFT JOIN TKD_Ryoshu AS TKD_Ryoshu11 ON TKD_SeiUch.SeiOutSeq = TKD_Ryoshu11.SeiOutSeq																																	
                 AND TKD_SeiUch.SeiRen = TKD_Ryoshu11.SeiRen																																	
                 LEFT JOIN TKD_Mishum AS eTKD_Mishum11 ON eTKD_SeiMei11.UkeNo = eTKD_Mishum11.UkeNo																																	
                 AND eTKD_SeiMei11.MisyuRen = eTKD_Mishum11.MisyuRen																																	
                 LEFT JOIN TKD_FutTum AS eTKD_FutTum11 ON eTKD_Mishum11.UkeNo = eTKD_FutTum11.UkeNo																																	
                 AND eTKD_Mishum11.FutuUnkRen = eTKD_FutTum11.UnkRen																																	
                 AND CASE																																	
                     WHEN eTKD_Mishum11.SeiFutSyu = 6 THEN 2																																	
                     ELSE 1																																	
                 END = eTKD_FutTum11.FutTumKbn																																	
                 AND eTKD_Mishum11.FutTumRen = eTKD_FutTum11.FutTumRen																																	
                 LEFT JOIN VPM_Futai AS eVPM_Futai11 ON eVPM_Futai11.FutaiCdSeq = eTKD_FutTum11.FutTumCdSeq																																	
                 AND eVPM_Futai11.SiyoKbn = 1																																	
            WHERE																																	
                TKD_Ryoshu11.RyoOutSeq = @RyoOutSeq																																	
            ORDER BY																																	
                TKD_SeiUch.SeiOutSeq,																																	
                TKD_SeiUch.SeiRen,																																	
                TKD_SeiUch.SeiMeiRen,																																	
                TKD_SeiUch.SeiUchRen																																	

		--*******************************************************************明細結果セット出力-END*************************************************************************
	END
	SET	@ROWCOUNT	=	@@ROWCOUNT
	SET @RyoOutSeqOut = @RyoOutSeq
RETURN																													

