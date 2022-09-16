using HassyaAllrightCloud.Domain.Entities;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TPM_CodeKbData
    {
        public string CodeKbnName { get; set; }
        public int CodeKb_CodeKbnSeq { get; set; }
        public string CodeKb_CodeSyu { get; set; }
        public string CodeKb_CodeKbn { get; set; }
        public string CodeKb_RyakuNm { get; set; }
        public string Text => $"{CodeKb_CodeKbn}　{CodeKb_RyakuNm}";
        public string TextPartnerBooking => $"{CodeKb_RyakuNm}";

    }

    public class VPM_RepairData
    {
        public int RepairCdSeq { get; set; }
        public int RepairCd { get; set; }
        public string RepairNm { get; set; }
        public string RepairSeiKbn { get; set; }
        public string Text => $"{RepairCd:D2}　{RepairNm}";
    }

    public class TPM_CodeKbDataOTHJINKBN
    {
        public int CodeKb_CodeKbnSeq { get; set; }
        public string CodeKb_CodeKbn { get; set; } = "";
        public string CodeKb_RyakuNm { get; set; } = "";
        public string Text => $"{CodeKb_RyakuNm}";

    }

    public class TPM_CodeKbDataKenCD
    {
        public int CodeKbCodeKbnSeq { get; set; }
        public string CodeKbCodeKbnNm { get; set; } = "";
        public int BasyoBasyoKenCdSeq { get; set; }
        public string BasyoBasyoMapCd { get; set; } = "";
        public int BasyoBasyoMapCdSeq { get; set; } 
        public string BasyoBasyoNm { get; set; } = "";
        public string KenCodeKbnNm { get; set; } = "";
        public string BasyoRyakuNm { get; set; } = "";
        public string Text => BasyoBasyoMapCdSeq == 0 ? string.Empty : $"{CodeKbCodeKbnNm}　{BasyoBasyoMapCd}：{BasyoRyakuNm}";
       
        public string TextHaiShaUpdate
        {
            get
            {
                return String.Format("{0} {1}", CodeKbCodeKbnNm, BasyoRyakuNm);
            }
        }      
    }

    public class TPM_CodeKbDataBunruiCD
    {
        public int CodeKbCodeKbnSeq { get; set; }
        public string CodeKbCodeKbnNm { get; set; } = "";
        public int HaiChiBunruiCdSeq { get; set; }
        public int HaiChiHaiSCdSeq { get; set; }
        public string HaiChiHaiSNm { get; set; } = "";
        public string HaiChiHaiSCd { get; set; } = "";
        public string HaiChiJyus1 { get; set; } = "";
        public string HaiChiJyus2 { get; set; } = "";
        public string HaiChiHaiSKigou { get; set; } = "";
        public string BUNRUICodeKbnNm { get; set; } = "";
        public string Text => HaiChiHaiSCdSeq == 0 ? string.Empty : $"{CodeKbCodeKbnNm}　{HaiChiHaiSCd}：{HaiChiHaiSNm}";
    }
    public class TPM_CodeKbDataDepot
    {
        public int CodeKbCodeKbnSeq { get; set; }
        public string CodeKbCodeKbnNm { get; set; }
        public int KoutuKoukCdSeq { get; set; }
        public int KoutuKoukCd { get; set; }
        public string KoutuRyakuNm { get; set; }
        public int BinBinCdSeq { get; set; }
        public int BinBinCd { get; set; }
        public string BinBinNm { get; set; } = "";
        public int KoutuBunruiCdSeq { get; set; }
        public string BUNRUICodeKbnNm { get; set; }
        public string BINSyuPaTime { get; set; }
        public string BINTouChTime { get; set; }
        public string BINSiyoStaYmd { get; set; }
        public string BINSiyoEndYmd { get; set; }                                
        public string Text => BinBinCdSeq==0?string.Empty:$"({CodeKbCodeKbnNm})　{KoutuKoukCd}：{KoutuRyakuNm}　{BinBinCd}：{BinBinNm}";
       /* public string TextHS => Koutu_KoukCdSeq == 0 && Bin_BinCdSeq != 0 ? "" : $"({CodeKb_CodeKbnNm})　{Koutu_KoukCd.ToString("D5")}：{Koutu_RyakuNm}　" +
            $"{Bin_BinCd.ToString("D5")??""} ：{Bin_BinNm??""}";*/
        public string TextHSItem => BinBinCdSeq==0  ? KoutuKoukCdSeq==0 ? string.Empty:$"({BUNRUICodeKbnNm})　{KoutuKoukCd.ToString("D5")}：{KoutuRyakuNm}" :
            $"({BUNRUICodeKbnNm})　{KoutuKoukCd.ToString("D5")}：{KoutuRyakuNm} {BinBinCd.ToString("D5")} ：{BinBinNm}";


    }

    public class CodeTypeData
    {
        public int TenantCdSeq { get; set; } = -1;
        public long CodeKbnSeq { get; set; } = -1;
        public string CodeSyu { get; set; }
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string RyakuNm { get; set; }
        public string CodeSeiKbn { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string Text => CodeKbnSeq == -1 ? "" : $"{CodeKbn} : {RyakuNm}";

        public CodeTypeData()
        {

        }

        public CodeTypeData(VpmCodeKb VpmCodeKb)
        {
            this.CodeKbnSeq = Convert.ToInt32(VpmCodeKb.CodeKbnSeq);
            this.CodeSyu = VpmCodeKb.CodeSyu;
            this.CodeKbn = VpmCodeKb.CodeKbn;
            this.CodeKbnNm = VpmCodeKb.CodeKbnNm;
            this.RyakuNm = VpmCodeKb.RyakuNm;
            this.CodeSeiKbn = VpmCodeKb.CodeSeiKbn;
            this.SiyoKbn = VpmCodeKb.SiyoKbn;
            this.UpdYmd = VpmCodeKb.UpdYmd;
            this.UpdTime = VpmCodeKb.UpdTime;
            this.UpdSyainCd = VpmCodeKb.UpdSyainCd;
            this.UpdPrgId = VpmCodeKb.UpdPrgId;
        }
    }

    public class TPM_CodeKbDataReport
    {
        public int CodeKb_CodeKbnSeq { get; set; }
        public string CodeKb_CodeSyu { get; set; }
        public string CodeKb_CodeKbn { get; set; }
        public string CodeKb_CodeKbnNm { get; set; }
        public string Text => $"{CodeKb_CodeKbn}：{CodeKb_CodeKbnNm}";
    }
}
