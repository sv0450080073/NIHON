/****** Object:  StoredProcedure [dbo].[RP_JomukirokuboReport]    Script Date: 2020/10/07 12:57:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE
PROCEDURE [dbo].[RP_JomukirokuboReport]
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
	
	DECLARE @count		int
			,@i			int
			,@j			int
			,@UkeNoparam		char(15)
			,@TeiDanNoparam smallint
			,@UnkRenparam smallint
			,@BunkRenpram smallint
			,@SyuKoYmdparam datetime
			,@SyuKoYmddefaultparam datetime
			,@KikYmdparam datetime
			,@ZenHaFlg tinyint
			,@KhakFlg tinyint
			,@TehNmcommon nvarchar(150)
		    ,@Jyus1common nvarchar(150)
			,@Jyus2common nvarchar(150)
			,@TehNm nvarchar(150)
			,@Jyus1 nvarchar(150)
			,@Jyus2 nvarchar(150)
	CREATE TABLE #TblHaiShaTmp
                 (
                       Row_Num bigint,
					   UkeNo char(15),
					   UnkRen smallint,
					   SyaSyuRen smallint,
					   TeiDanNo smallint,
					   BunkRen smallint,
					   HaiSSryCdSeq int,
					   GoSya char(4),
					   SyuKoYmd char(8),
					   KikYmd char(8),
					   ZenHaFlg tinyint,
					   KhakFlg tinyint,
					   IkNm nvarchar(50),
					   JyoSyaJin smallint,
					   DanTaNm2 nvarchar(100),
					   DanTaNm nvarchar(100),
					   EigyoNm nvarchar(100),
					   UkeCd int,
					   TenkoNo nvarchar(100),
					   SyaRyoNm nvarchar(100),
					   TeiCnt tinyint,
					   NenryoCd1Seq int,
					   CodeKbnNm1 nvarchar(100),
					   NenryoCd2Seq int,
					   CodeKbnNm2 nvarchar(100),
					   NenryoCd3Seq int,
					   CodeKbnNm3 varchar(100),
					   KataKbn tinyint,
					   SyaSyuDai int,
					   DriverNames nvarchar(250),
					   GuiderNames nvarchar(250),
					   EigyoCd int,
					   SyaRyoCd int,
					   SyuKoTime char(4)
                 )
    IF (@UkeCdFrom != @UkeCdTo
        AND
        @SyuKoYmd!=''
        and
        @SyuEigCdSeq!=0)
		INSERT INTO #TblHaiShaTmp
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
                 , HAISHA.UkeNo                                                                    AS '??????_????????????Seq'
                 , HAISHA.UnkRen                                                                   AS '??????_???????????????'
                 , HAISHA.SyaSyuRen                                                                AS '??????_????????????'
                 , HAISHA.TeiDanNo                                                                 AS '??????_????????????'
                 , HAISHA.BunkRen                                                                  AS '??????_????????????'
                 , HAISHA.HaiSSryCdSeq                                                             AS '??????_??????????????????????????????'
                 , HAISHA.GoSya                                                                    AS '??????_??????'
                 , HAISHA.SyuKoYmd
				 , HAISHA.KikYmd
				 ,UNKOBI.ZenHaFlg
				 ,UNKOBI.KhakFlg
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
				,EIGYOSHO.EigyoCd
				,SYARYO.SyaRyoCd
				,HAISHA.SyuKoTime
                 
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
                   AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --????????????????????????FROM???       	
                   AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --????????????????????????TO???       	
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
				   INSERT INTO #TblHaiShaTmp
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
                 , HAISHA.UkeNo                                                                    AS '??????_????????????Seq'
                 , HAISHA.UnkRen                                                                   AS '??????_???????????????'
                 , HAISHA.SyaSyuRen                                                                AS '??????_????????????'
                 , HAISHA.TeiDanNo                                                                 AS '??????_????????????'
                 , HAISHA.BunkRen                                                                  AS '??????_????????????'
                 , HAISHA.HaiSSryCdSeq                                                             AS '??????_??????????????????????????????'
                 , HAISHA.GoSya                                                                    AS '??????_??????'
                 , HAISHA.SyuKoYmd
				 , HAISHA.KikYmd
				 ,UNKOBI.ZenHaFlg
				 ,UNKOBI.KhakFlg
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
				   ,EIGYOSHO.EigyoCd
				,SYARYO.SyaRyoCd
				,HAISHA.SyuKoTime
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
                   AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --????????????????????????FROM???       	
                   AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --????????????????????????TO???       	
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
				   INSERT INTO #TblHaiShaTmp
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
                 , HAISHA.UkeNo                                                                    AS '??????_????????????Seq'
                 , HAISHA.UnkRen                                                                   AS '??????_???????????????'
                 , HAISHA.SyaSyuRen                                                                AS '??????_????????????'
                 , HAISHA.TeiDanNo                                                                 AS '??????_????????????'
                 , HAISHA.BunkRen                                                                  AS '??????_????????????'
                 , HAISHA.HaiSSryCdSeq                                                             AS '??????_??????????????????????????????'
                 , HAISHA.GoSya                                                                    AS '??????_??????'
                 ,HAISHA.SyuKoYmd
				 , HAISHA.KikYmd
				 ,UNKOBI.ZenHaFlg
				 ,UNKOBI.KhakFlg
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
                ,EIGYOSHO.EigyoCd
				,SYARYO.SyaRyoCd
				,HAISHA.SyuKoTime
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

set @i=1
select
           @count = COUNT(*)
    FROM
           #TblHaiShaTmp
CREATE TABLE #TblTeHaitmp
                 (
					   UkeNo char(15),
					   UnkRen smallint,
					   TeiDanNo smallint,
					   BunkRen smallint,
					   JyomuDate datetime,
					   TehNmcommon nvarchar(150),
					   Jyus1common nvarchar(150),
					   Jyus2common nvarchar(150),
					   TehNm nvarchar(150),
					   Jyus1 nvarchar(150),
					   Jyus2 nvarchar(150),
					   )
While(@i <= @count)
begin
SELECT
           @UkeNoparam  =UkeNo
         , @TeiDanNoparam =TeiDanNo
         , @UnkRenparam=UnkRen
         , @BunkRenpram=BunkRen
		 , @SyuKoYmdparam=convert(datetime, SyuKoYmd, 112)
		 , @SyuKoYmddefaultparam=convert(datetime, SyuKoYmd, 112)
		 ,@KikYmdparam=convert(datetime, KikYmd, 112)
		 ,@ZenHaFlg=ZenHaFlg
		 ,@KhakFlg=KhakFlg
    FROM
           #TblHaiShaTmp
    WHERE
           Row_Num=@i
set @j=1
WHILE (@SyuKoYmdparam <= @KikYmdparam)
BEGIN
	if(@ZenHaFlg=1 and @SyuKoYmdparam=@SyuKoYmddefaultparam)
	begin
	SELECT
                                       top 1
                                       @TehNmcommon=TEIHAI.TehNm                	
                                       ,@Jyus1common=BASYO.Jyus1 
                                       ,@Jyus2common=BASYO.Jyus2                	
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
                                       TEIHAI.UkeNo         = @UkeNoparam                 	
                                       AND TEIHAI.UnkRen    = @UnkRen                              	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0                  	
                                       AND TEIHAI.SiyoKbn   = 1
									   and TEIHAI.Nittei=@j
									   and TEIHAI.TomKbn=2
                                       and TEIHAI.TehaiCdSeq=1 
	SELECT
                                       top 1
                                       @TehNm=TEIHAI.TehNm                	
                                       ,@Jyus1=BASYO.Jyus1 
                                       ,@Jyus2=BASYO.Jyus2                	
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
                                       TEIHAI.UkeNo         = @UkeNoparam                 	
                                       AND TEIHAI.UnkRen    = @UnkRen                             	
                                       AND TEIHAI.TeiDanNo  = @TeiDanNo
                                       AND TEIHAI.BunkRen   = @BunkRen                  	
                                       AND TEIHAI.SiyoKbn   = 1
									   and TEIHAI.Nittei=@j
									   and TEIHAI.TomKbn=2
                                       and TEIHAI.TehaiCdSeq=1 
	end
	else if(@KhakFlg=1 and @SyuKoYmdparam=@KikYmdparam)
	begin
	SELECT
                                       top 1
                                       @TehNmcommon=TEIHAI.TehNm                	
                                       ,@Jyus1common=BASYO.Jyus1 
                                       ,@Jyus2common=BASYO.Jyus2                	
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
                                       TEIHAI.UkeNo         = @UkeNoparam                 	
                                       AND TEIHAI.UnkRen    = @UnkRen                              	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0                  	
                                       AND TEIHAI.SiyoKbn   = 1
									   and TEIHAI.Nittei=@j
									   and TEIHAI.TomKbn=3
                                       and TEIHAI.TehaiCdSeq=1 
	SELECT
                                       top 1
                                       @TehNm=TEIHAI.TehNm                	
                                       ,@Jyus1=BASYO.Jyus1 
                                       ,@Jyus2=BASYO.Jyus2                	
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
                                       TEIHAI.UkeNo         = @UkeNoparam                 	
                                       AND TEIHAI.UnkRen    = @UnkRen                             	
                                       AND TEIHAI.TeiDanNo  = @TeiDanNo
                                       AND TEIHAI.BunkRen   = @BunkRen                  	
                                       AND TEIHAI.SiyoKbn   = 1
									   and TEIHAI.Nittei=@j
									   and TEIHAI.TomKbn=3
                                       and TEIHAI.TehaiCdSeq=1 
	end
	else
	begin
	SELECT
                                       top 1
                                       @TehNmcommon=TEIHAI.TehNm                	
                                       ,@Jyus1common=BASYO.Jyus1 
                                       ,@Jyus2common=BASYO.Jyus2                	
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
                                       TEIHAI.UkeNo         = @UkeNoparam                 	
                                       AND TEIHAI.UnkRen    = @UnkRen                              	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0                  	
                                       AND TEIHAI.SiyoKbn   = 1
									   and TEIHAI.Nittei=@j
									   and TEIHAI.TomKbn=1
                                       and TEIHAI.TehaiCdSeq=1 
	SELECT
                                       top 1
                                       @TehNm=TEIHAI.TehNm                	
                                       ,@Jyus1=BASYO.Jyus1 
                                       ,@Jyus2=BASYO.Jyus2                	
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
                                       TEIHAI.UkeNo         = @UkeNoparam                 	
                                       AND TEIHAI.UnkRen    = @UnkRen                             	
                                       AND TEIHAI.TeiDanNo  = @TeiDanNo
                                       AND TEIHAI.BunkRen   = @BunkRen                  	
                                       AND TEIHAI.SiyoKbn   = 1
									   and TEIHAI.Nittei=@j
									   and TEIHAI.TomKbn=1
                                       and TEIHAI.TehaiCdSeq=1 
	end
	INSERT INTO #TblTeHaitmp
           (UkeNo
                , UnkRen
                , TeiDanNo
				,BunkRen
				,JyomuDate
				,TehNmcommon
				,Jyus1common
				,Jyus2common
				,TehNm
				,Jyus1
				,Jyus2
           )
    SELECT
             @UkeNoparam
			 ,@UnkRenparam
           , @TeiDanNoparam
		   ,@BunkRen
		   ,@SyuKoYmdparam
		   ,@TehNmcommon
		   ,@Jyus1common
		   ,@Jyus2common
		   ,@TehNm
		   ,@Jyus1
		   ,@Jyus2

 SET @SyuKoYmdparam = DATEADD(DAY, 1, @SyuKoYmdparam);
  set @TehNmcommon =null
  set @Jyus1common =null
			set @Jyus2common =null
			 set @TehNm =null
			set @Jyus1 =null
			 set @Jyus2 =null
 set @j=@j+1
end
SET @i=@i+1
end
select 
ROW_NUMBER() OVER(ORDER BY
                   CASE @SortOrder
                              WHEN 1
                                         THEN concat(Haisha.EigyoCd,', ',Haisha.SyaRyoCd,', ',Haisha.SyuKoYmd,', ',Haisha.SyuKoTime,', ',Haisha.UkeNo,', ',Haisha.UnkRen,', ',Haisha.TeiDanNo,', ',Haisha.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 2
                                         THEN concat(Haisha.EigyoCd,', ',Haisha.UkeNo,', ',Haisha.UnkRen,', ',Haisha.TeiDanNo,', ',Haisha.BunkRen,', ',Haisha.SyuKoYmd,', ',Haisha.SyuKoTime)
                   END ASC, CASE @SortOrder
                              WHEN 3
                                         THEN concat(Haisha.SyaRyoCd,', ',Haisha.SyuKoYmd,', ',Haisha.SyuKoTime,', ',Haisha.UkeNo,', ',Haisha.UnkRen,', ',Haisha.TeiDanNo,', ',Haisha.BunkRen)
                   END ASC, CASE @SortOrder
                              WHEN 4
                                         THEN concat(Haisha.EigyoCd,', ',Haisha.TenkoNo,', ',Haisha.SyuKoYmd,', ',Haisha.SyuKoTime,', ',Haisha.UkeNo,', ',Haisha.UnkRen,', ',Haisha.TeiDanNo,', ',Haisha.BunkRen)
                   END ASC)                                                                        AS Row_Num
                 , Haisha.UkeCd                                                                    AS '????????????'
                 , Haisha.UkeNo                                                                    AS '??????_????????????Seq'
                 , Haisha.UnkRen                                                                   AS '??????_???????????????'
                 , Haisha.SyaSyuRen                                                                AS '??????_????????????'
                 , Haisha.TeiDanNo                                                                 AS '??????_????????????'
                 , Haisha.BunkRen                                                                  AS '??????_????????????'
                 , Haisha.HaiSSryCdSeq                                                             AS '??????_??????????????????????????????'
                 , Haisha.GoSya                                                                    AS '??????_??????'
                 , FORMAT ( convert(datetime, Haisha.SyuKoYmd, 112), 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS '??????'
                 , Haisha.IkNm                                                                     AS '??????'
                 , Haisha.JyoSyaJin                                                                AS '????????????'
                 , Haisha.DanTaNm2                                                                 AS '?????????2'
                 , Haisha.DanTaNm                                                                  AS '?????????'
                 , Haisha.EigyoNm                                                                AS '????????????'
                 , Haisha.UkeCd                                                                    AS '????????????'
                 , Haisha.TenkoNo                                                                  AS '????????????'
                 , Haisha.SyaRyoNm                                                                 AS '??????????????????'
                 , Haisha.TeiCnt                                                                   AS '????????????'
                 , Haisha.NenryoCd1Seq                                                             AS '??????????????????seq'
                 , Haisha.CodeKbnNm1                                                               AS '??????1???'
                 , Haisha.NenryoCd2Seq                                                             AS '??????????????????seq'
                 , Haisha.CodeKbnNm2                                                               AS '??????2???'
                 , Haisha.NenryoCd3Seq                                                             AS '??????????????????seq'
                 , Haisha.CodeKbnNm3                                                               AS '??????3???'
                 , Haisha.KataKbn                                                                  AS '?????????'
				 ,Haisha.SyaSyuDai																   AS '????????????'
				 ,Haisha.DriverNames AS ?????????
				 ,Haisha.GuiderNames AS ???????????????
				 , FORMAT ( Tehai.JyomuDate, 'yyyy???MM???dd??????ddd???', 'ja-JP' ) AS 'JyomuDate'
				 ,Tehai.TehNmcommon AS ?????????common
				 ,Tehai.Jyus1common AS ?????????common
				 ,Tehai.Jyus2common AS ?????????common
				 ,Tehai.TehNm AS ?????????
				 ,Tehai.Jyus1 AS ?????????
				 ,Tehai.Jyus2 AS ?????????
from #TblTeHaitmp As Tehai
left join #TblHaiShaTmp As Haisha on Tehai.UkeNo=Haisha.UkeNo and Tehai.TeiDanNo=Haisha.TeiDanNo and Tehai.UnkRen=Haisha.UnkRen and Tehai.BunkRen=Haisha.BunkRen
 


GO