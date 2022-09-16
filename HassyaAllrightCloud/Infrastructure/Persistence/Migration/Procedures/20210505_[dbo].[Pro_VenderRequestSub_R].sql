USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_VenderRequestSub_R]    Script Date: 2021/05/05 10:59:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE OR ALTER   PROCEDURE [dbo].[Pro_VenderRequestSub_R]
		@bookingKeys Tblt_BookingKeyType READONLY,
		@tenantId int
AS
BEGIN

SELECT * FROM
(
SELECT 	
       CAST(0 AS BIT)						AS 'IsMainReport'
      ,YYKSHO.UkeNo            AS 'UkeNo'	
	  ,UNKOBI.UnkRen            AS 'UnkRen'																																																							
	  ,YYKSHO.TokuiTel         AS  'TokuiTel'																												
	  ,YYKSHO.TokuiTanNm        AS  'TokuiTanNm'																												
	  ,YYKSHO_TOKISK.TokuiNm   AS 'TokuiNm'																												
	  ,YYKSHO_TOKIST.SitenNm   AS 'TokuiSitenNm'																												
	  ,YOU_TOKISK.TokuiNm      AS 'TokiskTokuiNm'																												
      ,YOU_TOKIST.SitenNm      AS 'TokistTokuiSitenNm'																													
	  ,YOU_TOKIST.FaxNo        AS 'YouskFax'	
	  ,UNKOBI.DanTaNm          AS 'DanTaNm'																												
	  ,UNKOBI.KanjJyus1        AS  'KanjJyus1'																												
      ,UNKOBI.KanjJyus2        AS  'KanjJyus2'																													
	  ,UNKOBI.KanjTel          AS  'KanjTel'																												
	  ,UNKOBI.KanJNm           AS  'KanjNm'																												
	  ,HAISHA.HaiSYmd          AS  'HaisYmd' 																												
	  ,HAISHA.TouYmd           AS  'TouYmd' 																												
	  ,HAISHA.DanTaNm2          AS  'DanTaNm2'																												
	  ,HAISHA.IkNm             AS  'IkNm'																												
	  ,HAISHA.HaiSBinNm        AS  'HaiSBinNm'																												
	  ,HAISHA.HaiSKouKNm       AS  'HaiSKoukRyaku'																												
	  ,HAISHA.HaiSSetTime      AS  'HaiSSetTime'																												
	  ,HAISHA.HaiSTime         AS  'HaiSTime'																												
	  ,HAISHA.SyuPaTime        AS  'SyuPaTime'																												
	  ,HAISHA.HaiSNm           AS　'HaiSNm'																												
	  ,HAISHA.HaiSJyus1        AS  'HaiSJyus1'																												
	  ,HAISHA.HaiSJyus2        AS  'HaiSJyus2'																												
	  ,HAISHA.TouSKouKNm       AS  'TouChaKoukRyaku'																												
	  ,HAISHA.TouBinCdSeq      AS  'TouBinCdSeq'																												
	  ,HAISHA.TouSBinNm        AS  'TouSBinNm'																												
	  ,HAISHA.TouSetTime       AS  'TouSetTime'																												
	  ,HAISHA.TouNm            AS  'TouNm'																												
	  ,HAISHA.TouJyusyo1       AS  'TouJyus1'																												
	  ,HAISHA.TouJyusyo2       AS  'TouJyus2'																												
	  ,HAISHA.TouChTime        AS  'TouChTime'																												
	  ,HAISHA.JyoSyaJin        AS 'JyoSyaJin'																												
	  ,HAISHA.PlusJin          AS 'PlusJin'																												
	  ,HAISHA.OthJinKbn1       AS 'OthJinKbn1Ryaku'																																																									
	  ,HAISHA.OthJin1          AS 'OthJin1'																												
	  ,HAISHA.OthJinKbn2       AS 'OthJinKbn2Ryaku'																																																									
	  ,HAISHA.OthJin2          AS 'OthJin2'																												
	  ,HAISHA.SijJoKbn1        AS 'SijJokbn1Ryaku'																																																							
	  ,HAISHA.SijJoKbn2        AS 'SijJokbn2Ryaku'																																																						
	  ,HAISHA.SijJoKbn3        AS 'SijJokbn3Ryaku'																																																						
	  ,HAISHA.SijJoKbn4        AS 'SijJokbn4Ryaku'																																																					
	  ,HAISHA.SijJoKbn5        AS 'SijJokbn5Ryaku'	
      ,(SELECT SUM (SyaSyuDai) FROM TKD_YouSyu WHERE UkeNo = HAISHA.UkeNo 																											
            AND SiyoKbn = 1 AND UnkRen = HAISHA.UnkRen AND YouTblSeq = HAISHA.YouTblSeq) AS 'TotalBusRequired'		
	  ,HAISHA.GoSya            AS 'BusNum'																																																								
	  ,HAISHA.BikoNm           AS 'Biko'																												
	  ,HAISHA.YouTblSeq        AS 'YouTblSeq'																												
	  ,HAISHA.YouKataKbn       AS 'YouKataKbn'																												
	  ,HAISHA.TeiDanNo         AS 'TeiDanNo'
	  ,YOUSHA.ZeiKbn      AS 'YoushaZeiKbn'
      ,EIGYOS.EigyoNm   AS  'EigyoSNm'																													
	  ,EIGYOS.ZipCd     AS  'EigyoSZipCd'																												
	  ,EIGYOS.Jyus1     AS  'EigyoSJyuS1'																												
	  ,EIGYOS.Jyus2     AS  'EigyoSJyuS2'																												
	  ,EIGYOS.TelNo     AS  'EigyoSTel'																												
	  ,EIGYOS.FaxNo     AS  'EigyoSFax'	
	  ,HAISHA.BunkRen   AS  'BunkRen'
FROM @bookingKeys  b
LEFT JOIN TKD_Haisha AS  HAISHA 
		ON b.UkeNo = HAISHA.UkeNo
		AND b.UnkRen = HAISHA.UnkRen
LEFT JOIN TKD_Yyksho AS YYKSHO																							
       ON YYKSHO.UkeNo = HAISHA.UkeNo																							
       AND YYKSHO.SiyoKbn = 1																							
	  AND YYKSHO.TenantCdSeq = @tenantId     --ログインユーザーのテナントコードSeq																						
	  																						
LEFT JOIN  VPM_Eigyos AS EIGYOS 																							
       ON EIGYOS.EigyoCdSeq = YYKSHO.UkeEigCdSeq																							
	  AND EIGYOS.SiyoKbn = 1																																																													
																							
LEFT JOIN  VPM_Compny AS COMPNY 																							
       ON COMPNY.EigyoCdSeq = EIGYOS.EigyoCdSeq																							
	  AND COMPNY.CompanyCdSeq  = EIGYOS.CompanyCdSeq																						
	  AND COMPNY.SiyoKbn = 1 																						
	  AND COMPNY.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq																						
																							
LEFT JOIN TKD_Yousha AS YOUSHA																							
       ON YOUSHA.UkeNo = HAISHA.UkeNo																							
	  AND YOUSHA.UnkRen = HAISHA.UnkRen																						
	  AND YOUSHA.YouTblSeq = HAISHA.YouTblSeq
	  AND YOUSHA.SiyoKbn = 1
																							
LEFT JOIN  VPM_Tokisk AS YOU_TOKISK																							
       ON YOU_TOKISK.TokuiSeq = YOUSHA.YouCdSeq																							
      AND YOU_TOKISK.TenantCdSeq = @tenantId                    --ログインユーザーのテナントコードSeq																							
      AND YOU_TOKISK.SiyoStaYmd <= HAISHA.HaiSYmd																							
      AND YOU_TOKISK.SiyoEndYmd >= HAISHA.HaiSYmd																							
																							
LEFT JOIN  VPM_TokiSt AS YOU_TOKIST 																							
       ON YOU_TOKIST.TokuiSeq = YOUSHA.YouCdSeq																							
	  AND YOU_TOKIST.SitenCdSeq = YOUSHA.YouSitCdSeq																						
	  AND YOU_TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd																						
      AND YOU_TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd																							
																							
LEFT JOIN  VPM_Tokisk AS YYKSHO_TOKISK																							
       ON YYKSHO_TOKISK.TokuiSeq = YYKSHO.TokuiSeq																							
      AND YYKSHO_TOKISK.TenantCdSeq = @tenantId                   --ログインユーザーのテナントコードSeq																							
      AND YYKSHO_TOKISK.SiyoStaYmd <= HAISHA.HaiSYmd																							
      AND YYKSHO_TOKISK.SiyoEndYmd >= HAISHA.HaiSYmd																							
																							
LEFT JOIN  VPM_TokiSt AS YYKSHO_TOKIST 																							
       ON YYKSHO_TOKIST.TokuiSeq = YYKSHO.TokuiSeq																							
	  AND YYKSHO_TOKIST.SitenCdSeq = YYKSHO.SitenCdSeq																						
	  AND YYKSHO_TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd																						
      AND YYKSHO_TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd																							
																							
LEFT JOIN TKD_Unkobi AS UNKOBI																							
       ON UNKOBI.UkeNo = HAISHA.UkeNo																							
	  AND UNKOBI.UnkRen = HAISHA.UnkRen																						
	  AND UNKOBI.SiyoKbn = 1
) Loans
where Loans.YouTblSeq > 0

END


GO