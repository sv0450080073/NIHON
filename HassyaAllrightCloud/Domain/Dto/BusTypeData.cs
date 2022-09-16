using HassyaAllrightCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusTypeData
    {
        public string Katakbn { get; set; } = "-1";
        public string RyakuNm { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public int SyaSyuCd { get; set; }
        public string SyaSyuNm { get; set; }
        public string Text
        {
            get
            {
                if (Katakbn == "-1") { return ""; }
                else { return String.Format("{0} : {1} {2} : {3}", Katakbn, RyakuNm, SyaSyuCd, SyaSyuNm); }
            }
        }
    }

    public class BusTypesData
    {
        public int SyaSyuCdSeq { get; set; } = -1;
        public short SyaSyuCd { get; set; }
        public byte KataKbn { get; set; }
        public string SyaSyuNm { get; set; }
        public byte NenryoKbn { get; set; }
        public byte SoubiKbn { get; set; }
        public byte TokusyuKbn { get; set; }
        public string SyaSyuKigo { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string Text => SyaSyuCdSeq == -1 ? "" : $"{SyaSyuCd} : {SyaSyuNm}";

        public BusTypesData()
        {

        }

        public BusTypesData(VpmSyaSyu vpmSyaSyu)
        {
            SyaSyuCdSeq = vpmSyaSyu.SyaSyuCdSeq;
            SyaSyuCd = vpmSyaSyu.SyaSyuCd;
            KataKbn = vpmSyaSyu.KataKbn;
            SyaSyuNm = vpmSyaSyu.SyaSyuNm;
            NenryoKbn = vpmSyaSyu.NenryoKbn;
            SoubiKbn = vpmSyaSyu.SoubiKbn;
            TokusyuKbn = vpmSyaSyu.TokusyuKbn;
            SyaSyuKigo = vpmSyaSyu.SyaSyuKigo;
            SiyoKbn = vpmSyaSyu.SiyoKbn;
            UpdYmd = vpmSyaSyu.UpdYmd;
            UpdTime = vpmSyaSyu.UpdTime;
            UpdSyainCd = vpmSyaSyu.UpdSyainCd;
            UpdPrgId = vpmSyaSyu.UpdPrgId;
        }
    }
}
