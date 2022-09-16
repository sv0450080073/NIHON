using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Queries
{
    public class GetMaxMinFareFeeCalcByUkeno : IRequest<Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalc>>>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }
        public class Handler : IRequestHandler<GetMaxMinFareFeeCalcByUkeno, Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalc>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetMaxMinFareFeeCalcByUkeno> _logger;
            public Handler(KobodbContext context, ILogger<GetMaxMinFareFeeCalcByUkeno> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalc>>> Handle(GetMaxMinFareFeeCalcByUkeno request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalc>> result = new Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalc>>();
                    List<TkdBookingMaxMinFareFeeCalc> addNewFeeCalcList = new List<TkdBookingMaxMinFareFeeCalc>();
                    List<TkdBookingMaxMinFareFeeCalc> removeFeeCalcList = new List<TkdBookingMaxMinFareFeeCalc>();
                    List<TkdBookingMaxMinFareFeeCalc> updateFeeCalcList = new List<TkdBookingMaxMinFareFeeCalc>();
                    List<TkdBookingMaxMinFareFeeCalc> bookingMaxMinFareFeeCalsList = _context.TkdBookingMaxMinFareFeeCalc.Where(e => e.UkeNo == request.Ukeno && e.UnkRen == request.BookingData.UnkRen).ToList();
                    if (bookingMaxMinFareFeeCalsList != null)
                    {
                        // Case delete row
                        if (bookingMaxMinFareFeeCalsList.Count > 0)
                        {
                            int totalRecordMinMax = bookingMaxMinFareFeeCalsList.Max(t => t.SyaSyuRen);
                            for (int i = 1; i <= totalRecordMinMax; i++)
                            {
                                var bookingMaxMinFareFeeCals = bookingMaxMinFareFeeCalsList.FirstOrDefault(x => x.SyaSyuRen == i);
                                var vehicleTypeRow = request.BookingData.VehicleGridDataList.FirstOrDefault(x => x.RowID == i.ToString());
                                if (vehicleTypeRow == null)
                                {
                                    if (bookingMaxMinFareFeeCals != null)
                                    {
                                        bookingMaxMinFareFeeCalsList.Remove(bookingMaxMinFareFeeCals);
                                        removeFeeCalcList.Add(bookingMaxMinFareFeeCals);
                                    }
                                }
                            }
                        }
                        foreach (var vehicleTypeRow in request.BookingData.VehicleGridDataList)
                        {
                            MinMaxSettingFormData minMaxSettingFormData = vehicleTypeRow.minMaxForm;

                            if (minMaxSettingFormData.typeBus != -1)
                            {
                                var tkdBookingMaxMinFareFeeCals = bookingMaxMinFareFeeCalsList.
                                    FirstOrDefault(x => x.SyaSyuRen.ToString() == vehicleTypeRow.RowID);
                                // Case add row
                                if (tkdBookingMaxMinFareFeeCals == null)
                                {
                                    tkdBookingMaxMinFareFeeCals = new TkdBookingMaxMinFareFeeCalc();
                                    tkdBookingMaxMinFareFeeCals.UkeNo = request.Ukeno;
                                    tkdBookingMaxMinFareFeeCals.UnkRen = 1;
                                    tkdBookingMaxMinFareFeeCals.SyaSyuRen = Int16.Parse(vehicleTypeRow.RowID);
                                    addNewFeeCalcList.Add(tkdBookingMaxMinFareFeeCals);
                                }
                                else
                                {
                                    updateFeeCalcList.Add(tkdBookingMaxMinFareFeeCals);
                                }
                                // Case update row
                                tkdBookingMaxMinFareFeeCals.TransportationPlaceCodeSeq = minMaxSettingFormData.SelectedTranSportationOfficePlace.TransportationPlaceCodeSeq;
                                tkdBookingMaxMinFareFeeCals.KataKbn = (byte)minMaxSettingFormData.typeBus;
                                tkdBookingMaxMinFareFeeCals.ZeiKbn = (byte)minMaxSettingFormData.TaxType.IdValue;
                                tkdBookingMaxMinFareFeeCals.Zeiritsu = (decimal)minMaxSettingFormData.TaxRate;
                                tkdBookingMaxMinFareFeeCals.RunningKmSum = minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunning);
                                tkdBookingMaxMinFareFeeCals.RunningKmCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunning));
                                tkdBookingMaxMinFareFeeCals.RestraintTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.RestraintTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning)).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.ServiceKmSum = minMaxSettingFormData.minMaxGridData.Sum(t => t.ExactKmRunning);
                                tkdBookingMaxMinFareFeeCals.ServiceTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.ExactTimeRunning).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunning).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunning)).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverRunningKmSum = minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunningwithChgDriver);
                                tkdBookingMaxMinFareFeeCals.ChangeDriverRunningKmCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunningwithChgDriver));
                                tkdBookingMaxMinFareFeeCals.ChangeDriverRestraintTimeSum = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunningwithChgDriver)).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverRestraintTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning)).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverMidnightEarlyMorningTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunningwithChgDriver).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverMidnightEarlyMorningTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunningwithChgDriver)).ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCals.WaribikiKbn = (byte)minMaxSettingFormData.DiscountOption;
                                tkdBookingMaxMinFareFeeCals.AnnualContractFlag = (byte)minMaxSettingFormData.AnnualContractOption;
                                tkdBookingMaxMinFareFeeCals.SpecialFlg = (byte)minMaxSettingFormData.SpecialVehicleOption;
                                tkdBookingMaxMinFareFeeCals.FareMaxAmount = minMaxSettingFormData.getMaxUnitBusPriceDiscount();
                                tkdBookingMaxMinFareFeeCals.FareMinAmount = minMaxSettingFormData.getMinUnitBusPriceDiscount();
                                tkdBookingMaxMinFareFeeCals.FeeMaxAmount = minMaxSettingFormData.getMaxUnitBusFeeDiscount();
                                tkdBookingMaxMinFareFeeCals.FeeMinAmount = minMaxSettingFormData.getMinUnitBusFeeDiscount();
                                tkdBookingMaxMinFareFeeCals.UnitPriceMaxAmount = minMaxSettingFormData.getMaxUnitBusPriceDiscount() + minMaxSettingFormData.getMaxUnitBusFeeDiscount();
                                tkdBookingMaxMinFareFeeCals.UnitPriceMinAmount = minMaxSettingFormData.getMinUnitBusPriceDiscount() + minMaxSettingFormData.getMinUnitBusFeeDiscount();
                                tkdBookingMaxMinFareFeeCals.FareMaxAmountforKm = minMaxSettingFormData.getMaxUnitPriceForkm();
                                tkdBookingMaxMinFareFeeCals.FareMinAmountforKm = minMaxSettingFormData.getMinUnitPriceForkm();
                                tkdBookingMaxMinFareFeeCals.FareMaxAmountforHour = minMaxSettingFormData.getMaxUnitPriceForHour();
                                tkdBookingMaxMinFareFeeCals.FareMinAmountforHour = minMaxSettingFormData.getMinUnitPriceForHour();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverFareMaxAmountforKm = minMaxSettingFormData.getMaxUnitBusFeeForKmWithChgDriver();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverFareMinAmountforKm = minMaxSettingFormData.getMinUnitBusFeeForKmWithChgDriver();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverFareMaxAmountforHour = minMaxSettingFormData.getMaxUnitBusFeeforHourWithChgDriver();
                                tkdBookingMaxMinFareFeeCals.ChangeDriverFareMinAmountforHour = minMaxSettingFormData.getMinUnitBusFeeforHourWithChgDriver();
                                tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningFeeMaxAmount = minMaxSettingFormData.getMaxUnitBusSumFeeforMid9();
                                tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningFeeMinAmount = minMaxSettingFormData.getMinUnitBusSumFeeforMid9();
                                tkdBookingMaxMinFareFeeCals.SpecialVehicalFeeMaxAmount = minMaxSettingFormData.getMaxSpecialVehicalFee();
                                tkdBookingMaxMinFareFeeCals.SpecialVehicalFeeMinAmount = minMaxSettingFormData.getMinSpecialVehicalFee();
                                tkdBookingMaxMinFareFeeCals.UnitPriceIndex = decimal.Parse(vehicleTypeRow.UnitPriceIndex);
                                tkdBookingMaxMinFareFeeCals.FareIndex = decimal.Parse(minMaxSettingFormData.FareIndex);
                                tkdBookingMaxMinFareFeeCals.FeeIndex = decimal.Parse(minMaxSettingFormData.FeeIndex);
                                tkdBookingMaxMinFareFeeCals.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                tkdBookingMaxMinFareFeeCals.UpdTime = DateTime.Now.ToString("HHmmss");
                                tkdBookingMaxMinFareFeeCals.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                tkdBookingMaxMinFareFeeCals.UpdPrgId = "KJ1000";
                            }
                        }
                    }
                    result.Add(CommandMode.Create, addNewFeeCalcList);
                    result.Add(CommandMode.Update, updateFeeCalcList);
                    result.Add(CommandMode.Delete, removeFeeCalcList);
                    return Task.FromResult(result);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
