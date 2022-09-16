USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_SubContractorStatus_R_Csv]    Script Date: 2021/05/27 16:50:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





ALTER    PROC [dbo].[Pro_SubContractorStatus_R_Csv]
					@startDate varchar(8), @endDate varchar(8),
					@dateType int,
					@tokuiFrom int, @tokuiTo int, @sitenFrom int, @sitenTo int, -- for customer
					@bookingTypes varchar(max),
					@companyIds varchar(max),
					@brandStart int, @brandEnd int,
					@ukeCdFrom varchar(10), @ukeCdTo varchar(10),
					@staffFrom int, @staffTo int,
					@jitaFlg int,
					@tenantId int
AS
BEGIN

SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS 'No'
	   ,JM_YoyKbn.YoyaKbn �@  AS		'YoyaKbn'	--'�\��敪'											
       ,JM_YoyKbn.YoyaKbnNm  AS		'YoyaKbnNm'	--'�\��敪��'											
       ,TKD_Yousha.UkeNo     AS		'UkeNo'	--'��t�ԍ�'											
       ,TKD_Yousha.UnkRen    AS		'UnkRen'	--'�^�s���A��'											
       ,JT_Unkobi.HaiSYmd    AS		'HaiSYmd'	--'�^�s�z�ԔN����' 											
       ,JT_Unkobi.TouYmd     AS		'TouYmd'	--'�^�s�����N����' 											
       ,JM_Gyosya.GyosyaCd   AS		'GyosyaCd'	--'�Ǝ҃R�[�h'											
       ,JM_Tokisk.TokuiCd    AS		'TokuiCd'	--'���Ӑ�R�[�h' 											
       ,JM_TokiSt.SitenCd    AS		'SitenCd'	--'���Ӑ�x�X�R�[�h'											
       ,JM_Tokisk.TokuiNm    AS		'SkTokuiNm'	--'���Ӑ於'											
       ,JM_TokiSt.SitenNm    AS		'StSitenNm'	--'���Ӑ�x�X��' 											
       ,JM_Tokisk.RyakuNm    AS		'SkRyakuNm'	--'���Ӑ旪��' 											
       ,JM_TokiSt.RyakuNm    AS		'StRyakuNm'	--'���Ӑ�x�X����' 											
       ,JT_Yyksho.TokuiTanNm AS		'TokuiTanNm'	--'���Ӑ�S���Җ�'											
       ,JT_Yyksho.TokuiTel   AS		'TokuiTel'	--'���Ӑ�d�b�ԍ�'											
       ,JM_UkeEigyos.EigyoCd AS		'EigyoCd'	--'��t�c�Ə��R�[�h'											
       ,JM_UkeEigyos.EigyoNm AS		'EigyoNm'	--'��t�c�Ə���'											
       ,JM_UkeEigyos.RyakuNm AS		'EigyosRyakuNm'	--'��t�c�Ə�����'											
       ,JT_Unkobi.DanTaNm	 AS		'DanTaNm'	--'�c�̖�'											
       ,JT_Unkobi.IkNm		 AS		'IkNm'	--'�s�於'											
       ,JT_Unkobi.HaiSNm	 AS		'U_HaiSNm'	--'�z�Ԓn��(�^�s��)'											
       ,JT_Unkobi.HaiSTime   AS		'U_HaiSTime'	--'�z�Ԏ���(�^�s��)'											
       ,JT_Unkobi.HaiSJyus1  AS		'U_HaiSJyus1'	--'�z�Ԓn�Z��1(�^�s��)'											
       ,JT_Unkobi.HaiSJyus2  AS		'U_HaiSJyus2'	--'�z�Ԓn�Z��2(�^�s��)'											
       ,JM_HaiSBunrui.CodeKbn AS	'BunruiCodeKbn'	--'�z�Ԓn��ʋ@�֕��ރR�[�h(�^�s��)'											
       ,JM_HaiSKoutu.KoukCd   AS	'HaiSKoukCd'	--'�z�Ԓn��ʋ@�փR�[�h(�^�s��)'											
       ,JM_HaiSBunrui.CodeKbnNm AS	'JM_HaiSBunrui_CodeKbnNm'	--'�z�Ԓn��ʋ@�֕��ޖ�(�^�s��)'											
       ,JM_HaiSBunrui.RyakuNm AS	'JM_HaiSBunrui_RyakuNm'	--'�z�Ԓn��ʋ@�֕��ޗ���(�^�s��)'											
       ,JM_HaiSKoutu.KoukNm   AS	'JM_HaiSKoutu_KoukNm'	--'�z�Ԓn��ʋ@�֖�(�^�s��)'											
       ,JM_HaiSKoutu.RyakuNm  AS	'JM_HaiSKoutu_RyakuNm'	--'�z�Ԓn��ʋ@�֗���(�^�s��)'											
       ,JM_HaiSBin.BinCd		AS	'JM_HaiSBin_BinCd'	--'�z�Ԓn�փR�[�h(�^�s��)'											
       ,JM_HaiSBin.BinNm		AS	'JM_HaiSBin_BinNm'	--'�z�Ԓn�֖�(�^�s��)'											
       ,JT_Unkobi.TouNm			AS	'JT_Unkobi_TouNm'	--'�����n��(�^�s��)'											
       ,JT_Unkobi.TouChTime		AS	'JT_Unkobi_TouChTime'	--'��������(�^�s��)'											
       ,JT_Unkobi.TouJyusyo1	AS	'JT_Unkobi_TouJyusyo1'	--'�����n�Z��1(�^�s��)'											
       ,JT_Unkobi.TouJyusyo2	AS	'JT_Unkobi_TouJyusyo2'	--'�����n�Z��2(�^�s��)'											
       ,JM_TouChaBunrui.CodeKbn AS	'JM_TouChaBunrui_CodeKbn'	--'�����n��ʋ@�֕��ރR�[�h(�^�s��)'											
       ,JM_TouChaKoutu.KoukCd   AS	'JM_TouChaKoutu_KoukCd'	--'�����n��ʋ@�փR�[�h(�^�s��)'											
       ,JM_TouChaBunrui.CodeKbnNm	AS 'JM_TouChaBunrui_CodeKbnNm'	--'�����n��ʋ@�֕��ޖ�(�^�s��)'											
       ,JM_TouChaBunrui.RyakuNm		AS 'JM_TouChaBunrui_RyakuNm'	--'�����n��ʋ@�֕��ޗ���(�^�s��)'											
       ,JM_TouChaKoutu.KoukNm		AS	'JM_TouChaKoutu_KoukNm'	--'�����n��ʋ@�֖�(�^�s��)' 											
       ,JM_TouChaKoutu.RyakuNm		AS	'JM_TouChaKoutu_RyakuNm'	--'�����n��ʋ@�֗���(�^�s��)' 											
       ,JM_TouChaBin.BinCd			AS	'JM_TouChaBin_BinCd'	--'�����n�փR�[�h(�^�s��)' 											
       ,JM_TouChaBin.BinNm			AS	'JM_TouChaBin_BinNm'	--'�����n�֖�(�^�s��)' 											
       ,JT_Unkobi.JyoSyaJin			AS	'JT_Unkobi_JyoSyaJin'	--'��Ԑl��(�^�s��)' 											
       ,JT_Unkobi.PlusJin			AS	'JT_Unkobi_PlusJin'	--'�v���X�l��(�^�s��)' 											
       ,JT_SumYykSyu.SumDai			AS	'JT_SumYykSyu_SumDai'	--'���䐔' 											
       ,JT_SumHaisha.SumSyaRyoUnc	AS	'JT_SumHaisha_SumSyaRyoUnc'	--'�^��'											
       ,JT_Yyksho.ZeiKbn			AS	'JT_Yyksho_ZeiKbn'	--'�ŋ敪'											
       ,JM_ZeiKbn.CodeKbnNm			AS	'JM_ZeiKbn_CodeKbnNm'	--'�ŋ敪��'											
       ,JM_ZeiKbn.RyakuNm			AS	'JM_ZeiKbn_RyakuNm'	--'�ŋ敪��'											
       ,JT_Yyksho.Zeiritsu			AS	'JT_Yyksho_Zeiritsu'	--'����ŗ�' 											
       ,JT_SumHaisha.SumSyaRyoSyo	AS	'JT_SumHaisha_SumSyaRyoSyo'	--'����Ŋz' 											
       ,JT_Yyksho.TesuRitu			AS	'JT_Yyksho_TesuRitu'	--'�萔����'											
       ,JT_SumHaisha.SumSyaRyoTes	AS	'JT_SumHaisha_SumSyaRyoTes'	--'�萔���z' 											
	   ,JT_SumHaisha.SumSyaRyoUnc + JT_SumHaisha.SumSyaRyoSyo AS 'JT_SumHaisha_Charge'	--'�^�����v'										
       ,JT_SumFutTumGui.SumUriGakKin AS 'JT_SumFutTumGui_SumUriGakKin'	--'�K�C�h��'											
       ,JT_SumFutTumGui.SumSyaRyoSyo AS 'JT_SumFutTumGui_SumSyaRyoSyo'	--'�K�C�h�������'											
       ,JT_SumFutTumGui.SumSyaRyoTes AS 'JT_SumFutTumGui_SumSyaRyoTes'	--'�K�C�h���萔��'  											
	   ,JT_SumFutTumGui.SumUriGakKin + JT_SumFutTumGui.SumSyaRyoSyo AS 'JT_SumFutTumGui_Charge'	--'�K�C�h�����v��'										
       ,JT_SumFutTum.SumUriGakKin -(JT_SumFutTumGui.SumUriGakKin)	AS 'JT_SumFutTum_SumUriGakKin'	--'�t�ѐύ��i�z' 											
       ,JT_SumFutTum.SumSyaRyoSyo -(JT_SumFutTumGui.SumSyaRyoSyo)	AS 'JT_SumFutTum_SumSyaRyoSyo'	--'�t�ѐύ��i�����'											
       ,JT_SumFutTum.SumSyaRyoTes -(JT_SumFutTumGui.SumSyaRyoTes)	AS 'JT_SumFutTum_SumSyaRyoTes'	--'�t�ѐύ��i�萔���z'
	   ,JT_SumFutTum.SumUriGakKin -(JT_SumFutTumGui.SumUriGakKin) + JT_SumFutTum.SumSyaRyoSyo -(JT_SumFutTumGui.SumSyaRyoSyo) AS 'Total_YFutum'
       ,JM_YouGyosya.GyosyaKbn		AS 'GyosyaKbn'	--'�b�Ԑ�Ǝҋ敪'											
       ,JM_You.TokuiCd				AS 'You_TokuiCd'	--'�b�Ԑ�R�[�h'											
       ,JM_YouSiten.SitenCd			AS 'YouSiten_SitenCd'	--'�b�Ԑ�x�X�R�[�h'											
       ,JM_You.TokuiNm				AS 'You_TokuiNm'	--'�b�Ԑ於'											
       ,JM_YouSiten.SitenNm			AS 'YouSiten_SitenNm'	--'�b�Ԑ�x�X��'											
       ,JM_You.RyakuNm				AS 'You_RyakuNm'	--'�b�Ԑ旪��'											
       ,JM_YouSiten.RyakuNm			AS 'YouSiten_RyakuNm'	--'�b�Ԑ�x�X����'											
       ,JT_Haisha.GoSya				AS 'H_GoSya'	--'����'											
       ,JT_Haisha.DanTaNm2			AS 'H_DanTaNm2'	--'�c�̖��Q' 											
       ,JT_Haisha.IkNm				AS 'H_IkNm'	--'�s�於(�z��)'											
       ,JT_Haisha.HaiSNm			AS 'H_HaiSNm'	--'�z�Ԓn��(�z��)'											
       ,JT_Haisha.HaiSYmd			AS 'H_HaiSYmd'	--'�z�ԔN����(�z��)'											
       ,JT_Haisha.HaiSTime			AS 'H_HaiSTime'	--'�z�Ԏ���(�z��)'											
       ,JT_Haisha.HaiSJyus1			AS 'H_HaiSJyus1'	--'�z�Ԓn�Z��1(�z��)'											
       ,JT_Haisha.HaiSJyus2			AS 'H_HaiSJyus2'	--'�z�Ԓn�Z��1(�z��)'											
       ,JM_YouHaiSBunrui.CodeKbn	AS 'JM_YouHaiSBunrui_CodeKb'	--'�z�Ԓn��ʋ@�֕��ރR�[�h(�z��)'											
       ,JM_YouHaiSKoutu.KoukCd		AS 'JM_YouHaiSKoutu_KoukCd'	--'�z�Ԓn��ʋ@�փR�[�h(�z��)'											
       ,JM_YouHaiSBunrui.CodeKbnNm	AS 'JM_YouHaiSBunrui_CodeKbnNm'	--'�z�Ԓn��ʋ@�֕��ޖ�(�z��)'											
       ,JM_YouHaiSBunrui.RyakuNm	AS 'JM_YouHaiSBunrui_RyakuNm'	--'�z�Ԓn��ʋ@�֕��ޗ���(�z��)'											
       ,JM_YouHaiSKoutu.KoukNm		AS 'JM_YouHaiSKoutu_KoukNm'	--'�z�Ԓn��ʋ@�֖�(�z��)'											
       ,JM_YouHaiSKoutu.RyakuNm		AS 'JM_YouHaiSKoutu_RyakuNm'	--'�z�Ԓn��ʋ@�֗���(�z��)'											
       ,JM_YouHaiSBin.BinCd			AS 'JM_YouHaiSBin_BinCd'	--'�z�Ԓn�փR�[�h(�z��)'											
       ,JM_YouHaiSBin.BinNm			AS 'JM_YouHaiSBin_BinNm'	--'�z�Ԓn�֖�(�z��)'											
       ,JT_Haisha.TouNm				AS 'JT_Haisha_TouNm'	--'�����n��(�z��)'											
       ,JT_Haisha.TouYmd			AS 'JT_Haisha_TouYmd'	--'�����N����(�z��)'											
       ,JT_Haisha.TouChTime			AS 'JT_Haisha_TouChTime'	--'��������(�z��)'											
       ,JT_Haisha.TouJyusyo1		AS 'JT_Haisha_TouJyusyo1'	--'�����n�Z��1(�z��)'											
       ,JT_Haisha.TouJyusyo2		AS 'JT_Haisha_TouJyusyo2'	--'�����n�Z��2(�z��)'											
       ,JM_YouTouChaBunrui.CodeKbn	AS 'JM_YouTouChaBunrui_CodeKbn'	--'�����n��ʋ@�֕��ރR�[�h(�z��)'											
       ,JM_YouTouChaKoutu.KoukCd	AS 'JM_YouTouChaKoutu_KoukCd'	--'�����n��ʋ@�փR�[�h(�z��)'											
       ,JM_YouTouChaBunrui.CodeKbnNm AS'JM_YouTouChaBunrui_CodeKbnNm'	-- '�����n��ʋ@�֕��ޖ�(�z��)'											
       ,JM_YouTouChaBunrui.RyakuNm	AS 'JM_YouTouChaBunrui_RyakuNm'	--'�����n��ʋ@�֕��ޗ���(�z��)'											
       ,JM_YouTouChaKoutu.KoukNm	AS 'JM_YouTouChaKoutu_KoukNm'	--'�����n��ʋ@�֖�(�z��)'											
       ,JM_YouTouChaKoutu.RyakuNm	AS 'JM_YouTouChaKoutu_RyakuNm'	--'�����n��ʋ@�֗���(�z��)'											
       ,JM_YouTouChaBin.BinCd		AS 'JM_YouTouChaBin_BinCd'	--'�����n�փR�[�h(�z��)'											
       ,JM_YouTouChaBin.BinNm		AS 'JM_YouTouChaBin_BinNm'	--'�����n�֖�(�z��)'											
       ,JT_Haisha.JyoSyaJin			AS 'JT_Haisha_JyoSyaJin'	--'��Ԑl��(�z��)'											
       ,JT_Haisha.PlusJin			AS 'JT_Haisha_PlusJin'	--'�v���X�l��(�z��)'											
       ,JT_Haisha.YoushaUnc			AS 'JT_Haisha_YoushaUnc'	--'�b�ԉ^��' 											
       ,TKD_Yousha.ZeiKbn			AS 'TKD_Yousha_ZeiKbn'	--'�b�Ԑŋ敪'											
       ,JM_YouZeiKbn.CodeKbnNm		AS 'JM_YouZeiKbn_CodeKbnNm'	--'�b�Ԑŋ敪��'											
       ,JM_YouZeiKbn.RyakuNm		AS 'JM_YouZeiKbn_RyakuNm'	--'�b�Ԑŋ敪��' 											
       ,TKD_Yousha.Zeiritsu			AS 'TKD_Yousha_Zeiritsu'	--'�b�ԏ���ŗ�'											
       ,JT_Haisha.YoushaSyo			AS 'JT_Haisha_YoushaSyo'	--'�b�ԏ���Ŋz' 											
       ,TKD_Yousha.TesuRitu			AS 'TKD_Yousha_TesuRitu'	--'�b�Ԏ萔����' 											
       ,JT_Haisha.YoushaTes			AS 'JT_Haisha_YoushaTes'	--'�b�Ԏ萔���z' 											
	   ,JT_Haisha.YoushaUnc + JT_Haisha.YoushaSyo AS 'JT_Haisha_YouCharge'	--'�b�ԉ^�����v'										
       ,JT_SumYMFuTuGui.SumHaseiKin		AS '	'	--'�b�ԃK�C�h��'											
       ,JT_SumYMFuTuGui.SumSyaRyoSyo	AS 'JT_SumYMFuTuGui_SumSyaRyoSyo'	--'�b�ԃK�C�h�������'											
       ,JT_SumYMFuTuGui.SumSyaRyoTes	AS 'JT_SumYMFuTuGui_SumSyaRyoTes'	--'�b�ԃK�C�h���萔��' 											
	   ,JT_SumYMFuTuGui.SumHaseiKin + JT_SumYMFuTuGui.SumSyaRyoSyo	AS 'JT_SumYMFuTuGui_Charge'	--'�b�ԃK�C�h�����v'										
       ,JT_SumYMFuTu.SumHaseiKin -(JT_SumYMFuTuGui.SumHaseiKin)		AS 'JT_SumYMFuTu_SumHaseiKin'	--'�b�ԕt�ѐύ��i�z'											
       ,JT_SumYMFuTu.SumSyaRyoSyo -(JT_SumYMFuTuGui.SumSyaRyoSyo)	AS 'JT_SumYMFuTu_SumSyaRyoSyo'	--'�b�ԕt�ѐύ��i�����'											
       ,JT_SumYMFuTu.SumSyaRyoTes -(JT_SumYMFuTuGui.SumSyaRyoTes)	AS 'JT_SumYMFuTu_SumSyaRyoTes'	--'�b�ԕt�ѐύ��i�萔���z'											
	   ,JT_SumYMFuTu.SumHaseiKin -(JT_SumYMFuTuGui.SumHaseiKin)+ JT_SumYMFuTu.SumSyaRyoSyo -(JT_SumYMFuTuGui.SumSyaRyoSyo) AS	'Total_YMFutum' --'�b�ԕt�ѐύ��i���v'										
FROM TKD_Yousha											
INNER JOIN											
  (SELECT TKD_Yousha.UkeNo ,											
          TKD_Yousha.UnkRen											
   FROM TKD_Yousha											
   LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Yousha.UkeNo = JT_Yyksho.UkeNo											
   AND JT_Yyksho.YoyaSyu = 1											
   LEFT JOIN TKD_Haisha AS JT_Haisha ON TKD_Yousha.UkeNo = JT_Haisha.UkeNo											
   AND TKD_Yousha.UnkRen = JT_Haisha.UnkRen											
   AND TKD_Yousha.YouTblSeq = JT_Haisha.YouTblSeq											
   AND JT_Haisha.SiyoKbn = 1											
   LEFT JOIN VPM_Tokisk AS JM_You ON TKD_Yousha.YouCdSeq = JM_You.TokuiSeq											
   AND TKD_Yousha.HasYmd >= JM_You.SiyoStaYmd											
   AND TKD_Yousha.HasYmd <= JM_You.SiyoEndYmd											
   LEFT JOIN VPM_TokiSt AS JM_YouSiten ON TKD_Yousha.YouCdSeq = JM_YouSiten.TokuiSeq											
   AND TKD_Yousha.YouSitCdSeq = JM_YouSiten.SitenCdSeq											
   AND TKD_Yousha.HasYmd >= JM_YouSiten.SiyoStaYmd											
   AND TKD_Yousha.HasYmd <= JM_YouSiten.SiyoEndYmd											
   LEFT JOIN VPM_Gyosya AS JM_YouGyosya ON JM_You.GyosyaCdSeq = JM_YouGyosya.GyosyaCdSeq											
   AND JM_YouGyosya.SiyoKbn = 1											
   LEFT JOIN VPM_Eigyos AS JM_UkeEigyos ON JT_Yyksho.UkeEigCdSeq = JM_UkeEigyos.EigyoCdSeq											
   AND JM_UkeEigyos.SiyoKbn = 1											
   LEFT JOIN VPM_Compny AS JM_Compny ON JM_UkeEigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq											
   AND JM_Compny.SiyoKbn = 1											
   LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JM_YoyKbn.TenantCdSeq = @tenantId
   AND JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq											
   AND JM_YoyKbn.SiyoKbn = 1											
   LEFT JOIN VPM_Syain AS JM_Syain ON JM_Syain.SyainCdSeq = JT_Yyksho.EigTanCdSeq											
   WHERE TKD_Yousha.SiyoKbn = 1											
     --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) >= '002'											
     --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) <= '002'	
	 AND ISNULL(JT_Yyksho.TenantCdSeq, 0) = @tenantId
     AND( (@dateType = 1 AND JT_Haisha.HaiSYmd >= @startDate AND JT_Haisha.HaiSYmd <= @endDate)		--��� �z�ԔN����:  ��ʂŔN�������ڂł�From�̔ԍ�/ ��ʂŔN�������ڂł�To�̔ԍ�
	 OR (@dateType = 2 AND JT_Haisha.TouYmd >= @startDate AND JT_Haisha.TouYmd <= @endDate))			--��� �����N����:  ��ʂŔN�������ڂł�From�̔ԍ�/ ��ʂŔN�������ڂł�To�̔ԍ�
      AND ((@tokuiFrom = 0 and @tokuiTo = 0)
			or
			(@tokuiFrom <> @tokuiTo and TKD_Yousha.YouCdSeq = @tokuiFrom and TKD_Yousha.YouSitCdSeq >= @sitenFrom)
			or
			(@tokuiFrom <> @tokuiTo and TKD_Yousha.YouCdSeq = @tokuiTo and TKD_Yousha.YouSitCdSeq <= @sitenTo)
			or
			(@tokuiFrom = @tokuiTo and TKD_Yousha.YouCdSeq = @tokuiFrom and TKD_Yousha.YouSitCdSeq >= @sitenFrom and TKD_Yousha.YouSitCdSeq <= @sitenTo)
			or
			(@tokuiFrom = 0 and @tokuiTo <> 0 and ((TKD_Yousha.YouCdSeq = @tokuiTo and TKD_Yousha.YouSitCdSeq <= @sitenTo) or TKD_Yousha.YouCdSeq < @tokuiTo))
			or
			(@tokuiTo = 0 and @tokuiFrom <> 0 and ((TKD_Yousha.YouCdSeq = @tokuiFrom and TKD_Yousha.YouSitCdSeq >= @sitenFrom) or TKD_Yousha.YouCdSeq > @tokuiFrom))
			or
			(TKD_Yousha.YouCdSeq < @tokuiTo and TKD_Yousha.YouCdSeq > @tokuiFrom))
     AND ISNULL(JT_Yyksho.UkeCd, 0) >= @ukeCdFrom											
     AND ISNULL(JT_Yyksho.UkeCd, 0) <= @ukeCdTo	
     AND JM_YoyKbn.YoyaKbnSeq IN (SELECT * FROM FN_SplitString(@bookingTypes, '-'))	--��ʂŗ\��敪���ڂł�From�̔ԍ�										 
	 AND JM_Compny.CompanyCdSeq IN (SELECT * FROM FN_SplitString(@companyIds, '-'))	--��ʂŃ��O�C����Ђ̃R�[�hSEQ
     AND JM_UkeEigyos.EigyoCd >= @brandStart			--��ʂŎ�t�c�Ə��R�[�h���ڂł�From�̔ԍ�											
	 AND JM_UkeEigyos.EigyoCd <= @brandEnd			--��ʂŎ�t�c�Ə��R�[�h���ڂł�To�̔ԍ�
     AND TKD_Yousha.JitaFlg = CASE WHEN @jitaFlg = 0 THEN TKD_Yousha.JitaFlg ELSE 0 END			--��ʂŎ����Ћ敪���ڂł̖��o�͂�I������		
	 AND JM_Syain.SyainCdSeq >= @staffFrom		--��ʂŉc�ƒS���҃R�[�hSEQ���ڂł�From�̔ԍ�																										
	 AND JM_Syain.SyainCdSeq <= @staffTo			--��ʂŉc�ƒS���҃R�[�hSEQ���ڂł�To�̔ԍ�	
   GROUP BY TKD_Yousha.UkeNo ,											
            TKD_Yousha.UnkRen) AS WHERETABLE ON TKD_Yousha.UkeNo = WHERETABLE.UkeNo											
AND TKD_Yousha.UnkRen = WHERETABLE.UnkRen											
LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Yousha.UkeNo = JT_Yyksho.UkeNo											
LEFT JOIN TKD_Unkobi AS JT_Unkobi ON TKD_Yousha.UkeNo = JT_Unkobi.UkeNo											
AND TKD_Yousha.UnkRen = JT_Unkobi.UnkRen											
INNER JOIN TKD_Haisha AS JT_Haisha ON TKD_Yousha.UkeNo = JT_Haisha.UkeNo											
AND TKD_Yousha.UnkRen = JT_Haisha.UnkRen											
AND TKD_Yousha.YouTblSeq = JT_Haisha.YouTblSeq											
AND JT_Haisha.SiyoKbn = 1											
LEFT JOIN VPM_Tokisk AS JM_Tokisk ON JT_Yyksho.TokuiSeq = JM_Tokisk.TokuiSeq											
AND JT_Yyksho.SeiTaiYmd >= JM_Tokisk.SiyoStaYmd											
AND JT_Yyksho.SeiTaiYmd <= JM_Tokisk.SiyoEndYmd											
LEFT JOIN VPM_TokiSt AS JM_TokiSt ON JT_Yyksho.TokuiSeq = JM_TokiSt.TokuiSeq											
AND JT_Yyksho.SitenCdSeq = JM_TokiSt.SitenCdSeq											
AND JT_Yyksho.SeiTaiYmd >= JM_TokiSt.SiyoStaYmd											
AND JT_Yyksho.SeiTaiYmd <= JM_TokiSt.SiyoEndYmd											
LEFT JOIN VPM_Gyosya AS JM_Gyosya ON JM_Tokisk.GyosyaCdSeq = JM_Gyosya.GyosyaCdSeq											
AND JM_Gyosya.SiyoKbn = 1											
LEFT JOIN VPM_Koutu AS JM_HaiSKoutu ON JT_Unkobi.HaiSKouKCdSeq = JM_HaiSKoutu.KoukCdSeq											
AND JM_HaiSKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_HaiSBunrui ON JM_HaiSKoutu.BunruiCdSeq = JM_HaiSBunrui.CodeKbnSeq											
AND JM_HaiSBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_HaiSBin ON JT_Unkobi.HaiSBinCdSeq = JM_HaiSBin.BinCdSeq											
AND JT_Unkobi.HaiSYmd BETWEEN JM_HaiSBin.SiyoStaYmd AND JM_HaiSBin.SiyoEndYmd											
LEFT JOIN VPM_Koutu AS JM_TouChaKoutu ON JT_Unkobi.TouKouKCdSeq = JM_TouChaKoutu.KoukCdSeq											
AND JM_TouChaKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_TouChaBunrui ON JM_TouChaKoutu.BunruiCdSeq = JM_TouChaBunrui.CodeKbnSeq											
AND JM_TouChaBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_TouChaBin ON JT_Unkobi.TouBinCdSeq = JM_TouChaBin.BinCdSeq											
AND JT_Unkobi.HaiSYmd BETWEEN JM_TouChaBin.SiyoStaYmd AND JM_TouChaBin.SiyoEndYmd											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(SyaSyuDai) AS SumDai											
   FROM TKD_YykSyu											
   WHERE SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumYykSyu ON TKD_Yousha.UkeNo = JT_SumYykSyu.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumYykSyu.UnkRen											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(SyaRyoUnc) AS SumSyaRyoUnc ,											
          SUM(SyaRyoSyo) AS SumSyaRyoSyo ,											
          SUM(SyaRyoTes) AS SumSyaRyoTes											
   FROM TKD_Haisha											
   WHERE SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumHaisha ON TKD_Yousha.UkeNo = JT_SumHaisha.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumHaisha.UnkRen											
LEFT JOIN											
  (SELECT CodeKbn ,											
          CodeKbnNm ,											
          RyakuNm											
   FROM VPM_CodeKb											
   WHERE CodeSyu = 'ZEIKBN'											
     AND SiyoKbn = 1 ) AS JM_ZeiKbn ON JT_Yyksho.ZeiKbn = CONVERT(TINYINT,JM_ZeiKbn.CodeKbn)											
LEFT JOIN											
  (SELECT CodeKbn ,											
          CodeKbnNm ,											
          RyakuNm											
   FROM VPM_CodeKb											
   WHERE CodeSyu = 'ZEIKBN'											
     AND SiyoKbn = 1 ) AS JM_YouZeiKbn ON TKD_Yousha.ZeiKbn = CONVERT(TINYINT,JM_YouZeiKbn.CodeKbn)											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(UriGakKin) AS SumUriGakKin ,											
          SUM(SyaRyoSyo) AS SumSyaRyoSyo ,											
          SUM(SyaRyoTes) AS SumSyaRyoTes											
   FROM TKD_FutTum											
   WHERE TKD_FutTum.SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumFutTum ON TKD_Yousha.UkeNo = JT_SumFutTum.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumFutTum.UnkRen											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          SUM(UriGakKin) AS SumUriGakKin ,											
          SUM(SyaRyoSyo) AS SumSyaRyoSyo ,											
          SUM(SyaRyoTes) AS SumSyaRyoTes											
   FROM TKD_FutTum											
   LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_FutTum.FutTumCdSeq											
   AND VPM_Futai.SiyoKbn=1											
   WHERE TKD_FutTum.SiyoKbn = 1											
     AND VPM_Futai.FutGuiKbn=5											
   GROUP BY UkeNo ,											
            UnkRen) AS JT_SumFutTumGui ON TKD_Yousha.UkeNo = JT_SumFutTumGui.UkeNo											
AND TKD_Yousha.UnkRen = JT_SumFutTumGui.UnkRen											
LEFT JOIN											
  (SELECT CodeKbn ,											
          CodeKbnNm ,											
          RyakuNm											
   FROM VPM_CodeKb											
   WHERE CodeSyu = 'TESKBN'											
     AND SiyoKbn = 1 ) AS JM_TesKbnFut ON JM_TokiSt.TesKbnFut = CONVERT(TINYINT,JM_TesKbnFut.CodeKbn)											
LEFT JOIN VPM_Tokisk AS JM_You ON TKD_Yousha.YouCdSeq = JM_You.TokuiSeq											
AND TKD_Yousha.HasYmd >= JM_You.SiyoStaYmd											
AND TKD_Yousha.HasYmd <= JM_You.SiyoEndYmd											
LEFT JOIN VPM_TokiSt AS JM_YouSiten ON TKD_Yousha.YouCdSeq = JM_YouSiten.TokuiSeq											
AND TKD_Yousha.YouSitCdSeq = JM_YouSiten.SitenCdSeq											
AND TKD_Yousha.HasYmd >= JM_YouSiten.SiyoStaYmd											
AND TKD_Yousha.HasYmd <= JM_YouSiten.SiyoEndYmd											
LEFT JOIN VPM_Gyosya AS JM_YouGyosya ON JM_You.GyosyaCdSeq = JM_YouGyosya.GyosyaCdSeq											
AND JM_YouGyosya.SiyoKbn = 1											
LEFT JOIN VPM_Koutu AS JM_YouHaiSKoutu ON JT_Haisha.HaiSKouKCdSeq = JM_YouHaiSKoutu.KoukCdSeq											
AND JM_YouHaiSKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_YouHaiSBunrui ON JM_YouHaiSKoutu.BunruiCdSeq = JM_YouHaiSBunrui.CodeKbnSeq											
AND JM_YouHaiSBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_YouHaiSBin ON JT_Haisha.HaiSBinCdSeq = JM_YouHaiSBin.BinCdSeq											
AND JT_Haisha.HaiSYmd BETWEEN JM_YouHaiSBin.SiyoStaYmd AND JM_YouHaiSBin.SiyoEndYmd											
LEFT JOIN VPM_Koutu AS JM_YouTouChaKoutu ON JT_Haisha.TouKouKCdSeq = JM_YouTouChaKoutu.KoukCdSeq											
AND JM_YouTouChaKoutu.SiyoKbn = 1											
LEFT JOIN VPM_CodeKb AS JM_YouTouChaBunrui ON JM_YouTouChaKoutu.BunruiCdSeq = JM_YouTouChaBunrui.CodeKbnSeq											
AND JM_YouTouChaBunrui.SiyoKbn = 1											
LEFT JOIN VPM_Bin AS JM_YouTouChaBin ON JT_Haisha.TouBinCdSeq = JM_YouTouChaBin.BinCdSeq											
AND JT_Haisha.HaiSYmd BETWEEN JM_YouTouChaBin.SiyoStaYmd AND JM_YouTouChaBin.SiyoEndYmd											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          YouTblSeq ,											
          SUM(HaseiKin) AS SumHaseiKin ,											
          SUM(SyaRyoSyo) AS SumSyaryoSyo ,											
          SUM(SyaRyoTes) AS SumSyaryoTes											
   FROM TKD_YFutTu											
   WHERE TKD_YFutTu.SiyoKbn = 1											
   GROUP BY UkeNo ,											
            UnkRen ,											
            YouTblSeq) AS JT_SumYMFuTu ON JT_Haisha.UkeNo=JT_SumYMFuTu.UkeNo											
AND JT_Haisha.UnkRen=JT_SumYMFuTu.UnkRen											
AND JT_Haisha.YouTblSeq=JT_SumYMFuTu.YouTblSeq											
LEFT JOIN											
  (SELECT UkeNo ,											
          UnkRen ,											
          YouTblSeq ,											
          SUM(HaseiKin) AS SumHaseiKin ,											
          SUM(SyaRyoSyo) AS SumSyaryoSyo ,											
          SUM(SyaRyoTes) AS SumSyaryoTes											
   FROM TKD_YFutTu											
   LEFT JOIN VPM_Futai ON VPM_Futai.FutaiCdSeq=TKD_YFutTu.FutTumCdSeq											
   AND VPM_Futai.SiyoKbn=1											
   WHERE TKD_YFutTu.SiyoKbn = 1											
     AND VPM_Futai.FutGuiKbn=5											
   GROUP BY UkeNo ,											
            UnkRen ,											
            YouTblSeq) AS JT_SumYMFuTuGui ON JT_Haisha.UkeNo=JT_SumYMFuTuGui.UkeNo											
AND JT_Haisha.UnkRen=JT_SumYMFuTuGui.UnkRen											
AND JT_Haisha.YouTblSeq=JT_SumYMFuTuGui.YouTblSeq											
LEFT JOIN VPM_Eigyos AS JM_UkeEigyos ON JT_Yyksho.UkeEigCdSeq = JM_UkeEigyos.EigyoCdSeq											
AND JM_UkeEigyos.SiyoKbn = 1											
LEFT JOIN VPM_Compny AS JM_Compny ON JM_UkeEigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq											
AND JM_Compny.SiyoKbn = 1											
LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JM_YoyKbn.TenantCdSeq = @tenantId
AND JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq											
AND JM_YoyKbn.SiyoKbn = 1											
--WHERE TKD_Yousha.SiyoKbn = 1											
  --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) >= '002'											
  --AND dbo.FP_RpZero(3, ISNULL(JM_YouGyosya.GyosyaCd, 0)) <= '002'											

ORDER BY JT_Unkobi.HaiSYmd ,											
         TKD_Yousha.UkeNo ,											
         TKD_Yousha.UnkRen ,											
         JM_You.TokuiCd ,											
         TKD_Yousha.YouTblSeq											
END										
											
											
GO


