namespace HassyaAllrightCloud.Domain.Dto
{
    public class TPM_SyokumData
    {
        public int Syokum_SyokumuCdSeq { get; set; }
        public int Syokum_SyokumuCd { get; set; }
        public string Syokum_SyokumuNm { get; set; } = "";
        public int KyoSHe_SyainCdSeq { get; set; }
        public string Syain_SyainCd { get; set; } = "";
        public string Syain_SyainNm { get; set; } = "";
        public int Eigyos_EigyoCdSeq { get; set; }
        public string Eigyos_RyakuNm { get; set; } = "";
        public bool isView { get; set; } = true;
        public string Text => KyoSHe_SyainCdSeq != 0 ? $"{Eigyos_EigyoCdSeq}：{Eigyos_RyakuNm}　{Syain_SyainCd}：{Syain_SyainNm}　{(Syokum_SyokumuNm)}" : string.Empty;
        public string TextBusAllocation => KyoSHe_SyainCdSeq != 0 ? $"{Eigyos_EigyoCdSeq.ToString("D5")}：{Eigyos_RyakuNm}　{Syain_SyainCd}：{Syain_SyainNm}　{(Syokum_SyokumuNm)}" : string.Empty;
        public byte SyokumuKbn { get; set; }
    }
}
