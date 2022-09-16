using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BookingInputData
{
    public class HaishaInfoData
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public short TeidanNo { get; set; }
        public bool IsSelect { get; set; }
        public List<int> YouTblSeqList { get; set; }

        public string GoSya { get; set; }
        public string HaiSYmd { get; set; }
        public string TouYmd { get; set; }
        public string SyaSyuNm { get; set; }
        public string BranchName { get; set; }
        public string BorrowBranch { get; set; }
        public string SyaRyoNm { get; set; }
        public string Tsukomi { get; set; }
        public string Futai { get; set; }

        public bool IsBorrow { get => YouTblSeqList.Any(y => y != 0); }
        public string HasBorrow { get => YouTblSeqList.Any(y => y != 0) ? "〇" : ""; }

        public string GetStartEnd(string cultureinfo)
        {
            string result = "";
            DateTime start;
            if (DateTime.TryParseExact(HaiSYmd, "yyyymmdd", null, System.Globalization.DateTimeStyles.None, out start))
            {
                result = start.ToString("yyyy/MM/dd(ddd)", new CultureInfo(cultureinfo));
            }
            DateTime end;
            if (DateTime.TryParseExact(TouYmd, "yyyymmdd", null, System.Globalization.DateTimeStyles.None, out end))
            {
                result += " ~ " + end.ToString("yyyy/MM/dd(ddd)", new CultureInfo(cultureinfo));
            }
            return result;
        }
    }
}
