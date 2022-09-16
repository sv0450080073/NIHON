using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class AdvancePaymentDetailsModel
    {
        public string UkeNo { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string SeiEigZipCd { get; set; }
        public string SeiEigJyus1 { get; set; }
        public string SeiEigEigyoNm { get; set; }
        public string SeiEigCompanyNm { get; set; }
        public string SeiEigTelNo { get; set; }
        public string SeiEigFaxNo { get; set; }
        public string BankNm1 { get; set; }
        public string BankSitNm1 { get; set; }
        public string YokinSyuNm1 { get; set; }
        public string KouzaNo1 { get; set; }
        public string BankNm2 { get; set; }
        public string BankSitNm2 { get; set; }
        public string YokinSyuNm2 { get; set; }
        public string KouzaNo2 { get; set; }
        public string KouzaMeigi { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string DanTaNm { get; set; }
        public int SyaSyuDaiCnt { get; set; }
        public int EtcGaku { get; set; }
        public int TatekaeGaku { get; set; }
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public short SitenCd { get; set; }
    }

    public class AdvancePaymentDetailsChildModel
    {
        public int FutGuiKbn { get; set; }
        public string UkeNo { get; set; }
        public string GoSya { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string FutTumNm { get; set; }
        public string IriRyoNm { get; set; }
        public string DeRyoNm { get; set; }
        public int Kingaku { get; set; }

    }

    public class AdvancePaymentDetailsSearchParam : ICloneable
    {
        public byte OutputSetting { get; set; }
        public PaperSizeDropdown PaperSize { get; set; }
        public string ReceptionNumber { get; set; } = string.Empty;
        public DateTime? ScheduleYmdStart { get; set; }
        public DateTime? ScheduleYmdEnd { get; set; }
        public PaymentSearchDropdown? AddressSpectify { get; set; }
        public SeikyuSakiSearch StartAddress { get; set; }
        public SeikyuSakiSearch EndAddress { get; set; }
        public int TenantCdSeq { get; set; } = new ClaimModel().TenantID;
        public CustomerModel CustomerModelFrom { get; set; }
        public CustomerModel CustomerModelTo { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class SeikyuSakiSearch
    {
        public short GyosyaCd { get; set; }
        public short TokuiCd { get; set; }
        public string RyakuNm { get; set; }
        public short SitenCd { get; set; }
        public string SitenNm { get; set; }
        public string Text => $"{TokuiCd.ToString().PadLeft(5, '0')} : {RyakuNm} {SitenCd.ToString().PadLeft(5, '0')} : {SitenNm}";
    }

    public class PaymentSearchDropdown
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class PaperSizeDropdown
    {
        public byte Value { get; set; }
        public string Text { get; set; }
    }

    public class ChildModel
    {
        public AdvancePaymentDetailsChildModel PaymentDetailsChildListLeft { get; set; }
        public AdvancePaymentDetailsChildModel PaymentDetailsChildListRight { get; set; }
    }

    public class AdvancePaymentDetailsReportPDF
    {
        public AdvancePaymentDetailsModel Data { get; set; }
        public List<ChildModel> ChildModel { get; set; }
        public string CurrentDate { get; set; }
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
    }
}
