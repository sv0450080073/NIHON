USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[RP_JomukirokuboReport]    Script Date: 2020/11/19 14:27:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[RP_JomukirokuboReport]

GO

CREATE
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
    @SortOrder      smallint AS
    IF (@UkeCdFrom != @UkeCdTo
        AND
        @SyuKoYmd!=''
        and
        @SyuEigCdSeq!=0)
        SELECT
                   ROW_NUMBER() OVER(ORDER BY
                   CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCd,', ',SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HENSYA.TenkoNo,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '????????????'
                 , HAISHA.UkeNo                                                                    AS '??????_????????????Seq'
                 , HAISHA.UnkRen                                                                   AS '??????_???????????????'
                 , HAISHA.SyaSyuRen                                                                AS '??????_????????????'
                 , HAISHA.TeiDanNo                                                                 AS '??????_????????????'
                 , HAISHA.BunkRen                                                                  AS '??????_????????????'
                 , HAISHA.HaiSSryCdSeq                                                             AS '??????_??????????????????????????????'
                 , HAISHA.GoSya                                                                    AS '??????_??????'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS '??????'
                 , HAISHA.IkNm                                                                     AS '??????'
                 , HAISHA.JyoSyaJin                                                                AS '????????????'
                 , HAISHA.DanTaNm2                                                                 AS '?????????2'
                 , UNKOBI.DanTaNm                                                                  AS '?????????'
                 , EIGYOSHO.EigyoNm                                                                AS '????????????'
                 , YYKSHO.UkeCd                                                                    AS '????????????'
                 , HENSYA.TenkoNo                                                                  AS '????????????'
                 , SYARYO.SyaRyoNm                                                                 AS '??????????????????'
                 , SYARYO.TeiCnt                                                                   AS '????????????'
                 , SYARYO.NenryoCd1Seq                                                             AS '??????????????????seq'
                 , NENRYO1.CodeKbnNm                                                               AS '??????1???'
                 , SYARYO.NenryoCd2Seq                                                             AS '??????????????????seq'
                 , NENRYO2.CodeKbnNm                                                               AS '??????2???'
                 , SYARYO.NenryoCd3Seq                                                             AS '??????????????????seq'
                 , NENRYO3.CodeKbnNm                                                               AS '??????3???'
                 , SYASYU.KataKbn                                                                  AS '?????????'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '????????????'
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
                                                           AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                                           AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                       LEFT JOIN
                                                 VPM_Syokum AS SYOKUM
                                                 ON
                                                           SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                           AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --????????????????????????????????????????????????????????????????????????     	
                                                           AND SYOKUM.JigyoKbn = 1               --????????????:(????????????)      	
                                                           AND SYOKUM.SiyoKbn  = 1
                             WHERE
                                       HAIIN.UkeNo       =HAISHA.UkeNo --3/?????????????????????????????????????????????????????????       	
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
                   , 1, 1, '') as ?????????
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
                                                           AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                                           AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                       LEFT JOIN
                                                 VPM_Syokum AS SYOKUM
                                                 ON
                                                           SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                           AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --????????????????????????????????????????????????????????????????????????     	
                                                           AND SYOKUM.JigyoKbn    = 1               --????????????:(????????????)      	
                                                           AND SYOKUM.SiyoKbn     = 1
                                                           AND SYOKUM.TenantCdSeq = @TenantCdSeq
                             WHERE
                                       HAIIN.UkeNo       =HAISHA.UkeNo --3/?????????????????????????????????????????????????????????       	
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
                   , 1, 1, '') as ???????????????
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '?????????'
                                       --BASYO.Jyus1     AS '?????????',                 	
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       BASYO.Jyus1 AS '?????????'
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       --BASYO.Jyus1     AS '?????????'                 	
                                       BASYO.Jyus2 AS '?????????'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '?????????'
                                       --BASYO.Jyus1     AS '?????????',                 	
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       BASYO.Jyus1 AS '?????????'
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       --BASYO.Jyus1     AS '?????????'                 	
                                       BASYO.Jyus2 AS '?????????'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --????????????????????????TO???       	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --????????????????????????       	
                   AND HAISHA.KSKbn      <> 1               --???????????????       	
                   AND HAISHA.YouTblSeq   = 0               --????????????       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --???????????????????????????????????????Seq       	
                   AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq    --????????????????????????       	
                   AND HAISHA.SiyoKbn     = 1
                   --??????????????????????????????????????????????????????????????????????????? 	
        order by
                   CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCd,', ',SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC
                 , CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC
                 , CASE @SortOrder
                              WHEN 3
                                         THEN concat(SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC
                 , CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HENSYA.TenkoNo,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC else if(@UkeCdFrom != @UkeCdTo
                   AND @SyuKoYmd              !=''
                   and @SyuEigCdSeq            =0)
        SELECT
                   ROW_NUMBER() OVER(ORDER BY
                   CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCd,', ',SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HENSYA.TenkoNo,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '????????????'
                 , HAISHA.UkeNo                                                                    AS '??????_????????????Seq'
                 , HAISHA.UnkRen                                                                   AS '??????_???????????????'
                 , HAISHA.SyaSyuRen                                                                AS '??????_????????????'
                 , HAISHA.TeiDanNo                                                                 AS '??????_????????????'
                 , HAISHA.BunkRen                                                                  AS '??????_????????????'
                 , HAISHA.HaiSSryCdSeq                                                             AS '??????_??????????????????????????????'
                 , HAISHA.GoSya                                                                    AS '??????_??????'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS '??????'
                 , HAISHA.IkNm                                                                     AS '??????'
                 , HAISHA.JyoSyaJin                                                                AS '????????????'
                 , HAISHA.DanTaNm2                                                                 AS '?????????2'
                 , UNKOBI.DanTaNm                                                                  AS '?????????'
                 , EIGYOSHO.EigyoNm                                                                AS '????????????'
                 , YYKSHO.UkeCd                                                                    AS '????????????'
                 , HENSYA.TenkoNo                                                                  AS '????????????'
                 , SYARYO.SyaRyoNm                                                                 AS '??????????????????'
                 , SYARYO.TeiCnt                                                                   AS '????????????'
                 , SYARYO.NenryoCd1Seq                                                             AS '??????????????????seq'
                 , NENRYO1.CodeKbnNm                                                               AS '??????1???'
                 , SYARYO.NenryoCd2Seq                                                             AS '??????????????????seq'
                 , NENRYO2.CodeKbnNm                                                               AS '??????2???'
                 , SYARYO.NenryoCd3Seq                                                             AS '??????????????????seq'
                 , NENRYO3.CodeKbnNm                                                               AS '??????3???'
                 , SYASYU.KataKbn                                                                  AS '?????????'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '????????????'
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
                                                           AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                                           AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                       LEFT JOIN
                                                 VPM_Syokum AS SYOKUM
                                                 ON
                                                           SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                           AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --????????????????????????????????????????????????????????????????????????     	
                                                           AND SYOKUM.JigyoKbn = 1               --????????????:(????????????)      	
                                                           AND SYOKUM.SiyoKbn  = 1
                             WHERE
                                       HAIIN.UkeNo       =HAISHA.UkeNo --3/?????????????????????????????????????????????????????????       	
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
                   , 1, 1, '') as ?????????
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
                                                           AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                                           AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                       LEFT JOIN
                                                 VPM_Syokum AS SYOKUM
                                                 ON
                                                           SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                           AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --????????????????????????????????????????????????????????????????????????     	
                                                           AND SYOKUM.JigyoKbn    = 1               --????????????:(????????????)      	
                                                           AND SYOKUM.SiyoKbn     = 1
                                                           AND SYOKUM.TenantCdSeq = @TenantCdSeq
                             WHERE
                                       HAIIN.UkeNo       =HAISHA.UkeNo --3/?????????????????????????????????????????????????????????       	
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
                   , 1, 1, '') as ???????????????
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '?????????'
                                       --BASYO.Jyus1     AS '?????????',                 	
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0            --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       BASYO.Jyus1 AS '?????????'
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0            --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       --BASYO.Jyus1     AS '?????????'                 	
                                       BASYO.Jyus2 AS '?????????'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0            --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '?????????'
                                       --BASYO.Jyus1     AS '?????????',                 	
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       BASYO.Jyus1 AS '?????????'
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       --BASYO.Jyus1     AS '?????????'                 	
                                       BASYO.Jyus2 AS '?????????'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --????????????????????????TO???      	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --????????????????????????       	
                   AND HAISHA.KSKbn      <> 1               --???????????????       	
                   AND HAISHA.YouTblSeq   = 0               --????????????       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --???????????????????????????????????????Seq      	
                   AND HAISHA.SiyoKbn     = 1
                   --??????????????????????????????????????????????????????????????????????????? 	
        order by
                   CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCd,', ',SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC
                 , CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC
                 , CASE @SortOrder
                              WHEN 3
                                         THEN concat(SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC
                 , CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HENSYA.TenkoNo,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC else
        SELECT
                   ROW_NUMBER() OVER(ORDER BY
                   CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCd,', ',SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HENSYA.TenkoNo,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC)                                                                        AS Row_Num
                 , YYKSHO.UkeCd                                                                    AS '????????????'
                 , HAISHA.UkeNo                                                                    AS '??????_????????????Seq'
                 , HAISHA.UnkRen                                                                   AS '??????_???????????????'
                 , HAISHA.SyaSyuRen                                                                AS '??????_????????????'
                 , HAISHA.TeiDanNo                                                                 AS '??????_????????????'
                 , HAISHA.BunkRen                                                                  AS '??????_????????????'
                 , HAISHA.HaiSSryCdSeq                                                             AS '??????_??????????????????????????????'
                 , HAISHA.GoSya                                                                    AS '??????_??????'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS '??????'
                 , HAISHA.IkNm                                                                     AS '??????'
                 , HAISHA.JyoSyaJin                                                                AS '????????????'
                 , HAISHA.DanTaNm2                                                                 AS '?????????2'
                 , UNKOBI.DanTaNm                                                                  AS '?????????'
                 , EIGYOSHO.EigyoNm                                                                AS '????????????'
                 , YYKSHO.UkeCd                                                                    AS '????????????'
                 , HENSYA.TenkoNo                                                                  AS '????????????'
                 , SYARYO.SyaRyoNm                                                                 AS '??????????????????'
                 , SYARYO.TeiCnt                                                                   AS '????????????'
                 , SYARYO.NenryoCd1Seq                                                             AS '??????????????????seq'
                 , NENRYO1.CodeKbnNm                                                               AS '??????1???'
                 , SYARYO.NenryoCd2Seq                                                             AS '??????????????????seq'
                 , NENRYO2.CodeKbnNm                                                               AS '??????2???'
                 , SYARYO.NenryoCd3Seq                                                             AS '??????????????????seq'
                 , NENRYO3.CodeKbnNm                                                               AS '??????3???'
                 , SYASYU.KataKbn                                                                  AS '?????????'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '????????????'
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
                                                           AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                                           AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                       LEFT JOIN
                                                 VPM_Syokum AS SYOKUM
                                                 ON
                                                           SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                           AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --????????????????????????????????????????????????????????????????????????     	
                                                           AND SYOKUM.JigyoKbn = 1               --????????????:(????????????)      	
                                                           AND SYOKUM.SiyoKbn  = 1
                             WHERE
                                       HAIIN.UkeNo       =HAISHA.UkeNo --3/?????????????????????????????????????????????????????????       	
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
                   , 1, 1, '') as ?????????
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
                                                           AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                                           AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/???????????????????????????????????????????????????_???????????????       	
                                       LEFT JOIN
                                                 VPM_Syokum AS SYOKUM
                                                 ON
                                                           SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                           AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --????????????????????????????????????????????????????????????????????????     	
                                                           AND SYOKUM.JigyoKbn    = 1               --????????????:(????????????)      	
                                                           AND SYOKUM.SiyoKbn     = 1
                                                           AND SYOKUM.TenantCdSeq = @TenantCdSeq
                             WHERE
                                       HAIIN.UkeNo       =HAISHA.UkeNo --3/?????????????????????????????????????????????????????????       	
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
                   , 1, 1, '') as ???????????????
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '?????????'
                                       --BASYO.Jyus1     AS '?????????',                 	
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0            --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       BASYO.Jyus1 AS '?????????'
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0            --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       --BASYO.Jyus1     AS '?????????'                 	
                                       BASYO.Jyus2 AS '?????????'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1            --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = 0            --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????common
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '?????????'
                                       --BASYO.Jyus1     AS '?????????',                 	
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       BASYO.Jyus1 AS '?????????'
                                       --BASYO.Jyus2     AS '?????????'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '?????????'                	
                                       --BASYO.Jyus1     AS '?????????'                 	
                                       BASYO.Jyus2 AS '?????????'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/?????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.UnkRen    = 1               --3/????????????????????????????????????????????????????????????                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --???????????????                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ?????????
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq      	
        WHERE
                   YYKSHO.UkeCd     >= @UkeCdFrom
                   AND YYKSHO.UkeCd <= @UkeCdTo
                   --AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom                  --????????????????????????FROM???       	
                   --AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo                  --????????????????????????TO???       	
                   --AND HAISHA.SyuKoYmd = @SyuKoYmd              --????????????????????????       	
                   AND HAISHA.KSKbn      <> 1            --???????????????       	
                   AND HAISHA.YouTblSeq   = 0            --????????????       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq       	
                   --AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq                  --????????????????????????       	
                   AND HAISHA.SiyoKbn  = 1
                   AND HAISHA.TeiDanNo = @TeiDanNo --??????????????????????????????       	
                   AND HAISHA.UnkRen   = @UnkRen   --?????????????????????????????????       	
                   AND HAISHA.BunkRen  = @BunkRen  --??????????????????????????????       	
                   --??????????????????????????????????????????????????????????????????????????? 	
        order by
                   CASE @SortOrder
                              WHEN 1
                                         THEN concat(EIGYOSHO.EigyoCd,', ',SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC
                 , CASE @SortOrder
                              WHEN 2
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime)
                   END ASC
                 , CASE @SortOrder
                              WHEN 3
                                         THEN concat(SYARYO.SyaRyoCd,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC
                 , CASE @SortOrder
                              WHEN 4
                                         THEN concat(EIGYOSHO.EigyoCd,', ',HENSYA.TenkoNo,', ',HAISHA.SyuKoYmd,', ',HAISHA.SyuKoTime,', ',HAISHA.UkeNo,', ',HAISHA.UnkRen,', ',HAISHA.TeiDanNo,', ',HAISHA.BunkRen)
                   END ASC 	
GO


