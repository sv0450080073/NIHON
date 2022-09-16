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
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HAISHA.UkeNo                                                                    AS '�z��_��t�ԍ�Seq'
                 , HAISHA.UnkRen                                                                   AS '�z��_�^�s���A��'
                 , HAISHA.SyaSyuRen                                                                AS '�z��_�Ԏ�A��'
                 , HAISHA.TeiDanNo                                                                 AS '�z��_��c�ԍ�'
                 , HAISHA.BunkRen                                                                  AS '�z��_�����A��'
                 , HAISHA.HaiSSryCdSeq                                                             AS '�z��_�z�Ԏ��q�R�[�h�r�d�p'
                 , HAISHA.GoSya                                                                    AS '�z��_����'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '����'
                 , HAISHA.IkNm                                                                     AS '�s��'
                 , HAISHA.JyoSyaJin                                                                AS '��Ԑl��'
                 , HAISHA.DanTaNm2                                                                 AS '�c�̖�2'
                 , UNKOBI.DanTaNm                                                                  AS '�c�̖�'
                 , EIGYOSHO.EigyoNm                                                                AS '���ƎҖ�'
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HENSYA.TenkoNo                                                                  AS '�ԗ��_��'
                 , SYARYO.SyaRyoNm                                                                 AS '���q�o�^�ԍ�'
                 , SYARYO.TeiCnt                                                                   AS '��Ԓ��'
                 , SYARYO.NenryoCd1Seq                                                             AS '�R���R�[�h�Pseq'
                 , NENRYO1.CodeKbnNm                                                               AS '�R��1��'
                 , SYARYO.NenryoCd2Seq                                                             AS '�R���R�[�h�Qseq'
                 , NENRYO2.CodeKbnNm                                                               AS '�R��2��'
                 , SYARYO.NenryoCd3Seq                                                             AS '�R���R�[�h�Rseq'
                 , NENRYO3.CodeKbnNm                                                               AS '�R��3��'
                 , SYASYU.KataKbn                                                                  AS '�^�敪'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '�Ԏ�䐔'
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
                   , 1, 1, '') as �Ј���
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
                   , 1, 1, '') as �Ј��E����
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Pcommon
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Qcommon
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���P
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Q
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
        WHERE
                   concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))
                   AND HAISHA.KSKbn      <> 1               --�����ԈȊO       	
                   AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq       
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ 	
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
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HAISHA.UkeNo                                                                    AS '�z��_��t�ԍ�Seq'
                 , HAISHA.UnkRen                                                                   AS '�z��_�^�s���A��'
                 , HAISHA.SyaSyuRen                                                                AS '�z��_�Ԏ�A��'
                 , HAISHA.TeiDanNo                                                                 AS '�z��_��c�ԍ�'
                 , HAISHA.BunkRen                                                                  AS '�z��_�����A��'
                 , HAISHA.HaiSSryCdSeq                                                             AS '�z��_�z�Ԏ��q�R�[�h�r�d�p'
                 , HAISHA.GoSya                                                                    AS '�z��_����'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy�NMM��dd��(ddd)', 'ja-JP' ) AS '����'
                 , HAISHA.IkNm                                                                     AS '�s��'
                 , HAISHA.JyoSyaJin                                                                AS '��Ԑl��'
                 , HAISHA.DanTaNm2                                                                 AS '�c�̖�2'
                 , UNKOBI.DanTaNm                                                                  AS '�c�̖�'
                 , EIGYOSHO.EigyoNm                                                                AS '���ƎҖ�'
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HENSYA.TenkoNo                                                                  AS '�ԗ��_��'
                 , SYARYO.SyaRyoNm                                                                 AS '���q�o�^�ԍ�'
                 , SYARYO.TeiCnt                                                                   AS '��Ԓ��'
                 , SYARYO.NenryoCd1Seq                                                             AS '�R���R�[�h�Pseq'
                 , NENRYO1.CodeKbnNm                                                               AS '�R��1��'
                 , SYARYO.NenryoCd2Seq                                                             AS '�R���R�[�h�Qseq'
                 , NENRYO2.CodeKbnNm                                                               AS '�R��2��'
                 , SYARYO.NenryoCd3Seq                                                             AS '�R���R�[�h�Rseq'
                 , NENRYO3.CodeKbnNm                                                               AS '�R��3��'
                 , SYASYU.KataKbn                                                                  AS '�^�敪'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '�Ԏ�䐔'
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
                   , 1, 1, '') as �Ј���
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
                   , 1, 1, '') as �Ј��E����
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Pcommon
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Qcommon
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���P
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Q
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
        WHERE
                  concat(HAISHA.UkeNo,FORMAT(HAISHA.UnkRen, '000'),FORMAT(HAISHA.TeiDanNo, '000'),FORMAT(HAISHA.BunkRen, '000')) IN (select * from FN_SplitString(@UkenoList, ','))
                   AND HAISHA.KSKbn      <> 1               --�����ԈȊO       	
                   AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq       
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ 	
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
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HAISHA.UkeNo                                                                    AS '�z��_��t�ԍ�Seq'
                 , HAISHA.UnkRen                                                                   AS '�z��_�^�s���A��'
                 , HAISHA.SyaSyuRen                                                                AS '�z��_�Ԏ�A��'
                 , HAISHA.TeiDanNo                                                                 AS '�z��_��c�ԍ�'
                 , HAISHA.BunkRen                                                                  AS '�z��_�����A��'
                 , HAISHA.HaiSSryCdSeq                                                             AS '�z��_�z�Ԏ��q�R�[�h�r�d�p'
                 , HAISHA.GoSya                                                                    AS '�z��_����'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '����'
                 , HAISHA.IkNm                                                                     AS '�s��'
                 , HAISHA.JyoSyaJin                                                                AS '��Ԑl��'
                 , HAISHA.DanTaNm2                                                                 AS '�c�̖�2'
                 , UNKOBI.DanTaNm                                                                  AS '�c�̖�'
                 , EIGYOSHO.EigyoNm                                                                AS '���ƎҖ�'
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HENSYA.TenkoNo                                                                  AS '�ԗ��_��'
                 , SYARYO.SyaRyoNm                                                                 AS '���q�o�^�ԍ�'
                 , SYARYO.TeiCnt                                                                   AS '��Ԓ��'
                 , SYARYO.NenryoCd1Seq                                                             AS '�R���R�[�h�Pseq'
                 , NENRYO1.CodeKbnNm                                                               AS '�R��1��'
                 , SYARYO.NenryoCd2Seq                                                             AS '�R���R�[�h�Qseq'
                 , NENRYO2.CodeKbnNm                                                               AS '�R��2��'
                 , SYARYO.NenryoCd3Seq                                                             AS '�R���R�[�h�Rseq'
                 , NENRYO3.CodeKbnNm                                                               AS '�R��3��'
                 , SYASYU.KataKbn                                                                  AS '�^�敪'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '�Ԏ�䐔'
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
                   , 1, 1, '') as �Ј���
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
                   , 1, 1, '') as �Ј��E����
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Pcommon
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Qcommon
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���P
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Q
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j       	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����       	
                   AND HAISHA.KSKbn      <> 1               --�����ԈȊO       	
                   AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq       	
                   AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq    --��ʂ̏o�ɉc�Ə�       	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ 	
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
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HAISHA.UkeNo                                                                    AS '�z��_��t�ԍ�Seq'
                 , HAISHA.UnkRen                                                                   AS '�z��_�^�s���A��'
                 , HAISHA.SyaSyuRen                                                                AS '�z��_�Ԏ�A��'
                 , HAISHA.TeiDanNo                                                                 AS '�z��_��c�ԍ�'
                 , HAISHA.BunkRen                                                                  AS '�z��_�����A��'
                 , HAISHA.HaiSSryCdSeq                                                             AS '�z��_�z�Ԏ��q�R�[�h�r�d�p'
                 , HAISHA.GoSya                                                                    AS '�z��_����'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '����'
                 , HAISHA.IkNm                                                                     AS '�s��'
                 , HAISHA.JyoSyaJin                                                                AS '��Ԑl��'
                 , HAISHA.DanTaNm2                                                                 AS '�c�̖�2'
                 , UNKOBI.DanTaNm                                                                  AS '�c�̖�'
                 , EIGYOSHO.EigyoNm                                                                AS '���ƎҖ�'
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HENSYA.TenkoNo                                                                  AS '�ԗ��_��'
                 , SYARYO.SyaRyoNm                                                                 AS '���q�o�^�ԍ�'
                 , SYARYO.TeiCnt                                                                   AS '��Ԓ��'
                 , SYARYO.NenryoCd1Seq                                                             AS '�R���R�[�h�Pseq'
                 , NENRYO1.CodeKbnNm                                                               AS '�R��1��'
                 , SYARYO.NenryoCd2Seq                                                             AS '�R���R�[�h�Qseq'
                 , NENRYO2.CodeKbnNm                                                               AS '�R��2��'
                 , SYARYO.NenryoCd3Seq                                                             AS '�R���R�[�h�Rseq'
                 , NENRYO3.CodeKbnNm                                                               AS '�R��3��'
                 , SYASYU.KataKbn                                                                  AS '�^�敪'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '�Ԏ�䐔'
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
                   , 1, 1, '') as �Ј���
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
                   , 1, 1, '') as �Ј��E����
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Pcommon
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Qcommon
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���P
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Q
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j      	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����       	
                   AND HAISHA.KSKbn      <> 1               --�����ԈȊO       	
                   AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq      	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ 	
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
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HAISHA.UkeNo                                                                    AS '�z��_��t�ԍ�Seq'
                 , HAISHA.UnkRen                                                                   AS '�z��_�^�s���A��'
                 , HAISHA.SyaSyuRen                                                                AS '�z��_�Ԏ�A��'
                 , HAISHA.TeiDanNo                                                                 AS '�z��_��c�ԍ�'
                 , HAISHA.BunkRen                                                                  AS '�z��_�����A��'
                 , HAISHA.HaiSSryCdSeq                                                             AS '�z��_�z�Ԏ��q�R�[�h�r�d�p'
                 , HAISHA.GoSya                                                                    AS '�z��_����'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '����'
                 , HAISHA.IkNm                                                                     AS '�s��'
                 , HAISHA.JyoSyaJin                                                                AS '��Ԑl��'
                 , HAISHA.DanTaNm2                                                                 AS '�c�̖�2'
                 , UNKOBI.DanTaNm                                                                  AS '�c�̖�'
                 , EIGYOSHO.EigyoNm                                                                AS '���ƎҖ�'
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HENSYA.TenkoNo                                                                  AS '�ԗ��_��'
                 , SYARYO.SyaRyoNm                                                                 AS '���q�o�^�ԍ�'
                 , SYARYO.TeiCnt                                                                   AS '��Ԓ��'
                 , SYARYO.NenryoCd1Seq                                                             AS '�R���R�[�h�Pseq'
                 , NENRYO1.CodeKbnNm                                                               AS '�R��1��'
                 , SYARYO.NenryoCd2Seq                                                             AS '�R���R�[�h�Qseq'
                 , NENRYO2.CodeKbnNm                                                               AS '�R��2��'
                 , SYARYO.NenryoCd3Seq                                                             AS '�R���R�[�h�Rseq'
                 , NENRYO3.CodeKbnNm                                                               AS '�R��3��'
                 , SYASYU.KataKbn                                                                  AS '�^�敪'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '�Ԏ�䐔'
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
                   , 1, 1, '') as �Ј���
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
                   , 1, 1, '') as �Ј��E����
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Pcommon
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0
                                       AND TEIHAI.BunkRen   = 0--���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Qcommon
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���P
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z	
                                       AND TEIHAI.BunkRen   = HAISHA.BunkRen
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Q
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j       	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����       	
                   AND HAISHA.KSKbn      <> 1               --�����ԈȊO       	
                   AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq       	
                   AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq    --��ʂ̏o�ɉc�Ə�       	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ 	
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
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HAISHA.UkeNo                                                                    AS '�z��_��t�ԍ�Seq'
                 , HAISHA.UnkRen                                                                   AS '�z��_�^�s���A��'
                 , HAISHA.SyaSyuRen                                                                AS '�z��_�Ԏ�A��'
                 , HAISHA.TeiDanNo                                                                 AS '�z��_��c�ԍ�'
                 , HAISHA.BunkRen                                                                  AS '�z��_�����A��'
                 , HAISHA.HaiSSryCdSeq                                                             AS '�z��_�z�Ԏ��q�R�[�h�r�d�p'
                 , HAISHA.GoSya                                                                    AS '�z��_����'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '����'
                 , HAISHA.IkNm                                                                     AS '�s��'
                 , HAISHA.JyoSyaJin                                                                AS '��Ԑl��'
                 , HAISHA.DanTaNm2                                                                 AS '�c�̖�2'
                 , UNKOBI.DanTaNm                                                                  AS '�c�̖�'
                 , EIGYOSHO.EigyoNm                                                                AS '���ƎҖ�'
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HENSYA.TenkoNo                                                                  AS '�ԗ��_��'
                 , SYARYO.SyaRyoNm                                                                 AS '���q�o�^�ԍ�'
                 , SYARYO.TeiCnt                                                                   AS '��Ԓ��'
                 , SYARYO.NenryoCd1Seq                                                             AS '�R���R�[�h�Pseq'
                 , NENRYO1.CodeKbnNm                                                               AS '�R��1��'
                 , SYARYO.NenryoCd2Seq                                                             AS '�R���R�[�h�Qseq'
                 , NENRYO2.CodeKbnNm                                                               AS '�R��2��'
                 , SYARYO.NenryoCd3Seq                                                             AS '�R���R�[�h�Rseq'
                 , NENRYO3.CodeKbnNm                                                               AS '�R��3��'
                 , SYASYU.KataKbn                                                                  AS '�^�敪'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '�Ԏ�䐔'
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
                   , 1, 1, '') as �Ј���
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
                   , 1, 1, '') as �Ј��E����
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Pcommon
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Qcommon
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���P
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Q
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
        WHERE
                   YYKSHO.UkeCd          >= @UkeCdFrom
                   AND YYKSHO.UkeCd      <= @UkeCdTo
                   AND YYKSHO.YoyaKbnSeq IN (select * from FN_SplitString(@YoyaKbnSeqList, '-'))   --��ʂ̗\��敪�iTO�j      	
                   AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����       	
                   AND HAISHA.KSKbn      <> 1               --�����ԈȊO       	
                   AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq      	
                   AND HAISHA.SiyoKbn     = 1
				   AND YoyaSyu			  = 1
                   --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ 	
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
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HAISHA.UkeNo                                                                    AS '�z��_��t�ԍ�Seq'
                 , HAISHA.UnkRen                                                                   AS '�z��_�^�s���A��'
                 , HAISHA.SyaSyuRen                                                                AS '�z��_�Ԏ�A��'
                 , HAISHA.TeiDanNo                                                                 AS '�z��_��c�ԍ�'
                 , HAISHA.BunkRen                                                                  AS '�z��_�����A��'
                 , HAISHA.HaiSSryCdSeq                                                             AS '�z��_�z�Ԏ��q�R�[�h�r�d�p'
                 , HAISHA.GoSya                                                                    AS '�z��_����'
                 , FORMAT ( convert(datetime, HAISHA.SyuKoYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '����'
                 , HAISHA.IkNm                                                                     AS '�s��'
                 , HAISHA.JyoSyaJin                                                                AS '��Ԑl��'
                 , HAISHA.DanTaNm2                                                                 AS '�c�̖�2'
                 , UNKOBI.DanTaNm                                                                  AS '�c�̖�'
                 , EIGYOSHO.EigyoNm                                                                AS '���ƎҖ�'
                 , YYKSHO.UkeCd                                                                    AS '��t�ԍ�'
                 , HENSYA.TenkoNo                                                                  AS '�ԗ��_��'
                 , SYARYO.SyaRyoNm                                                                 AS '���q�o�^�ԍ�'
                 , SYARYO.TeiCnt                                                                   AS '��Ԓ��'
                 , SYARYO.NenryoCd1Seq                                                             AS '�R���R�[�h�Pseq'
                 , NENRYO1.CodeKbnNm                                                               AS '�R��1��'
                 , SYARYO.NenryoCd2Seq                                                             AS '�R���R�[�h�Qseq'
                 , NENRYO2.CodeKbnNm                                                               AS '�R��2��'
                 , SYARYO.NenryoCd3Seq                                                             AS '�R���R�[�h�Rseq'
                 , NENRYO3.CodeKbnNm                                                               AS '�R��3��'
                 , SYASYU.KataKbn                                                                  AS '�^�敪'
                 , (
                          SELECT
                                 SUM(SyaSyuDai)
                          FROM
                                 TKD_YykSyu
                          WHERE
                                 UkeNo       = HAISHA.UkeNo
                                 AND SiyoKbn = 1
                   )
                   AS '�Ԏ�䐔'
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
                   , 1, 1, '') as �Ј���
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
                   , 1, 1, '') as �Ј��E����
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��common
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Pcommon
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1            --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = 0            --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Qcommon
                 , (
                             SELECT
                                       top 1 TEIHAI.TehNm AS '��z��'
                                       --BASYO.Jyus1     AS '�Z���P',                 	
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as ��z��
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       BASYO.Jyus1 AS '�Z���P'
                                       --BASYO.Jyus2     AS '�Z���Q'                	
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���P
                 , (
                             SELECT
                                       top 1
                                       --TEIHAI.TehNm    AS '��z��'                	
                                       --BASYO.Jyus1     AS '�Z���P'                 	
                                       BASYO.Jyus2 AS '�Z���Q'
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
                                       TEIHAI.UkeNo         = HAISHA.UkeNo    --3/�@�^�s�w�������擾�Ŏ擾������t�ԍ�                  	
                                       AND TEIHAI.UnkRen    = 1               --3/�@�^�s�w�������擾�Ŏ擾�����^�s���A��                  	
                                       AND TEIHAI.TeiDanNo  = HAISHA.TeiDanNo --���ʂ̎�z                  	
                                       AND TEIHAI.SiyoKbn   = 1
                                       and TEIHAI.TehaiCdSeq=1
                             order by
                                       TEIHAI.TehRen
                   )
                   as �Z���Q
        FROM
                   TKD_Haisha AS HAISHA
                   INNER JOIN
                              TKD_Yyksho AS YYKSHO
                              ON
                                         YYKSHO.UkeNo           = HAISHA.UkeNo
                                         AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
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
                                         AND SYASYU.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_Eigyos AS EIGYOSHO
                              ON
                                         EIGYOSHO.EigyoCdSeq = HAISHA.SyuEigCdSeq
                   LEFT JOIN
                              VPM_Compny AS KAISHA
                              ON
                                         KAISHA.CompanyCdSeq    = EIGYOSHO.CompanyCdSeq
                                         AND KAISHA.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO1
                              ON
                                         NENRYO1.CodeKbnSeq      = SYARYO.NenryoCd1Seq
                                         AND NENRYO1.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO2
                              ON
                                         NENRYO2.CodeKbnSeq      = SYARYO.NenryoCd2Seq
                                         AND NENRYO2.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
                   LEFT JOIN
                              VPM_CodeKb AS NENRYO3
                              ON
                                         NENRYO3.CodeKbnSeq      = SYARYO.NenryoCd3Seq
                                         AND NENRYO3.CodeSyu     = 'NENRYOCD'
                                         AND NENRYO3.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq      	
        WHERE
                   YYKSHO.UkeCd     >= @UkeCdFrom
                   AND YYKSHO.UkeCd <= @UkeCdTo
                   --AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom                  --��ʂ̗\��敪�iFROM�j       	
                   --AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo                  --��ʂ̗\��敪�iTO�j       	
                   --AND HAISHA.SyuKoYmd = @SyuKoYmd              --��ʂ̏o�ɔN����       	
                   AND HAISHA.KSKbn      <> 1            --�����ԈȊO       	
                   AND HAISHA.YouTblSeq   = 0            --�b�ԈȊO       	
                   AND YYKSHO.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq       	
                   --AND HAISHA.SyuEigCdSeq = @SyuEigCdSeq                  --��ʂ̏o�ɉc�Ə�       	
                   AND HAISHA.SiyoKbn  = 1
				   AND YoyaSyu			  = 1
                   AND HAISHA.TeiDanNo = @TeiDanNo --�p�����[�^�̒�c�ԍ�       	
                   AND HAISHA.UnkRen   = @UnkRen   --�p�����[�^�̉^�s���A��       	
                   AND HAISHA.BunkRen  = @BunkRen  --�p�����[�^�̕����A��       	
                   --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ 	
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


