using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;
using System.Linq;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class YoyKbnHelper
    {
        public static List<ReservationData> GetListYoyKbnFromTo(ReservationData from, ReservationData to, List<ReservationData> source)
        {
            // select all
            if (from is null && to is null)
            {
                return source;
            }
            // select all-to
            if (from is null)
            {
                return source.Where(r => string.Compare(r.PriorityNum, to.PriorityNum) <= 0).ToList();
            }
            // select from-all
            if (to is null)
            {
                return source.Where(r => string.Compare(r.PriorityNum, from.PriorityNum) >= 0).ToList();
            }
            // from-to
            return source.Where(r => string.Compare(r.PriorityNum, from.PriorityNum) >= 0 && string.Compare(r.PriorityNum, to.PriorityNum) <= 0).ToList();
        }
        public static List<int> GetListYoyKbnFromToint(ReservationData from, ReservationData to, List<ReservationData> source)
        {
            // select all
            if (from is null && to is null)
            {
                return source.Select(t=>t.YoyaKbnSeq).ToList();
            }
            // select all-to
            if (from is null)
            {
                return source.Where(r => string.Compare(r.PriorityNum, to.PriorityNum) <= 0).Select(t=>t.YoyaKbnSeq).ToList();
            }
            // select from-all
            if (to is null)
            {
                return source.Where(r => string.Compare(r.PriorityNum, from.PriorityNum) >= 0).Select(t=>t.YoyaKbnSeq).ToList();
            }
            // from-to
            return source.Where(r => string.Compare(r.PriorityNum, from.PriorityNum) >= 0 && string.Compare(r.PriorityNum, to.PriorityNum) <= 0).Select(t=>t.YoyaKbnSeq).ToList();
        }
    }
}