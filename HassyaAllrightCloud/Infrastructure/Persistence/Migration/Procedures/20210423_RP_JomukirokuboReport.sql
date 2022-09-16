USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[RP_JomukirokuboReport]    Script Date: 2021/04/29 14:06:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER
PROCEDURE [dbo].[RP_JomukirokuboReport]
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
	if(@UkenoList!='' )
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
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HAISHA.UkeNo                                                                    AS '配車_受付番号Seq'
                 , HAISHA.UnkRen                                                                   AS '配車_運行日連番'
                 , HAISHA.SyaSyuRen                                                                AS '配車_車種連番'
                 , HAISHA.TeiDanNo                                                                 AS '配車_悌団番号'
                 , HAISHA.BunkRen                                                                  AS '配車_分割連番'
                 , HAISHA.HaiSSryCdSeq                                                             AS '配車_配車車輌コードＳＥＱ'
                 , HAISHA.GoSya                                                                    AS '配車_号車'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '日時'
                 , HAISHA.IkNm                                                                     AS '行先'
                 , HAISHA.JyoSyaJin                                                                AS '乗車人員'
                 , HAISHA.DanTaNm2                                                                 AS '団体名2'
                 , UNKOBI.DanTaNm                                                                  AS '団体名'
                 , EIGYOSHO.EigyoNm                                                                AS '事業者名'
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HENSYA.TenkoNo                                                                  AS '車両点呼'
                 , SYARYO.SyaRyoNm                                                                 AS '車輌登録番号'
                 , SYARYO.TeiCnt                                                                   AS '乗車定員'
                 , SYARYO.NenryoCd1Seq                                                             AS '燃料コード１seq'
                 , NENRYO1.CodeKbnNm                                                               AS '燃料1名'
                 , SYARYO.NenryoCd2Seq                                                             AS '燃料コード２seq'
                 , NENRYO2.CodeKbnNm                                                               AS '燃料2名'
                 , SYARYO.NenryoCd3Seq                                                             AS '燃料コード３seq'
                 , NENRYO3.CodeKbnNm                                                               AS '燃料3名'
                 , SYASYU.KataKbn                                                                  AS '型区分'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '車種台数'
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
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              TKD_Unkobi AS UNKOBI
                              ON
                                         UNKOBI.UkeNo      = HAISHA.UkeNo
                                         AND UNKOBI.UnkRen = HAISHA.UnkRen
                   LEFT JOIN
                              VPM_HenSya AS HENSYA
                              ON
                                         HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                         AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                         AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                   LEFT JOIN
                              VPM_SyaRyo AS SYARYO
                              ON
                                         SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                   LEFT JOIN
                              VPM_SyaSyu AS SYASYU
                              ON
                                         SYASYU.SyaSyuCdSeq     = SYARYO.SyaSyuCdSeq
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
        WHERE
                   concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))
                   AND HAISHA.KSKbn      <> 1               --未仮車以外       	
                   AND HAISHA.YouTblSeq   = 0               --傭車以外       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq       
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --画面で出力順に「出庫・車両コード順」を指定した場合 	
        order by
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
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HAISHA.UkeNo                                                                    AS '配車_受付番号Seq'
                 , HAISHA.UnkRen                                                                   AS '配車_運行日連番'
                 , HAISHA.SyaSyuRen                                                                AS '配車_車種連番'
                 , HAISHA.TeiDanNo                                                                 AS '配車_悌団番号'
                 , HAISHA.BunkRen                                                                  AS '配車_分割連番'
                 , HAISHA.HaiSSryCdSeq                                                             AS '配車_配車車輌コードＳＥＱ'
                 , HAISHA.GoSya                                                                    AS '配車_号車'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy年MM月dd日(ddd)', 'ja-JP' ) AS '日時'
                 , HAISHA.IkNm                                                                     AS '行先'
                 , HAISHA.JyoSyaJin                                                                AS '乗車人員'
                 , HAISHA.DanTaNm2                                                                 AS '団体名2'
                 , UNKOBI.DanTaNm                                                                  AS '団体名'
                 , EIGYOSHO.EigyoNm                                                                AS '事業者名'
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HENSYA.TenkoNo                                                                  AS '車両点呼'
                 , SYARYO.SyaRyoNm                                                                 AS '車輌登録番号'
                 , SYARYO.TeiCnt                                                                   AS '乗車定員'
                 , SYARYO.NenryoCd1Seq                                                             AS '燃料コード１seq'
                 , NENRYO1.CodeKbnNm                                                               AS '燃料1名'
                 , SYARYO.NenryoCd2Seq                                                             AS '燃料コード２seq'
                 , NENRYO2.CodeKbnNm                                                               AS '燃料2名'
                 , SYARYO.NenryoCd3Seq                                                             AS '燃料コード３seq'
                 , NENRYO3.CodeKbnNm                                                               AS '燃料3名'
                 , SYASYU.KataKbn                                                                  AS '型区分'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '車種台数'
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
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              TKD_Unkobi AS UNKOBI
                              ON
                                         UNKOBI.UkeNo      = HAISHA.UkeNo
                                         AND UNKOBI.UnkRen = HAISHA.UnkRen
                   LEFT JOIN
                              VPM_HenSya AS HENSYA
                              ON
                                         HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                         AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                         AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                   LEFT JOIN
                              VPM_SyaRyo AS SYARYO
                              ON
                                         SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                   LEFT JOIN
                              VPM_SyaSyu AS SYASYU
                              ON
                                         SYASYU.SyaSyuCdSeq     = SYARYO.SyaSyuCdSeq
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
        WHERE
                  concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000'),FORMAT(HAISHA.TeiDanNo, '000'),FORMAT(HAISHA.BunkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))
                   AND HAISHA.KSKbn      <> 1               --未仮車以外       	
                   AND HAISHA.YouTblSeq   = 0               --傭車以外       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq       
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --画面で出力順に「出庫・車両コード順」を指定した場合 	
        order by
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
    else 
 IF (@UkeCdFrom != @UkeCdTo
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
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HAISHA.UkeNo                                                                    AS '配車_受付番号Seq'
                 , HAISHA.UnkRen                                                                   AS '配車_運行日連番'
                 , HAISHA.SyaSyuRen                                                                AS '配車_車種連番'
                 , HAISHA.TeiDanNo                                                                 AS '配車_悌団番号'
                 , HAISHA.BunkRen                                                                  AS '配車_分割連番'
                 , HAISHA.HaiSSryCdSeq                                                             AS '配車_配車車輌コードＳＥＱ'
                 , HAISHA.GoSya                                                                    AS '配車_号車'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '日時'
                 , HAISHA.IkNm                                                                     AS '行先'
                 , HAISHA.JyoSyaJin                                                                AS '乗車人員'
                 , HAISHA.DanTaNm2                                                                 AS '団体名2'
                 , UNKOBI.DanTaNm                                                                  AS '団体名'
                 , EIGYOSHO.EigyoNm                                                                AS '事業者名'
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HENSYA.TenkoNo                                                                  AS '車両点呼'
                 , SYARYO.SyaRyoNm                                                                 AS '車輌登録番号'
                 , SYARYO.TeiCnt                                                                   AS '乗車定員'
                 , SYARYO.NenryoCd1Seq                                                             AS '燃料コード１seq'
                 , NENRYO1.CodeKbnNm                                                               AS '燃料1名'
                 , SYARYO.NenryoCd2Seq                                                             AS '燃料コード２seq'
                 , NENRYO2.CodeKbnNm                                                               AS '燃料2名'
                 , SYARYO.NenryoCd3Seq                                                             AS '燃料コード３seq'
                 , NENRYO3.CodeKbnNm                                                               AS '燃料3名'
                 , SYASYU.KataKbn                                                                  AS '型区分'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '車種台数'
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
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              TKD_Unkobi AS UNKOBI
                              ON
                                         UNKOBI.UkeNo      = HAISHA.UkeNo
                                         AND UNKOBI.UnkRen = HAISHA.UnkRen
                   LEFT JOIN
                              VPM_HenSya AS HENSYA
                              ON
                                         HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                         AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                         AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                   LEFT JOIN
                              VPM_SyaRyo AS SYARYO
                              ON
                                         SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                   LEFT JOIN
                              VPM_SyaSyu AS SYASYU
                              ON
                                         SYASYU.SyaSyuCdSeq     = SYARYO.SyaSyuCdSeq
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）       	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日       	
                   AND HAISHA.KSKbn      <> 1               --未仮車以外       	
                   AND HAISHA.YouTblSeq   = 0               --傭車以外       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq       	
                   AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq    --画面の出庫営業所       	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --画面で出力順に「出庫・車両コード順」を指定した場合 	
        order by
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
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HAISHA.UkeNo                                                                    AS '配車_受付番号Seq'
                 , HAISHA.UnkRen                                                                   AS '配車_運行日連番'
                 , HAISHA.SyaSyuRen                                                                AS '配車_車種連番'
                 , HAISHA.TeiDanNo                                                                 AS '配車_悌団番号'
                 , HAISHA.BunkRen                                                                  AS '配車_分割連番'
                 , HAISHA.HaiSSryCdSeq                                                             AS '配車_配車車輌コードＳＥＱ'
                 , HAISHA.GoSya                                                                    AS '配車_号車'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '日時'
                 , HAISHA.IkNm                                                                     AS '行先'
                 , HAISHA.JyoSyaJin                                                                AS '乗車人員'
                 , HAISHA.DanTaNm2                                                                 AS '団体名2'
                 , UNKOBI.DanTaNm                                                                  AS '団体名'
                 , EIGYOSHO.EigyoNm                                                                AS '事業者名'
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HENSYA.TenkoNo                                                                  AS '車両点呼'
                 , SYARYO.SyaRyoNm                                                                 AS '車輌登録番号'
                 , SYARYO.TeiCnt                                                                   AS '乗車定員'
                 , SYARYO.NenryoCd1Seq                                                             AS '燃料コード１seq'
                 , NENRYO1.CodeKbnNm                                                               AS '燃料1名'
                 , SYARYO.NenryoCd2Seq                                                             AS '燃料コード２seq'
                 , NENRYO2.CodeKbnNm                                                               AS '燃料2名'
                 , SYARYO.NenryoCd3Seq                                                             AS '燃料コード３seq'
                 , NENRYO3.CodeKbnNm                                                               AS '燃料3名'
                 , SYASYU.KataKbn                                                                  AS '型区分'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '車種台数'
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
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              TKD_Unkobi AS UNKOBI
                              ON
                                         UNKOBI.UkeNo      = HAISHA.UkeNo
                                         AND UNKOBI.UnkRen = HAISHA.UnkRen
                   LEFT JOIN
                              VPM_HenSya AS HENSYA
                              ON
                                         HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                         AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                         AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                   LEFT JOIN
                              VPM_SyaRyo AS SYARYO
                              ON
                                         SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                   LEFT JOIN
                              VPM_SyaSyu AS SYASYU
                              ON
                                         SYASYU.SyaSyuCdSeq     = SYARYO.SyaSyuCdSeq
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）      	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日       	
                   AND HAISHA.KSKbn      <> 1               --未仮車以外       	
                   AND HAISHA.YouTblSeq   = 0               --傭車以外       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq      	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --画面で出力順に「出庫・車両コード順」を指定した場合 	
        order by
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
IF (@UkeCdFrom = @UkeCdTo
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
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HAISHA.UkeNo                                                                    AS '配車_受付番号Seq'
                 , HAISHA.UnkRen                                                                   AS '配車_運行日連番'
                 , HAISHA.SyaSyuRen                                                                AS '配車_車種連番'
                 , HAISHA.TeiDanNo                                                                 AS '配車_悌団番号'
                 , HAISHA.BunkRen                                                                  AS '配車_分割連番'
                 , HAISHA.HaiSSryCdSeq                                                             AS '配車_配車車輌コードＳＥＱ'
                 , HAISHA.GoSya                                                                    AS '配車_号車'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '日時'
                 , HAISHA.IkNm                                                                     AS '行先'
                 , HAISHA.JyoSyaJin                                                                AS '乗車人員'
                 , HAISHA.DanTaNm2                                                                 AS '団体名2'
                 , UNKOBI.DanTaNm                                                                  AS '団体名'
                 , EIGYOSHO.EigyoNm                                                                AS '事業者名'
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HENSYA.TenkoNo                                                                  AS '車両点呼'
                 , SYARYO.SyaRyoNm                                                                 AS '車輌登録番号'
                 , SYARYO.TeiCnt                                                                   AS '乗車定員'
                 , SYARYO.NenryoCd1Seq                                                             AS '燃料コード１seq'
                 , NENRYO1.CodeKbnNm                                                               AS '燃料1名'
                 , SYARYO.NenryoCd2Seq                                                             AS '燃料コード２seq'
                 , NENRYO2.CodeKbnNm                                                               AS '燃料2名'
                 , SYARYO.NenryoCd3Seq                                                             AS '燃料コード３seq'
                 , NENRYO3.CodeKbnNm                                                               AS '燃料3名'
                 , SYASYU.KataKbn                                                                  AS '型区分'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '車種台数'
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
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              TKD_Unkobi AS UNKOBI
                              ON
                                         UNKOBI.UkeNo      = HAISHA.UkeNo
                                         AND UNKOBI.UnkRen = HAISHA.UnkRen
                   LEFT JOIN
                              VPM_HenSya AS HENSYA
                              ON
                                         HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                         AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                         AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                   LEFT JOIN
                              VPM_SyaRyo AS SYARYO
                              ON
                                         SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                   LEFT JOIN
                              VPM_SyaSyu AS SYASYU
                              ON
                                         SYASYU.SyaSyuCdSeq     = SYARYO.SyaSyuCdSeq
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）       	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日       	
                   AND HAISHA.KSKbn      <> 1               --未仮車以外       	
                   AND HAISHA.YouTblSeq   = 0               --傭車以外       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq       	
                   AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq    --画面の出庫営業所       	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --画面で出力順に「出庫・車両コード順」を指定した場合 	
        order by
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
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HAISHA.UkeNo                                                                    AS '配車_受付番号Seq'
                 , HAISHA.UnkRen                                                                   AS '配車_運行日連番'
                 , HAISHA.SyaSyuRen                                                                AS '配車_車種連番'
                 , HAISHA.TeiDanNo                                                                 AS '配車_悌団番号'
                 , HAISHA.BunkRen                                                                  AS '配車_分割連番'
                 , HAISHA.HaiSSryCdSeq                                                             AS '配車_配車車輌コードＳＥＱ'
                 , HAISHA.GoSya                                                                    AS '配車_号車'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '日時'
                 , HAISHA.IkNm                                                                     AS '行先'
                 , HAISHA.JyoSyaJin                                                                AS '乗車人員'
                 , HAISHA.DanTaNm2                                                                 AS '団体名2'
                 , UNKOBI.DanTaNm                                                                  AS '団体名'
                 , EIGYOSHO.EigyoNm                                                                AS '事業者名'
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HENSYA.TenkoNo                                                                  AS '車両点呼'
                 , SYARYO.SyaRyoNm                                                                 AS '車輌登録番号'
                 , SYARYO.TeiCnt                                                                   AS '乗車定員'
                 , SYARYO.NenryoCd1Seq                                                             AS '燃料コード１seq'
                 , NENRYO1.CodeKbnNm                                                               AS '燃料1名'
                 , SYARYO.NenryoCd2Seq                                                             AS '燃料コード２seq'
                 , NENRYO2.CodeKbnNm                                                               AS '燃料2名'
                 , SYARYO.NenryoCd3Seq                                                             AS '燃料コード３seq'
                 , NENRYO3.CodeKbnNm                                                               AS '燃料3名'
                 , SYASYU.KataKbn                                                                  AS '型区分'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '車種台数'
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
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              TKD_Unkobi AS UNKOBI
                              ON
                                         UNKOBI.UkeNo      = HAISHA.UkeNo
                                         AND UNKOBI.UnkRen = HAISHA.UnkRen
                   LEFT JOIN
                              VPM_HenSya AS HENSYA
                              ON
                                         HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                         AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                         AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                   LEFT JOIN
                              VPM_SyaRyo AS SYARYO
                              ON
                                         SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                   LEFT JOIN
                              VPM_SyaSyu AS SYASYU
                              ON
                                         SYASYU.SyaSyuCdSeq     = SYARYO.SyaSyuCdSeq
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --画面の予約区分（TO）      	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --画面の出庫年月日       	
                   AND HAISHA.KSKbn      <> 1               --未仮車以外       	
                   AND HAISHA.YouTblSeq   = 0               --傭車以外       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --ログインユーザーのテナントSeq      	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --画面で出力順に「出庫・車両コード順」を指定した場合 	
        order by
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
else
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
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HAISHA.UkeNo                                                                    AS '配車_受付番号Seq'
                 , HAISHA.UnkRen                                                                   AS '配車_運行日連番'
                 , HAISHA.SyaSyuRen                                                                AS '配車_車種連番'
                 , HAISHA.TeiDanNo                                                                 AS '配車_悌団番号'
                 , HAISHA.BunkRen                                                                  AS '配車_分割連番'
                 , HAISHA.HaiSSryCdSeq                                                             AS '配車_配車車輌コードＳＥＱ'
                 , HAISHA.GoSya                                                                    AS '配車_号車'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy年MM月dd日（ddd）', 'ja-JP' ) AS '日時'
                 , HAISHA.IkNm                                                                     AS '行先'
                 , HAISHA.JyoSyaJin                                                                AS '乗車人員'
                 , HAISHA.DanTaNm2                                                                 AS '団体名2'
                 , UNKOBI.DanTaNm                                                                  AS '団体名'
                 , EIGYOSHO.EigyoNm                                                                AS '事業者名'
                 , YYKSHO.UkeCd                                                                    AS '受付番号'
                 , HENSYA.TenkoNo                                                                  AS '車両点呼'
                 , SYARYO.SyaRyoNm                                                                 AS '車輌登録番号'
                 , SYARYO.TeiCnt                                                                   AS '乗車定員'
                 , SYARYO.NenryoCd1Seq                                                             AS '燃料コード１seq'
                 , NENRYO1.CodeKbnNm                                                               AS '燃料1名'
                 , SYARYO.NenryoCd2Seq                                                             AS '燃料コード２seq'
                 , NENRYO2.CodeKbnNm                                                               AS '燃料2名'
                 , SYARYO.NenryoCd3Seq                                                             AS '燃料コード３seq'
                 , NENRYO3.CodeKbnNm                                                               AS '燃料3名'
                 , SYASYU.KataKbn                                                                  AS '型区分'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '車種台数'
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
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1            --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = 0            --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '手配先'
                                       --BASYO.Jyus1     AS '住所１',                 	
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 手配先
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       BASYO.Jyus1 AS '住所１'
                                       --BASYO.Jyus2     AS '住所２'                	
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所１
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '手配先'                	
                                       --BASYO.Jyus1     AS '住所１'                 	
                                       BASYO.Jyus2 AS '住所２'
                             FROM
                                       TKD_Tehai AS TEIHAI
                                       LEFT JOIN
                                                 TKD_Unkobi AS UNKOBI
                                                 ON
                                                           TEIHAI.UkeNo      = UNKOBI.UkeNo
                                                           AND TEIHAI.UnkRen = UNKOBI.UnkRen
                                       LEFT JOIN
                                                 VPM_Basyo AS BASYO
                                                 ON
                                                           BASYO.TehaiCdSeq        = TEIHAI.TehaiCdSeq
                                                           and BASYO.BasyoMapCdSeq = TEIHAI.TehMapCdSeq
                             WHERE
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/　運行指示書情報取得で取得した受付番号                  	
                                       AND TEIHAI.UnkRen    = 1               --3/　運行指示書情報取得で取得した運行日連番                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --共通の手配                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as 住所２
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              TKD_Unkobi AS UNKOBI
                              ON
                                         UNKOBI.UkeNo      = HAISHA.UkeNo
                                         AND UNKOBI.UnkRen = HAISHA.UnkRen
                   LEFT JOIN
                              VPM_HenSya AS HENSYA
                              ON
                                         HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                         AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                         AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                   LEFT JOIN
                              VPM_SyaRyo AS SYARYO
                              ON
                                         SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                   LEFT JOIN
                              VPM_SyaSyu AS SYASYU
                              ON
                                         SYASYU.SyaSyuCdSeq     = SYARYO.SyaSyuCdSeq
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq      	
        WHERE
                   YYKSHO.UkeCd     >= @UkeCdFrom
                   AND YYKSHO.UkeCd <= @UkeCdTo
                   --AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom                  --画面の予約区分（FROM）       	
                   --AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo                  --画面の予約区分（TO）       	
                   --AND HAISHA.SyuKoYmd = @SyuKoYmd              --画面の出庫年月日       	
                   AND HAISHA.KSKbn      <> 1            --未仮車以外       	
                   AND HAISHA.YouTblSeq   = 0            --傭車以外       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナントSeq       	
                   --AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq                  --画面の出庫営業所       	
                   AND HAISHA.SiyoKbn  = 1
				   AND YoyaSyu			  = 1
                   AND HAISHA.TeiDanNo = @TeiDanNo --パラメータの梯団番号       	
                   AND HAISHA.UnkRen   = @UnkRen   --パラメータの運行日連番       	
                   AND HAISHA.BunkRen  = @BunkRen  --パラメータの分割連番       	
                   --画面で出力順に「出庫・車両コード順」を指定した場合 	
        order by
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


