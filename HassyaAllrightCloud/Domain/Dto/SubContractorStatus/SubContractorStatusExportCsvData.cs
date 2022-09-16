using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.SubContractorStatus
{
    public class SubContractorStatusExportCsvData
    {
        private string GetOutput(string output)
        {
            return !IsRepeatItem || IsInHandleRepeatMode ?  output : string.Empty;
        }

        public bool IsInHandleRepeatMode { get; set; } = true;
        public bool IsRepeatItem { get; set; } = true;
        public int No { get; set; }
        private string _yoyaKbn;
        public string YoyaKbn 
        { 
            get =>  GetOutput(_yoyaKbn); 
            set => _yoyaKbn = value; 
        }
        private string _yoyaKbnNm;
        public string YoyaKbnNm
        {
            get => GetOutput(_yoyaKbnNm);
            set => _yoyaKbnNm = value;
        }
        private string _ukeNo;
        public string UkeNo
        {
            get => GetOutput(_ukeNo);
            set => _ukeNo = value;
        }
        private string _unkRen;
        public string UnkRen
        {
            get => GetOutput(_unkRen);
            set => _unkRen = value;
        }
        private string _haiSYmd;
        public string HaiSYmd
        {
            get => GetOutput(_haiSYmd);
            set => _haiSYmd = value;
        }
        private string _touYmd;
        public string TouYmd
        {
            get => GetOutput(_touYmd);
            set => _touYmd = value;
        }
        private string _gyosyaCd;
        public string GyosyaCd
        {
            get => GetOutput(_gyosyaCd);
            set => _gyosyaCd = value;
        }
        private string _tokuiCd;
        public string TokuiCd
        {
            get => GetOutput(_tokuiCd);
            set => _tokuiCd = value;
        }
        private string _sitenCd;
        public string SitenCd
        {
            get => GetOutput(_sitenCd);
            set => _sitenCd = value;
        }
        private string _skTokuiNm;
        public string SkTokuiNm
        {
            get => GetOutput(_skTokuiNm);
            set => _skTokuiNm = value;
        }
        private string _stSitenNm;
        public string StSitenNm
        {
            get => GetOutput(_stSitenNm);
            set => _stSitenNm = value;
        }
        private string _skRyakuNm;
        public string SkRyakuNm
        {
            get => GetOutput(_skRyakuNm);
            set => _skRyakuNm = value;
        }
        private string _stRyakuNm;
        public string StRyakuNm
        {
            get => GetOutput(_stRyakuNm);
            set => _stRyakuNm = value;
        }
        private string _tokuiTanNm;
        public string TokuiTanNm
        {
            get => GetOutput(_tokuiTanNm);
            set => _tokuiTanNm = value;
        }
        private string _tokuiTel;
        public string TokuiTel
        {
            get => GetOutput(_tokuiTel);
            set => _tokuiTel = value;
        }
        private string _eigyoCd;
        public string EigyoCd
        {
            get => GetOutput(_eigyoCd);
            set => _eigyoCd = value;
        }
        private string _eigyoNm;
        public string EigyoNm
        {
            get => GetOutput(_eigyoNm);
            set => _eigyoNm = value;
        }
        private string _eigyosRyakuNm;
        public string EigyosRyakuNm
        {
            get => GetOutput(_eigyosRyakuNm);
            set => _eigyosRyakuNm = value;
        }
        private string _danTaNm;
        public string DanTaNm
        {
            get => GetOutput(_danTaNm);
            set => _danTaNm = value;
        }
        private string _ikNm;
        public string IkNm
        {
            get => GetOutput(_ikNm);
            set => _ikNm = value;
        }
        private string _u_HaiSNm;
        public string U_HaiSNm
        {
            get => GetOutput(_u_HaiSNm);
            set => _u_HaiSNm = value;
        }
        private string _u_HaiSTime;
        public string U_HaiSTime
        {
            get => GetOutput(_u_HaiSTime);
            set => _u_HaiSTime = value;
        }
        private string _u_HaiSJyus1;
        public string U_HaiSJyus1
        {
            get => GetOutput(_u_HaiSJyus1);
            set => _u_HaiSJyus1 = value;
        }
        private string _u_HaiSJyus2;
        public string U_HaiSJyus2
        {
            get => GetOutput(_u_HaiSJyus2);
            set => _u_HaiSJyus2 = value;
        }
        private string _bunruiCodeKbn;
        public string BunruiCodeKbn
        {
            get => GetOutput(_bunruiCodeKbn);
            set => _bunruiCodeKbn = value;
        }
        private string _haiSKoukCd;
        public string HaiSKoukCd
        {
            get => GetOutput(_haiSKoukCd);
            set => _haiSKoukCd = value;
        }
        private string _jM_HaiSBunrui_CodeKbnNm;
        public string JM_HaiSBunrui_CodeKbnNm
        {
            get => GetOutput(_jM_HaiSBunrui_CodeKbnNm);
            set => _jM_HaiSBunrui_CodeKbnNm = value;
        }
        private string _jM_HaiSBunrui_RyakuNm;
        public string JM_HaiSBunrui_RyakuNm
        {
            get => GetOutput(_jM_HaiSBunrui_RyakuNm);
            set => _jM_HaiSBunrui_RyakuNm = value;
        }
        private string _jM_HaiSKoutu_KoukNm;
        public string JM_HaiSKoutu_KoukNm
        {
            get => GetOutput(_jM_HaiSKoutu_KoukNm);
            set => _jM_HaiSKoutu_KoukNm = value;
        }
        private string _jM_HaiSKoutu_RyakuNm;
        public string JM_HaiSKoutu_RyakuNm
        {
            get => GetOutput(_jM_HaiSKoutu_RyakuNm);
            set => _jM_HaiSKoutu_RyakuNm = value;
        }
        private string _jM_HaiSBin_BinCd;
        public string JM_HaiSBin_BinCd
        {
            get => GetOutput(_jM_HaiSBin_BinCd);
            set => _jM_HaiSBin_BinCd = value;
        }
        private string _jM_HaiSBin_BinNm;
        public string JM_HaiSBin_BinNm
        {
            get => GetOutput(_jM_HaiSBin_BinNm);
            set => _jM_HaiSBin_BinNm = value;
        }
        private string _jT_Unkobi_TouNm;
        public string JT_Unkobi_TouNm
        {
            get => GetOutput(_jT_Unkobi_TouNm);
            set => _jT_Unkobi_TouNm = value;
        }
        private string _jT_Unkobi_TouChTime;
        public string JT_Unkobi_TouChTime
        {
            get => GetOutput(_jT_Unkobi_TouChTime);
            set => _jT_Unkobi_TouChTime = value;
        }
        private string _jT_Unkobi_TouJyusyo1;
        public string JT_Unkobi_TouJyusyo1
        {
            get => GetOutput(_jT_Unkobi_TouJyusyo1);
            set => _jT_Unkobi_TouJyusyo1 = value;
        }
        private string _jT_Unkobi_TouJyusyo2;
        public string JT_Unkobi_TouJyusyo2
        {
            get => GetOutput(_jT_Unkobi_TouJyusyo2);
            set => _jT_Unkobi_TouJyusyo2 = value;
        }
        private string _jM_TouChaBunrui_CodeKbn;
        public string JM_TouChaBunrui_CodeKbn
        {
            get => GetOutput(_jM_TouChaBunrui_CodeKbn);
            set => _jM_TouChaBunrui_CodeKbn = value;
        }
        private string _jM_TouChaKoutu_KoukCd;
        public string JM_TouChaKoutu_KoukCd
        {
            get => GetOutput(_jM_TouChaKoutu_KoukCd);
            set => _jM_TouChaKoutu_KoukCd = value;
        }
        private string _jM_TouChaBunrui_CodeKbnNm;
        public string JM_TouChaBunrui_CodeKbnNm
        {
            get => GetOutput(_jM_TouChaBunrui_CodeKbnNm);
            set => _jM_TouChaBunrui_CodeKbnNm = value;
        }
        private string _jM_TouChaBunrui_RyakuNm;
        public string JM_TouChaBunrui_RyakuNm
        {
            get => GetOutput(_jM_TouChaBunrui_RyakuNm);
            set => _jM_TouChaBunrui_RyakuNm = value;
        }
        private string _jM_TouChaKoutu_KoukNm;
        public string JM_TouChaKoutu_KoukNm
        {
            get => GetOutput(_jM_TouChaKoutu_KoukNm);
            set => _jM_TouChaKoutu_KoukNm = value;
        }
        private string _jM_TouChaKoutu_RyakuNm;
        public string JM_TouChaKoutu_RyakuNm
        {
            get => GetOutput(_jM_TouChaKoutu_RyakuNm);
            set => _jM_TouChaKoutu_RyakuNm = value;
        }
        private string _jM_TouChaBin_BinCd;
        public string JM_TouChaBin_BinCd
        {
            get => GetOutput(_jM_TouChaBin_BinCd);
            set => _jM_TouChaBin_BinCd = value;
        }
        private string _jM_TouChaBin_BinNm;
        public string JM_TouChaBin_BinNm
        {
            get => GetOutput(_jM_TouChaBin_BinNm);
            set => _jM_TouChaBin_BinNm = value;
        }
        private string _jT_Unkobi_JyoSyaJin;
        public string JT_Unkobi_JyoSyaJin
        {
            get => GetOutput(_jT_Unkobi_JyoSyaJin);
            set => _jT_Unkobi_JyoSyaJin = value;
        }
        private string _jT_Unkobi_PlusJin;
        public string JT_Unkobi_PlusJin
        {
            get => GetOutput(_jT_Unkobi_PlusJin);
            set => _jT_Unkobi_PlusJin = value;
        }
        private string _jT_SumYykSyu_SumDai;
        public string JT_SumYykSyu_SumDai
        {
            get => GetOutput(_jT_SumYykSyu_SumDai);
            set => _jT_SumYykSyu_SumDai = value;
        }
        private string _jT_SumHaisha_SumSyaRyoUnc;
        public string JT_SumHaisha_SumSyaRyoUnc
        {
            get => GetOutput(_jT_SumHaisha_SumSyaRyoUnc);
            set => _jT_SumHaisha_SumSyaRyoUnc = value;
        }
        private string _jT_Yyksho_ZeiKbn;
        public string JT_Yyksho_ZeiKbn
        {
            get => GetOutput(_jT_Yyksho_ZeiKbn);
            set => _jT_Yyksho_ZeiKbn = value;
        }
        private string _jM_ZeiKbn_RyakuNm;
        public string JM_ZeiKbn_RyakuNm
        {
            get => GetOutput(_jM_ZeiKbn_RyakuNm);
            set => _jM_ZeiKbn_RyakuNm = value;
        }
        private string _jT_Yyksho_Zeiritsu;
        public string JT_Yyksho_Zeiritsu
        {
            get => GetOutput(_jT_Yyksho_Zeiritsu);
            set => _jT_Yyksho_Zeiritsu = value;
        }
        private string _jT_SumHaisha_SumSyaRyoSyo;
        public string JT_SumHaisha_SumSyaRyoSyo
        {
            get => GetOutput(_jT_SumHaisha_SumSyaRyoSyo);
            set => _jT_SumHaisha_SumSyaRyoSyo = value;
        }
        private string _jT_Yyksho_TesuRitu;
        public string JT_Yyksho_TesuRitu
        {
            get => GetOutput(_jT_Yyksho_TesuRitu);
            set => _jT_Yyksho_TesuRitu = value;
        }
        private string _jT_SumHaisha_SumSyaRyoTes;
        public string JT_SumHaisha_SumSyaRyoTes
        {
            get => GetOutput(_jT_SumHaisha_SumSyaRyoTes);
            set => _jT_SumHaisha_SumSyaRyoTes = value;
        }
        private string _jT_SumHaisha_Charge;
        public string JT_SumHaisha_Charge
        {
            get => GetOutput(_jT_SumHaisha_Charge);
            set => _jT_SumHaisha_Charge = value;
        }
        private string _jT_SumFutTumGui_SumUriGakKin;
        public string JT_SumFutTumGui_SumUriGakKin
        {
            get => GetOutput(_jT_SumFutTumGui_SumUriGakKin);
            set => _jT_SumFutTumGui_SumUriGakKin = value;
        }
        private string _jT_SumFutTumGui_SumSyaRyoSyo;
        public string JT_SumFutTumGui_SumSyaRyoSyo
        {
            get => GetOutput(_jT_SumFutTumGui_SumSyaRyoSyo);
            set => _jT_SumFutTumGui_SumSyaRyoSyo = value;
        }
        private string _jT_SumFutTumGui_SumSyaRyoTes;
        public string JT_SumFutTumGui_SumSyaRyoTes
        {
            get => GetOutput(_jT_SumFutTumGui_SumSyaRyoTes);
            set => _jT_SumFutTumGui_SumSyaRyoTes = value;
        }
        private string _jT_SumFutTumGui_Charge;
        public string JT_SumFutTumGui_Charge
        {
            get => GetOutput(_jT_SumFutTumGui_Charge);
            set => _jT_SumFutTumGui_Charge = value;
        }
        private string _jT_SumFutTum_SumUriGakKin;
        public string JT_SumFutTum_SumUriGakKin
        {
            get => GetOutput(_jT_SumFutTum_SumUriGakKin);
            set => _jT_SumFutTum_SumUriGakKin = value;
        }
        private string _jT_SumFutTum_SumSyaRyoSyo;
        public string JT_SumFutTum_SumSyaRyoSyo
        {
            get => GetOutput(_jT_SumFutTum_SumSyaRyoSyo);
            set => _jT_SumFutTum_SumSyaRyoSyo = value;
        }
        private string _jT_SumFutTum_SumSyaRyoTes;
        public string JT_SumFutTum_SumSyaRyoTes
        {
            get => GetOutput(_jT_SumFutTum_SumSyaRyoTes);
            set => _jT_SumFutTum_SumSyaRyoTes = value;
        }
        private string _total_YFutum;
        public string Total_YFutum
        {
            get => GetOutput(_total_YFutum);
            set => _total_YFutum = value;
        }
        public string GyosyaKbn { get; set; }
        public string You_TokuiCd { get; set; }
        public string YouSiten_SitenCd { get; set; }
        public string You_TokuiNm { get; set; }
        public string YouSiten_SitenNm { get; set; }
        public string You_RyakuNm { get; set; }
        public string YouSiten_RyakuNm { get; set; }
        public string H_GoSya { get; set; }
        public string H_DanTaNm2 { get; set; }
        public string H_IkNm { get; set; }
        public string H_HaiSNm { get; set; }
        public string H_HaiSYmd { get; set; }
        public string H_HaiSTime { get; set; }
        public string H_HaiSJyus1 { get; set; }
        public string H_HaiSJyus2 { get; set; }
        public string JM_YouHaiSBunrui_CodeKb { get; set; }
        public string JM_YouHaiSKoutu_KoukCd { get; set; }
        public string JM_YouHaiSBunrui_CodeKbnNm { get; set; }
        public string JM_YouHaiSBunrui_RyakuNm { get; set; }
        public string JM_YouHaiSKoutu_KoukNm { get; set; }
        public string JM_YouHaiSKoutu_RyakuNm { get; set; }
        public string JM_YouHaiSBin_BinCd { get; set; }
        public string JM_YouHaiSBin_BinNm { get; set; }
        public string JT_Haisha_TouNm { get; set; }
        public string JT_Haisha_TouYmd { get; set; }
        public string JT_Haisha_TouChTime { get; set; }
        public string JT_Haisha_TouJyusyo1 { get; set; }
        public string JT_Haisha_TouJyusyo2 { get; set; }
        public string JM_YouTouChaBunrui_CodeKbn { get; set; }
        public string JM_YouTouChaKoutu_KoukCd { get; set; }
        public string JM_YouTouChaBunrui_CodeKbnNm { get; set; }
        public string JM_YouTouChaBunrui_RyakuNm { get; set; }
        public string JM_YouTouChaKoutu_KoukNm { get; set; }
        public string JM_YouTouChaKoutu_RyakuNm { get; set; }
        public string JM_YouTouChaBin_BinCd { get; set; }
        public string JM_YouTouChaBin_BinNm { get; set; }
        public string JT_Haisha_JyoSyaJin { get; set; }
        public string JT_Haisha_PlusJin { get; set; }
        public string JT_Haisha_YoushaUnc { get; set; }
        public string TKD_Yousha_ZeiKbn { get; set; }
        public string JM_YouZeiKbn_CodeKbnNm { get; set; }
        public string JM_YouZeiKbn_RyakuNm { get; set; }
        public string TKD_Yousha_Zeiritsu { get; set; }
        public string JT_Haisha_YoushaSyo { get; set; }
        public string TKD_Yousha_TesuRitu { get; set; }
        public string JT_Haisha_YoushaTes { get; set; }
        public string JT_Haisha_YouCharge { get; set; }

        private string _jT_SumYMFuTuGui_SumHaseiKin;
        public string JT_SumYMFuTuGui_SumHaseiKin
        {
            get { return GetOutput(_jT_SumYMFuTuGui_SumHaseiKin); }
            set { _jT_SumYMFuTuGui_SumHaseiKin = value; }
        }
        private string _jT_SumYMFuTuGui_SumSyaRyoSyo;
        public string JT_SumYMFuTuGui_SumSyaRyoSyo
        {
            get { return GetOutput(_jT_SumYMFuTuGui_SumSyaRyoSyo); }
            set { _jT_SumYMFuTuGui_SumSyaRyoSyo = value; }
        }
        private string _jT_SumYMFuTuGui_SumSyaRyoTes;
        public string JT_SumYMFuTuGui_SumSyaRyoTes
        {
            get { return GetOutput(_jT_SumYMFuTuGui_SumSyaRyoTes); }
            set { _jT_SumYMFuTuGui_SumSyaRyoTes = value; }
        }
        private string _jT_SumYMFuTuGui_Charge;
        public string JT_SumYMFuTuGui_Charge
        {
            get { return GetOutput(_jT_SumYMFuTuGui_Charge); }
            set { _jT_SumYMFuTuGui_Charge = value; }
        }
        private string _jT_SumYMFuTu_SumHaseiKin;
        public string JT_SumYMFuTu_SumHaseiKin
        {
            get { return GetOutput(_jT_SumYMFuTu_SumHaseiKin); }
            set { _jT_SumYMFuTu_SumHaseiKin = value; }
        }
        private string _jT_SumYMFuTu_SumSyaRyoSyo;
        public string JT_SumYMFuTu_SumSyaRyoSyo
        {
            get { return GetOutput(_jT_SumYMFuTu_SumSyaRyoSyo); }
            set { _jT_SumYMFuTu_SumSyaRyoSyo = value; }
        }
        private string _jT_SumYMFuTu_SumSyaRyoTes;
        public string JT_SumYMFuTu_SumSyaRyoTes
        {
            get { return GetOutput(_jT_SumYMFuTu_SumSyaRyoTes); }
            set { _jT_SumYMFuTu_SumSyaRyoTes = value; }
        }
        private string _total_YMFutum;
        public string Total_YMFutum
        {
            get { return GetOutput(_total_YMFutum); }
            set { _total_YMFutum = value; }
        }
    }
}
