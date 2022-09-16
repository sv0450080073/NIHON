using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetVehicleGridDataQuery : IRequest<IEnumerable<VehicleGridData>>
    {
        private readonly string _ukeNo;

        public GetVehicleGridDataQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetVehicleGridDataQuery, IEnumerable<VehicleGridData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetVehicleGridDataQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetVehicleGridDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<IEnumerable<VehicleGridData>> Handle(GetVehicleGridDataQuery request, CancellationToken cancellationToken)
            {
                List<VehicleGridData> vehicleGridData = new List<VehicleGridData>();
                List<TkdYykSyu> tkdYykSyuList = _context.TkdYykSyu.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1).ToList();
                List<TkdBookingMaxMinFareFeeCalc> tkdBookingMaxMinFareFeeCalcList =
                    _context.TkdBookingMaxMinFareFeeCalc.Where(x => x.UkeNo == request._ukeNo).ToList();
                List<TkdHaisha> tkdHaishaList = _context.TkdHaisha.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1).ToList();
                foreach (TkdYykSyu yykSyu in tkdYykSyuList)
                {
                    var tkdBookingMaxMinFareFeeCalc =
                        tkdBookingMaxMinFareFeeCalcList.FirstOrDefault(x => x.SyaSyuRen == yykSyu.SyaSyuRen);
                    var HasNippokbn = tkdHaishaList.Where(h => h.SyaSyuRen == yykSyu.SyaSyuRen && h.NippoKbn == 2).Any();
                    vehicleGridData.Add(new VehicleGridData()
                    {
                        RowID = yykSyu.SyaSyuRen.ToString(),
                        busTypeData = new BusTypeData { Katakbn = yykSyu.KataKbn.ToString(), SyaSyuCdSeq = yykSyu.SyaSyuCdSeq },
                        BusNum = yykSyu.SyaSyuDai.ToString(),
                        UnitPrice = yykSyu.SyaSyuTan.ToString(),
                        BusPrice = yykSyu.SyaRyoUnc.ToString(),
                        DriverNum = yykSyu.DriverNum.ToString(),
                        UnitBusPrice = yykSyu.UnitBusPrice.ToString(),
                        UnitBusFee = yykSyu.UnitBusFee.ToString(),
                        GuiderNum = yykSyu.GuiderNum.ToString(),
                        UnitGuiderFee = yykSyu.UnitGuiderPrice.ToString(),
                        GuiderFee = yykSyu.UnitGuiderFee.ToString(),
                        PriorityAutoAssignBranch = CollectKarieiPoupData(request._ukeNo, yykSyu.SyaSyuRen.ToString()),
                        UnitPriceIndex = tkdBookingMaxMinFareFeeCalc?.UnitPriceIndex.ToString() ?? "0",
                        // Get data for 予約引受書
                        minMaxForm = CollectMinMaxSettingFormData(tkdBookingMaxMinFareFeeCalc, request._ukeNo, yykSyu.SyaSyuRen.ToString()),
                        BusNumNippoKbn = HasNippokbn ? yykSyu.SyaSyuDai : 0
                    });
                }
                return Task.FromResult(vehicleGridData.AsEnumerable());
            }

            /// <summary>
            /// Collect data fro priority auto assign for branch
            /// </summary>
            /// <param name="ukeNo"></param>
            /// <param name="syaSyuRen">Row id</param>
            /// <returns></returns>
            private LoadSaleBranch CollectKarieiPoupData(string ukeNo, string syaSyuRen)
            {
                TkdKariei kariei = _context.TkdKariei.FirstOrDefault(_ => _.UkeNo == ukeNo && _.SyaSyuRen.ToString() == syaSyuRen);

                if(kariei != null)
                {
                    var branch = _context.VpmEigyos.FirstOrDefault(_ => _.EigyoCdSeq == kariei.KseigSeq);

                    if(branch != null)
                    {
                        return new LoadSaleBranch
                        {
                            EigyoCdSeq = branch.EigyoCdSeq,
                            EigyoCd = branch.EigyoCd,
                            EigyoName = branch.EigyoNm
                        };
                    }
                    return null;
                }

                return null;
            }

            /// <summary>
            /// Get data for 予約引受書
            /// </summary>
            /// <param name="args"></param>
            private MinMaxSettingFormData CollectMinMaxSettingFormData(TkdBookingMaxMinFareFeeCalc tkdBookingMaxMinFareFeeCalc, string ukeNo, string syaSyuRen)
            {
                TkdUnkobi tkdUnkobi = _context.TkdUnkobi.Where(x => x.UkeNo == ukeNo).FirstOrDefault();
                var minMaxSettingFormData = new MinMaxSettingFormData()
                {
                    GarageLeaveTime = CollectMyDateFromString(tkdUnkobi.SyukoYmd, tkdUnkobi.SyuKoTime),
                    BusStartTime = CollectMyDateFromString(tkdUnkobi.HaiSymd, tkdUnkobi.HaiStime),
                    BusArrivalTime = CollectMyDateFromString(tkdUnkobi.TouYmd, tkdUnkobi.TouChTime),
                    GarageReturnTime = CollectMyDateFromString(tkdUnkobi.KikYmd, tkdUnkobi.KikTime)
                };

                if (tkdBookingMaxMinFareFeeCalc != null)
                {
                    minMaxSettingFormData.typeBus = tkdBookingMaxMinFareFeeCalc.KataKbn;
                    minMaxSettingFormData.TaxType.IdValue = tkdBookingMaxMinFareFeeCalc.ZeiKbn;
                    minMaxSettingFormData.TaxRate = (float)tkdBookingMaxMinFareFeeCalc.Zeiritsu;
                    minMaxSettingFormData.DiscountOption = tkdBookingMaxMinFareFeeCalc.WaribikiKbn;
                    minMaxSettingFormData.AnnualContractOption = tkdBookingMaxMinFareFeeCalc.AnnualContractFlag;
                    minMaxSettingFormData.SpecialVehicleOption = tkdBookingMaxMinFareFeeCalc.SpecialFlg;
                    minMaxSettingFormData.MaxUnitPriceForKm = tkdBookingMaxMinFareFeeCalc.FareMaxAmountforKm;
                    minMaxSettingFormData.MinUnitPriceForKm = tkdBookingMaxMinFareFeeCalc.FareMinAmountforKm;
                    minMaxSettingFormData.MaxUnitPriceForHour = tkdBookingMaxMinFareFeeCalc.FareMaxAmountforHour;
                    minMaxSettingFormData.MinUnitPriceForHour = tkdBookingMaxMinFareFeeCalc.FareMinAmountforHour;
                    minMaxSettingFormData.MaxUnitBusPriceDiscount = tkdBookingMaxMinFareFeeCalc.FareMaxAmount;
                    minMaxSettingFormData.MinUnitBusPriceDiscount = tkdBookingMaxMinFareFeeCalc.FareMinAmount;
                    minMaxSettingFormData.MaxUnitBusFeeDiscount = tkdBookingMaxMinFareFeeCalc.FeeMaxAmount;
                    minMaxSettingFormData.MinUnitBusFeeDiscount = tkdBookingMaxMinFareFeeCalc.FeeMinAmount;
                    minMaxSettingFormData.MaxUnitPriceDiscount = tkdBookingMaxMinFareFeeCalc.UnitPriceMaxAmount;
                    minMaxSettingFormData.MinUnitPriceDiscount = tkdBookingMaxMinFareFeeCalc.UnitPriceMinAmount;
                    minMaxSettingFormData.MaxUnitBusFeeforKmWithChgDriver = tkdBookingMaxMinFareFeeCalc.ChangeDriverFareMaxAmountforKm;
                    minMaxSettingFormData.MinUnitBusFeeforKmrWithChgDriver = tkdBookingMaxMinFareFeeCalc.ChangeDriverFareMinAmountforKm;
                    minMaxSettingFormData.MaxUnitBusFeeforHourWithChgDriver = tkdBookingMaxMinFareFeeCalc.ChangeDriverFareMaxAmountforHour;
                    minMaxSettingFormData.MinUnitBusFeeforHourWithChgDriver = tkdBookingMaxMinFareFeeCalc.ChangeDriverFareMinAmountforHour;
                    minMaxSettingFormData.MaxSpecialVehicalFee = tkdBookingMaxMinFareFeeCalc.SpecialVehicalFeeMaxAmount;
                    minMaxSettingFormData.MinSpecialVehicalFee = tkdBookingMaxMinFareFeeCalc.SpecialVehicalFeeMinAmount;
                    minMaxSettingFormData.MaxUnitBusSumFeeforMid9 = tkdBookingMaxMinFareFeeCalc.MidnightEarlyMorningFeeMaxAmount;
                    minMaxSettingFormData.MinUnitBusSumFeeforMid9 = tkdBookingMaxMinFareFeeCalc.MidnightEarlyMorningFeeMinAmount;
                    minMaxSettingFormData.SelectedTranSportationOfficePlace = _context.VpmTransportationFeeRule.Where(r => r.TransportationPlaceCodeSeq == tkdBookingMaxMinFareFeeCalc.TransportationPlaceCodeSeq).SingleOrDefault();
                    minMaxSettingFormData.minMaxGridData = CollectMinMaxGridData(ukeNo, syaSyuRen);
                    minMaxSettingFormData.FareIndex = tkdBookingMaxMinFareFeeCalc.FareIndex.ToString();
                    minMaxSettingFormData.FeeIndex = tkdBookingMaxMinFareFeeCalc.FeeIndex.ToString();
                }
                return minMaxSettingFormData;
            }

            /// <summary>
            /// Get data for 予約引受書明細
            /// </summary>
            /// <param name="args"></param>
            private List<MinMaxGridData> CollectMinMaxGridData(string ukeNo, string syaSyuRen)
            {
                List<MinMaxGridData> minMaxGridDataList = new List<MinMaxGridData>();

                List<TkdBookingMaxMinFareFeeCalcMeisai> tkdBookingMaxMinFareFeeCalcMeisai =
                    _context.TkdBookingMaxMinFareFeeCalcMeisai.Where(x => x.UkeNo == ukeNo && x.SyaSyuRen.ToString() == syaSyuRen).ToList();

                foreach (TkdBookingMaxMinFareFeeCalcMeisai bookingMaxMinFareFeeCalcMeisai in tkdBookingMaxMinFareFeeCalcMeisai)
                {
                    var minMaxGridData = new MinMaxGridData()
                    {
                        rowID = bookingMaxMinFareFeeCalcMeisai.Nittei,
                        DateofScheduler = DateTime.ParseExact(bookingMaxMinFareFeeCalcMeisai.UnkYmd, "yyyyMMdd", new CultureInfo("ja-JP")),
                        KmRunning = (int)bookingMaxMinFareFeeCalcMeisai.RunningKm,
                        BusInspectionEndDate = CollectMyDateFromString(bookingMaxMinFareFeeCalcMeisai.InspectionEndYmd, bookingMaxMinFareFeeCalcMeisai.InspectionEndTime),
                        TimeRunning = CollectTimeFromString(bookingMaxMinFareFeeCalcMeisai.RestraintTime),
                        ExactKmRunning = (int)bookingMaxMinFareFeeCalcMeisai.ServiceKm,
                        ExactTimeRunning = CollectTimeFromString(bookingMaxMinFareFeeCalcMeisai.ServiceTime),
                        SpecialTimeRunning = CollectTimeFromString(bookingMaxMinFareFeeCalcMeisai.MidnightEarlyMorningTime),
                        isChangeDriver = Convert.ToBoolean(bookingMaxMinFareFeeCalcMeisai.ChangeDriverFeeFlag),
                        KmRunningwithChgDriver = (int)bookingMaxMinFareFeeCalcMeisai.ChangeDriverRunningKm,
                        TimeRunningwithChgDriver = CollectTimeFromString(bookingMaxMinFareFeeCalcMeisai.ChangeDriverRestraintTime),
                        SpecialTimeRunningwithChgDriver = CollectTimeFromString(bookingMaxMinFareFeeCalcMeisai.ChangeDriverMidnightEarlyMorningTime),
                        BusInspectionStartDate = CollectMyDateFromString(bookingMaxMinFareFeeCalcMeisai.InspectionStartYmd, bookingMaxMinFareFeeCalcMeisai.InspectionStartTime)
                    };

                    if ((minMaxGridData.DateofScheduler - minMaxGridData.BusInspectionStartDate.inpDate).Days > 0)
                    {
                        minMaxGridData.BusInspectionStartDate.isPreviousDay = true;
                        minMaxGridData.BusInspectionStartDate.inpDate = minMaxGridData.BusInspectionStartDate.inpDate.AddDays(1);
                    }

                    minMaxGridDataList.Add(minMaxGridData);
                }
                return minMaxGridDataList;
            }

            private BookingInputHelper.MyDate CollectMyDateFromString(string DateInString, string TimeInString)
            {
                DateTime DateInDatetime = DateTime.ParseExact(DateInString, "yyyyMMdd", new CultureInfo("ja-JP"));
                BookingInputHelper.MyTime TimeInMyTime = new BookingInputHelper.MyTime(Convert.ToInt32(TimeInString.Substring(0, 2)),
                        Convert.ToInt32(TimeInString.Substring(2)));
                return new BookingInputHelper.MyDate(DateInDatetime, TimeInMyTime);
            }

            private BookingInputHelper.MyTime CollectTimeFromString(string TimeInString)
            {
                return new BookingInputHelper.MyTime(Convert.ToInt32(TimeInString.Substring(0, 2)),
                        Convert.ToInt32(TimeInString.Substring(2)));
            }
        }
    }
}
