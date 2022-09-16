using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadSaleBranch
    {
        /// <summary>
        /// Mark this object is selected all item. Default values is <c>false</c>
        /// </summary>
        public bool IsSelectedAll { get; set; } = false;

        public int CompanyCdSeq { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRyakuNm { get; set; }
        public int CompanyCd { get; set; }
        public int EigyoCdSeq { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoName { get; set; }
        public string EigyoRyakuName { get; set; }
        public int TransportationPlaceCodeSeq { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0}：{1}　{2}：{3}", CompanyCd.ToString("D3"), CompanyName, EigyoCd.ToString("D5"), EigyoName);
            }
        }
        /// <summary>
        /// Get branch data info.
        /// <para>Format => [CompanyCd : CompanyRyakuNm EigyoCd : EigyoNm]</para>
        /// <para>If EigyoCd is 0, return "すべて"</para>
        /// <para>Else if can not convert to any format will return empty string</para>
        /// </summary>
        public string BranchInfo
        {
            get
            {
                if (CompanyCd > 0  && EigyoCd > 0 )
                {
                    return $"{CompanyCd.ToString("D3")}：{CompanyRyakuNm}　{EigyoCd.ToString("D5")}：{EigyoName}";
                }
                else if (IsSelectedAll || EigyoCd == 0)
                    return Constants.SelectedAll;

                return string.Empty;
            }
        }
        /// <summary>
        /// Get branch data display text.
        /// <para>Format => [EigyoCd : EigyoNm]</para>
        /// <para>If EigyoCd is 0, return "すべて"</para>
        /// <para>Else if can not convert to any format will return empty string</para>
        /// </summary>
        public string BranchText
        {
            get
            {
                if (EigyoCd > 0 && !string.IsNullOrEmpty(EigyoName))
                {
                    return $"{EigyoCd.ToString("D5")}：{EigyoName}";
                }
                else if (IsSelectedAll)
                    return Constants.SelectedAll;

                return string.Empty;
            }
        }
        public string BranchTextReport
        {
            get
            {
                if (EigyoCdSeq > 0)
                {
                    return $"{EigyoCd.ToString("D5")}：{EigyoName}";
                }              
                return string.Empty;
            }
        }
    }

    public class LoadSaleBranchList
    {
        public int EigyoCdSeq { get; set; } = -1;
        public int EigyoCd { get; set; }
        public string RyakuNm { get; set; }
        public int CompanyCdSeq { get; set; }
        public string Text => EigyoCdSeq == -1 ? "" : $"{EigyoCd.ToString("D5")} : {RyakuNm}";
    }

    public class SaleBranchData
    {
        public int EigyoCdSeq { get; set; } = -1;
        public int EigyoCd { get; set; }
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
        public string ExpItem { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string Text => EigyoCdSeq == -1 ? "" : $"{EigyoCd.ToString("D5")} : {RyakuNm}";

        public SaleBranchData()
        {

        }

        public SaleBranchData(VpmEigyos VpmEigyos)
        {
            this.EigyoCdSeq = VpmEigyos.EigyoCdSeq;
            this.EigyoCd = VpmEigyos.EigyoCd;
            this.CompanyCdSeq = VpmEigyos.CompanyCdSeq;
            this.EigyoNm = VpmEigyos.EigyoNm;
            this.RyakuNm = VpmEigyos.RyakuNm;
            this.ZipCd = VpmEigyos.ZipCd;
            this.Jyus1 = VpmEigyos.Jyus1;
            this.Jyus2 = VpmEigyos.Jyus2;
            this.TelNo = VpmEigyos.TelNo;
            this.FaxNo = VpmEigyos.FaxNo;
            this.SeiEigCdSeq = VpmEigyos.SeiEigCdSeq;
            this.BankCd1 = VpmEigyos.BankCd1;
            this.BankSitCd1 = VpmEigyos.BankSitCd1;
            this.YokinSyu1 = VpmEigyos.YokinSyu1;
            this.KouzaNo1 = VpmEigyos.KouzaNo1;
            this.BankCd2 = VpmEigyos.BankCd2;
            this.BankSitCd2 = VpmEigyos.BankSitCd2;
            this.YokinSyu2 = VpmEigyos.YokinSyu2;
            this.KouzaNo2 = VpmEigyos.KouzaNo2;
            this.BankCd3 = VpmEigyos.BankCd3;
            this.BankSitCd3 = VpmEigyos.BankSitCd3;
            this.YokinSyu3 = VpmEigyos.YokinSyu3;
            this.KouzaNo3 = VpmEigyos.KouzaNo3;
            this.KouzaMeigi = VpmEigyos.KouzaMeigi;
            this.SmtpdomNm = VpmEigyos.SmtpdomNm;
            this.SmtpsvrNm = VpmEigyos.SmtpsvrNm;
            this.SmtpportNo = VpmEigyos.SmtpportNo;
            this.Smtpninsyo = VpmEigyos.Smtpninsyo;
            this.PopsvrNm = VpmEigyos.PopsvrNm;
            this.PopportNo = VpmEigyos.PopportNo;
            this.Popninsyo = VpmEigyos.Popninsyo;
            this.MailUser = VpmEigyos.MailUser;
            this.MailPass = VpmEigyos.MailPass;
            this.MailAcc = VpmEigyos.MailAcc;
            this.KasEigFlg = VpmEigyos.KasEigFlg;
            this.NorEigFlg = VpmEigyos.NorEigFlg;
            this.SokoJunKbn = VpmEigyos.SokoJunKbn;
            this.CalcuSyuTime = VpmEigyos.CalcuSyuTime;
            this.CalcuTaiTime = VpmEigyos.CalcuTaiTime;
            this.ExpItem = VpmEigyos.ExpItem;
            this.SiyoKbn = VpmEigyos.SiyoKbn;
            this.UpdYmd = VpmEigyos.UpdYmd;
            this.UpdTime = VpmEigyos.UpdTime;
            this.UpdSyainCd = VpmEigyos.UpdSyainCd;
            this.UpdPrgId = VpmEigyos.UpdPrgId;
        }
    }
}
