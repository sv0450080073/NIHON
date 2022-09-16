using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class WorkHolidayTypeDataModel
    {
        public byte KinKyuKbn { get; set; }
        public string KinKyuKbnNm { get; set; }
        public int KinKyuCdSeq { get; set; }
        public short KinKyuCd { get; set; }
        public string KinKyuNm { get; set; }
        public string Text => $"{KinKyuCd.ToString("D3")} : {KinKyuNm}　{KinKyuKbn.ToString("D1")} : {KinKyuKbnNm}";
    }
}
