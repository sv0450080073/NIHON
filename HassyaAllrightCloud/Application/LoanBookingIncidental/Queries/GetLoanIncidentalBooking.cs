using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LoanBookingIncidental.Queries
{
    public class GetLoanIncidentalBooking : IRequest<LoanBookingIncidentalData>
    {
        private readonly int _tenantId;
        private readonly string _ukeNo;
        private readonly short _unkRen;
        private readonly int _youTblSeq;
        private readonly IncidentalViewMode _viewMode;

        public GetLoanIncidentalBooking(int tenantId, string ukeNo, short unkRen, int youTblSeq, IncidentalViewMode viewMode)
        {
            _tenantId = tenantId;
            _ukeNo = ukeNo;
            _unkRen = unkRen;
            _youTblSeq = youTblSeq;
            _viewMode = viewMode;
        }

        public class Handler : IRequestHandler<GetLoanIncidentalBooking, LoanBookingIncidentalData>
        {
            private readonly KobodbContext _context;
            private readonly IRoundSettingsService _roundSettingsService;
            public Handler(KobodbContext context, IRoundSettingsService roundSettingsService)
            {
                _context = context;
                _roundSettingsService = roundSettingsService;
            }

            public async Task<LoanBookingIncidentalData> Handle(GetLoanIncidentalBooking request, CancellationToken cancellationToken)
            {
                try
                {
                    var query1Result = await (from yousha in _context.TkdYousha
                                              join unkobi in _context.TkdUnkobi.Where(u => u.SiyoKbn == 1)
                                                on new { yousha.UkeNo, yousha.UnkRen } equals new { unkobi.UkeNo, unkobi.UnkRen } into unkobiGr
                                              from unkobi in unkobiGr.DefaultIfEmpty()
                                              from youtokisk in _context.VpmTokisk
                                                                            .Where(t => t.TenantCdSeq == request._tenantId
                                                                                && yousha.YouCdSeq == t.TokuiSeq
                                                                                && string.Compare(t.SiyoStaYmd, unkobi.HaiSymd) <= 0
                                                                                && string.Compare(t.SiyoEndYmd, unkobi.HaiSymd) >= 0).DefaultIfEmpty()
                                              from youtokist in _context.VpmTokiSt
                                                                            .Where(t => t.TokuiSeq == yousha.YouCdSeq
                                                                                && t.SitenCdSeq == yousha.YouSitCdSeq
                                                                                && string.Compare(t.SiyoStaYmd, unkobi.HaiSymd) <= 0
                                                                                && string.Compare(t.SiyoEndYmd, unkobi.HaiSymd) >= 0).DefaultIfEmpty()
                                              where yousha.UkeNo == request._ukeNo
                                                && yousha.UnkRen == request._unkRen
                                                && yousha.YouTblSeq == request._youTblSeq
                                                && yousha.SiyoKbn == 1
                                              select new
                                              {
                                                  YoushaUkeno = yousha.UkeNo,
                                                  YoushaUnkren = yousha.UnkRen,
                                                  YoushaYoutblseq = yousha.YouTblSeq,
                                                  YoushaYoucdseq = yousha.YouCdSeq,
                                                  YoushaYousitcdseq = yousha.YouSitCdSeq,
                                                  YoutokiskTokuicd = youtokisk.TokuiCd,
                                                  YoutokiskRyakunm = youtokisk.RyakuNm,
                                                  YoutokistSitencd = youtokist.SitenCd,
                                                  YoutokistRyakunm = youtokist.RyakuNm,
                                                  YoutokistTesuritufut = youtokist.TesuRituFut,
                                                  UnkobiHaisymd = unkobi.HaiSymd,
                                                  UnkobiTouymd = unkobi.TouYmd,
                                                  UnkobiSyukoymd = unkobi.SyukoYmd,
                                                  UnkobiKikymd = unkobi.KikYmd,
                                                  UnkobiZenhaflg = unkobi.ZenHaFlg,
                                                  UnkobiKhakflg = unkobi.KhakFlg,
                                                  TesKbnFut = youtokist.TesKbnFut
                                              }).SingleOrDefaultAsync();

                    var loanIncidentalBooking = new LoanBookingIncidentalData();
                    loanIncidentalBooking.UkeNo = request._ukeNo;
                    loanIncidentalBooking.UnkRen = request._unkRen;
                    loanIncidentalBooking.YouTblSeq = request._youTblSeq;
                    loanIncidentalBooking.FuttumKbnMode = request._viewMode;
                    loanIncidentalBooking.TokuiSiten = string.Format("{0:D4}:{1} {2:D4}:{3}",
                        query1Result.YoutokiskTokuicd, query1Result.YoutokiskRyakunm, query1Result.YoutokistSitencd, query1Result.YoutokistRyakunm);
                    loanIncidentalBooking.TesKbnFut = query1Result.TesKbnFut;

                    if (DateTime.TryParseExact(query1Result.UnkobiHaisymd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime dateTime))
                    {
                        loanIncidentalBooking.HaiSYmd = dateTime;
                    }
                    if (DateTime.TryParseExact(query1Result.UnkobiTouymd, "yyyyMMdd", null, DateTimeStyles.None, out dateTime))
                    {
                        loanIncidentalBooking.TouYmd = dateTime;
                    }
                    loanIncidentalBooking.DefaultFutaiChargeRate = query1Result.YoutokistTesuritufut;
                    loanIncidentalBooking.IsPreviousDay = Convert.ToBoolean(query1Result.UnkobiZenhaflg);
                    loanIncidentalBooking.RoundType = await GetRoundType();
                    loanIncidentalBooking.IsAfterDay = Convert.ToBoolean(query1Result.UnkobiKhakflg);
                    loanIncidentalBooking.YouFutTumRenMax = await GetYouFuttumRenMax(request);

                    loanIncidentalBooking.SettingQuantityList = await GetSettingQuantitySummary(request);
                    loanIncidentalBooking.LoadYFutTuList = await GetLoadYFutTuList(request);
                    foreach (var item in loanIncidentalBooking.LoadYFutTuList)
                    {
                        item.RoundType = loanIncidentalBooking.RoundType;
                        item.SettingQuantityList = await GetSettingQuantityListForEachItem(request, item.YouFutTumRen, item.ScheduleDate.Date.ToString("yyyyMMdd"));
                    }

                    return loanIncidentalBooking;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private async Task<List<SettingQuantity>> GetSettingQuantitySummary(GetLoanIncidentalBooking request)
            {
                var result = new List<SettingQuantity>();
                var queryResult = await (from haisha in _context.TkdHaisha
                                         join yousha in _context.TkdYousha.Where(y => y.SiyoKbn == 1)
                                           on new { haisha.UkeNo, haisha.UnkRen, haisha.YouTblSeq }
                                           equals new { yousha.UkeNo, yousha.UnkRen, yousha.YouTblSeq } into youshaGr
                                         from yousha in youshaGr.DefaultIfEmpty()
                                         from youshasaki in _context.VpmTokisk
                                                                       .Where(t => t.TenantCdSeq == request._tenantId
                                                                           && yousha.YouCdSeq == t.TokuiSeq
                                                                           && string.Compare(t.SiyoStaYmd, haisha.HaiSymd) <= 0
                                                                           && string.Compare(t.SiyoEndYmd, haisha.HaiSymd) >= 0).DefaultIfEmpty()
                                         from youshasakisiten in _context.VpmTokiSt
                                                                       .Where(t => t.TokuiSeq == yousha.YouCdSeq
                                                                           && t.SitenCdSeq == yousha.YouSitCdSeq
                                                                           && string.Compare(t.SiyoStaYmd, haisha.HaiSymd) <= 0
                                                                           && string.Compare(t.SiyoEndYmd, haisha.HaiSymd) >= 0).DefaultIfEmpty()
                                         where haisha.UkeNo == request._ukeNo
                                               && haisha.UnkRen == request._unkRen
                                               && haisha.YouTblSeq == request._youTblSeq
                                               && haisha.SiyoKbn == 1
                                         select new
                                         {
                                             BunkRen = haisha.BunkRen,
                                             BunKSyuJyn = haisha.BunKsyuJyn,
                                             GoSyaJyn = haisha.GoSyaJyn,
                                             GoSya = haisha.GoSya,
                                             Suryo = 0,
                                             UnkRen = haisha.UnkRen,
                                             TeiDanNo = haisha.TeiDanNo,
                                             YouSha = youshasaki.RyakuNm + " " + youshasakisiten.RyakuNm,
                                             LeaveDate = haisha.SyuKoYmd,
                                             ReturnDate = haisha.KikYmd
                                         }).ToListAsync();
                queryResult.ForEach(item =>
                {
                    var itemResult = new SettingQuantity();
                    itemResult.BunkRen = item.BunkRen;
                    itemResult.BunKSyuJyn = item.BunKSyuJyn;
                    itemResult.GoSyaJyn = item.GoSyaJyn;
                    itemResult.GoSya = item.GoSya;
                    itemResult.Suryo = "0";
                    itemResult.UnkRen = item.UnkRen;
                    itemResult.TeiDanNo = item.TeiDanNo;
                    itemResult.YouSha = item.YouSha;
                    if (DateTime.TryParseExact(item.ReturnDate, "yyyyMMdd", null, DateTimeStyles.None, out DateTime parseDt))
                    {
                        itemResult.GarageReturnDate = parseDt;
                    }
                    if (DateTime.TryParseExact(item.LeaveDate, "yyyyMMdd", null, DateTimeStyles.None, out parseDt))
                    {
                        itemResult.GarageLeaveDate = parseDt;
                    }
                    result.Add(itemResult);
                });
                return result;
            }

            private async Task<List<SettingQuantity>> GetSettingQuantityListForEachItem(GetLoanIncidentalBooking request, short youFutTumRen, string scheduleDate)
            {
                var result = new List<SettingQuantity>();
                var queryResult = await (from haisha in _context.TkdHaisha
                                         join ymfutu in _context.TkdYmfuTu.Where(m => m.FutTumKbn == (int)request._viewMode && m.SiyoKbn == 1 && m.YouFutTumRen == youFutTumRen)
                                           on new { haisha.UkeNo, haisha.UnkRen, haisha.YouTblSeq, haisha.TeiDanNo, haisha.BunkRen }
                                           equals new { ymfutu.UkeNo, ymfutu.UnkRen, ymfutu.YouTblSeq, ymfutu.TeiDanNo, ymfutu.BunkRen } into ymfutuGr
                                         from ymfutu in ymfutuGr.DefaultIfEmpty()
                                         join yousha in _context.TkdYousha.Where(y => y.SiyoKbn == 1)
                                           on new { haisha.UkeNo, haisha.UnkRen, haisha.YouTblSeq }
                                           equals new { yousha.UkeNo, yousha.UnkRen, yousha.YouTblSeq } into youshaGr
                                         from yousha in youshaGr.DefaultIfEmpty()
                                         from youshasaki in _context.VpmTokisk
                                                                       .Where(t => t.TenantCdSeq == request._tenantId
                                                                           && yousha.YouCdSeq == t.TokuiSeq
                                                                           && string.Compare(t.SiyoStaYmd, haisha.HaiSymd) <= 0
                                                                           && string.Compare(t.SiyoEndYmd, haisha.HaiSymd) >= 0).DefaultIfEmpty()
                                         from youshasakisiten in _context.VpmTokiSt
                                                                       .Where(t => t.TokuiSeq == yousha.YouCdSeq
                                                                           && t.SitenCdSeq == yousha.YouSitCdSeq
                                                                           && string.Compare(t.SiyoStaYmd, haisha.HaiSymd) <= 0
                                                                           && string.Compare(t.SiyoEndYmd, haisha.HaiSymd) >= 0).DefaultIfEmpty()
                                         where haisha.UkeNo == request._ukeNo
                                               && haisha.UnkRen == request._unkRen
                                               && haisha.YouTblSeq == request._youTblSeq
                                               && haisha.SiyoKbn == 1
                                               && string.Compare(haisha.SyuKoYmd, scheduleDate) <= 0
                                               && string.Compare(haisha.KikYmd, scheduleDate) >= 0
                                         select new
                                         {
                                             BunkRen = haisha.BunkRen,
                                             BunKSyuJyn = haisha.BunKsyuJyn,
                                             GoSyaJyn = haisha.GoSyaJyn,
                                             GoSya = haisha.GoSya,
                                             Suryo = ymfutu.Suryo.ToString(),
                                             UnkRen = haisha.UnkRen,
                                             TeiDanNo = haisha.TeiDanNo,
                                             YouSha = youshasaki.RyakuNm + " " + youshasakisiten.RyakuNm,
                                             LeaveDate = haisha.SyuKoYmd,
                                             ReturnDate = haisha.KikYmd
                                         }).ToListAsync();
                queryResult.ForEach(item =>
                {
                    var itemResult = new SettingQuantity();
                    itemResult.BunkRen = item.BunkRen;
                    itemResult.BunKSyuJyn = item.BunKSyuJyn;
                    itemResult.GoSyaJyn = item.GoSyaJyn;
                    itemResult.GoSya = item.GoSya;
                    itemResult.Suryo = item.Suryo?.ToString() ?? "0";
                    itemResult.UnkRen = item.UnkRen;
                    itemResult.TeiDanNo = item.TeiDanNo;
                    itemResult.YouSha = item.YouSha;
                    if (DateTime.TryParseExact(item.ReturnDate, "yyyyMMdd", null, DateTimeStyles.None, out DateTime parseDt))
                    {
                        itemResult.GarageReturnDate = parseDt;
                    }
                    if (DateTime.TryParseExact(item.LeaveDate, "yyyyMMdd", null, DateTimeStyles.None, out parseDt))
                    {
                        itemResult.GarageLeaveDate = parseDt;
                    }
                    result.Add(itemResult);
                });
                return result;
            }

            private async Task<List<LoadYFutTu>> GetLoadYFutTuList(GetLoanIncidentalBooking request)
            {
                var codekb = _context.VpmCodeKb.Where(c => c.TenantCdSeq == request._tenantId && c.SiyoKbn == 1);
                return await (from yfuttu in _context.TkdYfutTu
                              join yousha in _context.TkdYousha.Where(y => y.SiyoKbn == 1)
                                on new { yfuttu.UkeNo, yfuttu.UnkRen, yfuttu.YouTblSeq } equals new { yousha.UkeNo, yousha.UnkRen, yousha.YouTblSeq } into youshaGr
                              from yousha in youshaGr.DefaultIfEmpty()
                              from youtokisk in _context.VpmTokisk
                                                            .Where(t => t.TenantCdSeq == request._tenantId
                                                                && yousha.YouCdSeq == t.TokuiSeq
                                                                && string.Compare(t.SiyoStaYmd, yousha.HasYmd) <= 0
                                                                && string.Compare(t.SiyoEndYmd, yousha.HasYmd) >= 0).DefaultIfEmpty()
                              from youtokist in _context.VpmTokiSt
                                                            .Where(t => t.TokuiSeq == yousha.YouCdSeq
                                                                && t.SitenCdSeq == yousha.YouSitCdSeq
                                                                && string.Compare(t.SiyoStaYmd, yousha.HasYmd) <= 0
                                                                && string.Compare(t.SiyoEndYmd, yousha.HasYmd) >= 0).DefaultIfEmpty()
                              join seisankbn in codekb.Where(c => c.CodeSyu == "SEISANKBN")
                                on new { yfuttu.SeisanKbn } equals new { SeisanKbn = Convert.ToByte(seisankbn.CodeKbn) } into seisankbnGr
                              from seisankbn in seisankbnGr.DefaultIfEmpty()
                              join tomkbn in codekb.Where(c => c.CodeSyu == "TOMKBN")
                                on new { yfuttu.TomKbn } equals new { TomKbn = Convert.ToByte(tomkbn.CodeKbn) } into tomkbnGr
                              from tomkbn in tomkbnGr.DefaultIfEmpty()
                              join zeikbn in codekb.Where(c => c.CodeSyu == "FUTZEIKBN")
                                on new { yfuttu.ZeiKbn } equals new { ZeiKbn = Convert.ToByte(zeikbn.CodeKbn) } into zeikbnGr
                              from zeikbn in zeikbnGr.DefaultIfEmpty()
                              where yfuttu.UkeNo == request._ukeNo
                                && yfuttu.UnkRen == request._unkRen
                                && yfuttu.YouTblSeq == request._youTblSeq
                                && yfuttu.FutTumKbn == (int)request._viewMode
                                && yfuttu.SiyoKbn == 1
                              select new LoadYFutTu()
                              {
                                  IsSetGoukeiFromHaseiKin = true,
                                  YouFutTumRen = yfuttu.YouFutTumRen,
                                  FuttumKbnMode = (IncidentalViewMode)yfuttu.FutTumKbn,
                                  ScheduleDate = new Commons.Helpers.ScheduleSelectorModel()
                                  {
                                      Nittei = yfuttu.Nittei,
                                      TomKbn = yfuttu.TomKbn,
                                      DateString = yfuttu.HasYmd
                                  },
                                  YFutTuNm = yfuttu.FutTumNm,
                                  SelectedLoadYFutai = new LoadYFutai()
                                  {
                                      FutaiCdSeq = yfuttu.FutTumCdSeq
                                  },
                                  SelectedLoadYTsumi = new LoadYTsumi()
                                  {
                                      CodeKbnSeq = yfuttu.FutTumCdSeq
                                  },
                                  RyokinNm = yfuttu.IriRyoNm,
                                  SelectedLoadYRyoKin = new LoadYRyokin()
                                  {
                                      RyoKinCd = Convert.ToInt16(yfuttu.IriRyoCd),
                                      RyoKinTikuCd = yfuttu.IriRyoChiCd
                                  },
                                  ShuRyokinNm = yfuttu.DeRyoNm,
                                  SelectedLoadYShuRyoKin = new LoadYRyokin()
                                  {
                                      RyoKinCd = Convert.ToInt16(yfuttu.DeRyoCd),
                                      RyoKinTikuCd = yfuttu.DeRyoChiCd
                                  },
                                  SeisanNm = yfuttu.SeisanNm,
                                  SelectedLoadYSeisan = new LoadYSeisan()
                                  {
                                      SeisanCdSeq = yfuttu.SeisanCdSeq
                                  },
                                  SaveType = new YouShaSaveType()
                                  {
                                      Id = yfuttu.SeisanKbn
                                  },
                                  Suryo = yfuttu.Suryo.ToString(),
                                  Tanka = yfuttu.TanKa.ToString(),
                                  Zeiritsu = yfuttu.Zeiritsu.ToString(),
                                  //SyaRyoSyo = yfuttu.SyaRyoSyo.ToString(),
                                  TesuRitu = yfuttu.TesuRitu.ToString(),
                                  //SyaRyoTes = yfuttu.SyaRyoTes.ToString(),
                                  TaxType = new TaxTypeList()
                                  {
                                      IdValue = yfuttu.ZeiKbn
                                  },
                                  //Goukei = yfuttu.HaseiKin.ToString(), // load last
                              }).ToListAsync();
            }

            private async Task<short> GetYouFuttumRenMax(GetLoanIncidentalBooking request)
            {
                var futtumRenMax = (short)(await _context.TkdYfutTu
                                                            .Where(f => f.UkeNo == request._ukeNo
                                                                && f.UnkRen == request._unkRen
                                                                && f.YouTblSeq == request._youTblSeq
                                                                && f.FutTumKbn == (int)request._viewMode)
                                                            .Select(f => Convert.ToInt32(f.YouFutTumRen))
                                                            .ToListAsync())
                                                            .DefaultIfEmpty(0).Max(f => f);
                return futtumRenMax;
            }

            private async Task<RoundTaxAmountType> GetRoundType()
            {
                var syohiHasu = (byte)(await _roundSettingsService.GetHasuSettings(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID)).TaxSetting;
                //var syohiHasu = await _context.TkmKasSet.Where(k => k.CompanyCdSeq == 1).Select(k => k.SyohiHasu).SingleOrDefaultAsync();
                return (RoundTaxAmountType)syohiHasu;
            }
        }
    }
}
