﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Entities
{
    public partial class VpmAlert
    {
        public int TenantCdSeq { get; set; }
        public int AlertCdSeq { get; set; }
        public short AlertKbn { get; set; }
        public int AlertCd { get; set; }
        public string AlertNm { get; set; }
        public string AlertContent { get; set; }
        public int DefaultVal { get; set; }
        public byte DefaultTimeline { get; set; }
        public byte DefaultZengo { get; set; }
        public byte DefaultDisplayKbn { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
    }
}