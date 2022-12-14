USE [HOC_Kashikiri_New2109]
GO

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
                      , YYKSHO.UkeCd                                                                          AS '???t????'
                      , YYKSHO.TokuiTel                                                                       AS '???????d?b????'
                      , YYKSHO.TokuiTanNm                                                                     AS '???????S??????'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )        AS '?^?s??_?z???N????'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )         AS '?^?s??_?????N????'
                      , UNKOBI.DanTaNm                                                                        AS '?^?s??_?c????'
                      , UNKOBI.KanjJyus1                                                                      AS '?^?s??_?????Z???P'
                      , UNKOBI.KanjJyus2                                                                      AS '?^?s??_?????Z???Q'
                      , UNKOBI.KanjTel                                                                        AS '?^?s??_?????d?b????'
                      , UNKOBI.KanJNm                                                                         AS '?^?s??_????????'
                      , HAISHA.UkeNo                                                                          AS '?z??_???t????Seq'
                      , HAISHA.UnkRen                                                                         AS '?z??_?^?s???A??'
                      , HAISHA.SyaSyuRen                                                                      AS '?z??_?????A??'
                      , HAISHA.TeiDanNo                                                                       AS '?z??_???c????'
                      , HAISHA.BunkRen                                                                        AS '?z??_?????A??'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )        AS '?z??_?z???N????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '?z??_?z??????'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )         AS '?z??_?????N????'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '?z??_????????'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '?z??_?A??????'
                      , HAISHA.DanTaNm2                                                                       AS '?z??_?c????2'
                      , HAISHA.OthJinKbn1                                                                     AS '?z??_???????l???????R?[?h?P'
                      , OTHJIN1.CodeKbnNm                                                                     AS '???????l??1'
                      , HAISHA.OthJin1                                                                        AS '?z??_???????l???P??'
                      , HAISHA.OthJinKbn2                                                                     AS '?z??_???????l???????R?[?h?Q'
                      , OTHJIN2.CodeKbnNm                                                                     AS '???????l??2'
                      , HAISHA.OthJin2                                                                        AS '?z??_???????l???Q??'
                      , HAISHA.HaiSNm                                                                         AS '?z??_?z???n??'
                      , HAISHA.HaiSJyus1                                                                      AS '?z??_?z???n?Z???P'
                      , HAISHA.HaiSJyus2                                                                      AS '?z??_?z???n?Z???Q'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '?z??_?z???n????????'
                      , HAISHA.IkNm                                                                           AS '?z??_?s??????'
                      , HAISHA.SyuKoYmd                                                                       AS '?z??_?o???N????'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '?z??_?o??????'
                      , HAISHA.SyuEigCdSeq                                                                    AS '?z??_?o???c????Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '?o???c?????R?[?h'
                      , EIGYOSHO.EigyoNm                                                                      AS '?o???c??????'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '?z??_?o??????'
                      , HAISHA.JyoSyaJin                                                                      AS '?z??_?????l??'
                      , HAISHA.PlusJin                                                                        AS '?z??_?v???X?l??'
                      , HAISHA.GoSya                                                                          AS '?z??_????'
                      , HAISHA.HaiSKouKNm                                                                     AS '?z??_?z???n?????@????'
                      , HAISHA.HaiSBinNm                                                                      AS '?z??_?z???n????'
                      , HAISHA.TouSKouKNm                                                                     AS '?z??_?????n?????@????'
                      , HAISHA.TouSBinNm                                                                      AS '?z??_?????n????'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '?z??_?????n????????'
                      , HAISHA.TouNm                                                                          AS '?z??_?????n??'
                      , HAISHA.TouJyusyo1                                                                     AS '?z??_?????n?Z???P'
                      , HAISHA.TouJyusyo2                                                                     AS '?z??_?????n?Z???Q'
                      , YYKSYU.SyaSyuDai                                                                      AS '?\??????_????????'
                      , SYASYU.SyaSyuNm                                                                       AS '??????'
                      , SYARYO.SyaRyoCd                                                                       AS '?????R?[?h'
                      , SYARYO.SyaRyoNm                                                                       AS '??????'
                      , HENSYA.TenkoNo                                                                        AS '?????_??'
                      , SYARYO.KariSyaRyoNm                                                                   AS '????????'
                      , TOKISK.TokuiNm                                                                        AS '????????'
                      , TOKIST.SitenNm                                                                        AS '???????x?X??'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '?\??????_????????sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' ) AS '?????N????'
                      , incidental.UnkRen                                                                 AS '?^?s???A??'
                      , incidental.FutTumKbn                                                              AS '?t???????i????'
                      , incidental.FutTumRen                                                              AS '?t???????i?A??'
                      , incidental.FutTumNm                                                               AS '?t???????i??'
                      , incidental.SeisanNm                                                               AS '???Z??'
                      , incidental.FutTumCdSeq                                                            AS '?t???????i?R?[?h?r?d?p'
                      , incidental.Suryo                                                                  AS '????'
                      , incidental.TeiDanNo                                                               AS '???c????'
                      , incidental.BunkRen                                                                AS '?????A??'
                      , HAISHA.BikoNm                                                                     as '???l'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --?????????F?^?]???A?_???^?]???A?K?C?h?A?_???K?C?h     	
                                                                AND SYOKUM.JigyoKbn = 1               --????????:(?P?F????)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/?@?^?s?w?????????????????????????t????       	
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
                        , 1, 1, '') as ??????
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --?????????F?^?]???A?_???^?]???A?K?C?h?A?_???K?C?h     	
                                                                AND SYOKUM.JigyoKbn    = 1               --????????:(?P?F????)      	
                                                                AND SYOKUM.SiyoKbn     = 1
                                                                AND SYOKUM.TenantCdSeq = @TenantCdSeq
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/?@?^?s?w?????????????????????????t????       	
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
                        , 1, 1, '') as ?????E????
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq           	
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
                                                        And OTHJIN1.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
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
                                                            ) --3/?@?^?s?w???E?????L?^?????????????????????t????     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --?????????t?????iFROM?j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --?????????t?????iTO?j            	
                        AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --???????\???????iFROM?j            	
                        AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --???????\???????iTO?j            	
                        AND HAISHA.KSKbn      <> 1               --?????????O            	
                        AND HAISHA.YouTblSeq   = 0               --?b?????O            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --???????o???N????            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --???O?C?????[?U?[???e?i???gSeq
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
                      , YYKSHO.UkeCd                                                                          AS '???t????'
                      , YYKSHO.TokuiTel                                                                       AS '???????d?b????'
                      , YYKSHO.TokuiTanNm                                                                     AS '???????S??????'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )        AS '?^?s??_?z???N????'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )         AS '?^?s??_?????N????'
                      , UNKOBI.DanTaNm                                                                        AS '?^?s??_?c????'
                      , UNKOBI.KanjJyus1                                                                      AS '?^?s??_?????Z???P'
                      , UNKOBI.KanjJyus2                                                                      AS '?^?s??_?????Z???Q'
                      , UNKOBI.KanjTel                                                                        AS '?^?s??_?????d?b????'
                      , UNKOBI.KanJNm                                                                         AS '?^?s??_????????'
                      , HAISHA.UkeNo                                                                          AS '?z??_???t????Seq'
                      , HAISHA.UnkRen                                                                         AS '?z??_?^?s???A??'
                      , HAISHA.SyaSyuRen                                                                      AS '?z??_?????A??'
                      , HAISHA.TeiDanNo                                                                       AS '?z??_???c????'
                      , HAISHA.BunkRen                                                                        AS '?z??_?????A??'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )        AS '?z??_?z???N????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '?z??_?z??????'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )         AS '?z??_?????N????'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '?z??_????????'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '?z??_?A??????'
                      , HAISHA.DanTaNm2                                                                       AS '?z??_?c????2'
                      , HAISHA.OthJinKbn1                                                                     AS '?z??_???????l???????R?[?h?P'
                      , OTHJIN1.CodeKbnNm                                                                     AS '???????l??1'
                      , HAISHA.OthJin1                                                                        AS '?z??_???????l???P??'
                      , HAISHA.OthJinKbn2                                                                     AS '?z??_???????l???????R?[?h?Q'
                      , OTHJIN2.CodeKbnNm                                                                     AS '???????l??2'
                      , HAISHA.OthJin2                                                                        AS '?z??_???????l???Q??'
                      , HAISHA.HaiSNm                                                                         AS '?z??_?z???n??'
                      , HAISHA.HaiSJyus1                                                                      AS '?z??_?z???n?Z???P'
                      , HAISHA.HaiSJyus2                                                                      AS '?z??_?z???n?Z???Q'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '?z??_?z???n????????'
                      , HAISHA.IkNm                                                                           AS '?z??_?s??????'
                      , HAISHA.SyuKoYmd                                                                       AS '?z??_?o???N????'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '?z??_?o??????'
                      , HAISHA.SyuEigCdSeq                                                                    AS '?z??_?o???c????Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '?o???c?????R?[?h'
                      , EIGYOSHO.EigyoNm                                                                      AS '?o???c??????'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '?z??_?o??????'
                      , HAISHA.JyoSyaJin                                                                      AS '?z??_?????l??'
                      , HAISHA.PlusJin                                                                        AS '?z??_?v???X?l??'
                      , HAISHA.GoSya                                                                          AS '?z??_????'
                      , HAISHA.HaiSKouKNm                                                                     AS '?z??_?z???n?????@????'
                      , HAISHA.HaiSBinNm                                                                      AS '?z??_?z???n????'
                      , HAISHA.TouSKouKNm                                                                     AS '?z??_?????n?????@????'
                      , HAISHA.TouSBinNm                                                                      AS '?z??_?????n????'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '?z??_?????n????????'
                      , HAISHA.TouNm                                                                          AS '?z??_?????n??'
                      , HAISHA.TouJyusyo1                                                                     AS '?z??_?????n?Z???P'
                      , HAISHA.TouJyusyo2                                                                     AS '?z??_?????n?Z???Q'
                      , YYKSYU.SyaSyuDai                                                                      AS '?\??????_????????'
                      , SYASYU.SyaSyuNm                                                                       AS '??????'
                      , SYARYO.SyaRyoCd                                                                       AS '?????R?[?h'
                      , SYARYO.SyaRyoNm                                                                       AS '??????'
                      , HENSYA.TenkoNo                                                                        AS '?????_??'
                      , SYARYO.KariSyaRyoNm                                                                   AS '????????'
                      , TOKISK.TokuiNm                                                                        AS '????????'
                      , TOKIST.SitenNm                                                                        AS '???????x?X??'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '?\??????_????????sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' ) AS '?????N????'
                      , incidental.UnkRen                                                                 AS '?^?s???A??'
                      , incidental.FutTumKbn                                                              AS '?t???????i????'
                      , incidental.FutTumRen                                                              AS '?t???????i?A??'
                      , incidental.FutTumNm                                                               AS '?t???????i??'
                      , incidental.SeisanNm                                                               AS '???Z??'
                      , incidental.FutTumCdSeq                                                            AS '?t???????i?R?[?h?r?d?p'
                      , incidental.Suryo                                                                  AS '????'
                      , incidental.TeiDanNo                                                               AS '???c????'
                      , incidental.BunkRen                                                                AS '?????A??'
                      , HAISHA.BikoNm                                                                     as '???l'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --?????????F?^?]???A?_???^?]???A?K?C?h?A?_???K?C?h     	
                                                                AND SYOKUM.JigyoKbn = 1               --????????:(?P?F????)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/?@?^?s?w?????????????????????????t????       	
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
                        , 1, 1, '') as ??????
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --?????????F?^?]???A?_???^?]???A?K?C?h?A?_???K?C?h     	
                                                                AND SYOKUM.JigyoKbn    = 1               --????????:(?P?F????)      	
                                                                AND SYOKUM.SiyoKbn     = 1
                                                                AND SYOKUM.TenantCdSeq = @TenantCdSeq
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/?@?^?s?w?????????????????????????t????       	
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
                        , 1, 1, '') as ?????E????
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq           	
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
                                                        AND OTHJIN1.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
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
                                                            ) --3/?@?^?s?w???E?????L?^?????????????????????t????     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --?????????t?????iFROM?j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --?????????t?????iTO?j            	
                        AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --???????\???????iFROM?j            	
                        AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --???????\???????iTO?j            	
                        AND HAISHA.KSKbn      <> 1               --?????????O            	
                        AND HAISHA.YouTblSeq   = 0               --?b?????O            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --???????o???N????            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --???O?C?????[?U?[???e?i???gSeq
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
                      , YYKSHO.UkeCd                                                                          AS '???t????'
                      , YYKSHO.TokuiTel                                                                       AS '???????d?b????'
                      , YYKSHO.TokuiTanNm                                                                     AS '???????S??????'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )        AS '?^?s??_?z???N????'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )         AS '?^?s??_?????N????'
                      , UNKOBI.DanTaNm                                                                        AS '?^?s??_?c????'
                      , UNKOBI.KanjJyus1                                                                      AS '?^?s??_?????Z???P'
                      , UNKOBI.KanjJyus2                                                                      AS '?^?s??_?????Z???Q'
                      , UNKOBI.KanjTel                                                                        AS '?^?s??_?????d?b????'
                      , UNKOBI.KanJNm                                                                         AS '?^?s??_????????'
                      , HAISHA.UkeNo                                                                          AS '?z??_???t????Seq'
                      , HAISHA.UnkRen                                                                         AS '?z??_?^?s???A??'
                      , HAISHA.SyaSyuRen                                                                      AS '?z??_?????A??'
                      , HAISHA.TeiDanNo                                                                       AS '?z??_???c????'
                      , HAISHA.BunkRen                                                                        AS '?z??_?????A??'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )        AS '?z??_?z???N????'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '?z??_?z??????'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' )         AS '?z??_?????N????'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '?z??_????????'
					   , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '?z??_?A??????'
                      , HAISHA.DanTaNm2                                                                       AS '?z??_?c????2'
                      , HAISHA.OthJinKbn1                                                                     AS '?z??_???????l???????R?[?h?P'
                      , OTHJIN1.CodeKbnNm                                                                     AS '???????l??1'
                      , HAISHA.OthJin1                                                                        AS '?z??_???????l???P??'
                      , HAISHA.OthJinKbn2                                                                     AS '?z??_???????l???????R?[?h?Q'
                      , OTHJIN2.CodeKbnNm                                                                     AS '???????l??2'
                      , HAISHA.OthJin2                                                                        AS '?z??_???????l???Q??'
                      , HAISHA.HaiSNm                                                                         AS '?z??_?z???n??'
                      , HAISHA.HaiSJyus1                                                                      AS '?z??_?z???n?Z???P'
                      , HAISHA.HaiSJyus2                                                                      AS '?z??_?z???n?Z???Q'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '?z??_?z???n????????'
                      , HAISHA.IkNm                                                                           AS '?z??_?s??????'
                      , HAISHA.SyuKoYmd                                                                       AS '?z??_?o???N????'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '?z??_?o??????'
                      , HAISHA.SyuEigCdSeq                                                                    AS '?z??_?o???c????Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '?o???c?????R?[?h'
                      , EIGYOSHO.EigyoNm                                                                      AS '?o???c??????'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '?z??_?o??????'
                      , HAISHA.JyoSyaJin                                                                      AS '?z??_?????l??'
                      , HAISHA.PlusJin                                                                        AS '?z??_?v???X?l??'
                      , HAISHA.GoSya                                                                          AS '?z??_????'
                      , HAISHA.HaiSKouKNm                                                                     AS '?z??_?z???n?????@????'
                      , HAISHA.HaiSBinNm                                                                      AS '?z??_?z???n????'
                      , HAISHA.TouSKouKNm                                                                     AS '?z??_?????n?????@????'
                      , HAISHA.TouSBinNm                                                                      AS '?z??_?????n????'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '?z??_?????n????????'
                      , HAISHA.TouNm                                                                          AS '?z??_?????n??'
                      , HAISHA.TouJyusyo1                                                                     AS '?z??_?????n?Z???P'
                      , HAISHA.TouJyusyo2                                                                     AS '?z??_?????n?Z???Q'
                      , YYKSYU.SyaSyuDai                                                                      AS '?\??????_????????'
                      , SYASYU.SyaSyuNm                                                                       AS '??????'
                      , SYARYO.SyaRyoCd                                                                       AS '?????R?[?h'
                      , SYARYO.SyaRyoNm                                                                       AS '??????'
                      , HENSYA.TenkoNo                                                                        AS '?????_??'
                      , SYARYO.KariSyaRyoNm                                                                   AS '????????'
                      , TOKISK.TokuiNm                                                                        AS '????????'
                      , TOKIST.SitenNm                                                                        AS '???????x?X??'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '?\??????_????????sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy?NMM??dd???iddd?j', 'ja-JP' ) AS '?????N????'
                      , incidental.UnkRen                                                                 AS '?^?s???A??'
                      , incidental.FutTumKbn                                                              AS '?t???????i????'
                      , incidental.FutTumRen                                                              AS '?t???????i?A??'
                      , incidental.FutTumNm                                                               AS '?t???????i??'
                      , incidental.SeisanNm                                                               AS '???Z??'
                      , incidental.FutTumCdSeq                                                            AS '?t???????i?R?[?h?r?d?p'
                      , incidental.Suryo                                                                  AS '????'
                      , incidental.TeiDanNo                                                               AS '???c????'
                      , incidental.BunkRen                                                                AS '?????A??'
                      , HAISHA.BikoNm                                                                     as '???l'
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --?????????F?^?]???A?_???^?]???A?K?C?h?A?_???K?C?h     	
                                                                AND SYOKUM.JigyoKbn = 1               --????????:(?P?F????)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/?@?^?s?w?????????????????????????t????       	
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
                        , 1, 1, '') as ??????
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
                                                                AND KYOSHE.StaYmd <= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                                                AND KYOSHE.EndYmd >= HAISHA.HaiSYmd --3/?@?^?s?w???????????????????????z??_?o???N????       	
                                            LEFT JOIN
                                                      VPM_Syokum AS SYOKUM
                                                      ON
                                                                SYOKUM.SyokumuCdSeq = KYOSHE.SyokumuCdSeq
                                                                AND SYOKUM.SyokumuKbn IN (1, 2, 3, 4) --?????????F?^?]???A?_???^?]???A?K?C?h?A?_???K?C?h     	
                                                                AND SYOKUM.JigyoKbn = 1               --????????:(?P?F????)      	
                                                                AND SYOKUM.SiyoKbn  = 1
                                  WHERE
                                            HAIIN.UkeNo       =HAISHA.UkeNo --3/?@?^?s?w?????????????????????????t????       	
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
                        , 1, 1, '') as ?????E????
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
                                                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq           	
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
                                                        AND OTHJIN1.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
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
                                                        AND KAISHA.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq
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
                                                            ) --3/?@?^?s?w???E?????L?^?????????????????????t????     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd     >= @UkeCdFrom --?????????t?????iFROM?j            	
                        AND YYKSHO.UkeCd <= @UkeCdTo   --?????????t?????iTO?j            	
                        --AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom                  --???????\???????iFROM?j            	
                        --AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo                  --???????\???????iTO?j            	
                        AND HAISHA.KSKbn    <> 1 --?????????O            	
                        AND HAISHA.YouTblSeq = 0 --?b?????O            	
                        --AND HAISHA.SyuKoYmd = @SyuKoYmd              --???????o???N????            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq --???O?C?????[?U?[???e?i???gSeq            	
                        --AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq                  --???????o???c????            	
                        AND HAISHA.SiyoKbn  = 1
                        AND HAISHA.TeiDanNo = @TeiDanNo --?p?????[?^?????c????            	
                        AND HAISHA.UnkRen   = @UnkRen   --?p?????[?^???^?s???A??            	
                        AND HAISHA.BunkRen  = @BunkRen  --?p?????[?^???????A??            	
                        --???????o???????u?o???E?????R?[?h???v???w??????????	
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


