using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusType
    {
        public int SyaSyuCdSeq { get; set; }
        public byte KataKbn { get; set; }
        public string KataKbnNm { get; set; }
        public string SyaSyuNm { get; set; }
        public string DisplayName => SyaSyuCdSeq > 0 ? $"{KataKbnNm} : {SyaSyuNm}" : "";
    }

    public class BusDetail
    {
        public string BusTypeNameDetail { get; set; }
        public string Status { get; set; }
        public bool isAvailable { get; set; }
        public string SyaRyoCdSeq { get; set; }
    }

    public class GroupBusInfo
    {
        public string BusTypeName { get; set; }
        public string UnUseBusCount { get; set; }
        public string InUseBusCount { get; set; }
        public int BusCount { get; set; }
        public List<BusDetail> BusDetails { get; set; }
    }

    public class FormSearch
    {
        public DateTime SelectedDate { get; set; }
        public BusType BusType { get; set; }
    }

    public class BusAllocationDatas
    {
        public IEnumerable<int> BusAllocationsSeqs { get; set; }
        public IEnumerable<BusData> BusDatas { get; set; }
        public DateTime DateSelected { get; set; }
    }
}
