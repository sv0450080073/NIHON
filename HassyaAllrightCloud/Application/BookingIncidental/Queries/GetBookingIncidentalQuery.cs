using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
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

namespace HassyaAllrightCloud.Application.BookingIncidental.Queries
{
    public class GetBookingIncidentalQuery : IRequest<IncidentalBooking>
    {
        private readonly string _ukeNo;
        private readonly int _tenantId;
        private readonly int _companyId;
        private readonly IncidentalViewMode _viewMode;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetBookingIncidentalQuery(ITPM_CodeSyService codeSyuService, string ukeNo, int tenantId, int companyId, IncidentalViewMode viewMode)
        {
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _tenantId = tenantId;
            _companyId = companyId;
            _viewMode = viewMode;
        }

        public class Handler : IRequestHandler<GetBookingIncidentalQuery, IncidentalBooking>
        {
            private readonly KobodbContext _context;
            private readonly IRoundSettingsService _roundSettingsService;

            public Handler(KobodbContext context, IRoundSettingsService roundSettingsService)
            {
                _context = context;
                _roundSettingsService = roundSettingsService;
            }

            public async Task<IncidentalBooking> Handle(GetBookingIncidentalQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var yyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request._ukeNo).FirstOrDefaultAsync();
                    if (yyksho == null) return null;
                    var unkobi = await _context.TkdUnkobi.Where(x => x.UkeNo == request._ukeNo).FirstOrDefaultAsync();
                    var syaRyoNmList = await (from h in _context.TkdHaisha
                                              join s in _context.VpmSyaRyo on h.HaiSsryCdSeq equals s.SyaRyoCdSeq
                                              where h.UkeNo == request._ukeNo && h.SiyoKbn == 1
                                              select new
                                              {
                                                  h.HaiSsryCdSeq,
                                                  s.SyaRyoNm,
                                                  h.YouTblSeq,
                                                  h.BunkRen
                                              }).ToListAsync();
                    var youSha = await (from t in _context.VpmTokiSt
                                        let code = (from h in _context.TkdHaisha
                                                    join y in _context.TkdYousha
                                                    on new { h.UkeNo, h.YouTblSeq } equals new { y.UkeNo, y.YouTblSeq }
                                                    where h.UkeNo == request._ukeNo && h.SiyoKbn == 1
                                                    select new { y.YouCdSeq, y.YouSitCdSeq }).SingleOrDefault()
                                        where t.TokuiSeq == code.YouCdSeq && t.SitenCdSeq == code.YouSitCdSeq
                                        select t.RyakuNm).SingleOrDefaultAsync();
                    var futaiChargeRate = (from tokist in _context.VpmTokiSt
                                           join tokisk in _context.VpmTokisk
                                           on tokist.TokuiSeq equals tokisk.TokuiSeq
                                           where tokist.TokuiSeq == yyksho.TokuiSeq
                                           && tokist.SitenCdSeq == yyksho.SitenCdSeq
                                           && tokisk.TenantCdSeq == yyksho.TenantCdSeq
                                           && tokist.SiyoStaYmd.CompareTo(unkobi.HaiSymd) <= 0
                                           && tokist.SiyoEndYmd.CompareTo(unkobi.HaiSymd) >= 0
                                           select tokist.TesuRituFut).SingleOrDefault();
                    var haishaList = await (from h in _context.TkdHaisha
                                            join u in _context.TkdUnkobi
                                            on new { h.UkeNo, h.UnkRen } equals new { u.UkeNo, u.UnkRen } into uGr
                                            from uSub in uGr.DefaultIfEmpty()
                                            where h.UkeNo == request._ukeNo && h.SiyoKbn == 1
                                            select h).ToListAsync();
                    var yoyKbnList = await _context.VpmYoyKbn.ToListAsync();
                    var vpmSyaRyoList = await _context.VpmSyaRyo.ToListAsync();

                    var tokuiSiten = await (from tokisk in _context.VpmTokisk
                                            where tokisk.TenantCdSeq == request._tenantId
                                            join tokist in _context.VpmTokiSt on tokisk.TokuiSeq equals tokist.TokuiSeq into gr
                                            from tokist in gr.DefaultIfEmpty()
                                            where tokist.TokuiSeq == yyksho.TokuiSeq && tokist.SitenCdSeq == yyksho.SitenCdSeq
                                            select new {tokist.RyakuNm, tokist.TesKbnFut, Tokui = tokisk.RyakuNm}).SingleOrDefaultAsync();
                    var workingBranchList = await (from ei in _context.VpmEigyos
                                                   join he in _context.VpmHenSya on ei.EigyoCdSeq equals he.EigyoCdSeq
                                                   join ha in _context.TkdHaisha on he.SyaRyoCdSeq equals ha.HaiSsryCdSeq
                                                   where ha.UkeNo == request._ukeNo && ha.SiyoKbn == 1
                                                   select new
                                                   {
                                                       ei.RyakuNm,
                                                       ha.TeiDanNo,
                                                       ha.BunkRen,
                                                       he.StaYmd,
                                                       he.EndYmd
                                                   }).ToListAsync();
                    //var roundCode = _context.TkmKasSet.SingleOrDefault(c => c.CompanyCdSeq == request._companyId)?.SyohiHasu ?? 0;
                    var roundCode = (byte)(await _roundSettingsService.GetHasuSettings(request._companyId)).TaxSetting;
                    var futtumRenMax = _context.TkdFutTum.Where(f => f.UkeNo == request._ukeNo && f.FutTumKbn == (int)request._viewMode)
                                                            .Select(f => Convert.ToInt32(f.FutTumRen))
                                                            .ToList()
                                                            .DefaultIfEmpty(0).Max(f => f);

                    var incidental = new IncidentalBooking();
                    incidental.IsEditMode = await _context.TkdFutTum.Where(f => f.UkeNo == request._ukeNo && f.FutTumKbn == (int)request._viewMode).CountAsync() > 0;
                    incidental.FuttumRenMax = (short)futtumRenMax;
                    incidental.DefaultFutaiChargeRate = futaiChargeRate;
                    incidental.RoundType = (RoundTaxAmountType)roundCode;
                    incidental.UnkRen = unkobi.UnkRen;
                    incidental.YoyaKbn = yoyKbnList.FirstOrDefault(y => y.YoyaKbnSeq == yyksho.YoyaKbnSeq && y.TenantCdSeq == new ClaimModel().TenantID).YoyaKbnNm;
                    if (DateTime.TryParseExact(unkobi.HaiSymd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime parseDt))
                    {
                        incidental.HaiSYmd = parseDt;
                    }
                    if (DateTime.TryParseExact(unkobi.TouYmd, "yyyyMMdd", null, DateTimeStyles.None, out parseDt))
                    {
                        incidental.TouYmd = parseDt;
                    }
                    if (DateTime.TryParseExact(yyksho.UkeYmd, "yyyyMMdd", null, DateTimeStyles.None, out parseDt))
                    {
                        incidental.UkeYmd = parseDt;
                    }
                    incidental.UkeCd = yyksho.UkeCd;
                    incidental.DanTaNm = unkobi.DanTaNm;
                    incidental.Tokui = tokuiSiten.Tokui;
                    incidental.TokuSiten = tokuiSiten.RyakuNm;
                    incidental.TesKbnFut = tokuiSiten.TesKbnFut;

                    incidental.TokuiTanNm = yyksho.TokuiTanNm;
                    incidental.TokuiTel = yyksho.TokuiTel;
                    incidental.TokuiFax = yyksho.TokuiFax;
                    incidental.IkNm = unkobi.IkNm;
                    incidental.HaiSNm = unkobi.HaiSnm;
                    BookingInputHelper.MyTime tempTime = new BookingInputHelper.MyTime();
                    BookingInputHelper.MyTime.TryParse(unkobi.HaiStime, out tempTime);
                    incidental.HaiSTime = tempTime ?? new BookingInputHelper.MyTime();
                    BookingInputHelper.MyTime.TryParse(unkobi.SyuPaTime, out tempTime);
                    incidental.SyuPaTime = tempTime ?? new BookingInputHelper.MyTime();
                    incidental.TouNm = unkobi.TouNm;
                    BookingInputHelper.MyTime.TryParse(unkobi.TouChTime, out tempTime);
                    incidental.TouChTime = tempTime ?? new BookingInputHelper.MyTime();
                    incidental.JyoSyaJin = unkobi.JyoSyaJin;
                    incidental.PlusJin = unkobi.PlusJin;
                    incidental.DrvJin = unkobi.DrvJin;
                    incidental.GuiSu = unkobi.GuiSu;

                    incidental.OthJinKbn1 = (await request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) =>
                    {
                            return await 
                                (from c in _context.VpmCodeKb
                                where c.TenantCdSeq == tenantId && c.CodeSyu == codeSyu && Convert.ToByte(c.CodeKbn) == unkobi.OthJinKbn1
                                select c)
                                .ToListAsync();
                    }, request._tenantId, "OTHJINKBN"))
                    .SingleOrDefault()?.RyakuNm ?? string.Empty;
                    incidental.OthJin1 = unkobi.OthJin1;

                    incidental.OthJinKbn2 = (await request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) =>
                    {
                        return await
                            (from c in _context.VpmCodeKb
                             where c.TenantCdSeq == tenantId && c.CodeSyu == codeSyu && Convert.ToByte(c.CodeKbn) == unkobi.OthJinKbn2
                             select c)
                            .ToListAsync();
                    }, request._tenantId, "OTHJINKBN"))
                    .SingleOrDefault()?.RyakuNm ?? string.Empty;
                    incidental.OthJin2 = unkobi.OthJin2;
                    incidental.IsPreviousDay = unkobi.HaiSymd != unkobi.SyukoYmd;
                    incidental.IsAfterDay = unkobi.TouYmd != unkobi.KikYmd;
                    incidental.DefaultLoadedItemTaxType = _context.TkmKasSet.SingleOrDefault(k => k.CompanyCdSeq == new ClaimModel().CompanyID).TumZeiKbn;
                    incidental.FuttumKbnMode = request._viewMode;

                    foreach (var haisha in haishaList)
                    {
                        if (haisha.YouTblSeq == 0)
                        {
                            var vehicleInformation = new VehicleInformation();

                            vehicleInformation.No = haishaList.IndexOf(haisha) + 1;
                            if (DateTime.TryParseExact(haisha.HaiSymd, "yyyyMMdd", null, DateTimeStyles.None, out parseDt))
                            {
                                vehicleInformation.HaiSYmd = parseDt;
                            }
                            var syaryo = vpmSyaRyoList.Where(s => s.SyaRyoCdSeq == haisha.HaiSsryCdSeq).FirstOrDefault();
                            if (syaryo != null)
                            {
                                vehicleInformation.SyaRyoCd = syaryo.SyaRyoCd;
                                vehicleInformation.SyaRyoNm = syaryo.SyaRyoNm;
                            }
                            vehicleInformation.WorkingBranch
                                = workingBranchList
                                    .SingleOrDefault(w => w.TeiDanNo == haisha.TeiDanNo 
                                                            && w.BunkRen == haisha.BunkRen
                                                            && string.Compare(w.StaYmd, haisha.SyuKoYmd) <= 0
                                                            && string.Compare(haisha.SyuKoYmd, w.EndYmd) <= 0)
                                    ?.RyakuNm ?? string.Empty;

                            incidental.VehicleInformationList.Add(vehicleInformation);
                        }

                        var settingQuantity = new SettingQuantity();
                        if (DateTime.TryParseExact(haisha.KikYmd, "yyyyMMdd", null, DateTimeStyles.None, out parseDt))
                        {
                            settingQuantity.GarageReturnDate = parseDt;
                        }
                        if (DateTime.TryParseExact(haisha.SyuKoYmd, "yyyyMMdd", null, DateTimeStyles.None, out parseDt))
                        {
                            settingQuantity.GarageLeaveDate = parseDt;
                        }
                        settingQuantity.UnkRen = haisha.UnkRen;
                        settingQuantity.BunkRen = haisha.BunkRen;
                        settingQuantity.TeiDanNo = haisha.TeiDanNo;
                        settingQuantity.GoSyaJyn = haisha.GoSyaJyn;
                        settingQuantity.BunKSyuJyn = haisha.BunKsyuJyn;
                        settingQuantity.GoSya = haisha.GoSya;
                        var syaRyoItem = syaRyoNmList.FirstOrDefault(s => s.HaiSsryCdSeq == haisha.HaiSsryCdSeq && s.BunkRen == haisha.BunkRen);
                        if (syaRyoItem?.YouTblSeq == 0)
                        {
                            settingQuantity.SyaRyoNm = syaRyoItem.SyaRyoNm;
                        }
                        else
                        {
                            settingQuantity.SyaRyoNm = string.Empty;
                        }
                        settingQuantity.YouSha = youSha;
                        incidental.SettingQuantityList.Add(settingQuantity);
                    }

                    return incidental;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
