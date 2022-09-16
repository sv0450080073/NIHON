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
                      , YYKSHO.UkeCd                                                                          AS '��t�ԍ�'
                      , YYKSHO.TokuiTel                                                                       AS '���Ӑ�d�b�ԍ�'
                      , YYKSHO.TokuiTanNm                                                                     AS '���Ӑ�S���Җ�'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS '�^�s��_�z�ԔN����'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS '�^�s��_�����N����'
                      , UNKOBI.DanTaNm                                                                        AS '�^�s��_�c�̖�'
                      , UNKOBI.KanjJyus1                                                                      AS '�^�s��_�����Z���P'
                      , UNKOBI.KanjJyus2                                                                      AS '�^�s��_�����Z���Q'
                      , UNKOBI.KanjTel                                                                        AS '�^�s��_�����d�b�ԍ�'
                      , UNKOBI.KanJNm                                                                         AS '�^�s��_��������'
                      , HAISHA.UkeNo                                                                          AS '�z��_��t�ԍ�Seq'
                      , HAISHA.UnkRen                                                                         AS '�z��_�^�s���A��'
                      , HAISHA.SyaSyuRen                                                                      AS '�z��_�Ԏ�A��'
                      , HAISHA.TeiDanNo                                                                       AS '�z��_��c�ԍ�'
                      , HAISHA.BunkRen                                                                        AS '�z��_�����A��'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS '�z��_�z�ԔN����'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '�z��_�z�Ԏ���'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS '�z��_�����N����'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '�z��_��������'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '�z��_�A�Ɏ���'
                      , HAISHA.DanTaNm2                                                                       AS '�z��_�c�̖�2'
                      , HAISHA.OthJinKbn1                                                                     AS '�z��_���̑��l���敪�R�[�h�P'
                      , OTHJIN1.CodeKbnNm                                                                     AS '���̑��l��1'
                      , HAISHA.OthJin1                                                                        AS '�z��_���̑��l���P��'
                      , HAISHA.OthJinKbn2                                                                     AS '�z��_���̑��l���敪�R�[�h�Q'
                      , OTHJIN2.CodeKbnNm                                                                     AS '���̑��l��2'
                      , HAISHA.OthJin2                                                                        AS '�z��_���̑��l���Q��'
                      , HAISHA.HaiSNm                                                                         AS '�z��_�z�Ԓn��'
                      , HAISHA.HaiSJyus1                                                                      AS '�z��_�z�Ԓn�Z���P'
                      , HAISHA.HaiSJyus2                                                                      AS '�z��_�z�Ԓn�Z���Q'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '�z��_�z�Ԓn�ڑ�����'
                      , HAISHA.IkNm                                                                           AS '�z��_�s���於'
                      , HAISHA.SyuKoYmd                                                                       AS '�z��_�o�ɔN����'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '�z��_�o�Ɏ���'
                      , HAISHA.SyuEigCdSeq                                                                    AS '�z��_�o�ɉc�Ə�Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '�o�ɉc�Ə��R�[�h'
                      , EIGYOSHO.EigyoNm                                                                      AS '�o�ɉc�Ə���'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '�z��_�o������'
                      , HAISHA.JyoSyaJin                                                                      AS '�z��_��Ԑl��'
                      , HAISHA.PlusJin                                                                        AS '�z��_�v���X�l��'
                      , HAISHA.GoSya                                                                          AS '�z��_����'
                      , HAISHA.HaiSKouKNm                                                                     AS '�z��_�z�Ԓn��ʋ@�֖�'
                      , HAISHA.HaiSBinNm                                                                      AS '�z��_�z�Ԓn�֖�'
                      , HAISHA.TouSKouKNm                                                                     AS '�z��_�����n��ʋ@�֖�'
                      , HAISHA.TouSBinNm                                                                      AS '�z��_�����n�֖�'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '�z��_�����n�ڑ�����'
                      , HAISHA.TouNm                                                                          AS '�z��_�����n��'
                      , HAISHA.TouJyusyo1                                                                     AS '�z��_�����n�Z���P'
                      , HAISHA.TouJyusyo2                                                                     AS '�z��_�����n�Z���Q'
                      , YYKSYU.SyaSyuDai                                                                      AS '�\��Ԏ�_�Ԏ�䐔'
                      , SYASYU.SyaSyuNm                                                                       AS '�Ԏ햼'
                      , SYARYO.SyaRyoCd                                                                       AS '�ԗ��R�[�h'
                      , SYARYO.SyaRyoNm                                                                       AS '�ԗ���'
                      , HENSYA.TenkoNo                                                                        AS '�ԗ��_��'
                      , SYARYO.KariSyaRyoNm                                                                   AS '���ԗ���'
                      , TOKISK.TokuiNm                                                                        AS '���Ӑ於'
                      , TOKIST.SitenNm                                                                        AS '���Ӑ�x�X��'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '�\��Ԏ�_�Ԏ�䐔sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
                      , incidental.UnkRen                                                                 AS '�^�s���A��'
                      , incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
                      , incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
                      , incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
                      , incidental.SeisanNm                                                               AS '���Z��'
                      , incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
                      , incidental.Suryo                                                                  AS '����'
                      , incidental.TeiDanNo                                                               AS '��c�ԍ�'
                      , incidental.BunkRen                                                                AS '�����A��'
                      , HAISHA.BikoNm                                                                     as '���l'
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
                                                        And OTHJIN1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
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
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --��ʂ̎�t�ԍ��iFROM�j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --��ʂ̎�t�ԍ��iTO�j            	
                        AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --��ʂ̗\��敪�iFROM�j            	
                        AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --��ʂ̗\��敪�iTO�j            	
                        AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
                        AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq
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
                      , YYKSHO.UkeCd                                                                          AS '��t�ԍ�'
                      , YYKSHO.TokuiTel                                                                       AS '���Ӑ�d�b�ԍ�'
                      , YYKSHO.TokuiTanNm                                                                     AS '���Ӑ�S���Җ�'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS '�^�s��_�z�ԔN����'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS '�^�s��_�����N����'
                      , UNKOBI.DanTaNm                                                                        AS '�^�s��_�c�̖�'
                      , UNKOBI.KanjJyus1                                                                      AS '�^�s��_�����Z���P'
                      , UNKOBI.KanjJyus2                                                                      AS '�^�s��_�����Z���Q'
                      , UNKOBI.KanjTel                                                                        AS '�^�s��_�����d�b�ԍ�'
                      , UNKOBI.KanJNm                                                                         AS '�^�s��_��������'
                      , HAISHA.UkeNo                                                                          AS '�z��_��t�ԍ�Seq'
                      , HAISHA.UnkRen                                                                         AS '�z��_�^�s���A��'
                      , HAISHA.SyaSyuRen                                                                      AS '�z��_�Ԏ�A��'
                      , HAISHA.TeiDanNo                                                                       AS '�z��_��c�ԍ�'
                      , HAISHA.BunkRen                                                                        AS '�z��_�����A��'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS '�z��_�z�ԔN����'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '�z��_�z�Ԏ���'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS '�z��_�����N����'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '�z��_��������'
                      , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '�z��_�A�Ɏ���'
                      , HAISHA.DanTaNm2                                                                       AS '�z��_�c�̖�2'
                      , HAISHA.OthJinKbn1                                                                     AS '�z��_���̑��l���敪�R�[�h�P'
                      , OTHJIN1.CodeKbnNm                                                                     AS '���̑��l��1'
                      , HAISHA.OthJin1                                                                        AS '�z��_���̑��l���P��'
                      , HAISHA.OthJinKbn2                                                                     AS '�z��_���̑��l���敪�R�[�h�Q'
                      , OTHJIN2.CodeKbnNm                                                                     AS '���̑��l��2'
                      , HAISHA.OthJin2                                                                        AS '�z��_���̑��l���Q��'
                      , HAISHA.HaiSNm                                                                         AS '�z��_�z�Ԓn��'
                      , HAISHA.HaiSJyus1                                                                      AS '�z��_�z�Ԓn�Z���P'
                      , HAISHA.HaiSJyus2                                                                      AS '�z��_�z�Ԓn�Z���Q'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '�z��_�z�Ԓn�ڑ�����'
                      , HAISHA.IkNm                                                                           AS '�z��_�s���於'
                      , HAISHA.SyuKoYmd                                                                       AS '�z��_�o�ɔN����'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '�z��_�o�Ɏ���'
                      , HAISHA.SyuEigCdSeq                                                                    AS '�z��_�o�ɉc�Ə�Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '�o�ɉc�Ə��R�[�h'
                      , EIGYOSHO.EigyoNm                                                                      AS '�o�ɉc�Ə���'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '�z��_�o������'
                      , HAISHA.JyoSyaJin                                                                      AS '�z��_��Ԑl��'
                      , HAISHA.PlusJin                                                                        AS '�z��_�v���X�l��'
                      , HAISHA.GoSya                                                                          AS '�z��_����'
                      , HAISHA.HaiSKouKNm                                                                     AS '�z��_�z�Ԓn��ʋ@�֖�'
                      , HAISHA.HaiSBinNm                                                                      AS '�z��_�z�Ԓn�֖�'
                      , HAISHA.TouSKouKNm                                                                     AS '�z��_�����n��ʋ@�֖�'
                      , HAISHA.TouSBinNm                                                                      AS '�z��_�����n�֖�'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '�z��_�����n�ڑ�����'
                      , HAISHA.TouNm                                                                          AS '�z��_�����n��'
                      , HAISHA.TouJyusyo1                                                                     AS '�z��_�����n�Z���P'
                      , HAISHA.TouJyusyo2                                                                     AS '�z��_�����n�Z���Q'
                      , YYKSYU.SyaSyuDai                                                                      AS '�\��Ԏ�_�Ԏ�䐔'
                      , SYASYU.SyaSyuNm                                                                       AS '�Ԏ햼'
                      , SYARYO.SyaRyoCd                                                                       AS '�ԗ��R�[�h'
                      , SYARYO.SyaRyoNm                                                                       AS '�ԗ���'
                      , HENSYA.TenkoNo                                                                        AS '�ԗ��_��'
                      , SYARYO.KariSyaRyoNm                                                                   AS '���ԗ���'
                      , TOKISK.TokuiNm                                                                        AS '���Ӑ於'
                      , TOKIST.SitenNm                                                                        AS '���Ӑ�x�X��'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '�\��Ԏ�_�Ԏ�䐔sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
                      , incidental.UnkRen                                                                 AS '�^�s���A��'
                      , incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
                      , incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
                      , incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
                      , incidental.SeisanNm                                                               AS '���Z��'
                      , incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
                      , incidental.Suryo                                                                  AS '����'
                      , incidental.TeiDanNo                                                               AS '��c�ԍ�'
                      , incidental.BunkRen                                                                AS '�����A��'
                      , HAISHA.BikoNm                                                                     as '���l'
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
                                                        AND OTHJIN1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
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
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
        WHERE
                        YYKSHO.UkeCd          >= @UkeCdFrom      --��ʂ̎�t�ԍ��iFROM�j            	
                        AND YYKSHO.UkeCd      <= @UkeCdTo        --��ʂ̎�t�ԍ��iTO�j            	
                        AND YYKSHO.YoyaKbnSeq >= @YoyaKbnSeqFrom --��ʂ̗\��敪�iFROM�j            	
                        AND YYKSHO.YoyaKbnSeq <= @YoyaKbnSeqTo   --��ʂ̗\��敪�iTO�j            	
                        AND HAISHA.KSKbn      <> 1               --�����ԈȊO            	
                        AND HAISHA.YouTblSeq   = 0               --�b�ԈȊO            	
                        AND HAISHA.SyuKoYmd    = @SyuKoYmd       --��ʂ̏o�ɔN����            	
                        AND YYKSHO.TenantCdSeq = @TenantCdSeq    --���O�C�����[�U�[�̃e�i���gSeq
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
                      , YYKSHO.UkeCd                                                                          AS '��t�ԍ�'
                      , YYKSHO.TokuiTel                                                                       AS '���Ӑ�d�b�ԍ�'
                      , YYKSHO.TokuiTanNm                                                                     AS '���Ӑ�S���Җ�'
                      , FORMAT ( convert(datetime, UNKOBI.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS '�^�s��_�z�ԔN����'
                      , FORMAT ( convert(datetime, UNKOBI.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS '�^�s��_�����N����'
                      , UNKOBI.DanTaNm                                                                        AS '�^�s��_�c�̖�'
                      , UNKOBI.KanjJyus1                                                                      AS '�^�s��_�����Z���P'
                      , UNKOBI.KanjJyus2                                                                      AS '�^�s��_�����Z���Q'
                      , UNKOBI.KanjTel                                                                        AS '�^�s��_�����d�b�ԍ�'
                      , UNKOBI.KanJNm                                                                         AS '�^�s��_��������'
                      , HAISHA.UkeNo                                                                          AS '�z��_��t�ԍ�Seq'
                      , HAISHA.UnkRen                                                                         AS '�z��_�^�s���A��'
                      , HAISHA.SyaSyuRen                                                                      AS '�z��_�Ԏ�A��'
                      , HAISHA.TeiDanNo                                                                       AS '�z��_��c�ԍ�'
                      , HAISHA.BunkRen                                                                        AS '�z��_�����A��'
                      , FORMAT ( convert(datetime, HAISHA.HaiSYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )        AS '�z��_�z�ԔN����'
                      , CONCAT(SUBSTRING(HAISHA.HaiSTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSTime, 3, 2))       AS '�z��_�z�Ԏ���'
                      , FORMAT ( convert(datetime, HAISHA.TouYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' )         AS '�z��_�����N����'
                      , CONCAT(SUBSTRING(HAISHA.TouChTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouChTime, 3, 2))     AS '�z��_��������'
					   , HAISHA.KikYmd                                                                         AS 'KikYmd'
                      , CONCAT(SUBSTRING(HAISHA.KikTime, 1, 2) ,':' ,SUBSTRING(HAISHA.KikTime, 3, 2))         AS '�z��_�A�Ɏ���'
                      , HAISHA.DanTaNm2                                                                       AS '�z��_�c�̖�2'
                      , HAISHA.OthJinKbn1                                                                     AS '�z��_���̑��l���敪�R�[�h�P'
                      , OTHJIN1.CodeKbnNm                                                                     AS '���̑��l��1'
                      , HAISHA.OthJin1                                                                        AS '�z��_���̑��l���P��'
                      , HAISHA.OthJinKbn2                                                                     AS '�z��_���̑��l���敪�R�[�h�Q'
                      , OTHJIN2.CodeKbnNm                                                                     AS '���̑��l��2'
                      , HAISHA.OthJin2                                                                        AS '�z��_���̑��l���Q��'
                      , HAISHA.HaiSNm                                                                         AS '�z��_�z�Ԓn��'
                      , HAISHA.HaiSJyus1                                                                      AS '�z��_�z�Ԓn�Z���P'
                      , HAISHA.HaiSJyus2                                                                      AS '�z��_�z�Ԓn�Z���Q'
                      , CONCAT(SUBSTRING(HAISHA.HaiSSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.HaiSSetTime, 3, 2)) AS '�z��_�z�Ԓn�ڑ�����'
                      , HAISHA.IkNm                                                                           AS '�z��_�s���於'
                      , HAISHA.SyuKoYmd                                                                       AS '�z��_�o�ɔN����'
                      , CONCAT(SUBSTRING(HAISHA.SyuKoTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuKoTime, 3, 2))     AS '�z��_�o�Ɏ���'
                      , HAISHA.SyuEigCdSeq                                                                    AS '�z��_�o�ɉc�Ə�Seq'
                      , EIGYOSHO.EigyoCd                                                                      AS '�o�ɉc�Ə��R�[�h'
                      , EIGYOSHO.EigyoNm                                                                      AS '�o�ɉc�Ə���'
                      , CONCAT(SUBSTRING(HAISHA.SyuPaTime, 1, 2) ,':' ,SUBSTRING(HAISHA.SyuPaTime, 3, 2))     AS '�z��_�o������'
                      , HAISHA.JyoSyaJin                                                                      AS '�z��_��Ԑl��'
                      , HAISHA.PlusJin                                                                        AS '�z��_�v���X�l��'
                      , HAISHA.GoSya                                                                          AS '�z��_����'
                      , HAISHA.HaiSKouKNm                                                                     AS '�z��_�z�Ԓn��ʋ@�֖�'
                      , HAISHA.HaiSBinNm                                                                      AS '�z��_�z�Ԓn�֖�'
                      , HAISHA.TouSKouKNm                                                                     AS '�z��_�����n��ʋ@�֖�'
                      , HAISHA.TouSBinNm                                                                      AS '�z��_�����n�֖�'
                      , CONCAT(SUBSTRING(HAISHA.TouSetTime, 1, 2) ,':' ,SUBSTRING(HAISHA.TouSetTime, 3, 2))   AS '�z��_�����n�ڑ�����'
                      , HAISHA.TouNm                                                                          AS '�z��_�����n��'
                      , HAISHA.TouJyusyo1                                                                     AS '�z��_�����n�Z���P'
                      , HAISHA.TouJyusyo2                                                                     AS '�z��_�����n�Z���Q'
                      , YYKSYU.SyaSyuDai                                                                      AS '�\��Ԏ�_�Ԏ�䐔'
                      , SYASYU.SyaSyuNm                                                                       AS '�Ԏ햼'
                      , SYARYO.SyaRyoCd                                                                       AS '�ԗ��R�[�h'
                      , SYARYO.SyaRyoNm                                                                       AS '�ԗ���'
                      , HENSYA.TenkoNo                                                                        AS '�ԗ��_��'
                      , SYARYO.KariSyaRyoNm                                                                   AS '���ԗ���'
                      , TOKISK.TokuiNm                                                                        AS '���Ӑ於'
                      , TOKIST.SitenNm                                                                        AS '���Ӑ�x�X��'
                      , (
                               SELECT
                                      SUM(SyaSyuDai)
                               FROM
                                      TKD_YykSyu
                               WHERE
                                      UkeNo      = HAISHA.UkeNo
                                      and SiyoKbn=1
                        )
                                                                                                          AS '�\��Ԏ�_�Ԏ�䐔sum'
                      , FORMAT ( convert(datetime, incidental.HasYmd, 112), 'yyyy�NMM��dd���iddd�j', 'ja-JP' ) AS '�����N����'
                      , incidental.UnkRen                                                                 AS '�^�s���A��'
                      , incidental.FutTumKbn                                                              AS '�t�ѐύ��i�敪'
                      , incidental.FutTumRen                                                              AS '�t�ѐύ��i�A��'
                      , incidental.FutTumNm                                                               AS '�t�ѐύ��i��'
                      , incidental.SeisanNm                                                               AS '���Z��'
                      , incidental.FutTumCdSeq                                                            AS '�t�ѐύ��i�R�[�h�r�d�p'
                      , incidental.Suryo                                                                  AS '����'
                      , incidental.TeiDanNo                                                               AS '��c�ԍ�'
                      , incidental.BunkRen                                                                AS '�����A��'
                      , HAISHA.BikoNm                                                                     as '���l'
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
                        , 1, 1, '') as �Ј��E����
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
                                                        AND OTHJIN1.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
                                                        AND OTHJIN1.CodeSyu     = 'OTHJINKBN'
                        LEFT JOIN
                                        VPM_CodeKb AS OTHJIN2
                                        ON
                                                        OTHJIN2.CodeKbn         = CONVERT(varchar(10), HAISHA.OthJinKbn2)
                                                        AND OTHJIN2.TenantCdSeq = @TenantCdSeq --���O�C�����[�U�[�̃e�i���gSeq
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
                                                            ) --3/�@�^�s�w���E�斱�L�^���擾�Ŏ擾������t�ԍ�     	
                                                            AND MFUTTU.SiyoKbn = 1
                                                            AND FUTTUM.SiyoKbn = 1
                                        )
                                        incidental
                                        ON
                                                        incidental.UkeNo = HAISHA.UkeNo
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
                        AND HAISHA.TeiDanNo = @TeiDanNo --�p�����[�^�̒�c�ԍ�            	
                        AND HAISHA.UnkRen   = @UnkRen   --�p�����[�^�̉^�s���A��            	
                        AND HAISHA.BunkRen  = @BunkRen  --�p�����[�^�̕����A��            	
                        --��ʂŏo�͏��Ɂu�o�ɁE�ԗ��R�[�h���v���w�肵���ꍇ	
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


