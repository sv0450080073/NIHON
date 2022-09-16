using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CancelListReport.Queries
{
    public class SearchCancelBookingQuery : IRequest<List<CancelListSearchData>>
    {
        private readonly CancelListData _searchOption;
        private readonly ITPM_CodeSyService _codeSyService;
        private readonly int _tenantId;

        public SearchCancelBookingQuery(CancelListData searchOption, ITPM_CodeSyService codeSyService, int tenantId)
        {
            _searchOption = searchOption;
            _codeSyService = codeSyService;
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<SearchCancelBookingQuery, List<CancelListSearchData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<SearchCancelBookingQuery> _logger;

            public Handler(KobodbContext context, ILogger<SearchCancelBookingQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<CancelListSearchData>> Handle(SearchCancelBookingQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var condition = request._searchOption;
                    string YoyakuStr = "";
                    string YoyakuEnd = "";
                    string customerStr = "";
                    string customerEnd = "";
                    string SupplierStr = "";
                    string SupplierEnd = "";
                    if (condition.YoyakuFrom == null || condition.YoyakuFrom.YoyaKbnSeq == 0)
                    {
                        YoyakuStr = "";
                    }
                    if (condition.YoyakuTo == null || condition.YoyakuTo.YoyaKbnSeq == 0)
                    {
                        YoyakuEnd = "";
                    }
                    if (condition.GyosyaTokuiSakiFrom == null || condition.GyosyaTokuiSakiFrom.GyosyaCdSeq == 0)
                    {
                        customerStr = "";
                    }
                    else
                    {
                        customerStr = condition.GyosyaTokuiSakiFrom.GyosyaCd.ToString("D3") + (condition.TokiskTokuiSakiFrom == null ? "0000" : condition.TokiskTokuiSakiFrom.TokuiCd.ToString("D4")) + (condition.TokiStTokuiSakiFrom == null ? "0000" : condition.TokiStTokuiSakiFrom.SitenCd.ToString("D4"));
                    }
                    if (condition.GyosyaTokuiSakiTo == null || condition.GyosyaTokuiSakiTo.GyosyaCdSeq == 0)
                    {
                        customerEnd = "";
                    }
                    else
                    {
                        customerEnd = condition.GyosyaTokuiSakiTo.GyosyaCd.ToString("D3") + ((condition.TokiskTokuiSakiTo == null || condition.TokiskTokuiSakiTo.TokuiSeq == 0) ? "9999" : condition.TokiskTokuiSakiTo.TokuiCd.ToString("D4")) + ((condition.TokiStTokuiSakiTo == null || condition.TokiStTokuiSakiTo.SitenCdSeq == 0) ? "9999" : condition.TokiStTokuiSakiTo.SitenCd.ToString("D4"));
                    }
                    if (condition.GyosyaShiireSakiFrom == null || condition.GyosyaShiireSakiFrom.GyosyaCdSeq == 0)
                    {
                        SupplierStr = "";
                    }
                    else
                    {
                        SupplierStr = condition.GyosyaShiireSakiFrom.GyosyaCd.ToString("D3") + (condition.TokiskShiireSakiFrom == null ? "0000" : condition.TokiskShiireSakiFrom.TokuiCd.ToString("D4")) + (condition.TokiStShiireSakiFrom == null ? "0000" : condition.TokiStShiireSakiFrom.SitenCd.ToString("D4"));
                    }
                    if (condition.GyosyaShiireSakiTo == null || condition.GyosyaShiireSakiTo.GyosyaCdSeq == 0)
                    {
                        SupplierEnd = "";
                    }
                    else
                    {
                        SupplierEnd = condition.GyosyaShiireSakiTo.GyosyaCd.ToString("D3") + ((condition.TokiskShiireSakiTo == null || condition.TokiskShiireSakiTo.TokuiSeq == 0) ? "9999" : condition.TokiskShiireSakiTo.TokuiCd.ToString("D4")) + ((condition.TokiStShiireSakiTo == null  || condition.TokiStShiireSakiTo.SitenCdSeq == 0) ? "9999" : condition.TokiStShiireSakiTo.SitenCd.ToString("D4"));
                    }
                    int startDate = int.Parse(condition.StartDate.ToString(CommonConstants.FormatYMD));
                    int endDate = int.Parse(condition.EndDate.ToString(CommonConstants.FormatYMD));

                    var result = await request._codeSyService.FilterTenantIdByListCodeSyu(async (tenantIds, codeSyus) =>
                    {
                        return await
                               (
                                from unkobi in _context.TkdUnkobi
                                join yyksho in _context.TkdYyksho on unkobi.UkeNo equals yyksho.UkeNo into unysho
                                from subUnsho in unysho.DefaultIfEmpty()
                                join yyksyu in _context.TkdYykSyu on new { subUnsho.UkeNo, unkobi.UnkRen } equals new { yyksyu.UkeNo, yyksyu.UnkRen } into unshosyu
                                from subUnshosyu in unshosyu.DefaultIfEmpty()
                                join tokisk in _context.VpmTokisk on subUnsho.TokuiSeq equals tokisk.TokuiSeq
                                join tokiSt in _context.VpmTokiSt on new { subUnsho.SitenCdSeq, subUnsho.TokuiSeq } equals new { tokiSt.SitenCdSeq, tokiSt.TokuiSeq }
                                join sirTokisk in _context.VpmTokisk on subUnsho.SirCdSeq equals sirTokisk.TokuiSeq
                                join sirTokiSt in _context.VpmTokiSt on new { T1 = subUnsho.SirCdSeq, T2 = subUnsho.SirSitenCdSeq } equals new { T1 = sirTokiSt.TokuiSeq, T2 = sirTokiSt.SitenCdSeq }
                                join gyosa in _context.VpmGyosya on new { tokisk.GyosyaCdSeq, tokisk.TenantCdSeq } equals new { gyosa.GyosyaCdSeq, gyosa.TenantCdSeq } into gy
                                from subGyosa in gy.DefaultIfEmpty()
                                join gyosatoki in _context.VpmGyosya on new { sirTokisk.GyosyaCdSeq, sirTokisk.TenantCdSeq } equals new { gyosatoki.GyosyaCdSeq, gyosatoki.TenantCdSeq } into gy1
                                from subGyosa1 in gy1.DefaultIfEmpty()
                                join eigyos in _context.VpmEigyos on subUnsho.UkeEigCdSeq equals eigyos.EigyoCdSeq into ge
                                from subEigyos in ge.DefaultIfEmpty()
                                join company in _context.VpmCompny on new { subEigyos.CompanyCdSeq, TenantCdSeq = request._tenantId } equals new { company.CompanyCdSeq, company.TenantCdSeq } into cpn
                                from subCpn in cpn.DefaultIfEmpty()
                                join shain in _context.VpmSyain on subUnsho.CanTanSeq equals shain.SyainCdSeq into sc
                                from subShain in sc.DefaultIfEmpty()
                                join sain in _context.VpmSyain on subUnsho.EigTanCdSeq equals sain.SyainCdSeq into ss
                                from subSain in ss.DefaultIfEmpty()
                                join kyoshe in _context.VpmKyoShe on subUnsho.CanTanSeq equals kyoshe.SyainCdSeq into sk
                                from subKyoshe in sk.DefaultIfEmpty()
                                join egos in _context.VpmEigyos on subKyoshe.EigyoCdSeq equals egos.EigyoCdSeq into kyego
                                from subKyego in kyego.DefaultIfEmpty()
                                join company1 in _context.VpmCompny on new { subKyego.CompanyCdSeq, TenantCdSeq = request._tenantId } equals new { company1.CompanyCdSeq, company1.TenantCdSeq } into cpn1
                                from subCpn1 in cpn1.DefaultIfEmpty()
                                join syain in _context.VpmSyain on subUnsho.InTanCdSeq equals syain.SyainCdSeq into sksyain
                                from subSksyain in sksyain.DefaultIfEmpty()
                                join syasyu in _context.VpmSyaSyu on new { subUnshosyu.SyaSyuCdSeq, t = request._tenantId } equals new { syasyu.SyaSyuCdSeq, t = syasyu.TenantCdSeq } into ssy
                                from subSyasyu in ssy.DefaultIfEmpty()
                                join yoykbn in _context.VpmYoyKbn on new { subUnsho.YoyaKbnSeq, subUnsho.TenantCdSeq } equals new { yoykbn.YoyaKbnSeq, yoykbn.TenantCdSeq } into syo
                                from subYoyKbn in syo.DefaultIfEmpty()
                                join codeYoya in _context.VpmCodeKb on new { CodeKbn = (int)subUnsho.YoyaSyu, CodeSyu = codeSyus[0], TenantCdSeq = 1 } equals new { CodeKbn = Convert.ToInt32(codeYoya.CodeKbn), codeYoya.CodeSyu, codeYoya.TenantCdSeq } into yoyaCode
                                from subYoyaCode in yoyaCode.DefaultIfEmpty()
                                join codeKata in _context.VpmCodeKb on new { CodeKbn = (int)subUnshosyu.KataKbn, CodeSyu = codeSyus[1], TenantCdSeq = 1 } equals new { CodeKbn = Convert.ToInt32(codeKata.CodeKbn), codeKata.CodeSyu, codeKata.TenantCdSeq } into kataCode
                                from subKataCode in kataCode.DefaultIfEmpty()
                                where
                                      (subUnsho != null) &&
                                      (subUnshosyu != null) &&
                                      (condition.DateType == DateType.Cancellation ? (Convert.ToInt32(subUnsho.CanYmd).CompareTo(startDate) >= 0 && Convert.ToInt32(subUnsho.CanYmd).CompareTo(endDate) <= 0) : true) &&
                                      (condition.DateType == DateType.VehicleDelivery ? (Convert.ToInt32(unkobi.HaiSymd).CompareTo(startDate) >= 0 && Convert.ToInt32(unkobi.HaiSymd).CompareTo(endDate) <= 0) : true) &&
                                      (condition.DateType == DateType.Arrival ? (Convert.ToInt32(unkobi.TouYmd).CompareTo(startDate) >= 0 && Convert.ToInt32(unkobi.TouYmd).CompareTo(endDate) <= 0) : true) &&
                                      ((condition._ukeCdFrom == -1 || condition._ukeCdTo == -1) ? true : (subUnsho.UkeCd >= condition._ukeCdFrom && subUnsho.UkeCd <= condition._ukeCdTo)) &&
                                      (YoyakuStr == "" || (subUnsho.YoyaKbnSeq >= condition.YoyakuFrom.YoyaKbnSeq)) &&
                                      (YoyakuEnd == "" || (subUnsho.YoyaKbnSeq <= condition.YoyakuTo.YoyaKbnSeq)) &&
                                      (condition.CancelBookingType.CodeKbnName.Equals(Constants.SelectedAll) ? (subUnsho.YoyaSyu == 2 || subUnsho.YoyaSyu == 4) : subUnsho.YoyaSyu == Convert.ToInt32(condition.CancelBookingType.CodeKb_CodeKbn)) &&
                                      (condition.Company.CompanyInfo.Equals(Constants.SelectedAll) ? true : subCpn.CompanyCdSeq == condition.Company.CompanyCdSeq) &&
                                      (condition.BranchStart.BranchText.Equals(Constants.SelectedAll) ? true : subEigyos.EigyoCd >= condition.BranchStart.EigyoCd) &&
                                      (condition.BranchEnd.BranchText.Equals(Constants.SelectedAll) ? true : subEigyos.EigyoCd <= condition.BranchEnd.EigyoCd) &&
                                      (condition.StaffStart.StaffText.Equals(Constants.SelectedAll) ? true : Convert.ToInt64(subSain.SyainCd) >= Convert.ToInt64(condition.StaffStart.SyainCd)) &&
                                      (condition.StaffEnd.StaffText.Equals(Constants.SelectedAll) ? true : Convert.ToInt64(subSain.SyainCd) <= Convert.ToInt64(condition.StaffEnd.SyainCd)) &&
                                      (condition.CancelStaffStart.StaffText.Equals(Constants.SelectedAll) ? true : Convert.ToInt64(subShain.SyainCd) >= Convert.ToInt64(condition.CancelStaffStart.SyainCd)) &&
                                      (condition.CancelStaffEnd.StaffText.Equals(Constants.SelectedAll) ? true : Convert.ToInt64(subShain.SyainCd) <= Convert.ToInt64(condition.CancelStaffEnd.SyainCd)) &&
                                      (condition.CancelCharge.Option == ConfirmAction.All ? true : (condition.CancelCharge.Option == ConfirmAction.Yes ? subUnsho.CanUnc != 0 : subUnsho.CanUnc == 0)) &&
                                      (subUnsho.TenantCdSeq == request._tenantId) &&
                                      (tokisk.TenantCdSeq == request._tenantId) &&
                                      (sirTokisk.TenantCdSeq == request._tenantId) &&
                                      (string.Compare(tokisk.SiyoStaYmd, unkobi.HaiSymd) <= 0 && string.Compare(tokisk.SiyoEndYmd, unkobi.HaiSymd) >= 0) &&
                                      (string.Compare(tokiSt.SiyoStaYmd, unkobi.HaiSymd) <= 0 && string.Compare(tokiSt.SiyoEndYmd, unkobi.HaiSymd) >= 0) &&
                                      (string.Compare(sirTokisk.SiyoStaYmd, unkobi.HaiSymd) <= 0 && string.Compare(sirTokisk.SiyoEndYmd, unkobi.HaiSymd) >= 0) &&
                                      (string.Compare(sirTokiSt.SiyoStaYmd, unkobi.HaiSymd) <= 0 && string.Compare(sirTokiSt.SiyoEndYmd, unkobi.HaiSymd) >= 0) &&
                                      (string.Compare(subKyoshe.StaYmd, unkobi.HaiSymd) <= 0 && string.Compare(subKyoshe.EndYmd, unkobi.HaiSymd) >= 0)
                                select new CancelListSearchData
                                {
                                    Tokui = $"{subGyosa.GyosyaCd:000}{tokisk.TokuiCd:0000}{tokiSt.SitenCd:0000}",
                                    Shiire = $"{subGyosa1.GyosyaCd:000}{sirTokisk.TokuiCd:0000}{sirTokiSt.SitenCd:0000}",
                                    TokuiCd = tokisk.TokuiCd,
                                    CanCanYmd = subUnsho.CanYmd,
                                    TokuiSaki = tokisk.RyakuNm + tokiSt.RyakuNm,
                                    TokuiTanNm = subUnsho.TokuiTanNm,
                                    ShiireSaki = sirTokisk.RyakuNm + sirTokiSt.RyakuNm,
                                    CancelYmd = subUnsho.CanYmd,
                                    Eigos = subKyego.EigyoNm,
                                    InChargeStaff = subShain.SyainNm,
                                    BookingName = subUnsho.YoyaNm,
                                    CancelReason = subUnsho.CanRiy,
                                    FixedDate = subUnsho.KaktYmd,
                                    Status = string.IsNullOrWhiteSpace(subUnsho.KaktYmd) ? (unkobi.HaiSkbn == 1 ? "確定前" : (unkobi.HaiSkbn == 2 ? "配車済" : "一部配車済")) : "確定済",
                                    Cancel = "キャンセル",
                                    BookingAmount = subUnsho.UntKin.ToString(),
                                    TaxRate = subUnsho.Zeiritsu.ToString(),
                                    TaxAmount = subUnsho.ZeiRui.ToString(),
                                    ChargeRate = subUnsho.TesuRitu.ToString(),
                                    ChargeAmount = subUnsho.TesuRyoG.ToString(),
                                    CancelRate = subUnsho.CanRit.ToString(),
                                    CancelFee = subUnsho.CanUnc.ToString(),
                                    CancelTaxRate = subUnsho.CanSyoR.ToString(),
                                    CancelTaxFee = subUnsho.CanSyoG.ToString(),
                                    HaiSNm = unkobi.HaiSnm,
                                    TouNm = unkobi.TouNm,
                                    HaiSYmd = unkobi.HaiSymd,
                                    HaiSTime = unkobi.HaiStime,
                                    TouYmd = unkobi.TouYmd,
                                    TouChTime = unkobi.TouChTime,
                                    Driver = unkobi.DrvJin.ToString(),
                                    Guide = unkobi.GuiSu.ToString(),
                                    DanTaNm = unkobi.DanTaNm,
                                    KanJNm = unkobi.KanJnm,
                                    IkNm = unkobi.IkNm,
                                    BusTypeName = (subSyasyu.SyaSyuNm ?? "指定なし"),
                                    BusQuantity = subUnshosyu.SyaSyuDai.ToString(),
                                    Passenger = unkobi.JyoSyaJin.ToString(),
                                    PlusJin = unkobi.PlusJin.ToString(),
                                    InvoiceIssueDate = (from s in _context.TkdSeiMei
                                                        join p in _context.TkdSeiPrS on new { SiyoKbn = 1, s.SeiOutSeq } equals new { SiyoKbn = (int)p.SiyoKbn, p.SeiOutSeq } into ps
                                                        from subPs in ps.DefaultIfEmpty()
                                                        where s.SiyoKbn == 1 && s.UkeNo == subUnsho.UkeNo
                                                        select subPs.SeiHatYmd).FirstOrDefault(),
                                    ReceivedBranch = subEigyos.RyakuNm,
                                    InChargeStaff2 = subSain.SyainNm,
                                    InputBy = subSksyain.SyainNm,
                                    BookingType = subYoyKbn.YoyaKbnNm,
                                    BookingStatus = subYoyaCode.CodeKbnNm,

                                    UkeYmd = subUnsho.UkeYmd,
                                    UkeNo = subUnsho.UkeNo,
                                    UkeCd = subUnsho.UkeCd,
                                    UnkRen = subUnshosyu.UnkRen,
                                })
                                .OrderBy(_=>_.UkeNo)
                                .ThenBy(_=>_.UnkRen)
                                .ToListAsync();
                    }, request._tenantId, new List<string> { "YOYASYU", "KATAKBN" });
                    result = result.Where(x => (customerStr == "" || Convert.ToInt64(x.Tokui) >= Convert.ToInt64(customerStr))
                                           && (customerEnd == "" || Convert.ToInt64(x.Tokui) <= Convert.ToInt64(customerEnd))
                                           && (SupplierStr == "" || Convert.ToInt64(x.Shiire) >= Convert.ToInt64(SupplierStr))
                                           && (SupplierEnd == "" || Convert.ToInt64(x.Shiire) <= Convert.ToInt64(SupplierEnd))).ToList();
                    var uniqueBooking =
                        (from ac in result
                         group ac by new { ac.UkeNo, ac.UnkRen } into av
                         select new
                         {
                             av.Key.UkeNo,
                             av.Key.UnkRen,
                             ListData = av.ToList()
                         })
                         .OrderBy(_ => _.UkeNo)
                         .ThenBy(_ => _.UnkRen)
                         .AsEnumerable()
                         .ToList();

                    // This step collapsible multiple booking into one record.
                    var uniqueBkDetail =
                        (from ub in uniqueBooking
                         join ax in result.Distinct() on new { ub.UkeNo, ub.UnkRen } equals new { ax.UkeNo, ax.UnkRen }
                         let temp = ax.BusViewDatas = ub.ListData.Where(_ => _.BusQuantity != null).Select(_ => new BusViewData
                         {
                             BusType = _.BusTypeName,
                             Daisu = int.Parse(_.BusQuantity)
                         }).ToList()
                         select ax
                        ).OrderBy(_ => _.TokuiCd)
                        .ThenBy(_=>_.UkeNo)
                        .AsEnumerable()
                        .ToList();

                    return uniqueBkDetail;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return new List<CancelListSearchData>();
                }
            }
        }
    }
}
