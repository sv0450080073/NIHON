using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BookingTypeData
    {
        public BookingTypeData(VpmYoyKbn vpmYoyKbn)
        {
            YoyaKbnSeq = vpmYoyKbn.YoyaKbnSeq;
            YoyaKbn = vpmYoyKbn.YoyaKbn;
            YoyaKbnNm = vpmYoyKbn.YoyaKbnNm;
            Yoyagamsyu = vpmYoyKbn.Yoyagamsyu;
            UnsouKbn = vpmYoyKbn.UnsouKbn;
            SiyoKbn = vpmYoyKbn.SiyoKbn;
            UpdYmd = vpmYoyKbn.UpdYmd;
            UpdTime = vpmYoyKbn.UpdTime;
            UpdSyainCd = vpmYoyKbn.UpdSyainCd;
            UpdPrgId = vpmYoyKbn.UpdPrgId;
            TenantCdSeq = vpmYoyKbn.TenantCdSeq;
        }
        public BookingTypeData()
        {

        }

        public BookingTypeData(ReservationData r)
        {
            YoyaKbnSeq = r.YoyaKbnSeq;
            YoyaKbn = (byte)r.YoyaKbn;
            YoyaKbnNm = r.YoyaKbnNm;
            PriorityNum = r.PriorityNum;
            TenantCdSeq = r.TenantCdSeq;
        }

        public int TenantCdSeq { get; set; }
        public int YoyaKbnSeq { get; set; } = -1;
        public byte YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public byte Yoyagamsyu { get; set; }
        public byte UnsouKbn { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
        public string Text
        {
            get
            {
                if (YoyaKbnSeq == -1) { return ""; }
                else { return string.Format("{0} : {1}", YoyaKbn, YoyaKbnNm); }
            }
        }
        public string PriorityNum { get; set; }
    }
}
