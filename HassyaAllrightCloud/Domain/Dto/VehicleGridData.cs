using HassyaAllrightCloud.Commons.Helpers;
using System.Linq;
using HassyaAllrightCloud.Commons.Constants;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VehicleGridData
    {
        public string RowID { get; set; }
        private int _busNum;
        public string BusNum
        {
            get
            {
                return _busNum.ToString();
            }
            set
            {
                int.TryParse(value, out _busNum);
            }
        }
        private int _driverNum;
        public string DriverNum { get => _driverNum.ToString(); set => int.TryParse(value, out _driverNum); }

        private int unitBusPrice;
        public string UnitBusPrice
        {
            get
            {
                return unitBusPrice.ToString();
            }
            set
            {
                int tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.unitBusPrice = tmp;
            }
        }

        private int unitBusFee;
        public string UnitBusFee
        {
            get
            {
                return unitBusFee.ToString();
            }
            set
            {
                int tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.unitBusFee = tmp;
            }
        }

        private int unitPrice;
        public string UnitPrice
        {
            get
            {
                this.unitPrice = this.unitBusPrice + this.unitBusFee;
                return this.unitPrice.ToString();
            }
            set
            {
                int tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.unitPrice = tmp;
            }
        }

        private int busPrice;
        public string BusPrice
        {
            get
            {
                this.busPrice = this.unitPrice * this._busNum;
                return this.busPrice.ToString();
            }
            set
            {
                int tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.busPrice = tmp;
            }
        }
        private int _guiderNum;
        public string GuiderNum
        {
            get => _guiderNum.ToString();
            set => int.TryParse(value, out _guiderNum);
        }

        private int unitGuiderPrice;
        public string UnitGuiderFee
        {
            get
            {
                return unitGuiderPrice.ToString();
            }
            set
            {
                int tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.unitGuiderPrice = tmp;
            }
        }

        private int guiderFee;
        public string GuiderFee
        {
            get
            {
                this.guiderFee = this._guiderNum * this.unitGuiderPrice;
                return this.guiderFee.ToString();
            }
            set
            {
                int tmp;
                CommonUtil.NumberTryParse(value, out tmp);
                this.guiderFee = tmp;
            }
        }

        public BusTypeData busTypeData { get; set; } = new BusTypeData();

        /// <summary>
        /// #6621 fix bug null transportation fee rule will return tooltip empty
        /// </summary>
        /// <returns></returns>
        public string getTooltipForUnitBusPrice()
        {
            if (this.minMaxForm == null || this.minMaxForm.typeBus == -1 || this.minMaxForm.SelectedTranSportationOfficePlace == null) // Case the min-max popup is not setting yet
            {
                return "";
            }
            int maxUnitPriceForKm, minUnitPriceForKm, maxUnitPriceForHour, minUnitPriceForHour,
                sumOfAllKm, sumOfAllTime, maxUnitPrice, maxUnitPriceAfterDiscount, minUnitPrice, maxUnitBusFee, maxUnitBusFeeAfterDiscount, minUnitBusFee,
                exactMaxUnitPriceForKm, exactMinUnitPriceForKm, exactMaxUnitPriceForHour, exactMinUnitPriceForHour;
            maxUnitPriceForKm = minMaxForm.getMasterMaxUnitPriceForKm();
            minUnitPriceForKm = minMaxForm.getMasterMinUnitPriceForKm();
            maxUnitPriceForHour = minMaxForm.getMasterMaxUnitPriceForHour();
            minUnitPriceForHour = minMaxForm.getMasterMinUnitPriceForHour();

            sumOfAllKm = BookingInputHelper.Round(minMaxForm.minMaxGridData.Sum(t => t.KmRunning));
            BookingInputHelper.MyTime sumOfAllTimeMyTime = BookingInputHelper.Round(minMaxForm.minMaxGridData.Aggregate<MinMaxGridData,
                               BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning));
            sumOfAllTime = sumOfAllTimeMyTime.ToString().Length==5?int.Parse(sumOfAllTimeMyTime.ToString().Substring(0, 2)):int.Parse(sumOfAllTimeMyTime.ToString().Substring(0, 3));

            maxUnitPrice = minMaxForm.getMaxUnitPriceForkm() + minMaxForm.getMaxUnitPriceForHour();
            maxUnitPriceAfterDiscount = minMaxForm.getMaxUnitBusPriceDiscount();
            minUnitPrice = minMaxForm.getMinUnitPriceForkm() + minMaxForm.getMinUnitPriceForHour();
            maxUnitBusFee = minMaxForm.getMaxUnitBusFeeForKmWithChgDriver() + minMaxForm.getMaxUnitBusFeeforHourWithChgDriver() +
                minMaxForm.getMaxUnitBusSumFeeforMid9() + minMaxForm.getMaxSpecialVehicalFee();
            maxUnitBusFeeAfterDiscount = minMaxForm.getMaxUnitBusFeeDiscount();
            minUnitBusFee = minMaxForm.getMinUnitBusFeeForKmWithChgDriver() + minMaxForm.getMinUnitBusFeeforHourWithChgDriver() +
                minMaxForm.getMinUnitBusSumFeeforMid9() + minMaxForm.getMinSpecialVehicalFee();

            exactMaxUnitPriceForKm = sumOfAllKm * maxUnitPriceForKm;
            exactMinUnitPriceForKm = sumOfAllKm * minUnitPriceForKm;
            exactMaxUnitPriceForHour = sumOfAllTime * maxUnitPriceForHour;
            exactMinUnitPriceForHour = sumOfAllTime * minUnitPriceForHour;

            string tooltip = "<div class='text-left'><p>運賃の上限下限金額の範囲参照" +
                    "<table class='table table-sm text-center mb-2'>" +
                    "<tr style='background: #f3f3f3'><td></td><td>上限（割引済）</td><td>下限</td></tr>" +
                    "<tr><td><b>運賃</b></td><td><b>{0:n0} 円</b></td><td><b>{1:n0} 円</b></td></tr>" +
                    "<tr><td>料金</td><td>{2:n0} 円</td><td>{3:n0} 円</td></tr></table>" +
                    "▲ 運輸局：{4}<br />" +
                    "▲ 車種：{5}<br />" +
                    "▲ 割引：{6} %<br />" +
                    "▲ 特殊車両割増：{7}<br />" +
                    "&lt; 運賃の内訳 &gt;<br/>" +
                    "<table class='table table-sm text-center mb-2'>" +
                    "<tr style='background: #f3f3f3'><td></td><td>上限</td><td>下限</td></tr>" +
                    "<tr><td class='text-left'>キロ制運賃</td><td class='text-right'>{8:n0} 円 x {9:n0} キロ = {10:n0} 円</td><td class='text-right'>{14:n0} 円 x {15:n0} キロ = {16:n0} 円</td></tr>" +
                    "<tr><td class='text-left'>時間制運賃</td><td class='text-right'>{11:n0} 円 x {12:n0} 時間 = {13:n0} 円</td><td class='text-right'>{17:n0} 円 x {18:n0} 時間 = {19:n0} 円</td></tr>" +
                    "<tr><td class='text-right'>合計</td><td class='text-right'>{20:n0} 円</td><td class='text-right'>{21:n0} 円</td></tr></table></p>";


            return
                string.Format(tooltip, maxUnitPriceAfterDiscount, minUnitPrice, maxUnitBusFeeAfterDiscount, minUnitBusFee,
                minMaxForm.SelectedTranSportationOfficePlace.TransportationPlaceName,
                minMaxForm.getTypeBusName(),
                minMaxForm.getDisCountRate(),
                minMaxForm.SpecialVehicleOption == 0 ? "なし" : "あり",
                maxUnitPriceForKm, sumOfAllKm, exactMaxUnitPriceForKm,
                maxUnitPriceForHour, sumOfAllTime, exactMaxUnitPriceForHour,
                minUnitPriceForKm, sumOfAllKm, exactMinUnitPriceForKm,
                minUnitPriceForHour, sumOfAllTime, exactMinUnitPriceForHour,
                maxUnitPrice, minUnitPrice
                );
        }

        /// <summary>
        /// #6621 fix bug null transportation fee rule will return tooltip empty
        /// </summary>
        /// <returns></returns>
        public string getTooltipForUnitBusFee()
        {
            if (this.minMaxForm == null || this.minMaxForm.typeBus == -1 || this.minMaxForm.SelectedTranSportationOfficePlace == null) // Case the min-max popup is not setting yet
            {
                return "";
            }
            int maxUnitBusFeeForKmWithChgDriver, minUnitBusFeeForKmWithChgDriver, maxUnitBusFeeforHourWithChgDriver, minUnitBusFeeforHourWithChgDriver,
                maxUnitBusSumFeeforMid9, minUnitBusSumFeeforMid9,
                sumOfAllKm, sumOfAllTime, maxUnitBusFee, maxUnitBusFeeAfterDiscount, minUnitBusFee,
                maxUnitPrice, maxUnitPriceAfterDiscount, minUnitPrice, exactMaxUnitBusFeeForKmWithChgDriver,
                exactMinUnitBusFeeForKmWithChgDriver, exactMaxUnitBusFeeforHourWithChgDriver, exactMinUnitBusFeeforHourWithChgDriver,
                maxSpecialVehicalFee, minSpecialVehicalFee, maxUnitBusFeeWithChgDriver;

            string maxSpecialVehicalFeeFomula, minSpecialVehicalFeeFomula;
            string maxUnitBusSumFeeforMid9Fomula, minUnitBusSumFeeforMid9Fomula;

            maxUnitBusFeeForKmWithChgDriver = minMaxForm.SelectedTranSportationOfficePlace.ChangeDriverMaxUnitPriceforKm;
            minUnitBusFeeForKmWithChgDriver = minMaxForm.SelectedTranSportationOfficePlace.ChangeDriverMinUnitPriceforKm;
            maxUnitBusFeeforHourWithChgDriver = minMaxForm.SelectedTranSportationOfficePlace.ChangeDriverMaxUnitPriceforHour;
            minUnitBusFeeforHourWithChgDriver = minMaxForm.SelectedTranSportationOfficePlace.ChangeDriverMinUnitPriceforHour;
            maxSpecialVehicalFee = minMaxForm.getMaxSpecialVehicalFee(out maxSpecialVehicalFeeFomula);
            minSpecialVehicalFee = minMaxForm.getMinSpecialVehicalFee(out minSpecialVehicalFeeFomula);
            maxUnitBusSumFeeforMid9 = minMaxForm.getMaxUnitBusSumFeeforMid9(out maxUnitBusSumFeeforMid9Fomula);
            minUnitBusSumFeeforMid9 = minMaxForm.getMinUnitBusSumFeeforMid9(out minUnitBusSumFeeforMid9Fomula);

            maxUnitPrice = minMaxForm.getMaxUnitPriceForkm() + minMaxForm.getMaxUnitPriceForHour();
            maxUnitPriceAfterDiscount = minMaxForm.getMaxUnitBusPriceDiscount();
            minUnitPrice = minMaxForm.getMinUnitPriceForkm() + minMaxForm.getMinUnitPriceForHour();
            sumOfAllKm = BookingInputHelper.Round(minMaxForm.minMaxGridData.Sum(t => t.KmRunningwithChgDriver));
            BookingInputHelper.MyTime sumOfAllTimeMyTime = BookingInputHelper.Round(minMaxForm.minMaxGridData.Aggregate<MinMaxGridData,
                               BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunningwithChgDriver));
            sumOfAllTime = sumOfAllTimeMyTime.myHour;
            //sumOfAllTime = int.Parse(sumOfAllTimeMyTime.ToString().Substring(0, 2));
            maxUnitBusFee = minMaxForm.getMaxUnitBusFeeForKmWithChgDriver() + minMaxForm.getMaxUnitBusFeeforHourWithChgDriver() +
                minMaxForm.getMaxUnitBusSumFeeforMid9() + minMaxForm.getMaxSpecialVehicalFee();
            maxUnitBusFeeAfterDiscount = minMaxForm.getMaxUnitBusFeeDiscount();
            minUnitBusFee = minMaxForm.getMinUnitBusFeeForKmWithChgDriver() + minMaxForm.getMinUnitBusFeeforHourWithChgDriver() +
                minMaxForm.getMinUnitBusSumFeeforMid9() + minMaxForm.getMinSpecialVehicalFee();

            exactMaxUnitBusFeeForKmWithChgDriver = sumOfAllKm * maxUnitBusFeeForKmWithChgDriver;
            exactMinUnitBusFeeForKmWithChgDriver = sumOfAllKm * minUnitBusFeeForKmWithChgDriver;
            exactMaxUnitBusFeeforHourWithChgDriver = sumOfAllTime * maxUnitBusFeeforHourWithChgDriver;
            exactMinUnitBusFeeforHourWithChgDriver = sumOfAllTime * minUnitBusFeeforHourWithChgDriver;

            maxUnitBusFeeWithChgDriver = exactMaxUnitBusFeeForKmWithChgDriver + exactMaxUnitBusFeeforHourWithChgDriver;

            string tooltip = "<div class='text-left'><p>料金の上限下限金額の範囲参照" +
                "<table class='table table-sm text-center mb-2'>" +
                "<tr style='background: #f3f3f3'><td></td><td>上限（割引済）</td><td>下限</td></tr>" +
                "<tr><td>運賃</td><td>{0:n0} 円</td><td>{1:n0} 円</td></tr>" +
                "<tr><td><b>料金</b></td><td><b>{2:n0} 円</b></td><td><b>{3:n0} 円</b></td></tr></table>" +
                "&lt; 料金の内訳 &gt;<br/>" +
                "<table class='table table-sm text-center mb-2'>" +
                "<tr style='background: #f3f3f3'><td></td><td>上限</td><td>下限</td></tr>" +
                "<tr><td class='text-left'>交替運転手配置料金キロ制</td><td class='text-right'>{4:n0} 円</td><td class='text-right'>{5:n0} 円</td></tr>" +
                "<tr><td class='text-left'>交替運転手配置料金時間制</td><td class='text-right'>{6:n0} 円</td><td class='text-right'>{7:n0} 円</td></tr>" +
                "<tr><td class='text-left'>深夜早朝運行割増料金</td><td class='text-right'>{8:n0} 円</td><td class='text-right'>{9:n0} 円</td></tr><tr>" +
                "<td class='text-left'>特殊車両割増料金</td><td class='text-right'>{10:n0} 円</td><td class='text-right'>{11:n0} 円</td></tr>" +
                "<tr><td class='text-right'>合計</td><td class='text-right'>{12:n0} 円</td><td class='text-right'>{13:n0} 円</td></tr></table></p>";
            string tooltipFormat = string.Format(tooltip, maxUnitPriceAfterDiscount, minUnitPrice, maxUnitBusFeeAfterDiscount, minUnitBusFee,
                            exactMaxUnitBusFeeForKmWithChgDriver, exactMinUnitBusFeeForKmWithChgDriver,
                            exactMaxUnitBusFeeforHourWithChgDriver, exactMinUnitBusFeeforHourWithChgDriver,
                            maxUnitBusSumFeeforMid9, minUnitBusSumFeeforMid9,
                            maxSpecialVehicalFee, minSpecialVehicalFee,
                            maxUnitBusFee, minUnitBusFee);

            string changeDriverDetail = "<p>&lt; 交替運転手配置料金 内訳 &gt;<br />" +
                "・キロ制上限 = {0:n0} 円 x {1:n0} キロ = {2:n0} 円<br /> " +
                "・時間制上限 = {3:n0} 円 x {4:n0} 時間 = {5:n0} 円<br /> " +
                "・キロ制下限 = {6:n0} 円 x {7:n0} キロ = {8:n0} 円<br /> " +
                "・時間制下限 = {9:n0} 円 x {10:n0} 時間 = {11:n0} 円</p>";
            string changeDriverDetailFormat = string.Format(changeDriverDetail,
                            maxUnitBusFeeForKmWithChgDriver, sumOfAllKm, exactMaxUnitBusFeeForKmWithChgDriver,
                            maxUnitBusFeeforHourWithChgDriver, sumOfAllTime, exactMaxUnitBusFeeforHourWithChgDriver,
                            minUnitBusFeeForKmWithChgDriver, sumOfAllKm, exactMinUnitBusFeeForKmWithChgDriver,
                            minUnitBusFeeforHourWithChgDriver, sumOfAllTime, exactMinUnitBusFeeforHourWithChgDriver);

            string forMid9Detail = "";
            if (maxUnitBusSumFeeforMid9 != 0 && minUnitBusSumFeeforMid9 != 0)
            {
                forMid9Detail = "<p>&lt; 深夜早朝運行割増料金内訳 &gt;<br/>" + maxUnitBusSumFeeforMid9Fomula + minUnitBusSumFeeforMid9Fomula;
            }

            string specialVehicalDetail = "";
            if (maxSpecialVehicalFee != 0 && minSpecialVehicalFee != 0)
            {
                specialVehicalDetail = "<p>&lt; 特殊車両割増料金内訳 &gt;<br/>" + maxSpecialVehicalFeeFomula + minSpecialVehicalFeeFomula;
            }

            return tooltipFormat + changeDriverDetailFormat + forMid9Detail + specialVehicalDetail;
        }

        public string getTooltipForBranch()
        {
            return string.Format("{0}", PriorityAutoAssignBranch?.Text);
        }

        public MinMaxSettingFormData minMaxForm { get; set; } = new MinMaxSettingFormData();

        /// <summary>
        /// Priority auto assign bus for branch
        /// <para><c>null</c> => normal assign</para>
        /// <para><c>value</c> => <see cref="LoadSaleBranch"/> specified to assigned</para>
        /// </summary>
        public LoadSaleBranch PriorityAutoAssignBranch { get; set; }

        /// <summary>
        /// Check if MinMax of row is changed due to:
        /// BusType is changed
        /// TaxType for bus is changed
        /// TaxRate is Changed
        /// Bus schedule is changed
        /// Bus start date is changed
        /// #6582 change returnDateTime into converted state
        /// </summary>
        /// <returns>
        /// true - detail Min Max button green
        /// false - detail Min Max button red
        /// </returns>
        public bool CheckMinMaxChange(BookingFormData bookingdata)
        {
            float rateApplied = 0;
            if (bookingdata.TaxTypeforBus.IdValue == Constants.InTax.IdValue)
            {
                rateApplied = float.Parse(bookingdata.TaxRate);
            }
            var leaveDateTime = new BookingInputHelper.MyDate(bookingdata.ReservationTabData.GarageLeaveDate, bookingdata.ReservationTabData.SyuKoTime);
            var convertedDateTime = new BookingInputHelper.MyDate(bookingdata.ReservationTabData.GarageReturnDate, bookingdata.ReservationTabData.KikTime).ConvertedDate;
            var returnDateTime = new BookingInputHelper.MyDate(convertedDateTime, new BookingInputHelper.MyTime(convertedDateTime.Hour, convertedDateTime.Minute));
            BookingInputHelper.Scheduler scheduler = new BookingInputHelper.Scheduler(leaveDateTime, returnDateTime);
            bool isMinMaxNotSet = this.minMaxForm.typeBus == -1;
            bool isBusTypeIsNotSelected = this.busTypeData.Katakbn == "-1";
            bool isBusTypeNotChanged = this.minMaxForm.typeBus.ToString() == this.busTypeData.Katakbn;
            bool isTaxTypeNotChanged = ((this.minMaxForm.TaxType.IdValue == Constants.InTax.IdValue) == (bookingdata.TaxTypeforBus.IdValue == Constants.InTax.IdValue));
            bool isTaxRateNotChanged = this.minMaxForm.TaxRate == rateApplied;
            bool isScheduleNotChanged = this.minMaxForm.minMaxGridData.Count == scheduler.getRowforGrid();
            bool isStartNotChanged = this.minMaxForm.BusStartTime?.ConvertedDate.ToString(Formats.yyyyMMddHHmm)
                == new BookingInputHelper.MyDate(bookingdata.BusStartDate, bookingdata.BusStartTime).ConvertedDate.ToString(Formats.yyyyMMddHHmm);
            bool isEndNotChanged = this.minMaxForm.BusArrivalTime?.ConvertedDate.ToString(Formats.yyyyMMddHHmm)
                == new BookingInputHelper.MyDate(bookingdata.BusEndDate, bookingdata.BusEndTime).ConvertedDate.ToString(Formats.yyyyMMddHHmm);
            return ((isTaxTypeNotChanged && isTaxRateNotChanged && isBusTypeNotChanged && isScheduleNotChanged && isStartNotChanged && isEndNotChanged)
                        || isMinMaxNotSet || isBusTypeIsNotSelected);
        }

        private double unitPriceIndex = 0;
        public string UnitPriceIndex
        {
            get
            {
                return unitPriceIndex.ToString();
            }
            set
            {
                this.unitPriceIndex = double.Parse(value);
            }
        }

        public int BusNumNippoKbn { get; set; }
    }
}
