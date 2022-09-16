using DevExpress.Blazor.Internal;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingIncidental.Commands
{
    public class UpdateIncidentalBookingCommand : IRequest<Unit>
    {
        private readonly string _ukeNo;
        private readonly IncidentalBooking _incidentalBookingData;

        public UpdateIncidentalBookingCommand(string ukeNo, IncidentalBooking incidentalBookingData)
        {
            _ukeNo = ukeNo;
            _incidentalBookingData = incidentalBookingData;
        }

        public class Handler : IRequestHandler<UpdateIncidentalBookingCommand, Unit>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateIncidentalBookingCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    // check paid and coupon
                    var yyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request._ukeNo).FirstOrDefaultAsync();
                    if (yyksho.NyuKinKbn != 1 || yyksho.NcouKbn != 1)
                    {
                        throw new Exception("Booking has been paid or coupon");
                    }

                    byte futtumKbn = (byte)request._incidentalBookingData.FuttumKbnMode;
                    byte seiFutSyu = 2;
                    switch (request._incidentalBookingData.FuttumKbnMode)
                    {
                        case IncidentalViewMode.Futai:
                            seiFutSyu = 2;
                            break;
                        case IncidentalViewMode.Tsumi:
                            seiFutSyu = 6;
                            break;
                        default:
                            break;
                    }

                    var futtumListDb = await _context.TkdFutTum.Where(t => t.UkeNo == request._ukeNo && t.FutTumKbn == futtumKbn).ToListAsync();
                    var mFuttuListDb = await _context.TkdMfutTu.Where(t => t.UkeNo == request._ukeNo && t.FutTumKbn == futtumKbn).ToListAsync();
                    var mishumListDb = await _context.TkdMishum.Where(t => t.UkeNo == request._ukeNo && t.SeiFutSyu == seiFutSyu && t.FutTumRen > 0).ToListAsync();

                    var futtumList = CollectDataFuttum(request._incidentalBookingData, request._ukeNo);
                    var mFuttuList = CollectDataMFutTu(request._incidentalBookingData, request._ukeNo);
                    var mishumList = CollectDataMishum(request._incidentalBookingData, request._ukeNo);

                    // case delete all
                    if (futtumList.Count == 0)
                    {
                        if (futtumListDb.Any(f => f.NyuKinKbn != 1 || f.NcouKbn != 1))
                        {
                            throw new Exception("Cannot delete futtum already in charge");
                        }
                        futtumListDb.ForEach(f =>
                        {
                            f.SiyoKbn = 2;
                            f.HenKai++;
                            f.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            f.UpdTime = DateTime.Now.ToString("HHmmss");
                            f.UpdSyainCd = new ClaimModel().SyainCdSeq;
                            f.UpdPrgId = "KJ1400";
                        });
                        mFuttuListDb.ForEach(f =>
                        {
                            f.SiyoKbn = 2;
                            f.HenKai++;
                            f.Suryo = 0;
                            f.UriGakKin = 0;
                            f.SyaRyoSyo = 0;
                            f.SyaRyoTes = 0;
                            f.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            f.UpdTime = DateTime.Now.ToString("HHmmss");
                            f.UpdSyainCd = new ClaimModel().SyainCdSeq;
                            f.UpdPrgId = "KJ1400";
                        });
                        mishumListDb.ForEach(f =>
                        {
                            f.SiyoKbn = 2;
                            f.HenKai++;
                            f.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            f.UpdTime = DateTime.Now.ToString("HHmmss");
                            f.UpdSyainCd = new ClaimModel().SyainCdSeq;
                            f.UpdPrgId = "KJ1400";
                        });
                    }
                    else
                    {
                        short misyuRenMax = _context.TkdMishum.Where(m => m.UkeNo == request._ukeNo).Max(m => m.MisyuRen);
                        short fromFutRen = Math.Min(futtumListDb.Min(f => f.FutTumRen), futtumList.Min(f => f.FutTumRen));
                        short toFutRen = Math.Max(futtumListDb.Max(f => f.FutTumRen), futtumList.Max(f => f.FutTumRen));
                        for (short futRen = fromFutRen; futRen <= toFutRen; futRen++)
                        {
                            var futtumDb = futtumListDb.SingleOrDefault(f => f.FutTumRen == futRen);
                            var futtum = futtumList.SingleOrDefault(f => f.FutTumRen == futRen);

                            // case update
                            if (futtumDb != null && futtum != null)
                            {
                                // copy old field
                                futtum.UkeNo = futtumDb.UkeNo;
                                futtum.UnkRen = futtumDb.UnkRen;
                                futtum.FutTumKbn = futtumDb.FutTumKbn;
                                futtum.FutTumRen = futtumDb.FutTumRen;
                                //futtum.FutTumCdSeq = futtumDb.FutTumCdSeq;
                                futtum.NyuKinKbn = futtumDb.NyuKinKbn;
                                futtum.NcouKbn = futtumDb.NcouKbn;
                                futtum.ExpItem = futtumDb.ExpItem;
                                futtum.SortJun = futtumDb.SortJun;
                                futtum.SiyoKbn = futtumDb.SiyoKbn;
                                futtum.HenKai = (short)(futtumDb.HenKai + 1);
                                // copy new data
                                futtumDb.SimpleCloneProperties(futtum);
                                _context.Update(futtumDb);

                                var mishumToUpdate = mishumList.SingleOrDefault(m => m.FutTumRen == futtum.FutTumRen);
                                var mishumDb = mishumListDb.SingleOrDefault(m => m.FutTumRen == futtum.FutTumRen);
                                if(futtum.SeisanKbn == 2 && mishumDb != null)
                                {
                                    // Spec no.65
                                    mishumDb.SiyoKbn = 2;
                                    mishumDb.HenKai = (short)(mishumDb.HenKai + 1);
                                    _context.TkdMishum.Update(mishumDb);
                                }
                                else if (mishumToUpdate != null)
                                {
                                    if (mishumDb == null)
                                    {
                                        mishumToUpdate.MisyuRen = ++misyuRenMax;
                                        await _context.TkdMishum.AddAsync(mishumToUpdate);
                                    }
                                    else
                                    {
                                        // copy old field
                                        mishumToUpdate.UkeNo = mishumDb.UkeNo;
                                        mishumToUpdate.MisyuRen = mishumDb.MisyuRen;
                                        mishumToUpdate.SeiFutSyu = mishumDb.SeiFutSyu;
                                        mishumToUpdate.NyuKinRui = mishumDb.NyuKinRui;
                                        mishumToUpdate.CouKesRui = mishumDb.CouKesRui;
                                        mishumToUpdate.FutuUnkRen = mishumDb.FutuUnkRen;
                                        mishumToUpdate.FutTumRen = mishumDb.FutTumRen;
                                        mishumToUpdate.SiyoKbn = 1;
                                        mishumToUpdate.HenKai = (short)(mishumDb.HenKai + 1);
                                        // copy new data
                                        mishumDb.SimpleCloneProperties(mishumToUpdate);
                                        _context.TkdMishum.Update(mishumDb);
                                    }
                                }

                                var mFuttuListDbUpdate = mFuttuListDb.Where(f => f.FutTumRen == futtum.FutTumRen);
                                var mFuttuListUpdate = mFuttuList.Where(f => f.FutTumRen == futtum.FutTumRen);

                                if (mFuttuListUpdate.Count() == 0)
                                {
                                    _context.TkdMfutTu.UpdateRange(mFuttuListDbUpdate);
                                    continue;
                                }
                                if (mFuttuListDbUpdate.Count() == 0)
                                {
                                    await _context.TkdMfutTu.AddRangeAsync(mFuttuListUpdate);
                                    continue;
                                }

                                var teiDanRen = mFuttuListDbUpdate.Select(m => new { m.TeiDanNo, m.BunkRen })
                                    .Union(mFuttuListUpdate.Select(m => new { m.TeiDanNo, m.BunkRen })).Distinct();
                                foreach (var item in teiDanRen)
                                {
                                    var mFuttuDb = mFuttuListDbUpdate.SingleOrDefault(f => f.TeiDanNo == item.TeiDanNo && f.BunkRen == item.BunkRen);
                                    var mFuttu = mFuttuListUpdate.SingleOrDefault(f => f.TeiDanNo == item.TeiDanNo && f.BunkRen == item.BunkRen);

                                    // case update (include deleted)
                                    if (mFuttuDb != null && mFuttu != null)
                                    {
                                        mFuttu.UkeNo = mFuttuDb.UkeNo;
                                        mFuttu.UnkRen = mFuttuDb.UnkRen;
                                        mFuttu.FutTumKbn = mFuttuDb.FutTumKbn;
                                        mFuttu.FutTumRen = mFuttuDb.FutTumRen;
                                        mFuttu.TeiDanNo = mFuttuDb.TeiDanNo;
                                        mFuttu.BunkRen = mFuttuDb.BunkRen;
                                        mFuttu.HenKai = (short)(mFuttuDb.HenKai + 1);
                                        mFuttuDb.SimpleCloneProperties(mFuttu);
                                    }
                                    // case insert
                                    else if (mFuttuDb == null && mFuttu != null)
                                    {
                                        await _context.TkdMfutTu.AddAsync(mFuttu);
                                    }
                                    // case delete
                                    else if (mFuttuDb != null && mFuttu == null)
                                    {
                                        mFuttuDb.SiyoKbn = 2;
                                        mFuttuDb.HenKai++;
                                        mFuttuDb.Suryo = 0;
                                        mFuttuDb.UriGakKin = 0;
                                        mFuttuDb.SyaRyoSyo = 0;
                                        mFuttuDb.SyaRyoTes = 0;
                                        mFuttuDb.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                        mFuttuDb.UpdTime = DateTime.Now.ToString("HHmmss");
                                        mFuttuDb.UpdSyainCd = new ClaimModel().SyainCdSeq;
                                        mFuttuDb.UpdPrgId = "KJ1400";
                                    }
                                }

                                _context.TkdMfutTu.UpdateRange(mFuttuListDbUpdate);
                            }
                            // case insert
                            else if (futtumDb == null && futtum != null)
                            {
                                await _context.TkdFutTum.AddAsync(futtum);
                                await _context.TkdMfutTu.AddRangeAsync(mFuttuList.Where(m => m.FutTumRen == futtum.FutTumRen));
                                var mishumToInsert = mishumList.SingleOrDefault(m => m.FutTumRen == futtum.FutTumRen);
                                if (mishumToInsert != null)
                                {
                                    mishumToInsert.MisyuRen = ++misyuRenMax;
                                    await _context.TkdMishum.AddAsync(mishumToInsert);
                                }
                            }
                            // case delete
                            else if (futtumDb != null && futtum == null)
                            {
                                if (futtumDb.NyuKinKbn != 1 || futtumDb.NcouKbn != 1)
                                {
                                    throw new Exception("Cannot delete futtum already in charge");
                                }
                                futtumDb.SiyoKbn = 2;
                                futtumDb.HenKai++;
                                futtumDb.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                futtumDb.UpdTime = DateTime.Now.ToString("HHmmss");
                                futtumDb.UpdSyainCd = new ClaimModel().SyainCdSeq;
                                futtumDb.UpdPrgId = "KJ1400";

                                mFuttuListDb.Where(m => m.FutTumRen == futtumDb.FutTumRen).ForEach(m =>
                                {
                                    m.SiyoKbn = 2;
                                    m.HenKai++;
                                    m.Suryo = 0;
                                    m.UriGakKin = 0;
                                    m.SyaRyoSyo = 0;
                                    m.SyaRyoTes = 0;
                                    m.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                    m.UpdTime = DateTime.Now.ToString("HHmmss");
                                    m.UpdSyainCd = new ClaimModel().SyainCdSeq;
                                    m.UpdPrgId = "KJ1400";
                                });
                                mishumListDb.Where(m => m.FutTumRen == futtumDb.FutTumRen).ForEach(m =>
                                {
                                    m.SiyoKbn = 2;
                                    m.HenKai++;
                                    m.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                    m.UpdTime = DateTime.Now.ToString("HHmmss");
                                    m.UpdSyainCd = new ClaimModel().SyainCdSeq;
                                    m.UpdPrgId = "KJ1400";
                                });
                            }
                        }
                    }

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
                            futu.UpdSyainCd = new ClaimModel().SyainCdSeq;
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

                foreach (var loadFuttum in incidentalBooking.LoadFuttumList)
                {
                    if (byte.Parse(loadFuttum.SelectedLoadSeisanKbn.CodeKbn) == 2)
                    {

                    }
                    else if (byte.Parse(loadFuttum.SelectedLoadSeisanKbn.CodeKbn) == 1
                        && int.Parse(loadFuttum.Tanka) > 0)
                    {
                        var mishum = new TkdMishum();
                        mishum.UkeNo = ukeNo;
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
