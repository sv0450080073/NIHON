using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.CodeKb.Queries;
using HassyaAllrightCloud.Application.VenderRequestReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IVenderRequestService : IReportService
    {
        /// <summary>
        /// Get data for VenderRequest report
        /// </summary>
        /// <param name="reportCondition"></param>
        /// <returns></returns>
        Task<List<VenderRequestReportData>> GetVenderRequestReport(VenderRequestFormData reportCondition, int tenantId);
        /// <summary>
        /// Get report paged to display on report
        /// </summary>
        /// <param name="reportDatas"></param>
        /// <returns></returns>
        Task<List<VenderRequestReportData>> GetVenderRequestReportPaged(List<VenderRequestReportData> reportDatas);
        /// <summary>
        /// Check VenderRequest report had data to display.
        /// </summary>
        /// <param name="reportCondition"></param>
        /// <param name="tenantId"></param>
        /// <returns><c>true</c>: had data, Otherwise <c>false</c></returns>
        Task<bool> VenderRequestReportHadData(VenderRequestFormData reportCondition, int tenantId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Dictionary<string, string> GetFieldValues(VenderRequestFormData data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filterValues"></param>
        void ApplyFilter(ref VenderRequestFormData data, Dictionary<string, string> filterValues, List<ReservationClassComponentData> listReservation,
            List<CustomerComponentGyosyaData> listGyosya, List<CustomerComponentTokiskData> listTokisk, List<CustomerComponentTokiStData> listTokist);
        /// <summary>
        /// Export PDF from Supermenu Vehicle
        /// </summary>
        void ExportPDF(List<VenderRequestReportData> listData, IJSRuntime IJSRuntime);
        /// <summary>
        /// Create condition and getdata
        /// </summary>
        /// <param name="UkeCd"></param>
        /// <param name="Date"></param>
        /// <param name="UnkRen"></param>
        /// <returns></returns>
        Task<(bool, List<VenderRequestReportData>, VenderRequestFormData)> SetParamAndCheckData(string UkeCd, string Date, string UnkRen);
    }

    public class VenderRequestService : IVenderRequestService
    {
        private readonly IMediator _mediatR;
        private readonly ITPM_CodeSyService _codeSyService;
        private IReportLayoutSettingService _reportLayoutSettingService;

        public VenderRequestService(IMediator mediatR,
            ITPM_CodeSyService codeSyService, IReportLayoutSettingService reportLayoutSettingService)
        {
            _mediatR = mediatR;
            _codeSyService = codeSyService;
            _reportLayoutSettingService = reportLayoutSettingService;
        }

        public async Task<List<VenderRequestReportData>> GetVenderRequestReport(VenderRequestFormData reportCondition, int tenantId)
        {
            if (reportCondition == null)
                throw new ArgumentNullException(nameof(reportCondition));

            List<VenderRequestReportData> results = new List<VenderRequestReportData>();

            var mainsRp = await _mediatR.Send(new GetVenderRequestDataMainReportQuery(tenantId, reportCondition));

            if (mainsRp.Any())
            {
                var ukesRp = mainsRp.Select(_ => Tuple.Create(_.UkeNo, _.UnkRen))
                                    .Distinct();

                var subsRp = await _mediatR.Send(new GetVenderRequestDataSubReportQuery(tenantId, ukesRp));

                if (!(subsRp?.Any() ?? false))
                {
                    return results;
                }

                var rp = ArrangeMainAndSubReport(mainsRp, subsRp);

                var newRp = await CollectDataForReport(rp, tenantId);

                FixMissingDataForTehaiAndKotei(newRp);

                return newRp;
            }
            else
            {
                return results;
            }
        }

        #region Collect data for report
        private void CollectDataForSijJoKNm(VenderRequestReportData item, VpmKyoSet kyoSet)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (kyoSet == null)
                throw new ArgumentNullException(nameof(kyoSet));

            item.SijJoKbnNm1 = kyoSet.SijJoKnm1;
            item.SijJoKbnNm2 = kyoSet.SijJoKnm2;
            item.SijJoKbnNm3 = kyoSet.SijJoKnm3;
            item.SijJoKbnNm4 = kyoSet.SijJoKnm4;
            item.SijJoKbnNm5 = kyoSet.SijJoKnm5;

        }

        private async Task CollectDataYfutTum(VenderRequestReportData item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var yFuttus = item.IsMainReport ? await _mediatR.Send(new GetYFuttuByBookingKeyAndRequestId(item.UkeNo, item.UnkRen, item.YouTblSeq)) :
                                              await _mediatR.Send(new GetYMFuttuByBookingKeyAndRequestKey(item.UkeNo, item.UnkRen, item.YouTblSeq, item.BunkRen));

            if (yFuttus != null)
            {
                item.YFuts = yFuttus.Where(_ => _.FutTumKbn == 1).ToList();
                item.YTums = yFuttus.Where(_ => _.FutTumKbn == 2).ToList();
            }
        }

        private async Task CollectDataBusLoansMainRp(VenderRequestReportData item, int tenantId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.BusLoanInfos = await _mediatR.Send(new GetCodeKbByBookingKeyAndYoushaQuery(item.YouTblSeq, item.UkeNo, item.UnkRen, tenantId, _codeSyService));
        }

        private async Task CollectDataBusLoansSubRp(VenderRequestReportData item, int tenantId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var busLoanInfo = await _mediatR.Send(new GetCodeKATAKBNByYoushaCodeKbnQuery(tenantId, item.YouKataKbn, _codeSyService));

            item.BusLoan1 = new VenderRequestReportBusLoanInfo
            {
                RyakuNm = busLoanInfo.FirstOrDefault()?.CodeKb_RyakuNm ?? string.Empty,
                IsMainReport = false,
            };
        }

        private async Task CollectDataCodeKb(VenderRequestReportData item, int tenantId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var codeZeikbn = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.YoushaZeiKbn, "ZEIKBN", tenantId, _codeSyService));

            var codeSijJoKbn1 = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.SijJokbn1Ryaku, "SIJJOKBN1", tenantId, _codeSyService));
            var codeSijJoKbn2 = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.SijJokbn2Ryaku, "SIJJOKBN2", tenantId, _codeSyService));
            var codeSijJoKbn3 = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.SijJokbn3Ryaku, "SIJJOKBN3", tenantId, _codeSyService));
            var codeSijJoKbn4 = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.SijJokbn4Ryaku, "SIJJOKBN4", tenantId, _codeSyService));
            var codeSijJoKbn5 = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.SijJokbn5Ryaku, "SIJJOKBN5", tenantId, _codeSyService));

            var codeOtherJin1 = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.OthJinKbn1Ryaku, "OTHJINKBN", tenantId, _codeSyService));
            var codeOtherJin2 = await _mediatR.Send(new GetCodeKbByCodeSyuAndCodeKbnQuery(item.OthJinKbn2Ryaku, "OTHJINKBN", tenantId, _codeSyService));

            item.ZeikbnRyakuNm = codeZeikbn.FirstOrDefault()?.CodeKb_RyakuNm ?? string.Empty;

            item.SijJokbn1Ryaku = codeSijJoKbn1.FirstOrDefault()?.CodeKbnName ?? string.Empty;
            item.SijJokbn2Ryaku = codeSijJoKbn2.FirstOrDefault()?.CodeKbnName ?? string.Empty;
            item.SijJokbn3Ryaku = codeSijJoKbn3.FirstOrDefault()?.CodeKbnName ?? string.Empty;
            item.SijJokbn4Ryaku = codeSijJoKbn4.FirstOrDefault()?.CodeKbnName ?? string.Empty;
            item.SijJokbn5Ryaku = codeSijJoKbn5.FirstOrDefault()?.CodeKbnName ?? string.Empty;

            item.OthJinKbn1Ryaku = codeOtherJin1.FirstOrDefault()?.CodeKbnName ?? string.Empty;
            item.OthJinKbn2Ryaku = codeOtherJin2.FirstOrDefault()?.CodeKbnName ?? string.Empty;
        }

        private async Task CollectDataForKoteiTehaiMainRp(VenderRequestReportData item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var koteis = await _mediatR.Send(new GetKoteiForMainRpByBookingKeyQuery(item.UkeNo, item.UnkRen, item.TeiDanNo));
            var tehais = await _mediatR.Send(new GetTehaiForMainRpByBookingKeyQuery(item.UkeNo, item.UnkRen, item.TeiDanNo));

            item.KoteiTehais = MergeKoteiAndTehai(koteis, tehais);
        }

        private async Task CollectDataForKoteiTehaiSubRp(VenderRequestReportData item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var koteis = await _mediatR.Send(new GetKoteiForSubRpByBookingKeyQuery(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen));
            var tehais = await _mediatR.Send(new GetTehaiForSubRpByBookingKeyQuery(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen));

            item.KoteiTehais = MergeKoteiAndTehai(koteis, tehais);
        }

        private List<KoteiTehaiVenderRequestReport> MergeKoteiAndTehai(List<KoteiTehaiVenderRequestReport> koteis, List<KoteiTehaiVenderRequestReport> tehais)
        {
            var listDate = koteis.Concat(tehais)
                                 .Select(_ => _.Date)
                                 .Distinct()
                                 .OrderBy(_ => _)
                                 .ToList();

            var koteiTehais = new List<KoteiTehaiVenderRequestReport>();

            listDate.ForEach(date =>
            {
                var listKoteiTehais = new List<KoteiTehaiVenderRequestReport>();

                var koteisByDate = koteis.Where(_ => _.Date == date).Select(_ => new KoteiTehaiVenderRequestReport
                {
                    Koutei = _.Koutei
                });
                var tehaisByDate = tehais.Where(_ => _.Date == date).Select(_ => new KoteiTehaiVenderRequestReport
                {
                    TehaiNm = _.TehaiNm,
                    TehaiTel = _.TehaiTel
                });

                if (koteisByDate.Count() >= (tehaisByDate.Count() * 2))
                {
                    listKoteiTehais.AddRange(koteisByDate);

                    foreach (var (item, index) in tehaisByDate.WithIndex())
                    {
                        listKoteiTehais[index * 2].TehaiDisplay = item.TehaiNm;
                        listKoteiTehais[(index * 2) + 1].TehaiDisplay = item.TehaiTel;
                    }
                }
                else
                {
                    tehaisByDate.ToList().ForEach(tehai =>
                    {
                        listKoteiTehais.AddRange(new List<KoteiTehaiVenderRequestReport>
                        {
                            new KoteiTehaiVenderRequestReport{TehaiDisplay = tehai.TehaiNm},
                            new KoteiTehaiVenderRequestReport{TehaiDisplay = tehai.TehaiTel},
                        });
                    });

                    foreach (var (item, index) in koteisByDate.WithIndex())
                    {
                        listKoteiTehais[index].Koutei = item.Koutei;
                    }
                }

                listKoteiTehais.First().Date = date;
                koteiTehais.AddRange(listKoteiTehais);
            });

            return koteiTehais;
        }

        private void FixMissingDataForTehaiAndKotei(List<VenderRequestReportData> reportDatas)
        {
            if (reportDatas == null)
                throw new ArgumentNullException(nameof(reportDatas));

            List<KoteiTehaiVenderRequestReport> koteiTehaisMainRp = new List<KoteiTehaiVenderRequestReport>();

            foreach (var item in reportDatas)
            {
                if (item.IsMainReport)
                {
                    koteiTehaisMainRp = item.KoteiTehais;
                }
                else
                {
                    if (!item.KoteiTehais.Any())
                    {
                        item.KoteiTehais = koteiTehaisMainRp;
                    }
                }
            }
        }

        private async Task CollectDataForMainReport(VenderRequestReportData item, int tenantId)
        {
            await CollectDataBusLoansMainRp(item, tenantId);
            await CollectDataForKoteiTehaiMainRp(item);
        }

        private async Task CollectDataForSubReport(VenderRequestReportData item, int tenantId)
        {
            await CollectDataBusLoansSubRp(item, tenantId);
            await CollectDataForKoteiTehaiSubRp(item);
        }

        private async Task CollectDataCommonForReport(VenderRequestReportData item, VpmKyoSet kyoSet, int tenantId)
        {
            CollectDataForSijJoKNm(item, kyoSet);
            await CollectDataCodeKb(item, tenantId);
            await CollectDataYfutTum(item);
        }

        private async Task<List<VenderRequestReportData>> CollectDataForReport(List<VenderRequestReportData> reportDatas, int tenantId)
        {
            if (reportDatas == null)
                throw new ArgumentNullException(nameof(reportDatas));

            var kyoSet = await _mediatR.Send(new GetTopOneKyoSetQuery());
            foreach (var item in reportDatas)
            {
                await CollectDataCommonForReport(item, kyoSet, tenantId);

                if (item.IsMainReport)
                {
                    await CollectDataForMainReport(item, tenantId);
                }
                else
                {
                    await CollectDataForSubReport(item, tenantId);
                }
            }

            return await Task.FromResult(reportDatas);
        }

        #endregion

        #region Paging report
        /// Logic pagging order: BusLoans -> YFuts -> YTums -> Kotei & Tehai
        private void PaggingBusLoans(List<VenderRequestReportData> reportDatas)
        {
            if (reportDatas == null)
                throw new ArgumentNullException(nameof(reportDatas));

            for (int index = 0; index < reportDatas.Count; index++)
            {
                var item = reportDatas.ElementAt(index);

                if (item.IsMainReport)
                {
                    if (!item.IsChildPage)// make sure just pagging for main page
                    {
                        var pageds = PagedList<VenderRequestReportBusLoanInfo>.GetAllPageds(item.BusLoanInfos.AsQueryable(), 5);

                        for (int ip = 0; ip < pageds.Count; ip++)
                        {
                            var page = pageds.ElementAt(ip);

                            if (ip == 0) //current page
                            {
                                FillDataForBusLoans(item, page);
                            }
                            else // extra page
                            {
                                if (!reportDatas.ElementAtOrDefault(index + ip)?.IsChildPage ?? true) // end of page or not have child page
                                {
                                    VenderRequestReportData newPage = new VenderRequestReportData();
                                    newPage.SimpleCloneProperties(item);
                                    newPage.BusLoanInfos = null;
                                    newPage.KoteiTehais = new PagedList<KoteiTehaiVenderRequestReport>(Enumerable.Repeat<KoteiTehaiVenderRequestReport>(null, 25).ToList());
                                    newPage.YFut1 = newPage.YFut2 = newPage.YFut3 = newPage.YFut4 = newPage.YFut5 = null;
                                    newPage.YTum1 = newPage.YTum2 = newPage.YTum3 = newPage.YTum4 = newPage.YTum5 = null;
                                    newPage.IsChildPage = true;

                                    FillDataForBusLoans(newPage, page);

                                    reportDatas.Insert(index + ip, newPage);
                                }
                                else
                                {
                                    FillDataForBusLoans(reportDatas.ElementAt(index + ip), page);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PaggingYFuts(List<VenderRequestReportData> reportDatas)
        {
            if (reportDatas == null)
                throw new ArgumentNullException(nameof(reportDatas));

            for (int index = 0; index < reportDatas.Count; index++)
            {
                var item = reportDatas.ElementAt(index);
                if (!item.IsChildPage)
                {
                    var pageds = PagedList<YFuttuVenderRequestReportData>.GetAllPageds(item.YFuts.AsQueryable(), 6);

                    for (int ip = 0; ip < pageds.Count; ip++)
                    {
                        var page = pageds.ElementAt(ip);

                        if (ip == 0)
                        {
                            FillDataForYFutTums(item, page);
                        }
                        else
                        {
                            if (!reportDatas.ElementAtOrDefault(index + ip)?.IsChildPage ?? true) // end of page or not have child page
                            {
                                VenderRequestReportData newPage = new VenderRequestReportData();
                                newPage.SimpleCloneProperties(item);
                                newPage.YFuts = null;
                                newPage.BusLoan1 = newPage.BusLoan2 = newPage.BusLoan3 = newPage.BusLoan4 = newPage.BusLoan5 = null;
                                newPage.YTum1 = newPage.YTum2 = newPage.YTum3 = newPage.YTum4 = newPage.YTum5 = null;
                                newPage.KoteiTehais = new PagedList<KoteiTehaiVenderRequestReport>(Enumerable.Repeat<KoteiTehaiVenderRequestReport>(null, 25).ToList());
                                newPage.IsChildPage = true;

                                FillDataForYFutTums(newPage, page);

                                reportDatas.Insert(index + ip, newPage);
                            }
                            else
                            {
                                FillDataForYFutTums(reportDatas.ElementAt(index + ip), page);
                            }
                        }
                    }
                }
            }
        }

        private void PaggingYTums(List<VenderRequestReportData> reportDatas)
        {
            if (reportDatas == null)
                throw new ArgumentNullException(nameof(reportDatas));

            for (int index = 0; index < reportDatas.Count; index++)
            {
                var item = reportDatas.ElementAt(index);

                if (!item.IsChildPage)
                {
                    var pageds = PagedList<YFuttuVenderRequestReportData>.GetAllPageds(item.YTums.AsQueryable(), 6);

                    for (int ip = 0; ip < pageds.Count; ip++)
                    {
                        var page = pageds.ElementAt(ip);

                        if (ip == 0) //current page
                        {
                            FillDataForYFutTums(item, page, false);
                        }
                        else // extra page
                        {
                            if (!reportDatas.ElementAtOrDefault(index + ip)?.IsChildPage ?? true) // end of page or not have child page
                            {
                                VenderRequestReportData newPage = new VenderRequestReportData();
                                newPage.SimpleCloneProperties(item);
                                newPage.YTums = null;
                                newPage.BusLoan1 = newPage.BusLoan2 = newPage.BusLoan3 = newPage.BusLoan4 = newPage.BusLoan5 = null;
                                newPage.YFut1 = newPage.YFut2 = newPage.YFut3 = newPage.YFut4 = newPage.YFut5 = null;
                                newPage.KoteiTehais = new PagedList<KoteiTehaiVenderRequestReport>(Enumerable.Repeat<KoteiTehaiVenderRequestReport>(null, 25).ToList());
                                newPage.IsChildPage = true;

                                FillDataForYFutTums(newPage, page, false);

                                reportDatas.Insert(index + ip, newPage);
                            }
                            else
                            {
                                FillDataForYFutTums(reportDatas.ElementAt(index + ip), page, false);
                            }
                        }
                    }
                }
            }
        }

        private void PaggingKoteiTehai(List<VenderRequestReportData> reportDatas)
        {
            if (reportDatas == null)
                throw new ArgumentNullException(nameof(reportDatas));

            int itemPerPage = 25;

            for (int index = 0; index < reportDatas.Count; index++)
            {
                var item = reportDatas.ElementAt(index);

                if (!item.IsChildPage)
                {
                    var pageds = PagedList<KoteiTehaiVenderRequestReport>.GetAllPageds(item.KoteiTehais.AsQueryable(), itemPerPage);
                    if (pageds.Any())
                    {
                        pageds.ElementAt(pageds.Count - 1).AddRange(Enumerable.Repeat<KoteiTehaiVenderRequestReport>(null, itemPerPage - pageds.ElementAt(pageds.Count - 1).Count));
                    }
                    else
                    {
                        var emptyPage = new PagedList<KoteiTehaiVenderRequestReport>(Enumerable.Repeat<KoteiTehaiVenderRequestReport>(null, itemPerPage).ToList());
                        pageds.Add(emptyPage);
                    }

                    for (int ip = 0; ip < pageds.Count; ip++)
                    {
                        var page = pageds.ElementAt(ip);

                        if (ip == 0) //current page
                        {
                            item.KoteiTehais = page;
                        }
                        else // extra page
                        {

                            if (!reportDatas.ElementAtOrDefault(index + ip)?.IsChildPage ?? true) // end of page or not have child page
                            {
                                VenderRequestReportData newPage = new VenderRequestReportData();
                                newPage.SimpleCloneProperties(item);
                                newPage.BusLoan1 = newPage.BusLoan2 = newPage.BusLoan3 = newPage.BusLoan4 = newPage.BusLoan5 = null;
                                newPage.YFut1 = newPage.YFut2 = newPage.YFut3 = newPage.YFut4 = newPage.YFut5 = null;
                                newPage.YTum1 = newPage.YTum2 = newPage.YTum3 = newPage.YTum4 = newPage.YTum5 = null;
                                newPage.KoteiTehais = null;
                                newPage.IsChildPage = true;

                                newPage.KoteiTehais = page;

                                reportDatas.Insert(index + ip, newPage);
                            }
                            else
                            {
                                reportDatas.ElementAtOrDefault(index + ip).KoteiTehais = page;
                            }
                        }
                    }
                }
            }
        }

        private void FillDataForYFutTums(VenderRequestReportData item, List<YFuttuVenderRequestReportData> data, bool isFuts = true)
        {
            try
            {
                if (isFuts)
                {
                    item.YFut1 = item.YFut2 = item.YFut3 = item.YFut4 = item.YFut5 = item.YFut6 = null;

                    item.YFut1 = data[0];
                    item.YFut2 = data[1];
                    item.YFut3 = data[2];
                    item.YFut4 = data[3];
                    item.YFut5 = data[4];
                    item.YFut6 = data[5];
                }
                else
                {
                    item.YTum1 = item.YTum2 = item.YTum3 = item.YTum4 = item.YTum5 = item.YTum6 = null;

                    item.YTum1 = data[0];
                    item.YTum2 = data[1];
                    item.YTum3 = data[2];
                    item.YTum4 = data[3];
                    item.YTum5 = data[4];
                    item.YTum6 = data[5];
                }
            }
            catch { }
        }

        private void FillDataForBusLoans(VenderRequestReportData item, List<VenderRequestReportBusLoanInfo> busLoanInfos)
        {
            try
            {
                item.BusLoan1 = item.BusLoan2 = item.BusLoan3 = item.BusLoan4 = item.BusLoan5 = null;

                item.BusLoan1 = new VenderRequestReportBusLoanInfo { RyakuNm = busLoanInfos[0].RyakuNm, SyaRyoUnc = busLoanInfos[0].SyaRyoUnc, SyaSyuTan = busLoanInfos[0].SyaSyuTan, SyaSyuDai = busLoanInfos[0].SyaSyuDai };
                item.BusLoan2 = new VenderRequestReportBusLoanInfo { RyakuNm = busLoanInfos[1].RyakuNm, SyaRyoUnc = busLoanInfos[1].SyaRyoUnc, SyaSyuTan = busLoanInfos[1].SyaSyuTan, SyaSyuDai = busLoanInfos[1].SyaSyuDai };
                item.BusLoan3 = new VenderRequestReportBusLoanInfo { RyakuNm = busLoanInfos[2].RyakuNm, SyaRyoUnc = busLoanInfos[2].SyaRyoUnc, SyaSyuTan = busLoanInfos[2].SyaSyuTan, SyaSyuDai = busLoanInfos[2].SyaSyuDai };
                item.BusLoan4 = new VenderRequestReportBusLoanInfo { RyakuNm = busLoanInfos[3].RyakuNm, SyaRyoUnc = busLoanInfos[3].SyaRyoUnc, SyaSyuTan = busLoanInfos[3].SyaSyuTan, SyaSyuDai = busLoanInfos[3].SyaSyuDai };
                item.BusLoan5 = new VenderRequestReportBusLoanInfo { RyakuNm = busLoanInfos[4].RyakuNm, SyaRyoUnc = busLoanInfos[4].SyaRyoUnc, SyaSyuTan = busLoanInfos[4].SyaSyuTan, SyaSyuDai = busLoanInfos[4].SyaSyuDai };
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// Merge list sub pages into list main pages
        /// </summary>
        /// <param name="mains"></param>
        /// <param name="subs"></param>
        /// <returns></returns>
        private List<VenderRequestReportData> ArrangeMainAndSubReport(List<VenderRequestReportData> mains, List<VenderRequestReportData> subs)
        {
            if (mains is null)
                throw new ArgumentNullException(nameof(mains));

            if (subs is null)
                throw new ArgumentNullException(nameof(subs));

            List<VenderRequestReportData> results = new List<VenderRequestReportData>();

            mains.OrderBy(_ => _.UkeNo)
                .ThenBy(_ => _.UnkRen)
                .ToList()
                .ForEach(main =>
            {
                results.Add(main);

                var childs = subs.Where(_ => _.UkeNo == main.UkeNo && _.UnkRen == main.UnkRen && _.YouTblSeq == main.YouTblSeq);
                results.AddRange(childs);
            });

            return results;
        }

        public Task<List<VenderRequestReportData>> GetVenderRequestReportPaged(List<VenderRequestReportData> reportDatas)
        {
            PaggingBusLoans(reportDatas);
            PaggingYFuts(reportDatas);
            PaggingYTums(reportDatas);
            PaggingKoteiTehai(reportDatas);

            return Task.FromResult(reportDatas);
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            ObjectDataSource dataSource = new ObjectDataSource();
            var previewParam = EncryptHelper.DecryptFromUrl<VenderRequestFormData>(queryParams);
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Venderrequestform, BaseNamespace.Venderrequestform, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);

            var reportDatas = await GetVenderRequestReport(previewParam, new ClaimModel().TenantID);

            var reportResult = await GetVenderRequestReportPaged(reportDatas);

            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<VenderRequestReportData>),
                Value = reportResult
            };
            dataSource.Name = "objectDataSource2";
            dataSource.DataSource = typeof(VenderRequestReportDataSource);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<bool> VenderRequestReportHadData(VenderRequestFormData reportCondition, int tenantId)
        {
            var mainsRp = await _mediatR.Send(new GetVenderRequestDataMainReportQuery(tenantId, reportCondition));

            return mainsRp?.Any() ?? false;
        }

        public Dictionary<string, string> GetFieldValues(VenderRequestFormData data)
        {
            return new Dictionary<string, string>
            {
                [nameof(data.StartDate)] = data.StartDate.ToString("yyyyMMdd"),
                [nameof(data.EndDate)] = data.EndDate.ToString("yyyyMMdd"),
                [nameof(data.UkeCdFrom)] = data.UkeCdFrom,
                [nameof(data.UkeCdTo)] = data.UkeCdTo,
                [nameof(data.BookingTypeStart)] = data.BookingTypeStart?.YoyaKbnSeq.ToString() ?? string.Empty,
                [nameof(data.BookingTypeEnd)] = data.BookingTypeEnd?.YoyaKbnSeq.ToString() ?? string.Empty,
                [nameof(data.Branch)] = data.Branch?.EigyoCdSeq.ToString() ?? string.Empty,
                [nameof(data.SelectedGyosyaFrom)] = data.SelectedGyosyaFrom != null ? data.SelectedGyosyaFrom.GyosyaCdSeq.ToString() : string.Empty,
                [nameof(data.SelectedTokiskFrom)] = data.SelectedTokiskFrom != null ? data.SelectedTokiskFrom.TokuiSeq.ToString() : string.Empty,
                [nameof(data.SelectedTokiStFrom)] = data.SelectedTokiStFrom != null ? data.SelectedTokiStFrom.SitenCdSeq.ToString() : string.Empty,
                [nameof(data.SelectedGyosyaTo)] = data.SelectedGyosyaTo != null ? data.SelectedGyosyaTo.GyosyaCdSeq.ToString() : string.Empty,
                [nameof(data.SelectedTokiskTo)] = data.SelectedTokiskTo != null ? data.SelectedTokiskTo.TokuiSeq.ToString() : string.Empty,
                [nameof(data.SelectedTokiStTo)] = data.SelectedTokiStTo != null ? data.SelectedTokiStTo.SitenCdSeq.ToString() : string.Empty,
            };
        }

        public void ApplyFilter(ref VenderRequestFormData data, Dictionary<string, string> filterValues, List<ReservationClassComponentData> listReservation,
            List<CustomerComponentGyosyaData> listGyosya, List<CustomerComponentTokiskData> listTokisk, List<CustomerComponentTokiStData> listTokist)
        {
            string outValueString = string.Empty;
            DateTime dt = new DateTime();
            var dataPropList = data
                .GetType()
                .GetProperties()
                .Where(d => d.CanWrite && d.CanRead)
                .ToList();
            foreach (var dataProp in dataPropList)
            {
                if (filterValues.TryGetValue(dataProp.Name, out outValueString))
                {
                    if (dataProp.PropertyType.IsGenericType || dataProp.PropertyType.IsClass && dataProp.PropertyType != typeof(string))
                    {
                        continue;
                    }
                    dynamic setValue = null;
                    if (dataProp.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParseExact(outValueString, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                        {
                            setValue = dt;
                        }
                    }
                    else if (dataProp.PropertyType == typeof(string))
                    {
                        setValue = outValueString;
                    }
                    dataProp.SetValue(data, setValue);
                }
            }

            if (filterValues.ContainsKey(nameof(data.Branch)))
            {
                if (int.TryParse(filterValues[nameof(data.Branch)], out int outValue))
                {
                    data.Branch = new LoadSaleBranch() { EigyoCdSeq = outValue };
                }
            }
            if (filterValues.ContainsKey(nameof(data.SelectedGyosyaFrom)))
            {
                var cusIds = filterValues[nameof(data.SelectedGyosyaFrom)];
                if (int.TryParse(cusIds, out int outValue))
                {
                    data.SelectedGyosyaFrom = listGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
            }
            if (filterValues.ContainsKey(nameof(data.SelectedTokiskFrom)))
            {
                var cusIds = filterValues[nameof(data.SelectedTokiskFrom)];
                if (int.TryParse(cusIds, out int outValue))
                {
                    var gyosyaCdSeq = data.SelectedGyosyaFrom.GyosyaCdSeq;
                    data.SelectedTokiskFrom = listTokisk.FirstOrDefault(_ => _.GyosyaCdSeq == gyosyaCdSeq && _.TokuiSeq == outValue);
                }
            }
            if (filterValues.ContainsKey(nameof(data.SelectedTokiStFrom)))
            {
                var cusIds = filterValues[nameof(data.SelectedTokiStFrom)];
                if (int.TryParse(cusIds, out int outValue))
                {
                    var tokuiSeq = data.SelectedTokiskFrom.TokuiSeq;
                    data.SelectedTokiStFrom = listTokist.FirstOrDefault(_ => _.TokuiSeq == tokuiSeq && _.SitenCdSeq == outValue);
                }
            }
            if (filterValues.ContainsKey(nameof(data.SelectedGyosyaTo)))
            {
                var cusIds = filterValues[nameof(data.SelectedGyosyaTo)];
                if (int.TryParse(cusIds, out int outValue))
                {
                    data.SelectedGyosyaTo = listGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                }
            }
            if (filterValues.ContainsKey(nameof(data.SelectedTokiskTo)))
            {
                var cusIds = filterValues[nameof(data.SelectedTokiskTo)];
                if (int.TryParse(cusIds, out int outValue))
                {
                    var gyosyaCdSeq = data.SelectedGyosyaTo.GyosyaCdSeq;
                    data.SelectedTokiskTo = listTokisk.FirstOrDefault(_ => _.GyosyaCdSeq == gyosyaCdSeq && _.TokuiSeq == outValue);
                }
            }
            if (filterValues.ContainsKey(nameof(data.SelectedTokiStTo)))
            {
                var cusIds = filterValues[nameof(data.SelectedTokiStTo)];
                if (int.TryParse(cusIds, out int outValue))
                {
                    var tokuiSeq = data.SelectedTokiskTo.TokuiSeq;
                    data.SelectedTokiStTo = listTokist.FirstOrDefault(_ => _.TokuiSeq == tokuiSeq && _.SitenCdSeq == outValue);
                }
            }
            if (filterValues.ContainsKey(nameof(data.BookingTypeStart)))
            {
                if (int.TryParse(filterValues[nameof(data.BookingTypeStart)], out int outValue))
                {
                    data.BookingTypeStart = listReservation.FirstOrDefault(_ => _.YoyaKbnSeq == outValue);
                }
            }
            if (filterValues.ContainsKey(nameof(data.BookingTypeEnd)))
            {
                if (int.TryParse(filterValues[nameof(data.BookingTypeEnd)], out int outValue))
                {
                    data.BookingTypeEnd = listReservation.FirstOrDefault(_ => _.YoyaKbnSeq == outValue);
                }
            }
        }

        public async void ExportPDF(List<VenderRequestReportData> listData, IJSRuntime IJSRuntime)
        {
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Venderrequestform, BaseNamespace.Venderrequestform, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);
            report.DataSource = listData;
            await new System.Threading.Tasks.TaskFactory().StartNew(() =>
            {
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {
                    report.ExportToPdf(ms);

                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    IJSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "VenderRequestReport");
                }
            });
        }

        public async Task<(bool, List<VenderRequestReportData>, VenderRequestFormData)> SetParamAndCheckData(string UkeCd, string Date, string UnkRen)
        {
            DateTime dateTimeConvert;
            VenderRequestFormData searchParam = new VenderRequestFormData();
            dateTimeConvert = DateTime.ParseExact(Date, "yyyyMMdd", new CultureInfo("ja-JP"));
            searchParam.UkeCdFrom = UkeCd;
            searchParam.UkeCdTo = UkeCd;
            searchParam._ukeCdFrom = int.Parse(UkeCd);
            searchParam._ukeCdTo = int.Parse(UkeCd);
            searchParam.StartDate = dateTimeConvert;
            searchParam.EndDate = dateTimeConvert;
            searchParam.OutputSetting = OutputInstruction.Pdf;
            var reportDatas = await GetVenderRequestReport(searchParam, new ClaimModel().TenantID);
            if (reportDatas != null && reportDatas.Any())
            {
                var reportResult = await GetVenderRequestReportPaged(reportDatas);
                if (reportResult.Any())
                {
                    return (false, reportResult, searchParam);
                }
                else
                {
                    return (true, new List<VenderRequestReportData>(), searchParam);
                }
            }
            else
            {
                return (true, new List<VenderRequestReportData>(), searchParam);
            }
        }
    }
}