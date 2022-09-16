using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.CommonComponents
{
    public class ReservationClassComponentData
    {
        public int YoyaKbnSeq { get; set; }
        public byte YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public string Text => $"{YoyaKbn:00} : {YoyaKbnNm}";
        public bool IsSelect { get; set; }
    }
}
