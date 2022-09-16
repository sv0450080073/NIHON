using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class FareFeeCorrectionModel
    {
        public string SyaRyoUnc { get; set; }
        public string SyaRyoSyo { get; set; }
        public string SyaRyoTes { get; set; }
        public string YouUnc { get; set; }
        public string YouZei { get; set; }
        public string YouTes { get; set; }
        public string UntKin { get; set; }
        public string ZeiKbn { get; set; }
        public string ZeiRitu { get; set; }
        public string ZeiRuiGaku { get; set; }
        public string TesuRyo { get; set; }
        public string TesuRyoGaku { get; set; }
        public int ActiveV { get; set; }
        public int ActiveL { get; set; }
    }

    public class CompanyValidate
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string RyakuNm { get; set; }
        public string SyoriYm { get; set; }
    }

    public class Reservation
    {
        public string YoyaSyuCodeKbnNm { get; set; }
        public string YoyaSyuRyakuNm { get; set; }
        public byte YoyaKbnSeqYoyaKbn { get; set; }
        public string YoyaKbnSeqYoyaKbnNm { get; set; }
        public int UkeEigCdSeqEigyoCd { get; set; }
        public string UkeEigCdSeqEigyoNm { get; set; }
        public string UkeEigCdSeqRyakuNm { get; set; }
        public int UkeCompanyCdSeqCompanyCd { get; set; }
        public string UkeCompanyCdSeqCompanyNm { get; set; }
        public string UkeCompanyCdSeqRyakuNm { get; set; }
        public int SeiEigCdSeqEigyoCd { get; set; }
        public string SeiEigCdSeqEigyoNm { get; set; }
        public string SeiEigCdSeqRyakuNm { get; set; }
        public int SeiCompanyCdSeqCompanyCd { get; set; }
        public string SeiCompanyCdSeqCompanyNm { get; set; }
        public string SeiCompanyCdSeqRyakuNm { get; set; }
        public int IraEigCdSeqEigyoCd { get; set; }
        public string IraEigCdSeqEigyoNm { get; set; }
        public string IraEigCdSeqRyakuNm { get; set; }
        public int IraCompanyCdSeqCompanyCd { get; set; }
        public string IraCompanyCdSeqCompanyNm { get; set; }
        public string IraCompanyCdSeqRyakuNm { get; set; }
        public string EigTanCdSeqSyainCd { get; set; }
        public string EigTanCdSeqSyainNm { get; set; }
        public string InTanCdSeqSyainCd { get; set; }
        public string InTanCdSeqSyainNm { get; set; }
        public short TokSeqTokuiCd { get; set; }
        public string TokSeqKana { get; set; }
        public string TokSeqTokuiNm { get; set; }
        public string TokSeqRyakuNm { get; set; }
        public int TokSeqGyosyaCdSeq { get; set; }
        public short TokGyosyaCdSeqGyosyaCd { get; set; }
        public string TokGyosyaCdSeqGyosyaNm { get; set; }
        public byte TokGyosyaCdSeqGyosyaKbn { get; set; }
        public string TokGyosyaKbnCodeKbnNm { get; set; }
        public string TokGyosyaKbnRyakuNm { get; set; }
        public short SitenCdSeqSitenCd { get; set; }
        public string SitenCdSeqKana { get; set; }
        public string SitenCdSeqSitenNm { get; set; }
        public string SitenCdSeqRyakuNm { get; set; }
        public int SitenCdSeqSeiCdSeq { get; set; }
        public int SitenCdSeqSeiSitenCdSeq { get; set; }
        public decimal SitenCdSeqTesuRituFut { get; set; }
        public decimal SitenCdSeqTesuRituGui { get; set; }
        public byte SitenCdSeqTesKbnFut { get; set; }
        public byte SitenCdSeqTesKbnGui { get; set; }
        public short SirCdSeqTokuiCd { get; set; }
        public string SirCdSeqKana { get; set; }
        public string SirCdSeqTokuiNm { get; set; }
        public string SirCdSeqRyakuNm { get; set; }
        public int SirCdSeqGyosyaCdSeq { get; set; }
        public short SirGyosyaCdSeqGyosyaCd { get; set; }
        public string SirGyosyaCdSeqGyosyaNm { get; set; }
        public byte SirGyosyaCdSeqGyosyaKbn { get; set; }
        public string SirGyoSyaKbnCodeKbnNm { get; set; }
        public string SirGyoSyaKbnRyakuNm { get; set; }
        public short SirSitenCdSeqSitenCd { get; set; }
        public string SirSitenCdSeqKana { get; set; }
        public string SirSitenCdSeqSitenNm { get; set; }
        public string SirSitenCdSeqRyakuNm { get; set; }
        public string ZeiKbnCodeKbnNm { get; set; }
        public string ZeiKbnRyakuNm { get; set; }
        public string SeiKyuKbnSeqCodeKbnNm { get; set; }
        public string SeiKyuKbnSeqRyakuNm { get; set; }
        public string CanZKbnCodeKbnNm { get; set; }
        public string CanZKbnRyakuNm { get; set; }
        public string CanTanSeqSyainCd { get; set; }
        public string CanTanSeqSyainNm { get; set; }
        public string CanFuTanSeqSyainCd { get; set; }
        public string CanFuTanSeqSyainNm { get; set; }
        public string KSKbnCodeKbnNm { get; set; }
        public string KSKbnRyakuNm { get; set; }
        public string KHinKbnCodeKbnNm { get; set; }
        public string KHinKbnRyakuNm { get; set; }
        public string HaiSKbnCodeKbnNm { get; set; }
        public string HaiSKbnRyakuNm { get; set; }
        public string HaiIKbnCodeKbnNm { get; set; }
        public string HaiIKbnRyakuNm { get; set; }
        public string NippoKbnCodeKbnNm { get; set; }
        public string NippoKbnRyakuNm { get; set; }
        public string YouKbnCodeKbnNm { get; set; }
        public string YouKbnRyakuNm { get; set; }
        public string NyuKinKbnCodeKbnNm { get; set; }
        public string NyuKinKbnRyakuNm { get; set; }
        public string NCouKbnCodeKbnNm { get; set; }
        public string NCouKbnRyakuNm { get; set; }
        public string SihKbnCodeKbnNm { get; set; }
        public string SihKbnRyakuNm { get; set; }
        public string SCouKbnCodeKbnNm { get; set; }
        public string SCouKbnRyakuNm { get; set; }
        public string UpdSyainCdSyainCd { get; set; }
        public string UpdSyainCdSyainNm { get; set; }
        public decimal UntKin { get; set; }
        public byte ZeiKbn { get; set; }
        public decimal Zeiritsu { get; set; }
        public decimal ZeiRui { get; set; }
        public decimal TesuRitu { get; set; }
        public decimal TesuRyoG { get; set; }
    }

    public class VehicleAllocation
    {
        public byte KaknKais { get; set; }
        public string KaktYmd { get; set; }
        public string DanTaNm { get; set; }
        public short YouCdSeqTokuiCd { get; set; }
        public string YouCdSeqRyakuNm { get; set; }
        public short YouSitCdSeqSitenCd { get; set; }
        public string YouSitCdSeqRyakuNm { get; set; }
        public int SyuEigCdSeqEigyoCd { get; set; }
        public string SyuEigCdSeqRyakuNm { get; set; }
        public int KikSEigCdSeqEigyoCd { get; set; }
        public string KikSEigCdSeqRyakuNm { get; set; }
        public int HaiSSryCdSeqEigyoCd { get; set; }
        public string HaiSSryCdSeqRyakuNm { get; set; }
        public int KSSyaRSeqEigyoCd { get; set; }
        public string KSSyaRSeqRyakuNm { get; set; }
        public int HaiSSyaRCdSeqSyaRyoCd { get; set; }
        public string HaiSSyaRCdSeqSyaRyoNm { get; set; }
        public int HaiSSyaRCdSeqSyainCdSeq { get; set; }
        public int KSSyaRCdSeqSyaRyoCd { get; set; }
        public string KSSyaRCdSeqKariSyaRyoNm { get; set; }
        public int KSSyaRCdSeqSyainCdSeq { get; set; }
        public short HaiSSyaSCdSeqSyaSyuCd { get; set; }
        public string HaiSSyaSCdSeqSyaSyuNm { get; set; }
        public short KSSyaSCdSeqSyaSyuCd { get; set; }
        public string KSSyaSCdSeqSyaSyuNm { get; set; }
        public int HaiChBunruiCdSeq { get; set; }
        public string HaiChHaiSCd { get; set; }
        public int HaiKoBunruiCdSeq { get; set; }
        public int HaiKoKouKCd { get; set; }
        public string HaiKoBunruiCd { get; set; }
        public string HaiKoBunruiRyakuNm { get; set; }
        public int HaiBBinCd { get; set; }
        public int TouChBunruiCdSeq { get; set; }
        public string TouChHaiSCd { get; set; }
        public int TouKoBunruiCdSeq { get; set; }
        public int TouKoKouKCd { get; set; }
        public string TouKoBunruiCd { get; set; }
        public string TouKoBunruiRyakuNm { get; set; }
        public int TouBBinCd { get; set; }
        public string OthJinKbn1RyakuNm { get; set; }
        public string OthJinKbn2RyakuNm { get; set; }
        public string YouKataKbnRyakuNm { get; set; }
        public string UkeJyKbnRyakuNm { get; set; }
        public string TokuiSeqRyakuNm { get; set; }
        public string SitenCdSeqRyakuNm { get; set; }
        public string UHaiSYmd { get; set; }
        public string UHaiSTime { get; set; }
        public string UTouYmd { get; set; }
        public string UTouChTime { get; set; }
        public byte UZenHaFlg { get; set; }
        public byte UKhakFlg { get; set; }
        public int IkBasyoKenCdSeq { get; set; }
        public string IkBasyoMapCd { get; set; }
        public short YouGyosyaCdSeqGyosyaCd { get; set; }
        public string YouGyosyaCdSeqGyosyaNm { get; set; }
        public string UkeNo { get; set; }
        public short YouUnkRen { get; set; }
        public int YouTblSeq { get; set; }
        public short YouHenKai { get; set; }
        public int YouYouCdSeq { get; set; }
        public int YouYouSitCdSeq { get; set; }
        public string YouHasYmd { get; set; }
        public string YouSihYotYmd { get; set; }
        public string YouSihYm { get; set; }
        public int YouSyaRyoUnc { get; set; }
        public byte YouZeiKbn { get; set; }
        public decimal YouZeiritsu { get; set; }
        public int YouSyaRyoSyo { get; set; }
        public decimal YouTesuRitu { get; set; }
        public int YouSyaRyoTes { get; set; }
        public byte YouJitaFlg { get; set; }
        public int YouCompanyCdSeq { get; set; }
        public byte YouSihKbn { get; set; }
        public byte YouSiyoKbn { get; set; }
        public string YouUpdYmd { get; set; }
        public string YouUpdTime { get; set; }
        public int YouUpdSyainCd { get; set; }
        public string YouUpdPrgID { get; set; }
        public byte HaiSSyaSCdSeqKataKbn { get; set; }
        public byte KSSyaSCdSeqKataKbn { get; set; }
        public byte MiKSSyaSCdSeqKataKbn { get; set; }
        public int UkeEigCdSeqCompanyCd { get; set; }
        public int HaiSSryCdSeqCompanyCd { get; set; }
        public int KSSyaRSeqCompanyCd { get; set; }
        public int SyuEigCdSeqCompanyCd { get; set; }
        public short TotalDrvJin { get; set; }
        public short TotalGuiSu { get; set; }
        public string GoSya { get; set; }
        public short BunKSyuJyn { get; set; }
        public byte HaiSKbn { get; set; }
        public byte KSKbn { get; set; }
        public int SyaRyoUnc { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public int YoushaUnc { get; set; }
        public int YoushaSyo { get; set; }
        public int YoushaTes { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
    }

    public class Vehicle
    {
        public short YouCdSeqTokuiCd { get; set; }
        public string YouCdSeqKana { get; set; }
        public string YouCdSeqTokuiNm { get; set; }
        public string YouCdSeqRyakuNm { get; set; }
        public int YouCdSeqGyosyaCdSeq { get; set; }
        public short YouGyosyaCdSeqGyosyaCd { get; set; }
        public string YouGyosyaCdSeqGyosyaNm { get; set; }
        public byte YouGyosyaCdSeqGyosyaKbn { get; set; }
        public string GyoSyaKbnCodeKbnNm { get; set; }
        public string GyoSyaKbnRyakuNm { get; set; }
        public short YouSitCdSeqSitenCd { get; set; }
        public string YouSitCdSeqKana { get; set; }
        public string YouSitCdSeqSitenNm { get; set; }
        public string YouSitCdSeqRyakuNm { get; set; }
        public string ZeiKbnRyakuNm { get; set; }
        public string JitaFlgCodeKbnNm { get; set; }
        public string JitaFlgRyakuNm { get; set; }
        public int CompanyCdSeqCompnyCd { get; set; }
        public string CompanyCdSeqCompnyNm { get; set; }
        public string CompanyCdSeqRyakuNm { get; set; }
        public string SihKbnCodeKbnNm { get; set; }
        public string SihKbnRyakuNm { get; set; }
        public string SCouKbnCodeKbnNm { get; set; }
        public string SCouKbnRyakuNm { get; set; }
        public string UpdSyainCdSyainCd { get; set; }
        public string UpdSyainCdSyainNm { get; set; }
        public int YouTblSeq { get; set; }
        public short UnkRen { get; set; }
    }

    public class ReservationGrid
    {
        public int No { get; set; } = 1;
        public string UnkYmd { get; set; }
        public string GoSya { get; set; }
        public string Eigyo { get; set; }
        public string SyaRyo { get; set; }
        public string YouSha { get; set; }
        public int SyaRyoUnc
        {
            get { if (Flag1 == true) { return SyaRyoUncTmp; } else { return 0; } }
            set
            {
                SyaRyoUncTmp = value;
            }
        }
        public int SyaRyoSyo
        {
            get { if (Flag1 == true) { return SyaRyoSyoTmp; } else { return 0; } }
            set
            {
                SyaRyoSyoTmp = value;
            }
        }
        public int SyaRyoTes
        {
            get { if (Flag1 == true) { return SyaRyoTesTmp; } else { return 0; } }
            set
            {
                SyaRyoTesTmp = value;
            }
        }
        public int YouUnc
        {
            get { if (Flag1 == true) { return YouUncTmp; } else { return 0; } }
            set
            {
                YouUncTmp = value;
            }
        }
        public int YouZei
        {
            get { if (Flag1 == true) { return YouZeiTmp; } else { return 0; } }
            set
            {
                YouZeiTmp = value;
            }
        }
        public int YouTes
        {
            get { if (Flag1 == true) { return YouTesTmp; } else { return 0; } }
            set
            {
                YouTesTmp = value;
            }
        }
        public string HaishaYmd { get; set; }
        public string TouYmd { get; set; }
        public string CurrentDate { get; set; }
        public bool Flag1 => UkeCompanyCdSeqCompanyCd == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID ? true : (UkeCompanyCdSeqCompanyCd != new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID ? false : ((UkeCompanyCdSeqCompanyCd != new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID && CompanyCd == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID && YouJitaFlg == 0) ? true : false));
        public bool Flag2 => UkeCompanyCdSeqCompanyCd == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID ? true : (UkeCompanyCdSeqCompanyCd != new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID ? false : ((UkeCompanyCdSeqCompanyCd != new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID && CompanyCd == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID && YouJitaFlg != 0) ? true : false));
        public int CompanyCd { get; set; }
        public int UkeCompanyCdSeqCompanyCd { get; set; }
        public byte YouJitaFlg { get; set; }
        public int SyaRyoUncTmp { get; set; }
        public int SyaRyoSyoTmp { get; set; }
        public int SyaRyoTesTmp { get; set; }
        public int YouUncTmp { get; set; }
        public int YouZeiTmp { get; set; }
        public int YouTesTmp { get; set; }
        public int YouTblSeq { get; set; }
        public short UnkRen { get; set; }
        public string UkeNo { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public bool IsExistVehicleData { get; set; }
        public List<ReservationChange> ReservationChange { get; set; }
    }

    public class ReservationChange
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public int SyaRyoUnc { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public int YouUnc { get; set; }
        public int YouZei { get; set; }
        public int YouTes { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
    }

    public class HaitaCheckModel
    {
        public long YykshoUpdYmdTime { get; set; }
        public long HaishaMaxUpdYmdTime { get; set; }
        public long YoushaMaxUpdYmdTime { get; set; }
    }
}
