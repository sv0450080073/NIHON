using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TPM_SyaRyoData
    {
        public bool IsSelectedAll { get; set; } = false;
        public int SyaRyoCdSeq { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public int NinkaKbn { get; set; }
        public string VehicleInfo
        {
            get
            {
                if (IsSelectedAll || SyaRyoCdSeq == 0)
                    return string.Empty;
                return $"{SyaRyoCd.ToString("D5")}：{SyaRyoNm}";
            }
        }
    }
}
