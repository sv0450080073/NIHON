using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CustomerClassification
    {
        public int TenantCdSeq { get; set; }
        public int JyoKyakuCdSeq { get; set; } = -1;
        public byte JyoKyakuCd { get; set; }
        public string JyoKyakuNm { get; set; }
        public string RyakuNm { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string Text => JyoKyakuCdSeq == -1 ? "" : $"{JyoKyakuCd} : {RyakuNm}";

        public CustomerClassification()
        {

        }

        public CustomerClassification(VpmJyoKya VpmJyoKya)
        {
            //TODO DantaiCdSeq need to declare again
            //this.DantaiCdSeq = VpmJyoKya.DantaiCdSeq;
            this.JyoKyakuCdSeq = VpmJyoKya.JyoKyakuCdSeq;
            this.JyoKyakuCd = VpmJyoKya.JyoKyakuCd;
            this.JyoKyakuNm = VpmJyoKya.JyoKyakuNm;
            this.RyakuNm = VpmJyoKya.RyakuNm;
            this.SiyoKbn = VpmJyoKya.SiyoKbn;
            this.UpdYmd = VpmJyoKya.UpdYmd;
            this.UpdTime = VpmJyoKya.UpdTime;
            this.UpdSyainCd = VpmJyoKya.UpdSyainCd;
            this.UpdPrgId = VpmJyoKya.UpdPrgId;
        }
    }
}
