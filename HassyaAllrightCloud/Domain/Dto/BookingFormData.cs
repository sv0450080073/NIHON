using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Entities;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.IService;
using System.Globalization;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BookingFormData
    {
        public bool IsPaidOrCoupon { get; set; }
        public bool IsLock { get; set; }
        public bool IsDailyReportRegisted { get; set; } // TKD_Yyksho.NippoKbn != 1
        public bool IsDisableEdit
        {
            get
            {
                return IsPaidOrCoupon || IsLock;
            }
        }

        public TPM_CodeKbCodeSyuData BookingStatus { get; set; }
        public bool IsCopyMode { get; set; } = false;
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public ReservationClassComponentData CurrentBookingType { get; set; } = new ReservationClassComponentData();
        public LoadSaleBranch SelectedSaleBranch { get; set; } = new LoadSaleBranch();
        public LoadStaff SelectedStaff { get; set; } = new LoadStaff();
        public string TextOrganizationName { get; set; } = "";
        public CustomerComponentGyosyaData customerComponentGyosyaData { get; set; } = new CustomerComponentGyosyaData();
        public CustomerComponentTokiskData customerComponentTokiskData { get; set; }
        public CustomerComponentTokiStData customerComponentTokiStData { get; set; }
        public DateTime BusStartDate { get; set; } = DateTime.Today;
        public MyTime BusStartTime { get; set; } = new MyTime(0,0);
        public DateTime BusEndDate { get; set; } = DateTime.Today;
        public MyTime BusEndTime { get; set; } = new MyTime(23,59);
        public bool PreDaySetting { get; set; } = false;
        public bool AftDaySetting { set; get; } = false;
        public DateTime InvoiceDate { set; get; } = DateTime.Today;
        public string InvoiceMonth { get; set; } = DateTime.Now.ToString("yyyy年MM月");
        public TaxTypeList TaxTypeforBus { get; set; } = new TaxTypeList();
        public HasuSettings HasuSet { get; set; } = new HasuSettings { TaxSetting = RoundSettings.Round, FeeSetting = RoundSettings.Round };
        private float _taxRate = 10;
        public string TaxRate
        {
            get
            {
                return _taxRate.ToString();
            }
            set
            {
                if(float.TryParse(value, out float newValue))
                {
                    _taxRate = (float)Math.Truncate(newValue * 10) / 10;
                }
            }
        }

        private long taxBus;
        public string TaxBus
        {
            get
            {
                if (this.TaxTypeforBus.IdValue == Constants.NoTax.IdValue)
                {
                    this.taxBus = 0;
                }
                else
                {
                    decimal totalPrice = (decimal)VehicleGridDataList.Sum(t => long.Parse(t.BusPrice));
                    decimal taxRate = (decimal)Math.Round(this._taxRate / 100, 3);

                    if (this.TaxTypeforBus.IdValue == Constants.ForeignTax.IdValue)
                    {
                        this.taxBus = (long)RoundHelper[HasuSet.TaxSetting](totalPrice * taxRate);
                    }
                    else // InTax
                    {
                        this.taxBus = (long)RoundHelper[HasuSet.TaxSetting]((totalPrice * taxRate) / (1 + taxRate));
                    }
                }

                return this.taxBus.ToString();
            }
            set
            {
                long tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.taxBus = tmp;
            }
        }
        public TaxTypeList TaxTypeforGuider { get; set; } = new TaxTypeList();        

        private long taxGuider;
        public string TaxGuider
        {
            get
            {
                if (this.TaxTypeforGuider.IdValue == Constants.NoTax.IdValue)
                {
                    this.taxGuider = 0;
                }
                else
                {
                    decimal totalPrice = (decimal)VehicleGridDataList.Sum(t => long.Parse(t.GuiderFee));
                    decimal taxRate = (decimal)Math.Round(this._taxRate / 100, 3);

                    if (this.TaxTypeforGuider.IdValue == Constants.ForeignTax.IdValue)
                    {
                        this.taxGuider = (long)RoundHelper[HasuSet.TaxSetting](totalPrice * taxRate);
                    }
                    else // InTax
                    {
                        this.taxGuider = (long)RoundHelper[HasuSet.TaxSetting]((totalPrice * taxRate) / (1 + taxRate));
                    }
                }

                return this.taxGuider.ToString();
            }
            set
            {
                long tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.taxGuider = tmp;
            }
        }
        private float _feeBusRate;
        public string FeeBusRate
        {
            get
            {
                return _feeBusRate.ToString();
            }
            set
            {
                if (float.TryParse(value, out float newValue))
                {
                    _feeBusRate = (float)Math.Truncate(newValue * 10) / 10;
                }
            }
        }

        private long feeBus;
        public string FeeBus
        {
            get
            {
                if (this.TaxTypeforBus.IdValue == Constants.ForeignTax.IdValue)
                {
                    var total = (decimal)VehicleGridDataList.Sum(t => long.Parse(t.BusPrice));
                    var fee = long.Parse(TaxBus);
                    var feeRate = (decimal)Math.Round(this._feeBusRate / 100,3);
                    this.feeBus = (long)RoundHelper[HasuSet.FeeSetting]((total + fee) * feeRate);
                }
                else
                {
                    this.feeBus = (long)RoundHelper[HasuSet.FeeSetting]((decimal)VehicleGridDataList.Sum(t => long.Parse(t.BusPrice)) * (decimal)Math.Round(this._feeBusRate / 100,3));
                }
                return this.feeBus.ToString();
            }
            set
            {
                long tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.feeBus = tmp;
            }
        }
        private float _feeGuiderRate;
        public string FeeGuiderRate
        {
            get
            {
                return _feeGuiderRate.ToString();
            }
            set
            {
                if (float.TryParse(value, out float newValue))
                {
                    _feeGuiderRate = (float)Math.Truncate(newValue * 10) / 10;
                }
            }
        }

        private long feeGuider;
        public string FeeGuider
        {
            get
            {
                if (this.TaxTypeforGuider.IdValue == Constants.ForeignTax.IdValue)
                {
                    var total = (decimal)VehicleGridDataList.Sum(t => long.Parse(t.GuiderFee));
                    var fee = long.Parse(TaxGuider);
                    var feeRate = (decimal)Math.Round(this._feeGuiderRate / 100, 3);
                    this.feeGuider = (long)RoundHelper[HasuSet.FeeSetting]((total + fee) * feeRate);
                }
                else
                {
                    this.feeGuider = (long)RoundHelper[HasuSet.FeeSetting]((decimal)VehicleGridDataList.Sum(t => long.Parse(t.GuiderFee)) * (decimal)Math.Round(this._feeGuiderRate / 100,3));
                }
                return this.feeGuider.ToString();
            }
            set
            {
                long tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.feeGuider = tmp;
            }
        }
        public string YykshoUpdYmdTime { get; set; }
        public string UnkoUpdYmdTime { get; set; }
        public string YykSyuUpdYmdTime { get; set; }
        public string HaishaUpdYmdTime { get; set; }
        public string MishumUpdYmdTime { get; set; }
        public string KakninUpdYmdTime { get; set; }
        public string BookingMaxMinFareFeeCalcUpdYmdTime { get; set; }
        public string BookingMaxMinFareFeeCalcMeisaiUpdYmdTime { get; set; }
        public string UnkobiFileUpdYmdTime { get; set; }
        public List<VehicleGridData> VehicleGridDataList { get; set; } = new List<VehicleGridData>();
        public long TotalGuiderFee
        {
            get
            {
                if(VehicleGridDataList is null) return 0;
                return VehicleGridDataList.Sum(t => long.Parse(t.GuiderFee));
            }
        }
        public SupervisorTabData SupervisorTabData { get; set; } = new SupervisorTabData();
        public ReservationTabData ReservationTabData { get; set; } = new ReservationTabData();
        public string BikoNm { get; set; }
        // use this field in TkdYyksho to check if booking fixed or not
        public string KaktYmd { get; set; }
        public byte HaiSKbn { get; set; }
        public List<ConfirmationTabData> ConfirmationTabDataList { get; set; } = new List<ConfirmationTabData>();
        public PassengerType PassengerTypeData { get; set; }
        public CancelTickedData CancelTickedData { get; set; } = new CancelTickedData();
        public InvoiceType SelectedInvoiceType { get; set; }
        public List<LoadFuttum> FuttumViewList { get; set; } = new List<LoadFuttum>();
        public List<BookingArrangementData> ArrangementTabList { get; set; } = new List<BookingArrangementData>();

        /// <summary>
        /// True - Update with Auto Assign
        /// False - Update without Auto Assign
        /// </summary>
        public bool IsUpdateWithAutoAssign { get; set; } = false;

        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Verify if booking data is modified and should re auto assign vehicle
        /// </summary>
        /// <param name="previous">previous booking data</param>
        /// <returns>
        /// true - when haisha is modified and should be updated with re auto assign vehicle
        /// false - can be updated without re-auto assign vehicle
        /// </returns>
        public bool VerifyHaishaModified(BookingFormData previous)
        {
            //Start time, end time
            if (this.BusStartDate.Year != previous.BusStartDate.Year || 
                this.BusStartDate.Month != previous.BusStartDate.Month || 
                this.BusStartDate.Day != previous.BusStartDate.Day || 
                this.BusStartTime.Str != previous.BusStartTime.Str || 
                //this.BusStartTime.myMinute != previous.BusStartTime.myMinute ||
                this.BusEndDate.Year != previous.BusEndDate.Year ||
                this.BusEndDate.Month != previous.BusEndDate.Month ||
                this.BusEndDate.Day != previous.BusEndDate.Day ||
                this.BusEndTime.Str != previous.BusEndTime.Str/* ||
                this.BusEndTime.myMinute != previous.BusEndTime.myMinute*/)
            {
                return true;
            }
            //Number of vehicle grid
            if (this.VehicleGridDataList.Count != previous.VehicleGridDataList.Count)
            {
                return true;
            }
            for (int i = 0; i < this.VehicleGridDataList.Count; i++)
            {
                var currentEigyo = this.VehicleGridDataList[i].PriorityAutoAssignBranch != null ? this.VehicleGridDataList[i].PriorityAutoAssignBranch.EigyoCdSeq : -1;
                var previousEigyo = previous.VehicleGridDataList[i].PriorityAutoAssignBranch != null ? previous.VehicleGridDataList[i].PriorityAutoAssignBranch.EigyoCdSeq : -1;
                //Bus type - Bus num
                if (this.VehicleGridDataList[i].busTypeData.Katakbn != previous.VehicleGridDataList[i].busTypeData.Katakbn ||
                    this.VehicleGridDataList[i].busTypeData.SyaSyuCdSeq != previous.VehicleGridDataList[i].busTypeData.SyaSyuCdSeq ||
                    this.VehicleGridDataList[i].BusNum != previous.VehicleGridDataList[i].BusNum ||
                    this.VehicleGridDataList[i].RowID != previous.VehicleGridDataList[i].RowID ||
                    currentEigyo != previousEigyo)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Verify if booking data is modified and should select vehicle to be removed
        /// </summary>
        /// <param name="previous">previous booking data</param>
        /// <returns>
        /// true - select vehicle => open popup for user to choose
        /// false - no need to select => no popup for user to choose
        /// </returns>
        public bool VerifyHaishaRemoved(BookingFormData previous)
        {
            foreach (var row in this.VehicleGridDataList.Select(v => v.RowID))
            {
                var currentBusNum = this.VehicleGridDataList.FirstOrDefault(v => v.RowID == row).BusNum;
                var previousBusNum = previous.VehicleGridDataList.FirstOrDefault(v => v.RowID == row)?.BusNum ?? currentBusNum;
                if (int.Parse(currentBusNum) < int.Parse(previousBusNum))
                {
                    return true;
                }
            }
            return false;
        }
        public void SetBookingFromYoushaNotice(YoushaNoticeData newData)
        {
            this.BusStartDate = DateTime.ParseExact(newData.HaiSYmd, "yyyyMMdd", new CultureInfo("ja-JP"));
            this.BusStartTime = new BookingInputHelper.MyTime(Convert.ToInt32(newData.HaiSTime.Substring(0, 2)), Convert.ToInt32(newData.HaiSTime.Substring(2)));
            this.BusEndDate = DateTime.ParseExact(newData.TouYmd, "yyyyMMdd", new CultureInfo("ja-JP"));
            this.BusEndTime = new BookingInputHelper.MyTime(Convert.ToInt32(newData.TouChTime.Substring(0, 2)), Convert.ToInt32(newData.TouChTime.Substring(2)));
            this.TextOrganizationName = newData.DanTaNm;
        }
        public void SetBookingFromData(BookingFormData newData)
        {
            this.IsPaidOrCoupon = newData.IsPaidOrCoupon;
            this.IsLock = newData.IsLock;
            this.IsDailyReportRegisted = newData.IsDailyReportRegisted;
            this.BookingStatus = newData.BookingStatus;
            this.IsCopyMode = false;
            this.CurrentBookingType = newData.CurrentBookingType;
            this.SelectedSaleBranch = newData.SelectedSaleBranch;
            this.SelectedStaff = newData.SelectedStaff;
            this.TextOrganizationName = newData.TextOrganizationName;
            this.customerComponentTokiStData = newData.customerComponentTokiStData;
            this.customerComponentTokiskData = newData.customerComponentTokiskData;
            this.customerComponentGyosyaData = newData.customerComponentGyosyaData;
            this.BusStartDate = newData.BusStartDate;
            this.BusStartTime = newData.BusStartTime;
            this.BusEndDate = newData.BusEndDate;
            this.BusEndTime = newData.BusEndTime;
            this.PreDaySetting = newData.PreDaySetting;
            this.AftDaySetting = newData.AftDaySetting;
            this.InvoiceDate = newData.InvoiceDate;
            this.InvoiceMonth = newData.InvoiceMonth;
            this.TaxTypeforBus = newData.TaxTypeforBus;
            this.TaxRate = newData.TaxRate;
            this.taxBus = newData.taxBus;
            this.TaxBus = newData.TaxBus;
            this.TaxTypeforGuider = newData.TaxTypeforGuider;
            this.taxGuider = newData.taxGuider;
            this.TaxGuider = newData.TaxGuider;
            this.FeeBusRate = newData.FeeBusRate;
            this.feeBus = newData.feeBus;
            this.FeeBus = newData.FeeBus;
            this.FeeGuiderRate = newData.FeeGuiderRate;
            this.feeGuider = newData.feeGuider;
            this.FeeGuider = newData.FeeGuider;
            this.VehicleGridDataList = newData.VehicleGridDataList;
            this.SupervisorTabData = newData.SupervisorTabData;
            this.ReservationTabData.SetData(newData.ReservationTabData);
            this.BikoNm = newData.BikoNm;
            this.ConfirmationTabDataList = newData.ConfirmationTabDataList;
            this.IsUpdateWithAutoAssign = newData.IsUpdateWithAutoAssign;
            this.PassengerTypeData = newData.PassengerTypeData;
            this.SelectedInvoiceType = newData.SelectedInvoiceType;
            this.CancelTickedData.SetData(newData.CancelTickedData);
            this.KaktYmd = newData.KaktYmd;
            this.FuttumViewList = newData.FuttumViewList;
            this.ArrangementTabList = newData.ArrangementTabList;
            this.CustomData = newData.CustomData;
            this.UnkRen = newData.UnkRen;
            this.YykshoUpdYmdTime = newData.YykshoUpdYmdTime;
            this.UnkoUpdYmdTime = newData.UnkoUpdYmdTime;
            this.YykSyuUpdYmdTime = newData.YykSyuUpdYmdTime;
            this.HaishaUpdYmdTime = newData.HaishaUpdYmdTime;
            this.MishumUpdYmdTime = newData.MishumUpdYmdTime;
            this.KakninUpdYmdTime = newData.KakninUpdYmdTime;
            this.BookingMaxMinFareFeeCalcUpdYmdTime = newData.BookingMaxMinFareFeeCalcUpdYmdTime;
            this.BookingMaxMinFareFeeCalcMeisaiUpdYmdTime = newData.BookingMaxMinFareFeeCalcMeisaiUpdYmdTime;
            this.UnkobiFileUpdYmdTime = newData.UnkobiFileUpdYmdTime;
            this.UkeNo = newData.UkeNo;
            this.CurrentMaxSyaSyuRen = newData.CurrentMaxSyaSyuRen;
            this.HaiSKbn = newData.HaiSKbn;
        }

        public void CalculateCancelFee()
        {
            // CancelFeeRate range 0 -> 99.9 => can convert to int32
            float cancelFeeRate = 0f;
            float.TryParse(this.CancelTickedData.CancelFeeRate, out cancelFeeRate);

            int uriGakKin = this.CancelTickedData.UriGakKin;
            int newCanCelFee = Convert.ToInt32(Math.Ceiling(cancelFeeRate / 100 * uriGakKin));
            this.CancelTickedData.CancelFee = newCanCelFee.ToString();
        }

        public void CalculateCancelTaxFee()
        {
            int newCancelTaxFee = 0;
            TaxTypeList taxType = this.CancelTickedData.CancelTaxType;
            float cancelTaxRate = float.Parse(this.CancelTickedData.CancelTaxRate);
            int cancelFee = int.Parse(this.CancelTickedData.CancelFee);

            if (taxType.IdValue == Constants.NoTax.IdValue)
            {
                newCancelTaxFee = 0;
            }
            else if (taxType.IdValue == Constants.InTax.IdValue)
            {
                newCancelTaxFee = Convert.ToInt32(RoundHelper[HasuSet.TaxSetting]((decimal)((cancelFee * cancelTaxRate) / (100 + cancelTaxRate))));
            }
            else
            {
                newCancelTaxFee = Convert.ToInt32(RoundHelper[HasuSet.TaxSetting]((decimal)((cancelFee * cancelTaxRate / 100))));
            }
            this.CancelTickedData.CancelTaxFee = newCancelTaxFee.ToString();
        }

        /// <summary>
        /// Get bus garage date time
        /// </summary>
        /// <returns>Tuple with two elements (<see cref="MyDate"/>, <see cref="MyDate"/>): leave date time, return date time</returns>
        public (MyDate Leave, MyDate Return) GetBusGarageDateTime()
        {
            BookingInputHelper.MyDate garageReturnDateTime = new BookingInputHelper.MyDate(ReservationTabData.GarageReturnDate, ReservationTabData.KikTime);
            BookingInputHelper.MyDate garageLeaveDateTime = new BookingInputHelper.MyDate(ReservationTabData.GarageLeaveDate, ReservationTabData.SyuKoTime);

            return (garageLeaveDateTime, garageReturnDateTime);
        }

        /// <summary>
        /// Get bus date time
        /// </summary>
        /// <returns>Tuple with two elements (<see cref="MyDate"/>, <see cref="MyDate"/>): start date time, end date time</returns>
        public (MyDate Start, MyDate End) GetBusDateTime()
        {
            BookingInputHelper.MyDate busStartDateTime = new BookingInputHelper.MyDate(BusStartDate, BusStartTime);
            BookingInputHelper.MyDate busEndDateTime = new BookingInputHelper.MyDate(BusEndDate, BusEndTime);

            return (busStartDateTime, busEndDateTime);
        }

        public string GetAverageUnitPriceIndex()
        {
            string result = "0";
            if (this.VehicleGridDataList.Where(v => v.UnitPriceIndex != "0").ToList().Count() > 0)
            {
                result = Math.Round(this.VehicleGridDataList.Where(v => v.UnitPriceIndex != "0").Average(t => double.Parse(t.UnitPriceIndex)), 1, MidpointRounding.AwayFromZero).ToString();
            }
            return result;
        }
        public bool IsYoushaNote { get; set; } = false;
        public List<BookingInputData.HaishaInfoData> ListToRemove { get; set; } = new List<BookingInputData.HaishaInfoData>();
        public int CurrentMaxSyaSyuRen { get; set; }
    };

    public class TPM_CodeKbCodeSyuData
    {
        public string CodeKbn { get; set; } // use this to save
        public string RyakuNm { get; set; }
        public bool Checked { get; set; }
        public string Text
        {
            get =>  CodeKbn == "" ? string.Empty:$"{CodeKbn}:{RyakuNm}";
        }
        public string TextBusTypeList
        {
            get => CodeKbn == "" ? string.Empty : $"{RyakuNm}";
        }
    }

    public class JourneyNoteView
    {
        public JourneyNoteView()
        {
            NoteViewList = new List<string>();
        }
        public string DateView { get; set; }
        public List<string> NoteViewList { get; set; }
    }
    public class PassengerType
    {
        public int JyoKyakuCdSeq { get; set; } // use this to save
        public string CodeKbn { get; set; }
        public string CodeKbnRyakuNm { get; set; }
        public byte JyoKyakuCd { get; set; }
        public string JyoKyaRyakuNm { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0}:{1} {2:D2}:{3}", CodeKbn, CodeKbnRyakuNm, JyoKyakuCd, JyoKyaRyakuNm);
            }
        }
    }
}
