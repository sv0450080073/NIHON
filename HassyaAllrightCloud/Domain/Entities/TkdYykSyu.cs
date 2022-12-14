// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Entities
{
    public partial class TkdYykSyu
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public short HenKai { get; set; }
        public int SyaSyuCdSeq { get; set; }
        public byte KataKbn { get; set; }
        public short SyaSyuDai { get; set; }
        public int SyaSyuTan { get; set; }
        /// <summary>
        /// 運賃（バス代）
        /// </summary>
        public int SyaRyoUnc { get; set; }
        /// <summary>
        /// 運転手人数
        /// </summary>
        public byte DriverNum { get; set; }
        /// <summary>
        /// 運賃単価
        /// </summary>
        public int UnitBusPrice { get; set; }
        /// <summary>
        /// 料金単価
        /// </summary>
        public int UnitBusFee { get; set; }
        /// <summary>
        /// ガイド人数
        /// </summary>
        public byte GuiderNum { get; set; }
        /// <summary>
        /// ガイド単価
        /// </summary>
        public int UnitGuiderPrice { get; set; }
        /// <summary>
        /// ガイド料金
        /// </summary>
        public int UnitGuiderFee { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
    }
}