using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class MinMaxSettingFormData
    {
        public int typeBus { get; set; } = -1;

        private TaxTypeList taxType = new TaxTypeList { IdValue = -1, StringValue = string.Empty };
        public TaxTypeList TaxType
        {
            get => taxType;
            set
            {
                taxType.IdValue = value.IdValue;
                taxType.StringValue = value.StringValue;
            }
        }

        private float taxRateApplied = 0;
        public float TaxRate
        {
            get => taxRateApplied;
            set
            {
                taxRateApplied = taxType.IdValue == Constants.InTax.IdValue ? value : 0;
            }
        }

        public BookingInputHelper.MyDate GarageLeaveTime { get; set; }// 出庫日時
        public BookingInputHelper.MyDate BusStartTime { get; set; }//配車日時
        public BookingInputHelper.MyDate BusArrivalTime { get; set; } //到着日時
        public BookingInputHelper.MyDate GarageReturnTime { get; set; } //入庫日時

        private int unitPrice = 0;
        public string UnitPrice
        {
            get
            {
                return this.unitPrice.ToString();
            }
            set
            {
                this.unitPrice = int.Parse(value);
            }
        }

        /// <summary>
        /// Get the total kilometers running.
        /// </summary>
        public int TotalKmRunning 
        {
            get
            {
                int total = 0;
                minMaxGridData ??= new List<MinMaxGridData>();

                minMaxGridData.ForEach(item =>
                {
                    total += item.KmRunning;
                });

                return total;
            }
        }

        /// <summary>
        /// Get the total kilometers running.
        /// </summary>
        public int TotalExactKmRunning
        {
            get
            {
                int total = 0;
                minMaxGridData ??= new List<MinMaxGridData>();

                minMaxGridData.ForEach(item =>
                {
                    total += item.ExactKmRunning;
                });

                return total;
            }
        }

        /// <summary>
        /// Get the total kilometers running with change driver.
        /// </summary>
        public int TotalKmRunningWithChangeDriver
        {
            get
            {
                int total = 0;
                minMaxGridData ??= new List<MinMaxGridData>();

                minMaxGridData.ForEach(item =>
                {
                    total += item.KmRunningwithChgDriver;
                });

                return total;
            }
        }

        private int unitFee = 0;
        public string UnitFee
        {
            get
            {
                return this.unitFee.ToString();
            }
            set
            {
                this.unitFee = int.Parse(value);
            }
        }
        public int DiscountOption { get; set; } = 0;
        public int AnnualContractOption { get; set; } = 0;
        public int SpecialVehicleOption { get; set; } = 0;

        public int MaxUnitPriceForKm { get; set; } = 0;
        public int MinUnitPriceForKm { get; set; } = 0;

        public int MaxUnitPriceForHour { get; set; } = 0;
        public int MinUnitPriceForHour { get; set; } = 0;

        public int MaxUnitBusPriceDiscount { get; set; } = 0;
        public int MinUnitBusPriceDiscount { get; set; } = 0;

        public int MaxUnitBusFeeDiscount { get; set; } = 0;

        public int MinUnitBusFeeDiscount { get; set; } = 0;

        public int MaxUnitPriceDiscount { get; set; } = 0;

        public int MinUnitPriceDiscount { get; set; } = 0;

        public int MaxUnitBusFeeforKmWithChgDriver { get; set; } = 0;
        public int MinUnitBusFeeforKmrWithChgDriver { get; set; } = 0;

        public int MaxUnitBusFeeforHourWithChgDriver { get; set; } = 0;
        public int MinUnitBusFeeforHourWithChgDriver { get; set; } = 0;

        public int MaxSpecialVehicalFee { get; set; } = 0;
        public int MinSpecialVehicalFee { get; set; } = 0;

        public int MaxUnitBusSumFeeforMid9 { get; set; } = 0;
        public int MinUnitBusSumFeeforMid9 { get; set; } = 0;

        public VpmTransportationFeeRule SelectedTranSportationOfficePlace { get; set; }

        public List<MinMaxGridData> minMaxGridData { get; set; } = new List<MinMaxGridData>
        { // GRID
        };

        public MinMaxSettingFormData()
        {

        }

        public MinMaxSettingFormData(MinMaxSettingFormData old)
        {
            this.typeBus = old.typeBus;
            if (old.GarageLeaveTime != null)
            {
                this.GarageLeaveTime = new BookingInputHelper.MyDate(old.GarageLeaveTime);
            }
            if (old.BusStartTime != null)
            {
                this.BusStartTime = new BookingInputHelper.MyDate(old.BusStartTime);
            }
            if (old.BusArrivalTime != null)
            {
                this.BusArrivalTime = new BookingInputHelper.MyDate(old.BusArrivalTime);
            }
            if (old.GarageReturnTime != null)
            {
                this.GarageReturnTime = new BookingInputHelper.MyDate(old.GarageReturnTime);
            }
            this.UnitPrice = old.UnitPrice;
            this.DiscountOption = old.DiscountOption;
            this.AnnualContractOption = old.AnnualContractOption;
            this.SpecialVehicleOption = old.SpecialVehicleOption;
            this.MaxUnitPriceForKm = old.MaxUnitPriceForKm;
            this.MinUnitPriceForKm = old.MinUnitPriceForKm;
            this.MaxUnitPriceForHour = old.MaxUnitPriceForHour;
            this.MinUnitPriceForHour = old.MinUnitPriceForHour;
            this.MaxUnitBusPriceDiscount = old.MaxUnitBusPriceDiscount;
            this.MinUnitBusPriceDiscount = old.MinUnitBusPriceDiscount;
            this.MaxUnitBusFeeDiscount = old.MaxUnitBusFeeDiscount;
            this.MinUnitBusFeeDiscount = old.MinUnitBusFeeDiscount;
            this.MaxUnitPriceDiscount = old.MaxUnitPriceDiscount;
            this.MinUnitPriceDiscount = old.MinUnitPriceDiscount;
            this.MaxUnitBusFeeforKmWithChgDriver = old.MaxUnitBusFeeforKmWithChgDriver;
            this.MinUnitBusFeeforKmrWithChgDriver = old.MinUnitBusFeeforKmrWithChgDriver;
            this.MaxUnitBusFeeforHourWithChgDriver = old.MaxUnitBusFeeforHourWithChgDriver;
            this.MinUnitBusFeeforHourWithChgDriver = old.MinUnitBusFeeforHourWithChgDriver;
            this.MaxSpecialVehicalFee = old.MaxSpecialVehicalFee;
            this.MinSpecialVehicalFee = old.MinSpecialVehicalFee;
            this.MaxUnitBusSumFeeforMid9 = old.MaxUnitBusSumFeeforMid9;
            this.MinUnitBusSumFeeforMid9 = old.MinUnitBusSumFeeforMid9;
            this.TaxType = old.TaxType;
            this.TaxRate = old.TaxRate;
            this.FareIndex = old.FareIndex;
            this.FeeIndex = old.FeeIndex;

            this.SelectedTranSportationOfficePlace = old.SelectedTranSportationOfficePlace;

            this.minMaxGridData = new List<MinMaxGridData> { };
            foreach (var item in old.minMaxGridData)
            {
                this.minMaxGridData.Add(new MinMaxGridData(item));
            }
        }

        public BookingInputHelper.MyTime getExactTimeRunning()
        {
            BookingInputHelper.MyTime tmp = new BookingInputHelper.MyTime();
            if (this.minMaxGridData.Count == 1)
            {
                tmp = this.BusArrivalTime - this.BusStartTime;
            }
            return tmp;
        }

        public string getTypeBusName()
        {
            switch (typeBus)
            {
                case 1:
                    return "大型";
                case 2:
                    return "中型";
                case 3:
                    return "小型";
                default:
                    return "未特定";
            }
        }

        /// <summary>
        /// #6621 fix bug null transportation fee rule will return 0
        /// </summary>
        /// <returns></returns>
        public int getMasterMaxUnitPriceForKm()
        {
            if (this.SelectedTranSportationOfficePlace == null)
            {
                return 0;
            }
            switch (typeBus)
            {
                case 1:
                    return this.SelectedTranSportationOfficePlace.BigVehicalMaxUnitPriceforKm;
                case 2:
                    return this.SelectedTranSportationOfficePlace.MedVehicalMaxUnitPriceforKm;
                default:
                    return this.SelectedTranSportationOfficePlace.SmallVehicalMaxUnitPriceforKm;
            }
        }

        public int getMaxUnitPriceForkm()
        {
            int roundSumKmRunning = BookingInputHelper.Round(this.minMaxGridData.Sum(t => t.KmRunning));
            int maxUnitPricefrKm = 0;
            if (this.SelectedTranSportationOfficePlace != null)
            {
                maxUnitPricefrKm = roundSumKmRunning * getMasterMaxUnitPriceForKm();
            }
            this.MaxUnitPriceForKm = maxUnitPricefrKm;
            return this.MaxUnitPriceForKm;
        }

        /// <summary>
        /// #6621 fix bug null transportation fee rule will return 0
        /// </summary>
        /// <returns></returns>
        public int getMasterMinUnitPriceForKm()
        {
            if (this.SelectedTranSportationOfficePlace == null)
            {
                return 0;
            }
            switch (typeBus)
            {
                case 1:
                    return this.SelectedTranSportationOfficePlace.BigVehicalMinUnitPriceforKm;
                case 2:
                    return this.SelectedTranSportationOfficePlace.MedVehicalMinUnitPriceforKm;
                default:
                    return this.SelectedTranSportationOfficePlace.SmallVehicalMinUnitPriceforKm;
            }
        }

        public int getMinUnitPriceForkm()
        {
            int roundSumKmRunning = BookingInputHelper.Round(this.minMaxGridData.Sum(t => t.KmRunning));
            int minUnitPricefrKm = 0;
            if (this.SelectedTranSportationOfficePlace != null)
            {
                minUnitPricefrKm = roundSumKmRunning * getMasterMinUnitPriceForKm();
            }
            this.MinUnitPriceForKm = minUnitPricefrKm;
            return this.MinUnitPriceForKm;
        }

        /// <summary>
        /// #6621 fix bug null transportation fee rule will return 0
        /// </summary>
        /// <returns></returns>
        public int getMasterMaxUnitPriceForHour()
        {
            if (this.SelectedTranSportationOfficePlace == null)
            {
                return 0;
            }
            switch (typeBus)
            {
                case 1:
                    return this.SelectedTranSportationOfficePlace.BigVehicalMaxUnitPriceforHour;
                case 2:
                    return this.SelectedTranSportationOfficePlace.MedVehicalMaxUnitPriceforHour;
                default:
                    return this.SelectedTranSportationOfficePlace.SmallVehicalMaxUnitPriceforHour;
            }
        }

        public int getMaxUnitPriceForHour()
        {
            int maxUnitPricefrHr = 0;

            BookingInputHelper.MyTime roundSumTimeRunning = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning));
            if (this.SelectedTranSportationOfficePlace != null)
            {
                maxUnitPricefrHr = roundSumTimeRunning.myHour * getMasterMaxUnitPriceForHour();
            }

            this.MaxUnitPriceForHour = maxUnitPricefrHr;
            return this.MaxUnitPriceForHour;
        }

        /// <summary>
        /// #6621 fix bug null transportation fee rule will return 0
        /// </summary>
        /// <returns></returns>
        public int getMasterMinUnitPriceForHour()
        {
            if (this.SelectedTranSportationOfficePlace == null)
            {
                return 0;
            }
            switch (typeBus)
            {
                case 1:
                    return this.SelectedTranSportationOfficePlace.BigVehicalMinUnitPriceforHour;
                case 2:
                    return this.SelectedTranSportationOfficePlace.MedVehicalMinUnitPriceforHour;
                default:
                    return this.SelectedTranSportationOfficePlace.SmallVehicalMinUnitPriceforHour;
            }
        }

        public int getMinUnitPriceForHour()
        {
            int minUnitPricefrHr = 0;

            BookingInputHelper.MyTime roundSumTimeRunning = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning));
            if (this.SelectedTranSportationOfficePlace != null)
            {
                minUnitPricefrHr = roundSumTimeRunning.myHour * getMasterMinUnitPriceForHour();
            }

            this.MinUnitPriceForHour = minUnitPricefrHr;
            return this.MinUnitPriceForHour;
        }

        public int getDisCountRate()
        {
            int rate = 0;
            switch (this.DiscountOption)
            {
                case 0:
                    rate = 0;
                    break;
                case 1:
                    rate = 30;
                    break;
                case 2:
                    rate = 20;
                    break;
            }
            return rate;
        }

        /*
         * Return value after round-up
         * Ticket 610 if max price < min price => max price = min price
         */
        public int getMaxUnitBusPriceDiscount()
        {
            int MaxUnitBusPrice = this.getMaxUnitPriceForkm() + this.getMaxUnitPriceForHour();
            double rate = (100 - getDisCountRate()) / 100.0;
            double tax = 1;
            if (TaxType.IdValue == Constants.InTax.IdValue)
            {
                tax = (100 + TaxRate) / 100;
            }
            this.MaxUnitBusPriceDiscount = (int)Math.Ceiling(MaxUnitBusPrice * rate * tax);
            if (this.MaxUnitBusPriceDiscount < getMinUnitBusPriceDiscount())
            {
                this.MaxUnitBusPriceDiscount = getMinUnitBusPriceDiscount();
            }
            return this.MaxUnitBusPriceDiscount;
        }

        /*
         * Return value after round-up
         */
        public int getMinUnitBusPriceDiscount()
        {
            int MinUnitBusPrice = this.getMinUnitPriceForkm() + this.getMinUnitPriceForHour();
            double rate = 1;
            double tax = 1;
            if (TaxType.IdValue == Constants.InTax.IdValue)
            {
                tax = (100 + TaxRate) / 100;
            }
            this.MinUnitBusPriceDiscount = (int)Math.Ceiling(MinUnitBusPrice * rate * tax);
            return this.MinUnitBusPriceDiscount;
        }

        /*
         * Return value after round-up
         * #677 Discount is not applied.
         */
        public int getMaxUnitBusFeeDiscount()
        {
            int sum = this.getMaxUnitBusFeeForKmWithChgDriver() + this.getMaxUnitBusFeeforHourWithChgDriver() +
                          this.getMaxUnitBusSumFeeforMid9() + this.getMaxSpecialVehicalFee();
            double rate = 1;
            double tax = 1;
            if (TaxType.IdValue == Constants.InTax.IdValue)
            {
                tax = (100 + TaxRate) / 100;
            }
            this.MaxUnitBusFeeDiscount = (int)Math.Ceiling(sum * rate * tax);
            return this.MaxUnitBusFeeDiscount;
        }

        /*
         * Return value after round-up
         * #677 Discount is not applied.
         */
        public int getMinUnitBusFeeDiscount()
        {
            int sum = this.getMinUnitBusFeeForKmWithChgDriver() + this.getMinUnitBusFeeforHourWithChgDriver() +
                    this.getMinUnitBusSumFeeforMid9() + this.getMinSpecialVehicalFee();
            double rate = 1;
            double tax = 1;
            if (TaxType.IdValue == Constants.InTax.IdValue)
            {
                tax = (100 + TaxRate) / 100;
            }
            this.MinUnitBusFeeDiscount = (int)Math.Ceiling(sum * rate * tax);
            return this.MinUnitBusFeeDiscount;
        }

        public int getMaxUnitPriceDiscount()
        {
            return this.MaxUnitPriceDiscount = this.MaxUnitBusPriceDiscount + this.MaxUnitBusFeeDiscount;
        }

        public int getMinUnitPriceDiscount()
        {
            return this.MinUnitPriceDiscount = this.MinUnitBusPriceDiscount + this.MinUnitBusFeeDiscount;
        }
        public int getMaxUnitBusFeeForKmWithChgDriver()
        {
            int roundKmRunningWithChgDriver = BookingInputHelper.Round(minMaxGridData.Sum(t => t.KmRunningwithChgDriver));
            if (this.SelectedTranSportationOfficePlace != null)
            {
                this.MaxUnitBusFeeforKmWithChgDriver = roundKmRunningWithChgDriver * this.SelectedTranSportationOfficePlace.ChangeDriverMaxUnitPriceforKm;
            }
            return this.MaxUnitBusFeeforKmWithChgDriver;

        }

        public int getMinUnitBusFeeForKmWithChgDriver()
        {
            int roundKmRunningWithChgDriver = BookingInputHelper.Round(minMaxGridData.Sum(t => t.KmRunningwithChgDriver));
            if (this.SelectedTranSportationOfficePlace != null)
            {
                this.MinUnitBusFeeforKmrWithChgDriver = roundKmRunningWithChgDriver * this.SelectedTranSportationOfficePlace.ChangeDriverMinUnitPriceforKm;
            }
            return this.MinUnitBusFeeforKmrWithChgDriver;
        }

        public int getMaxUnitBusFeeforHourWithChgDriver()
        {

            BookingInputHelper.MyTime roundSumTimeRunningWithChangeDriver = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunningwithChgDriver));
            if (this.SelectedTranSportationOfficePlace != null)
            {
                this.MaxUnitBusFeeforHourWithChgDriver = roundSumTimeRunningWithChangeDriver.myHour * this.SelectedTranSportationOfficePlace.ChangeDriverMaxUnitPriceforHour;
            }
            return this.MaxUnitBusFeeforHourWithChgDriver;
        }

        public int getMinUnitBusFeeforHourWithChgDriver()
        {

            BookingInputHelper.MyTime roundSumTimeRunningWithChangeDriver = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunningwithChgDriver));
            if (this.SelectedTranSportationOfficePlace != null)
            {
                this.MinUnitBusFeeforHourWithChgDriver = roundSumTimeRunningWithChangeDriver.myHour * this.SelectedTranSportationOfficePlace.ChangeDriverMinUnitPriceforHour;
            }
            return this.MinUnitBusFeeforHourWithChgDriver;
        }

        public int getMaxSpecialVehicalFee()
        {
            string Formula;
            return this.getMaxSpecialVehicalFee(out Formula);
        }

        /*
         * Return value after round-up
         * TODO refactor code => make 'applyDiscount', 'applyTax', 'roundUp' function
         */
        public int getMaxSpecialVehicalFee(out string Formula)
        {
            Formula = "・増料金上限 = {0:n0} 円 x {1:n0} % = {2:n0} 円<br /> ";
            if (this.SelectedTranSportationOfficePlace != null)
            {
                double rate = (100 - getDisCountRate()) / 100.0;
                var maxUnitBusPrice = (int)Math.Ceiling((this.getMaxUnitPriceForkm() + this.getMaxUnitPriceForHour()) * rate);
                this.MaxSpecialVehicalFee = (int)Math.Ceiling(maxUnitBusPrice * this.SpecialVehicleOption * this.SelectedTranSportationOfficePlace.SpecialVehicalMaxRate / 100.0);
                Formula = String.Format(Formula, getMaxUnitBusPriceDiscount(), this.SelectedTranSportationOfficePlace.SpecialVehicalMaxRate, this.MaxSpecialVehicalFee);
            }
            return this.MaxSpecialVehicalFee;
        }

        public int getMinSpecialVehicalFee()
        {
            string Formula;
            return this.getMinSpecialVehicalFee(out Formula);
        }

        /*
         * Return value after round-up
         * TODO refactor code => make 'applyDiscount', 'applyTax', 'roundUp' function
         */
        public int getMinSpecialVehicalFee(out string Formula)
        {
            Formula = "・増料金下限 = {0:n0} 円 x {1:n0} % = {2:n0} 円<br /> ";

            if (this.SelectedTranSportationOfficePlace != null)
            {
                double rate = 1; // no discount
                var minUnitBusPrice = (int)Math.Ceiling((this.getMinUnitPriceForkm() + this.getMinUnitPriceForHour()) * rate);
                this.MinSpecialVehicalFee = (int)Math.Ceiling(minUnitBusPrice * this.SpecialVehicleOption * this.SelectedTranSportationOfficePlace.SpecialVehicalMinRate / 100.0);
                Formula = String.Format(Formula, getMinUnitBusPriceDiscount(), this.SelectedTranSportationOfficePlace.SpecialVehicalMinRate, this.MinSpecialVehicalFee);
            }
            return this.MinSpecialVehicalFee;
        }

        public int getMaxUnitBusSumFeeforMid9()
        {
            string formula;
            return getMaxUnitBusSumFeeforMid9(out formula);
        }
        /*
         * Return value after round-up
         */
        public int getMaxUnitBusSumFeeforMid9(out string Formula)
        {
            Formula = "・増料金上限 = {0:n0} 円 x {1:n0} 時間 x {2:n0} %";
            BookingInputHelper.MyTime roundSpecialTimeRunning = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunning));
            BookingInputHelper.MyTime roundSpecialTimeRunningwithChgDriver = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunningwithChgDriver));

            if (this.SelectedTranSportationOfficePlace != null)
            {
                this.MaxUnitBusSumFeeforMid9 = (int)Math.Ceiling((roundSpecialTimeRunning.myHour * getMasterMaxUnitPriceForHour() * this.SelectedTranSportationOfficePlace.RunningMid9MaxRate / 100.0)
                            + (roundSpecialTimeRunningwithChgDriver.myHour * this.SelectedTranSportationOfficePlace.ChangeDriverMaxUnitPriceforHour * this.SelectedTranSportationOfficePlace.RunningMid9MaxRate / 100.0));

                Formula = String.Format(Formula, getMasterMaxUnitPriceForHour(), roundSpecialTimeRunning.myHour, this.SelectedTranSportationOfficePlace.RunningMid9MaxRate);
                if (roundSpecialTimeRunningwithChgDriver.myHour != 0)
                {
                    string strWithChangeDriver = " + {0:n0} 円 x {1:n0} 時間 x {2:n0} %";
                    Formula += String.Format(strWithChangeDriver, this.SelectedTranSportationOfficePlace.ChangeDriverMaxUnitPriceforHour, roundSpecialTimeRunningwithChgDriver.myHour, this.SelectedTranSportationOfficePlace.RunningMid9MaxRate);
                }
                Formula += String.Format(" = {0:n0} 円<br /> ", this.MaxUnitBusSumFeeforMid9);
            }
            return this.MaxUnitBusSumFeeforMid9;
        }

        public int getMinUnitBusSumFeeforMid9()
        {
            string formula;
            return getMinUnitBusSumFeeforMid9(out formula);
        }
        /*
         * Return value after round-up
         */
        public int getMinUnitBusSumFeeforMid9(out string Formula)
        {
            Formula = "・増料金下限 = {0:n0} 円 x {1:n0} 時間 x {2:n0} %";
            BookingInputHelper.MyTime roundSpecialTimeRunning = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunning));
            BookingInputHelper.MyTime roundSpecialTimeRunningwithChgDriver = BookingInputHelper.Round(minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunningwithChgDriver));

            if (this.SelectedTranSportationOfficePlace != null)
            {
                this.MinUnitBusSumFeeforMid9 = (int)Math.Ceiling((roundSpecialTimeRunning.myHour * getMasterMinUnitPriceForHour() * this.SelectedTranSportationOfficePlace.RunningMid9MinRate / 100.0)
                    + (roundSpecialTimeRunningwithChgDriver.myHour * this.SelectedTranSportationOfficePlace.ChangeDriverMinUnitPriceforHour * this.SelectedTranSportationOfficePlace.RunningMid9MinRate / 100.0));

                Formula = String.Format(Formula, getMasterMinUnitPriceForHour(), roundSpecialTimeRunning.myHour, this.SelectedTranSportationOfficePlace.RunningMid9MinRate);
                if (roundSpecialTimeRunningwithChgDriver.myHour != 0)
                {
                    string strWithChangeDriver = " + {0:n0} 円 x {1:n0} 時間 x {2:n0} %";
                    Formula += String.Format(strWithChangeDriver, this.SelectedTranSportationOfficePlace.ChangeDriverMinUnitPriceforHour, roundSpecialTimeRunningwithChgDriver.myHour, this.SelectedTranSportationOfficePlace.RunningMid9MinRate);
                }
                Formula += String.Format(" = {0:n0} 円<br /> ", this.MinUnitBusSumFeeforMid9);
            }

            return this.MinUnitBusSumFeeforMid9;
        }

        private double fareIndex = 0;
        public string FareIndex
        {
            get
            {
                return this.fareIndex.ToString();
            }
            set
            {
                this.fareIndex = double.Parse(value);
            }
        }
        private double feeIndex = 0;
        public string FeeIndex
        {
            get
            {
                return this.feeIndex.ToString();
            }
            set
            {
                this.feeIndex = double.Parse(value);
            }
        }

        public void CalculateFareIndex()
        {
            int tmp;
            if (int.TryParse(UnitPrice, out tmp) ||
                int.TryParse(UnitPrice.Normalize(System.Text.NormalizationForm.FormKC), out tmp))
            { // Convert from Full-width to Haft-width
                int min = getMinUnitBusPriceDiscount();
                int max = getMaxUnitBusPriceDiscount();
                var index = Math.Round(((double)((tmp - min) / ((max - min) / 100.0))), 1);
                FareIndex = index < 0 ? "0" : index.ToString();
            }
        }

        public void CalculateFeeIndex()
        {
            int tmp;
            if (int.TryParse(UnitFee, out tmp) ||
                int.TryParse(UnitFee.Normalize(System.Text.NormalizationForm.FormKC), out tmp))
            { // Convert from Full-width to Haft-width
                int min = getMinUnitBusFeeDiscount();
                int max = getMaxUnitBusFeeDiscount();
                if (max - min == 0)
                {
                    FeeIndex = "0";
                }
                else
                {
                    FeeIndex = Math.Round(((double)((tmp - min) / ((max - min) / 100.0))), 1).ToString();
                }
            }
        }

        public void CalculateFareFromIndex()
        {
            int min = getMinUnitBusPriceDiscount();
            int max = getMaxUnitBusPriceDiscount();
            UnitPrice = Math.Round(((max - min) / 100.0 * double.Parse(FareIndex) + min)).ToString();
        }

        public void CalculateFeeFromIndex()
        {
            int min = getMinUnitBusFeeDiscount();
            int max = getMaxUnitBusFeeDiscount();
            UnitFee = Math.Round(((max - min) / 100.0 * double.Parse(FeeIndex) + min)).ToString();
        }
    }
}
