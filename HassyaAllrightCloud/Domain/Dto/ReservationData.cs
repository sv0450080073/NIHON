using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReservationData
    {
        /// <summary>
        /// Mark this object is selected all item. Default values is <c>false</c>
        /// </summary>
        public bool IsSelectedAll { get; set; } = false;
        public int TenantCdSeq { get; set; }
        public int YoyaKbnSeq { get; set; }
        public int YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public string PriorityNum { get; set; } = "0";
        public string Text => YoyaKbnSeq == 0 ? "すべて" : $"{YoyaKbnNm}";

        public string BookingTypeName
        {
            get
            {
                if (IsSelectedAll || YoyaKbnSeq == 0)
                    return Constants.SelectedAll;
                return YoyaKbnNm;
            }
        }

        public string BookingTypeInfo
        {
            get
            {
                if (IsSelectedAll || YoyaKbnSeq == 0)
                    return Constants.SelectedAll;
                return $"{YoyaKbn.ToString("D2")}：{YoyaKbnNm}"; ;
            }
        }
        public string YoyaCodeName
        {
            get
            {
                if (YoyaKbnSeq != 0)
                {
                    return $"{YoyaKbn +" : "+ YoyaKbnNm}";
                }
                return string.Empty;
            }
        }
    }
}
