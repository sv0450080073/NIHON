using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BillOfficeData
    {
        public int EigyoCdSeq { get; set; }
        public string EigyoCd { get; set; }
        public int CompanyCdSeq { get; set; }
        public string EigyoNm { get; set; }
        public string RyakuNm { get; set; }
        public string ZipCd { get; set; }
        public string Jyus1 { get; set; }
        public string Jyus2 { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public int SeiEigCdSeq { get; set; }
        public string BankCd1 { get; set; }
        public string BankSitCd1 { get; set; }
        public byte YokinSyu1 { get; set; }
        public string KouzaNo1 { get; set; }
        public string BankCd2 { get; set; }
        public string BankSitCd2 { get; set; }
        public byte YokinSyu2 { get; set; }
        public string KouzaNo2 { get; set; }
        public string BankCd3 { get; set; }
        public string BankSitCd3 { get; set; }
        public byte YokinSyu3 { get; set; }
        public string KouzaNo3 { get; set; }
        public string KouzaMeigi { get; set; }
        public string SmtpdomNm { get; set; }
        public string SmtpsvrNm { get; set; }
        public string SmtpportNo { get; set; }
        public byte Smtpninsyo { get; set; }
        public string PopsvrNm { get; set; }
        public string PopportNo { get; set; }
        public byte Popninsyo { get; set; }
        public string MailUser { get; set; }
        public string MailPass { get; set; }
        public string MailAcc { get; set; }
        public byte KasEigFlg { get; set; }
        public byte NorEigFlg { get; set; }
        public byte SokoJunKbn { get; set; }
        public string CalcuSyuTime { get; set; }
        public string CalcuTaiTime { get; set; }
        public int TransportationPlaceCodeSeq { get; set; }
        public string ExpItem { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string Text => $"{EigyoCd} : {EigyoNm}";
    }
}
