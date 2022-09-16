using HassyaAllrightCloud.Commons.Constants;
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
    public class GetMaxMinFareFeeCalcMeisaiByUkenoQuery : IRequest<Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalcMeisai>>>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }
        public class Handler : IRequestHandler<GetMaxMinFareFeeCalcMeisaiByUkenoQuery, Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalcMeisai>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetMaxMinFareFeeCalcMeisaiByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetMaxMinFareFeeCalcMeisaiByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalcMeisai>>> Handle(GetMaxMinFareFeeCalcMeisaiByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalcMeisai>> result = new Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalcMeisai>>();
                    List<TkdBookingMaxMinFareFeeCalcMeisai> addNewMeiSaiList = new List<TkdBookingMaxMinFareFeeCalcMeisai>();
                    List<TkdBookingMaxMinFareFeeCalcMeisai> removeMeiSaiList = new List<TkdBookingMaxMinFareFeeCalcMeisai>();
                    List<TkdBookingMaxMinFareFeeCalcMeisai> updateMeiSaiList = new List<TkdBookingMaxMinFareFeeCalcMeisai>();
                    List<TkdBookingMaxMinFareFeeCalcMeisai> bookingMaxMinFareFeeCalsMeisaiList =
                    _context.TkdBookingMaxMinFareFeeCalcMeisai.Where(e => e.UkeNo == request.Ukeno && e.UnkRen == request.BookingData.UnkRen).ToList();
                    if (bookingMaxMinFareFeeCalsMeisaiList != null)
                    {
                        if (bookingMaxMinFareFeeCalsMeisaiList.Count > 0)
                        {
                            // Case delete row                
                            int totalRecordMinMaxMeisai = bookingMaxMinFareFeeCalsMeisaiList.Max(t => t.SyaSyuRen);
                            for (int i = 1; i <= totalRecordMinMaxMeisai; i++)
                            {
                                var bookingMaxMinMeisaiBaseOnSyaSyuRen = bookingMaxMinFareFeeCalsMeisaiList.Where(x => x.SyaSyuRen == i).ToList();
                                var vehicleTypeRow = request.BookingData.VehicleGridDataList.FirstOrDefault(x => x.RowID == i.ToString());
                                if (vehicleTypeRow == null)
                                {
                                    if (bookingMaxMinMeisaiBaseOnSyaSyuRen != null)
                                    {
                                        foreach (var bookingMaxMinFareFeeCalsMeisai in bookingMaxMinMeisaiBaseOnSyaSyuRen)
                                        {
                                            bookingMaxMinFareFeeCalsMeisaiList.Remove(bookingMaxMinFareFeeCalsMeisai);
                                            removeMeiSaiList.Add(bookingMaxMinFareFeeCalsMeisai);
                                        }

                                    }
                                }
                            }
                        }
                        foreach (var vehicleTypeRow in request.BookingData.VehicleGridDataList)
                        {
                            MinMaxSettingFormData minMaxSettingFormData = vehicleTypeRow.minMaxForm;
                            foreach (var minMaxGridData in minMaxSettingFormData.minMaxGridData) // Loop each row in grid of Min-Max popup
                            {
                                var tkdBookingMaxMinFareFeeCalsMeisai = bookingMaxMinFareFeeCalsMeisaiList.
                                    FirstOrDefault(x => x.SyaSyuRen.ToString() == vehicleTypeRow.RowID && x.Nittei == minMaxGridData.rowID);
                                // Case add row
                                if (tkdBookingMaxMinFareFeeCalsMeisai == null)
                                {
                                    tkdBookingMaxMinFareFeeCalsMeisai = new TkdBookingMaxMinFareFeeCalcMeisai();
                                    tkdBookingMaxMinFareFeeCalsMeisai.UkeNo = request.Ukeno;
                                    tkdBookingMaxMinFareFeeCalsMeisai.UnkRen = 1;
                                    tkdBookingMaxMinFareFeeCalsMeisai.SyaSyuRen = Int16.Parse(vehicleTypeRow.RowID);
                                    tkdBookingMaxMinFareFeeCalsMeisai.Nittei = (byte)(minMaxGridData.rowID);
                                    addNewMeiSaiList.Add(tkdBookingMaxMinFareFeeCalsMeisai);
                                }
                                else
                                {
                                    updateMeiSaiList.Add(tkdBookingMaxMinFareFeeCalsMeisai);
                                }
                                // Case update row
                                tkdBookingMaxMinFareFeeCalsMeisai.UnkYmd = minMaxGridData.DateofScheduler.ToString("yyyyMMdd");
                                tkdBookingMaxMinFareFeeCalsMeisai.RunningKm = minMaxGridData.KmRunning;
                                tkdBookingMaxMinFareFeeCalsMeisai.InspectionStartYmd = minMaxGridData.BusInspectionStartDate.ConvertedDate.ToString("yyyyMMdd");
                                tkdBookingMaxMinFareFeeCalsMeisai.InspectionStartTime = minMaxGridData.BusInspectionStartDate.inpTime.ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCalsMeisai.InspectionEndYmd = minMaxGridData.BusInspectionEndDate.inpDate.ToString("yyyyMMdd");
                                tkdBookingMaxMinFareFeeCalsMeisai.InspectionEndTime = minMaxGridData.BusInspectionEndDate.inpTime.ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCalsMeisai.RestraintTime = minMaxGridData.TimeRunning.ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCalsMeisai.ServiceKm = minMaxGridData.ExactKmRunning;
                                tkdBookingMaxMinFareFeeCalsMeisai.ServiceTime = minMaxGridData.ExactTimeRunning.ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCalsMeisai.MidnightEarlyMorningTime = minMaxGridData.SpecialTimeRunning.ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverFeeFlag = Convert.ToByte(minMaxGridData.isChangeDriver);
                                tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverRunningKm = minMaxGridData.KmRunningwithChgDriver; // #7981
                                tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverRestraintTime = minMaxGridData.TimeRunningwithChgDriver.ToStringWithoutDelimiter();
                                tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverMidnightEarlyMorningTime = minMaxGridData.SpecialTimeRunningwithChgDriver.ToStringWithoutDelimiter(); ;
                                tkdBookingMaxMinFareFeeCalsMeisai.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                tkdBookingMaxMinFareFeeCalsMeisai.UpdTime = DateTime.Now.ToString("HHmmss");
                                tkdBookingMaxMinFareFeeCalsMeisai.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                tkdBookingMaxMinFareFeeCalsMeisai.UpdPrgId = "KJ1000";
                            }
                        }
                    }
                    result.Add(CommandMode.Create, addNewMeiSaiList);
                    result.Add(CommandMode.Update, updateMeiSaiList);
                    result.Add(CommandMode.Delete, removeMeiSaiList);
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
