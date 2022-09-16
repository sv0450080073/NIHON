using BlazorContextMenu;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.TransportDailyReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Pages;
using HassyaAllrightCloud.Pages.Components.TransportDailyReport;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITransportDailyReportService
    {
        Task<List<CompanySearchData>> GetListCompanyForSearch(int CompanyCdSeq);
        Task<List<EigyoSearchData>> GetListEigyoForSearch(int TenantCdSeq, int CompanyCdSeq);
        Task<List<TransportDailyReportData>> GetListTransportDailyReport(TransportDailyReportSearchParams searchParams);
        Task<List<TotalTransportDailyReportData>> GetTotalTransportDailyReport(TransportDailyReportSearchParams searchParams);
        Task<List<TransportDailyReportPDF>> GetDataPDF(TransportDailyReportSearchParams searchParams);
    }

    public class TransportDailyReportService : ITransportDailyReportService, IReportService
    {
        private readonly IMediator _mediator;
        private IStringLocalizer<ListData> _lang;
        private readonly IReportLayoutSettingService _reportLayoutSettingService;
        public TransportDailyReportService(IMediator mediator, IStringLocalizer<ListData> lang, IReportLayoutSettingService reportLayoutSettingService)
        {
            _mediator = mediator;
            _lang = lang;
            _reportLayoutSettingService = reportLayoutSettingService;
        }

        public async Task<List<CompanySearchData>> GetListCompanyForSearch(int CompanyCdSeq)
        {
            return await _mediator.Send(new GetListCompanyForSearchQuery() { CompanyCdSeq = CompanyCdSeq });
        }

        public async Task<List<EigyoSearchData>> GetListEigyoForSearch(int TenantCdSeq, int CompanyCdSeq)
        {
            return await _mediator.Send(new GetListEigyoForSearchQuery() { TenantCdSeq = TenantCdSeq, CompanyCdSeq = CompanyCdSeq });
        }

        public async Task<List<TransportDailyReportData>> GetListTransportDailyReport(TransportDailyReportSearchParams searchParams)
        {
            return await _mediator.Send(new GetListTransportDailyReportQuery() { searchParams = searchParams });
        }

        public async Task<List<TotalTransportDailyReportData>> GetTotalTransportDailyReport(TransportDailyReportSearchParams searchParams)
        {
            return await _mediator.Send(new GetTotalTransportDailyReportQuery() { searchParams = searchParams });
        }

        public async Task<List<TransportDailyReportPDF>> GetDataPDF(TransportDailyReportSearchParams searchParams)
        {
            return await _mediator.Send(new GetTransportDailyReportPDFQuery() { SearchParam = searchParams });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<TransportDailyReportSearchParams>(queryParams);
            XtraReport report = null;
            if (searchParams.pageSize.Value == 0)
            {
                report = new Reports.TransportDailyReportA4();
            }
            else if (searchParams.pageSize.Value == 1)
            {
                report = new Reports.TransportDailyReportA3();
            }
            else
            {
                report = new Reports.TransportDailyReportB4();
            }
            //XtraReport report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.TransportDaily, BaseNamespace.TransportDailyReportTemplate, new ClaimModel().TenantID, searchParams.pageSize.Value);
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetDataPDF(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<TransportDailyReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(TransportDailyReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }
    }
}
