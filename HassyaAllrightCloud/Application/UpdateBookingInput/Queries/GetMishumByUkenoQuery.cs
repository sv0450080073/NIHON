using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Queries
{
    public class GetMishumByUkenoQuery : IRequest<Dictionary<CommandMode, List<TkdMishum>>>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }
        public class Handler : IRequestHandler<GetMishumByUkenoQuery, Dictionary<CommandMode, List<TkdMishum>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetMishumByUkenoQuery> _logger;
            private readonly int _maxUkeCd;
            private readonly string _newUkeNo;
            public Handler(KobodbContext context, ILogger<GetMishumByUkenoQuery> logger, ITKD_YykshoListService yykshoService)
            {
                _context = context;
                _logger = logger;
                (_maxUkeCd, _newUkeNo) = yykshoService.GetNewUkeNo(new ClaimModel().TenantID).Result;

            }
            public Task<Dictionary<CommandMode, List<TkdMishum>>> Handle(GetMishumByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdMishum>> result = new Dictionary<CommandMode, List<TkdMishum>>();
                    List<TkdMishum> mishumList = _context.TkdMishum.Where(e => e.UkeNo == request.Ukeno && e.SiyoKbn == 1).ToList();
                    List<TkdMishum> updateMishumList = new List<TkdMishum>();
                    List<TkdMishum> addNewMishumList = new List<TkdMishum>();
                    List<TkdMishum> removeMishumList = new List<TkdMishum>();
                    if (mishumList != null)
                    {
                        TkdMishum mishumForBus = mishumList.FirstOrDefault(e => e.SeiFutSyu == 1);
                        TkdMishum mishumForGuider = mishumList.FirstOrDefault(e => e.SeiFutSyu == 5);
                        if (mishumForBus != null)
                            UpdateMishumForBus(request.BookingData, updateMishumList, mishumForBus);
                        else
                            addNewMishumList = CollectDataMishum(request.BookingData);
                        // Not exist
                        if (mishumForGuider == null)
                        {
                            // If GuiderNumber > 0 => add new
                            if (Convert.ToInt16(request.BookingData.VehicleGridDataList.Sum(t => short.Parse(t.GuiderNum))) >= 1)
                            {
                                AddnewMishumForGuider(request.Ukeno, request.BookingData, addNewMishumList);
                            }
                        }
                        // Exist
                        else
                        {
                            // If GuiderNumber = 0 => remove current
                            if (Convert.ToInt16(request.BookingData.VehicleGridDataList.Sum(t => short.Parse(t.GuiderNum))) < 1)
                            {
                                removeMishumList.Add(mishumForGuider);
                            }
                            // Update exsited one
                            else
                            {
                                UpdateMishumForGuider(request.BookingData, updateMishumList, mishumForGuider);
                            }
                        }
                    }
                    result.Add(CommandMode.Create, addNewMishumList);
                    result.Add(CommandMode.Update, updateMishumList);
                    result.Add(CommandMode.Delete, removeMishumList);
                    return Task.FromResult(result);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            private static void UpdateMishumForGuider(BookingFormData bookingdata, List<TkdMishum> updateMishumList, TkdMishum mishumForGuider)
            {
                mishumForGuider.HenKai++; // 変更回数
                mishumForGuider.UriGakKin = bookingdata.VehicleGridDataList.Sum(t => int.Parse(t.GuiderFee)); // 売上額
                mishumForGuider.SyaRyoSyo = Convert.ToInt32(bookingdata.TaxGuider); // 消費税額
                mishumForGuider.SyaRyoTes = Convert.ToInt32(bookingdata.FeeGuider); // 手数料額

                if (bookingdata.TaxTypeforGuider.IdValue == 2)
                {
                    mishumForGuider.UriGakKin -= mishumForGuider.SyaRyoSyo;
                }
                mishumForGuider.SeiKin = mishumForGuider.UriGakKin + mishumForGuider.SyaRyoSyo;
                if (bookingdata.customerComponentTokiStData.TesKbn == 2)
                {
                    mishumForGuider.SeiKin -= mishumForGuider.SyaRyoTes;
                }

                mishumForGuider.UpdYmd = DateTime.Now.ToString("yyyyMMdd"); ;
                mishumForGuider.UpdTime = DateTime.Now.ToString("HHmmss"); ;
                mishumForGuider.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishumForGuider.UpdPrgId = "KJ1000";
                updateMishumList.Add(mishumForGuider);
            }

            private void AddnewMishumForGuider(string ukeno, BookingFormData bookingdata, List<TkdMishum> addNewMishumList)
            {
                TkdMishum mishumForGuider = new TkdMishum();
                mishumForGuider.UkeNo = ukeno;
                mishumForGuider.MisyuRen = 2; // 未収明細連番
                mishumForGuider.HenKai = 0; // 変更回数
                mishumForGuider.SeiFutSyu = (byte)5; // 請求付帯種別 : 1:運賃2:付帯3:通行料4:手配料5ガイド料6:積込品7:キャンセル料
                mishumForGuider.UriGakKin = bookingdata.VehicleGridDataList.Sum(t => int.Parse(t.GuiderFee)); // 売上額
                mishumForGuider.SyaRyoSyo = Convert.ToInt32(bookingdata.TaxGuider); // 消費税額
                mishumForGuider.SyaRyoTes = Convert.ToInt32(bookingdata.FeeGuider); // 手数料額

                if (bookingdata.TaxTypeforGuider.IdValue == 2)
                {
                    mishumForGuider.UriGakKin -= mishumForGuider.SyaRyoSyo;
                }
                mishumForGuider.SeiKin = mishumForGuider.UriGakKin + mishumForGuider.SyaRyoSyo;
                if (bookingdata.customerComponentTokiStData.TesKbn == 2)
                {
                    mishumForGuider.SeiKin -= mishumForGuider.SyaRyoTes;
                }

                mishumForGuider.NyuKinRui = 0;
                mishumForGuider.CouKesRui = 0;
                mishumForGuider.FutuUnkRen = 0;
                mishumForGuider.FutTumRen = 0;
                mishumForGuider.SiyoKbn = 1;
                mishumForGuider.UpdYmd = DateTime.Now.ToString("yyyyMMdd"); ;
                mishumForGuider.UpdTime = DateTime.Now.ToString("HHmmss"); ;
                mishumForGuider.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishumForGuider.UpdPrgId = "KJ1000";
                addNewMishumList.Add(mishumForGuider);
            }

            private void UpdateMishumForBus(BookingFormData bookingdata, List<TkdMishum> updateMishumList, TkdMishum mishumForBus)
            {
                mishumForBus.HenKai++; // 変更回数
                mishumForBus.UriGakKin = bookingdata.VehicleGridDataList.Sum(t => int.Parse(t.BusPrice)); // 売上額
                mishumForBus.SyaRyoSyo = Convert.ToInt32(bookingdata.TaxBus); // 消費税額
                mishumForBus.SyaRyoTes = Convert.ToInt32(bookingdata.FeeBus); // 手数料額

                if (bookingdata.TaxTypeforBus.IdValue == 2)
                {
                    mishumForBus.UriGakKin -= mishumForBus.SyaRyoSyo;
                }
                mishumForBus.SeiKin = mishumForBus.UriGakKin + mishumForBus.SyaRyoSyo;
                if (bookingdata.customerComponentTokiStData.TesKbn == 2)
                {
                    mishumForBus.SeiKin -= mishumForBus.SyaRyoTes;
                }
                
                mishumForBus.UpdYmd = DateTime.Now.ToString("yyyyMMdd"); ;
                mishumForBus.UpdTime = DateTime.Now.ToString("HHmmss"); ;
                mishumForBus.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishumForBus.UpdPrgId = "KJ1000";
                updateMishumList.Add(mishumForBus);
            }

            private List<TkdMishum> CollectDataMishum(BookingFormData bookingdata)
            {
                List<TkdMishum> listMishum = new List<TkdMishum> { };
                TkdMishum mishumForBus = new TkdMishum();
                mishumForBus.MisyuRen = 1; // 未収明細連番
                mishumForBus.HenKai = 0; // 変更回数
                mishumForBus.SeiFutSyu = (byte)1; // 請求付帯種別 : 1:運賃2:付帯3:通行料4:手配料5ガイド料6:積込品7:キャンセル料

                // #8091
                mishumForBus.SyaRyoSyo = Convert.ToInt32(bookingdata.TaxBus); // 消費税額
                mishumForBus.SyaRyoTes = Convert.ToInt32(bookingdata.FeeBus); // 手数料額
                mishumForBus.UriGakKin = bookingdata.VehicleGridDataList.Sum(t => int.Parse(t.BusPrice));
                if (bookingdata.TaxTypeforBus.IdValue == 2)
                {
                    mishumForBus.UriGakKin -= mishumForBus.SyaRyoSyo;
                }
                mishumForBus.SeiKin = mishumForBus.UriGakKin + mishumForBus.SyaRyoSyo;
                if (bookingdata.customerComponentTokiStData.TesKbn == 2)
                {
                    mishumForBus.SeiKin -= mishumForBus.SyaRyoTes;
                }

                mishumForBus.NyuKinRui = 0;
                mishumForBus.CouKesRui = 0;
                mishumForBus.FutuUnkRen = 0;
                mishumForBus.FutTumRen = 0;
                mishumForBus.SiyoKbn = 1;
                mishumForBus.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                mishumForBus.UpdTime = DateTime.Now.ToString("HHmmss");
                mishumForBus.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishumForBus.UpdPrgId = "KJ1000";
                mishumForBus.UkeNo = bookingdata.UkeNo;

                listMishum.Add(mishumForBus);
                if (Convert.ToInt16(bookingdata.VehicleGridDataList.Sum(t => short.Parse(t.GuiderNum))) < 1)
                {
                    return listMishum;
                }

                return listMishum;
            }
        }
    }
}
