using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class HyperFormData
    {
        // 配車日
        public DateTime? HaishaBiFrom { get; set; }
        public DateTime? HaishaBiTo { get; set; }

        // 到着日
        public DateTime? TochakuBiFrom { get; set; }
        public DateTime? TochakuBiTo { get; set; }

        // 予約日
        public DateTime? YoyakuBiFrom { get; set; }
        public DateTime? YoyakuBiTo { get; set; }

        // 受付番号
        public string UketsukeBangoFrom { get; set; }
        public string UketsukeBangoTo { get; set; }

        // 予約区分
        // New
        public ReservationClassComponentData YoyakuFrom { get; set; }
        public ReservationClassComponentData YoyakuTo { get; set; }

        // 営業担当
        public StaffsData EigyoTantoShaFrom { get; set; }
        public StaffsData EigyoTantoShaTo { get; set; }

        // 受付営業所
        public SaleBranchData UketsukeEigyoJoFrom { get; set; }
        public SaleBranchData UketsukeEigyoJoTo { get; set; }

        // 入力担当
        public StaffsData NyuryokuTantoShaFrom { get; set; }
        public StaffsData NyuryokuTantoShaTo { get; set; }

        // 得意先

        // New
        public CustomerComponentGyosyaData GyosyaTokuiSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaTokuiSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiTo { get; set; }

        // 仕入先

        // New
        public CustomerComponentGyosyaData GyosyaShiireSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaShiireSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiTo { get; set; }

        // 団体区分
        public CodeTypeData DantaiKbnFrom { get; set; }
        public CodeTypeData DantaiKbnTo { get; set; }

        // 客種区分
        public CustomerClassification KyakuDaneKbnFrom { get; set; }
        public CustomerClassification KyakuDaneKbnTo { get; set; }

        // 行先
        public LoadLocation YukiSakiFrom { get; set; }
        public LoadLocation YukiSakiTo { get; set; }

        // 配車地
        public LoadDispatchArea HaishaChiFrom { get; set; }
        public LoadDispatchArea HaishaChiTo { get; set; }

        // 発生地
        public LoadLocation HasseiChiFrom { get; set; }
        public LoadLocation HasseiChiTo { get; set; }

        // エリア
        public LoadLocation AreaFrom { get; set; }
        public LoadLocation AreaTo { get; set; }

        // 車種
        public BusTypesData ShashuFrom { get; set; }
        public BusTypesData ShashuTo { get; set; }

        // 車種単価
        public string ShashuTankaFrom { get; set; }
        public string ShashuTankaTo { get; set; }

        // 受付条件
        public VpmCodeKb UketsukeJokenFrom { get; set; }
        public VpmCodeKb UketsukeJokenTo { get; set; }

        // 帳票設定
        public OutputReportType OutputType { get; set; } = OutputReportType.Preview;
        public ComboboxFixField PageSize { get; set; } = new ComboboxFixField { IdValue = 1, StringValue = "A4" };
        public ComboboxFixField ActiveHeaderOption { get; set; } = ShowHeaderOptions.ShowHeaderOptionData[0];
        public ComboboxFixField GroupType { get; set; } = GroupTypes.GroupTypeData[0];
        public ComboboxFixField DelimiterType { get; set; } = DelimiterTypes.DelimiterTypeData[2];
        public int ActiveV { get; set; } = (int)ViewMode.Medium;
        public int dateType { get; set; } = (int)DateType.Dispatch;
        public ComboboxFixField MaxMinSetting { get; set; } = null;
        public ComboboxFixField ReservationStatus { get; set; } = null;
        public int Type { get; set; } = 1;

        // 予約ST
        // public VpmYoyKbn YoyakuSTFrom { get; set; }
        // public VpmYoyKbn YoyakuSTTo { get; set; }

        // 団体名
        public string DantaiNm { get; set; } // P.M.Nhat add new prop 2020/09/04

        public HyperFormData()
        {
        }
        public HyperFormData(HyperFormData hyperFormData)
        {
            HaishaBiFrom = hyperFormData.HaishaBiFrom;
            HaishaBiTo = hyperFormData.HaishaBiTo;
            TochakuBiFrom = hyperFormData.TochakuBiFrom;
            TochakuBiTo = hyperFormData.TochakuBiTo;
            YoyakuBiFrom = hyperFormData.YoyakuBiFrom;
            YoyakuBiTo = hyperFormData.YoyakuBiTo;
            UketsukeBangoFrom = hyperFormData.UketsukeBangoFrom;
            UketsukeBangoTo = hyperFormData.UketsukeBangoTo;
            YoyakuFrom = hyperFormData.YoyakuFrom;
            YoyakuTo = hyperFormData.YoyakuTo;
            EigyoTantoShaFrom = hyperFormData.EigyoTantoShaFrom;
            EigyoTantoShaTo = hyperFormData.EigyoTantoShaTo;
            UketsukeEigyoJoFrom = hyperFormData.UketsukeEigyoJoFrom;
            UketsukeEigyoJoTo = hyperFormData.UketsukeEigyoJoTo;
            NyuryokuTantoShaFrom = hyperFormData.NyuryokuTantoShaFrom;
            NyuryokuTantoShaTo = hyperFormData.NyuryokuTantoShaTo;
            GyosyaTokuiSakiFrom = hyperFormData.GyosyaTokuiSakiFrom;
            GyosyaTokuiSakiTo = hyperFormData.GyosyaTokuiSakiTo;
            TokiskTokuiSakiFrom = hyperFormData.TokiskTokuiSakiFrom;
            TokiskTokuiSakiTo = hyperFormData.TokiskTokuiSakiTo;
            TokiStTokuiSakiFrom = hyperFormData.TokiStTokuiSakiFrom;
            TokiStTokuiSakiTo = hyperFormData.TokiStTokuiSakiTo;
            GyosyaShiireSakiFrom = hyperFormData.GyosyaShiireSakiFrom;
            GyosyaShiireSakiTo = hyperFormData.GyosyaShiireSakiTo;
            TokiskShiireSakiFrom = hyperFormData.TokiskShiireSakiFrom;
            TokiskShiireSakiTo = hyperFormData.TokiskShiireSakiTo;
            TokiStShiireSakiFrom = hyperFormData.TokiStShiireSakiFrom;
            TokiStShiireSakiTo = hyperFormData.TokiStShiireSakiTo;
            DantaiKbnFrom = hyperFormData.DantaiKbnFrom;
            DantaiKbnTo = hyperFormData.DantaiKbnTo;
            KyakuDaneKbnFrom = hyperFormData.KyakuDaneKbnFrom;
            KyakuDaneKbnTo = hyperFormData.KyakuDaneKbnTo;
            YukiSakiFrom = hyperFormData.YukiSakiFrom;
            YukiSakiTo = hyperFormData.YukiSakiTo;
            HaishaChiFrom = hyperFormData.HaishaChiFrom;
            HaishaChiTo = hyperFormData.HaishaChiTo;
            HasseiChiFrom = hyperFormData.HasseiChiFrom;
            HasseiChiTo = hyperFormData.HasseiChiTo;
            AreaFrom = hyperFormData.AreaFrom;
            AreaTo = hyperFormData.AreaTo;
            ShashuFrom = hyperFormData.ShashuFrom;
            ShashuTo = hyperFormData.ShashuTo;
            ShashuTankaFrom = hyperFormData.ShashuTankaFrom;
            ShashuTankaTo = hyperFormData.ShashuTankaTo;
            UketsukeJokenFrom = hyperFormData.UketsukeJokenFrom;
            UketsukeJokenTo = hyperFormData.UketsukeJokenTo;
            // YoyakuSTFrom = hyperFormData.YoyakuSTFrom;
            // YoyakuSTTo = hyperFormData.YoyakuSTTo;
            DantaiNm = hyperFormData.DantaiNm; // P.M.Nhat add new prop 2020/09/04
            OutputType = hyperFormData.OutputType;
            PageSize = hyperFormData.PageSize;
            ActiveHeaderOption = hyperFormData.ActiveHeaderOption;
            GroupType = hyperFormData.GroupType;
            DelimiterType = hyperFormData.DelimiterType;
            ActiveV = hyperFormData.ActiveV;
            dateType = hyperFormData.dateType;
            MaxMinSetting = hyperFormData.MaxMinSetting;
            ReservationStatus = hyperFormData.ReservationStatus;
            Type = hyperFormData.Type;
        }

        public void Reinit()
        {
            this.HaishaBiFrom = null;
            this.HaishaBiTo = null;
            this.TochakuBiFrom = null;
            this.TochakuBiTo = null;
            this.YoyakuBiFrom = null;
            this.YoyakuBiTo = null;
            this.UketsukeBangoFrom = null;
            this.UketsukeBangoTo = null;
            this.YoyakuFrom = null;
            this.YoyakuTo = null;
            this.EigyoTantoShaFrom = null;
            this.EigyoTantoShaTo = null;
            this.UketsukeEigyoJoFrom = null;
            this.UketsukeEigyoJoTo = null;
            this.NyuryokuTantoShaFrom = null;
            this.NyuryokuTantoShaTo = null;
            this.GyosyaTokuiSakiFrom = null;
            this.GyosyaTokuiSakiTo = null;
            this.TokiskTokuiSakiFrom = null;
            this.TokiskTokuiSakiTo = null;
            this.TokiStTokuiSakiFrom = null;
            this.TokiStTokuiSakiTo = null;
            this.GyosyaShiireSakiFrom = null;
            this.GyosyaShiireSakiTo = null;
            this.TokiskShiireSakiFrom = null;
            this.TokiskShiireSakiTo = null;
            this.TokiStShiireSakiFrom = null;
            this.TokiStShiireSakiTo = null;
            this.DantaiKbnFrom = null;
            this.DantaiKbnTo = null;
            this.KyakuDaneKbnFrom = null;
            this.KyakuDaneKbnTo = null;
            this.YukiSakiFrom = null;
            this.YukiSakiTo = null;
            this.HaishaChiFrom = null;
            this.HaishaChiTo = null;
            this.HasseiChiFrom = null;
            this.HasseiChiTo = null;
            this.AreaFrom = null;
            this.AreaTo = null;
            this.ShashuFrom = null;
            this.ShashuTo = null;
            this.ShashuTankaFrom = null;
            this.ShashuTankaTo = null;
            this.UketsukeJokenFrom = null;
            this.UketsukeJokenTo = null;
            this.DantaiNm = null;
            this.OutputType = OutputReportType.Preview;
            this.PageSize = new ComboboxFixField { IdValue = 1, StringValue = "A4" };
            this.ActiveHeaderOption = ShowHeaderOptions.ShowHeaderOptionData[0];
            this.GroupType = GroupTypes.GroupTypeData[0];
            this.DelimiterType = DelimiterTypes.DelimiterTypeData[2];
            this.ActiveV = (int)ViewMode.Medium;
            this.dateType = (int)DateType.Dispatch;
            this.MaxMinSetting = null;
            this.ReservationStatus = null;
            this.Type = 1;
        }
        public string ToQueryString()
        {
            List<string> Result = new List<string>();
            foreach (PropertyInfo Property in this.GetType().GetProperties())
            {
                if (Property.GetValue(this, null) != null)
                {
                    switch (Property.Name)
                    {
                        case nameof(this.HaishaBiFrom):
                        case nameof(this.HaishaBiTo):
                        case nameof(this.TochakuBiFrom):
                        case nameof(this.TochakuBiTo):
                        case nameof(this.YoyakuBiFrom):
                        case nameof(this.YoyakuBiTo):
                            Result.Add(Property.Name + "=" + ((DateTime)Property.GetValue(this, null)).ToString("yyyyMMdd"));
                            break;
                        case nameof(this.UketsukeBangoFrom):
                        case nameof(this.UketsukeBangoTo):
                        case nameof(this.ShashuTankaFrom):
                        case nameof(this.ShashuTankaTo):
                            Result.Add(Property.Name + "=" + (string)Property.GetValue(this, null));
                            break;
                        case nameof(this.YoyakuFrom):
                        case nameof(this.YoyakuTo):
                            Result.Add(Property.Name + "=" + ((ReservationClassComponentData)Property.GetValue(this, null)).YoyaKbn);
                            break;
                        case nameof(this.EigyoTantoShaFrom):
                        case nameof(this.EigyoTantoShaTo):
                        case nameof(this.NyuryokuTantoShaFrom):
                        case nameof(this.NyuryokuTantoShaTo):
                            Result.Add(Property.Name + "=" + ((StaffsData)Property.GetValue(this, null)).SyainCd);
                            break;
                        case nameof(this.UketsukeEigyoJoFrom):
                        case nameof(this.UketsukeEigyoJoTo):
                            Result.Add(Property.Name + "=" + ((SaleBranchData)Property.GetValue(this, null)).EigyoCd);
                            break;
                        case nameof(this.GyosyaTokuiSakiFrom):
                        case nameof(this.GyosyaTokuiSakiTo):
                        case nameof(this.GyosyaShiireSakiFrom):
                        case nameof(this.GyosyaShiireSakiTo):
                            CustomerComponentGyosyaData Gyosya = (CustomerComponentGyosyaData)Property.GetValue(this, null);
                            Result.Add(Property.Name + "=" + Gyosya.GyosyaCd.ToString());
                            break;
                        case nameof(this.TokiskTokuiSakiFrom):
                        case nameof(this.TokiskTokuiSakiTo):
                        case nameof(this.TokiskShiireSakiFrom):
                        case nameof(this.TokiskShiireSakiTo):
                            CustomerComponentTokiskData Tokisk = (CustomerComponentTokiskData)Property.GetValue(this, null);
                            Result.Add(Property.Name + "=" + Tokisk.TokuiCd.ToString());
                            break;
                        case nameof(this.TokiStTokuiSakiFrom):
                        case nameof(this.TokiStTokuiSakiTo):
                        case nameof(this.TokiStShiireSakiFrom):
                        case nameof(this.TokiStShiireSakiTo):
                            CustomerComponentTokiStData TokiSt = (CustomerComponentTokiStData)Property.GetValue(this, null);
                            Result.Add(Property.Name + "=" + TokiSt.SitenCd.ToString());
                            break;
                        case nameof(this.DantaiKbnFrom):
                        case nameof(this.DantaiKbnTo):
                            Result.Add(Property.Name + "=" + ((CodeTypeData)Property.GetValue(this, null)).CodeKbn);
                            break;
                        case nameof(this.KyakuDaneKbnFrom):
                        case nameof(this.KyakuDaneKbnTo):
                            Result.Add(Property.Name + "=" + ((CustomerClassification)Property.GetValue(this, null)).JyoKyakuCd);
                            break;
                        case nameof(this.YukiSakiFrom):
                        case nameof(this.YukiSakiTo):
                        case nameof(this.HasseiChiFrom):
                        case nameof(this.HasseiChiTo):
                        case nameof(this.AreaFrom):
                        case nameof(this.AreaTo):
                            LoadLocation Location = (LoadLocation)Property.GetValue(this, null);
                            Result.Add(Property.Name + "=" + $"{Location.CodeKbn:00}{Location.BasyoMapCd}");
                            break;
                        case nameof(this.HaishaChiFrom):
                        case nameof(this.HaishaChiTo):
                            LoadDispatchArea DispatchArea = (LoadDispatchArea)Property.GetValue(this, null);
                            Result.Add(Property.Name + "=" + $"{DispatchArea.CodeKbn:00}{DispatchArea.HaiSCd}");
                            break;
                        case nameof(this.ShashuFrom):
                        case nameof(this.ShashuTo):
                            Result.Add(Property.Name + "=" + ((BusTypesData)Property.GetValue(this, null)).SyaSyuCd);
                            break;
                        case nameof(this.UketsukeJokenFrom):
                        case nameof(this.UketsukeJokenTo):
                            Result.Add(Property.Name + "=" + ((VpmCodeKb)Property.GetValue(this, null)).CodeKbn);
                            break;
                        // P.M.Nhat add new prop 2020/09/04
                        case nameof(this.DantaiNm):
                            Result.Add(Property.Name + "=" + Property.GetValue(this, null));
                            break;
                        case nameof(this.MaxMinSetting):
                        case nameof(this.ReservationStatus):
                            Result.Add(Property.Name + "=" + ((ComboboxFixField)Property.GetValue(this, null)).IdValue);
                            break;
                        default:
                            break;
                    }
                }
            }
            return string.Join("&", Result.ToArray());
        }

        static public HyperFormData ToObject(string HaishaBiFrom, string HaishaBiTo, string TochakuBiFrom, string TochakuBiTo, string YoyakuBiFrom, string YoyakuBiTo, string UketsukeBangoFrom, string UketsukeBangoTo,
            string YoyakuFrom, string YoyakuTo, List<ReservationClassComponentData> ListReservationClass,
            string EigyoTantoShaFrom, string EigyoTantoShaTo, List<StaffsData> StaffList,
            string UketsukeEigyoJoFrom, string UketsukeEigyoJoTo, List<SaleBranchData> SaleBranchList,
            string NyuryokuTantoShaFrom, string NyuryokuTantoShaTo,
            string GyosyaTokuiSakiFrom, string GyosyaTokuiSakiTo, List<CustomerComponentGyosyaData> ListGyosya,
            string TokiskTokuiSakiFrom, string TokiskTokuiSakiTo, List<CustomerComponentTokiskData> ListTokisk,
             string TokiStTokuiSakiFrom, string TokiStTokuiSakiTo, List<CustomerComponentTokiStData> ListTokiSt,
            string GyosyaShiireSakiFrom, string GyosyaShiireSakiTo,
             string TokiskShiireSakiFrom, string TokiskShiireSakiTo,
            string TokiStShiireSakiFrom, string TokiStShiireSakiTo,
            string DantaiKbnFrom, string DantaiKbnTo, List<CodeTypeData> CodeKbList,
            string KyakuDaneKbnFrom, string KyakuDaneKbnTo, List<CustomerClassification> CustomerClassificationList,
            string YukiSakiFrom, string YukiSakiTo, List<LoadLocation> DestinationList,
            string HaishaChiFrom, string HaishaChiTo, List<LoadDispatchArea> DispatchList,
            string HasseiChiFrom, string HasseiChiTo, List<LoadLocation> OriginList,
            string AreaFrom, string AreaTo, List<LoadLocation> AreaList,
            string ShashuFrom, string ShashuTo, List<BusTypesData> BusTypeList,
            string ShashuTankaFrom, string ShashuTankaTo,
            string UketsukeJokenFrom, string UketsukeJokenTo, List<VpmCodeKb> ConditionList,
            string DantaiNm,
            string MaxMinSetting, List<ComboboxFixField> MaxMinSettingList,
            string ReservationStatus, List<ComboboxFixField> ReservationStatusList)
        {
            try
            {
                HyperFormData Result = new HyperFormData();
                List<int> Datetypes = new List<int> {};
                // 配車日
                if (HaishaBiFrom != null)
                {
                    Result.HaishaBiFrom = DateTime.ParseExact(HaishaBiFrom, "yyyyMMdd", null);
                    Datetypes.Add((int)DateType.Dispatch);
                }
                if (HaishaBiTo != null)
                {
                    Result.HaishaBiTo = DateTime.ParseExact(HaishaBiTo, "yyyyMMdd", null);
                    Datetypes.Add((int)DateType.Dispatch);
                }

                // 到着日
                if (TochakuBiFrom != null)
                {
                    Result.TochakuBiFrom = DateTime.ParseExact(TochakuBiFrom, "yyyyMMdd", null);
                    Datetypes.Add((int)DateType.Arrival);
                }
                if (TochakuBiTo != null)
                {
                    Result.TochakuBiTo = DateTime.ParseExact(TochakuBiTo, "yyyyMMdd", null);
                    Datetypes.Add((int)DateType.Arrival);
                }

                // 予約日
                if (YoyakuBiFrom != null)
                {
                    Result.YoyakuBiFrom = DateTime.ParseExact(YoyakuBiFrom, "yyyyMMdd", null);
                    Datetypes.Add((int)DateType.Reservation);
                }
                if (YoyakuBiTo != null)
                {
                    Result.YoyakuBiTo = DateTime.ParseExact(YoyakuBiTo, "yyyyMMdd", null);
                    Datetypes.Add((int)DateType.Reservation);
                }
                Result.dateType = Datetypes.Min();

                // 受付番号
                if (UketsukeBangoFrom != null && UketsukeBangoFrom.Length <= 10 && long.TryParse(UketsukeBangoFrom, out _))
                {
                    Result.UketsukeBangoFrom = UketsukeBangoFrom;
                }
                if (UketsukeBangoTo != null && UketsukeBangoTo.Length <= 10 && long.TryParse(UketsukeBangoTo, out _))
                {
                    Result.UketsukeBangoTo = UketsukeBangoTo;
                }

                // 予約区分
                if (YoyakuFrom != null)
                {
                    Result.YoyakuFrom = ListReservationClass.FirstOrDefault(_ => _.YoyaKbn == int.Parse(YoyakuFrom));
                }
                if (YoyakuTo != null)
                {
                    Result.YoyakuTo = ListReservationClass.FirstOrDefault(_ => _.YoyaKbn == int.Parse(YoyakuTo));
                }

                // 営業担当
                if (EigyoTantoShaFrom != null)
                {
                    Result.EigyoTantoShaFrom = StaffList.Find(x => x != null && x.SyainCd == EigyoTantoShaFrom);
                }
                if (EigyoTantoShaTo != null)
                {
                    Result.EigyoTantoShaTo = StaffList.Find(x => x != null && x.SyainCd == EigyoTantoShaTo);
                }

                // 受付営業所
                if (UketsukeEigyoJoFrom != null)
                {
                    Result.UketsukeEigyoJoFrom = SaleBranchList.Find(x => x != null && x.EigyoCd == int.Parse(UketsukeEigyoJoFrom));
                }
                if (UketsukeEigyoJoTo != null) {
                    Result.UketsukeEigyoJoTo = SaleBranchList.Find(x => x != null && x.EigyoCd == int.Parse(UketsukeEigyoJoTo));
                }

                // 入力担当
                if (NyuryokuTantoShaFrom != null)
                {
                    Result.NyuryokuTantoShaFrom = StaffList.Find(x => x != null && x.SyainCd == NyuryokuTantoShaFrom);
                }
                if (NyuryokuTantoShaTo != null)
                {
                    Result.NyuryokuTantoShaTo = StaffList.Find(x => x != null && x.SyainCd == NyuryokuTantoShaTo);
                }

                // 
                if (GyosyaTokuiSakiFrom != null)
                {
                    Result.GyosyaTokuiSakiFrom  = ListGyosya.FirstOrDefault(_ => _.GyosyaCd == int.Parse(GyosyaTokuiSakiFrom));
                    if (TokiskTokuiSakiFrom != null)
                    {
                        List<CustomerComponentTokiskData> TokiskData = new List<CustomerComponentTokiskData>();
                        List<CustomerComponentTokiStData> TokiStData = new List<CustomerComponentTokiStData>();
                        TokiskData = ListTokisk.Where(_ => _.GyosyaCdSeq == (Result.GyosyaTokuiSakiFrom?.GyosyaCdSeq ?? -1)).ToList();
                        Result.TokiskTokuiSakiFrom = TokiskData.FirstOrDefault(_ => _.TokuiCd == int.Parse(TokiskTokuiSakiFrom));
                        if (TokiStTokuiSakiFrom != null)
                        {
                            TokiStData = ListTokiSt.Where(_ => _.TokuiSeq == (Result.TokiskTokuiSakiFrom?.TokuiSeq ?? -1)).ToList();
                            Result.TokiStTokuiSakiFrom = TokiStData.FirstOrDefault(_ => _.SitenCd == int.Parse(TokiStTokuiSakiFrom));
                        }
                    }
                }
                if (GyosyaTokuiSakiTo != null)
                {
                    Result.GyosyaTokuiSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCd == int.Parse(GyosyaTokuiSakiTo));
                    if (TokiskTokuiSakiTo != null)
                    {
                        List<CustomerComponentTokiskData> TokiskData = new List<CustomerComponentTokiskData>();
                        List<CustomerComponentTokiStData> TokiStData = new List<CustomerComponentTokiStData>();
                        TokiskData = ListTokisk.Where(_ => _.GyosyaCdSeq == (Result.GyosyaTokuiSakiTo?.GyosyaCdSeq ?? -1)).ToList();
                        Result.TokiskTokuiSakiTo = TokiskData.FirstOrDefault(_ => _.TokuiCd == int.Parse(TokiskTokuiSakiTo));
                        if (TokiStTokuiSakiTo != null)
                        {
                            TokiStData = ListTokiSt.Where(_ => _.TokuiSeq == (Result.TokiskTokuiSakiTo?.TokuiSeq ?? -1)).ToList();
                            Result.TokiStTokuiSakiTo = TokiStData.FirstOrDefault(_ => _.SitenCd == int.Parse(TokiStTokuiSakiTo));
                        }
                    }
                }

                // 仕入先
                if (GyosyaShiireSakiFrom != null)
                {
                    Result.GyosyaShiireSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCd == int.Parse(GyosyaShiireSakiFrom));
                    if (TokiskShiireSakiFrom != null)
                    {
                        List<CustomerComponentTokiskData> TokiskData = new List<CustomerComponentTokiskData>();
                        List<CustomerComponentTokiStData> TokiStData = new List<CustomerComponentTokiStData>();
                        TokiskData = ListTokisk.Where(_ => _.GyosyaCdSeq == (Result.GyosyaShiireSakiFrom?.GyosyaCdSeq ?? -1)).ToList();
                        Result.TokiskShiireSakiFrom = TokiskData.FirstOrDefault(_ => _.TokuiCd == int.Parse(TokiskShiireSakiFrom));
                        if (TokiStShiireSakiFrom != null)
                        {
                            TokiStData = ListTokiSt.Where(_ => _.TokuiSeq == (Result.TokiskShiireSakiFrom?.TokuiSeq ?? -1)).ToList();
                            Result.TokiStShiireSakiFrom = TokiStData.FirstOrDefault(_ => _.SitenCd == int.Parse(TokiStShiireSakiFrom));
                        }
                    }
                }
                if (GyosyaShiireSakiTo != null)
                {
                    Result.GyosyaShiireSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCd == int.Parse(GyosyaShiireSakiTo));
                    if (TokiskShiireSakiTo != null)
                    {
                        List<CustomerComponentTokiskData> TokiskData = new List<CustomerComponentTokiskData>();
                        List<CustomerComponentTokiStData> TokiStData = new List<CustomerComponentTokiStData>();
                        TokiskData = ListTokisk.Where(_ => _.GyosyaCdSeq == (Result.GyosyaShiireSakiTo?.GyosyaCdSeq ?? -1)).ToList();
                        Result.TokiskShiireSakiTo = TokiskData.FirstOrDefault(_ => _.TokuiCd == int.Parse(TokiskShiireSakiTo));
                        if (TokiStShiireSakiTo != null)
                        {
                            TokiStData = ListTokiSt.Where(_ => _.TokuiSeq == (Result.TokiskShiireSakiTo?.TokuiSeq ?? -1)).ToList();
                            Result.TokiStShiireSakiTo = TokiStData.FirstOrDefault(_ => _.SitenCd == int.Parse(TokiStShiireSakiTo));
                        }
                    }
                }

                // 団体区分
                if (DantaiKbnFrom != null)
                {
                    Result.DantaiKbnFrom = CodeKbList.Find(x => x != null && x.CodeKbn == DantaiKbnFrom);
                }
                if (DantaiKbnTo != null)
                {
                    Result.DantaiKbnTo = CodeKbList.Find(x => x != null && x.CodeKbn == DantaiKbnTo);
                }

                // 客種区分
                if (KyakuDaneKbnFrom != null)
                {
                    Result.KyakuDaneKbnFrom = CustomerClassificationList.Find(x => x != null && x.JyoKyakuCd == int.Parse(KyakuDaneKbnFrom));
                }
                if (KyakuDaneKbnTo != null)
                {
                    Result.KyakuDaneKbnTo = CustomerClassificationList.Find(x => x != null && x.JyoKyakuCd == int.Parse(KyakuDaneKbnTo));
                }

                // 行先
                if (YukiSakiFrom != null)
                {
                    Result.YukiSakiFrom = DestinationList.Find(x => x != null && $"{x.CodeKbn:00}{x.BasyoMapCd}" == YukiSakiFrom);
                }
                if (YukiSakiTo != null)
                {
                    Result.YukiSakiTo = DestinationList.Find(x => x != null && $"{x.CodeKbn:00}{x.BasyoMapCd}" == YukiSakiTo);
                }

                // 配車地
                if (HaishaChiFrom != null)
                {
                    Result.HaishaChiFrom = DispatchList.Find(x => x != null && $"{x.CodeKbn:00}{x.HaiSCd}" == HaishaChiFrom);
                }
                if (HaishaChiTo != null)
                {
                    Result.HaishaChiTo = DispatchList.Find(x => x != null && $"{x.CodeKbn:00}{x.HaiSCd}" == HaishaChiTo);
                }

                // 発生地
                if (HasseiChiFrom != null)
                {
                    Result.HasseiChiFrom = OriginList.Find(x => x != null && $"{x.CodeKbn:00}{x.BasyoMapCd}" == HasseiChiFrom);
                }
                if (HasseiChiTo != null)
                {
                    Result.HasseiChiTo = OriginList.Find(x => x != null && $"{x.CodeKbn:00}{x.BasyoMapCd}" == HasseiChiTo);
                }

                // エリア
                if (AreaFrom != null)
                {
                    Result.AreaFrom = AreaList.Find(x => x != null && $"{x.CodeKbn:00}{x.BasyoMapCd}" == AreaFrom);
                }
                if (AreaTo != null)
                {
                    Result.AreaTo = AreaList.Find(x => x != null && $"{x.CodeKbn:00}{x.BasyoMapCd}" == AreaTo);
                }

                // 車種
                if (ShashuFrom != null)
                {
                    Result.ShashuFrom = BusTypeList.Find(x => x != null && x.SyaSyuCd == int.Parse(ShashuFrom));
                }
                if (ShashuTo != null)
                {
                    Result.ShashuTo = BusTypeList.Find(x => x != null && x.SyaSyuCd == int.Parse(ShashuTo));
                }

                // 車種単価
                if (ShashuTankaFrom != null && ShashuTankaFrom.Length <= 9 && long.TryParse(ShashuTankaFrom, out _))
                {
                    Result.ShashuTankaFrom = ShashuTankaFrom;
                }
                if (ShashuTankaTo != null && ShashuTankaTo.Length <= 9 && long.TryParse(ShashuTankaTo, out _))
                {
                    Result.ShashuTankaTo = ShashuTankaTo;
                }

                // 受付条件
                if (UketsukeJokenFrom != null)
                {
                    Result.UketsukeJokenFrom = ConditionList.Find(x => x != null && x.CodeKbn == UketsukeJokenFrom);
                }
                if (UketsukeJokenTo != null)
                {
                    Result.UketsukeJokenTo = ConditionList.Find(x => x != null && x.CodeKbn == UketsukeJokenTo);
                }

                // 団体名
                Result.DantaiNm = HttpUtility.UrlDecode(DantaiNm); // P.M.Nhat add new prop 2020/09/04

                // 上限下限設定
                if (MaxMinSetting != null && int.TryParse(MaxMinSetting, out _))
                {
                    Result.MaxMinSetting = MaxMinSettingList.Find(x => x != null && x.IdValue == int.Parse(MaxMinSetting));
                }

                // 予約ステータス
                if (ReservationStatus != null && int.TryParse(ReservationStatus, out _))
                {
                    Result.ReservationStatus = ReservationStatusList.Find(x => x != null && x.IdValue == int.Parse(ReservationStatus));
                }

                return Result;
            }
            catch (Exception e)
            {
                return new HyperFormData();
            }
        }
    }
}
