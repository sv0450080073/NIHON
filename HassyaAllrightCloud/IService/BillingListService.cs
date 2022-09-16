using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.BillPrint.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillingList;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IBillingListService
    {
        public Task<BillingListDetailResult> GetBillingListDetailAsync(BillingListFilter billingListFilter);
        public Task<List<string>> GetBillingListDetailCodeAsync(BillingListFilter billingListFilter);
        public Task<BillingListResult> GetBillingListAsync(BillingListFilter billingListFilter);
        public Task<List<BillingListGridCsvData>> GetBillingListCsvAsync(BillingListFilter billingListFilter);
        public Task<List<BillingListDetailGridCsvData>> GetBillingListDetailCsvAsync(BillingListFilter billingListFilter);
        public Task<List<BillingListDetailReport>> GetBillingListDetailReportAsync(BillingListFilter billingListFilter);
        public Task<List<BillingListReport>> GetBillingListReportAsync(BillingListFilter billingListFilter);

    }
    public class BillingListService : IBillingListService, IReportService
    {
        private IMediator mediatR;
        public BillingListService(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        public async Task<BillingListDetailResult> GetBillingListDetailAsync(BillingListFilter billingListFilter)
        {
            return await mediatR.Send(new GetBillingListDetailAsyncQuery { billingListFilter = billingListFilter });
        }

        public async Task<List<string>> GetBillingListDetailCodeAsync(BillingListFilter billingListFilter)
        {
            return await mediatR.Send(new GetBillingListDetailCodesAsyncQuery { billingListFilter = billingListFilter });
        }

        public async Task<BillingListResult> GetBillingListAsync(BillingListFilter billingListFilter)
        {
            return await mediatR.Send(new GetBillingListAsyncQuery { billingListFilter = billingListFilter });
        }

        public async Task<List<BillingListDetailGridCsvData>> GetBillingListDetailCsvAsync(BillingListFilter billingListFilter)
        {
            return await mediatR.Send(new GetBillingListDetailCsvAsyncQuery { billingListFilter = billingListFilter });
        }

        public async Task<List<BillingListGridCsvData>> GetBillingListCsvAsync(BillingListFilter billingListFilter)
        {
            return await mediatR.Send(new GetBillingListCsvAsyncQuery { billingListFilter = billingListFilter });
        }

        public async Task<List<BillingListDetailReport>> GetBillingListDetailReportAsync(BillingListFilter billingListFilter)
        {
            return await mediatR.Send(new GetBillingListDetailReportAsyncQuery { billingListFilter = billingListFilter });
        }

        public async Task<List<BillingListReport>> GetBillingListReportAsync(BillingListFilter billingListFilter)
        {
            return await mediatR.Send(new GetBillingListReportAsyncQuery { billingListFilter = billingListFilter });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<BillingListFilter>(queryParams);
            XtraReport report = new XtraReport();
            if (searchParams.isListMode)
            {
                report = new Reports.ReportTemplate.BillingListReport.BillingListReportA4();
                if (searchParams.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                {
                    report = new Reports.ReportTemplate.BillingListReport.BillingListReportA3();
                }
                else
                {
                    if (searchParams.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                        report = new Reports.ReportTemplate.BillingListReport.BillingListReportB4();
                }
                ObjectDataSource dataSource = new ObjectDataSource();
                var data = await GetBillingListReportAsync(searchParams);
                Parameter param = new Parameter()
                {
                    Name = "data",
                    Type = typeof(List<BillingListReport>),
                    Value = data
                };
                dataSource.Name = "objectDataSource1";
                dataSource.DataSource = typeof(BillingListReportDS);
                dataSource.Constructor = new ObjectConstructorInfo(param);
                dataSource.DataMember = "_data";
                report.DataSource = dataSource;
            } else
            {
                report = new Reports.ReportTemplate.BillingListReport.BillingListDetailReportA4();
                if (searchParams.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                {
                    report = new Reports.ReportTemplate.BillingListReport.BillingListDetailReportA3();
                }
                else
                {
                    if (searchParams.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                        report = new Reports.ReportTemplate.BillingListReport.BillingListDetailReportB4();
                }
                ObjectDataSource dataSource = new ObjectDataSource();
                var data = await GetBillingListDetailReportAsync(searchParams);
                Parameter param = new Parameter()
                {
                    Name = "data",
                    Type = typeof(List<BillingListDetailReport>),
                    Value = data
                };
                dataSource.Name = "objectDataSource1";
                dataSource.DataSource = typeof(BillingListDetailReportDS);
                dataSource.Constructor = new ObjectConstructorInfo(param);
                dataSource.DataMember = "_data";
                report.DataSource = dataSource;
            }
            return report;
        }
    }
}

