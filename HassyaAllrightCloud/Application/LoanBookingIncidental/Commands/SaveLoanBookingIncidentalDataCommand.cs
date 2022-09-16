using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
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

namespace HassyaAllrightCloud.Application.LoanBookingIncidental.Commands
{
    public class SaveLoanBookingIncidentalDataCommand : IRequest<Unit>
    {
        private readonly LoanBookingIncidentalData _incidentalData;

        public SaveLoanBookingIncidentalDataCommand(LoanBookingIncidentalData incidentalData)
        {
            _incidentalData = incidentalData;
        }

        public class Handler : IRequestHandler<SaveLoanBookingIncidentalDataCommand, Unit>
        {
            private readonly KobodbContext _context;
            private readonly string _formName;

            public Handler(KobodbContext context)
            {
                _context = context;
                _formName = "KU2300";
            }

            public async Task<Unit> Handle(SaveLoanBookingIncidentalDataCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    // pre define
                    string ukeNo = request._incidentalData.UkeNo;
                    short unkRen = request._incidentalData.UnkRen;
                    int youTblSeq = request._incidentalData.YouTblSeq;
                    int futtumKbn;
                    int sihFutSyu;
                    switch (request._incidentalData.FuttumKbnMode)
                    {
                        case IncidentalViewMode.Futai:
                            futtumKbn = 1;
                            sihFutSyu = 2;
                            break;
                        case IncidentalViewMode.Tsumi:
                            futtumKbn = 2;
                            sihFutSyu = 6;
                            break;
                        case IncidentalViewMode.All:
                        default:
                            futtumKbn = 0;
                            sihFutSyu = 0;
                            break;
                    }
                    var yFutTuList = new Dictionary<FormEditState, List<TkdYfutTu>>()
                    {
                        { FormEditState.Added, new List<TkdYfutTu>() },
                        { FormEditState.Edited, new List<TkdYfutTu>() },
                        { FormEditState.Deleted, new List<TkdYfutTu>() },
                        { FormEditState.None, new List<TkdYfutTu>() },
                    };
                    var yMFutTuList = new List<TkdYmfuTu>();
                    var mihrimList = new List<TkdMihrim>();
                    var futtumList = new List<TkdFutTum>();
                    var mishumList = new List<TkdMishum>();
                    Func<TkdYfutTu, object> yfuttuSelecterKey = f => new { f.YouFutTumRen };
                    Func<TkdYmfuTu, object> ymfutuSelecterKey = f => new { f.YouFutTumRen, f.TeiDanNo, f.BunkRen };
                    Func<TkdMihrim, object> mihrimSelecterKey = f => new { f.YouFutTumRen };

                    // parse data ui to entity
                    CollectData(request, ref yFutTuList, ref yMFutTuList, ref mihrimList, ref futtumList, ref mishumList);
                    List<short> editDeleteYouFutTumRen = yFutTuList[FormEditState.Edited].Union(yFutTuList[FormEditState.Deleted]).Select(_ => _.YouFutTumRen).ToList();

                    // (s1): get data from database for [edit, delete] action only
                    var yFutTuListDb = await _context.TkdYfutTu
                        .Where(f => f.UkeNo == ukeNo
                                    && f.UnkRen == unkRen
                                    && f.YouTblSeq == youTblSeq
                                    && f.FutTumKbn == futtumKbn
                                    && f.SiyoKbn == 1
                                    && editDeleteYouFutTumRen.Contains(f.YouFutTumRen))
                        .ToListAsync();
                    var yMFutTuListDb = await _context.TkdYmfuTu
                        .Where(f => f.UkeNo == ukeNo
                                    && f.UnkRen == unkRen
                                    && f.YouTblSeq == youTblSeq
                                    && f.FutTumKbn == futtumKbn
                                    && editDeleteYouFutTumRen.Contains(f.YouFutTumRen))
                        .ToListAsync();
                    var mihrimListDb = await _context.TkdMihrim
                        .Where(f => f.UkeNo == ukeNo
                                    && f.UnkRen == unkRen
                                    && f.YouTblSeq == youTblSeq
                                    && f.SihFutSyu == sihFutSyu
                                    && f.YouFutTumRen != 0
                                    && f.SiyoKbn == 1
                                    && editDeleteYouFutTumRen.Contains(f.YouFutTumRen))
                        .ToListAsync();

                    // (s2): filter to specific list with corresponding action {insert, update, delete}
                    var addedYFutTuList = yFutTuList[FormEditState.Added];
                    var editedYFutTuListDb = yFutTuList[FormEditState.Edited]
                        .Join(yFutTuListDb, yfuttuSelecterKey, yfuttuSelecterKey, (s1, s2) => UpdateTkdYfutTu(s2, s1))
                        .ToList();
                    var deletedYFutTuList = yFutTuList[FormEditState.Deleted];

                    // (s3): take action
                    // s3-1: insert
                    if (addedYFutTuList.Count > 0)
                    {
                        var addedFuttumRenList = addedYFutTuList.Select(f => f.YouFutTumRen).ToList();

                        await _context.TkdYfutTu.AddRangeAsync(addedYFutTuList);
                        await _context.TkdYmfuTu.AddRangeAsync(yMFutTuList.Where(f => addedFuttumRenList.Contains(f.YouFutTumRen)));
                        await _context.TkdMihrim.AddRangeAsync(mihrimList.Where(f => addedFuttumRenList.Contains(f.YouFutTumRen)));
                    }
                    // s3-2: delete
                    if (deletedYFutTuList.Count > 0)
                    {
                        var deletedFuttumRenList = deletedYFutTuList.Select(f => f.YouFutTumRen).ToList();

                        var deletedYFutTuListDb = yFutTuListDb
                            .Where(_ => deletedFuttumRenList.Contains(_.YouFutTumRen))
                            .Select(f => MarkAsDeletedTkdYfutTu(f))
                            .ToList();
                        var deletedTkdYmfuTuListDb = yMFutTuListDb
                            .Where(f => deletedFuttumRenList.Contains(f.YouFutTumRen))
                            .Select(f => MarkAsDeletedTkdYmfuTu(f))
                            .ToList();
                        var deletedTkdMihrimListDb = mihrimListDb
                            .Where(f => deletedFuttumRenList.Contains(f.YouFutTumRen))
                            .Select(f => MarkAsDeletedTkdMihrim(f))
                            .ToList();

                        _context.TkdYfutTu.UpdateRange(deletedYFutTuListDb);
                        _context.TkdYmfuTu.UpdateRange(deletedTkdYmfuTuListDb);
                        _context.TkdMihrim.UpdateRange(deletedTkdMihrimListDb);
                    }
                    // s3-3: edit
                    if (editedYFutTuListDb.Count > 0)
                    {
                        var editedFuttumRenList = editedYFutTuListDb.Select(f => f.YouFutTumRen).ToList();

                        // update mihrim
                        var editedTkdMihrimList = mihrimList.Where(f => editedFuttumRenList.Contains(f.YouFutTumRen)).ToList();
                        mihrimListDb = mihrimListDb.Where(f => editedFuttumRenList.Contains(f.YouFutTumRen)).ToList();
                        var editedyMihrimListDb = mihrimList
                            .Join(mihrimListDb, mihrimSelecterKey, mihrimSelecterKey, (s1, s2) => UpdateTkdMihrim(s2, s1))
                            .ToList();

                        // update ymfutu
                        yMFutTuListDb = yMFutTuListDb.Where(f => editedFuttumRenList.Contains(f.YouFutTumRen)).ToList();
                        var editedTkdYmfuTuList = yMFutTuList.Where(f => editedFuttumRenList.Contains(f.YouFutTumRen)).ToList();
                        var addedyMFutTuList = editedTkdYmfuTuList
                            .Except(yMFutTuListDb, new TkdTkdYmfuTuComparer())
                            .ToList();
                        var deletedyMFutTuList = yMFutTuListDb
                            .Except(editedTkdYmfuTuList, new TkdTkdYmfuTuComparer())
                            .Select(f => MarkAsDeletedTkdYmfuTu(f))
                            .ToList();
                        var editedyMFutTuListDb = editedTkdYmfuTuList
                            .Join(yMFutTuListDb, ymfutuSelecterKey, ymfutuSelecterKey, (s1, s2) => UpdateTkdYmfuTu(s2, s1))
                            .ToList();

                        _context.TkdYfutTu.UpdateRange(editedYFutTuListDb);
                        _context.TkdMihrim.UpdateRange(editedyMihrimListDb);
                        await _context.TkdYmfuTu.AddRangeAsync(addedyMFutTuList);
                        _context.TkdYmfuTu.UpdateRange(editedyMFutTuListDb.Union(deletedyMFutTuList));
                    }

                    if (request._incidentalData.IsSaveMishumFuttum)
                    {
                        await _context.TkdFutTum.AddRangeAsync(futtumList);
                        await _context.TkdMishum.AddRangeAsync(mishumList);
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

            private TkdYfutTu UpdateTkdYfutTu(TkdYfutTu oldYFutTu, TkdYfutTu newYFutTu)
            {
                newYFutTu.UkeNo = oldYFutTu.UkeNo;
                newYFutTu.UnkRen = oldYFutTu.UnkRen;
                newYFutTu.YouTblSeq = oldYFutTu.YouTblSeq;
                newYFutTu.FutTumKbn = oldYFutTu.FutTumKbn;
                newYFutTu.YouFutTumRen = oldYFutTu.YouFutTumRen;
                newYFutTu.HenKai = ++oldYFutTu.HenKai;
                newYFutTu.SihKbn = 1;
                newYFutTu.ScouKbn = 1;
                newYFutTu.SiyoKbn = 1;

                return oldYFutTu.SimpleCloneProperties(newYFutTu);
            }

            private TkdYmfuTu UpdateTkdYmfuTu(TkdYmfuTu oldTkdYmfuTu, TkdYmfuTu newTkdYmfuTu)
            {
                newTkdYmfuTu.UkeNo = oldTkdYmfuTu.UkeNo;
                newTkdYmfuTu.UnkRen = oldTkdYmfuTu.UnkRen;
                newTkdYmfuTu.YouTblSeq = oldTkdYmfuTu.YouTblSeq;
                newTkdYmfuTu.FutTumKbn = oldTkdYmfuTu.FutTumKbn;
                newTkdYmfuTu.YouFutTumRen = oldTkdYmfuTu.YouFutTumRen;
                newTkdYmfuTu.TeiDanNo = oldTkdYmfuTu.TeiDanNo;
                newTkdYmfuTu.BunkRen = oldTkdYmfuTu.BunkRen;
                newTkdYmfuTu.HenKai = ++oldTkdYmfuTu.HenKai;

                return oldTkdYmfuTu.SimpleCloneProperties(newTkdYmfuTu);
            }

            private TkdMihrim UpdateTkdMihrim(TkdMihrim oldTkdMihrim, TkdMihrim newTkdMihrim)
            {
                newTkdMihrim.UkeNo = oldTkdMihrim.UkeNo;
                newTkdMihrim.MihRen = oldTkdMihrim.MihRen;
                newTkdMihrim.SihRaiRui = oldTkdMihrim.SihRaiRui;
                newTkdMihrim.CouKesRui = oldTkdMihrim.CouKesRui;
                newTkdMihrim.HenKai = ++oldTkdMihrim.HenKai;

                return oldTkdMihrim.SimpleCloneProperties(newTkdMihrim);
            }

            private TkdYfutTu MarkAsDeletedTkdYfutTu(TkdYfutTu deletedYFutTu)
            {
                deletedYFutTu.SiyoKbn = 2;
                deletedYFutTu.HenKai++;
                deletedYFutTu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                deletedYFutTu.UpdTime = DateTime.Now.ToString("HHmmss");
                deletedYFutTu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                deletedYFutTu.UpdPrgId = _formName;

                return deletedYFutTu;
            }

            private TkdYmfuTu MarkAsDeletedTkdYmfuTu(TkdYmfuTu deletedTkdYmfuTu)
            {
                deletedTkdYmfuTu.SiyoKbn = 2;
                deletedTkdYmfuTu.HenKai++;
                deletedTkdYmfuTu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                deletedTkdYmfuTu.UpdTime = DateTime.Now.ToString("HHmmss");
                deletedTkdYmfuTu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                deletedTkdYmfuTu.UpdPrgId = _formName;

                return deletedTkdYmfuTu;
            }

            private TkdMihrim MarkAsDeletedTkdMihrim(TkdMihrim deletedTkdMihrim)
            {
                deletedTkdMihrim.SiyoKbn = 2;
                deletedTkdMihrim.HenKai++;
                deletedTkdMihrim.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                deletedTkdMihrim.UpdTime = DateTime.Now.ToString("HHmmss");
                deletedTkdMihrim.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                deletedTkdMihrim.UpdPrgId = _formName;

                return deletedTkdMihrim;
            }

            private void CollectData(SaveLoanBookingIncidentalDataCommand request,
                ref Dictionary<FormEditState, List<TkdYfutTu>> yFutTuList,
                ref List<TkdYmfuTu> yMFutTuList,
                ref List<TkdMihrim> mihrimList,
                ref List<TkdFutTum> futtumList,
                ref List<TkdMishum> mishumList) 
            {
                string ukeNo = request._incidentalData.UkeNo;
                short unkRen = request._incidentalData.UnkRen;
                int youTblSeq = request._incidentalData.YouTblSeq;
                byte tesKbnFut = request._incidentalData.TesKbnFut;
                IncidentalViewMode viewMode = request._incidentalData.FuttumKbnMode;
                short mihRenMax = (short)_context.TkdMihrim
                    .Where(m => m.UkeNo == ukeNo)
                    .Select(m => Convert.ToInt32(m.MihRen))
                    .ToList()
                    .DefaultIfEmpty(0)
                    .Max();
                short misyuRenMax = (short)_context.TkdMishum
                    .Where(m => m.UkeNo == ukeNo)
                    .Select(m => Convert.ToInt32(m.MisyuRen))
                    .ToList()
                    .DefaultIfEmpty(0)
                    .Max();
                short futTumRenMax = (short)_context.TkdFutTum
                        .Where(f => f.UkeNo == ukeNo && f.UnkRen == unkRen && f.FutTumKbn == (int)viewMode)
                        .Select(m => Convert.ToInt32(m.FutTumRen))
                        .ToList()
                        .DefaultIfEmpty(0)
                        .Max();
                short youFutTumRenMax = (short)_context.TkdYfutTu
                        .Where(f => f.UkeNo == ukeNo && f.UnkRen == unkRen && f.FutTumKbn == (int)viewMode && f.YouTblSeq == youTblSeq)
                        .Select(m => Convert.ToInt32(m.YouFutTumRen))
                        .ToList()
                        .DefaultIfEmpty(0)
                        .Max();

                foreach (var loadYfuttu in request._incidentalData.LoadYFutTuList)
                {
                    short setYouFutTumRenMax = loadYfuttu.EditState == FormEditState.Added ? ++youFutTumRenMax : loadYfuttu.YouFutTumRen;
                    yFutTuList[loadYfuttu.EditState].Add(CollectDataYFutTu(loadYfuttu, ukeNo, unkRen, youTblSeq, viewMode, setYouFutTumRenMax));
                    if(loadYfuttu.EditState == FormEditState.Added && request._incidentalData.IsSaveMishumFuttum)
                    {
                        futtumList.Add(CollectDataFuttum(loadYfuttu, ukeNo, unkRen, viewMode, ++futTumRenMax));
                    }
                    if (int.Parse(loadYfuttu.Suryo) > 0 && loadYfuttu.SettingQuantityList.Sum(e => int.Parse(e.Suryo)) == int.Parse(loadYfuttu.Suryo))
                    {
                        yMFutTuList.AddRange(CollectDataYMFutTu(loadYfuttu, ukeNo, unkRen, youTblSeq, viewMode, setYouFutTumRenMax));
                    }
                    if (loadYfuttu.SaveType.Id == 1)
                    {
                        mihrimList.Add(CollectDataMihrim(loadYfuttu, ukeNo, unkRen, youTblSeq, viewMode, tesKbnFut, ref mihRenMax));
                        if (loadYfuttu.EditState == FormEditState.Added && request._incidentalData.IsSaveMishumFuttum)
                        {
                            mishumList.Add(CollectDataMishum(loadYfuttu, ukeNo, unkRen, viewMode, futTumRenMax, tesKbnFut, ref misyuRenMax));
                        }
                    }
                }
            }

            private TkdYfutTu CollectDataYFutTu(LoadYFutTu loadYfuttu, string ukeNo, short unkRen, int youTblSeq, IncidentalViewMode viewMode, short setYouFutTumRenMax)
            {
                var yfuttu = new TkdYfutTu();

                yfuttu.UkeNo = ukeNo;
                yfuttu.UnkRen = unkRen;
                yfuttu.YouTblSeq = youTblSeq;
                switch (viewMode)
                {
                    case IncidentalViewMode.Futai:
                        yfuttu.FutTumKbn = 1;
                        yfuttu.FutTumCdSeq = loadYfuttu.SelectedLoadYFutai.FutaiCdSeq;
                        break;
                    case IncidentalViewMode.Tsumi:
                        yfuttu.FutTumKbn = 2;
                        yfuttu.FutTumCdSeq = loadYfuttu.SelectedLoadYTsumi.CodeKbnSeq;
                        break;
                    case IncidentalViewMode.All:
                    default:
                        break;
                }
                yfuttu.FutTumNm = loadYfuttu.YFutTuNm;
                yfuttu.YouFutTumRen = setYouFutTumRenMax;
                yfuttu.HenKai = 0;
                yfuttu.Nittei = loadYfuttu.ScheduleDate.Nittei;
                yfuttu.TomKbn = loadYfuttu.ScheduleDate.TomKbn;
                yfuttu.HasYmd = loadYfuttu.ScheduleDate.Date.ToString("yyyyMMdd");
                yfuttu.IriRyoChiCd = loadYfuttu.SelectedLoadYRyoKin?.RyoKinTikuCd ?? 0;
                yfuttu.IriRyoCd = loadYfuttu.SelectedLoadYRyoKin?.RyoKinCd.ToString("D3") ?? string.Empty;
                yfuttu.IriRyoNm = loadYfuttu?.RyokinNm ?? string.Empty;
                yfuttu.DeRyoChiCd = loadYfuttu.SelectedLoadYShuRyoKin?.RyoKinTikuCd ?? 0;
                yfuttu.DeRyoCd = loadYfuttu.SelectedLoadYShuRyoKin?.RyoKinCd.ToString("D3") ?? string.Empty;
                yfuttu.DeRyoNm = loadYfuttu?.ShuRyokinNm ?? string.Empty;
                yfuttu.SeisanCdSeq = loadYfuttu.SelectedLoadYSeisan.SeisanCdSeq;
                yfuttu.SeisanNm = loadYfuttu?.SeisanNm ?? string.Empty;
                yfuttu.SeisanKbn = loadYfuttu.SaveType.Id;
                yfuttu.TanKa = int.Parse(loadYfuttu.Tanka);
                yfuttu.Suryo = short.Parse(loadYfuttu.Suryo);
                yfuttu.HaseiKin = loadYfuttu.GoukeiWithoutTax;
                yfuttu.ZeiKbn = Convert.ToByte(loadYfuttu.TaxType.IdValue);
                yfuttu.Zeiritsu = decimal.Parse(loadYfuttu.Zeiritsu);
                yfuttu.SyaRyoSyo = loadYfuttu.SyaRyoSyo;
                yfuttu.TesuRitu = decimal.Parse(loadYfuttu.TesuRitu);
                yfuttu.SyaRyoTes = loadYfuttu.SyaRyoTes;
                yfuttu.SihKbn = 1;
                yfuttu.ScouKbn = 1;
                yfuttu.SiyoKbn = 1;

                yfuttu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                yfuttu.UpdTime = DateTime.Now.ToString("HHmmss");
                yfuttu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                yfuttu.UpdPrgId = _formName;

                return yfuttu;
            }

            private List<TkdYmfuTu> CollectDataYMFutTu(LoadYFutTu loadYfuttu, string ukeNo, short unkRen, int youTblSeq, IncidentalViewMode viewMode, short setYouFutTumRenMax)
            {
                var result = new List<TkdYmfuTu>();
                foreach (var loadYmfuttu in loadYfuttu.SettingQuantityList)
                {
                    var yMfuttu = new TkdYmfuTu();

                    yMfuttu.UkeNo = ukeNo;
                    yMfuttu.UnkRen = unkRen;
                    yMfuttu.YouTblSeq = youTblSeq;
                    yMfuttu.YouFutTumRen = setYouFutTumRenMax;
                    yMfuttu.BunkRen = loadYmfuttu.BunkRen;
                    yMfuttu.TeiDanNo = loadYmfuttu.TeiDanNo;
                    switch (viewMode)
                    {
                        case IncidentalViewMode.Futai:
                            yMfuttu.FutTumKbn = 1;
                            break;
                        case IncidentalViewMode.Tsumi:
                            yMfuttu.FutTumKbn = 2;
                            break;
                        case IncidentalViewMode.All:
                        default:
                            break;
                    }
                    yMfuttu.HenKai = 0;
                    yMfuttu.Suryo = short.Parse(loadYmfuttu.Suryo);

                    int dividedTotal = yMfuttu.Suryo * int.Parse(loadYfuttu.Tanka);
                    if (loadYfuttu.TaxType.IdValue == Constants.ForeignTax.IdValue || loadYfuttu.TaxType.IdValue == Constants.NoTax.IdValue)
                    {
                        yMfuttu.HaseiKin = dividedTotal;
                    }
                    else
                    {
                        yMfuttu.HaseiKin = dividedTotal == 0
                            ? 0
                            : (int)(dividedTotal - (decimal.Parse(loadYfuttu.Zeiritsu) * dividedTotal) / (100 + decimal.Parse(loadYfuttu.Zeiritsu)));
                    }

                    yMfuttu.SyaRyoSyo = loadYfuttu.SyaRyoSyo * yMfuttu.Suryo / int.Parse(loadYfuttu.Suryo);
                    yMfuttu.SyaRyoTes = loadYfuttu.SyaRyoTes * yMfuttu.Suryo / int.Parse(loadYfuttu.Suryo);

                    yMfuttu.SiyoKbn = 1;
                    yMfuttu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    yMfuttu.UpdTime = DateTime.Now.ToString("HHmmss");
                    yMfuttu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    yMfuttu.UpdPrgId = _formName;

                    result.Add(yMfuttu);
                }

                int diffUrigakKin = loadYfuttu.GoukeiWithoutTax - result.Sum(f => f.HaseiKin);
                int assingeIndex = 0;
                int assingeUnit = 0;
                if (diffUrigakKin != 0)
                {
                    while(diffUrigakKin != 0)
                    {
                        assingeUnit = diffUrigakKin < 0 ? -1 : 1;
                        if(result[assingeIndex].HaseiKin > 0)
                        {
                            result[assingeIndex].HaseiKin += assingeUnit;
                            diffUrigakKin -= assingeUnit;
                        }
                        assingeIndex++;
                        if(assingeIndex == result.Count) assingeIndex = 0;
                    }
                }

                int diffSyaRyoSyo = loadYfuttu.SyaRyoSyo - result.Sum(f => f.SyaRyoSyo);
                if (diffSyaRyoSyo != 0)
                {
                    assingeIndex = 0;
                    while(diffSyaRyoSyo != 0)
                    {
                        assingeUnit = diffSyaRyoSyo < 0 ? -1 : 1;
                        if(result[assingeIndex].SyaRyoSyo > 0)
                        {
                            result[assingeIndex].SyaRyoSyo += assingeUnit;
                            diffSyaRyoSyo -= assingeUnit;
                        }
                        assingeIndex++;
                        if(assingeIndex == result.Count) assingeIndex = 0;
                    }
                }

                int diffSyaRyoTes = loadYfuttu.SyaRyoTes - result.Sum(f => f.SyaRyoTes);
                if (diffSyaRyoTes != 0)
                {
                    assingeIndex = 0;
                    while(diffSyaRyoTes != 0)
                    {
                        assingeUnit = diffSyaRyoTes < 0 ? -1 : 1;
                        if(result[assingeIndex].SyaRyoTes > 0)
                        {
                            result[assingeIndex].SyaRyoTes += assingeUnit;
                            diffSyaRyoTes -= assingeUnit;
                        }
                        assingeIndex++;
                        if(assingeIndex == result.Count) assingeIndex = 0;
                    }
                }
                return result;
            }

            private TkdMihrim CollectDataMihrim(LoadYFutTu loadYfuttu, string ukeNo, short unkRen, int youTblSeq, IncidentalViewMode viewMode, byte tesKbnFut, ref short mihRenMax)
            {
                var mishum = new TkdMihrim();
                mishum.UkeNo = ukeNo;
                mishum.MihRen = ++mihRenMax;
                mishum.HenKai = 0;
                switch (viewMode)
                {
                    case IncidentalViewMode.Futai:
                        mishum.SihFutSyu = 2;
                        break;
                    case IncidentalViewMode.Tsumi:
                        mishum.SihFutSyu = 6;
                        break;
                    default:
                        break;
                }
                mishum.UnkRen = unkRen;
                mishum.YouTblSeq = youTblSeq;

                mishum.HaseiKin = loadYfuttu.GoukeiWithoutTax;
                mishum.SyaRyoSyo = loadYfuttu.SyaRyoSyo;
                mishum.SyaRyoTes = loadYfuttu.SyaRyoTes;
                mishum.YoushaGak = loadYfuttu.ZeikomiKin;
                if (tesKbnFut == 2)// #8092
                {
                    mishum.YoushaGak -= loadYfuttu.SyaRyoTes;
                }

                mishum.SihRaiRui = 0;
                mishum.CouKesRui = 0;
                mishum.YouFutTumRen = loadYfuttu.YouFutTumRen;

                mishum.SiyoKbn = 1;
                mishum.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                mishum.UpdTime = DateTime.Now.ToString("HHmmss");
                mishum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishum.UpdPrgId = _formName;

                return mishum;
            }

            private TkdFutTum CollectDataFuttum(LoadYFutTu loadYFutTu, string ukeNo, short unkRen, IncidentalViewMode viewMode, short futTumRen)
            {
                var tkdFuttum = new TkdFutTum();

                tkdFuttum.UkeNo = ukeNo;
                tkdFuttum.UnkRen = unkRen;
                switch (viewMode)
                {
                    case IncidentalViewMode.Futai:
                        tkdFuttum.FutTumKbn = 1;
                        tkdFuttum.FutTumCdSeq = loadYFutTu.SelectedLoadYFutai.FutaiCdSeq;
                        break;
                    case IncidentalViewMode.Tsumi:
                        tkdFuttum.FutTumKbn = 2;
                        tkdFuttum.FutTumCdSeq = loadYFutTu.SelectedLoadYTsumi.CodeKbnSeq;
                        break;
                    default:
                        break;
                }
                tkdFuttum.FutTumRen = ++futTumRen;
                tkdFuttum.HenKai = 0;
                tkdFuttum.Nittei = loadYFutTu.ScheduleDate.Nittei;
                tkdFuttum.TomKbn = loadYFutTu.ScheduleDate.TomKbn;
                tkdFuttum.FutTumNm = loadYFutTu.YFutTuNm;
                tkdFuttum.HasYmd = loadYFutTu.ScheduleDate.Date.ToString("yyyyMMdd");
                tkdFuttum.IriRyoChiCd = loadYFutTu.SelectedLoadYRyoKin?.RyoKinTikuCd ?? 0;
                tkdFuttum.IriRyoCd = loadYFutTu.SelectedLoadYRyoKin?.RyoKinCd.ToString("D3") ?? string.Empty;
                tkdFuttum.IriRyoNm = loadYFutTu.RyokinNm ?? string.Empty;
                tkdFuttum.DeRyoChiCd = loadYFutTu.SelectedLoadYShuRyoKin?.RyoKinTikuCd ?? 0;
                tkdFuttum.DeRyoCd = loadYFutTu.SelectedLoadYShuRyoKin?.RyoKinCd.ToString("D3") ?? string.Empty;
                tkdFuttum.DeRyoNm = loadYFutTu.ShuRyokinNm ?? string.Empty;
                tkdFuttum.SeisanCdSeq = loadYFutTu.SelectedLoadYSeisan.SeisanCdSeq;
                tkdFuttum.SeisanNm = loadYFutTu.SeisanNm;
                tkdFuttum.SeisanKbn = loadYFutTu.SaveType.Id;
                tkdFuttum.TanKa = int.Parse(loadYFutTu.Tanka);
                tkdFuttum.Suryo = short.Parse(loadYFutTu.Suryo);
                tkdFuttum.UriGakKin = loadYFutTu.GoukeiWithoutTax;
                tkdFuttum.ZeiKbn = Convert.ToByte(loadYFutTu.TaxType.IdValue);
                tkdFuttum.Zeiritsu = decimal.Parse(loadYFutTu.Zeiritsu);
                tkdFuttum.SyaRyoSyo = loadYFutTu.SyaRyoSyo;
                tkdFuttum.TesuRitu = decimal.Parse(loadYFutTu.TesuRitu);
                tkdFuttum.SyaRyoTes = loadYFutTu.SyaRyoTes;
                tkdFuttum.NyuKinKbn = 1;
                tkdFuttum.NcouKbn = 1;
                tkdFuttum.BikoNm = string.Empty;
                tkdFuttum.ExpItem = string.Empty;
                tkdFuttum.SortJun = 0;
                tkdFuttum.SirSitenCdSeq = 0;
                tkdFuttum.SireCdSeq = 0;
                tkdFuttum.SirTanKa = 0;
                tkdFuttum.SirSuryo = 0;
                tkdFuttum.SirGakKin = 0;
                tkdFuttum.SirZeiKbn = 1;
                tkdFuttum.SirZeiritsu = 0;
                tkdFuttum.SirSyaRyoSyo = 0;
                tkdFuttum.SiyoKbn = 1;

                tkdFuttum.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                tkdFuttum.UpdTime = DateTime.Now.ToString("HHmmss");
                tkdFuttum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdFuttum.UpdPrgId = _formName;

                return tkdFuttum;
            }

            private TkdMishum CollectDataMishum(LoadYFutTu loadMihrim, string ukeNo, short unkRen, IncidentalViewMode viewMode, short futTumRen, byte tesKbnFut, ref short misyuRenMax)
            {
                var mishum = new TkdMishum();
                mishum.UkeNo = ukeNo;
                mishum.MisyuRen = ++misyuRenMax;
                mishum.HenKai = 0;
                switch (viewMode)
                {
                    case IncidentalViewMode.Futai:
                        mishum.SeiFutSyu = 2;
                        break;
                    case IncidentalViewMode.Tsumi:
                        mishum.SeiFutSyu = 6;
                        break;
                    default:
                        break;
                }
                mishum.FutTumRen = futTumRen;
                mishum.UriGakKin = loadMihrim.GoukeiWithoutTax;
                mishum.SyaRyoSyo = loadMihrim.SyaRyoSyo;
                mishum.SyaRyoTes = loadMihrim.SyaRyoTes;
                mishum.SeiKin = loadMihrim.ZeikomiKin;
                if (tesKbnFut == 2)// #8092
                {
                    mishum.SeiKin -= loadMihrim.SyaRyoTes;
                }
                mishum.NyuKinRui = 0;
                mishum.CouKesRui = 0;
                mishum.FutuUnkRen = unkRen;
                mishum.SiyoKbn = 1;
                mishum.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                mishum.UpdTime = DateTime.Now.ToString("HHmmss");
                mishum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishum.UpdPrgId = _formName;

                return mishum;
            }
        }
    }
}
