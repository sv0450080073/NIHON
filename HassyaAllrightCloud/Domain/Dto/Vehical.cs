using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class Vehical
    {
        public string Ukeno { get; set; } = string.Empty;
        public int EigyoCdSeq { get; set; }
        public int KataKbn { get; set; }
        public VpmSyaRyo VehicleModel { get; set; }
    }
}
