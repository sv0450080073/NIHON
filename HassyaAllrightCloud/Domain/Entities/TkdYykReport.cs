﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Entities
{
    public partial class TkdYykReport
    {
        /// <summary>
        /// 受付番号
        /// </summary>
        public string UkeNo { get; set; }
        /// <summary>
        /// 運行日連番
        /// </summary>
        public short UnkRen { get; set; }
        /// <summary>
        /// 車種連番
        /// </summary>
        public short SyaSyuRen { get; set; }
        /// <summary>
        /// 変更回数
        /// </summary>
        public short HenKai { get; set; }
        /// <summary>
        /// 総走行時間
        /// </summary>
        public string AllSokoTime { get; set; }
        /// <summary>
        /// 点検時間
        /// </summary>
        public string CheckTime { get; set; }
        /// <summary>
        /// 調整時間
        /// </summary>
        public string AdjustTime { get; set; }
        /// <summary>
        /// 深夜早朝時間
        /// </summary>
        public string ShinSoTime { get; set; }
        /// <summary>
        /// 総走行キロ
        /// </summary>
        public decimal AllSokoKm { get; set; }
        /// <summary>
        /// 実車時間
        /// </summary>
        public string JiSaTime { get; set; }
        /// <summary>
        /// 実車キロ
        /// </summary>
        public decimal JiSaKm { get; set; }
        /// <summary>
        /// 割引区分
        /// </summary>
        public byte WaribikiKbn { get; set; }
        public byte ChangeFlg { get; set; }
        public string ChangeKoskTime { get; set; }
        public string ChangeShinTime { get; set; }
        public decimal ChangeSokoKm { get; set; }
        /// <summary>
        /// 特殊車両料金フラグ
        /// </summary>
        public byte SpecialFlg { get; set; }
        /// <summary>
        /// 最終更新年月日
        /// </summary>
        public string UpdYmd { get; set; }
        /// <summary>
        /// 最終更新時間
        /// </summary>
        public string UpdTime { get; set; }
        /// <summary>
        /// 最終更新社員コードＳＥＱ
        /// </summary>
        public int UpdSyainCd { get; set; }
        /// <summary>
        /// 最終更新プログラムＩＤ
        /// </summary>
        public string UpdPrgId { get; set; }
        public int? UnsoSyasyuUnt { get; set; }
        public int? UnsoSyasyuRyokin { get; set; }
    }
}