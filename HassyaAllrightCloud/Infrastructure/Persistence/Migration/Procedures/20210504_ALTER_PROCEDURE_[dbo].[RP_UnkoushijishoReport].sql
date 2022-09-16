USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[RP_UnkoushijishoReport]    Script Date: 2021/05/04 9:07:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER
PROCEDURE [dbo].[RP_UnkoushijishoReport]
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
						  , YYKSHO.UkeCd                                                                          AS '受付番号'
						  , YYKSHO.TokuiTel                                                                       AS '得意先電話番号'
						  , YYKSHO.TokuiTanNm                                                                     AS '得意先担当者名'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '運行日_配車年月日'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '運行日_到着年月日'
						  , UNKOBI.DanTaNm                                                                        AS '運行日_団体名'
						  , UNKOBI.KanjJyus1                                                                      AS '運行日_幹事住所１'
						  , UNKOBI.KanjJyus2                                                                      AS '運行日_幹事住所２'
						  , UNKOBI.KanjTel                                                                        AS '運行日_幹事電話番号'
						  , UNKOBI.KanJNm                                                                         AS '運行日_幹事氏名'
						  , HAISHA.UkeNo                                                                          AS '配車_受付番号Seq'
						  , HAISHA.UnkRen                                                                         AS '配車_運行日連番'
						  , HAISHA.SyaSyuRen                                                                      AS '配車_車種連番'
						  , HAISHA.TeiDanNo                                                                       AS '配車_悌団番号'
						  , HAISHA.BunkRen                                                                        AS '配車_分割連番'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '配車_配車年月日'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '配車_配車時間'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '配車_到着年月日'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '配車_到着時間'
						  , HAISHA.KikYmd                                                                         AS 'KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '配車_帰庫時間'
						  , HAISHA.DanTaNm2                                                                       AS '配車_団体名2'
						  , HAISHA.OthJinKbn1                                                                     AS '配車_その他人員区分コード１'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'その他人員1'
						  , HAISHA.OthJin1                                                                        AS '配車_その他人員１数'
						  , HAISHA.OthJinKbn2                                                                     AS '配車_その他人員区分コード２'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'その他人員2'
						  , HAISHA.OthJin2                                                                        AS '配車_その他人員２数'
						  , HAISHA.HaiSNm                                                                         AS '配車_配車地名'
						  , HAISHA.HaiSJyus1                                                                      AS '配車_配車地住所１'
						  , HAISHA.HaiSJyus2                                                                      AS '配車_配車地住所２'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '配車_配車地接続時間'
						  , HAISHA.IkNm                                                                           AS '配車_行き先名'
						  , HAISHA.SyuKoYmd                                                                       AS '配車_出庫年月日'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '配車_出庫時間'
						  , HAISHA.SyuEigCdSeq                                                                    AS '配車_出庫営業所Seq'
						  , EIGYOSHO.EigyoCd                                                                      AS '出庫営業所コード'
						  , EIGYOSHO.EigyoNm                                                                      AS '出庫営業所名'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '配車_出発時間'
						  , HAISHA.JyoSyaJin                                                                      AS '配車_乗車人員'
						  , HAISHA.PlusJin                                                                        AS '配車_プラス人員'
						  , HAISHA.GoSya                                                                          AS '配車_号車'
						  , HAISHA.HaiSKouKNm                                                                     AS '配車_配車地交通機関名'
						  , HAISHA.HaiSBinNm                                                                      AS '配車_配車地便名'
						  , HAISHA.TouSKouKNm                                                                     AS '配車_到着地交通機関名'
						  , HAISHA.TouSBinNm                                                                      AS '配車_到着地便名'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '配車_到着地接続時間'
						  , HAISHA.TouNm                                                                          AS '配車_到着地名'
						  , HAISHA.TouJyusyo1                                                                     AS '配車_到着地住所１'
						  , HAISHA.TouJyusyo2                                                                     AS '配車_到着地住所２'
						  , YYKSYU.SyaSyuDai                                                                      AS '予約車種_車種台数'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS '車種名'
						  , SYARYO.SyaRyoCd                                                                       AS '車両コード'
						  , SYARYO.SyaRyoNm                                                                       AS '車両名'
						  , HENSYA.TenkoNo                                                                        AS '車両点呼'
						  , SYARYO.KariSyaRyoNm                                                                   AS '仮車両名'
						  , TOKISK.TokuiNm                                                                        AS '得意先名'
						  , TOKIST.SitenNm                                                                        AS '得意先支店名'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS '予約車種_車種台数sum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '発生年月日'
						  --, incidental.UnkRen                                                                 AS '運行日連番'
						  --, incidental.FutTumKbn                                                              AS '付帯積込品区分'
						  --, incidental.FutTumRen                                                              AS '付帯積込品連番'
						  --, incidental.FutTumNm                                                               AS '付帯積込品名'
						  --, incidental.SeisanNm                                                               AS '精算名'
						  --, incidental.FutTumCdSeq                                                            AS '付帯積込品コードＳＥＱ'
						  --, incidental.Suryo                                                                  AS '数量'
						  --, incidental.TeiDanNo                                                               AS '悌団番号'
						  --, incidental.BunkRen                                                                AS '分割連番'
						  , HAISHA.BikoNm                                                                     as '備考'
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
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
																	AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
							, 1, 1, '') as 社員名
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
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
																	AND SYOKUM.JigyoKbn    = 1               --事業区分:(１：貸切)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
							, 1, 1, '') as 社員職務名

							, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
															AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq           	
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
															And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
															AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
							LEFT JOIN
											VPM_CodeKb AS OTHJIN2
											ON
															CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
															AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
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
															AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq
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
							--									) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
							--									AND MFUTTU.SiyoKbn = 1
							--									AND FUTTUM.SiyoKbn = 1
							--				)
							--				incidental
							--				ON
							--								incidental.UkeNo = HAISHA.UkeNo
			WHERE
							concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))          	
							AND HAISHA.KSKbn      <> 1               --未仮車以外            	
							AND HAISHA.YouTblSeq   = 0               --傭車以外       
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
						  , YYKSHO.UkeCd                                                                          AS '受付番号'
						  , YYKSHO.TokuiTel                                                                       AS '得意先電話番号'
						  , YYKSHO.TokuiTanNm                                                                     AS '得意先担当者名'
						  , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '運行日_配車年月日'
						  , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '運行日_到着年月日'
						  , UNKOBI.DanTaNm                                                                        AS '運行日_団体名'
						  , UNKOBI.KanjJyus1                                                                      AS '運行日_幹事住所１'
						  , UNKOBI.KanjJyus2                                                                      AS '運行日_幹事住所２'
						  , UNKOBI.KanjTel                                                                        AS '運行日_幹事電話番号'
						  , UNKOBI.KanJNm                                                                         AS '運行日_幹事氏名'
						  , HAISHA.UkeNo                                                                          AS '配車_受付番号Seq'
						  , HAISHA.UnkRen                                                                         AS '配車_運行日連番'
						  , HAISHA.SyaSyuRen                                                                      AS '配車_車種連番'
						  , HAISHA.TeiDanNo                                                                       AS '配車_悌団番号'
						  , HAISHA.BunkRen                                                                        AS '配車_分割連番'
						  , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '配車_配車年月日'
						  , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '配車_配車時間'
						  , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '配車_到着年月日'
						  , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '配車_到着時間'
						  , HAISHA.KikYmd                                                                         AS 'KikYmd'
						  , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '配車_帰庫時間'
						  , HAISHA.DanTaNm2                                                                       AS '配車_団体名2'
						  , HAISHA.OthJinKbn1                                                                     AS '配車_その他人員区分コード１'
						  , OTHJIN1.CodeKbnNm                                                                     AS 'その他人員1'
						  , HAISHA.OthJin1                                                                        AS '配車_その他人員１数'
						  , HAISHA.OthJinKbn2                                                                     AS '配車_その他人員区分コード２'
						  , OTHJIN2.CodeKbnNm                                                                     AS 'その他人員2'
						  , HAISHA.OthJin2                                                                        AS '配車_その他人員２数'
						  , HAISHA.HaiSNm                                                                         AS '配車_配車地名'
						  , HAISHA.HaiSJyus1                                                                      AS '配車_配車地住所１'
						  , HAISHA.HaiSJyus2                                                                      AS '配車_配車地住所２'
						  , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '配車_配車地接続時間'
						  , HAISHA.IkNm                                                                           AS '配車_行き先名'
						  , HAISHA.SyuKoYmd                                                                       AS '配車_出庫年月日'
						  , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '配車_出庫時間'
						  , HAISHA.SyuEigCdSeq                                                                    AS '配車_出庫営業所Seq'
						  , EIGYOSHO.EigyoCd                                                                      AS '出庫営業所コード'
						  , EIGYOSHO.EigyoNm                                                                      AS '出庫営業所名'
						  , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '配車_出発時間'
						  , HAISHA.JyoSyaJin                                                                      AS '配車_乗車人員'
						  , HAISHA.PlusJin                                                                        AS '配車_プラス人員'
						  , HAISHA.GoSya                                                                          AS '配車_号車'
						  , HAISHA.HaiSKouKNm                                                                     AS '配車_配車地交通機関名'
						  , HAISHA.HaiSBinNm                                                                      AS '配車_配車地便名'
						  , HAISHA.TouSKouKNm                                                                     AS '配車_到着地交通機関名'
						  , HAISHA.TouSBinNm                                                                      AS '配車_到着地便名'
						  , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '配車_到着地接続時間'
						  , HAISHA.TouNm                                                                          AS '配車_到着地名'
						  , HAISHA.TouJyusyo1                                                                     AS '配車_到着地住所１'
						  , HAISHA.TouJyusyo2                                                                     AS '配車_到着地住所２'
						  , YYKSYU.SyaSyuDai                                                                      AS '予約車種_車種台数'
						  , SYASYU_SYARYO.SyaSyuNm                                                                       AS '車種名'
						  , SYARYO.SyaRyoCd                                                                       AS '車両コード'
						  , SYARYO.SyaRyoNm                                                                       AS '車両名'
						  , HENSYA.TenkoNo                                                                        AS '車両点呼'
						  , SYARYO.KariSyaRyoNm                                                                   AS '仮車両名'
						  , TOKISK.TokuiNm                                                                        AS '得意先名'
						  , TOKIST.SitenNm                                                                        AS '得意先支店名'
						  , (
								   SELECT
										  SUM(SyaSyuDai)
								   FROM
										  TKD_YykSyu
								   WHERE
										  UkeNo      = HAISHA.UkeNo
										  and SiyoKbn=1
							)
																											  AS '予約車種_車種台数sum'
						  --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '発生年月日'
						  --, incidental.UnkRen                                                                 AS '運行日連番'
						  --, incidental.FutTumKbn                                                              AS '付帯積込品区分'
						  --, incidental.FutTumRen                                                              AS '付帯積込品連番'
						  --, incidental.FutTumNm                                                               AS '付帯積込品名'
						  --, incidental.SeisanNm                                                               AS '精算名'
						  --, incidental.FutTumCdSeq                                                            AS '付帯積込品コードＳＥＱ'
						  --, incidental.Suryo                                                                  AS '数量'
						  --, incidental.TeiDanNo                                                               AS '悌団番号'
						  --, incidental.BunkRen                                                                AS '分割連番'
						  , HAISHA.BikoNm                                                                     as '備考'
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
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
																	AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
																	AND SYOKUM.SiyoKbn  = 1
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
							, 1, 1, '') as 社員名
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
																	AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
																	AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
												LEFT JOIN
														  VPM_Syokum AS SYOKUM
														  ON
																	SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
																	AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
																	AND SYOKUM.JigyoKbn    = 1               --事業区分:(１：貸切)      	
																	AND SYOKUM.SiyoKbn     = 1
																	AND SYOKUM.TenantCdSeq = @TenantCdSeq
									  WHERE
												HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
							, 1, 1, '') as 社員職務名
							, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
															AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq           	
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
															And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
															AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
							LEFT JOIN
											VPM_CodeKb AS OTHJIN2
											ON
															CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
															AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
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
															AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq
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
							--									) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
							--									AND MFUTTU.SiyoKbn = 1
							--									AND FUTTUM.SiyoKbn = 1
							--				)
							--				incidental
							--				ON
							--								incidental.UkeNo = HAISHA.UkeNo
			WHERE
							concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000'),FORMAT(HAISHA.TeiDanNo, '000'),FORMAT(HAISHA.BunkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))            	
							AND HAISHA.KSKbn      <> 1               --未仮車以外            	
							AND HAISHA.YouTblSeq   = 0               --傭車以外       
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
                      , YYKSHO.UkeCd                                                                          AS '受付番号'
                      , YYKSHO.TokuiTel                                                                       AS '得意先電話番号'
                      , YYKSHO.TokuiTanNm                                                                     AS '得意先担当者名'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '運行日_配車年月日'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '運行日_到着年月日'
                      , UNKOBI.DanTaNm                                                                        AS '運行日_団体名'
                      , UNKOBI.KanjJyus1                                                                      AS '運行日_幹事住所１'
                      , UNKOBI.KanjJyus2                                                                      AS '運行日_幹事住所２'
                      , UNKOBI.KanjTel                                                                        AS '運行日_幹事電話番号'
                      , UNKOBI.KanJNm                                                                         AS '運行日_幹事氏名'
                      , HAISHA.UkeNo                                                                          AS '配車_受付番号Seq'
                      , HAISHA.UnkRen                                                                         AS '配車_運行日連番'
                      , HAISHA.SyaSyuRen                                                                      AS '配車_車種連番'
                      , HAISHA.TeiDanNo                                                                       AS '配車_悌団番号'
                      , HAISHA.BunkRen                                                                        AS '配車_分割連番'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '配車_配車年月日'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '配車_配車時間'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '配車_到着年月日'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '配車_到着時間'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '配車_帰庫時間'
                      , HAISHA.DanTaNm2                                                                       AS '配車_団体名2'
                      , HAISHA.OthJinKbn1                                                                     AS '配車_その他人員区分コード１'
                      , OTHJIN1.CodeKbnNm                                                                     AS 'その他人員1'
                      , HAISHA.OthJin1                                                                        AS '配車_その他人員１数'
                      , HAISHA.OthJinKbn2                                                                     AS '配車_その他人員区分コード２'
                      , OTHJIN2.CodeKbnNm                                                                     AS 'その他人員2'
                      , HAISHA.OthJin2                                                                        AS '配車_その他人員２数'
                      , HAISHA.HaiSNm                                                                         AS '配車_配車地名'
                      , HAISHA.HaiSJyus1                                                                      AS '配車_配車地住所１'
                      , HAISHA.HaiSJyus2                                                                      AS '配車_配車地住所２'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '配車_配車地接続時間'
                      , HAISHA.IkNm                                                                           AS '配車_行き先名'
                      , HAISHA.SyuKoYmd                                                                       AS '配車_出庫年月日'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '配車_出庫時間'
                      , HAISHA.SyuEigCdSeq                                                                    AS '配車_出庫営業所Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '出庫営業所コード'
                      , EIGYOSHO.EigyoNm                                                                      AS '出庫営業所名'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '配車_出発時間'
                      , HAISHA.JyoSyaJin                                                                      AS '配車_乗車人員'
                      , HAISHA.PlusJin                                                                        AS '配車_プラス人員'
                      , HAISHA.GoSya                                                                          AS '配車_号車'
                      , HAISHA.HaiSKouKNm                                                                     AS '配車_配車地交通機関名'
                      , HAISHA.HaiSBinNm                                                                      AS '配車_配車地便名'
                      , HAISHA.TouSKouKNm                                                                     AS '配車_到着地交通機関名'
                      , HAISHA.TouSBinNm                                                                      AS '配車_到着地便名'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '配車_到着地接続時間'
                      , HAISHA.TouNm                                                                          AS '配車_到着地名'
                      , HAISHA.TouJyusyo1                                                                     AS '配車_到着地住所１'
                      , HAISHA.TouJyusyo2                                                                     AS '配車_到着地住所２'
                      , YYKSYU.SyaSyuDai                                                                      AS '予約車種_車種台数'
                      , SYASYU_SYARYO.SyaSyuNm                                                                       AS '車種名'
                      , SYARYO.SyaRyoCd                                                                       AS '車両コード'
                      , SYARYO.SyaRyoNm                                                                       AS '車両名'
                      , HENSYA.TenkoNo                                                                        AS '車両点呼'
                      , SYARYO.KariSyaRyoNm                                                                   AS '仮車両名'
                      , TOKISK.TokuiNm                                                                        AS '得意先名'
                      , TOKIST.SitenNm                                                                        AS '得意先支店名'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '予約車種_車種台数sum'
                      --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '発生年月日'
                      --, incidental.UnkRen                                                                 AS '運行日連番'
                      --, incidental.FutTumKbn                                                              AS '付帯積込品区分'
                      --, incidental.FutTumRen                                                              AS '付帯積込品連番'
                      --, incidental.FutTumNm                                                               AS '付帯積込品名'
                      --, incidental.SeisanNm                                                               AS '精算名'
                      --, incidental.FutTumCdSeq                                                            AS '付帯積込品コードＳＥＱ'
                      --, incidental.Suryo                                                                  AS '数量'
                      --, incidental.TeiDanNo                                                               AS '悌団番号'
                      --, incidental.BunkRen                                                                AS '分割連番'
                      , HAISHA.BikoNm                                                                     as '備考'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員名
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn    = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn     = 1
                                                                AND SYOKUM.TenantCdSeq = @TenantCdSeq
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員職務名
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq           	
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
                                                        And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq
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
                        --                                    ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --画面の受付番号（FROM）            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --画面の受付番号（TO）            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）            	
                        AND HAISHA.KSKbn      <> 1               --未仮車以外            	
                        AND HAISHA.YouTblSeq   = 0               --傭車以外            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq
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
                      , YYKSHO.UkeCd                                                                          AS '受付番号'
                      , YYKSHO.TokuiTel                                                                       AS '得意先電話番号'
                      , YYKSHO.TokuiTanNm                                                                     AS '得意先担当者名'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '運行日_配車年月日'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '運行日_到着年月日'
                      , UNKOBI.DanTaNm                                                                        AS '運行日_団体名'
                      , UNKOBI.KanjJyus1                                                                      AS '運行日_幹事住所１'
                      , UNKOBI.KanjJyus2                                                                      AS '運行日_幹事住所２'
                      , UNKOBI.KanjTel                                                                        AS '運行日_幹事電話番号'
                      , UNKOBI.KanJNm                                                                         AS '運行日_幹事氏名'
                      , HAISHA.UkeNo                                                                          AS '配車_受付番号Seq'
                      , HAISHA.UnkRen                                                                         AS '配車_運行日連番'
                      , HAISHA.SyaSyuRen                                                                      AS '配車_車種連番'
                      , HAISHA.TeiDanNo                                                                       AS '配車_悌団番号'
                      , HAISHA.BunkRen                                                                        AS '配車_分割連番'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '配車_配車年月日'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '配車_配車時間'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '配車_到着年月日'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '配車_到着時間'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '配車_帰庫時間'
                      , HAISHA.DanTaNm2                                                                       AS '配車_団体名2'
                      , HAISHA.OthJinKbn1                                                                     AS '配車_その他人員区分コード１'
                      , OTHJIN1.CodeKbnNm                                                                     AS 'その他人員1'
                      , HAISHA.OthJin1                                                                        AS '配車_その他人員１数'
                      , HAISHA.OthJinKbn2                                                                     AS '配車_その他人員区分コード２'
                      , OTHJIN2.CodeKbnNm                                                                     AS 'その他人員2'
                      , HAISHA.OthJin2                                                                        AS '配車_その他人員２数'
                      , HAISHA.HaiSNm                                                                         AS '配車_配車地名'
                      , HAISHA.HaiSJyus1                                                                      AS '配車_配車地住所１'
                      , HAISHA.HaiSJyus2                                                                      AS '配車_配車地住所２'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '配車_配車地接続時間'
                      , HAISHA.IkNm                                                                           AS '配車_行き先名'
                      , HAISHA.SyuKoYmd                                                                       AS '配車_出庫年月日'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '配車_出庫時間'
                      , HAISHA.SyuEigCdSeq                                                                    AS '配車_出庫営業所Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '出庫営業所コード'
                      , EIGYOSHO.EigyoNm                                                                      AS '出庫営業所名'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '配車_出発時間'
                      , HAISHA.JyoSyaJin                                                                      AS '配車_乗車人員'
                      , HAISHA.PlusJin                                                                        AS '配車_プラス人員'
                      , HAISHA.GoSya                                                                          AS '配車_号車'
                      , HAISHA.HaiSKouKNm                                                                     AS '配車_配車地交通機関名'
                      , HAISHA.HaiSBinNm                                                                      AS '配車_配車地便名'
                      , HAISHA.TouSKouKNm                                                                     AS '配車_到着地交通機関名'
                      , HAISHA.TouSBinNm                                                                      AS '配車_到着地便名'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '配車_到着地接続時間'
                      , HAISHA.TouNm                                                                          AS '配車_到着地名'
                      , HAISHA.TouJyusyo1                                                                     AS '配車_到着地住所１'
                      , HAISHA.TouJyusyo2                                                                     AS '配車_到着地住所２'
                      , YYKSYU.SyaSyuDai                                                                      AS '予約車種_車種台数'
                      , SYASYU_SYARYO.SyaSyuNm                                                                       AS '車種名'
                      , SYARYO.SyaRyoCd                                                                       AS '車両コード'
                      , SYARYO.SyaRyoNm                                                                       AS '車両名'
                      , HENSYA.TenkoNo                                                                        AS '車両点呼'
                      , SYARYO.KariSyaRyoNm                                                                   AS '仮車両名'
                      , TOKISK.TokuiNm                                                                        AS '得意先名'
                      , TOKIST.SitenNm                                                                        AS '得意先支店名'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '予約車種_車種台数sum'
                      --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '発生年月日'
                      --, incidental.UnkRen                                                                 AS '運行日連番'
                      --, incidental.FutTumKbn                                                              AS '付帯積込品区分'
                      --, incidental.FutTumRen                                                              AS '付帯積込品連番'
                      --, incidental.FutTumNm                                                               AS '付帯積込品名'
                      --, incidental.SeisanNm                                                               AS '精算名'
                      --, incidental.FutTumCdSeq                                                            AS '付帯積込品コードＳＥＱ'
                      --, incidental.Suryo                                                                  AS '数量'
                      --, incidental.TeiDanNo                                                               AS '悌団番号'
                      --, incidental.BunkRen                                                                AS '分割連番'
                      , HAISHA.BikoNm                                                                     as '備考'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員名
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn    = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn     = 1
                                                                AND SYOKUM.TenantCdSeq = @TenantCdSeq
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員職務名
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq           	
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
                                                        AND OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq
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
                        --                                    ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --画面の受付番号（FROM）            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --画面の受付番号（TO）            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）            	
                        AND HAISHA.KSKbn      <> 1               --未仮車以外            	
                        AND HAISHA.YouTblSeq   = 0               --傭車以外            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq
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
                      , YYKSHO.UkeCd                                                                          AS '受付番号'
                      , YYKSHO.TokuiTel                                                                       AS '得意先電話番号'
                      , YYKSHO.TokuiTanNm                                                                     AS '得意先担当者名'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '運行日_配車年月日'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '運行日_到着年月日'
                      , UNKOBI.DanTaNm                                                                        AS '運行日_団体名'
                      , UNKOBI.KanjJyus1                                                                      AS '運行日_幹事住所１'
                      , UNKOBI.KanjJyus2                                                                      AS '運行日_幹事住所２'
                      , UNKOBI.KanjTel                                                                        AS '運行日_幹事電話番号'
                      , UNKOBI.KanJNm                                                                         AS '運行日_幹事氏名'
                      , HAISHA.UkeNo                                                                          AS '配車_受付番号Seq'
                      , HAISHA.UnkRen                                                                         AS '配車_運行日連番'
                      , HAISHA.SyaSyuRen                                                                      AS '配車_車種連番'
                      , HAISHA.TeiDanNo                                                                       AS '配車_悌団番号'
                      , HAISHA.BunkRen                                                                        AS '配車_分割連番'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '配車_配車年月日'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '配車_配車時間'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '配車_到着年月日'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '配車_到着時間'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '配車_帰庫時間'
                      , HAISHA.DanTaNm2                                                                       AS '配車_団体名2'
                      , HAISHA.OthJinKbn1                                                                     AS '配車_その他人員区分コード１'
                      , OTHJIN1.CodeKbnNm                                                                     AS 'その他人員1'
                      , HAISHA.OthJin1                                                                        AS '配車_その他人員１数'
                      , HAISHA.OthJinKbn2                                                                     AS '配車_その他人員区分コード２'
                      , OTHJIN2.CodeKbnNm                                                                     AS 'その他人員2'
                      , HAISHA.OthJin2                                                                        AS '配車_その他人員２数'
                      , HAISHA.HaiSNm                                                                         AS '配車_配車地名'
                      , HAISHA.HaiSJyus1                                                                      AS '配車_配車地住所１'
                      , HAISHA.HaiSJyus2                                                                      AS '配車_配車地住所２'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '配車_配車地接続時間'
                      , HAISHA.IkNm                                                                           AS '配車_行き先名'
                      , HAISHA.SyuKoYmd                                                                       AS '配車_出庫年月日'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '配車_出庫時間'
                      , HAISHA.SyuEigCdSeq                                                                    AS '配車_出庫営業所Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '出庫営業所コード'
                      , EIGYOSHO.EigyoNm                                                                      AS '出庫営業所名'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '配車_出発時間'
                      , HAISHA.JyoSyaJin                                                                      AS '配車_乗車人員'
                      , HAISHA.PlusJin                                                                        AS '配車_プラス人員'
                      , HAISHA.GoSya                                                                          AS '配車_号車'
                      , HAISHA.HaiSKouKNm                                                                     AS '配車_配車地交通機関名'
                      , HAISHA.HaiSBinNm                                                                      AS '配車_配車地便名'
                      , HAISHA.TouSKouKNm                                                                     AS '配車_到着地交通機関名'
                      , HAISHA.TouSBinNm                                                                      AS '配車_到着地便名'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '配車_到着地接続時間'
                      , HAISHA.TouNm                                                                          AS '配車_到着地名'
                      , HAISHA.TouJyusyo1                                                                     AS '配車_到着地住所１'
                      , HAISHA.TouJyusyo2                                                                     AS '配車_到着地住所２'
                      , YYKSYU.SyaSyuDai                                                                      AS '予約車種_車種台数'
                      , SYASYU_SYARYO.SyaSyuNm                                                                       AS '車種名'
                      , SYARYO.SyaRyoCd                                                                       AS '車両コード'
                      , SYARYO.SyaRyoNm                                                                       AS '車両名'
                      , HENSYA.TenkoNo                                                                        AS '車両点呼'
                      , SYARYO.KariSyaRyoNm                                                                   AS '仮車両名'
                      , TOKISK.TokuiNm                                                                        AS '得意先名'
                      , TOKIST.SitenNm                                                                        AS '得意先支店名'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '予約車種_車種台数sum'
                      --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '発生年月日'
                      --, incidental.UnkRen                                                                 AS '運行日連番'
                      --, incidental.FutTumKbn                                                              AS '付帯積込品区分'
                      --, incidental.FutTumRen                                                              AS '付帯積込品連番'
                      --, incidental.FutTumNm                                                               AS '付帯積込品名'
                      --, incidental.SeisanNm                                                               AS '精算名'
                      --, incidental.FutTumCdSeq                                                            AS '付帯積込品コードＳＥＱ'
                      --, incidental.Suryo                                                                  AS '数量'
                      --, incidental.TeiDanNo                                                               AS '悌団番号'
                      --, incidental.BunkRen                                                                AS '分割連番'
                      , HAISHA.BikoNm                                                                     as '備考'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員名
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn    = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn     = 1
                                                                AND SYOKUM.TenantCdSeq = @TenantCdSeq
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員職務名
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq           	
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
                                                        And OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq
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
                        --                                    ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --画面の受付番号（FROM）            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --画面の受付番号（TO）            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）            	
                        AND HAISHA.KSKbn      <> 1               --未仮車以外            	
                        AND HAISHA.YouTblSeq   = 0               --傭車以外            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq
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
                      , YYKSHO.UkeCd                                                                          AS '受付番号'
                      , YYKSHO.TokuiTel                                                                       AS '得意先電話番号'
                      , YYKSHO.TokuiTanNm                                                                     AS '得意先担当者名'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '運行日_配車年月日'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '運行日_到着年月日'
                      , UNKOBI.DanTaNm                                                                        AS '運行日_団体名'
                      , UNKOBI.KanjJyus1                                                                      AS '運行日_幹事住所１'
                      , UNKOBI.KanjJyus2                                                                      AS '運行日_幹事住所２'
                      , UNKOBI.KanjTel                                                                        AS '運行日_幹事電話番号'
                      , UNKOBI.KanJNm                                                                         AS '運行日_幹事氏名'
                      , HAISHA.UkeNo                                                                          AS '配車_受付番号Seq'
                      , HAISHA.UnkRen                                                                         AS '配車_運行日連番'
                      , HAISHA.SyaSyuRen                                                                      AS '配車_車種連番'
                      , HAISHA.TeiDanNo                                                                       AS '配車_悌団番号'
                      , HAISHA.BunkRen                                                                        AS '配車_分割連番'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '配車_配車年月日'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '配車_配車時間'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '配車_到着年月日'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '配車_到着時間'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '配車_帰庫時間'
                      , HAISHA.DanTaNm2                                                                       AS '配車_団体名2'
                      , HAISHA.OthJinKbn1                                                                     AS '配車_その他人員区分コード１'
                      , OTHJIN1.CodeKbnNm                                                                     AS 'その他人員1'
                      , HAISHA.OthJin1                                                                        AS '配車_その他人員１数'
                      , HAISHA.OthJinKbn2                                                                     AS '配車_その他人員区分コード２'
                      , OTHJIN2.CodeKbnNm                                                                     AS 'その他人員2'
                      , HAISHA.OthJin2                                                                        AS '配車_その他人員２数'
                      , HAISHA.HaiSNm                                                                         AS '配車_配車地名'
                      , HAISHA.HaiSJyus1                                                                      AS '配車_配車地住所１'
                      , HAISHA.HaiSJyus2                                                                      AS '配車_配車地住所２'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '配車_配車地接続時間'
                      , HAISHA.IkNm                                                                           AS '配車_行き先名'
                      , HAISHA.SyuKoYmd                                                                       AS '配車_出庫年月日'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '配車_出庫時間'
                      , HAISHA.SyuEigCdSeq                                                                    AS '配車_出庫営業所Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '出庫営業所コード'
                      , EIGYOSHO.EigyoNm                                                                      AS '出庫営業所名'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '配車_出発時間'
                      , HAISHA.JyoSyaJin                                                                      AS '配車_乗車人員'
                      , HAISHA.PlusJin                                                                        AS '配車_プラス人員'
                      , HAISHA.GoSya                                                                          AS '配車_号車'
                      , HAISHA.HaiSKouKNm                                                                     AS '配車_配車地交通機関名'
                      , HAISHA.HaiSBinNm                                                                      AS '配車_配車地便名'
                      , HAISHA.TouSKouKNm                                                                     AS '配車_到着地交通機関名'
                      , HAISHA.TouSBinNm                                                                      AS '配車_到着地便名'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '配車_到着地接続時間'
                      , HAISHA.TouNm                                                                          AS '配車_到着地名'
                      , HAISHA.TouJyusyo1                                                                     AS '配車_到着地住所１'
                      , HAISHA.TouJyusyo2                                                                     AS '配車_到着地住所２'
                      , YYKSYU.SyaSyuDai                                                                      AS '予約車種_車種台数'
                      , SYASYU_SYARYO.SyaSyuNm                                                                       AS '車種名'
                      , SYARYO.SyaRyoCd                                                                       AS '車両コード'
                      , SYARYO.SyaRyoNm                                                                       AS '車両名'
                      , HENSYA.TenkoNo                                                                        AS '車両点呼'
                      , SYARYO.KariSyaRyoNm                                                                   AS '仮車両名'
                      , TOKISK.TokuiNm                                                                        AS '得意先名'
                      , TOKIST.SitenNm                                                                        AS '得意先支店名'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '予約車種_車種台数sum'
                      --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '発生年月日'
                      --, incidental.UnkRen                                                                 AS '運行日連番'
                      --, incidental.FutTumKbn                                                              AS '付帯積込品区分'
                      --, incidental.FutTumRen                                                              AS '付帯積込品連番'
                      --, incidental.FutTumNm                                                               AS '付帯積込品名'
                      --, incidental.SeisanNm                                                               AS '精算名'
                      --, incidental.FutTumCdSeq                                                            AS '付帯積込品コードＳＥＱ'
                      --, incidental.Suryo                                                                  AS '数量'
                      --, incidental.TeiDanNo                                                               AS '悌団番号'
                      --, incidental.BunkRen                                                                AS '分割連番'
                      , HAISHA.BikoNm                                                                     as '備考'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員名
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn    = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn     = 1
                                                                AND SYOKUM.TenantCdSeq = @TenantCdSeq
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員職務名
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq           	
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
                                                        AND OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                       CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq
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
                        --                                    ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --画面の受付番号（FROM）            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --画面の受付番号（TO）            	
                        AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）            	
                        AND HAISHA.KSKbn      <> 1               --未仮車以外            	
                        AND HAISHA.YouTblSeq   = 0               --傭車以外            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq
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
                      , YYKSHO.UkeCd                                                                          AS '受付番号'
                      , YYKSHO.TokuiTel                                                                       AS '得意先電話番号'
                      , YYKSHO.TokuiTanNm                                                                     AS '得意先担当者名'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '運行日_配車年月日'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '運行日_到着年月日'
                      , UNKOBI.DanTaNm                                                                        AS '運行日_団体名'
                      , UNKOBI.KanjJyus1                                                                      AS '運行日_幹事住所１'
                      , UNKOBI.KanjJyus2                                                                      AS '運行日_幹事住所２'
                      , UNKOBI.KanjTel                                                                        AS '運行日_幹事電話番号'
                      , UNKOBI.KanJNm                                                                         AS '運行日_幹事氏名'
                      , HAISHA.UkeNo                                                                          AS '配車_受付番号Seq'
                      , HAISHA.UnkRen                                                                         AS '配車_運行日連番'
                      , HAISHA.SyaSyuRen                                                                      AS '配車_車種連番'
                      , HAISHA.TeiDanNo                                                                       AS '配車_悌団番号'
                      , HAISHA.BunkRen                                                                        AS '配車_分割連番'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )        AS '配車_配車年月日'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '配車_配車時間'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )         AS '配車_到着年月日'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '配車_到着時間'
					   , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '配車_帰庫時間'
                      , HAISHA.DanTaNm2                                                                       AS '配車_団体名2'
                      , HAISHA.OthJinKbn1                                                                     AS '配車_その他人員区分コード１'
                      , OTHJIN1.CodeKbnNm                                                                     AS 'その他人員1'
                      , HAISHA.OthJin1                                                                        AS '配車_その他人員１数'
                      , HAISHA.OthJinKbn2                                                                     AS '配車_その他人員区分コード２'
                      , OTHJIN2.CodeKbnNm                                                                     AS 'その他人員2'
                      , HAISHA.OthJin2                                                                        AS '配車_その他人員２数'
                      , HAISHA.HaiSNm                                                                         AS '配車_配車地名'
                      , HAISHA.HaiSJyus1                                                                      AS '配車_配車地住所１'
                      , HAISHA.HaiSJyus2                                                                      AS '配車_配車地住所２'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '配車_配車地接続時間'
                      , HAISHA.IkNm                                                                           AS '配車_行き先名'
                      , HAISHA.SyuKoYmd                                                                       AS '配車_出庫年月日'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '配車_出庫時間'
                      , HAISHA.SyuEigCdSeq                                                                    AS '配車_出庫営業所Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '出庫営業所コード'
                      , EIGYOSHO.EigyoNm                                                                      AS '出庫営業所名'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '配車_出発時間'
                      , HAISHA.JyoSyaJin                                                                      AS '配車_乗車人員'
                      , HAISHA.PlusJin                                                                        AS '配車_プラス人員'
                      , HAISHA.GoSya                                                                          AS '配車_号車'
                      , HAISHA.HaiSKouKNm                                                                     AS '配車_配車地交通機関名'
                      , HAISHA.HaiSBinNm                                                                      AS '配車_配車地便名'
                      , HAISHA.TouSKouKNm                                                                     AS '配車_到着地交通機関名'
                      , HAISHA.TouSBinNm                                                                      AS '配車_到着地便名'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '配車_到着地接続時間'
                      , HAISHA.TouNm                                                                          AS '配車_到着地名'
                      , HAISHA.TouJyusyo1                                                                     AS '配車_到着地住所１'
                      , HAISHA.TouJyusyo2                                                                     AS '配車_到着地住所２'
                      , YYKSYU.SyaSyuDai                                                                      AS '予約車種_車種台数'
                      , SYASYU_SYARYO.SyaSyuNm                                                                       AS '車種名'
                      , SYARYO.SyaRyoCd                                                                       AS '車両コード'
                      , SYARYO.SyaRyoNm                                                                       AS '車両名'
                      , HENSYA.TenkoNo                                                                        AS '車両点呼'
                      , SYARYO.KariSyaRyoNm                                                                   AS '仮車両名'
                      , TOKISK.TokuiNm                                                                        AS '得意先名'
                      , TOKIST.SitenNm                                                                        AS '得意先支店名'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '予約車種_車種台数sum'
                      --, FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '発生年月日'
                      --, incidental.UnkRen                                                                 AS '運行日連番'
                      --, incidental.FutTumKbn                                                              AS '付帯積込品区分'
                      --, incidental.FutTumRen                                                              AS '付帯積込品連番'
                      --, incidental.FutTumNm                                                               AS '付帯積込品名'
                      --, incidental.SeisanNm                                                               AS '精算名'
                      --, incidental.FutTumCdSeq                                                            AS '付帯積込品コードＳＥＱ'
                      --, incidental.Suryo                                                                  AS '数量'
                      --, incidental.TeiDanNo                                                               AS '悌団番号'
                      --, incidental.BunkRen                                                                AS '分割連番'
                      , HAISHA.BikoNm                                                                     as '備考'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員名
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/　運行指示書情報取得で取得した配車_出庫年月日       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --取得対象：運転手、契約運転手、ガイド、契約ガイド     	
                                                                AND SYOKUM.JigyoKbn = 1               --事業区分:(１：貸切)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号       	
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
                        , 1, 1, '') as 社員職務名
						, STUFF(
                        (
                                  SELECT
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            top 5 ','+FORMAT ( convert(datetime, FUTTUM.HasYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' )
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                            ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq           	
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
                                                        AND OTHJIN1.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        CONVERT(tinyint,OTHJIN2.CodeKbn)         = HAISHA.OthJinKbn2
                                                        AND OTHJIN2.TenantCdSeq = dbo.CheckCodeKbFunct('OTHJINKBN',@TenantCdSeq) --ログインユーザーのテナントSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq
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
                        --                                    ) --3/　運行指示・乗務記録情報取得で取得した受付番号     	
                        --                                    AND MFUTTU.SiyoKbn = 1
                        --                                    AND FUTTUM.SiyoKbn = 1
                        --                )
                        --                incidental
                        --                ON
                        --                                incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd     >= @UkeCdFrom --画面の受付番号（FROM）            	
                        AND YYKSHO.UkeCd <= @UkeCdTo   --画面の受付番号（TO）            	
                        --AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom                  --画面の予約区分（FROM）            	
                        --AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo                  --画面の予約区分（TO）            	
                        AND HAISHA.KSKbn    <> 1 --未仮車以外            	
                        AND HAISHA.YouTblSeq = 0 --傭車以外            	
                        --AND HAISHA.SyuKoYmd = @SyuKoYmd              --画面の出庫年月日            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq            	
                        --AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq                  --画面の出庫営業所            	
                        AND HAISHA.SiyoKbn  = 1
						AND YoyaSyu			  = 1
                        AND HAISHA.TeiDanNo = @TeiDanNo --パラメータの梯団番号            	
                        AND HAISHA.UnkRen   = @UnkRen   --パラメータの運行日連番            	
                        AND HAISHA.BunkRen  = @BunkRen  --パラメータの分割連番       
						
                        --画面で出力順に「出庫・車両コード順」を指定した場合	
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