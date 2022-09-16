USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[RP_UnkoushijishoReportNew]    Script Date: 2021/05/17 8:08:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE OR ALTER  
PROCEDURE [dbo].[RP_UnkoushijishoReportNew]
    @TenantCdSeq    int,
    @SyuKoYmd       char(8),
    @UkeCdFrom      int,
    @UkeCdTo        int,
    @YoyaKbnSeqList nvarchar(MAX),
    @SyuEigCdSeq    int,
    @TeiDanNo       smallint,
    @UnkRen         smallint,
    @BunkRen        smallint,
	@UkenoList		nvarchar(MAX),
	@FormOutput		int,
    @SortOrder      smallint AS
	if(@UkenoList!='')
		if(@FormOutput=1)
		SELECT
							ROW_NUMBER() OVER(ORDER BY
							 CASE @SortOrder
								  WHEN 1
											 THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 2
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
					   END ASC, CASE @SortOrder
								  WHEN 3
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 4
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
					   END ASC)                                                                              AS Row_Num
						  , YYKSHO.UkeCd                                                                          AS 'YYKSHO_UkeCd'
						  , YYKSHO.TokuiTel                                                                       AS 'YYKSHO_TokuiTel'
						  , YYKSHO.TokuiTanNm                                                                     AS 'YYKSHO_TokuiTanNm'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'UNKOBI_HaiSYmd'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'UNKOBI_TouYmd'
						  , UNKOBI.DanTaNm                                                                        AS 'UNKOBI_DanTaNm'
						  , UNKOBI.KanjJyus1                                                                      AS 'UNKOBI_KanjJyus1'
						  , UNKOBI.KanjJyus2                                                                      AS 'UNKOBI_KanjJyus2'
						  , UNKOBI.KanjTel                                                                        AS 'UNKOBI_KanjTel'
						  , UNKOBI.KanJNm                                                                         AS 'UNKOBI_KanJNm'
						  , HAISHA.UkeNo                                                                          AS 'HAISHA_UkeNo'
						  , HAISHA.UnkRen                                                                         AS 'HAISHA_UnkRen'
						  , HAISHA.SyaSyuRen                                                                      AS 'HAISHA_SyaSyuRen'
						  , HAISHA.TeiDanNo                                                                       AS 'HAISHA_TeiDanNo'
						  , HAISHA.BunkRen                                                                        AS 'HAISHA_BunkRen'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'HAISHA_HaiSYmd'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS 'HAISHA_HaiSTime'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'HAISHA_TouYmd'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS 'HAISHA_TouChTime'
						  , HAISHA.KikYmd                                                                         AS 'HAISHA_KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS 'HAISHA_KikTime'
						  , HAISHA.DanTaNm2                                                                       AS 'HAISHA_DanTaNm2'
						  , HAISHA.OthJinKbn1                                                                     AS 'HAISHA_OthJinKbn1'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'OTHJIN1_CodeKbnNm'
						  , HAISHA.OthJin1                                                                        AS 'HAISHA_OthJin1'
						  , HAISHA.OthJinKbn2                                                                     AS 'HAISHA_OthJinKbn2'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'OTHJIN2_CodeKbnNm'
						  , HAISHA.OthJin2                                                                        AS 'HAISHA_OthJin2'
						  , HAISHA.HaiSNm                                                                         AS 'HAISHA_HaiSNm'
						  , HAISHA.HaiSJyus1                                                                      AS 'HAISHA_HaiSJyus1'
						  , HAISHA.HaiSJyus2                                                                      AS 'HAISHA_HaiSJyus2'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS 'HAISHA_HaiSSetTime'
						  , HAISHA.IkNm                                                                           AS 'HAISHA_IkNm'
						  , HAISHA.SyuKoYmd                                                                       AS 'HAISHA_SyuKoYmd'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS 'HAISHA_SyuKoTime'
						  , HAISHA.SyuEigCdSeq                                                                    AS 'HAISHA_SyuEigCdSeq'
						  , EIGYOSHO.EigyoCd                                                                      AS 'EIGYOSHO_EigyoCd'
						  , EIGYOSHO.EigyoNm                                                                      AS 'EIGYOSHO_EigyoNm'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS 'HAISHA_SyuPaTime'
						  , HAISHA.JyoSyaJin                                                                      AS 'HAISHA_JyoSyaJin'
						  , HAISHA.PlusJin                                                                        AS 'HAISHA_PlusJin'
						  , HAISHA.GoSya                                                                          AS 'HAISHA_GoSya'
						  , HAISHA.HaiSKouKNm                                                                     AS 'HAISHA_HaiSKouKNm'
						  , HAISHA.HaiSBinNm                                                                      AS 'HAISHA_HaiSBinNm'
						  , HAISHA.TouSKouKNm                                                                     AS 'HAISHA_TouSKouKNm'
						  , HAISHA.TouSBinNm                                                                      AS 'HAISHA_TouSBinNm'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS 'HAISHA_TouSetTime'
						  , HAISHA.TouNm                                                                          AS 'HAISHA_TouNm'
						  , HAISHA.TouJyusyo1                                                                     AS 'HAISHA_TouJyusyo1'
						  , HAISHA.TouJyusyo2                                                                     AS 'HAISHA_TouJyusyo2'
						  , YYKSYU.SyaSyuDai                                                                      AS 'YYKSYU_SyaSyuDai'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS 'SYASYU_SYARYO_SyaSyuNm'
						  , SYARYO.SyaRyoCd                                                                       AS 'SYARYO_SyaRyoCd'
						  , SYARYO.SyaRyoNm                                                                       AS 'SYARYO_SyaRyoNm'
						  , HENSYA.TenkoNo                                                                        AS 'HENSYA_TenkoNo'
						  , SYARYO.KariSyaRyoNm                                                                   AS 'SYARYO_KariSyaRyoNm'
						  , TOKISK.TokuiNm                                                                        AS 'TOKISK_TokuiNm'
						  , TOKIST.SitenNm                                                                        AS 'TOKIST_SitenNm'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS 'YYKSYU_SyaSyuDaisum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
						  --, incidental.UnkRen                                                                 AS '�^�s���A��'
						  --, incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
						  --, incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
						  --, incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
						  --, incidental.SeisanNm                                                               AS '���Z��'
						  --, incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
						  --, incidental.Suryo                                                                  AS '����'
						  --, incidental.TeiDanNo                                                               AS '��c�ԍ�'
						  --, incidental.BunkRen                                                                AS '�����A��'
						  , HAISHA.BikoNm                                                                     as 'HAISHA_BikoNm'
						  , STUFF(
							(
									  SELECT
												',' + SYAIN.SyainNm
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as DriverList
						  , STUFF(
							(
									  SELECT
												',' + CAST(SYOKUM.SyokumuNm as nvarchar(20))
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn    = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as GuiderList

							, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=1
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalDate
							,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=1
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+CAST( MFUTTU.Suryo AS varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSuryo
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsDate
						 ,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.FutTumNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsFutTumNm
							,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSeisanNm
							,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+Cast(MFUTTU.Suryo as varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSuryo
			FROM
							TKD_Haisha AS HAISHA
							LEFT JOIN
											TKD_Unkobi AS UNKOBI
											ON
															UNKOBI.UkeNo      = HAISHA.UkeNo
															AND UNKOBI.UnkRen = HAISHA.UnkRen
							INNER JOIN
											TKD_Yyksho AS YYKSHO
											ON
															YYKSHO.UkeNo           = HAISHA.UkeNo
															AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq           	
							LEFT JOIN
											TKD_YykSyu AS YYKSYU
											ON
															YYKSYU.UkeNo        = HAISHA.UkeNo
															AND YYKSYU.UnkRen   = HAISHA.UnkRen
															AND YYKSYU.SyaSyuRen=HAISHA.SyaSyuRen
															And YYKSYU.SiyoKbn  =1
							LEFT JOIN
											VPM_SyaSyu AS SYASYU
											ON
															SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
															and SYASYU.TenantCdSeq=@TenantCdSeq
							LEFT JOIN
											VPM_SyaRyo AS SYARYO
											ON
															SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
							LEFT JOIN
                                        VPM_SyaSyu AS SYASYU_SYARYO
                                        ON
                                                        SYARYO.SyaSyuCdSeq = SYASYU_SYARYO.SyaSyuCdSeq
														and SYASYU_SYARYO.TenantCdSeq=@TenantCdSeq
							LEFT JOIN
											VPM_HenSya AS HENSYA
											ON
															HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
															AND HENSYA.StaYmd <= HAISHA.HaiSYmd
															AND HENSYA.EndYmd >= HAISHA.HaiSYmd
							LEFT JOIN
											VPM_CodeKb AS OTHJIN1
											ON
															 CONVERT(tinyint,OTHJIN1.CodeKbn)         = HAISHA.OthJinKbn1
															And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
															AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
							LEFT JOIN
											VPM_CodeKb AS OTHJIN2
											ON
															CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
															AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
															AND OTHJIN2.CodeSyu     = 'OTHJINKBN'
							LEFT JOIN
											VPM_Tokisk AS TOKISK
											ON
															TOKISK.TokuiSeq = YYKSHO.TokuiSeq
							LEFT JOIN
											VPM_TokiSt AS TOKIST
											ON
															TOKIST.TokuiSeq        = YYKSHO.TokuiSeq
															AND TOKIST.SitenCdSeq  = YYKSHO.SitenCdSeq
															AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd
															AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd
							LEFT JOIN
											VPM_Eigyos AS EIGYOSHO
											ON
															EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
							LEFT JOIN
											VPM_Compny AS KAISHA
											ON
															KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
															AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
							--LEFT OUTER JOIN
							--				(
							--						  SELECT
							--									FUTTUM.HasYmd
							--								  , FUTTUM.UkeNo
							--								  , FUTTUM.UnkRen
							--								  , FUTTUM.FutTumKbn
							--								  , FUTTUM.FutTumRen
							--								  , FUTTUM.FutTumNm
							--								  , FUTTUM.SeisanNm
							--								  , FUTTUM.FutTumCdSeq
							--								  , MFUTTU.Suryo
							--								  , MFUTTU.TeiDanNo
							--								  , MFUTTU.BunkRen
							--						  FROM
							--									TKD_MFutTu AS MFUTTU
							--									LEFT JOIN
							--											  TKD_FutTum AS FUTTUM
							--											  ON
							--														FUTTUM.UkeNo         = MFUTTU.UkeNo
							--														AND FUTTUM.UnkRen    = MFUTTU.UnkRen
							--														AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
							--														AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
							--						  WHERE
							--									MFUTTU.UkeNo IN
							--									(
							--										   select
							--												  UkeNo
							--										   from
							--												  TKD_Haisha
							--										   where
							--												  SyuKoYmd   = @SyuKoYmd
							--												  and SiyoKbn=1
							--									) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
							--									AND MFUTTU.SiyoKbn = 1
							--									AND FUTTUM.SiyoKbn = 1
							--				)
							--				incidental
							--				ON
							--								incidental.UkeNo = HAISHA.UkeNo
			WHERE
							concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))          	
							AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
							AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       
							AND HAISHA.SiyoKbn     = 1
							AND YoyaSyu			  = 1
							
			ORDER BY
							 CASE @SortOrder
								  WHEN 1
											 THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 2
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
					   END ASC, CASE @SortOrder
								  WHEN 3
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 4
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
					   END ASC
		if(@FormOutput=2)
		SELECT
							ROW_NUMBER() OVER(ORDER BY
							 CASE @SortOrder
								  WHEN 1
											 THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 2
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
					   END ASC, CASE @SortOrder
								  WHEN 3
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 4
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
					   END ASC)                                                                              AS Row_Num
						  , YYKSHO.UkeCd                                                                          AS 'YYKSHO_UkeCd'
						  , YYKSHO.TokuiTel                                                                       AS 'YYKSHO_TokuiTel'
						  , YYKSHO.TokuiTanNm                                                                     AS 'YYKSHO_TokuiTanNm'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'UNKOBI_HaiSYmd'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'UNKOBI_TouYmd'
						  , UNKOBI.DanTaNm                                                                        AS 'UNKOBI_DanTaNm'
						  , UNKOBI.KanjJyus1                                                                      AS 'UNKOBI_KanjJyus1'
						  , UNKOBI.KanjJyus2                                                                      AS 'UNKOBI_KanjJyus2'
						  , UNKOBI.KanjTel                                                                        AS 'UNKOBI_KanjTel'
						  , UNKOBI.KanJNm                                                                         AS 'UNKOBI_KanJNm'
						  , HAISHA.UkeNo                                                                          AS 'HAISHA_UkeNo'
						  , HAISHA.UnkRen                                                                         AS 'HAISHA_UnkRen'
						  , HAISHA.SyaSyuRen                                                                      AS 'HAISHA_SyaSyuRen'
						  , HAISHA.TeiDanNo                                                                       AS 'HAISHA_TeiDanNo'
						  , HAISHA.BunkRen                                                                        AS 'HAISHA_BunkRen'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'HAISHA_HaiSYmd'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS 'HAISHA_HaiSTime'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'HAISHA_TouYmd'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS 'HAISHA_TouChTime'
						  , HAISHA.KikYmd                                                                         AS 'HAISHA_KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS 'HAISHA_KikTime'
						  , HAISHA.DanTaNm2                                                                       AS 'HAISHA_DanTaNm2'
						  , HAISHA.OthJinKbn1                                                                     AS 'HAISHA_OthJinKbn1'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'OTHJIN1_CodeKbnNm'
						  , HAISHA.OthJin1                                                                        AS 'HAISHA_OthJin1'
						  , HAISHA.OthJinKbn2                                                                     AS 'HAISHA_OthJinKbn2'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'OTHJIN2_CodeKbnNm'
						  , HAISHA.OthJin2                                                                        AS 'HAISHA_OthJin2'
						  , HAISHA.HaiSNm                                                                         AS 'HAISHA_HaiSNm'
						  , HAISHA.HaiSJyus1                                                                      AS 'HAISHA_HaiSJyus1'
						  , HAISHA.HaiSJyus2                                                                      AS 'HAISHA_HaiSJyus2'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS 'HAISHA_HaiSSetTime'
						  , HAISHA.IkNm                                                                           AS 'HAISHA_IkNm'
						  , HAISHA.SyuKoYmd                                                                       AS 'HAISHA_SyuKoYmd'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS 'HAISHA_SyuKoTime'
						  , HAISHA.SyuEigCdSeq                                                                    AS 'HAISHA_SyuEigCdSeq'
						  , EIGYOSHO.EigyoCd                                                                      AS 'EIGYOSHO_EigyoCd'
						  , EIGYOSHO.EigyoNm                                                                      AS 'EIGYOSHO_EigyoNm'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS 'HAISHA_SyuPaTime'
						  , HAISHA.JyoSyaJin                                                                      AS 'HAISHA_JyoSyaJin'
						  , HAISHA.PlusJin                                                                        AS 'HAISHA_PlusJin'
						  , HAISHA.GoSya                                                                          AS 'HAISHA_GoSya'
						  , HAISHA.HaiSKouKNm                                                                     AS 'HAISHA_HaiSKouKNm'
						  , HAISHA.HaiSBinNm                                                                      AS 'HAISHA_HaiSBinNm'
						  , HAISHA.TouSKouKNm                                                                     AS 'HAISHA_TouSKouKNm'
						  , HAISHA.TouSBinNm                                                                      AS 'HAISHA_TouSBinNm'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS 'HAISHA_TouSetTime'
						  , HAISHA.TouNm                                                                          AS 'HAISHA_TouNm'
						  , HAISHA.TouJyusyo1                                                                     AS 'HAISHA_TouJyusyo1'
						  , HAISHA.TouJyusyo2                                                                     AS 'HAISHA_TouJyusyo2'
						  , YYKSYU.SyaSyuDai                                                                      AS 'YYKSYU_SyaSyuDai'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS 'SYASYU_SYARYO_SyaSyuNm'
						  , SYARYO.SyaRyoCd                                                                       AS 'SYARYO_SyaRyoCd'
						  , SYARYO.SyaRyoNm                                                                       AS 'SYARYO_SyaRyoNm'
						  , HENSYA.TenkoNo                                                                        AS 'HENSYA_TenkoNo'
						  , SYARYO.KariSyaRyoNm                                                                   AS 'SYARYO_KariSyaRyoNm'
						  , TOKISK.TokuiNm                                                                        AS 'TOKISK_TokuiNm'
						  , TOKIST.SitenNm                                                                        AS 'TOKIST_SitenNm'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS 'YYKSYU_SyaSyuDaisum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
						  --, incidental.UnkRen                                                                 AS '�^�s���A��'
						  --, incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
						  --, incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
						  --, incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
						  --, incidental.SeisanNm                                                               AS '���Z��'
						  --, incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
						  --, incidental.Suryo                                                                  AS '����'
						  --, incidental.TeiDanNo                                                               AS '��c�ԍ�'
						  --, incidental.BunkRen                                                                AS '�����A��'
						  , HAISHA.BikoNm                                                                     as 'HAISHA_BikoNm'
						  , STUFF(
							(
									  SELECT
												',' + SYAIN.SyainNm
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as DriverList
						  , STUFF(
							(
									  SELECT
												',' + CAST(SYOKUM.SyokumuNm as nvarchar(20))
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn    = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as GuiderList
							, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=1
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=1
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+CAST( MFUTTU.Suryo AS varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=1
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSuryo
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=2
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.FutTumNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=2
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsFutTumNm
							,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=2
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+Cast(MFUTTU.Suryo as varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															and FUTTUM.FutTumKbn=2
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSuryo
			FROM
							TKD_Haisha AS HAISHA
							LEFT JOIN
											TKD_Unkobi AS UNKOBI
											ON
															UNKOBI.UkeNo      = HAISHA.UkeNo
															AND UNKOBI.UnkRen = HAISHA.UnkRen
							INNER JOIN
											TKD_Yyksho AS YYKSHO
											ON
															YYKSHO.UkeNo           = HAISHA.UkeNo
															AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq           	
							LEFT JOIN
											TKD_YykSyu AS YYKSYU
											ON
															YYKSYU.UkeNo        = HAISHA.UkeNo
															AND YYKSYU.UnkRen   = HAISHA.UnkRen
															AND YYKSYU.SyaSyuRen=HAISHA.SyaSyuRen
															And YYKSYU.SiyoKbn  =1
							LEFT JOIN
											VPM_SyaSyu AS SYASYU
											ON
															SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
															and SYASYU.TenantCdSeq=@TenantCdSeq
							LEFT JOIN
											VPM_SyaRyo AS SYARYO
											ON
															SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
							LEFT JOIN
                                        VPM_SyaSyu AS SYASYU_SYARYO
                                        ON
                                                        SYARYO.SyaSyuCdSeq = SYASYU_SYARYO.SyaSyuCdSeq
														and SYASYU_SYARYO.TenantCdSeq=@TenantCdSeq
							LEFT JOIN
											VPM_HenSya AS HENSYA
											ON
															HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
															AND HENSYA.StaYmd <= HAISHA.HaiSYmd
															AND HENSYA.EndYmd >= HAISHA.HaiSYmd
							LEFT JOIN
											VPM_CodeKb AS OTHJIN1
											ON
															CONVERT(tinyint,OTHJIN1.CodeKbn)         = HAISHA.OthJinKbn1
															And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
															AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
							LEFT JOIN
											VPM_CodeKb AS OTHJIN2
											ON
															CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
															AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
															AND OTHJIN2.CodeSyu     = 'OTHJINKBN'
							LEFT JOIN
											VPM_Tokisk AS TOKISK
											ON
															TOKISK.TokuiSeq = YYKSHO.TokuiSeq
							LEFT JOIN
											VPM_TokiSt AS TOKIST
											ON
															TOKIST.TokuiSeq        = YYKSHO.TokuiSeq
															AND TOKIST.SitenCdSeq  = YYKSHO.SitenCdSeq
															AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd
															AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd
							LEFT JOIN
											VPM_Eigyos AS EIGYOSHO
											ON
															EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
							LEFT JOIN
											VPM_Compny AS KAISHA
											ON
															KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
															AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
							--LEFT OUTER JOIN
							--				(
							--						  SELECT
							--									FUTTUM.HasYmd
							--								  , FUTTUM.UkeNo
							--								  , FUTTUM.UnkRen
							--								  , FUTTUM.FutTumKbn
							--								  , FUTTUM.FutTumRen
							--								  , FUTTUM.FutTumNm
							--								  , FUTTUM.SeisanNm
							--								  , FUTTUM.FutTumCdSeq
							--								  , MFUTTU.Suryo
							--								  , MFUTTU.TeiDanNo
							--								  , MFUTTU.BunkRen
							--						  FROM
							--									TKD_MFutTu AS MFUTTU
							--									LEFT JOIN
							--											  TKD_FutTum AS FUTTUM
							--											  ON
							--														FUTTUM.UkeNo         = MFUTTU.UkeNo
							--														AND FUTTUM.UnkRen    = MFUTTU.UnkRen
							--														AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
							--														AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
							--						  WHERE
							--									MFUTTU.UkeNo IN
							--									(
							--										   select
							--												  UkeNo
							--										   from
							--												  TKD_Haisha
							--										   where
							--												  SyuKoYmd   = @SyuKoYmd
							--												  and SiyoKbn=1
							--									) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
							--									AND MFUTTU.SiyoKbn = 1
							--									AND FUTTUM.SiyoKbn = 1
							--				)
							--				incidental
							--				ON
							--								incidental.UkeNo = HAISHA.UkeNo
			WHERE
							concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000'),FORMAT(HAISHA.TeiDanNo, '000'),FORMAT(HAISHA.BunkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))            	
							AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
							AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       
							AND HAISHA.SiyoKbn     = 1
							AND YoyaSyu			  = 1
							
			ORDER BY
							 CASE @SortOrder
								  WHEN 1
											 THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 2
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
					   END ASC, CASE @SortOrder
								  WHEN 3
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
					   END ASC, CASE @SortOrder
								  WHEN 4
											 THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
					   END ASC
    else IF (@UkeCdFrom != @UkeCdTo
        AND
        @SyuKoYmd!=''
        and
        @SyuEigCdSeq!=0)
        SELECT
                        ROW_NUMBER() OVER(ORDER BY
                         CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC)                                                                              AS Row_Num
                      , YYKSHO.UkeCd                                                                          AS 'YYKSHO_UkeCd'
						  , YYKSHO.TokuiTel                                                                       AS 'YYKSHO_TokuiTel'
						  , YYKSHO.TokuiTanNm                                                                     AS 'YYKSHO_TokuiTanNm'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'UNKOBI_HaiSYmd'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'UNKOBI_TouYmd'
						  , UNKOBI.DanTaNm                                                                        AS 'UNKOBI_DanTaNm'
						  , UNKOBI.KanjJyus1                                                                      AS 'UNKOBI_KanjJyus1'
						  , UNKOBI.KanjJyus2                                                                      AS 'UNKOBI_KanjJyus2'
						  , UNKOBI.KanjTel                                                                        AS 'UNKOBI_KanjTel'
						  , UNKOBI.KanJNm                                                                         AS 'UNKOBI_KanJNm'
						  , HAISHA.UkeNo                                                                          AS 'HAISHA_UkeNo'
						  , HAISHA.UnkRen                                                                         AS 'HAISHA_UnkRen'
						  , HAISHA.SyaSyuRen                                                                      AS 'HAISHA_SyaSyuRen'
						  , HAISHA.TeiDanNo                                                                       AS 'HAISHA_TeiDanNo'
						  , HAISHA.BunkRen                                                                        AS 'HAISHA_BunkRen'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'HAISHA_HaiSYmd'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS 'HAISHA_HaiSTime'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'HAISHA_TouYmd'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS 'HAISHA_TouChTime'
						  , HAISHA.KikYmd                                                                         AS 'HAISHA_KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS 'HAISHA_KikTime'
						  , HAISHA.DanTaNm2                                                                       AS 'HAISHA_DanTaNm2'
						  , HAISHA.OthJinKbn1                                                                     AS 'HAISHA_OthJinKbn1'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'OTHJIN1_CodeKbnNm'
						  , HAISHA.OthJin1                                                                        AS 'HAISHA_OthJin1'
						  , HAISHA.OthJinKbn2                                                                     AS 'HAISHA_OthJinKbn2'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'OTHJIN2_CodeKbnNm'
						  , HAISHA.OthJin2                                                                        AS 'HAISHA_OthJin2'
						  , HAISHA.HaiSNm                                                                         AS 'HAISHA_HaiSNm'
						  , HAISHA.HaiSJyus1                                                                      AS 'HAISHA_HaiSJyus1'
						  , HAISHA.HaiSJyus2                                                                      AS 'HAISHA_HaiSJyus2'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS 'HAISHA_HaiSSetTime'
						  , HAISHA.IkNm                                                                           AS 'HAISHA_IkNm'
						  , HAISHA.SyuKoYmd                                                                       AS 'HAISHA_SyuKoYmd'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS 'HAISHA_SyuKoTime'
						  , HAISHA.SyuEigCdSeq                                                                    AS 'HAISHA_SyuEigCdSeq'
						  , EIGYOSHO.EigyoCd                                                                      AS 'EIGYOSHO_EigyoCd'
						  , EIGYOSHO.EigyoNm                                                                      AS 'EIGYOSHO_EigyoNm'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS 'HAISHA_SyuPaTime'
						  , HAISHA.JyoSyaJin                                                                      AS 'HAISHA_JyoSyaJin'
						  , HAISHA.PlusJin                                                                        AS 'HAISHA_PlusJin'
						  , HAISHA.GoSya                                                                          AS 'HAISHA_GoSya'
						  , HAISHA.HaiSKouKNm                                                                     AS 'HAISHA_HaiSKouKNm'
						  , HAISHA.HaiSBinNm                                                                      AS 'HAISHA_HaiSBinNm'
						  , HAISHA.TouSKouKNm                                                                     AS 'HAISHA_TouSKouKNm'
						  , HAISHA.TouSBinNm                                                                      AS 'HAISHA_TouSBinNm'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS 'HAISHA_TouSetTime'
						  , HAISHA.TouNm                                                                          AS 'HAISHA_TouNm'
						  , HAISHA.TouJyusyo1                                                                     AS 'HAISHA_TouJyusyo1'
						  , HAISHA.TouJyusyo2                                                                     AS 'HAISHA_TouJyusyo2'
						  , YYKSYU.SyaSyuDai                                                                      AS 'YYKSYU_SyaSyuDai'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS 'SYASYU_SYARYO_SyaSyuNm'
						  , SYARYO.SyaRyoCd                                                                       AS 'SYARYO_SyaRyoCd'
						  , SYARYO.SyaRyoNm                                                                       AS 'SYARYO_SyaRyoNm'
						  , HENSYA.TenkoNo                                                                        AS 'HENSYA_TenkoNo'
						  , SYARYO.KariSyaRyoNm                                                                   AS 'SYARYO_KariSyaRyoNm'
						  , TOKISK.TokuiNm                                                                        AS 'TOKISK_TokuiNm'
						  , TOKIST.SitenNm                                                                        AS 'TOKIST_SitenNm'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS 'YYKSYU_SyaSyuDaisum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
						  --, incidental.UnkRen                                                                 AS '�^�s���A��'
						  --, incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
						  --, incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
						  --, incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
						  --, incidental.SeisanNm                                                               AS '���Z��'
						  --, incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
						  --, incidental.Suryo                                                                  AS '����'
						  --, incidental.TeiDanNo                                                               AS '��c�ԍ�'
						  --, incidental.BunkRen                                                                AS '�����A��'
						  , HAISHA.BikoNm                                                                     as 'HAISHA_BikoNm'
						  , STUFF(
							(
									  SELECT
												',' + SYAIN.SyainNm
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as DriverList
						  , STUFF(
							(
									  SELECT
												',' + CAST(SYOKUM.SyokumuNm as nvarchar(20))
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn    = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as GuiderList
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+CAST( MFUTTU.Suryo AS varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSuryo
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.FutTumNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsFutTumNm
						,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+Cast(MFUTTU.Suryo as varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSuryo
        FROM
                        TKD_Haisha AS HAISHA
                        LEFT JOIN
                                        TKD_Unkobi AS UNKOBI
                                        ON
                                                        UNKOBI.UkeNo      = HAISHA.UkeNo
                                                        AND UNKOBI.UnkRen = HAISHA.UnkRen
                        INNER JOIN
                                        TKD_Yyksho AS YYKSHO
                                        ON
                                                        YYKSHO.UkeNo           = HAISHA.UkeNo
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq           	
                        LEFT JOIN
                                        TKD_YykSyu AS YYKSYU
                                        ON
                                                        YYKSYU.UkeNo        = HAISHA.UkeNo
                                                        AND YYKSYU.UnkRen   = HAISHA.UnkRen
                                                        AND YYKSYU.SyaSyuRen=HAISHA.SyaSyuRen
                                                        And YYKSYU.SiyoKbn  =1
                        LEFT JOIN
                                        VPM_SyaSyu AS SYASYU
                                        ON
                                                        SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
														and SYASYU.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
						LEFT JOIN
                                        VPM_SyaSyu AS SYASYU_SYARYO
                                        ON
                                                        SYARYO.SyaSyuCdSeq = SYASYU_SYARYO.SyaSyuCdSeq
														and SYASYU_SYARYO.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        CONVERT(tinyint,OTHJIN1.CodeKbn)         = HAISHA.OthJinKbn1
                                                        And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN2.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_Tokisk AS TOKISK
                                        ON
                                                        TOKISK.TokuiSeq = YYKSHO.TokuiSeq
                        LEFT JOIN
                                        VPM_TokiSt AS TOKIST
                                        ON
                                                        TOKIST.TokuiSeq        = YYKSHO.TokuiSeq
                                                        AND TOKIST.SitenCdSeq  = YYKSHO.SitenCdSeq
                                                        AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd
                                                        AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_Eigyos AS EIGYOSHO
                                        ON
                                                        EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                        LEFT JOIN
                                        VPM_Compny AS KAISHA
                                        ON
                                                        KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                        --LEFT OUTER JOIN
                        --                (
                        --                          SELECT
                        --                                    FUTTUM.HasYmd
                        --                                  , FUTTUM.UkeNo
                        --                                  , FUTTUM.UnkRen
                        --                                  , FUTTUM.FutTumKbn
                        --                                  , FUTTUM.FutTumRen
                        --                                  , FUTTUM.FutTumNm
                        --                                  , FUTTUM.SeisanNm
                        --                                  , FUTTUM.FutTumCdSeq
                        --                                  , MFUTTU.Suryo
                        --                                  , MFUTTU.TeiDanNo
                        --                                  , MFUTTU.BunkRen
                        --                          FROM
                        --                                    TKD_MFutTu AS MFUTTU
                        --                                    LEFT JOIN
                        --                                              TKD_FutTum AS FUTTUM
                        --                                              ON
                        --                                                        FUTTUM.UkeNo         = MFUTTU.UkeNo
                        --                                                        AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                        --                                                        AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                        --                                                        AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                        --                          WHERE
                        --                                    MFUTTU.UkeNo IN
                        --                                    (
                        --                                           select
                        --                                                  UkeNo
                        --                                           from
                        --                                                  TKD_Haisha
                        --                                           where
                        --                                                  SyuKoYmd   = @SyuKoYmd
                        --                                                  and SiyoKbn=1
                        --                                    ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --��ʂ̎�t�ԍ��iFROM�j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --��ʂ̎�t�ԍ��iTO�j            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j            	
                        AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
                        AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq
                        and HAISHA.SyuEigCdSeq = @SyuEigCdSeq
                        AND HAISHA.SiyoKbn     = 1
						AND YoyaSyu			  = 1
						
        ORDER BY
                         CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC 
	else if(@UkeCdFrom != @UkeCdTo
                        AND @SyuKoYmd              !=''
                        and @SyuEigCdSeq            =0)
        SELECT
                        ROW_NUMBER() OVER(ORDER BY
                          CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC)                                                                              AS Row_Num
                     , YYKSHO.UkeCd                                                                          AS 'YYKSHO_UkeCd'
						  , YYKSHO.TokuiTel                                                                       AS 'YYKSHO_TokuiTel'
						  , YYKSHO.TokuiTanNm                                                                     AS 'YYKSHO_TokuiTanNm'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'UNKOBI_HaiSYmd'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'UNKOBI_TouYmd'
						  , UNKOBI.DanTaNm                                                                        AS 'UNKOBI_DanTaNm'
						  , UNKOBI.KanjJyus1                                                                      AS 'UNKOBI_KanjJyus1'
						  , UNKOBI.KanjJyus2                                                                      AS 'UNKOBI_KanjJyus2'
						  , UNKOBI.KanjTel                                                                        AS 'UNKOBI_KanjTel'
						  , UNKOBI.KanJNm                                                                         AS 'UNKOBI_KanJNm'
						  , HAISHA.UkeNo                                                                          AS 'HAISHA_UkeNo'
						  , HAISHA.UnkRen                                                                         AS 'HAISHA_UnkRen'
						  , HAISHA.SyaSyuRen                                                                      AS 'HAISHA_SyaSyuRen'
						  , HAISHA.TeiDanNo                                                                       AS 'HAISHA_TeiDanNo'
						  , HAISHA.BunkRen                                                                        AS 'HAISHA_BunkRen'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'HAISHA_HaiSYmd'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS 'HAISHA_HaiSTime'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'HAISHA_TouYmd'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS 'HAISHA_TouChTime'
						  , HAISHA.KikYmd                                                                         AS 'HAISHA_KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS 'HAISHA_KikTime'
						  , HAISHA.DanTaNm2                                                                       AS 'HAISHA_DanTaNm2'
						  , HAISHA.OthJinKbn1                                                                     AS 'HAISHA_OthJinKbn1'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'OTHJIN1_CodeKbnNm'
						  , HAISHA.OthJin1                                                                        AS 'HAISHA_OthJin1'
						  , HAISHA.OthJinKbn2                                                                     AS 'HAISHA_OthJinKbn2'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'OTHJIN2_CodeKbnNm'
						  , HAISHA.OthJin2                                                                        AS 'HAISHA_OthJin2'
						  , HAISHA.HaiSNm                                                                         AS 'HAISHA_HaiSNm'
						  , HAISHA.HaiSJyus1                                                                      AS 'HAISHA_HaiSJyus1'
						  , HAISHA.HaiSJyus2                                                                      AS 'HAISHA_HaiSJyus2'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS 'HAISHA_HaiSSetTime'
						  , HAISHA.IkNm                                                                           AS 'HAISHA_IkNm'
						  , HAISHA.SyuKoYmd                                                                       AS 'HAISHA_SyuKoYmd'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS 'HAISHA_SyuKoTime'
						  , HAISHA.SyuEigCdSeq                                                                    AS 'HAISHA_SyuEigCdSeq'
						  , EIGYOSHO.EigyoCd                                                                      AS 'EIGYOSHO_EigyoCd'
						  , EIGYOSHO.EigyoNm                                                                      AS 'EIGYOSHO_EigyoNm'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS 'HAISHA_SyuPaTime'
						  , HAISHA.JyoSyaJin                                                                      AS 'HAISHA_JyoSyaJin'
						  , HAISHA.PlusJin                                                                        AS 'HAISHA_PlusJin'
						  , HAISHA.GoSya                                                                          AS 'HAISHA_GoSya'
						  , HAISHA.HaiSKouKNm                                                                     AS 'HAISHA_HaiSKouKNm'
						  , HAISHA.HaiSBinNm                                                                      AS 'HAISHA_HaiSBinNm'
						  , HAISHA.TouSKouKNm                                                                     AS 'HAISHA_TouSKouKNm'
						  , HAISHA.TouSBinNm                                                                      AS 'HAISHA_TouSBinNm'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS 'HAISHA_TouSetTime'
						  , HAISHA.TouNm                                                                          AS 'HAISHA_TouNm'
						  , HAISHA.TouJyusyo1                                                                     AS 'HAISHA_TouJyusyo1'
						  , HAISHA.TouJyusyo2                                                                     AS 'HAISHA_TouJyusyo2'
						  , YYKSYU.SyaSyuDai                                                                      AS 'YYKSYU_SyaSyuDai'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS 'SYASYU_SYARYO_SyaSyuNm'
						  , SYARYO.SyaRyoCd                                                                       AS 'SYARYO_SyaRyoCd'
						  , SYARYO.SyaRyoNm                                                                       AS 'SYARYO_SyaRyoNm'
						  , HENSYA.TenkoNo                                                                        AS 'HENSYA_TenkoNo'
						  , SYARYO.KariSyaRyoNm                                                                   AS 'SYARYO_KariSyaRyoNm'
						  , TOKISK.TokuiNm                                                                        AS 'TOKISK_TokuiNm'
						  , TOKIST.SitenNm                                                                        AS 'TOKIST_SitenNm'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS 'YYKSYU_SyaSyuDaisum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
						  --, incidental.UnkRen                                                                 AS '�^�s���A��'
						  --, incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
						  --, incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
						  --, incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
						  --, incidental.SeisanNm                                                               AS '���Z��'
						  --, incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
						  --, incidental.Suryo                                                                  AS '����'
						  --, incidental.TeiDanNo                                                               AS '��c�ԍ�'
						  --, incidental.BunkRen                                                                AS '�����A��'
						  , HAISHA.BikoNm                                                                     as 'HAISHA_BikoNm'
						  , STUFF(
							(
									  SELECT
												',' + SYAIN.SyainNm
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as DriverList
						  , STUFF(
							(
									  SELECT
												',' + CAST(SYOKUM.SyokumuNm as nvarchar(20))
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn    = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as GuiderList
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+CAST( MFUTTU.Suryo AS varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSuryo
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.FutTumNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsFutTumNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+Cast(MFUTTU.Suryo as varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSuryo
        FROM
                        TKD_Haisha AS HAISHA
                        LEFT JOIN
                                        TKD_Unkobi AS UNKOBI
                                        ON
                                                        UNKOBI.UkeNo      = HAISHA.UkeNo
                                                        AND UNKOBI.UnkRen = HAISHA.UnkRen
                        INNER JOIN
                                        TKD_Yyksho AS YYKSHO
                                        ON
                                                        YYKSHO.UkeNo           = HAISHA.UkeNo
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq           	
                        LEFT JOIN
                                        TKD_YykSyu AS YYKSYU
                                        ON
                                                        YYKSYU.UkeNo        = HAISHA.UkeNo
                                                        AND YYKSYU.UnkRen   = HAISHA.UnkRen
                                                        AND YYKSYU.SyaSyuRen=HAISHA.SyaSyuRen
                                                        And YYKSYU.SiyoKbn  =1
                        LEFT JOIN
                                        VPM_SyaSyu AS SYASYU
                                        ON
                                                        SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
														and SYASYU.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
						LEFT JOIN
                                        VPM_SyaSyu AS SYASYU_SYARYO
                                        ON
                                                        SYARYO.SyaSyuCdSeq = SYASYU_SYARYO.SyaSyuCdSeq
														and SYASYU_SYARYO.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        CONVERT(tinyint,OTHJIN1.CodeKbn)         = HAISHA.OthJinKbn1
                                                        AND OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN2.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_Tokisk AS TOKISK
                                        ON
                                                        TOKISK.TokuiSeq        = YYKSHO.TokuiSeq
                                                        AND TOKISK.TenantCdSeq = @TenantCdSeq
                        LEFT JOIN
                                        VPM_TokiSt AS TOKIST
                                        ON
                                                        TOKIST.TokuiSeq        = YYKSHO.TokuiSeq
                                                        AND TOKIST.SitenCdSeq  = YYKSHO.SitenCdSeq
                                                        AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd
                                                        AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_Eigyos AS EIGYOSHO
                                        ON
                                                        EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                        LEFT JOIN
                                        VPM_Compny AS KAISHA
                                        ON
                                                        KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                        --LEFT OUTER JOIN
                        --                (
                        --                          SELECT
                        --                                    FUTTUM.HasYmd
                        --                                  , FUTTUM.UkeNo
                        --                                  , FUTTUM.UnkRen
                        --                                  , FUTTUM.FutTumKbn
                        --                                  , FUTTUM.FutTumRen
                        --                                  , FUTTUM.FutTumNm
                        --                                  , FUTTUM.SeisanNm
                        --                                  , FUTTUM.FutTumCdSeq
                        --                                  , MFUTTU.Suryo
                        --                                  , MFUTTU.TeiDanNo
                        --                                  , MFUTTU.BunkRen
                        --                          FROM
                        --                                    TKD_MFutTu AS MFUTTU
                        --                                    LEFT JOIN
                        --                                              TKD_FutTum AS FUTTUM
                        --                                              ON
                        --                                                        FUTTUM.UkeNo         = MFUTTU.UkeNo
                        --                                                        AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                        --                                                        AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                        --                                                        AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                        --                          WHERE
                        --                                    MFUTTU.UkeNo IN
                        --                                    (
                        --                                           select
                        --                                                  UkeNo
                        --                                           from
                        --                                                  TKD_Haisha
                        --                                           where
                        --                                                  SyuKoYmd   = @SyuKoYmd
                        --                                                  and SiyoKbn=1
                        --                                    ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --��ʂ̎�t�ԍ��iFROM�j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --��ʂ̎�t�ԍ��iTO�j            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j            	
                        AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
                        AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq
                        AND HAISHA.SiyoKbn     = 1
						AND YoyaSyu			  = 1
						
        ORDER BY
                          CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC 


				   else IF (@UkeCdFrom = @UkeCdTo
        AND
        @SyuKoYmd!=''
        and
        @SyuEigCdSeq!=0)
        SELECT
                        ROW_NUMBER() OVER(ORDER BY
                         CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC)                                                                              AS Row_Num
                      , YYKSHO.UkeCd                                                                          AS 'YYKSHO_UkeCd'
						  , YYKSHO.TokuiTel                                                                       AS 'YYKSHO_TokuiTel'
						  , YYKSHO.TokuiTanNm                                                                     AS 'YYKSHO_TokuiTanNm'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'UNKOBI_HaiSYmd'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'UNKOBI_TouYmd'
						  , UNKOBI.DanTaNm                                                                        AS 'UNKOBI_DanTaNm'
						  , UNKOBI.KanjJyus1                                                                      AS 'UNKOBI_KanjJyus1'
						  , UNKOBI.KanjJyus2                                                                      AS 'UNKOBI_KanjJyus2'
						  , UNKOBI.KanjTel                                                                        AS 'UNKOBI_KanjTel'
						  , UNKOBI.KanJNm                                                                         AS 'UNKOBI_KanJNm'
						  , HAISHA.UkeNo                                                                          AS 'HAISHA_UkeNo'
						  , HAISHA.UnkRen                                                                         AS 'HAISHA_UnkRen'
						  , HAISHA.SyaSyuRen                                                                      AS 'HAISHA_SyaSyuRen'
						  , HAISHA.TeiDanNo                                                                       AS 'HAISHA_TeiDanNo'
						  , HAISHA.BunkRen                                                                        AS 'HAISHA_BunkRen'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'HAISHA_HaiSYmd'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS 'HAISHA_HaiSTime'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'HAISHA_TouYmd'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS 'HAISHA_TouChTime'
						  , HAISHA.KikYmd                                                                         AS 'HAISHA_KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS 'HAISHA_KikTime'
						  , HAISHA.DanTaNm2                                                                       AS 'HAISHA_DanTaNm2'
						  , HAISHA.OthJinKbn1                                                                     AS 'HAISHA_OthJinKbn1'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'OTHJIN1_CodeKbnNm'
						  , HAISHA.OthJin1                                                                        AS 'HAISHA_OthJin1'
						  , HAISHA.OthJinKbn2                                                                     AS 'HAISHA_OthJinKbn2'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'OTHJIN2_CodeKbnNm'
						  , HAISHA.OthJin2                                                                        AS 'HAISHA_OthJin2'
						  , HAISHA.HaiSNm                                                                         AS 'HAISHA_HaiSNm'
						  , HAISHA.HaiSJyus1                                                                      AS 'HAISHA_HaiSJyus1'
						  , HAISHA.HaiSJyus2                                                                      AS 'HAISHA_HaiSJyus2'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS 'HAISHA_HaiSSetTime'
						  , HAISHA.IkNm                                                                           AS 'HAISHA_IkNm'
						  , HAISHA.SyuKoYmd                                                                       AS 'HAISHA_SyuKoYmd'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS 'HAISHA_SyuKoTime'
						  , HAISHA.SyuEigCdSeq                                                                    AS 'HAISHA_SyuEigCdSeq'
						  , EIGYOSHO.EigyoCd                                                                      AS 'EIGYOSHO_EigyoCd'
						  , EIGYOSHO.EigyoNm                                                                      AS 'EIGYOSHO_EigyoNm'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS 'HAISHA_SyuPaTime'
						  , HAISHA.JyoSyaJin                                                                      AS 'HAISHA_JyoSyaJin'
						  , HAISHA.PlusJin                                                                        AS 'HAISHA_PlusJin'
						  , HAISHA.GoSya                                                                          AS 'HAISHA_GoSya'
						  , HAISHA.HaiSKouKNm                                                                     AS 'HAISHA_HaiSKouKNm'
						  , HAISHA.HaiSBinNm                                                                      AS 'HAISHA_HaiSBinNm'
						  , HAISHA.TouSKouKNm                                                                     AS 'HAISHA_TouSKouKNm'
						  , HAISHA.TouSBinNm                                                                      AS 'HAISHA_TouSBinNm'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS 'HAISHA_TouSetTime'
						  , HAISHA.TouNm                                                                          AS 'HAISHA_TouNm'
						  , HAISHA.TouJyusyo1                                                                     AS 'HAISHA_TouJyusyo1'
						  , HAISHA.TouJyusyo2                                                                     AS 'HAISHA_TouJyusyo2'
						  , YYKSYU.SyaSyuDai                                                                      AS 'YYKSYU_SyaSyuDai'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS 'SYASYU_SYARYO_SyaSyuNm'
						  , SYARYO.SyaRyoCd                                                                       AS 'SYARYO_SyaRyoCd'
						  , SYARYO.SyaRyoNm                                                                       AS 'SYARYO_SyaRyoNm'
						  , HENSYA.TenkoNo                                                                        AS 'HENSYA_TenkoNo'
						  , SYARYO.KariSyaRyoNm                                                                   AS 'SYARYO_KariSyaRyoNm'
						  , TOKISK.TokuiNm                                                                        AS 'TOKISK_TokuiNm'
						  , TOKIST.SitenNm                                                                        AS 'TOKIST_SitenNm'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS 'YYKSYU_SyaSyuDaisum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
						  --, incidental.UnkRen                                                                 AS '�^�s���A��'
						  --, incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
						  --, incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
						  --, incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
						  --, incidental.SeisanNm                                                               AS '���Z��'
						  --, incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
						  --, incidental.Suryo                                                                  AS '����'
						  --, incidental.TeiDanNo                                                               AS '��c�ԍ�'
						  --, incidental.BunkRen                                                                AS '�����A��'
						  , HAISHA.BikoNm                                                                     as 'HAISHA_BikoNm'
						  , STUFF(
							(
									  SELECT
												',' + SYAIN.SyainNm
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as DriverList
						  , STUFF(
							(
									  SELECT
												',' + CAST(SYOKUM.SyokumuNm as nvarchar(20))
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn    = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as GuiderList
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+CAST( MFUTTU.Suryo AS varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSuryo
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.FutTumNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsFutTumNm
						,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+Cast(MFUTTU.Suryo as varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSuryo
        FROM
                        TKD_Haisha AS HAISHA
                        LEFT JOIN
                                        TKD_Unkobi AS UNKOBI
                                        ON
                                                        UNKOBI.UkeNo      = HAISHA.UkeNo
                                                        AND UNKOBI.UnkRen = HAISHA.UnkRen
                        INNER JOIN
                                        TKD_Yyksho AS YYKSHO
                                        ON
                                                        YYKSHO.UkeNo           = HAISHA.UkeNo
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq           	
                        LEFT JOIN
                                        TKD_YykSyu AS YYKSYU
                                        ON
                                                        YYKSYU.UkeNo        = HAISHA.UkeNo
                                                        AND YYKSYU.UnkRen   = HAISHA.UnkRen
                                                        AND YYKSYU.SyaSyuRen=HAISHA.SyaSyuRen
                                                        And YYKSYU.SiyoKbn  =1
                        LEFT JOIN
                                        VPM_SyaSyu AS SYASYU
                                        ON
                                                        SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
														and SYASYU.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
						LEFT JOIN
                                        VPM_SyaSyu AS SYASYU_SYARYO
                                        ON
                                                        SYARYO.SyaSyuCdSeq = SYASYU_SYARYO.SyaSyuCdSeq
														and SYASYU_SYARYO.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        CONVERT(tinyint,OTHJIN1.CodeKbn)         = HAISHA.OthJinKbn1
                                                        And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN2.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_Tokisk AS TOKISK
                                        ON
                                                        TOKISK.TokuiSeq = YYKSHO.TokuiSeq
                        LEFT JOIN
                                        VPM_TokiSt AS TOKIST
                                        ON
                                                        TOKIST.TokuiSeq        = YYKSHO.TokuiSeq
                                                        AND TOKIST.SitenCdSeq  = YYKSHO.SitenCdSeq
                                                        AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd
                                                        AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_Eigyos AS EIGYOSHO
                                        ON
                                                        EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                        LEFT JOIN
                                        VPM_Compny AS KAISHA
                                        ON
                                                        KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                        --LEFT OUTER JOIN
                        --                (
                        --                          SELECT
                        --                                    FUTTUM.HasYmd
                        --                                  , FUTTUM.UkeNo
                        --                                  , FUTTUM.UnkRen
                        --                                  , FUTTUM.FutTumKbn
                        --                                  , FUTTUM.FutTumRen
                        --                                  , FUTTUM.FutTumNm
                        --                                  , FUTTUM.SeisanNm
                        --                                  , FUTTUM.FutTumCdSeq
                        --                                  , MFUTTU.Suryo
                        --                                  , MFUTTU.TeiDanNo
                        --                                  , MFUTTU.BunkRen
                        --                          FROM
                        --                                    TKD_MFutTu AS MFUTTU
                        --                                    LEFT JOIN
                        --                                              TKD_FutTum AS FUTTUM
                        --                                              ON
                        --                                                        FUTTUM.UkeNo         = MFUTTU.UkeNo
                        --                                                        AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                        --                                                        AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                        --                                                        AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                        --                          WHERE
                        --                                    MFUTTU.UkeNo IN
                        --                                    (
                        --                                           select
                        --                                                  UkeNo
                        --                                           from
                        --                                                  TKD_Haisha
                        --                                           where
                        --                                                  SyuKoYmd   = @SyuKoYmd
                        --                                                  and SiyoKbn=1
                        --                                    ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --��ʂ̎�t�ԍ��iFROM�j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --��ʂ̎�t�ԍ��iTO�j            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j            	
                        AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
                        AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq
                        and HAISHA.SyuEigCdSeq = @SyuEigCdSeq
                        AND HAISHA.SiyoKbn     = 1
						AND YoyaSyu			  = 1
						
        ORDER BY
                         CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC 
	else if(@UkeCdFrom = @UkeCdTo
                        AND @SyuKoYmd              !=''
                        and @SyuEigCdSeq            =0)
        SELECT
                        ROW_NUMBER() OVER(ORDER BY
                          CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC)                                                                              AS Row_Num
                      , YYKSHO.UkeCd                                                                          AS 'YYKSHO_UkeCd'
						  , YYKSHO.TokuiTel                                                                       AS 'YYKSHO_TokuiTel'
						  , YYKSHO.TokuiTanNm                                                                     AS 'YYKSHO_TokuiTanNm'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'UNKOBI_HaiSYmd'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'UNKOBI_TouYmd'
						  , UNKOBI.DanTaNm                                                                        AS 'UNKOBI_DanTaNm'
						  , UNKOBI.KanjJyus1                                                                      AS 'UNKOBI_KanjJyus1'
						  , UNKOBI.KanjJyus2                                                                      AS 'UNKOBI_KanjJyus2'
						  , UNKOBI.KanjTel                                                                        AS 'UNKOBI_KanjTel'
						  , UNKOBI.KanJNm                                                                         AS 'UNKOBI_KanJNm'
						  , HAISHA.UkeNo                                                                          AS 'HAISHA_UkeNo'
						  , HAISHA.UnkRen                                                                         AS 'HAISHA_UnkRen'
						  , HAISHA.SyaSyuRen                                                                      AS 'HAISHA_SyaSyuRen'
						  , HAISHA.TeiDanNo                                                                       AS 'HAISHA_TeiDanNo'
						  , HAISHA.BunkRen                                                                        AS 'HAISHA_BunkRen'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'HAISHA_HaiSYmd'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS 'HAISHA_HaiSTime'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'HAISHA_TouYmd'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS 'HAISHA_TouChTime'
						  , HAISHA.KikYmd                                                                         AS 'HAISHA_KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS 'HAISHA_KikTime'
						  , HAISHA.DanTaNm2                                                                       AS 'HAISHA_DanTaNm2'
						  , HAISHA.OthJinKbn1                                                                     AS 'HAISHA_OthJinKbn1'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'OTHJIN1_CodeKbnNm'
						  , HAISHA.OthJin1                                                                        AS 'HAISHA_OthJin1'
						  , HAISHA.OthJinKbn2                                                                     AS 'HAISHA_OthJinKbn2'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'OTHJIN2_CodeKbnNm'
						  , HAISHA.OthJin2                                                                        AS 'HAISHA_OthJin2'
						  , HAISHA.HaiSNm                                                                         AS 'HAISHA_HaiSNm'
						  , HAISHA.HaiSJyus1                                                                      AS 'HAISHA_HaiSJyus1'
						  , HAISHA.HaiSJyus2                                                                      AS 'HAISHA_HaiSJyus2'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS 'HAISHA_HaiSSetTime'
						  , HAISHA.IkNm                                                                           AS 'HAISHA_IkNm'
						  , HAISHA.SyuKoYmd                                                                       AS 'HAISHA_SyuKoYmd'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS 'HAISHA_SyuKoTime'
						  , HAISHA.SyuEigCdSeq                                                                    AS 'HAISHA_SyuEigCdSeq'
						  , EIGYOSHO.EigyoCd                                                                      AS 'EIGYOSHO_EigyoCd'
						  , EIGYOSHO.EigyoNm                                                                      AS 'EIGYOSHO_EigyoNm'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS 'HAISHA_SyuPaTime'
						  , HAISHA.JyoSyaJin                                                                      AS 'HAISHA_JyoSyaJin'
						  , HAISHA.PlusJin                                                                        AS 'HAISHA_PlusJin'
						  , HAISHA.GoSya                                                                          AS 'HAISHA_GoSya'
						  , HAISHA.HaiSKouKNm                                                                     AS 'HAISHA_HaiSKouKNm'
						  , HAISHA.HaiSBinNm                                                                      AS 'HAISHA_HaiSBinNm'
						  , HAISHA.TouSKouKNm                                                                     AS 'HAISHA_TouSKouKNm'
						  , HAISHA.TouSBinNm                                                                      AS 'HAISHA_TouSBinNm'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS 'HAISHA_TouSetTime'
						  , HAISHA.TouNm                                                                          AS 'HAISHA_TouNm'
						  , HAISHA.TouJyusyo1                                                                     AS 'HAISHA_TouJyusyo1'
						  , HAISHA.TouJyusyo2                                                                     AS 'HAISHA_TouJyusyo2'
						  , YYKSYU.SyaSyuDai                                                                      AS 'YYKSYU_SyaSyuDai'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS 'SYASYU_SYARYO_SyaSyuNm'
						  , SYARYO.SyaRyoCd                                                                       AS 'SYARYO_SyaRyoCd'
						  , SYARYO.SyaRyoNm                                                                       AS 'SYARYO_SyaRyoNm'
						  , HENSYA.TenkoNo                                                                        AS 'HENSYA_TenkoNo'
						  , SYARYO.KariSyaRyoNm                                                                   AS 'SYARYO_KariSyaRyoNm'
						  , TOKISK.TokuiNm                                                                        AS 'TOKISK_TokuiNm'
						  , TOKIST.SitenNm                                                                        AS 'TOKIST_SitenNm'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS 'YYKSYU_SyaSyuDaisum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
						  --, incidental.UnkRen                                                                 AS '�^�s���A��'
						  --, incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
						  --, incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
						  --, incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
						  --, incidental.SeisanNm                                                               AS '���Z��'
						  --, incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
						  --, incidental.Suryo                                                                  AS '����'
						  --, incidental.TeiDanNo                                                               AS '��c�ԍ�'
						  --, incidental.BunkRen                                                                AS '�����A��'
						  , HAISHA.BikoNm                                                                     as 'HAISHA_BikoNm'
						  , STUFF(
							(
									  SELECT
												',' + SYAIN.SyainNm
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as DriverList
						  , STUFF(
							(
									  SELECT
												',' + CAST(SYOKUM.SyokumuNm as nvarchar(20))
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn    = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as GuiderList
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+CAST( MFUTTU.Suryo AS varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSuryo
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.FutTumNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsFutTumNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+Cast(MFUTTU.Suryo as varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSuryo
        FROM
                        TKD_Haisha AS HAISHA
                        LEFT JOIN
                                        TKD_Unkobi AS UNKOBI
                                        ON
                                                        UNKOBI.UkeNo      = HAISHA.UkeNo
                                                        AND UNKOBI.UnkRen = HAISHA.UnkRen
                        INNER JOIN
                                        TKD_Yyksho AS YYKSHO
                                        ON
                                                        YYKSHO.UkeNo           = HAISHA.UkeNo
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq           	
                        LEFT JOIN
                                        TKD_YykSyu AS YYKSYU
                                        ON
                                                        YYKSYU.UkeNo        = HAISHA.UkeNo
                                                        AND YYKSYU.UnkRen   = HAISHA.UnkRen
                                                        AND YYKSYU.SyaSyuRen=HAISHA.SyaSyuRen
                                                        And YYKSYU.SiyoKbn  =1
                        LEFT JOIN
                                        VPM_SyaSyu AS SYASYU
                                        ON
                                                        SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
														and SYASYU.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
						LEFT JOIN
                                        VPM_SyaSyu AS SYASYU_SYARYO
                                        ON
                                                        SYARYO.SyaSyuCdSeq = SYASYU_SYARYO.SyaSyuCdSeq
														and SYASYU_SYARYO.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        CONVERT(tinyint,OTHJIN1.CodeKbn)         = HAISHA.OthJinKbn1
                                                        AND OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                       CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN2.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_Tokisk AS TOKISK
                                        ON
                                                        TOKISK.TokuiSeq        = YYKSHO.TokuiSeq
                                                        AND TOKISK.TenantCdSeq = @TenantCdSeq
                        LEFT JOIN
                                        VPM_TokiSt AS TOKIST
                                        ON
                                                        TOKIST.TokuiSeq        = YYKSHO.TokuiSeq
                                                        AND TOKIST.SitenCdSeq  = YYKSHO.SitenCdSeq
                                                        AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd
                                                        AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_Eigyos AS EIGYOSHO
                                        ON
                                                        EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                        LEFT JOIN
                                        VPM_Compny AS KAISHA
                                        ON
                                                        KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                        --LEFT OUTER JOIN
                        --                (
                        --                          SELECT
                        --                                    FUTTUM.HasYmd
                        --                                  , FUTTUM.UkeNo
                        --                                  , FUTTUM.UnkRen
                        --                                  , FUTTUM.FutTumKbn
                        --                                  , FUTTUM.FutTumRen
                        --                                  , FUTTUM.FutTumNm
                        --                                  , FUTTUM.SeisanNm
                        --                                  , FUTTUM.FutTumCdSeq
                        --                                  , MFUTTU.Suryo
                        --                                  , MFUTTU.TeiDanNo
                        --                                  , MFUTTU.BunkRen
                        --                          FROM
                        --                                    TKD_MFutTu AS MFUTTU
                        --                                    LEFT JOIN
                        --                                              TKD_FutTum AS FUTTUM
                        --                                              ON
                        --                                                        FUTTUM.UkeNo         = MFUTTU.UkeNo
                        --                                                        AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                        --                                                        AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                        --                                                        AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                        --                          WHERE
                        --                                    MFUTTU.UkeNo IN
                        --                                    (
                        --                                           select
                        --                                                  UkeNo
                        --                                           from
                        --                                                  TKD_Haisha
                        --                                           where
                        --                                                  SyuKoYmd   = @SyuKoYmd
                        --                                                  and SiyoKbn=1
                        --                                    ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --��ʂ̎�t�ԍ��iFROM�j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --��ʂ̎�t�ԍ��iTO�j            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j            	
                        AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
                        AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq
                        AND HAISHA.SiyoKbn     = 1
						AND YoyaSyu			  = 1
						
        ORDER BY
                          CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC 

	ELSE
        SELECT
                        ROW_NUMBER() OVER(ORDER BY
                         CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC)                                                                              AS Row_Num
                      , YYKSHO.UkeCd                                                                          AS 'YYKSHO_UkeCd'
						  , YYKSHO.TokuiTel                                                                       AS 'YYKSHO_TokuiTel'
						  , YYKSHO.TokuiTanNm                                                                     AS 'YYKSHO_TokuiTanNm'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'UNKOBI_HaiSYmd'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'UNKOBI_TouYmd'
						  , UNKOBI.DanTaNm                                                                        AS 'UNKOBI_DanTaNm'
						  , UNKOBI.KanjJyus1                                                                      AS 'UNKOBI_KanjJyus1'
						  , UNKOBI.KanjJyus2                                                                      AS 'UNKOBI_KanjJyus2'
						  , UNKOBI.KanjTel                                                                        AS 'UNKOBI_KanjTel'
						  , UNKOBI.KanJNm                                                                         AS 'UNKOBI_KanJNm'
						  , HAISHA.UkeNo                                                                          AS 'HAISHA_UkeNo'
						  , HAISHA.UnkRen                                                                         AS 'HAISHA_UnkRen'
						  , HAISHA.SyaSyuRen                                                                      AS 'HAISHA_SyaSyuRen'
						  , HAISHA.TeiDanNo                                                                       AS 'HAISHA_TeiDanNo'
						  , HAISHA.BunkRen                                                                        AS 'HAISHA_BunkRen'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS 'HAISHA_HaiSYmd'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS 'HAISHA_HaiSTime'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS 'HAISHA_TouYmd'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS 'HAISHA_TouChTime'
						  , HAISHA.KikYmd                                                                         AS 'HAISHA_KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS 'HAISHA_KikTime'
						  , HAISHA.DanTaNm2                                                                       AS 'HAISHA_DanTaNm2'
						  , HAISHA.OthJinKbn1                                                                     AS 'HAISHA_OthJinKbn1'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'OTHJIN1_CodeKbnNm'
						  , HAISHA.OthJin1                                                                        AS 'HAISHA_OthJin1'
						  , HAISHA.OthJinKbn2                                                                     AS 'HAISHA_OthJinKbn2'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'OTHJIN2_CodeKbnNm'
						  , HAISHA.OthJin2                                                                        AS 'HAISHA_OthJin2'
						  , HAISHA.HaiSNm                                                                         AS 'HAISHA_HaiSNm'
						  , HAISHA.HaiSJyus1                                                                      AS 'HAISHA_HaiSJyus1'
						  , HAISHA.HaiSJyus2                                                                      AS 'HAISHA_HaiSJyus2'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS 'HAISHA_HaiSSetTime'
						  , HAISHA.IkNm                                                                           AS 'HAISHA_IkNm'
						  , HAISHA.SyuKoYmd                                                                       AS 'HAISHA_SyuKoYmd'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS 'HAISHA_SyuKoTime'
						  , HAISHA.SyuEigCdSeq                                                                    AS 'HAISHA_SyuEigCdSeq'
						  , EIGYOSHO.EigyoCd                                                                      AS 'EIGYOSHO_EigyoCd'
						  , EIGYOSHO.EigyoNm                                                                      AS 'EIGYOSHO_EigyoNm'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS 'HAISHA_SyuPaTime'
						  , HAISHA.JyoSyaJin                                                                      AS 'HAISHA_JyoSyaJin'
						  , HAISHA.PlusJin                                                                        AS 'HAISHA_PlusJin'
						  , HAISHA.GoSya                                                                          AS 'HAISHA_GoSya'
						  , HAISHA.HaiSKouKNm                                                                     AS 'HAISHA_HaiSKouKNm'
						  , HAISHA.HaiSBinNm                                                                      AS 'HAISHA_HaiSBinNm'
						  , HAISHA.TouSKouKNm                                                                     AS 'HAISHA_TouSKouKNm'
						  , HAISHA.TouSBinNm                                                                      AS 'HAISHA_TouSBinNm'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS 'HAISHA_TouSetTime'
						  , HAISHA.TouNm                                                                          AS 'HAISHA_TouNm'
						  , HAISHA.TouJyusyo1                                                                     AS 'HAISHA_TouJyusyo1'
						  , HAISHA.TouJyusyo2                                                                     AS 'HAISHA_TouJyusyo2'
						  , YYKSYU.SyaSyuDai                                                                      AS 'YYKSYU_SyaSyuDai'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS 'SYASYU_SYARYO_SyaSyuNm'
						  , SYARYO.SyaRyoCd                                                                       AS 'SYARYO_SyaRyoCd'
						  , SYARYO.SyaRyoNm                                                                       AS 'SYARYO_SyaRyoNm'
						  , HENSYA.TenkoNo                                                                        AS 'HENSYA_TenkoNo'
						  , SYARYO.KariSyaRyoNm                                                                   AS 'SYARYO_KariSyaRyoNm'
						  , TOKISK.TokuiNm                                                                        AS 'TOKISK_TokuiNm'
						  , TOKIST.SitenNm                                                                        AS 'TOKIST_SitenNm'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS 'YYKSYU_SyaSyuDaisum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
						  --, incidental.UnkRen                                                                 AS '�^�s���A��'
						  --, incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
						  --, incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
						  --, incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
						  --, incidental.SeisanNm                                                               AS '���Z��'
						  --, incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
						  --, incidental.Suryo                                                                  AS '����'
						  --, incidental.TeiDanNo                                                               AS '��c�ԍ�'
						  --, incidental.BunkRen                                                                AS '�����A��'
						  , HAISHA.BikoNm                                                                     as 'HAISHA_BikoNm'
						  , STUFF(
							(
									  SELECT
												',' + SYAIN.SyainNm
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as DriverList
						  , STUFF(
							(
									  SELECT
												',' + CAST(SYOKUM.SyokumuNm as nvarchar(20))
									  FROM
												TKD_Haiin AS HAIIN
												LEFT JOIN
														  VPM_Syain AS SYAIN
														  ON
																	SYAIN.SyainCdSeq = HAIIN.SyainCdSeq
												LEFT JOIN
														  VPM_KyoSHe AS KYOSHE
														  ON
																	KYOSHE.SyainCdSeq  = HAIIN.SyainCdSeq
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/�@�^�s�w�������擾�Ŏ擾�����z��_�o�ɔN����       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --�擾�ΏہF�^�]��A�_��^�]��A�K�C�h�A�_��K�C�h     	
																	AND SYOKUM.JigyoKbn    = 1               --���Ƌ敪:(�P�F�ݐ�)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�       	
												AND HAIIN.SiyoKbn = 1
												and HAIIN.UnkRen  =HAISHA.UnkRen
												and HAIIN.BunkRen =HAISHA.BunkRen
												and HAIIN.TeiDanNo=HAISHA.TeiDanNo
									  ORDER BY
												HAIIN.UkeNo ASC
											  , HAIIN.UnkRen ASC
											  , HAIIN.TeiDanNo ASC
											  , HAIIN.BunkRen ASC
											  , KYOSHE.SyokumuCdSeq ASC FOR XML PATH ('')
							)
							, 1, 1, '') as GuiderList
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+CAST( MFUTTU.Suryo AS varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=1
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as incidentalSuryo
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsDate
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.FutTumNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsFutTumNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FUTTUM.SeisanNm
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSeisanNm
										,STUFF(
                        (
                                  SELECT
                                                            top 5 ','+Cast(MFUTTU.Suryo as varchar)
                                                  FROM
                                                            TKD_MFutTu AS MFUTTU
                                                            LEFT JOIN
                                                                      TKD_FutTum AS FUTTUM
                                                                      ON
                                                                                FUTTUM.UkeNo         = MFUTTU.UkeNo
                                                                                AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                                                                                AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                                                                                AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                                                  WHERE
                                                            MFUTTU.UkeNo IN
                                                            (
                                                                   select
                                                                          UkeNo
                                                                   from
                                                                          TKD_Haisha
                                                                   where
                                                                          SyuKoYmd   =@SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
															and MFUTTU.UkeNo=HAISHA.UkeNo
															and FUTTUM.FutTumKbn=2
															and MFUTTU.TeiDanNo=HAISHA.TeiDanNo
															 ORDER BY
                                            FUTTUM.UkeNo ASC
                                          , FUTTUM.UnkRen ASC
                                          , FUTTUM.FutTumRen ASC FOR XML PATH ('')
                                        ) , 1, 1, '') as LoadedgoodsSuryo
        FROM
                        TKD_Haisha AS HAISHA
                        LEFT JOIN
                                        TKD_Unkobi AS UNKOBI
                                        ON
                                                        UNKOBI.UkeNo      = HAISHA.UkeNo
                                                        AND UNKOBI.UnkRen = HAISHA.UnkRen
                        INNER JOIN
                                        TKD_Yyksho AS YYKSHO
                                        ON
                                                        YYKSHO.UkeNo           = HAISHA.UkeNo
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq           	
                        LEFT JOIN
                                        TKD_YykSyu AS YYKSYU
                                        ON
                                                        YYKSYU.UkeNo        = HAISHA.UkeNo
                                                        AND YYKSYU.UnkRen   = HAISHA.UnkRen
                                                        AND YYKSYU.SyaSyuRen=HAISHA.SyaSyuRen
                                                        And YYKSYU.SiyoKbn  =1
                        LEFT JOIN
                                        VPM_SyaSyu AS SYASYU
                                        ON
                                                        SYASYU.SyaSyuCdSeq = YYKSYU.SyaSyuCdSeq
														and SYASYU.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
						LEFT JOIN
                                        VPM_SyaSyu AS SYASYU_SYARYO
                                        ON
                                                        SYARYO.SyaSyuCdSeq = SYASYU_SYARYO.SyaSyuCdSeq
														and SYASYU_SYARYO.TenantCdSeq=@TenantCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        CONVERT(tinyint,OTHJIN1.CodeKbn)         = HAISHA.OthJinKbn1
                                                        AND OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN2.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_Tokisk AS TOKISK
                                        ON
                                                        TOKISK.TokuiSeq = YYKSHO.TokuiSeq
                        LEFT JOIN
                                        VPM_TokiSt AS TOKIST
                                        ON
                                                        TOKIST.TokuiSeq        = YYKSHO.TokuiSeq
                                                        AND TOKIST.SitenCdSeq  = YYKSHO.SitenCdSeq
                                                        AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd
                                                        AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_Eigyos AS EIGYOSHO
                                        ON
                                                        EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                        LEFT JOIN
                                        VPM_Compny AS KAISHA
                                        ON
                                                        KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                        --LEFT OUTER JOIN
                        --                (
                        --                          SELECT
                        --                                    FUTTUM.HasYmd
                        --                                  , FUTTUM.UkeNo
                        --                                  , FUTTUM.UnkRen
                        --                                  , FUTTUM.FutTumKbn
                        --                                  , FUTTUM.FutTumRen
                        --                                  , FUTTUM.FutTumNm
                        --                                  , FUTTUM.SeisanNm
                        --                                  , FUTTUM.FutTumCdSeq
                        --                                  , MFUTTU.Suryo
                        --                                  , MFUTTU.TeiDanNo
                        --                                  , MFUTTU.BunkRen
                        --                          FROM
                        --                                    TKD_MFutTu AS MFUTTU
                        --                                    LEFT JOIN
                        --                                              TKD_FutTum AS FUTTUM
                        --                                              ON
                        --                                                        FUTTUM.UkeNo         = MFUTTU.UkeNo
                        --                                                        AND FUTTUM.UnkRen    = MFUTTU.UnkRen
                        --                                                        AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
                        --                                                        AND FUTTUM.FutTumRen = MFUTTU.FutTumRen
                        --                          WHERE
                        --                                    MFUTTU.UkeNo IN
                        --                                    (
                        --                                           select
                        --                                                  UkeNo
                        --                                           from
                        --                                                  TKD_Haisha
                        --                                           where
                        --                                                  SyuKoYmd   = @SyuKoYmd
                        --                                                  and SiyoKbn=1
                        --                                    ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd     >= @UkeCdFrom --��ʂ̎�t�ԍ��iFROM�j            	
                        AND YYKSHO.UkeCd <= @UkeCdTo   --��ʂ̎�t�ԍ��iTO�j            	
                        --AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom                  --��ʂ̗\��敪�iFROM�j            	
                        --AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo                  --��ʂ̗\��敪�iTO�j            	
                        AND HAISHA.KSKbn    <> 1 --�����ԈȊO            	
                        AND HAISHA.YouTblSeq = 0 --�b�ԈȊO            	
                        --AND HAISHA.SyuKoYmd = @SyuKoYmd              --��ʂ̏o�ɔN����            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq            	
                        --AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq                  --��ʂ̏o�ɉc�Ə�            	
                        AND HAISHA.SiyoKbn  = 1
						AND YoyaSyu			  = 1
                        AND HAISHA.TeiDanNo = @TeiDanNo --�p�����[�^�̒�c�ԍ�            	
                        AND HAISHA.UnkRen   = @UnkRen   --�p�����[�^�̉^�s���A��            	
                        AND HAISHA.BunkRen  = @BunkRen  --�p�����[�^�̕����A��       
						
                        --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ	
        ORDER BY
                          CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCdseq,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.TeiDanNo ,', ',HAISHA.UkeNo ,', ',HAISHA.UnkRen ,', ',HAISHA.BunkRen )
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',SYARYO.SyaRyoCd ,', ',HAISHA.SyuKoYmd ,', ',HAISHA.SyuKoTime ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCdseq ,', ',HENSYA.TenkoNo ,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC
GO


