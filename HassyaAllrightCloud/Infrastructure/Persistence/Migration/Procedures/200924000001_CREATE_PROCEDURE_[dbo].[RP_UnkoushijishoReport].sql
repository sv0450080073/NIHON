/****** Object:  StoredProcedure [dbo].[RP_UnkoushijishoReport]    Script Date: 2020/09/25 9:22:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE
PROCEDURE [dbo].[RP_UnkoushijishoReport]
    @TenantCdSeq    int,
    @SyuKoYmd       char(8),
    @UkeCdFrom      int,
    @UkeCdTo        int,
    @YoyaKbnSeqFrom int,
    @YoyaKbnSeqTo   int,
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
                        END ASC)                                                                              AS Row_Num
                      , YYKSHO.UkeCd                                                                          AS '????????????'
                      , YYKSHO.TokuiTel                                                                       AS '?????????????????????'
                      , YYKSHO.TokuiTanNm                                                                     AS '?????????????????????'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )        AS '?????????_???????????????'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )         AS '?????????_???????????????'
                      , UNKOBI.DanTaNm                                                                        AS '?????????_?????????'
                      , UNKOBI.KanjJyus1                                                                      AS '?????????_???????????????'
                      , UNKOBI.KanjJyus2                                                                      AS '?????????_???????????????'
                      , UNKOBI.KanjTel                                                                        AS '?????????_??????????????????'
                      , UNKOBI.KanJNm                                                                         AS '?????????_????????????'
                      , HAISHA.UkeNo                                                                          AS '??????_????????????Seq'
                      , HAISHA.UnkRen                                                                         AS '??????_???????????????'
                      , HAISHA.SyaSyuRen                                                                      AS '??????_????????????'
                      , HAISHA.TeiDanNo                                                                       AS '??????_????????????'
                      , HAISHA.BunkRen                                                                        AS '??????_????????????'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )        AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '??????_????????????'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )         AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '??????_????????????'
                      , HAISHA.DanTaNm2                                                                       AS '??????_?????????2'
                      , HAISHA.OthJinKbn1                                                                     AS '??????_?????????????????????????????????'
                      , OTHJIN1.CodeKbnNm                                                                     AS '???????????????1'
                      , HAISHA.OthJin1                                                                        AS '??????_?????????????????????'
                      , HAISHA.OthJinKbn2                                                                     AS '??????_?????????????????????????????????'
                      , OTHJIN2.CodeKbnNm                                                                     AS '???????????????2'
                      , HAISHA.OthJin2                                                                        AS '??????_?????????????????????'
                      , HAISHA.HaiSNm                                                                         AS '??????_????????????'
                      , HAISHA.HaiSJyus1                                                                      AS '??????_??????????????????'
                      , HAISHA.HaiSJyus2                                                                      AS '??????_??????????????????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '??????_?????????????????????'
                      , HAISHA.IkNm                                                                           AS '??????_????????????'
                      , HAISHA.SyuKoYmd                                                                       AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.SyuEigCdSeq                                                                    AS '??????_???????????????Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '????????????????????????'
                      , EIGYOSHO.EigyoNm                                                                      AS '??????????????????'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.JyoSyaJin                                                                      AS '??????_????????????'
                      , HAISHA.PlusJin                                                                        AS '??????_???????????????'
                      , HAISHA.GoSya                                                                          AS '??????_??????'
                      , HAISHA.HaiSKouKNm                                                                     AS '??????_????????????????????????'
                      , HAISHA.HaiSBinNm                                                                      AS '??????_???????????????'
                      , HAISHA.TouSKouKNm                                                                     AS '??????_????????????????????????'
                      , HAISHA.TouSBinNm                                                                      AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '??????_?????????????????????'
                      , HAISHA.TouNm                                                                          AS '??????_????????????'
                      , HAISHA.TouJyusyo1                                                                     AS '??????_??????????????????'
                      , HAISHA.TouJyusyo2                                                                     AS '??????_??????????????????'
                      , YYKSYU.SyaSyuDai                                                                      AS '????????????_????????????'
                      , SYASYU.SyaSyuNm                                                                       AS '?????????'
                      , SYARYO.SyaRyoCd                                                                       AS '???????????????'
                      , SYARYO.SyaRyoNm                                                                       AS '?????????'
                      , HENSYA.TenkoNo                                                                        AS '????????????'
                      , SYARYO.KariSyaRyoNm                                                                   AS '????????????'
                      , TOKISK.TokuiNm                                                                        AS '????????????'
                      , TOKIST.SitenNm                                                                        AS '??????????????????'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '????????????_????????????sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS '???????????????'
                      , incidental.UnkRen                                                                 AS '???????????????'
                      , incidental.FutTumKbn                                                              AS '?????????????????????'
                      , incidental.FutTumRen                                                              AS '?????????????????????'
                      , incidental.FutTumNm                                                               AS '??????????????????'
                      , incidental.SeisanNm                                                               AS '?????????'
                      , incidental.FutTumCdSeq                                                            AS '?????????????????????????????????'
                      , incidental.Suryo                                                                  AS '??????'
                      , incidental.TeiDanNo                                                               AS '????????????'
                      , incidental.BunkRen                                                                AS '????????????'
                      , HAISHA.BikoNm                                                                     as '??????'
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq           	
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
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        OTHJIN1.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn1)
                                                        And OTHJIN1.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
                        LEFT OUTER JOIN
                                        (
                                                  SELECT
                                                            FUTTUM.HasYmd
                                                          , FUTTUM.UkeNo
                                                          , FUTTUM.UnkRen
                                                          , FUTTUM.FutTumKbn
                                                          , FUTTUM.FutTumRen
                                                          , FUTTUM.FutTumNm
                                                          , FUTTUM.SeisanNm
                                                          , FUTTUM.FutTumCdSeq
                                                          , MFUTTU.Suryo
                                                          , MFUTTU.TeiDanNo
                                                          , MFUTTU.BunkRen
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
                                                                          SyuKoYmd   = @SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/?????????????????????????????????????????????????????????????????????     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --????????????????????????FROM???            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --????????????????????????TO???            	
                        AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --????????????????????????FROM???            	
                        AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --????????????????????????TO???            	
                        AND HAISHA.KSKbn      <> 1               --???????????????            	
                        AND HAISHA.YouTblSeq   = 0               --????????????            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --????????????????????????            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --???????????????????????????????????????Seq
                        and HAISHA.SyuEigCdSeq = @SyuEigCdSeq
                        AND HAISHA.SiyoKbn     = 1
        ORDER BY
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
                        END ASC)                                                                              AS Row_Num
                      , YYKSHO.UkeCd                                                                          AS '????????????'
                      , YYKSHO.TokuiTel                                                                       AS '?????????????????????'
                      , YYKSHO.TokuiTanNm                                                                     AS '?????????????????????'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )        AS '?????????_???????????????'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )         AS '?????????_???????????????'
                      , UNKOBI.DanTaNm                                                                        AS '?????????_?????????'
                      , UNKOBI.KanjJyus1                                                                      AS '?????????_???????????????'
                      , UNKOBI.KanjJyus2                                                                      AS '?????????_???????????????'
                      , UNKOBI.KanjTel                                                                        AS '?????????_??????????????????'
                      , UNKOBI.KanJNm                                                                         AS '?????????_????????????'
                      , HAISHA.UkeNo                                                                          AS '??????_????????????Seq'
                      , HAISHA.UnkRen                                                                         AS '??????_???????????????'
                      , HAISHA.SyaSyuRen                                                                      AS '??????_????????????'
                      , HAISHA.TeiDanNo                                                                       AS '??????_????????????'
                      , HAISHA.BunkRen                                                                        AS '??????_????????????'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )        AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '??????_????????????'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )         AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '??????_????????????'
                      , HAISHA.DanTaNm2                                                                       AS '??????_?????????2'
                      , HAISHA.OthJinKbn1                                                                     AS '??????_?????????????????????????????????'
                      , OTHJIN1.CodeKbnNm                                                                     AS '???????????????1'
                      , HAISHA.OthJin1                                                                        AS '??????_?????????????????????'
                      , HAISHA.OthJinKbn2                                                                     AS '??????_?????????????????????????????????'
                      , OTHJIN2.CodeKbnNm                                                                     AS '???????????????2'
                      , HAISHA.OthJin2                                                                        AS '??????_?????????????????????'
                      , HAISHA.HaiSNm                                                                         AS '??????_????????????'
                      , HAISHA.HaiSJyus1                                                                      AS '??????_??????????????????'
                      , HAISHA.HaiSJyus2                                                                      AS '??????_??????????????????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '??????_?????????????????????'
                      , HAISHA.IkNm                                                                           AS '??????_????????????'
                      , HAISHA.SyuKoYmd                                                                       AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.SyuEigCdSeq                                                                    AS '??????_???????????????Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '????????????????????????'
                      , EIGYOSHO.EigyoNm                                                                      AS '??????????????????'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.JyoSyaJin                                                                      AS '??????_????????????'
                      , HAISHA.PlusJin                                                                        AS '??????_???????????????'
                      , HAISHA.GoSya                                                                          AS '??????_??????'
                      , HAISHA.HaiSKouKNm                                                                     AS '??????_????????????????????????'
                      , HAISHA.HaiSBinNm                                                                      AS '??????_???????????????'
                      , HAISHA.TouSKouKNm                                                                     AS '??????_????????????????????????'
                      , HAISHA.TouSBinNm                                                                      AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '??????_?????????????????????'
                      , HAISHA.TouNm                                                                          AS '??????_????????????'
                      , HAISHA.TouJyusyo1                                                                     AS '??????_??????????????????'
                      , HAISHA.TouJyusyo2                                                                     AS '??????_??????????????????'
                      , YYKSYU.SyaSyuDai                                                                      AS '????????????_????????????'
                      , SYASYU.SyaSyuNm                                                                       AS '?????????'
                      , SYARYO.SyaRyoCd                                                                       AS '???????????????'
                      , SYARYO.SyaRyoNm                                                                       AS '?????????'
                      , HENSYA.TenkoNo                                                                        AS '????????????'
                      , SYARYO.KariSyaRyoNm                                                                   AS '????????????'
                      , TOKISK.TokuiNm                                                                        AS '????????????'
                      , TOKIST.SitenNm                                                                        AS '??????????????????'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '????????????_????????????sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS '???????????????'
                      , incidental.UnkRen                                                                 AS '???????????????'
                      , incidental.FutTumKbn                                                              AS '?????????????????????'
                      , incidental.FutTumRen                                                              AS '?????????????????????'
                      , incidental.FutTumNm                                                               AS '??????????????????'
                      , incidental.SeisanNm                                                               AS '?????????'
                      , incidental.FutTumCdSeq                                                            AS '?????????????????????????????????'
                      , incidental.Suryo                                                                  AS '??????'
                      , incidental.TeiDanNo                                                               AS '????????????'
                      , incidental.BunkRen                                                                AS '????????????'
                      , HAISHA.BikoNm                                                                     as '??????'
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq           	
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
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        OTHJIN1.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn1)
                                                        AND OTHJIN1.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
                        LEFT OUTER JOIN
                                        (
                                                  SELECT
                                                            FUTTUM.HasYmd
                                                          , FUTTUM.UkeNo
                                                          , FUTTUM.UnkRen
                                                          , FUTTUM.FutTumKbn
                                                          , FUTTUM.FutTumRen
                                                          , FUTTUM.FutTumNm
                                                          , FUTTUM.SeisanNm
                                                          , FUTTUM.FutTumCdSeq
                                                          , MFUTTU.Suryo
                                                          , MFUTTU.TeiDanNo
                                                          , MFUTTU.BunkRen
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
                                                                          SyuKoYmd   = @SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/?????????????????????????????????????????????????????????????????????     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --????????????????????????FROM???            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --????????????????????????TO???            	
                        AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --????????????????????????FROM???            	
                        AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --????????????????????????TO???            	
                        AND HAISHA.KSKbn      <> 1               --???????????????            	
                        AND HAISHA.YouTblSeq   = 0               --????????????            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --????????????????????????            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --???????????????????????????????????????Seq
                        AND HAISHA.SiyoKbn     = 1
        ORDER BY
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
                        END ASC ELSE
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
                        END ASC)                                                                              AS Row_Num
                      , YYKSHO.UkeCd                                                                          AS '????????????'
                      , YYKSHO.TokuiTel                                                                       AS '?????????????????????'
                      , YYKSHO.TokuiTanNm                                                                     AS '?????????????????????'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )        AS '?????????_???????????????'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )         AS '?????????_???????????????'
                      , UNKOBI.DanTaNm                                                                        AS '?????????_?????????'
                      , UNKOBI.KanjJyus1                                                                      AS '?????????_???????????????'
                      , UNKOBI.KanjJyus2                                                                      AS '?????????_???????????????'
                      , UNKOBI.KanjTel                                                                        AS '?????????_??????????????????'
                      , UNKOBI.KanJNm                                                                         AS '?????????_????????????'
                      , HAISHA.UkeNo                                                                          AS '??????_????????????Seq'
                      , HAISHA.UnkRen                                                                         AS '??????_???????????????'
                      , HAISHA.SyaSyuRen                                                                      AS '??????_????????????'
                      , HAISHA.TeiDanNo                                                                       AS '??????_????????????'
                      , HAISHA.BunkRen                                                                        AS '??????_????????????'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )        AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '??????_????????????'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' )         AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '??????_????????????'
					   , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '??????_????????????'
                      , HAISHA.DanTaNm2                                                                       AS '??????_?????????2'
                      , HAISHA.OthJinKbn1                                                                     AS '??????_?????????????????????????????????'
                      , OTHJIN1.CodeKbnNm                                                                     AS '???????????????1'
                      , HAISHA.OthJin1                                                                        AS '??????_?????????????????????'
                      , HAISHA.OthJinKbn2                                                                     AS '??????_?????????????????????????????????'
                      , OTHJIN2.CodeKbnNm                                                                     AS '???????????????2'
                      , HAISHA.OthJin2                                                                        AS '??????_?????????????????????'
                      , HAISHA.HaiSNm                                                                         AS '??????_????????????'
                      , HAISHA.HaiSJyus1                                                                      AS '??????_??????????????????'
                      , HAISHA.HaiSJyus2                                                                      AS '??????_??????????????????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '??????_?????????????????????'
                      , HAISHA.IkNm                                                                           AS '??????_????????????'
                      , HAISHA.SyuKoYmd                                                                       AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.SyuEigCdSeq                                                                    AS '??????_???????????????Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '????????????????????????'
                      , EIGYOSHO.EigyoNm                                                                      AS '??????????????????'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '??????_????????????'
                      , HAISHA.JyoSyaJin                                                                      AS '??????_????????????'
                      , HAISHA.PlusJin                                                                        AS '??????_???????????????'
                      , HAISHA.GoSya                                                                          AS '??????_??????'
                      , HAISHA.HaiSKouKNm                                                                     AS '??????_????????????????????????'
                      , HAISHA.HaiSBinNm                                                                      AS '??????_???????????????'
                      , HAISHA.TouSKouKNm                                                                     AS '??????_????????????????????????'
                      , HAISHA.TouSBinNm                                                                      AS '??????_???????????????'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '??????_?????????????????????'
                      , HAISHA.TouNm                                                                          AS '??????_????????????'
                      , HAISHA.TouJyusyo1                                                                     AS '??????_??????????????????'
                      , HAISHA.TouJyusyo2                                                                     AS '??????_??????????????????'
                      , YYKSYU.SyaSyuDai                                                                      AS '????????????_????????????'
                      , SYASYU.SyaSyuNm                                                                       AS '?????????'
                      , SYARYO.SyaRyoCd                                                                       AS '???????????????'
                      , SYARYO.SyaRyoNm                                                                       AS '?????????'
                      , HENSYA.TenkoNo                                                                        AS '????????????'
                      , SYARYO.KariSyaRyoNm                                                                   AS '????????????'
                      , TOKISK.TokuiNm                                                                        AS '????????????'
                      , TOKIST.SitenNm                                                                        AS '??????????????????'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '????????????_????????????sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS '???????????????'
                      , incidental.UnkRen                                                                 AS '???????????????'
                      , incidental.FutTumKbn                                                              AS '?????????????????????'
                      , incidental.FutTumRen                                                              AS '?????????????????????'
                      , incidental.FutTumNm                                                               AS '??????????????????'
                      , incidental.SeisanNm                                                               AS '?????????'
                      , incidental.FutTumCdSeq                                                            AS '?????????????????????????????????'
                      , incidental.Suryo                                                                  AS '??????'
                      , incidental.TeiDanNo                                                               AS '????????????'
                      , incidental.BunkRen                                                                AS '????????????'
                      , HAISHA.BikoNm                                                                     as '??????'
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
                        , 1, 1, '') as ???????????????
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq           	
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
                        LEFT JOIN
                                        VPM_SyaRyo AS SYARYO
                                        ON
                                                        SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                        LEFT JOIN
                                        VPM_HenSya AS HENSYA
                                        ON
                                                        HENSYA.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
                                                        AND HENSYA.StaYmd <= HAISHA.HaiSYmd
                                                        AND HENSYA.EndYmd >= HAISHA.HaiSYmd
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN1
                                        ON
                                                        OTHJIN1.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn1)
                                                        AND OTHJIN1.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq
                        LEFT OUTER JOIN
                                        (
                                                  SELECT
                                                            FUTTUM.HasYmd
                                                          , FUTTUM.UkeNo
                                                          , FUTTUM.UnkRen
                                                          , FUTTUM.FutTumKbn
                                                          , FUTTUM.FutTumRen
                                                          , FUTTUM.FutTumNm
                                                          , FUTTUM.SeisanNm
                                                          , FUTTUM.FutTumCdSeq
                                                          , MFUTTU.Suryo
                                                          , MFUTTU.TeiDanNo
                                                          , MFUTTU.BunkRen
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
                                                                          SyuKoYmd   = @SyuKoYmd
                                                                          and SiyoKbn=1
                                                            ) --3/?????????????????????????????????????????????????????????????????????     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd     >= @UkeCdFrom --????????????????????????FROM???            	
                        AND YYKSHO.UkeCd <= @UkeCdTo   --????????????????????????TO???            	
                        --AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom                  --????????????????????????FROM???            	
                        --AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo                  --????????????????????????TO???            	
                        AND HAISHA.KSKbn    <> 1 --???????????????            	
                        AND HAISHA.YouTblSeq = 0 --????????????            	
                        --AND HAISHA.SyuKoYmd = @SyuKoYmd              --????????????????????????            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???????????????????????????????????????Seq            	
                        --AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq                  --????????????????????????            	
                        AND HAISHA.SiyoKbn  = 1
                        AND HAISHA.TeiDanNo = @TeiDanNo --??????????????????????????????            	
                        AND HAISHA.UnkRen   = @UnkRen   --?????????????????????????????????            	
                        AND HAISHA.BunkRen  = @BunkRen  --??????????????????????????????            	
                        --???????????????????????????????????????????????????????????????????????????	
        ORDER BY
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