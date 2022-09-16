using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportationSummary.Queries
{
    public class GetJitHous : IRequest<List<JitHouReports>>
    {
        public SearchParam SearchParam { get; set; }
        public class Handler : IRequestHandler<GetJitHous, List<JitHouReports>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<JitHouReports>> Handle(GetJitHous request, CancellationToken cancellationToken)
            {
                try
                {
                    List<JitHouItem> rows = null;
                    List<JitHouReports> jitHouReports = new List<JitHouReports>();
                    List<JitHouReportItem> jitHouReportItems = new List<JitHouReportItem>();
                    var jitHouReport = new JitHouReports();

                    await _context.LoadStoredProc("dbo.PK_SpJitHou_R")
                       .AddParam("@StrDate", request?.SearchParam?.StrDate ?? "")
                       .AddParam("@CompnyCd", request?.SearchParam?.CompnyCd.ToString() ?? "")
                       .AddParam("@StrEigyoCd", request?.SearchParam?.StrEigyoCd.ToString() == "0" ? null : request?.SearchParam?.StrEigyoCd.ToString())
                       .AddParam("@EndEigyoCd", request?.SearchParam?.EndEigyoCd.ToString() == "0" ? null : request?.SearchParam?.EndEigyoCd.ToString())
                       .AddParam("@StrUnsouKbn", request?.SearchParam?.StrUnsouKbn.ToString() == "0" ? null : request?.SearchParam?.StrUnsouKbn.ToString())
                       .AddParam("@EndUnsouKbn", request?.SearchParam?.EndUnsouKbn.ToString() == "0" ? null : request?.SearchParam?.EndUnsouKbn.ToString())
                       .AddParam("@TenantCdSeq", request?.SearchParam?.TenantCdSeq.ToString() ?? "")
                       .AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                   .ExecAsync(async r => rows = await r.ToListAsync<JitHouItem>());
                    //group by UnsouKbnNm to divide page
                    var jitHouResult = rows?.GroupBy(x => x.UnsouKbnNm).ToList();

                    //check list any
                    if (jitHouResult.Any())
                    {
                        foreach (var groupJitHouResult in jitHouResult)
                        {
                            decimal tmp_Large_NobeJyoCnt = 0;
                            decimal tmp_Large_NobeRinCnt = 0;
                            decimal tmp_Large_NobeSumCnt = 0;
                            decimal tmp_Large_NobeJitCnt = 0;
                            decimal tmp_Large_SoukoKm_Jisya = 0;
                            decimal tmp_Large_SoukoKm_Kaiso = 0;
                            decimal tmp_Large_SoukoKm_Sum = 0;
                            decimal tmp_Large_YusouJin = 0;
                            decimal tmp_Large_UnkoCnt = 0;
                            decimal tmp_Large_UnkoOthAllCnt = 0;
                            decimal tmp_Large_UnsoSyu = 0;
                            decimal tmp_Large_DaySoukoKm = 0;
                            decimal tmp_Large_DayUnsoCnt = 0;
                            decimal tmp_Large_DayUnsoSyu = 0;
                            decimal tmp_Large_UnkoJisyaKm = 0;
                            decimal tmp_Medium_NobeJyoCnt = 0;
                            decimal tmp_Medium_NobeRinCnt = 0;
                            decimal tmp_Medium_NobeSumCnt = 0;
                            decimal tmp_Medium_NobeJitCnt = 0;
                            decimal tmp_Medium_JitudoRitu = 0;
                            decimal tmp_Medium_RinjiZouRitu = 0;
                            decimal tmp_Medium_SoukoKm_Jisya = 0;
                            decimal tmp_Medium_SoukoKm_Kaiso = 0;
                            decimal tmp_Medium_SoukoKm_Sum = 0;
                            decimal tmp_Medium_YusouJin = 0;
                            decimal tmp_Medium_UnkoCnt = 0;
                            decimal tmp_Medium_UnkoOthAllCnt = 0;
                            decimal tmp_Medium_UnsoSyu = 0;
                            decimal tmp_Medium_DaySoukoKm = 0;
                            decimal tmp_Medium_DayUnsoCnt = 0;
                            decimal tmp_Medium_DayUnsoSyu = 0;
                            decimal tmp_Medium_UnkoJisyaKm = 0;
                            decimal tmp_Small_NobeJyoCnt = 0;
                            decimal tmp_Small_NobeRinCnt = 0;
                            decimal tmp_Small_NobeSumCnt = 0;
                            decimal tmp_Small_NobeJitCnt = 0;
                            decimal tmp_Small_JitudoRitu = 0;
                            decimal tmp_Small_RinjiZouRitu = 0;
                            decimal tmp_Small_SoukoKm_Jisya = 0;
                            decimal tmp_Small_SoukoKm_Kaiso = 0;
                            decimal tmp_Small_SoukoKm_Sum = 0;
                            decimal tmp_Small_YusouJin = 0;
                            decimal tmp_Small_UnkoCnt = 0;
                            decimal tmp_Small_UnkoOthAllCnt = 0;
                            decimal tmp_Small_UnsoSyu = 0;
                            decimal tmp_Small_DaySoukoKm = 0;
                            decimal tmp_Small_DayUnsoCnt = 0;
                            decimal tmp_Small_DayUnsoSyu = 0;
                            decimal tmp_Small_UnkoJisyaKm = 0;

                            //3 record make by 1 group 大型,中型,小型
                            var groupReport = groupJitHouResult
                                                   .Select((x, i) => new { Index = i, Value = x })
                                                   .GroupBy(x => x.Index / 3)
                                                   .Select(x => x.Select(v => v.Value).ToList())
                                                   .ToList();

                            //with each group caculate
                            jitHouReportItems = new List<JitHouReportItem>();
                            foreach (var group in groupReport)
                            {
                                jitHouReportItems.Add(new JitHouReportItem
                                {
                                    Shipping = group.FirstOrDefault().UnsouKbnNm,
                                    ProcessingDate = group.FirstOrDefault().SyoriYm,
                                    CompanyName = request?.SearchParam?.CompanyName.ToString(),
                                    EigyoName = group.FirstOrDefault().EigyoNm,
                                    Large_NobeJyoCnt = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJyoCnt.ToString("#,##0"),
                                    Large_NobeRinCnt = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeRinCnt.ToString("#,##0"),
                                    Large_NobeSumCnt = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeSumCnt.ToString("#,##0"),
                                    Large_NobeJitCnt = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt.ToString("#,##0"),
                                    Large_JitudoRitu = (group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeSumCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "大型") != null) ? ((decimal)group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt / (decimal)group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeSumCnt * 100).ToString("#,##0.0") : "0.0",
                                    Large_RinjiZouRitu = (group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "大型") != null) ? ((decimal)group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeRinCnt / (decimal)group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt * 100).ToString("#,##0.0") : "0.0",
                                    Large_SoukoKm_Jisya = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.JitJisaKm.ToString(FormatString.FormatDecimalTwoPlace),
                                    Large_SoukoKm_Kaiso = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.JitKisoKm.ToString(FormatString.FormatDecimalTwoPlace),
                                    Large_SoukoKm_Sum = Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.JitSumKm).ToString(FormatString.FormatDecimalTwoPlace),
                                    Large_YusouJin = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.YusoJin.ToString("#,##0"),
                                    Large_UnkoCnt = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.UnkoCnt.ToString("#,##0"),
                                    Large_UnkoOthAllCnt = group.FirstOrDefault(x => x.KataKbnNm == "大型")?.UnkoOthAllCnt.ToString("#,##0"),
                                    Large_UnsoSyu = Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.UnsoSyu).ToString("#,##0"),
                                    Large_DaySoukoKm = (group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "大型") != null) ? (Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.JitSumKm) / (decimal)group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt).ToString("#,##0.0") : "0.0",
                                    Large_DayUnsoCnt = (group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "大型") != null) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.YusoJin) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt)).ToString("#,##0.0") : "0.0",
                                    Large_DayUnsoSyu = (group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "大型") != null) ? (Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.UnsoSyu) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.NobeJitCnt)).ToString("#,##0") : "0",
                                    Large_UnkoJisyaKm = (group.FirstOrDefault(x => x.KataKbnNm == "大型")?.UnkoCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "大型") != null) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.JitJisaKm) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "大型")?.UnkoCnt)).ToString("#,##0.0") : "0.0",
                                    Medium_NobeJyoCnt = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJyoCnt.ToString("#,##0"),
                                    Medium_NobeRinCnt = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeRinCnt.ToString("#,##0"),
                                    Medium_NobeSumCnt = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeSumCnt.ToString("#,##0"),
                                    Medium_NobeJitCnt = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt.ToString("#,##0"),
                                    Medium_JitudoRitu = (group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeSumCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "中型") != null) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeSumCnt) * 100).ToString("#,##0.0") : "0.0",
                                    Medium_RinjiZouRitu = (group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "中型") != null) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeRinCnt) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt) * 100).ToString("#,##0.0") : "0.0",
                                    Medium_SoukoKm_Jisya = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.JitJisaKm.ToString(FormatString.FormatDecimalTwoPlace),
                                    Medium_SoukoKm_Kaiso = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.JitKisoKm.ToString(FormatString.FormatDecimalTwoPlace),
                                    Medium_SoukoKm_Sum = Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.JitSumKm).ToString(FormatString.FormatDecimalTwoPlace),
                                    Medium_YusouJin = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.YusoJin.ToString("#,##0"),
                                    Medium_UnkoCnt = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.UnkoCnt.ToString("#,##0"),
                                    Medium_UnkoOthAllCnt = group.FirstOrDefault(x => x.KataKbnNm == "中型")?.UnkoOthAllCnt.ToString("#,##0"),
                                    Medium_UnsoSyu = Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.UnsoSyu).ToString("#,##0"),
                                    Medium_DaySoukoKm = (group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "中型") != null) ? (Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.JitSumKm) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt)).ToString("#,##0.0") : "0.0",
                                    Medium_DayUnsoCnt = (group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "中型") != null) ? ((decimal)group.FirstOrDefault(x => x.KataKbnNm == "中型")?.YusoJin / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt)).ToString("#,##0.0") : "0.0",
                                    Medium_DayUnsoSyu = (group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt > 0 && group.FirstOrDefault(x => x.KataKbnNm == "中型") != null) ? (Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.UnsoSyu) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.NobeJitCnt)).ToString("#,##0") : "0",
                                    Medium_UnkoJisyaKm = group.FirstOrDefault(x => x.KataKbnNm == "中型").UnkoCnt > 0 ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.JitJisaKm) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "中型")?.UnkoCnt)).ToString("#,##0.0") : "0.0",
                                    Small_NobeJyoCnt = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJyoCnt.ToString("#,##0"),
                                    Small_NobeRinCnt = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeRinCnt.ToString("#,##0"),
                                    Small_NobeSumCnt = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeSumCnt.ToString("#,##0"),
                                    Small_NobeJitCnt = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt.ToString("#,##0"),
                                    Small_JitudoRitu = ((group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeSumCnt > 0) && (group.FirstOrDefault(x => x.KataKbnNm == "小型") != null)) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeSumCnt) * 100).ToString("#,##0.0") : "0.0",
                                    Small_RinjiZouRitu = ((group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt > 0) && (group.FirstOrDefault(x => x.KataKbnNm == "小型") != null)) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeRinCnt) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt) * 100).ToString("#,##0.0") : "0.0",
                                    Small_SoukoKm_Jisya = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.JitJisaKm.ToString(FormatString.FormatDecimalTwoPlace),
                                    Small_SoukoKm_Kaiso = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.JitKisoKm.ToString(FormatString.FormatDecimalTwoPlace),
                                    Small_SoukoKm_Sum = Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.JitSumKm).ToString(FormatString.FormatDecimalTwoPlace),
                                    Small_YusouJin = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.YusoJin.ToString("#,##0"),
                                    Small_UnkoCnt = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.UnkoCnt.ToString("#,##0"),
                                    Small_UnkoOthAllCnt = group.FirstOrDefault(x => x.KataKbnNm == "小型")?.UnkoOthAllCnt.ToString("#,##0"),
                                    Small_UnsoSyu = Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.UnsoSyu).ToString("#,##0"),
                                    Small_DaySoukoKm = ((group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt > 0) && (group.FirstOrDefault(x => x.KataKbnNm == "小型") != null)) ? (Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.JitSumKm) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt)).ToString("#,##0.0") : "0.0",
                                    Small_DayUnsoCnt = ((group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt > 0) && (group.FirstOrDefault(x => x.KataKbnNm == "小型") != null)) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.YusoJin) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt)).ToString("#,##0.0") : "0.0",
                                    Small_DayUnsoSyu = ((group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt > 0) && (group.FirstOrDefault(x => x.KataKbnNm == "小型") != null)) ? (Convert.ToDecimal(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.UnsoSyu) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.NobeJitCnt)).ToString("#,##0") : "0",
                                    Small_UnkoJisyaKm = ((group.FirstOrDefault(x => x.KataKbnNm == "小型")?.UnkoCnt > 0) && (group.FirstOrDefault(x => x.KataKbnNm == "小型") != null)) ? ((decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.JitJisaKm) / (decimal)(group.FirstOrDefault(x => x.KataKbnNm == "小型")?.UnkoCnt)).ToString("#,##0.0") : "0.0",
                                });
                            }

                            var totalPagePerGroup = (jitHouReportItems.Count / 4) + 1;
                            var indexSum = totalPagePerGroup * 4;

                            int i = 0;
                            var jitHouReportTmp = new JitHouReports();
                            var jitHouReportPerPageTmp = new JitHouReportPerPage();
                            var jitHouReportPerPage = new JitHouReportPerPage();
                            jitHouReportTmp.jitHouReportPerPage = new List<JitHouReportPerPage>();
                            while (i < indexSum)
                            {
                                if (jitHouReportPerPage?.JitHouReportItem1 == null)
                                {
                                    if (i < jitHouReportItems.Count)
                                    {
                                        jitHouReportPerPage.JitHouReportItem1 = jitHouReportItems[i];
                                        tmp_Large_NobeJyoCnt = tmp_Large_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJyoCnt ?? "0");
                                        tmp_Large_NobeRinCnt = tmp_Large_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeRinCnt ?? "0");
                                        tmp_Large_NobeSumCnt = tmp_Large_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeSumCnt ?? "0");
                                        tmp_Large_NobeJitCnt = tmp_Large_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJitCnt ?? "0");
                                        tmp_Large_SoukoKm_Jisya = tmp_Large_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Jisya);
                                        tmp_Large_SoukoKm_Kaiso = tmp_Large_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Kaiso);
                                        tmp_Large_SoukoKm_Sum = tmp_Large_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Sum);
                                        tmp_Large_YusouJin = tmp_Large_YusouJin + decimal.Parse(jitHouReportItems[i]?.Large_YusouJin ?? "0");
                                        tmp_Large_UnkoCnt = tmp_Large_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoCnt ?? "0");
                                        tmp_Large_UnkoOthAllCnt = tmp_Large_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoOthAllCnt ?? "0");
                                        tmp_Large_UnsoSyu = tmp_Large_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Large_UnsoSyu ?? "0");
                                        tmp_Large_DaySoukoKm = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Sum) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoCnt = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_YusouJin) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoSyu = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_UnsoSyu) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_UnkoJisyaKm = (Convert.ToDecimal(tmp_Large_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Large_UnkoCnt) : 0);
                                        tmp_Medium_NobeJyoCnt = tmp_Medium_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJyoCnt ?? "0");
                                        tmp_Medium_NobeRinCnt = tmp_Medium_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeRinCnt ?? "0");
                                        tmp_Medium_NobeSumCnt = tmp_Medium_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeSumCnt ?? "0");
                                        tmp_Medium_NobeJitCnt = tmp_Medium_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJitCnt ?? "0");
                                        tmp_Medium_JitudoRitu = tmp_Medium_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Medium_JitudoRitu ?? "0");
                                        tmp_Medium_RinjiZouRitu = tmp_Medium_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Medium_RinjiZouRitu ?? "0");
                                        tmp_Medium_SoukoKm_Jisya = tmp_Medium_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Jisya);
                                        tmp_Medium_SoukoKm_Kaiso = tmp_Medium_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Kaiso);
                                        tmp_Medium_SoukoKm_Sum = tmp_Medium_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Sum);
                                        tmp_Medium_YusouJin = tmp_Medium_YusouJin + decimal.Parse(jitHouReportItems[i]?.Medium_YusouJin ?? "0");
                                        tmp_Medium_UnkoCnt = tmp_Medium_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoCnt ?? "0");
                                        tmp_Medium_UnkoOthAllCnt = tmp_Medium_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoOthAllCnt ?? "0");
                                        tmp_Medium_UnsoSyu = tmp_Medium_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Medium_UnsoSyu ?? "0");
                                        tmp_Medium_DaySoukoKm = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Sum) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoCnt = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_YusouJin) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoSyu = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_UnsoSyu) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_UnkoJisyaKm = (Convert.ToDecimal(tmp_Medium_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Medium_UnkoCnt) : 0);
                                        tmp_Small_NobeJyoCnt = tmp_Small_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJyoCnt ?? "0");
                                        tmp_Small_NobeRinCnt = tmp_Small_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeRinCnt ?? "0");
                                        tmp_Small_NobeSumCnt = tmp_Small_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeSumCnt ?? "0");
                                        tmp_Small_NobeJitCnt = tmp_Small_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJitCnt ?? "0");
                                        tmp_Small_JitudoRitu = tmp_Small_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Small_JitudoRitu ?? "0");
                                        tmp_Small_RinjiZouRitu = tmp_Small_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Small_RinjiZouRitu ?? "0");
                                        tmp_Small_SoukoKm_Jisya = tmp_Small_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Jisya);
                                        tmp_Small_SoukoKm_Kaiso = tmp_Small_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Kaiso);
                                        tmp_Small_SoukoKm_Sum = tmp_Small_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Sum);
                                        tmp_Small_YusouJin = tmp_Small_YusouJin + decimal.Parse(jitHouReportItems[i]?.Small_YusouJin ?? "0");
                                        tmp_Small_UnkoCnt = tmp_Small_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoCnt ?? "0");
                                        tmp_Small_UnkoOthAllCnt = tmp_Small_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoOthAllCnt ?? "0");
                                        tmp_Small_UnsoSyu = tmp_Small_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Small_UnsoSyu ?? "0");
                                        tmp_Small_DaySoukoKm = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Sum) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoCnt = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_YusouJin) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoSyu = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_UnsoSyu) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_UnkoJisyaKm = (Convert.ToDecimal(tmp_Small_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Small_UnkoCnt) : 0);
                                    }
                                    else if (i >= jitHouReportItems.Count && i < indexSum)
                                        jitHouReportPerPage.JitHouReportItem1 = new JitHouReportItem
                                        {
                                            RecordNull = true,
                                            Shipping = jitHouReportItems.FirstOrDefault()?.Shipping,
                                            ProcessingDate = jitHouReportItems.FirstOrDefault()?.ProcessingDate,
                                            CompanyName = request?.SearchParam?.CompanyName.ToString(),
                                        };
                                }
                                else if (jitHouReportPerPage?.JitHouReportItem2 == null)
                                {
                                    if (i < jitHouReportItems.Count)
                                    {
                                        jitHouReportPerPage.JitHouReportItem2 = jitHouReportItems[i];
                                        tmp_Large_NobeJyoCnt = tmp_Large_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJyoCnt ?? "0");
                                        tmp_Large_NobeRinCnt = tmp_Large_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeRinCnt ?? "0");
                                        tmp_Large_NobeSumCnt = tmp_Large_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeSumCnt ?? "0");
                                        tmp_Large_NobeJitCnt = tmp_Large_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJitCnt ?? "0");
                                        tmp_Large_SoukoKm_Jisya = tmp_Large_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Jisya);
                                        tmp_Large_SoukoKm_Kaiso = tmp_Large_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Kaiso);
                                        tmp_Large_SoukoKm_Sum = tmp_Large_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Sum);
                                        tmp_Large_YusouJin = tmp_Large_YusouJin + decimal.Parse(jitHouReportItems[i]?.Large_YusouJin ?? "0");
                                        tmp_Large_UnkoCnt = tmp_Large_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoCnt ?? "0");
                                        tmp_Large_UnkoOthAllCnt = tmp_Large_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoOthAllCnt ?? "0");
                                        tmp_Large_UnsoSyu = tmp_Large_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Large_UnsoSyu ?? "0");
                                        tmp_Large_DaySoukoKm = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Sum) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoCnt = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_YusouJin) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoSyu = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_UnsoSyu) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_UnkoJisyaKm = (Convert.ToDecimal(tmp_Large_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Large_UnkoCnt) : 0);
                                        tmp_Medium_NobeJyoCnt = tmp_Medium_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJyoCnt ?? "0");
                                        tmp_Medium_NobeRinCnt = tmp_Medium_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeRinCnt ?? "0");
                                        tmp_Medium_NobeSumCnt = tmp_Medium_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeSumCnt ?? "0");
                                        tmp_Medium_NobeJitCnt = tmp_Medium_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJitCnt ?? "0");
                                        tmp_Medium_JitudoRitu = tmp_Medium_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Medium_JitudoRitu ?? "0");
                                        tmp_Medium_RinjiZouRitu = tmp_Medium_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Medium_RinjiZouRitu ?? "0");
                                        tmp_Medium_SoukoKm_Jisya = tmp_Medium_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Jisya);
                                        tmp_Medium_SoukoKm_Kaiso = tmp_Medium_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Kaiso);
                                        tmp_Medium_SoukoKm_Sum = tmp_Medium_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Sum);
                                        tmp_Medium_YusouJin = tmp_Medium_YusouJin + decimal.Parse(jitHouReportItems[i]?.Medium_YusouJin ?? "0");
                                        tmp_Medium_UnkoCnt = tmp_Medium_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoCnt ?? "0");
                                        tmp_Medium_UnkoOthAllCnt = tmp_Medium_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoOthAllCnt ?? "0");
                                        tmp_Medium_UnsoSyu = tmp_Medium_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Medium_UnsoSyu ?? "0");
                                        tmp_Medium_DaySoukoKm = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Sum) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoCnt = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_YusouJin) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoSyu = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_UnsoSyu) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_UnkoJisyaKm = (Convert.ToDecimal(tmp_Medium_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Medium_UnkoCnt) : 0);
                                        tmp_Small_NobeJyoCnt = tmp_Small_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJyoCnt ?? "0");
                                        tmp_Small_NobeRinCnt = tmp_Small_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeRinCnt ?? "0");
                                        tmp_Small_NobeSumCnt = tmp_Small_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeSumCnt ?? "0");
                                        tmp_Small_NobeJitCnt = tmp_Small_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJitCnt ?? "0");
                                        tmp_Small_JitudoRitu = tmp_Small_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Small_JitudoRitu ?? "0");
                                        tmp_Small_RinjiZouRitu = tmp_Small_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Small_RinjiZouRitu ?? "0");
                                        tmp_Small_SoukoKm_Jisya = tmp_Small_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Jisya);
                                        tmp_Small_SoukoKm_Kaiso = tmp_Small_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Kaiso);
                                        tmp_Small_SoukoKm_Sum = tmp_Small_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Sum);
                                        tmp_Small_YusouJin = tmp_Small_YusouJin + decimal.Parse(jitHouReportItems[i]?.Small_YusouJin ?? "0");
                                        tmp_Small_UnkoCnt = tmp_Small_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoCnt ?? "0");
                                        tmp_Small_UnkoOthAllCnt = tmp_Small_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoOthAllCnt ?? "0");
                                        tmp_Small_UnsoSyu = tmp_Small_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Small_UnsoSyu ?? "0");
                                        tmp_Small_DaySoukoKm = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Sum) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoCnt = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_YusouJin) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoSyu = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_UnsoSyu) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_UnkoJisyaKm = (Convert.ToDecimal(tmp_Small_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Small_UnkoCnt) : 0);
                                    }
                                    else if (i >= jitHouReportItems.Count && i < indexSum)
                                        jitHouReportPerPage.JitHouReportItem2 = new JitHouReportItem
                                        {
                                            RecordNull = true
                                        };
                                }
                                else if (jitHouReportPerPage?.JitHouReportItem3 == null)
                                {
                                    if (i < jitHouReportItems.Count)
                                    {
                                        jitHouReportPerPage.JitHouReportItem3 = jitHouReportItems[i];
                                        tmp_Large_NobeJyoCnt = tmp_Large_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJyoCnt ?? "0");
                                        tmp_Large_NobeRinCnt = tmp_Large_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeRinCnt ?? "0");
                                        tmp_Large_NobeSumCnt = tmp_Large_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeSumCnt ?? "0");
                                        tmp_Large_NobeJitCnt = tmp_Large_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJitCnt ?? "0");
                                        tmp_Large_SoukoKm_Jisya = tmp_Large_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Jisya);
                                        tmp_Large_SoukoKm_Kaiso = tmp_Large_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Kaiso);
                                        tmp_Large_SoukoKm_Sum = tmp_Large_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Sum);
                                        tmp_Large_YusouJin = tmp_Large_YusouJin + decimal.Parse(jitHouReportItems[i]?.Large_YusouJin ?? "0");
                                        tmp_Large_UnkoCnt = tmp_Large_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoCnt ?? "0");
                                        tmp_Large_UnkoOthAllCnt = tmp_Large_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoOthAllCnt ?? "0");
                                        tmp_Large_UnsoSyu = tmp_Large_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Large_UnsoSyu ?? "0");
                                        tmp_Large_DaySoukoKm = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Sum) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoCnt = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_YusouJin) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoSyu = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_UnsoSyu) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_UnkoJisyaKm = (Convert.ToDecimal(tmp_Large_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Large_UnkoCnt) : 0);
                                        tmp_Medium_NobeJyoCnt = tmp_Medium_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJyoCnt ?? "0");
                                        tmp_Medium_NobeRinCnt = tmp_Medium_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeRinCnt ?? "0");
                                        tmp_Medium_NobeSumCnt = tmp_Medium_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeSumCnt ?? "0");
                                        tmp_Medium_NobeJitCnt = tmp_Medium_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJitCnt ?? "0");
                                        tmp_Medium_JitudoRitu = tmp_Medium_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Medium_JitudoRitu ?? "0");
                                        tmp_Medium_RinjiZouRitu = tmp_Medium_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Medium_RinjiZouRitu ?? "0");
                                        tmp_Medium_SoukoKm_Jisya = tmp_Medium_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Jisya);
                                        tmp_Medium_SoukoKm_Kaiso = tmp_Medium_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Kaiso);
                                        tmp_Medium_SoukoKm_Sum = tmp_Medium_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Sum);
                                        tmp_Medium_YusouJin = tmp_Medium_YusouJin + decimal.Parse(jitHouReportItems[i]?.Medium_YusouJin ?? "0");
                                        tmp_Medium_UnkoCnt = tmp_Medium_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoCnt ?? "0");
                                        tmp_Medium_UnkoOthAllCnt = tmp_Medium_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoOthAllCnt ?? "0");
                                        tmp_Medium_UnsoSyu = tmp_Medium_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Medium_UnsoSyu ?? "0");
                                        tmp_Medium_DaySoukoKm = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Sum) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoCnt = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_YusouJin) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoSyu = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_UnsoSyu) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_UnkoJisyaKm = (Convert.ToDecimal(tmp_Medium_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Medium_UnkoCnt) : 0);
                                        tmp_Small_NobeJyoCnt = tmp_Small_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJyoCnt ?? "0");
                                        tmp_Small_NobeRinCnt = tmp_Small_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeRinCnt ?? "0");
                                        tmp_Small_NobeSumCnt = tmp_Small_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeSumCnt ?? "0");
                                        tmp_Small_NobeJitCnt = tmp_Small_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJitCnt ?? "0");
                                        tmp_Small_JitudoRitu = tmp_Small_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Small_JitudoRitu ?? "0");
                                        tmp_Small_RinjiZouRitu = tmp_Small_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Small_RinjiZouRitu ?? "0");
                                        tmp_Small_SoukoKm_Jisya = tmp_Small_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Jisya);
                                        tmp_Small_SoukoKm_Kaiso = tmp_Small_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Kaiso);
                                        tmp_Small_SoukoKm_Sum = tmp_Small_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Sum);
                                        tmp_Small_YusouJin = tmp_Small_YusouJin + decimal.Parse(jitHouReportItems[i]?.Small_YusouJin ?? "0");
                                        tmp_Small_UnkoCnt = tmp_Small_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoCnt ?? "0");
                                        tmp_Small_UnkoOthAllCnt = tmp_Small_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoOthAllCnt ?? "0");
                                        tmp_Small_UnsoSyu = tmp_Small_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Small_UnsoSyu ?? "0");
                                        tmp_Small_DaySoukoKm = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Sum) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoCnt = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_YusouJin) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoSyu = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_UnsoSyu) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_UnkoJisyaKm = (Convert.ToDecimal(tmp_Small_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Small_UnkoCnt) : 0);
                                    }
                                    else if (i >= jitHouReportItems.Count && i < indexSum)
                                        jitHouReportPerPage.JitHouReportItem3 = new JitHouReportItem
                                        {
                                            RecordNull = true
                                        };
                                }
                                else if (jitHouReportPerPage?.JitHouReportItem4 == null)
                                {
                                    if (i < jitHouReportItems.Count)
                                    {
                                        jitHouReportPerPage.JitHouReportItem4 = jitHouReportItems[i];
                                        tmp_Large_NobeJyoCnt = tmp_Large_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJyoCnt ?? "0");
                                        tmp_Large_NobeRinCnt = tmp_Large_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeRinCnt ?? "0");
                                        tmp_Large_NobeSumCnt = tmp_Large_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeSumCnt ?? "0");
                                        tmp_Large_NobeJitCnt = tmp_Large_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Large_NobeJitCnt ?? "0");
                                        tmp_Large_SoukoKm_Jisya = tmp_Large_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Jisya);
                                        tmp_Large_SoukoKm_Kaiso = tmp_Large_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Kaiso);
                                        tmp_Large_SoukoKm_Sum = tmp_Large_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Large_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Large_SoukoKm_Sum);
                                        tmp_Large_YusouJin = tmp_Large_YusouJin + decimal.Parse(jitHouReportItems[i]?.Large_YusouJin ?? "0");
                                        tmp_Large_UnkoCnt = tmp_Large_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoCnt ?? "0");
                                        tmp_Large_UnkoOthAllCnt = tmp_Large_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Large_UnkoOthAllCnt ?? "0");
                                        tmp_Large_UnsoSyu = tmp_Large_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Large_UnsoSyu ?? "0");
                                        tmp_Large_DaySoukoKm = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Sum) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoCnt = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_YusouJin) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_DayUnsoSyu = (Convert.ToDecimal(tmp_Large_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Large_UnsoSyu) / Convert.ToDecimal(tmp_Large_NobeJitCnt) : 0);
                                        tmp_Large_UnkoJisyaKm = (Convert.ToDecimal(tmp_Large_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Large_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Large_UnkoCnt) : 0);
                                        tmp_Medium_NobeJyoCnt = tmp_Medium_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJyoCnt ?? "0");
                                        tmp_Medium_NobeRinCnt = tmp_Medium_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeRinCnt ?? "0");
                                        tmp_Medium_NobeSumCnt = tmp_Medium_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeSumCnt ?? "0");
                                        tmp_Medium_NobeJitCnt = tmp_Medium_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Medium_NobeJitCnt ?? "0");
                                        tmp_Medium_JitudoRitu = tmp_Medium_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Medium_JitudoRitu ?? "0");
                                        tmp_Medium_RinjiZouRitu = tmp_Medium_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Medium_RinjiZouRitu ?? "0");
                                        tmp_Medium_SoukoKm_Jisya = tmp_Medium_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Jisya);
                                        tmp_Medium_SoukoKm_Kaiso = tmp_Medium_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Kaiso);
                                        tmp_Medium_SoukoKm_Sum = tmp_Medium_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Medium_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Medium_SoukoKm_Sum);
                                        tmp_Medium_YusouJin = tmp_Medium_YusouJin + decimal.Parse(jitHouReportItems[i]?.Medium_YusouJin ?? "0");
                                        tmp_Medium_UnkoCnt = tmp_Medium_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoCnt ?? "0");
                                        tmp_Medium_UnkoOthAllCnt = tmp_Medium_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Medium_UnkoOthAllCnt ?? "0");
                                        tmp_Medium_UnsoSyu = tmp_Medium_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Medium_UnsoSyu ?? "0");
                                        tmp_Medium_DaySoukoKm = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Sum) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoCnt = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_YusouJin) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_DayUnsoSyu = (Convert.ToDecimal(tmp_Medium_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Medium_UnsoSyu) / Convert.ToDecimal(tmp_Medium_NobeJitCnt) : 0);
                                        tmp_Medium_UnkoJisyaKm = (Convert.ToDecimal(tmp_Medium_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Medium_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Medium_UnkoCnt) : 0);
                                        tmp_Small_NobeJyoCnt = tmp_Small_NobeJyoCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJyoCnt ?? "0");
                                        tmp_Small_NobeRinCnt = tmp_Small_NobeRinCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeRinCnt ?? "0");
                                        tmp_Small_NobeSumCnt = tmp_Small_NobeSumCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeSumCnt ?? "0");
                                        tmp_Small_NobeJitCnt = tmp_Small_NobeJitCnt + decimal.Parse(jitHouReportItems[i]?.Small_NobeJitCnt ?? "0");
                                        tmp_Small_JitudoRitu = tmp_Small_JitudoRitu + decimal.Parse(jitHouReportItems[i]?.Small_JitudoRitu ?? "0");
                                        tmp_Small_RinjiZouRitu = tmp_Small_RinjiZouRitu + decimal.Parse(jitHouReportItems[i]?.Small_RinjiZouRitu ?? "0");
                                        tmp_Small_SoukoKm_Jisya = tmp_Small_SoukoKm_Jisya + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Jisya) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Jisya);
                                        tmp_Small_SoukoKm_Kaiso = tmp_Small_SoukoKm_Kaiso + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Kaiso) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Kaiso);
                                        tmp_Small_SoukoKm_Sum = tmp_Small_SoukoKm_Sum + decimal.Parse(string.IsNullOrEmpty(jitHouReportItems[i]?.Small_SoukoKm_Sum) ? "0" : jitHouReportItems[i]?.Small_SoukoKm_Sum);
                                        tmp_Small_YusouJin = tmp_Small_YusouJin + decimal.Parse(jitHouReportItems[i]?.Small_YusouJin ?? "0");
                                        tmp_Small_UnkoCnt = tmp_Small_UnkoCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoCnt ?? "0");
                                        tmp_Small_UnkoOthAllCnt = tmp_Small_UnkoOthAllCnt + decimal.Parse(jitHouReportItems[i]?.Small_UnkoOthAllCnt ?? "0");
                                        tmp_Small_UnsoSyu = tmp_Small_UnsoSyu + decimal.Parse(jitHouReportItems[i]?.Small_UnsoSyu ?? "0");
                                        tmp_Small_DaySoukoKm = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Sum) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoCnt = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_YusouJin) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_DayUnsoSyu = (Convert.ToDecimal(tmp_Small_NobeJitCnt) > 0 ? Convert.ToDecimal(tmp_Small_UnsoSyu) / Convert.ToDecimal(tmp_Small_NobeJitCnt) : 0);
                                        tmp_Small_UnkoJisyaKm = (Convert.ToDecimal(tmp_Small_UnkoCnt) > 0 ? Convert.ToDecimal(tmp_Small_SoukoKm_Jisya) / Convert.ToDecimal(tmp_Small_UnkoCnt) : 0);
                                    }
                                    else if (i == indexSum - 1)
                                    {
                                        jitHouReportPerPage.JitHouReportItem4 = new JitHouReportItem();
                                        jitHouReportPerPage.JitHouReportItem4.EigyoName = "合　計";
                                        jitHouReportPerPage.JitHouReportItem4.Large_NobeJyoCnt = tmp_Large_NobeJyoCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_NobeRinCnt = tmp_Large_NobeRinCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_NobeSumCnt = tmp_Large_NobeSumCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_NobeJitCnt = tmp_Large_NobeJitCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_JitudoRitu = (tmp_Large_NobeSumCnt > 0 ? (tmp_Large_NobeJitCnt / tmp_Large_NobeSumCnt * 100) : 0).ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_RinjiZouRitu = (tmp_Large_NobeJitCnt > 0 ? (tmp_Large_NobeRinCnt / tmp_Large_NobeJitCnt * 100) : 0).ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_SoukoKm_Jisya = tmp_Large_SoukoKm_Jisya.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Large_SoukoKm_Kaiso = tmp_Large_SoukoKm_Kaiso.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Large_SoukoKm_Sum = tmp_Large_SoukoKm_Sum.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Large_YusouJin = tmp_Large_YusouJin.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_UnkoCnt = tmp_Large_UnkoCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_UnkoOthAllCnt = tmp_Large_UnkoOthAllCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_UnsoSyu = tmp_Large_UnsoSyu.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_DaySoukoKm = tmp_Large_DaySoukoKm.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_DayUnsoCnt = tmp_Large_DayUnsoCnt.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_DayUnsoSyu = tmp_Large_DayUnsoSyu.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Large_UnkoJisyaKm = tmp_Large_UnkoJisyaKm.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_NobeJyoCnt = tmp_Medium_NobeJyoCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_NobeRinCnt = tmp_Medium_NobeRinCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_NobeSumCnt = tmp_Medium_NobeSumCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_NobeJitCnt = tmp_Medium_NobeJitCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_JitudoRitu = (tmp_Medium_NobeSumCnt > 0 ? (tmp_Medium_NobeJitCnt / tmp_Medium_NobeSumCnt * 100) : 0).ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_RinjiZouRitu = (tmp_Medium_NobeJitCnt > 0 ? (tmp_Medium_NobeRinCnt / tmp_Medium_NobeJitCnt * 100) : 0).ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_SoukoKm_Jisya = tmp_Medium_SoukoKm_Jisya.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Medium_SoukoKm_Kaiso = tmp_Medium_SoukoKm_Kaiso.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Medium_SoukoKm_Sum = tmp_Medium_SoukoKm_Sum.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Medium_YusouJin = tmp_Medium_YusouJin.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_UnkoCnt = tmp_Medium_UnkoCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_UnkoOthAllCnt = tmp_Medium_UnkoOthAllCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_UnsoSyu = tmp_Medium_UnsoSyu.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_DaySoukoKm = tmp_Medium_DaySoukoKm.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_DayUnsoCnt = tmp_Medium_DayUnsoCnt.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_DayUnsoSyu = tmp_Medium_DayUnsoSyu.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Medium_UnkoJisyaKm = tmp_Medium_UnkoJisyaKm.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_NobeJyoCnt = tmp_Small_NobeJyoCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_NobeRinCnt = tmp_Small_NobeRinCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_NobeSumCnt = tmp_Small_NobeSumCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_NobeJitCnt = tmp_Small_NobeJitCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_JitudoRitu = (tmp_Small_NobeSumCnt > 0 ? (tmp_Small_NobeJitCnt / tmp_Small_NobeSumCnt * 100) : 0).ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_RinjiZouRitu = (tmp_Small_NobeJitCnt > 0 ? (tmp_Small_NobeRinCnt / tmp_Small_NobeJitCnt * 100) : 0).ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_SoukoKm_Jisya = tmp_Small_SoukoKm_Jisya.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Small_SoukoKm_Kaiso = tmp_Small_SoukoKm_Kaiso.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Small_SoukoKm_Sum = tmp_Small_SoukoKm_Sum.ToString(FormatString.FormatDecimalTwoPlace);
                                        jitHouReportPerPage.JitHouReportItem4.Small_YusouJin = tmp_Small_YusouJin.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_UnkoCnt = tmp_Small_UnkoCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_UnkoOthAllCnt = tmp_Small_UnkoOthAllCnt.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_UnsoSyu = tmp_Small_UnsoSyu.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_DaySoukoKm = tmp_Small_DaySoukoKm.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_DayUnsoCnt = tmp_Small_DayUnsoCnt.ToString("#,##0.0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_DayUnsoSyu = tmp_Small_DayUnsoSyu.ToString("#,##0");
                                        jitHouReportPerPage.JitHouReportItem4.Small_UnkoJisyaKm = tmp_Small_UnkoJisyaKm.ToString("#,##0.0");
                                    }
                                }

                                i++;
                                if (i % 4 == 0)
                                {
                                    jitHouReportPerPageTmp = jitHouReportPerPage;
                                    jitHouReportTmp.jitHouReportPerPage.Add(jitHouReportPerPageTmp);
                                    jitHouReports.Add(jitHouReportTmp);
                                    jitHouReportTmp = new JitHouReports();
                                    jitHouReportPerPageTmp = new JitHouReportPerPage();
                                    jitHouReportTmp.jitHouReportPerPage = new List<JitHouReportPerPage>();
                                    jitHouReportPerPage = new JitHouReportPerPage();
                                }
                            }
                        }
                    }
                    return jitHouReports;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
