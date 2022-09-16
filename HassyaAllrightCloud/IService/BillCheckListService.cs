using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.BillCheckList.Queries;
using HassyaAllrightCloud.Application.BillOffice.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
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
    public interface IBillCheckListService
    {
        public Task<List<BillOfficeData>> GetBillOffice(int tenantID);
        public Task<(int, List<BillCheckListTotalData>, List<BillCheckListGridData>)> GetBillCheckListGridData(BillsCheckListFormData billCheckList, int CompanyId, int TenantId, int? OffSet = null, int Fetch = 0);
        public Task<List<BillCheckListReportPDF>> GetBillCheckListReportData(BillsCheckListFormData billCheckList, int CompanyId, int TenantId);
        public Task<List<BillCheckListModelCsvData>> GetBillCheckListReportCsv(BillsCheckListFormData billCheckList, int CompanyId, int TenantId);
        public Task<List<BillCheckListTotalData>> GetBillCheckListGridTotalData(BillsCheckListFormData billCheckList, int CompanyId, int TenantId);
        public Task<List<BillAddress>> GetBillAddress(int TenantId);
        public Task<List<BillAddress>> GetBillCheckListForCmbBillAddress(BillsCheckListFormData billCheckList, int CompanyId, int TenantId);
    }
    public class BillCheckListService : IBillCheckListService, IReportService
    {
        private IMediator mediatR;
        public BillCheckListService(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        public async Task<(int, List<BillCheckListTotalData>, List<BillCheckListGridData>)> GetBillCheckListGridData(BillsCheckListFormData billCheckList, int CompanyId, int TenantId, int? OffSet = null, int Fetch = 0)
        {
            return await mediatR.Send(new GetDataForBillCheckListGridQuery { billCheckListData = billCheckList, companyId = CompanyId , tenantId = TenantId , fetch = Fetch , offSet = OffSet });
        }

        public async Task<List<BillCheckListTotalData>> GetBillCheckListGridTotalData(BillsCheckListFormData billCheckList, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetDataForBillCheckListToTalAllQuery { billCheckListData = billCheckList, companyId = CompanyId, tenantId = TenantId });
        }

        public async Task<List<BillOfficeData>> GetBillOffice(int tenantID)
        {
            return await mediatR.Send(new GetBillOfficeListQuery { _tenantId = tenantID });
        }
        public async Task<List<BillAddress>> GetBillCheckListForCmbBillAddress(BillsCheckListFormData billCheckList, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetDataForCmbBillAddressDistinctQuery { billCheckListData = billCheckList, companyId = CompanyId, tenantId = TenantId });
        }

        public async Task<List<BillCheckListReportPDF>> GetBillCheckListReportData(BillsCheckListFormData billCheckList, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetDataForBillCheckListReportQuery { billCheckListData = billCheckList, companyId = CompanyId, tenantId = TenantId });
        }
        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<BillsCheckListFormData>(queryParams);

            XtraReport report = new Reports.BillCheckListReportA4();
            if (searchParams.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
            {
                report = new Reports.BillCheckListReportA3();
            }
            else
            {
                if (searchParams.PageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                    report = new Reports.BillCheckListReportB4();
            }
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetBillCheckListReportData(searchParams, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<BillCheckListReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(BillCheckListReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<List<BillCheckListModelCsvData>> GetBillCheckListReportCsv(BillsCheckListFormData billCheckList, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetDataForBillCheckListReportCsvQuery { billCheckListData = billCheckList, companyId = CompanyId, tenantId = TenantId});
        }

        public async Task<List<BillAddress>> GetBillAddress(int TenantId)
        {
            return await mediatR.Send(new GetBillAddressQuery { tenantId = TenantId });
        }
    }
}

