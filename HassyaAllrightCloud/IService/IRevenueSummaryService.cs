using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.RevenueSummary.Queries;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Pages;
using MediatR;
using System.Linq;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using HassyaAllrightCloud.Reports.ReportTemplate.TransportationRevenue;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DataAccess.ObjectBinding;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Reports.DataSource;

namespace HassyaAllrightCloud.IService
{
    public interface IRevenueSummaryService : IReportService
    {
        /// <summary>
        /// Get combobox items for show header combobox
        /// </summary>
        /// <returns></returns>
        List<ComboboxBaseItem> GetShowHeaderOptions();
        /// <summary>
        /// Get combobox items for KukuriKbn combobox
        /// </summary>
        /// <returns></returns>
        List<ComboboxBaseItem> GetKukuriKbnItems();
        /// <summary>
        /// Get combobox items for Separator combobox
        /// </summary>
        /// <returns></returns>
        List<ComboboxBaseItem> GetSeparatorOptions();
        /// <summary>
        /// Get combobox items for YoyaKbn combobox
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<YoyaKbnDto>> GetYoyaKbnItems(int tenantId);
        /// <summary>
        /// Get combobox items for TesuInKbn combobox
        /// </summary>
        /// <returns></returns>
        List<ComboboxBaseItem> GetTesuInKbnItems();
        /// <summary>
        /// Get Daily Transportation Revenue data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DailyRevenueData> GetDailyRevenueData(DailyRevenueSearchModel model);
        /// <summary>
        /// Get Monthly Trasportation Revenue Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<MonthlyRevenueData> GetMonthlyRevenueData(MonthlyRevenueSearchModel model);
        /// <summary>
        /// Get list of Eigyo are available for Daily Transportation Revenue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<EigyoListItem>> GetEigyoListForDailyRevenueReport(TransportationRevenueSearchModel model);
        /// <summary>
        /// Get List of UriYmd are available for Daily Transportation Revenue Report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<string>> GetUriYmdForDailyRevenueReport(DailyRevenueSearchModel model);
        /// <summary>
        /// Get list of Eigyo are available for Monthly Transportation Revenue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<EigyoListItem>> GetEigyoListForMonthlyRevenueReport(TransportationRevenueSearchModel model);
        /// <summary>
        /// Get daily transportation revenue report data 
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<List<DailyRevenueReportData>> GetDailyRevenueReportData(TransportationRevenueSearchModel searchModel);
        /// <summary>
        /// Get monthly transportation revenue report data 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<MonthlyRevenueReportData>> GetMonthlyRevenueReportData(TransportationRevenueSearchModel model);
        /// <summary>
        /// Get daily transportation revenue report data to export csv
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<StringBuilder> GetDailyRevenueCSVReportData(TransportationRevenueSearchModel searchModel);
        /// <summary>
        /// Get monthly transportation revenue report data to export csv
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<StringBuilder> GetMonthlyRevenueCSVReportData(TransportationRevenueSearchModel model);
        /// <summary>
        /// Get combobox items for PageSize combobox
        /// </summary>
        /// <returns></returns>
        IEnumerable<ComboboxBaseItem> GetPageSizes();
        XtraReport GetRevenueReportInstanceByPageSize(PageSize size, bool isDailyReport);
    }

    public class RevenueSummaryService : IRevenueSummaryService
    {
        private IStringLocalizer<RevenueSummary> _lang;
        private IMediator _mediator;
        public const string KinouId = "KS1100";
        private IReportLoadingService _reportLoadingService { get; }

        public RevenueSummaryService(
            IStringLocalizer<RevenueSummary> lang,
            IMediator mediator,
            IReportLoadingService reportLoadingService)
        {
            _lang = lang;
            _mediator = mediator;
            _reportLoadingService = reportLoadingService;
        }

        public List<ComboboxBaseItem> GetShowHeaderOptions()
        {
            return new List<ComboboxBaseItem>()
            {
                new ComboboxBaseItem()
                {
                    Text = _lang["ShowHeaderComboboxItem"],
                    Value = (int)ShowHeaderEnum.ShowHeader
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["DoNotShowHeaderComboboxItem"],
                    Value = (int)ShowHeaderEnum.DoNotShowHeader
                }
            };
        }

        public List<ComboboxBaseItem> GetKukuriKbnItems()
        {
            return new List<ComboboxBaseItem>()
            {
                new ComboboxBaseItem()
                {
                    Text = _lang["PutInDoubleQuaterComboboxItem"],
                    Value = (int)WrapContentEnum.PutInDoubleQuater
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["DoNotPutInDoubleQuaterComboboxItem"],
                    Value = (int)WrapContentEnum.DoNotPutInDoubleQuater
                }
            };
        }

        public List<ComboboxBaseItem> GetSeparatorOptions()
        {
            return new List<ComboboxBaseItem>()
            {
                new ComboboxBaseItem()
                {
                    Text = _lang["SeparateByTabComboboxItem"],// tab
                    Value = (int)SeperatorEnum.ByTab
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["SeparateBySemicolonComboboxItem"],//semicolon
                    Value = (int)SeperatorEnum.BySemicolon
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["SeparateByCommaComboboxItem"],//comma
                    Value = (int)SeperatorEnum.ByComma
                }
            };
        }

        public async Task<IEnumerable<YoyaKbnDto>> GetYoyaKbnItems(int tenantId)
        {
            return await _mediator.Send(new GetYoyaKbns() { TenantId = tenantId});
        }

        public List<ComboboxBaseItem> GetTesuInKbnItems()
        {
            return new List<ComboboxBaseItem>()
            {
                new ComboboxBaseItem()
                {
                    Text = _lang["IncludingFeeComboboxItem"],
                    Value = (int)TesuInKbnEnum.IncludingFee
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["WithoutCommissionComboboxItem"],
                    Value = (int)TesuInKbnEnum.WithoutCommission
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["MasterSettingComboboxItem"],
                    Value = (int)TesuInKbnEnum.MasterSetting
                }
            };
        }

        public async Task<DailyRevenueData> GetDailyRevenueData(DailyRevenueSearchModel model)
        {
            return await _mediator.Send(new GetDailyRevenueData() { SearchModel = model });
        }

        public async Task<MonthlyRevenueData> GetMonthlyRevenueData(MonthlyRevenueSearchModel model)
        {
            return await _mediator.Send(new GetMonthlyRevenueData() { SearchModel = model });
        }

        public async Task<List<EigyoListItem>> GetEigyoListForMonthlyRevenueReport(TransportationRevenueSearchModel model)
        {
            return await _mediator.Send(new GetEigyoListForMonthlyRevenueReport() { SearchModel = model });
        }

        public async Task<List<EigyoListItem>> GetEigyoListForDailyRevenueReport(TransportationRevenueSearchModel model)
        {
            return await _mediator.Send(new GetEigyoListForDailyRevenueReport() { SearchModel = model });
        }

        public async Task<List<string>> GetUriYmdForDailyRevenueReport(DailyRevenueSearchModel model)
        {
            return await _mediator.Send(new GetUriYmdForDailyRevenueReport() { SearchModel = model });
        }

        public IEnumerable<ComboboxBaseItem> GetPageSizes()
        {
            return new List<ComboboxBaseItem>()
            {
                new ComboboxBaseItem()
                {
                    Text = _lang["A4"],
                    Value = (int)PageSize.A4
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["A3"],
                    Value = (int)PageSize.A3
                },
                new ComboboxBaseItem()
                {
                    Text = _lang["B4"],
                    Value = (int)PageSize.B4
                }
            };
        }

        public List<List<T>> SubList<T>(List<T> list, int num)
        {
            List<List<T>> result = new List<List<T>>();
            var count = list.Count / num;
            var temp = list.Count % num;
            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    result.Add(list.Skip(i * num).Take(num).ToList());
                }
                if (temp != 0)
                    result.Add(list.Skip(count * num).Take(temp).ToList());
            }
            else
                result.Add(list);

            return result;
        }

        public List<MonthlyRevenueReportItem> ConvertToReportItems(List<MonthlyRevenueItem> items)
        {
            var result = new List<MonthlyRevenueReportItem>();
            var index = 0;
            foreach (var item in items)
            {
                var temp = new MonthlyRevenueReportItem();
                temp.No = (++index).AddCommas();
                temp.UriYmd = string.IsNullOrEmpty(item.UriYmd) ? string.Empty : item.UriYmd.Substring(item.UriYmd.Length - 2);
                temp.KeiKin = item.KeiKin.AddCommas();
                temp.JisSyaSyuDai = item.JisSyaSyuDai.AddCommas();
                temp.JisSyaRyoUnc = item.JisSyaRyoUnc.AddCommas();
                temp.JisSyaRyoSyo = item.JisSyaRyoSyo.AddCommas();
                temp.JisSyaRyoTes = item.JisSyaRyoTes.AddCommas();
                temp.JisSyaRyoSum = item.JisSyaRyoSum.AddCommas();
                temp.GaiUriGakKin = item.GaiUriGakKin.AddCommas();
                temp.GaiSyaRyoSyo = item.GaiSyaRyoSyo.AddCommas();
                temp.GaiSyaRyoTes = item.GaiSyaRyoTes.AddCommas();
                temp.GaiSyaRyoSum = item.GaiSyaRyoSum.AddCommas();
                temp.EtcUriGakKin = item.EtcUriGakKin.AddCommas();
                temp.EtcSyaRyoSyo = item.EtcSyaRyoSyo.AddCommas();
                temp.EtcSyaRyoTes = item.EtcSyaRyoTes.AddCommas();
                temp.EtcSyaRyoSum = item.EtcSyaRyoSum.AddCommas();
                temp.CanUnc = item.CanUnc.AddCommas();
                temp.CanSyoG = item.CanSyoG.AddCommas();
                temp.CanSum = item.CanSum.AddCommas();
                temp.UntSoneki = item.UntSoneki.AddCommas();
                if (item.DetailItems.Any())
                {
                    var subIndex = 0;
                    foreach (var detail in item.DetailItems)
                    {
                        if (subIndex != 0)
                        {
                            temp = new MonthlyRevenueReportItem();
                            temp.No = (++index).AddCommas();
                        }

                        temp.YouRyakuNm = detail.YouRyakuNm;
                        temp.YouSitRyakuNm = detail.YouSitRyakuNm;
                        temp.YouSyaSyuDai = detail.YouSyaSyuDai.AddCommas();
                        temp.YouSyaRyoUnc = detail.YouSyaRyoUnc.AddCommas();
                        temp.YouSyaRyoSyo = detail.YouSyaRyoSyo.AddCommas();
                        temp.YouSyaRyoTes = detail.YouSyaRyoTes.AddCommas();
                        temp.YouG = detail.YouG.AddCommas();
                        temp.YfuUriGakKin = detail.YfuUriGakKin.AddCommas();
                        temp.YfuSyaRyoSyo = detail.YfuSyaRyoSyo.AddCommas();
                        temp.YfuSyaRyoTes = detail.YfuSyaRyoTes.AddCommas();
                        temp.YouFutG = detail.YouFutG.AddCommas();
                        subIndex++;
                        result.Add(temp);
                    }
                }
                else
                {
                    result.Add(temp);
                }
            }

            return result;
        }

        public List<DailyRevenueReportItem> ConvertToReportItems(List<DailyRevenueItem> items)
        {
            var result = new List<DailyRevenueReportItem>();
            var index = 0;
            foreach (var item in items)
            {
                var temp = new DailyRevenueReportItem();
                temp.No = (++index).AddCommas();
                temp.TokRyakuNm = item.TokRyakuNm;
                temp.SitRyakuNm = item.SitRyakuNm;
                temp.UkeNo = item.UkeNo;
                temp.DanTaNm = item.DanTaNm;
                temp.IkNm = item.IkNm;
                temp.KeiKin = item.KeiKin.AddCommas();
                temp.JisSyaSyuDai = item.JisSyaSyuDai.AddCommas();
                temp.JisSyaRyoUnc = item.JisSyaRyoUnc.AddCommas();
                temp.JisSyaRyoSyo = item.JisSyaRyoSyo.AddCommas();
                temp.JisSyaRyoTes = item.JisSyaRyoTes.AddCommas();
                temp.JisSyaRyoSum = item.JisSyaRyoSum.AddCommas();
                temp.GaiUriGakKin = item.GaiUriGakKin.AddCommas();
                temp.GaiSyaRyoSyo = item.GaiSyaRyoSyo.AddCommas();
                temp.GaiSyaRyoTes = item.GaiSyaRyoTes.AddCommas();
                temp.GaiSyaRyoSum = item.GaiSyaRyoSum.AddCommas();
                temp.EtcUriGakKin = item.EtcUriGakKin.AddCommas();
                temp.EtcSyaRyoSyo = item.EtcSyaRyoSyo.AddCommas();
                temp.EtcSyaRyoTes = item.EtcSyaRyoTes.AddCommas();
                temp.EtcSyaRyoSum = item.EtcSyaRyoSum.AddCommas();
                temp.CanUnc = item.CanUnc.AddCommas();
                temp.CanSyoG = item.CanSyoG.AddCommas();
                temp.CanSum = item.CanSum.AddCommas();
                temp.UntSoneki = item.UntSoneki.AddCommas();

                if (item.DetailItems.Any())
                {
                    var subIndex = 0;
                    foreach (var detail in item.DetailItems)
                    {
                        if (subIndex != 0)
                        {
                            temp = new DailyRevenueReportItem();
                            temp.No = (++index).AddCommas();
                        }

                        temp.YouRyakuNm = detail.YouRyakuNm;
                        temp.YouSitRyakuNm = detail.YouSitRyakuNm;
                        temp.YouSyaSyuDai = detail.YouSyaSyuDai.AddCommas();
                        temp.YouSyaRyoUnc = detail.YouSyaRyoUnc.AddCommas();
                        temp.YouSyaRyoSyo = detail.YouSyaRyoSyo.AddCommas();
                        temp.YouSyaRyoTes = detail.YouSyaRyoTes.AddCommas();
                        temp.YouG = detail.YouG.AddCommas();
                        temp.YfuUriGakKin = detail.YfuUriGakKin.AddCommas();
                        temp.YfuSyaRyoSyo = detail.YfuSyaRyoSyo.AddCommas();
                        temp.YfuSyaRyoTes = detail.YfuSyaRyoTes.AddCommas();
                        temp.YouFutG = detail.YouFutG.AddCommas();
                        subIndex++;
                        result.Add(temp);
                    }
                }
                else
                {
                    result.Add(temp);
                }
            }

            return result;
        }

        public async Task<List<DailyRevenueReportData>> GetDailyRevenueReportData(TransportationRevenueSearchModel model)
        {
            var reportDataSource = new List<DailyRevenueReportData>();
            var now = DateTime.Now;
            var pageSize = 24;
            var key = model.ReportId;
            var searchModels = new List<DailyRevenueSearchModel>();
            var eigyoListItems = await GetEigyoListForDailyRevenueReport(model);

            var tesukomiKbn = GetTesuInKbnItems().Find(o => o.Value == (int)model.TesuInKbn)?.Text;

            if (eigyoListItems.Any())
            {
                foreach (var eigyo in eigyoListItems)
                {
                    var dailyModel = new DailyRevenueSearchModel();
                    dailyModel.Eigyo = eigyo;
                    dailyModel.RevenueSearchModel = model;
                    var uriYmdList = await GetUriYmdForDailyRevenueReport(dailyModel);
                    foreach (var uriYmd in uriYmdList)
                    {
                        var searchModel = new DailyRevenueSearchModel();
                        searchModel.RevenueSearchModel = model;
                        searchModel.UriYmd = uriYmd;
                        searchModel.Eigyo = eigyo;
                        searchModel.TesukomiKbn = tesukomiKbn;
                        searchModels.Add(searchModel);
                    }
                }
            }
            var index = 0;
            foreach (var searchModel in searchModels)
            {
                index++;
                if (_reportLoadingService.IsCanceled(key)) break;
                else await _reportLoadingService.UpdateProgress(GetPercent(index, searchModels.Count), key);

                var result = await GetDailyRevenueData(searchModel);
                if (!result.DailyRevenueItems.Any()) continue;
                var s1 = result.SummaryResult.FirstOrDefault(i => i.MesaiKbn == 1);
                var s2 = result.SummaryResult.FirstOrDefault(i => i.MesaiKbn == 2);
                var s1Detail = result.DetailItems.FirstOrDefault(i => i.MesaiKbn == 1);
                var s2Detail = result.DetailItems.FirstOrDefault(i => i.MesaiKbn == 2);

                var reportItems = ConvertToReportItems(result.DailyRevenueItems.ToList());
                var dataList = SubList(reportItems, pageSize);

                var commonData = new DailyRevenueCommonData()
                {
                    UriYmd = $": {searchModel.UriYmd}",
                    EigyoKbnNm = searchModel.RevenueSearchModel.EigyoKbn == EigyoKbnEnum.ReceptionOffice ? _lang["ReceptionOfficeLabel"] : _lang["BillingOfficeLabel"],
                    EigyoKbn = $": {searchModel.Eigyo?.EigyoCd:00000} {searchModel.Eigyo?.RyakuNm}",
                    TesukomiKbnNm = $": {searchModel.TesukomiKbn}",
                    EigyoFrom = searchModel.RevenueSearchModel.EigyoFrom == 0 ? ":" : $": {searchModel.RevenueSearchModel.EigyoFrom:00000} {searchModel.RevenueSearchModel.EigyoNmFrom}",
                    EigyoTo = searchModel.RevenueSearchModel.EigyoTo == 0 ? string.Empty : $"{searchModel.RevenueSearchModel.EigyoTo:00000} {searchModel.RevenueSearchModel.EigyoNmTo}",
                    UriYmdFrom = $": {searchModel.RevenueSearchModel.UriYmdFrom.AddSlash2YYYYMMDD()}",
                    UriYmdTo = $"{searchModel.RevenueSearchModel.UriYmdTo.AddSlash2YYYYMMDD()}",
                    Syain = $"{new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd} {new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name}",
                    ProcessingDate = now.ToString(DateTimeFormat.yyyyMMddSlashHHmmColon)
                };

                var summaries = new List<DailyRevenueSummary>()
                            {
                                new DailyRevenueSummary()
                                {
                                    Name = _lang["TotalByDay"] ,
                                    ZeiKomi = s1.SJyuSyaRyoRui.AddCommas(),
                                    JisSyaSyuDai = result.DailyRevenueItems.Sum(i => i.JisSyaSyuDai).AddCommas(),
                                    JisSyaRyoUnc = s1.SJisSyaRyoUnc.AddCommas(),
                                    JisSyaRyoSyo = s1.SJisSyaRyoSyo.AddCommas(),
                                    JisSyaRyoTes = s1.SJisSyaRyoTes.AddCommas(),
                                    JisG = (s1.SJisSyaRyoUnc + s1.SJisSyaRyoSyo).AddCommas(),
                                    GuiUri = result.DailyRevenueItems.Sum(i => i.GaiUriGakKin).AddCommas(),
                                    GuitZei = result.DailyRevenueItems.Sum(i => i.GaiSyaRyoSyo).AddCommas(),
                                    GuiTes = result.DailyRevenueItems.Sum(i => i.GaiSyaRyoTes).AddCommas(),
                                    GuiG = result.DailyRevenueItems.Sum(i => i.GaiSyaRyoSum).AddCommas(),
                                    OtherFutUri = s1.SFutUriGakKin.AddCommas(),
                                    OtherFuttZei = s1.SFutSyaRyoSyo.AddCommas(),
                                    OtherFutTes = s1.SFutSyaRyoTes.AddCommas(),
                                    OtherFutG = (s1.SFutUriGakKin + s1.SFutSyaRyoSyo).AddCommas(),
                                    CanKin = result.DailyRevenueItems.Sum(i => i.CanUnc).AddCommas(),
                                    CanZei =  result.DailyRevenueItems.Sum(i => i.CanSyoG).AddCommas(),
                                    CanG = result.DailyRevenueItems.Sum(i => i.CanSum).AddCommas(),
                                    YouNm = string.Empty,
                                    YouDai = s1Detail.YouSyaSyuDai.AddCommas(),
                                    YouUnt = s1Detail.YouSyaRyoUnc.AddCommas(),
                                    YouZei = s1Detail.YouSyaRyoSyo.AddCommas(),
                                    YouTes = s1Detail.YouSyaRyoTes.AddCommas(),
                                    YouG = s1Detail.YouG.AddCommas(),
                                    YouFutHas = s1Detail.YfuUriGakKin.AddCommas(),
                                    YouFutZei = s1Detail.YfuSyaRyoSyo.AddCommas(),
                                    YouFutTes = s1Detail.YfuSyaRyoTes.AddCommas(),
                                    YouFutG = s1Detail.YouFutG.AddCommas(),
                                    DailySoneki = s1.SSoneki.AddCommas(),
                                },
                                new DailyRevenueSummary()
                                {
                                    Name = _lang["Accumulation"],
                                    ZeiKomi = s2.SJyuSyaRyoRui.AddCommas(),
                                    JisSyaSyuDai = s2.JisSyaSyuDai.AddCommas(),
                                    JisSyaRyoUnc = s2.SJisSyaRyoUnc.AddCommas(),
                                    JisSyaRyoSyo = s2.SJisSyaRyoSyo.AddCommas(),
                                    JisSyaRyoTes = s2.SJisSyaRyoTes.AddCommas(),
                                    JisG = (s2.SJisSyaRyoUnc + s2.SJisSyaRyoSyo).AddCommas(),
                                    GuiUri = s2.GaiUriGakKin.AddCommas(),
                                    GuitZei = s2.GaiSyaRyoSyo.AddCommas(),
                                    GuiTes = s2.GaiSyaRyoTes.AddCommas(),
                                    GuiG = s2.GaiSyaRyoSum.AddCommas(),
                                    OtherFutUri = s2.SFutUriGakKin.AddCommas(),
                                    OtherFuttZei = s2.SFutSyaRyoSyo.AddCommas(),
                                    OtherFutTes = s2.SFutSyaRyoTes.AddCommas(),
                                    OtherFutG = (s2.SFutUriGakKin + s2.SFutSyaRyoSyo).AddCommas(),
                                    CanKin = s2.CanUnc.AddCommas(),
                                    CanZei =  s2.CanSyoG.AddCommas(),
                                    CanG = s2.CanSum.AddCommas(),
                                    YouNm = string.Empty,
                                    YouDai = s2Detail.YouSyaSyuDai.AddCommas(),
                                    YouUnt = s2Detail.YouSyaRyoUnc.AddCommas(),
                                    YouZei = s2Detail.YouSyaRyoSyo.AddCommas(),
                                    YouTes = s2Detail.YouSyaRyoTes.AddCommas(),
                                    YouG = s2Detail.YouG.AddCommas(),
                                    YouFutHas = s2Detail.YfuUriGakKin.AddCommas(),
                                    YouFutZei = s2Detail.YfuSyaRyoSyo.AddCommas(),
                                    YouFutTes = s2Detail.YfuSyaRyoTes.AddCommas(),
                                    YouFutG = s2Detail.YouFutG.AddCommas(),
                                    DailySoneki = s2.SSoneki.AddCommas(),
                                }
                            };

                for (var i = 0; i < dataList.Count; i++)
                {
                    var data = new DailyRevenueReportData();
                    data.CommonData = commonData;
                    data.DailyRevenueItems = dataList[i];

                    if (i == dataList.Count - 1)
                    {
                        if (s1 != null || s2 != null)
                            data.Summaries = summaries;
                    }
                    else
                    {
                        data.Summaries = new List<DailyRevenueSummary>()
                        {
                            new DailyRevenueSummary()
                            {
                                Name = _lang["TotalByDay"]
                            },
                            new DailyRevenueSummary()
                            {
                                 Name = _lang["Accumulation"]
                            }
                        };
                    }
                    reportDataSource.Add(data);
                }
            }
            return reportDataSource;
        }

        private int GetPercent(int currentIndex, int total)
        {
            return currentIndex * 100 / total;
        }

        public async Task<List<MonthlyRevenueReportData>> GetMonthlyRevenueReportData(TransportationRevenueSearchModel model)
        {
            var now = DateTime.Now;
            var pageSize = 24;
            var reportDataSource = new List<MonthlyRevenueReportData>();
            var searchModels = await GetSearchModelsForMonthlyReport(model);
            var key = model.ReportId; var index = 0;
            foreach (var searchModel in searchModels)
            {
                index++;
                if (_reportLoadingService.IsCanceled(key)) break;
                else await _reportLoadingService.UpdateProgress(GetPercent(index, searchModels.Count), key);

                var result = await GetMonthlyRevenueData(searchModel);
                if (!result.MonthlyRevenueItems.Any()) continue;
                var s2 = result.SummaryResult.FirstOrDefault(i => i.MesaiKbn == 2);
                var s2Detail = result.DetailItems.FirstOrDefault(i => i.MesaiKbn == 2);

                var commonData = new MonthlyRevenueCommonData()
                {
                    UriYm = $": {searchModel.RevenueSearchModel.UriYmdFrom.Substring(0, model.UriYmdFrom.Length - 2).AddSlash2YYYYMM()}",
                    EigyoKbnNm = searchModel.RevenueSearchModel.EigyoKbn == EigyoKbnEnum.ReceptionOffice ? _lang["ReceptionOfficeLabel"] : _lang["BillingOfficeLabel"],
                    EigyoKbn = $": {searchModel.Eigyo?.EigyoCd:00000} {searchModel.Eigyo?.RyakuNm}",
                    TesukomiKbnNm = $": {searchModel.TesukomiKbn}",
                    Eigyo = $": {searchModel.Eigyo.EigyoCd:00000} {searchModel.Eigyo.RyakuNm}",
                    Syain = $"{new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd} {new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name}",
                    ProcessingDate = now.ToString(DateTimeFormat.yyyyMMddSlashHHmmColon)
                };

                var reportItems = ConvertToReportItems(result.MonthlyRevenueItems.ToList());
                var dataList = SubList(reportItems, pageSize);
                for (var i = 0; i < dataList.Count; i++)
                {
                    var data = new MonthlyRevenueReportData();
                    var subList = dataList[i];
                    data.CommonData = commonData;
                    data.MonthlyRevenueItems = subList;

                    data.Summaries = new List<MonthlyRevenueSummary>()
                    {
                        new MonthlyRevenueSummary()
                        {
                            Name = _lang["TotalPage"],
                            ZeiKomi = subList.Sum(i => ParseLong(i.KeiKin)).AddCommas(),
                            JisSyaSyuDai = subList.Sum(i => ParseInt(i.JisSyaSyuDai)).AddCommas(),
                            JisSyaRyoUnc = subList.Sum(i => ParseLong(i.JisSyaRyoUnc)).AddCommas(),
                            JisSyaRyoSyo = subList.Sum(i => ParseLong(i.JisSyaRyoSyo)).AddCommas(),
                            JisSyaRyoTes = subList.Sum(i => ParseLong(i.JisSyaRyoTes)).AddCommas(),
                            JisG = subList.Sum(i => ParseLong(i.JisSyaRyoSum)).AddCommas(),
                            GuiUri = subList.Sum(i => ParseInt(i.GaiUriGakKin)).AddCommas(),
                            GuitZei = subList.Sum(i => ParseInt(i.GaiSyaRyoSyo)).AddCommas(),
                            GuiTes = subList.Sum(i => ParseInt(i.GaiSyaRyoTes)).AddCommas(),
                            GuiG = subList.Sum(i => ParseInt(i.GaiSyaRyoSum)).AddCommas(),
                            OtherFutUri = subList.Sum(i => ParseInt(i.EtcUriGakKin)).AddCommas(),
                            OtherFuttZei = subList.Sum(i => ParseInt(i.EtcSyaRyoSyo)).AddCommas(),
                            OtherFutTes = subList.Sum(i => ParseInt(i.EtcSyaRyoTes)).AddCommas(),
                            OtherFutG = subList.Sum(i => ParseInt(i.EtcSyaRyoSum)).AddCommas(),
                            CanKin = subList.Sum(i => ParseInt(i.CanUnc)).AddCommas(),
                            CanZei = subList.Sum(i => ParseInt(i.CanSyoG)).AddCommas(),
                            CanG = subList.Sum(i => ParseInt(i.CanSum)).AddCommas(),
                            YouNm = string.Empty,
                            YouDai = subList.Sum(i => ParseInt(i.YouSyaSyuDai)).AddCommas(),
                            YouUnt = subList.Sum(i => ParseInt(i.YouSyaRyoUnc)).AddCommas(),
                            YouZei = subList.Sum(i => ParseInt(i.YouSyaRyoSyo)).AddCommas(),
                            YouTes = subList.Sum(i => ParseInt(i.YouSyaRyoTes)).AddCommas(),
                            YouG = subList.Sum(i => ParseInt(i.YouG)).AddCommas(),
                            YouFutHas = subList.Sum(i => ParseInt(i.YfuUriGakKin)).AddCommas(),
                            YouFutZei = subList.Sum(i => ParseInt(i.YfuSyaRyoSyo)).AddCommas(),
                            YouFutTes = subList.Sum(i => ParseInt(i.YfuSyaRyoTes)).AddCommas(),
                            YouFutG = subList.Sum(i => ParseInt(i.YouFutG)).AddCommas(),
                            DailySoneki = subList.Sum(i => ParseInt(i.UntSoneki)).AddCommas()
                        }

                    };

                    if (i == dataList.Count - 1)
                    {
                        data.Summaries.Add(new MonthlyRevenueSummary()
                        {
                            Name = _lang["TotalByMonth"],
                            ZeiKomi = s2.SJyuSyaRyoRui.AddCommas(),
                            JisSyaSyuDai = s2.JisSyaSyuDai.AddCommas(),
                            JisSyaRyoUnc = s2.SJisSyaRyoUnc.AddCommas(),
                            JisSyaRyoSyo = s2.SJisSyaRyoSyo.AddCommas(),
                            JisSyaRyoTes = s2.SJisSyaRyoTes.AddCommas(),
                            JisG = (s2.SJisSyaRyoUnc + s2.SJisSyaRyoSyo).AddCommas(),
                            GuiUri = s2.GaiUriGakKin.AddCommas(),
                            GuitZei = s2.GaiSyaRyoSyo.AddCommas(),
                            GuiTes = s2.GaiSyaRyoTes.AddCommas(),
                            GuiG = s2.GaiSyaRyoSum.AddCommas(),
                            OtherFutUri = s2.EtcUriGakKin.AddCommas(),
                            OtherFuttZei = s2.EtcSyaRyoSyo.AddCommas(),
                            OtherFutTes = s2.EtcSyaRyoTes.AddCommas(),
                            OtherFutG = s2.EtcSyaRyoSum.AddCommas(),
                            CanKin = s2.CanUnc.AddCommas(),
                            CanZei = s2.CanSyoG.AddCommas(),
                            CanG = s2.CanSum.AddCommas(),
                            YouNm = string.Empty,
                            YouDai = s2Detail.YouSyaSyuDai.AddCommas(),
                            YouUnt = s2Detail.YouSyaRyoUnc.AddCommas(),
                            YouZei = s2Detail.YouSyaRyoSyo.AddCommas(),
                            YouTes = s2Detail.YouSyaRyoTes.AddCommas(),
                            YouG = s2Detail.YouG.AddCommas(),
                            YouFutHas = s2Detail.YfuUriGakKin.AddCommas(),
                            YouFutZei = s2Detail.YfuSyaRyoSyo.AddCommas(),
                            YouFutTes = s2Detail.YfuSyaRyoTes.AddCommas(),
                            YouFutG = s2Detail.YouFutG.AddCommas(),
                            DailySoneki = s2.SSoneki.AddCommas(),
                        });
                    }
                    else
                    {
                        data.Summaries.Add(new MonthlyRevenueSummary()
                        {
                            Name = _lang["TotalByMonth"],
                        });
                    }

                    reportDataSource.Add(data);
                }
            }

            return reportDataSource;
        }
        private long ParseLong(string value)
        {
            return string.IsNullOrEmpty(value) ? 0 : long.Parse(value.Replace(",", ""));
        }

        private int ParseInt(string value)
        {
            return string.IsNullOrEmpty(value) ? 0 : int.Parse(value.Replace(",", ""));
        }
        public async Task<StringBuilder> GetMonthlyRevenueCSVReportData(TransportationRevenueSearchModel model)
        {
            var kinou = await GetKinouFromCustomKi();
            var jippiFlg = kinou.JippiFlg;
            var futaiMeiFlg = kinou.FutaiMeiFlg;
            var searchModels = await GetSearchModelsForMonthlyReport(model);
            var csvData = new List<MonthlyRevenueCSVReportData>();
            var index = 0;
            var searchModelCount = 0;
            var key = model.ReportId;
            foreach (var searchModel in searchModels)
            {
                searchModelCount++;
                if (_reportLoadingService.IsCanceled(key)) break;
                else await _reportLoadingService.UpdateProgress(GetPercent(searchModelCount, searchModels.Count), key);

                var result = await GetMonthlyRevenueData(searchModel);
                if (!result.MonthlyRevenueItems.Any()) continue;
                result.MonthlyRevenueItems = result.MonthlyRevenueItems.Where(i => i.MesaiKbn == 3).ToList();
                result.DetailItems = result.DetailItems.Where(i => i.MesaiKbn == 3);

                foreach (var item in result.MonthlyRevenueItems)
                {
                    var temp1 = new MonthlyRevenueCSVReportData();
                    temp1.No = (++index).ToString();
                    temp1.UriYmd = item.UriYmd;
                    temp1.EigyoCd = item.EigyoCd.ToString();
                    temp1.EigyoNm = item.EigyoNm;
                    temp1.EigyoRyak = item.EigyoRyak;
                    temp1.KeiKin = item.KeiKin;

                    temp1.JisSyaRyoUnc = item.JisSyaRyoUnc;
                    temp1.JisSyaSyuDai = item.JisSyaSyuDai;
                    temp1.JisSyaRyoSyo = item.JisSyaRyoSyo;
                    temp1.JisSyaRyoTes = item.JisSyaRyoTes;
                    temp1.JisSyaRyoSum = item.JisSyaRyoSum;
                    temp1.GaiUriGakKin = item.GaiUriGakKin;
                    temp1.GaiSyaRyoSyo = item.GaiSyaRyoSyo;
                    temp1.GaiSyaRyoTes = item.GaiSyaRyoTes;
                    temp1.GaiSyaRyoSum = item.GaiSyaRyoSum;

                    temp1.HighwayUriGakKin = futaiMeiFlg == 1 ? item.HighwayUriGakKin : 0;
                    temp1.HighwaySyaRyoSyo = futaiMeiFlg == 1 ? item.HighwaySyaRyoSyo : 0;
                    temp1.HighwaySyaRyoTes = futaiMeiFlg == 1 ? item.HighwaySyaRyoTes : 0;
                    temp1.HighwaySyaRyoSum = futaiMeiFlg == 1 ? item.HighwaySyaRyoSum : 0;
                    temp1.HotelUriGakKin = futaiMeiFlg == 1 ? item.HotelUriGakKin : 0;
                    temp1.HotelSyaRyoSyo = futaiMeiFlg == 1 ? item.HotelSyaRyoSyo : 0;
                    temp1.HotelSyaRyoTes = futaiMeiFlg == 1 ? item.HotelSyaRyoTes : 0;
                    temp1.HotelSyaRyoSum = futaiMeiFlg == 1 ? item.HotelSyaRyoSum : 0;
                    temp1.ParkingUriGakKin = futaiMeiFlg == 1 ? item.ParkingUriGakKin : 0;
                    temp1.ParkingSyaRyoSyo = futaiMeiFlg == 1 ? item.ParkingSyaRyoSyo : 0;
                    temp1.ParkingSyaRyoTes = futaiMeiFlg == 1 ? item.ParkingSyaRyoTes : 0;
                    temp1.ParkingSyaRyoSum = futaiMeiFlg == 1 ? item.ParkingSyaRyoSum : 0;

                    temp1.UriGakKin = futaiMeiFlg == 1 ? item.OtherUriGakKin : item.EtcUriGakKin;
                    temp1.SyaRyoSyo = futaiMeiFlg == 1 ? item.OtherSyaRyoSyo : item.EtcSyaRyoSyo;
                    temp1.SyaRyoTes = futaiMeiFlg == 1 ? item.OtherSyaRyoTes : item.EtcSyaRyoTes;
                    temp1.SyaRyoSum = futaiMeiFlg == 1 ? item.OtherSyaRyoSum : item.EtcSyaRyoSum;

                    temp1.CanUnc = item.CanUnc;
                    temp1.CanSyoG = item.CanSyoG;
                    temp1.CanSum = item.CanSum;
                    temp1.UntSoneki = item.UntSoneki;
                    var detailItems = result.DetailItems.Where(i => i.EigyoCd == item.EigyoCd && i.UriYmd == item.UriYmd);
                    csvData.Add(temp1);
                    foreach (var detailItem in detailItems)
                    {
                        var temp = new MonthlyRevenueCSVReportData();
                        temp.No = (++index).ToString();
                        temp.YouRyakuNm = detailItem.YouRyakuNm;
                        temp.YouSitRyakuNm = detailItem.YouSitRyakuNm;
                        temp.YouGyosyaCd = detailItem.YouGyosyaCd.ToString();
                        temp.YouCd = detailItem.YouCd.ToString();
                        temp.YouSitCd = detailItem.YouSitCd.ToString();
                        temp.YouGyosyaNm = detailItem.YouGyosyaNm;
                        temp.YouNm = detailItem.YouNm;
                        temp.YouSitenNm = detailItem.YouSitenNm;
                        temp.YouSyaRyoUnc = detailItem.YouSyaRyoUnc.AddCommas();
                        temp.YouSyaSyuDai = detailItem.YouSyaSyuDai.AddCommas();
                        temp.YouSyaRyoSyo = detailItem.YouSyaRyoSyo.AddCommas();
                        temp.YouSyaRyoTes = detailItem.YouSyaRyoTes.AddCommas();
                        temp.YouG = detailItem.YouG.AddCommas();
                        temp.YfuUriGakKin = detailItem.YfuUriGakKin.AddCommas();
                        temp.YfuSyaRyoSyo = detailItem.YfuSyaRyoSyo.AddCommas();
                        temp.YfuSyaRyoTes = detailItem.YfuSyaRyoTes.AddCommas();
                        temp.YouFutG = detailItem.YouFutG.AddCommas();
                        csvData.Add(temp);
                    }
                }
            }

            var columns = BuildMonthlyDisplayColumns(jippiFlg, futaiMeiFlg);
            var csv = BuildCsvFile(csvData, columns, model.KugiriCharType, model.KukuriKbn, model.OutputWithHeader);
            return csv;
        }

        public async Task<List<MonthlyRevenueSearchModel>> GetSearchModelsForMonthlyReport(TransportationRevenueSearchModel model)
        {
            var eigyoListItems = await GetEigyoListForMonthlyRevenueReport(model);
            var searchModels = new List<MonthlyRevenueSearchModel>();
            var tesukomiKbn = GetTesuInKbnItems().Find(o => o.Value == (int)model.TesuInKbn)?.Text;
            if (eigyoListItems.Any())
            {
                foreach (var eigyo in eigyoListItems)
                {
                    var searchModel = new MonthlyRevenueSearchModel();
                    searchModel.RevenueSearchModel = model;
                    searchModel.Eigyo = eigyo;
                    searchModel.TesukomiKbn = tesukomiKbn;
                    searchModels.Add(searchModel);
                }
            }
            return searchModels;
        }

        private async Task<Kinou> GetKinouFromCustomKi()
        {
            var kinou = await _mediator.Send(new GetKinouFromCustomKi()
            {
                SearchModel = new CustomKiSearchModel()
                {
                    KinouId = KinouId,
                    SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq
                }
            });

            if (kinou == null)
            {
                kinou = await _mediator.Send(new GetKinouFromCustomKi()
                {
                    SearchModel = new CustomKiSearchModel()
                    {
                        KinouId = KinouId,
                        SyainCdSeq = 0
                    }
                });
            }

            var jippiFlg = 0;
            var futaiMeiFlg = 0;
            if (int.TryParse(kinou.Kinou02, out var i) && int.TryParse(kinou.Kinou01, out var j))
            {
                jippiFlg = i;
                futaiMeiFlg = j;
            }
            return new Kinou()
            {
                FutaiMeiFlg = futaiMeiFlg,
                JippiFlg = jippiFlg
            };
        }

        public async Task<StringBuilder> GetDailyRevenueCSVReportData(TransportationRevenueSearchModel model)
        {
            var kinou = await GetKinouFromCustomKi();
            var jippiFlg = kinou.JippiFlg;
            var futaiMeiFlg = kinou.FutaiMeiFlg;
            var searchModels = new List<DailyRevenueSearchModel>();
            var eigyoListItems = await GetEigyoListForDailyRevenueReport(model);

            var tesuInKbn = GetTesuInKbnItems().Find(o => o.Value == (int)model.TesuInKbn);
            var key = model.ReportId;
            if (eigyoListItems.Any())
            {
                foreach (var eigyo in eigyoListItems)
                {
                    var dailyModel = new DailyRevenueSearchModel();
                    dailyModel.Eigyo = eigyo;
                    dailyModel.RevenueSearchModel = model;
                    var uriYmdList = await GetUriYmdForDailyRevenueReport(dailyModel);
                    foreach (var uriYmd in uriYmdList)
                    {
                        var searchModel = new DailyRevenueSearchModel();
                        searchModel.RevenueSearchModel = model;
                        searchModel.UriYmd = uriYmd;
                        searchModel.Eigyo = eigyo;
                        searchModels.Add(searchModel);
                    }
                }
            }
            var csvData = new List<DailyRevenueCSVReportData>();
            var index = 0;
            var searchModelCount = 0;
            foreach (var searchModel in searchModels)
            {
                searchModelCount++;
                if (_reportLoadingService.IsCanceled(key)) break;
                else await _reportLoadingService.UpdateProgress(GetPercent(searchModelCount, searchModels.Count), key);

                var result = await GetDailyRevenueData(searchModel);
                if (!result.DailyRevenueItems.Any()) continue;
                result.DailyRevenueItems = result.DailyRevenueItems.Where(i => i.MesaiKbn == 3).ToList();
                result.DetailItems = result.DetailItems.Where(i => i.MesaiKbn == 3);

                foreach (var item in result.DailyRevenueItems)
                {
                    var temp1 = new DailyRevenueCSVReportData();
                    temp1.No = (++index).ToString();
                    temp1.UriYmd = searchModel.UriYmd.Replace("/", "");
                    temp1.EigyoCd = searchModel.RevenueSearchModel.EigyoKbn == EigyoKbnEnum.BillingOffice ? item.SeiEigyoCd : item.UkeEigyoCd;
                    temp1.EigyoNm = searchModel.RevenueSearchModel.EigyoKbn == EigyoKbnEnum.BillingOffice ? item.SeiEigyoNm : item.UkeEigyoNm;
                    temp1.RyakuNm = searchModel.RevenueSearchModel.EigyoKbn == EigyoKbnEnum.BillingOffice ? item.SeiEigyoRyak : item.UkeRyakuNm;
                    temp1.TesuInKbnCd = tesuInKbn.Value.ToString();
                    temp1.TesuInKbnNm = tesuInKbn.Text;
                    temp1.UkeNo = item.UkeNo;
                    temp1.UkeEigyoCd = item.UkeEigyoCd;
                    temp1.UkeEigyoNm = item.UkeEigyoNm;
                    temp1.UkeRyakuNm = item.UkeRyakuNm;
                    temp1.GyosyaCd = item.GyosyaCd;
                    temp1.TokuiCd = item.TokuiCd;
                    temp1.SitenCd = item.SitenCd;
                    temp1.GyosyaNm = item.GyosyaNm;
                    temp1.TokuiNm = item.TokuiNm;
                    temp1.SitenNm = item.SitenNm;
                    temp1.TokRyakuNm = item.TokRyakuNm;
                    temp1.SitRyakuNm = item.SitRyakuNm;
                    temp1.SirGyosyaCd = item.SirGyosyaCd;
                    temp1.SirCd = item.SirCd;
                    temp1.SirSitenCd = item.SirSitenCd;
                    temp1.SirGyosyaNm = item.SirGyosyaNm;
                    temp1.SirNm = item.SirNm;
                    temp1.SirSitenNm = item.SirSitenNm;
                    temp1.SirRyakuNm = item.SirRyakuNm;
                    temp1.SirSitRyakuNm = item.SirSitRyakuNm;
                    temp1.DanTaNm = item.DanTaNm;
                    temp1.IkNm = item.IkNm;
                    temp1.Nissu = item.Nissu;
                    temp1.KeiKin = item.KeiKin;

                    temp1.JisSyaRyoUnc = item.JisSyaRyoUnc;
                    temp1.JisSyaSyuDai = item.JisSyaSyuDai;
                    temp1.JisZeiKbn = item.JisZeiKbn;
                    temp1.JisZeiKbnNm = item.JisZeiKbnNm;
                    temp1.JisSyaRyoSyoRit = item.JisSyaRyoSyoRit;
                    temp1.JisSyaRyoSyo = item.JisSyaRyoSyo;
                    temp1.JisSyaRyoTesRit = item.JisSyaRyoTesRit;
                    temp1.JisSyaRyoTes = item.JisSyaRyoTes;
                    temp1.JisSyaRyoSum = item.JisSyaRyoSum;
                    temp1.GaiUriGakKin = item.GaiUriGakKin;
                    temp1.GaiSyaRyoSyo = item.GaiSyaRyoSyo;
                    temp1.GaiSyaRyoTes = item.GaiSyaRyoTes;
                    temp1.GaiSyaRyoSum = item.GaiSyaRyoSum;

                    temp1.HighwayUriGakKin = futaiMeiFlg == 1 ? item.HighwayUriGakKin : 0;
                    temp1.HighwaySyaRyoSyo = futaiMeiFlg == 1 ? item.HighwaySyaRyoSyo : 0;
                    temp1.HighwaySyaRyoTes = futaiMeiFlg == 1 ? item.HighwaySyaRyoTes : 0;
                    temp1.HighwaySyaRyoSum = futaiMeiFlg == 1 ? item.HighwaySyaRyoSum : 0;
                    temp1.HotelUriGakKin = futaiMeiFlg == 1 ? item.HotelUriGakKin : 0;
                    temp1.HotelSyaRyoSyo = futaiMeiFlg == 1 ? item.HotelSyaRyoSyo : 0;
                    temp1.HotelSyaRyoTes = futaiMeiFlg == 1 ? item.HotelSyaRyoTes : 0;
                    temp1.HotelSyaRyoSum = futaiMeiFlg == 1 ? item.HotelSyaRyoSum : 0;
                    temp1.ParkingUriGakKin = futaiMeiFlg == 1 ? item.ParkingUriGakKin : 0;
                    temp1.ParkingSyaRyoSyo = futaiMeiFlg == 1 ? item.ParkingSyaRyoSyo : 0;
                    temp1.ParkingSyaRyoTes = futaiMeiFlg == 1 ? item.ParkingSyaRyoTes : 0;
                    temp1.ParkingSyaRyoSum = futaiMeiFlg == 1 ? item.ParkingSyaRyoSum : 0;
                    temp1.UriGakKin = futaiMeiFlg == 1 ? item.OtherUriGakKin : item.EtcUriGakKin;
                    temp1.SyaRyoSyo = futaiMeiFlg == 1 ? item.OtherSyaRyoSyo : item.EtcSyaRyoSyo;
                    temp1.SyaRyoTes = futaiMeiFlg == 1 ? item.OtherSyaRyoTes : item.EtcSyaRyoTes;
                    temp1.SyaRyoSum = futaiMeiFlg == 1 ? item.OtherSyaRyoSum : item.EtcSyaRyoSum;
                    temp1.CanUnc = item.CanUnc;
                    temp1.CanZKbn = item.CanZKbn;
                    temp1.CanZKbnNm = item.CanZKbnNm;
                    temp1.CanSyoR = item.CanSyoR;
                    temp1.CanSyoG = item.CanSyoG;
                    temp1.CanSum = item.CanSum;
                    temp1.UntSoneki = item.UntSoneki;
                    csvData.Add(temp1);

                    var detailItems = result.DetailItems.Where(i => i.UkeNo == item.UkeNo && i.UnkRen == item.UnkRen);
                    foreach (var detailItem in detailItems)
                    {
                        var temp = new DailyRevenueCSVReportData();
                        temp.No = (++index).ToString();
                        temp.YouGyosyaCd = detailItem.YouGyosyaCd.ToString();
                        temp.YouRyakuNm = detailItem.YouRyakuNm;
                        temp.YouSitRyakuNm = detailItem.YouSitRyakuNm;
                        temp.YouSyaRyoUnc = detailItem.YouSyaRyoUnc;
                        temp.YouSyaSyuDai = detailItem.YouSyaSyuDai;
                        temp.YouZeiritsu = detailItem.YouZeiritsu;
                        temp.YouSyaRyoSyo = detailItem.YouSyaRyoSyo;
                        temp.YouTesuRitu = detailItem.YouTesuRitu;
                        temp.YouSyaRyoTes = detailItem.YouSyaRyoTes;
                        temp.YouG = detailItem.YouG.ToString();
                        temp.YfuUriGakKin = detailItem.YfuUriGakKin;
                        temp.YfuSyaRyoSyo = detailItem.YfuSyaRyoSyo;
                        temp.YfuSyaRyoTes = detailItem.YfuSyaRyoTes;
                        temp.YouFutG = detailItem.YouFutG;
                        temp.YouCd = detailItem.YouCd;
                        temp.YouSitCd = detailItem.YouSitCd;
                        temp.YouGyosyaNm = detailItem.YouGyosyaNm;
                        temp.YouNm = detailItem.YouNm;
                        temp.YouSitenNm = detailItem.YouSitenNm;
                        temp.YouZeiKbn = detailItem.YouZeiKbn;
                        temp.YouZKbnNm = detailItem.YouZKbnNm;
                        csvData.Add(temp);
                    }
                }
            }

            var columns = BuildDailyDisplayColumns(jippiFlg, futaiMeiFlg);
            var csv = BuildCsvFile(csvData, columns, model.KugiriCharType, model.KukuriKbn, model.OutputWithHeader);
            return csv;
        }

        private List<DisplayColumn> BuildDailyDisplayColumns(int jippiFlg, int futaiMeiFlg)
        {
            var columns = new List<DisplayColumn>()
            {
                new DisplayColumn(){ColumnName = "№", PropName = nameof(DailyRevenueCSVReportData.No)},
                new DisplayColumn(){ColumnName = "売上年月日", PropName = nameof(DailyRevenueCSVReportData.UriYmd)},
                new DisplayColumn(){ColumnName = "営業所コード", PropName = nameof(DailyRevenueCSVReportData.EigyoCd)},
                new DisplayColumn(){ColumnName = "営業所名", PropName = nameof(DailyRevenueCSVReportData.EigyoNm)},
                new DisplayColumn(){ColumnName = "営業所略名", PropName = nameof(DailyRevenueCSVReportData.RyakuNm)},
                new DisplayColumn(){ColumnName = "手数料込み区分", PropName = nameof(DailyRevenueCSVReportData.TesuInKbnCd)},
                new DisplayColumn(){ColumnName = "手数料込み区分名", PropName = nameof(DailyRevenueCSVReportData.TesuInKbnNm)},
                new DisplayColumn(){ColumnName = "受付番号", PropName = nameof(DailyRevenueCSVReportData.UkeNo)},
                new DisplayColumn(){ColumnName = "受付営業所コード", PropName = nameof(DailyRevenueCSVReportData.UkeEigyoCd)},
                new DisplayColumn(){ColumnName = "受付営業所名", PropName = nameof(DailyRevenueCSVReportData.UkeEigyoNm)},
                new DisplayColumn(){ColumnName = "受付営業所略名", PropName = nameof(DailyRevenueCSVReportData.UkeRyakuNm)},
                new DisplayColumn(){ColumnName = "得意先業者コード", PropName = nameof(DailyRevenueCSVReportData.GyosyaCd)},
                new DisplayColumn(){ColumnName = "得意先コード", PropName = nameof(DailyRevenueCSVReportData.TokuiCd)},
                new DisplayColumn(){ColumnName = "得意先支店コード", PropName = nameof(DailyRevenueCSVReportData.SitenCd)},
                new DisplayColumn(){ColumnName = "得意先業者コード名", PropName = nameof(DailyRevenueCSVReportData.GyosyaNm)},
                new DisplayColumn(){ColumnName = "得意先名", PropName = nameof(DailyRevenueCSVReportData.TokuiNm)},
                new DisplayColumn(){ColumnName = "得意先支店名", PropName = nameof(DailyRevenueCSVReportData.SitenNm)},
                new DisplayColumn(){ColumnName = "得意先略名", PropName = nameof(DailyRevenueCSVReportData.TokRyakuNm)},
                new DisplayColumn(){ColumnName = "得意先支店略名", PropName = nameof(DailyRevenueCSVReportData.SitRyakuNm)},
                new DisplayColumn(){ColumnName = "仕入先業者コード", PropName = nameof(DailyRevenueCSVReportData.SirGyosyaCd)},
                new DisplayColumn(){ColumnName = "仕入先コード", PropName = nameof(DailyRevenueCSVReportData.SirCd)},
                new DisplayColumn(){ColumnName = "仕入先支店コード", PropName = nameof(DailyRevenueCSVReportData.SirSitenCd)},
                new DisplayColumn(){ColumnName = "仕入先業者コード名", PropName = nameof(DailyRevenueCSVReportData.SirGyosyaNm)},
                new DisplayColumn(){ColumnName = "仕入先名", PropName = nameof(DailyRevenueCSVReportData.SirNm)},
                new DisplayColumn(){ColumnName = "仕入先支店名", PropName = nameof(DailyRevenueCSVReportData.SirSitenNm)},
                new DisplayColumn(){ColumnName = "仕入先略名", PropName = nameof(DailyRevenueCSVReportData.SirRyakuNm)},
                new DisplayColumn(){ColumnName = "仕入先支店略名", PropName = nameof(DailyRevenueCSVReportData.SirSitRyakuNm)},
                new DisplayColumn(){ColumnName = "団体名", PropName = nameof(DailyRevenueCSVReportData.DanTaNm)},
                new DisplayColumn(){ColumnName = "行き先名", PropName = nameof(DailyRevenueCSVReportData.IkNm)},
                new DisplayColumn(){ColumnName = "日数", PropName = nameof(DailyRevenueCSVReportData.Nissu)},
                new DisplayColumn(){ColumnName = "税込運賃", PropName = nameof(DailyRevenueCSVReportData.KeiKin)},
                new DisplayColumn(){ColumnName = "傭車先業者コード", PropName = nameof(DailyRevenueCSVReportData.YouGyosyaCd)},
                new DisplayColumn(){ColumnName = "傭車先コード", PropName = nameof(DailyRevenueCSVReportData.YouCd)},
                new DisplayColumn(){ColumnName = "傭車先支店コード", PropName = nameof(DailyRevenueCSVReportData.YouSitCd)},
                new DisplayColumn(){ColumnName = "傭車先業者コード名", PropName = nameof(DailyRevenueCSVReportData.YouGyosyaNm)},
                new DisplayColumn(){ColumnName = "傭車先名", PropName = nameof(DailyRevenueCSVReportData.YouNm)},
                new DisplayColumn(){ColumnName = "傭車先支店名", PropName = nameof(DailyRevenueCSVReportData.YouSitenNm)},
                new DisplayColumn(){ColumnName = "傭車先略名", PropName = nameof(DailyRevenueCSVReportData.YouRyakuNm)},
                new DisplayColumn(){ColumnName = "傭車先支店略名", PropName = nameof(DailyRevenueCSVReportData.YouSitRyakuNm)},
                new DisplayColumn(){ColumnName = "傭車項目・運賃", PropName = nameof(DailyRevenueCSVReportData.YouSyaRyoUnc)},
                new DisplayColumn(){ColumnName = "傭車項目・台数", PropName = nameof(DailyRevenueCSVReportData.YouSyaSyuDai)},
                new DisplayColumn(){ColumnName = "傭車項目・税区分", PropName = nameof(DailyRevenueCSVReportData.YouZeiKbn)},
                new DisplayColumn(){ColumnName = "傭車項目・税区分名", PropName = nameof(DailyRevenueCSVReportData.YouZKbnNm)},
                new DisplayColumn(){ColumnName = "傭車項目・消費税率", PropName = nameof(DailyRevenueCSVReportData.YouZeiritsu)},
                new DisplayColumn(){ColumnName = "傭車項目・消費税額", PropName = nameof(DailyRevenueCSVReportData.YouSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "傭車項目・手数料率", PropName = nameof(DailyRevenueCSVReportData.YouTesuRitu)},
                new DisplayColumn(){ColumnName = "傭車項目・手数料額", PropName = nameof(DailyRevenueCSVReportData.YouSyaRyoTes)},
                new DisplayColumn(){ColumnName = "傭車項目・合計", PropName = nameof(DailyRevenueCSVReportData.YouG)},
                new DisplayColumn(){ColumnName = "傭車付帯・発生額", PropName = nameof(DailyRevenueCSVReportData.YfuUriGakKin)},
                new DisplayColumn(){ColumnName = "傭車付帯・消費税額", PropName = nameof(DailyRevenueCSVReportData.YfuSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "傭車付帯・手数料額", PropName = nameof(DailyRevenueCSVReportData.YfuSyaRyoTes)},
                new DisplayColumn(){ColumnName = "傭車付帯・合計", PropName = nameof(DailyRevenueCSVReportData.YouFutG)},
                new DisplayColumn(){ColumnName = "自社項目・運賃", PropName = nameof(DailyRevenueCSVReportData.JisSyaRyoUnc)},
                new DisplayColumn(){ColumnName = "自社項目・台数", PropName = nameof(DailyRevenueCSVReportData.JisSyaSyuDai)},
                new DisplayColumn(){ColumnName = "自社項目・税区分", PropName = nameof(DailyRevenueCSVReportData.JisZeiKbn)},
                new DisplayColumn(){ColumnName = "自社項目・税区分名", PropName = nameof(DailyRevenueCSVReportData.JisZeiKbnNm)},
                new DisplayColumn(){ColumnName = "自社項目・消費税率", PropName = nameof(DailyRevenueCSVReportData.JisSyaRyoSyoRit)},
                new DisplayColumn(){ColumnName = "自社項目・消費税額", PropName = nameof(DailyRevenueCSVReportData.JisSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "自社項目・手数料率", PropName = nameof(DailyRevenueCSVReportData.JisSyaRyoTesRit)},
                new DisplayColumn(){ColumnName = "自社項目・手数料額", PropName = nameof(DailyRevenueCSVReportData.JisSyaRyoTes)},
                new DisplayColumn(){ColumnName = "自社項目・合計", PropName = nameof(DailyRevenueCSVReportData.JisSyaRyoSum)},
                new DisplayColumn(){ColumnName = "ガイド料・売上額", PropName = nameof(DailyRevenueCSVReportData.GaiUriGakKin)},
                new DisplayColumn(){ColumnName = "ガイド料・消費税額", PropName = nameof(DailyRevenueCSVReportData.GaiSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "ガイド料・手数料額", PropName = nameof(DailyRevenueCSVReportData.GaiSyaRyoTes)},
                new DisplayColumn(){ColumnName = "ガイド料・合計", PropName = nameof(DailyRevenueCSVReportData.GaiSyaRyoSum)},
            };

            if (futaiMeiFlg == 1)
            {
                columns.AddRange(new List<DisplayColumn>() {
                    new DisplayColumn(){ColumnName = "通行料付帯・売上額", PropName = nameof(DailyRevenueCSVReportData.HighwayUriGakKin)},
                    new DisplayColumn(){ColumnName = "通行料付帯・消費税額", PropName = nameof(DailyRevenueCSVReportData.HighwaySyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "通行料付帯・手数料額", PropName = nameof(DailyRevenueCSVReportData.HighwaySyaRyoTes)},
                    new DisplayColumn(){ColumnName = "通行料付帯・合計", PropName = nameof(DailyRevenueCSVReportData.HighwaySyaRyoSum)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・売上額", PropName = nameof(DailyRevenueCSVReportData.HotelUriGakKin)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・消費税額", PropName = nameof(DailyRevenueCSVReportData.HotelSyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・手数料額", PropName = nameof(DailyRevenueCSVReportData.HotelSyaRyoTes)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・合計", PropName = nameof(DailyRevenueCSVReportData.HotelSyaRyoSum)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・売上額", PropName = nameof(DailyRevenueCSVReportData.ParkingUriGakKin)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・消費税額", PropName = nameof(DailyRevenueCSVReportData.ParkingSyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・手数料額", PropName = nameof(DailyRevenueCSVReportData.ParkingSyaRyoTes)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・合計", PropName = nameof(DailyRevenueCSVReportData.ParkingSyaRyoSum)},
                });
            }
            columns.AddRange(new List<DisplayColumn>() {
                    new DisplayColumn(){ColumnName = "その他付帯・売上額", PropName = nameof(DailyRevenueCSVReportData.UriGakKin)},
                    new DisplayColumn(){ColumnName = "その他付帯・消費税額", PropName = nameof(DailyRevenueCSVReportData.SyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "その他付帯・手数料額", PropName = nameof(DailyRevenueCSVReportData.SyaRyoTes)},
                    new DisplayColumn(){ColumnName = "その他付帯・合計", PropName = nameof(DailyRevenueCSVReportData.SyaRyoSum)},
                    new DisplayColumn(){ColumnName = "キャンセル料・金額", PropName = nameof(DailyRevenueCSVReportData.CanUnc)},
                    new DisplayColumn(){ColumnName = "キャンセル料・税区分", PropName = nameof(DailyRevenueCSVReportData.CanZKbn)},
                    new DisplayColumn(){ColumnName = "キャンセル料・税区分名", PropName = nameof(DailyRevenueCSVReportData.CanZKbnNm)},
                    new DisplayColumn(){ColumnName = "キャンセル料・消費税率", PropName = nameof(DailyRevenueCSVReportData.CanSyoR)},
                    new DisplayColumn(){ColumnName = "キャンセル料・消費税額", PropName = nameof(DailyRevenueCSVReportData.CanSyoG)},
                    new DisplayColumn(){ColumnName = "キャンセル料・合計", PropName = nameof(DailyRevenueCSVReportData.CanSum)},
                    new DisplayColumn(){ColumnName = "損益", PropName = nameof(DailyRevenueCSVReportData.UntSoneki)},
                });
            return columns;
        }

        private List<DisplayColumn> BuildMonthlyDisplayColumns(int jippiFlg, int futaiMeiFlg)
        {
            var columns = new List<DisplayColumn>()
            {
                new DisplayColumn(){ColumnName = "№", PropName = nameof(MonthlyRevenueCSVReportData.No)},
                new DisplayColumn(){ColumnName = "売上年月日", PropName = nameof(MonthlyRevenueCSVReportData.UriYmd)},
                new DisplayColumn(){ColumnName = "営業所コード", PropName = nameof(MonthlyRevenueCSVReportData.EigyoCd)},
                new DisplayColumn(){ColumnName = "営業所名", PropName = nameof(MonthlyRevenueCSVReportData.EigyoNm)},
                new DisplayColumn(){ColumnName = "営業所略名", PropName = nameof(MonthlyRevenueCSVReportData.EigyoRyak)},
                new DisplayColumn(){ColumnName = "税込運賃", PropName = nameof(MonthlyRevenueCSVReportData.KeiKin)},
                new DisplayColumn(){ColumnName = "傭車先業者コード", PropName = nameof(MonthlyRevenueCSVReportData.YouGyosyaCd)},
                new DisplayColumn(){ColumnName = "傭車先コード", PropName = nameof(MonthlyRevenueCSVReportData.YouCd)},
                new DisplayColumn(){ColumnName = "傭車先支店コード", PropName = nameof(MonthlyRevenueCSVReportData.YouSitCd)},
                new DisplayColumn(){ColumnName = "傭車先業者コード名", PropName = nameof(MonthlyRevenueCSVReportData.YouGyosyaNm)},
                new DisplayColumn(){ColumnName = "傭車先名", PropName = nameof(MonthlyRevenueCSVReportData.YouNm)},
                new DisplayColumn(){ColumnName = "傭車先支店名", PropName = nameof(MonthlyRevenueCSVReportData.YouSitenNm)},
                new DisplayColumn(){ColumnName = "傭車先略名", PropName = nameof(MonthlyRevenueCSVReportData.YouRyakuNm)},
                new DisplayColumn(){ColumnName = "傭車先支店略名", PropName = nameof(MonthlyRevenueCSVReportData.YouSitRyakuNm)},
                new DisplayColumn(){ColumnName = "傭車項目・運賃", PropName = nameof(MonthlyRevenueCSVReportData.YouSyaRyoUnc)},
                new DisplayColumn(){ColumnName = "傭車項目・台数", PropName = nameof(MonthlyRevenueCSVReportData.YouSyaSyuDai)},
                new DisplayColumn(){ColumnName = "傭車項目・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.YouSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "傭車項目・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.YouSyaRyoTes)},
                new DisplayColumn(){ColumnName = "傭車項目・合計", PropName = nameof(MonthlyRevenueCSVReportData.YouG)},
                new DisplayColumn(){ColumnName = "傭車付帯・発生額", PropName = nameof(MonthlyRevenueCSVReportData.YfuUriGakKin)},
                new DisplayColumn(){ColumnName = "傭車付帯・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.YfuSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "傭車付帯・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.YfuSyaRyoTes)},
                new DisplayColumn(){ColumnName = "傭車付帯・合計", PropName = nameof(MonthlyRevenueCSVReportData.YouFutG)},
                new DisplayColumn(){ColumnName = "自社項目・運賃", PropName = nameof(MonthlyRevenueCSVReportData.JisSyaRyoUnc)},
                new DisplayColumn(){ColumnName = "自社項目・台数", PropName = nameof(MonthlyRevenueCSVReportData.JisSyaSyuDai)},
                new DisplayColumn(){ColumnName = "自社項目・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.JisSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "自社項目・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.JisSyaRyoTes)},
                new DisplayColumn(){ColumnName = "自社項目・合計", PropName = nameof(MonthlyRevenueCSVReportData.JisSyaRyoSum)},
                new DisplayColumn(){ColumnName = "ガイド料・売上額", PropName = nameof(MonthlyRevenueCSVReportData.GaiUriGakKin)},
                new DisplayColumn(){ColumnName = "ガイド料・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.GaiSyaRyoSyo)},
                new DisplayColumn(){ColumnName = "ガイド料・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.GaiSyaRyoTes)},
                new DisplayColumn(){ColumnName = "ガイド料・合計", PropName = nameof(MonthlyRevenueCSVReportData.GaiSyaRyoSum)},
            };

            if (futaiMeiFlg == 1)
            {
                columns.AddRange(new List<DisplayColumn>() {
                    new DisplayColumn(){ColumnName = "通行料付帯・売上額", PropName = nameof(MonthlyRevenueCSVReportData.HighwayUriGakKin)},
                    new DisplayColumn(){ColumnName = "通行料付帯・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.HighwaySyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "通行料付帯・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.HighwaySyaRyoTes)},
                    new DisplayColumn(){ColumnName = "通行料付帯・合計", PropName = nameof(MonthlyRevenueCSVReportData.HighwaySyaRyoSum)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・売上額", PropName = nameof(MonthlyRevenueCSVReportData.HotelUriGakKin)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.HotelSyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.HotelSyaRyoTes)},
                    new DisplayColumn(){ColumnName = "宿泊料付帯・合計", PropName = nameof(MonthlyRevenueCSVReportData.HotelSyaRyoSum)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・売上額", PropName = nameof(MonthlyRevenueCSVReportData.ParkingUriGakKin)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.ParkingSyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.ParkingSyaRyoTes)},
                    new DisplayColumn(){ColumnName = "駐車料付帯・合計", PropName = nameof(MonthlyRevenueCSVReportData.ParkingSyaRyoSum)}
                });
            }
            columns.AddRange(new List<DisplayColumn>() {
                    new DisplayColumn(){ColumnName = "その他付帯・売上額", PropName = nameof(MonthlyRevenueCSVReportData.UriGakKin)},
                    new DisplayColumn(){ColumnName = "その他付帯・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.SyaRyoSyo)},
                    new DisplayColumn(){ColumnName = "その他付帯・手数料額", PropName = nameof(MonthlyRevenueCSVReportData.SyaRyoTes)},
                    new DisplayColumn(){ColumnName = "その他付帯・合計", PropName = nameof(MonthlyRevenueCSVReportData.SyaRyoSum)},
                    new DisplayColumn(){ColumnName = "キャンセル料・金額", PropName = nameof(MonthlyRevenueCSVReportData.CanUnc)},
                    new DisplayColumn(){ColumnName = "キャンセル料・消費税額", PropName = nameof(MonthlyRevenueCSVReportData.CanSyoG)},
                    new DisplayColumn(){ColumnName = "キャンセル料・合計", PropName = nameof(MonthlyRevenueCSVReportData.CanSum)},
                    new DisplayColumn(){ColumnName = "損益", PropName = nameof(MonthlyRevenueCSVReportData.UntSoneki)}
                });
            return columns;
        }

        private StringBuilder BuildCsvFile<T>(List<T> csvData, List<DisplayColumn> columns, SeperatorEnum seperatorEnum, WrapContentEnum wrapContentEnum, ShowHeaderEnum showHeaderEnum)
        {
            var stringBuilder = new StringBuilder();
            var seperator = seperatorEnum == SeperatorEnum.ByComma ? "," : seperatorEnum == SeperatorEnum.BySemicolon ? ";" : "\t";
            var wrapContentBy = wrapContentEnum == WrapContentEnum.PutInDoubleQuater ? "\"" : "";
            if (showHeaderEnum == ShowHeaderEnum.ShowHeader)
                stringBuilder.AppendLine(string.Join(seperator, columns.Select(c => $"{wrapContentBy}{c.ColumnName}{wrapContentBy}")));

            foreach (var item in csvData)
            {
                stringBuilder.AppendLine(string.Join(seperator, columns.Select(c => $"{wrapContentBy}{item.GetType().GetProperty(c.PropName).GetValue(item, null)}{wrapContentBy}")));
            }

            return stringBuilder;
        }

        public XtraReport GetRevenueReportInstanceByPageSize(PageSize size, bool isDailyReport)
        {
            XtraReport report;
            if (isDailyReport)
            {
                switch (size)
                {
                    case PageSize.B4:
                        report = new DailyTransportationRevenueB4();
                        break;
                    case PageSize.A3:
                        report = new DailyTransportationRevenueA3();
                        break;
                    default: // default is A4
                        report = new DailyTransportationRevenue();
                        break;
                }
            }
            else
            {
                switch (size)
                {
                    case PageSize.B4:
                        report = new MonthlyTransportationRevenueB4();
                        break;
                    case PageSize.A3:
                        report = new MonthlyTransportationRevenueA3();
                        break;
                    default: // default is A4
                        report = new MonthlyTransportationRevenue();
                        break;
                }
            }
            return report;
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<TransportationRevenueSearchModel>(queryParams);
            XtraReport report;
            if (searchParams.IsDailyReport)
            {
                var data = await GetDailyRevenueReportData(searchParams);
                report = GetRevenueReportInstanceByPageSize(searchParams.PageSize, true);
                report.DataSource = InitObjectDataSource(data, typeof(DailyRevenueReportDataSource), "objectDataSource2");
            }
            else
            {
                var data = await GetMonthlyRevenueReportData(searchParams);
                report = GetRevenueReportInstanceByPageSize(searchParams.PageSize, false);
                report.DataSource = InitObjectDataSource(data, typeof(MonthlyRevenueReportDataSource), "objectDataSource1");
            }

            return report;
        }

        private ObjectDataSource InitObjectDataSource<T>(T data, Type dataSourceType, string dataSourceName)
        {
            Parameter param = new Parameter()
            {
                Name = "dataSource",
                Type = typeof(T),
                Value = data
            };
            ObjectDataSource dataSource = new ObjectDataSource();
            dataSource.Name = dataSourceName;
            dataSource.DataSource = dataSourceType;
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "DataSource";
            return dataSource;
        }
    }
}
