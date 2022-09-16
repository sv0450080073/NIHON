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
    public class GetYyksyuByUkenoQuery : IRequest<Dictionary<CommandMode, List<TkdYykSyu>>>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }
        public class Handler : IRequestHandler<GetYyksyuByUkenoQuery, Dictionary<CommandMode, List<TkdYykSyu>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetYyksyuByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetYyksyuByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<Dictionary<CommandMode, List<TkdYykSyu>>> Handle(GetYyksyuByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdYykSyu>> result = new Dictionary<CommandMode, List<TkdYykSyu>>();
                    List<TkdYykSyu> listYykSyu = _context.TkdYykSyu.Where(e => e.UkeNo == request.Ukeno && e.UnkRen == request.BookingData.UnkRen && e.SiyoKbn == 1).ToList();
                    List<TkdYykSyu> addNewYykSyuList = new List<TkdYykSyu>();
                    List<TkdYykSyu> removeYykSyuList = new List<TkdYykSyu>();
                    List<TkdYykSyu> updateYykSyuList = new List<TkdYykSyu>();
                    if (listYykSyu != null)
                    {
                        // Case delete row
                        int totalRecordYyksyu = listYykSyu.Max(t => t.SyaSyuRen);
                        for (int i = 1; i <= totalRecordYyksyu; i++)
                        {
                            var yyksyu = listYykSyu.FirstOrDefault(x => x.SyaSyuRen == i);
                            var vehicleTypeRow = request.BookingData.VehicleGridDataList.FirstOrDefault(x => x.RowID == i.ToString());
                            if (vehicleTypeRow == null)
                            {
                                if (yyksyu != null)
                                {
                                    yyksyu.SiyoKbn = 2;
                                    listYykSyu.Remove(yyksyu);
                                    removeYykSyuList.Add(yyksyu);
                                }
                            }
                        }

                        foreach (var vehicleTypeRow in request.BookingData.VehicleGridDataList)
                        {
                            var yyksyu = listYykSyu.FirstOrDefault(x => x.SyaSyuRen.ToString() == vehicleTypeRow.RowID);
                            // Case add new row
                            if (yyksyu == null)
                            {
                                yyksyu = new TkdYykSyu();
                                yyksyu.UkeNo = request.Ukeno;
                                yyksyu.SyaSyuRen = (short)(Int16.Parse(vehicleTypeRow.RowID));
                                yyksyu.SiyoKbn = 1;
                                yyksyu.HenKai = 0;
                                addNewYykSyuList.Add(yyksyu);
                            }
                            else
                            {
                                // Case update row
                                yyksyu.HenKai++;
                                updateYykSyuList.Add(yyksyu);
                            }
                            yyksyu.UnkRen = 1;
                            yyksyu.KataKbn = (byte)Convert.ToInt32(vehicleTypeRow.busTypeData.Katakbn);
                            yyksyu.SyaSyuCdSeq = Convert.ToInt32(vehicleTypeRow.busTypeData.SyaSyuCdSeq);
                            yyksyu.SyaSyuDai = Convert.ToInt16(vehicleTypeRow.BusNum);
                            yyksyu.SyaSyuTan = Convert.ToInt32(vehicleTypeRow.UnitPrice);
                            yyksyu.SyaRyoUnc = Convert.ToInt32(vehicleTypeRow.BusPrice);
                            yyksyu.DriverNum = byte.Parse(vehicleTypeRow.DriverNum);
                            yyksyu.UnitBusPrice = Convert.ToInt32(vehicleTypeRow.UnitBusPrice);
                            yyksyu.UnitBusFee = Convert.ToInt32(vehicleTypeRow.UnitBusFee);
                            yyksyu.GuiderNum = byte.Parse(vehicleTypeRow.GuiderNum);
                            yyksyu.UnitGuiderPrice = Convert.ToInt32(vehicleTypeRow.UnitGuiderFee);
                            yyksyu.UnitGuiderFee = Convert.ToInt32(vehicleTypeRow.GuiderFee);
                            yyksyu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            yyksyu.UpdTime = DateTime.Now.ToString("HHmmss");
                            yyksyu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            yyksyu.UpdPrgId = "KJ1000";
                        }
                    }
                    result.Add(CommandMode.Create, addNewYykSyuList);
                    result.Add(CommandMode.Update, updateYykSyuList);
                    result.Add(CommandMode.Delete, removeYykSyuList);
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
