using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingIncidental.Commands
{
    public class InsertIncidentalBookingCommand : IRequest<Unit>
    {
        private readonly string _ukeNo;
        private readonly IncidentalBooking _incidentalBookingData;

        public InsertIncidentalBookingCommand(string ukeNo, IncidentalBooking incidentalBookingData)
        {
            _ukeNo = ukeNo;
            _incidentalBookingData = incidentalBookingData;
        }

        public class Handler : IRequestHandler<InsertIncidentalBookingCommand, Unit>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(InsertIncidentalBookingCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    var futumList = CollectDataFuttum(request._incidentalBookingData, request._ukeNo);
                    var mFutTuList = CollectDataMFutTu(request._incidentalBookingData, request._ukeNo);
                    var mishumList = CollectDataMishum(request._incidentalBookingData, request._ukeNo);

                    await _context.TkdFutTum.AddRangeAsync(futumList);
                    await _context.TkdMfutTu.AddRangeAsync(mFutTuList);
                    await _context.TkdMishum.AddRangeAsync(mishumList);

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }

            private List<TkdFutTum> CollectDataFuttum(IncidentalBooking incidentalBooking, string ukeNo)
            {
                var result = new List<TkdFutTum>();

                foreach (var loadFuttum in incidentalBooking.LoadFuttumList)
                {
                    var tkdFuttum = new TkdFutTum();
                    tkdFuttum.UkeNo = ukeNo;

                    tkdFuttum.UnkRen = incidentalBooking.UnkRen;
                    switch (loadFuttum.FuttumKbnMode)
                    {
                        case IncidentalViewMode.Futai:
                            tkdFuttum.FutTumKbn = 1;
                            tkdFuttum.FutTumRen = loadFuttum.FutTumRen;
                            tkdFuttum.FutTumCdSeq = loadFuttum.SelectedLoadFutai.FutaiCdSeq;
                            break;
                        case IncidentalViewMode.Tsumi:
                            tkdFuttum.FutTumKbn = 2;
                            tkdFuttum.FutTumRen = loadFuttum.FutTumRen;
                            tkdFuttum.FutTumCdSeq = loadFuttum.SelectedLoadTsumi.CodeKbnSeq;
                            break;
                        default:
                            break;
                    }
                    tkdFuttum.HenKai = 0;
                    tkdFuttum.Nittei = loadFuttum.ScheduleDate.Nittei;
                    tkdFuttum.TomKbn = loadFuttum.ScheduleDate.TomKbn;
                    tkdFuttum.FutTumNm = loadFuttum.FutTumNm;
                    tkdFuttum.HasYmd = loadFuttum.ScheduleDate.Date.ToString("yyyyMMdd");
                    tkdFuttum.IriRyoChiCd = byte.Parse(loadFuttum.SelectedLoadNyuRyokinName?.RyokinTikuCd ?? "0");
                    tkdFuttum.IriRyoCd = loadFuttum.SelectedLoadNyuRyokinName?.RyokinCd ?? string.Empty;
                    tkdFuttum.IriRyoNm = loadFuttum.IriRyoNm ?? string.Empty;
                    tkdFuttum.DeRyoChiCd = byte.Parse(loadFuttum.SelectedLoadShuRyokin?.RyokinTikuCd ?? "0");
                    tkdFuttum.DeRyoCd = loadFuttum.SelectedLoadShuRyokin?.RyokinCd ?? string.Empty;
                    tkdFuttum.DeRyoNm = loadFuttum.DeRyoNm ?? string.Empty;
                    tkdFuttum.SeisanCdSeq = loadFuttum.SelectedLoadSeisanCd.SeisanCdSeq;//
                    tkdFuttum.SeisanNm = loadFuttum.SeisanNm;
                    tkdFuttum.SeisanKbn = Convert.ToByte(loadFuttum.SelectedLoadSeisanKbn.CodeKbn);
                    tkdFuttum.TanKa = int.Parse(loadFuttum.Tanka);
                    tkdFuttum.Suryo = short.Parse(loadFuttum.Suryo);
                    tkdFuttum.UriGakKin = loadFuttum.TotalAmountWithoutTax;
                    tkdFuttum.ZeiKbn = Convert.ToByte(loadFuttum.TaxType.IdValue);
                    tkdFuttum.Zeiritsu = decimal.Parse(loadFuttum.Zeiritsu);
                    tkdFuttum.SyaRyoSyo = loadFuttum.SyaRyoSyo;
                    tkdFuttum.TesuRitu = decimal.Parse(loadFuttum.TesuRitu);
                    tkdFuttum.SyaRyoTes = loadFuttum.SyaRyoTes;
                    tkdFuttum.NyuKinKbn = 1;
                    tkdFuttum.NcouKbn = 1;
                    tkdFuttum.BikoNm = loadFuttum.NoteInput ?? string.Empty;
                    tkdFuttum.ExpItem = string.Empty;// spec update
                    tkdFuttum.SortJun = 0;// spec update
                    tkdFuttum.SirSitenCdSeq = loadFuttum.SelectedCustomer?.SitenCdSeq ?? 0; //Update option: not selected
                    tkdFuttum.SireCdSeq = loadFuttum.SelectedCustomer?.TokuiSeq ?? 0; //Update option: not selected
                    tkdFuttum.SirTanKa = int.Parse(loadFuttum.SirTanka);
                    tkdFuttum.SirSuryo = short.Parse(loadFuttum.SirSuryo);
                    tkdFuttum.SirGakKin = loadFuttum.SirGakKinWithoutTax;
                    tkdFuttum.SirZeiKbn = Convert.ToByte(loadFuttum.SirTaxType.IdValue);
                    tkdFuttum.SirZeiritsu = decimal.Parse(loadFuttum.SirZeiritsu);
                    tkdFuttum.SirSyaRyoSyo = loadFuttum.SirSyaRyoSyo;
                    tkdFuttum.SiyoKbn = 1;

                    tkdFuttum.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdFuttum.UpdTime = DateTime.Now.ToString("HHmmss");
                    tkdFuttum.UpdSyainCd = new ClaimModel().SyainCdSeq;
                    tkdFuttum.UpdPrgId = "KJ1400";

                    result.Add(tkdFuttum);
                }

                return result;
            }

            private List<TkdMfutTu> CollectDataMFutTu(IncidentalBooking incidentalBooking, string ukeNo)
            {
                var result = new List<TkdMfutTu>();

                foreach (var loadFuttum in incidentalBooking.LoadFuttumList)
                {
                    var mfuttuList = new List<TkdMfutTu>();
                    if (int.Parse(loadFuttum.Suryo) > 0 && loadFuttum.SettingQuantityList.Sum(e => int.Parse(e.Suryo)) == int.Parse(loadFuttum.Suryo))
                    {
                        foreach (var mfutuItem in loadFuttum.SettingQuantityList)
                        {
                            var futu = new TkdMfutTu();
                            futu.UkeNo = ukeNo;
                            futu.UnkRen = mfutuItem.UnkRen;
                            switch (loadFuttum.FuttumKbnMode)
                            {
                                case IncidentalViewMode.Futai:
                                    futu.FutTumKbn = 1;
                                    futu.FutTumRen = loadFuttum.FutTumRen;
                                    break;
                                case IncidentalViewMode.Tsumi:
                                    futu.FutTumKbn = 2;
                                    futu.FutTumRen = loadFuttum.FutTumRen;
                                    break;
                                default:
                                    break;
                            }
                            futu.TeiDanNo = mfutuItem.TeiDanNo;
                            futu.BunkRen = mfutuItem.BunkRen;
                            futu.HenKai = 0;
                            futu.Suryo = short.Parse(mfutuItem.Suryo);

                            int dividedTotalPrice = futu.Suryo * int.Parse(loadFuttum.Tanka);
                            if (loadFuttum.TaxType.IdValue == Constants.ForeignTax.IdValue || loadFuttum.TaxType.IdValue == Constants.NoTax.IdValue)
                            {
                                futu.UriGakKin = dividedTotalPrice;
                            }
                            else
                            {
                                futu.UriGakKin = dividedTotalPrice <= 0
                                    ? 0
                                    : (int)(dividedTotalPrice - (decimal.Parse(loadFuttum.Zeiritsu) * dividedTotalPrice) / (100 + decimal.Parse(loadFuttum.Zeiritsu)));
                            }
                            futu.SyaRyoSyo = loadFuttum.SyaRyoSyo * futu.Suryo / int.Parse(loadFuttum.Suryo);
                            futu.SyaRyoTes = loadFuttum.SyaRyoTes * futu.Suryo / int.Parse(loadFuttum.Suryo);

                            futu.SiyoKbn = 1;
                            futu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            futu.UpdTime = DateTime.Now.ToString("HHmmss");
                            futu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            futu.UpdPrgId = "KJ1400";

                            mfuttuList.Add(futu);
                        }

                        int diffUrigakKin = loadFuttum.TotalAmountWithoutTax - mfuttuList.Sum(f => f.UriGakKin);
                        int assingeIndex = 0;
                        int assingeUnit = 0;
                        if (diffUrigakKin != 0)
                        {
                            while(diffUrigakKin != 0)
                            {
                                assingeUnit = diffUrigakKin < 0 ? -1 : 1;
                                if(mfuttuList[assingeIndex].UriGakKin > 0)
                                {
                                    mfuttuList[assingeIndex].UriGakKin += assingeUnit;
                                    diffUrigakKin -= assingeUnit;
                                }
                                assingeIndex++;
                                if(assingeIndex == mfuttuList.Count) assingeIndex = 0;
                            }
                        }

                        int diffSyaRyoSyo = loadFuttum.SyaRyoSyo - mfuttuList.Sum(f => f.SyaRyoSyo);
                        if (diffSyaRyoSyo != 0)
                        {
                            assingeIndex = 0;
                            while(diffSyaRyoSyo != 0)
                            {
                                assingeUnit = diffSyaRyoSyo < 0 ? -1 : 1;
                                if(mfuttuList[assingeIndex].SyaRyoSyo > 0)
                                {
                                    mfuttuList[assingeIndex].SyaRyoSyo += assingeUnit;
                                    diffSyaRyoSyo -= assingeUnit;
                                }
                                assingeIndex++;
                                if(assingeIndex == mfuttuList.Count) assingeIndex = 0;
                            }
                        }

                        int diffSyaRyoTes = loadFuttum.SyaRyoTes - mfuttuList.Sum(f => f.SyaRyoTes);
                        if (diffSyaRyoTes != 0)
                        {
                            assingeIndex = 0;
                            while(diffSyaRyoTes != 0)
                            {
                                assingeUnit = diffSyaRyoTes < 0 ? -1 : 1;
                                if(mfuttuList[assingeIndex].SyaRyoTes > 0)
                                {
                                    mfuttuList[assingeIndex].SyaRyoTes += assingeUnit;
                                    diffSyaRyoTes -= assingeUnit;
                                }
                                assingeIndex++;
                                if(assingeIndex == mfuttuList.Count) assingeIndex = 0;
                            }
                        }
                    }
                    result.AddRange(mfuttuList);
                }

                return result;
            }

            private List<TkdMishum> CollectDataMishum(IncidentalBooking incidentalBooking, string ukeNo)
            {
                var result = new List<TkdMishum>();
                short misyuRenMax = (short)_context.TkdMishum.Where(m => m.UkeNo == ukeNo)
                    .Select(m => Convert.ToInt32(m.MisyuRen))
                    .ToList().DefaultIfEmpty(0).Max();

                foreach (var loadFuttum in incidentalBooking.LoadFuttumList)
                {
                    if (byte.Parse(loadFuttum.SelectedLoadSeisanKbn.CodeKbn) == 1
                        && int.Parse(loadFuttum.Tanka) > 0)
                    {
                        var mishum = new TkdMishum();
                        mishum.UkeNo = ukeNo;
                        mishum.MisyuRen = ++misyuRenMax;
                        mishum.HenKai = 0;
                        switch (loadFuttum.FuttumKbnMode)
                        {
                            case IncidentalViewMode.Futai:
                                mishum.SeiFutSyu = 2;
                                mishum.FutTumRen = loadFuttum.FutTumRen;
                                break;
                            case IncidentalViewMode.Tsumi:
                                mishum.SeiFutSyu = 6;
                                mishum.FutTumRen = loadFuttum.FutTumRen;
                                break;
                            default:
                                break;
                        }
                        mishum.UriGakKin = loadFuttum.TotalAmountWithoutTax;
                        mishum.SyaRyoSyo = loadFuttum.SyaRyoSyo;
                        mishum.SyaRyoTes = loadFuttum.SyaRyoTes;
                        mishum.SeiKin = loadFuttum.ZeikomiKin;
                        if (incidentalBooking.TesKbnFut == 2)// #8092
                        {
                            mishum.SeiKin -= loadFuttum.SyaRyoTes;
                        }
                        mishum.NyuKinRui = 0;
                        mishum.CouKesRui = 0;
                        mishum.FutuUnkRen = incidentalBooking.UnkRen;
                        mishum.SiyoKbn = 1;
                        mishum.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        mishum.UpdTime = DateTime.Now.ToString("HHmmss");
                        mishum.UpdSyainCd = new ClaimModel().SyainCdSeq;
                        mishum.UpdPrgId = "KJ1400";

                        result.Add(mishum);
                    }
                }

                return result;
            }
        }
    }
}
