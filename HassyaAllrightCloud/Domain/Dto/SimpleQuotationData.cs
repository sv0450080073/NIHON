using System;
using System.Collections.Generic;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using System.Linq;
using System.Globalization;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class SimpleQuotationData
    {
        public int TenantId { get; set; }
        public int UserLoginId { get; set; }
        public SimpleQuotationData(int tenantId, int userLoginId)
        {
            StartPickupDate = DateTime.Today;
            EndPickupDate = DateTime.Today;
            StartArrivalDate = DateTime.Today;
            EndArrivalDate = DateTime.Today;
            ExportType = OutputReportType.Preview;
            OutputOrientation = OutputOrientation.Horizontal;
            _ukeCdFrom = 0;
            _ukeCdTo = 0;

            TenantId = tenantId;
            UserLoginId = userLoginId;
        }
        public OutputReportType ExportType { get; set; }
        public OutputOrientation OutputOrientation { get; set; }
        public DateTime? StartPickupDate { get; set; }
        public DateTime? EndPickupDate { get; set; }
        public DateTime? StartArrivalDate { get; set; }
        public DateTime? EndArrivalDate { get; set; }

        // 予約区分
        // New
        public ReservationClassComponentData YoyakuFrom { get; set; }
        public ReservationClassComponentData YoyakuTo { get; set; }

        private double _ukeCdFrom;
        public string UkeCdFrom
        {
            get
            {
                if(_ukeCdFrom == 0) return string.Empty;
                return _ukeCdFrom.ToString("0000000000");
            }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    _ukeCdFrom = 0;
                }
                else if (double.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out double newValue))
                {
                    if(newValue < 0) return;
                    else if(newValue > 9999999999) _ukeCdFrom = 9999999999;
                    else _ukeCdFrom = newValue;
                }
            }
        }
        private double _ukeCdTo;
        public string UkeCdTo
        {
            get
            {
                if(_ukeCdTo == 0) return string.Empty;
                return _ukeCdTo.ToString("0000000000");
            }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    _ukeCdTo = 0;
                }
                else if (double.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out double newValue))
                {
                    if(newValue < 0) return;
                    else if(newValue > 9999999999) _ukeCdTo = 9999999999;
                    else _ukeCdTo = newValue;
                }
            }
        }

        // 仕入先
        // New
        public CustomerComponentGyosyaData GyosyaShiireSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaShiireSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiTo { get; set; }

        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }
        public bool Fare { get; set; }
    }

    public class SimpleQuotationReportFilter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bookingKeyList">booking of filtered data</param>
        /// <param name="tenantId">filter tenantcdseq</param>
        /// <param name="isDisplayMinMaxPrice">is display min max fare fee in report</param>
        /// <param name="outputOrientation">Simple => SimpleQuotation/[Horizontal, Vertical] => QuotationWihJourney</param>
        public SimpleQuotationReportFilter(
            List<BookingKeyData> bookingKeyList,
            int tenantId,
            bool isDisplayMinMaxPrice,
            QuotationReportType reportType = QuotationReportType.Simple)
        {
            BookingKeyList = bookingKeyList;
            TenantId = tenantId;
            IsDisplayMinMaxPrice = isDisplayMinMaxPrice;
            ReportType = reportType;
        }

        public List<BookingKeyData> BookingKeyList { get; set; }
        public int TenantId { get; set; }
        public bool IsDisplayMinMaxPrice { get; set; }
        public bool IsWithJourney => ReportType != QuotationReportType.Simple;
        public QuotationReportType ReportType { get; set; } = QuotationReportType.Simple;
    }

    public class SimpleQuotationDataReport
    {
        /// <summary>
        /// Stand for one page: primary key [ukeno, unkren]
        /// </summary>
        public SimpleQuotationDataReport()
        {
            HeaderData = new Header();
            BodyDataList = new List<Body>();
            BodyJourneyDataList = new List<BodyJourney>();
            KoteiDataList = new List<BodyJourney>();
            TehaiDataList = new List<BodyJourney>();
            FooterData = new Footer();
            FooterCarCountList = new List<FooterCarCount>();
            CurrentPage = 1;
            TotalPage = 1;
        }

        /// <summary>
        /// current page of [ukeno, unkren]
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// total page of that [ukeno, unkren]
        /// </summary>
        public int TotalPage { get; set; }
        public Header HeaderData { get; set; }
        public List<Body> BodyDataList { get; set; }
        public List<BodyJourney> BodyJourneyDataList { get; set; }
        public List<BodyJourney> KoteiDataList { get; set; }
        public List<BodyJourney> TehaiDataList { get; set; }
        public Footer FooterData { get; set; }
        public List<FooterCarCount> FooterCarCountList { get; set; }
        public long LargeCarCount
        {
            get
            {
                return FooterCarCountList.FirstOrDefault(c => c.CodeKbnNm.Equals("大型"))?.Value
                    ?? 0;
            }
        }
        public long MediumCarCount
        {
            get
            {
                return FooterCarCountList.FirstOrDefault(c => c.CodeKbnNm.Equals("中型"))?.Value
                    ?? 0;
            }
        }
        public long SmallCarCount
        {
            get
            {
                return FooterCarCountList.FirstOrDefault(c => c.CodeKbnNm.Equals("小型"))?.Value
                    ?? 0;
            }
        }
        public string FarePrice { get; set; }
        public string FareTax { get; set; }
        public string TaxIncludePrice { get; set; }
        public string GuiderCost { get; set; }
        public string FutaiCost { get; set; }
        public string TsumiCost { get; set; }
        public string TehaiCost { get; set; }
        public string TransportCost { get; set; }
        public string MitTotal { get; set; }
        public string TotalSyaRyoSyo { get; set; }
        public string Total { get; set; }

        /// <summary>
        /// Use before paging to save summary value between pages
        /// </summary>
        public void UpdateAllSummaryFields()
        {
            if (BodyDataList is null || BodyDataList.Count == 0)
            {
                FarePrice = FareTax = TaxIncludePrice = MitTotal = TotalSyaRyoSyo = Total = string.Empty;
                return;
            }
            var fareDataList = BodyDataList.Where(f => f.RowType == 1);

            var result = fareDataList.Sum(item => item.Suryo * item.Tanka);
            FarePrice = string.Format("{0:N0}", result);

            var result2 = fareDataList.Sum(item => item.Suryo * item.SyaRyoSyo);
            FareTax = string.Format("{0:N0}", result2);

            var result3 = fareDataList.Sum(item => item.UriGakKin);
            TaxIncludePrice = string.Format("{0:N0}", result3);

            var result4 = BodyDataList.Sum(item => item.Suryo * item.TankaWithoutTax);
            MitTotal = string.Format("{0:N0}", result4);

            var result5 = BodyDataList.Sum(item => item.Suryo * item.SyaRyoSyo);
            TotalSyaRyoSyo = string.Format("{0:N0}", result5);

            var result6 = BodyDataList.Sum(item => item.UriGakKin);
            Total = string.Format("{0:N0}", result6);

            var guiderDataList = BodyDataList.Where(f => f.RowType == 2);
            var result7 = guiderDataList.Sum(item => item.UriGakKin);
            GuiderCost = string.Format("{0:N0}", result7);

            var futaiDataList = BodyDataList.Where(f => f.RowType == 3 && f.FutTumKbn == 1 && f.FutGuiKbn == 2);
            var result8 = futaiDataList.Sum(item => item.UriGakKin);
            FutaiCost = string.Format("{0:N0}", result8);

            var tsumiDataList = BodyDataList.Where(f => f.RowType == 3 && f.FutTumKbn == 2);
            var result9 = tsumiDataList.Sum(item => item.UriGakKin);
            TsumiCost = string.Format("{0:N0}", result9);

            var tehaiDataList = BodyDataList.Where(f => f.RowType == 3 && f.FutTumKbn == 1 && f.FutGuiKbn == 4);
            var result10 = tehaiDataList.Sum(item => item.UriGakKin);
            TehaiCost = string.Format("{0:N0}", result10);

            var transportDataList = BodyDataList.Where(f => f.RowType == 3 && f.FutTumKbn == 1 && f.FutGuiKbn == 3);
            var result11 = transportDataList.Sum(item => item.UriGakKin);
            TransportCost = string.Format("{0:N0}", result11);
        }

        #region hard code for print footer quotation journey
        public Body Row1
        {
            get
            {
                if(BodyDataList is null && BodyDataList.Count < 1)
                {
                    return new Body();
                }
                return BodyDataList[0];
            }
        }
        public Body Row2 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 2)
                {
                    return new Body();
                }
                return BodyDataList[1];
            }
        }
        public Body Row3 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 3)
                {
                    return new Body();
                }
                return BodyDataList[2];
            }
        }
        public Body Row4 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 4)
                {
                    return new Body();
                }
                return BodyDataList[3];
            }
        }
        public Body Row5 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 5)
                {
                    return new Body();
                }
                return BodyDataList[4];
            }
        }
        public Body Row6 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 6)
                {
                    return new Body();
                }
                return BodyDataList[5];
            }
        }
        public Body Row7 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 7)
                {
                    return new Body();
                }
                return BodyDataList[6];
            }
        }
        public Body Row8 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 8)
                {
                    return new Body();
                }
                return BodyDataList[7];
            }
        }
        public Body Row9 
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 9)
                {
                    return new Body();
                }
                return BodyDataList[8];
            }
        }
        public Body Row10
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 10)
                {
                    return new Body();
                }
                return BodyDataList[9];
            }
        }
        public Body Row11
        {
            get
            {
                if (BodyDataList is null && BodyDataList.Count < 11)
                {
                    return new Body();
                }
                return BodyDataList[10];
            }
        }
        #endregion

        public class Header
        {
            public string UkeNo { get; set; }
            public int UnkRen { get; set; }
            public long UkeCd { get; set; }
            public string DantaNm { get; set; }
            public string CompanyNm { get; set; }
            public string EigyoNm { get; set; }
            public string Jyus1 { get; set; }
            public string Jyus2 { get; set; }
            public string Eigyos_TelNo { get; set; }
            public string Eigyos_FaxNo { get; set; }
            public string SyainNm { get; set; }
            public string HaiSYmd { get; set; }
            public string TouYmd { get; set; }
            public short JyoSyaJin { get; set; }
            public short PlusJin { get; set; }
            public string PlusJinJyoSyaJinDisplay
            {
                get
                {
                    return string.Format("{0:N0}人 + {1:N0}人", JyoSyaJin, PlusJin);
                }
            }
            public string TokuiNm { get; set; }
            public string TokuRyakuNm { get; set; }
            public string KanJNm { get; set; }
            public string TokiSt_TelNo { get; set; }
            public string TokiSt_FaxNo { get; set; }
            public string MitBiko { get; set; }

            public string NitteiDisplay
            {
                get
                {
                    if(DateTime.TryParseExact(HaiSYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime haisYmd)
                        && DateTime.TryParseExact(TouYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime touYmd))
                    {
                        return string.Format("{0:yyyy'年'MM'月'dd'日'(ddd)} ～ {1:yyyy'年'MM'月'dd'日'(ddd)}",
                            haisYmd,
                            touYmd);
                    }
                    return string.Empty;
                }
            }
        }

        public class Body
        {
            public Body(bool isEmpty = false)
            {
                IsEmpty = isEmpty;
            }
            public bool IsEmpty { get; set; }
            public string UkeNo { get; set; }
            public int UnkRen { get; set; }
            public byte FutGuiKbn { get; set; }
            /// <summary>
            /// TKD_Futtum.FutTumKbn
            /// </summary>
            public byte FutTumKbn { get; set; }
            /// <summary>
            /// TKD_Futtum.FuttumNm
            /// </summary>
            public string ItemName { get; set; }
            /// <summary>
            /// TKD_Futtum.BikoNm
            /// </summary>
            public string BikoNm { get; set; }

            /// <summary>
            /// 1: Fare / 2: guider fee / 3: incidental (FutGuiKbn: [2:futai, 4:tehai, 3:transport])
            /// </summary>
            public byte RowType { get; set; }
            /// <summary>
            /// Suryo
            /// </summary>
            public short Suryo { get; set; }
            /// <summary>
            /// Tanka
            /// </summary>
            public int Tanka { get; set; }
            public long TankaWithoutTax
            {
                get
                {
                    if(ZeiKbn == Constants.InTax.IdValue)
                    {
                        return Tanka - SyaRyoSyo;
                    }
                    return Tanka;
                }
            }
            /// <summary>
            /// Tax type
            /// </summary>
            public byte ZeiKbn { get; set; }
            /// <summary>
            /// Tax type name
            /// </summary>
            public string TaxTypeName { get; set; }
            /// <summary>
            /// tax rate
            /// </summary>
            public decimal Zeiritsu { get; set; }
            /// <summary>
            /// Round type
            /// </summary>
            public byte SyohiHasu { get; set; }
            public long SyaRyoSyo
            {
                get
                {
                    if(ZeiKbn == Constants.NoTax.IdValue)
                    {
                        return 0;
                    }
                    if(ZeiKbn == Constants.ForeignTax.IdValue)
                    {
                        return (long)BookingInputHelper.RoundHelper[(RoundSettings)SyohiHasu](Zeiritsu * Tanka / 100);
                    }
                    if(ZeiKbn == Constants.InTax.IdValue)
                    {
                        return (long)BookingInputHelper.RoundHelper[(RoundSettings)SyohiHasu](Zeiritsu * Tanka / (Zeiritsu + 100));
                    }
                    return 0;
                }
            }
            public double UriGakKin
            {
                get
                {
                    if(ZeiKbn == Constants.ForeignTax.IdValue)
                    {
                        return Suryo * (Tanka + SyaRyoSyo);
                    }
                    return Suryo * Tanka;
                }
            }
            
        }

        public class BodyJourney
        {
            public bool IsHideDate { get; set; }
            public string UkeNo { get; set; }
            public int UnkRen { get; set; }
            public string HaiSYmd { get; set; }
            public short Nittei { get; set; }
            public string Koutei { get; set; }
            public string TehNm { get; set; }

            public string JourneyDate
            {
                get
                {
                    if(!IsHideDate && DateTime.TryParseExact(HaiSYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime outDt))
                    {
                        return outDt.AddDays(Nittei - 1).ToString("yyyy/MM/dd (ddd)");
                    }
                    return string.Empty;
                }
            }
        }

        public class FooterCarCount
        {
            public string UkeNo { get; set; }
            public int UnkRen { get; set; }
            public long Value { get; set; }
            public string CodeKbnNm { get; set; }
            public string CodeKbn { get; set; }
        }

        public class Footer
        {
            public bool IsDisplayMinMaxPrice { get; set; }
            public string UkeNo { get; set; }
            public int UnkRen { get; set; }
            public long SoukouKiro { get; set; }
            public long SoukouTime { get; set; }
            public string MinMaxLabelDisplay => IsDisplayMinMaxPrice ? "上限下限" : string.Empty;
            public string MinMaxPriceDisplay
            {
                get
                {
                    if(IsDisplayMinMaxPrice)
                    {
                        return string.Format("{0:N0} / {1:N0}", MaxPrice, MinPrice);
                    }
                    return string.Empty;
                }
            }
            public long MaxPrice { get; set; }
            public long MinPrice { get; set; }
        }
    }
}
