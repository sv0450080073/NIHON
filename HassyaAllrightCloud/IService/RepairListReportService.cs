using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.RepairListReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.DataAccess.ObjectBinding;
using HassyaAllrightCloud.Reports.DataSource;
using System.Globalization;

namespace HassyaAllrightCloud.IService
{
    public interface IRepairListReportService : IReportService
    {
        Task<List<RepairListReportPDF>> GetPDFData(RepairListData searchParams);
        Task<List<CurrentRepairList>> GetListBusPairForSearch(RepairListData searchParams);
        Dictionary<string, string> GetFieldValues(RepairListData reportData);
        void ApplyFilter(ref RepairListData reportData, List<CompanyChartData> companychartlst,
        List<DepartureOfficeData> vehicledispatchofficelst, List<TPM_SyaRyoData> Vehicles, List<RepairDivision> RepairDivision, Dictionary<string, string> filterValues);
    }
    public class RepairListReportService : IRepairListReportService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediator;
        public RepairListReportService(KobodbContext context, IMediator mediator)
        {
            _dbContext = context;
            _mediator = mediator;
        }
        public async Task<List<CurrentRepairList>> GetListBusPairForSearch(RepairListData searchParams)
        {
            return await _mediator.Send(new GetListBusPairForSearch() { SearchParams = searchParams });
        }

        public async Task<List<RepairListReportPDF>> GetPDFData(RepairListData searchParams)
        {
            var data = new List<RepairListReportPDF>();
            var itemHeader = SetValueHearder(searchParams);
            // var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMDHm);
            var page = 1;
            var listCurrentRepair = new List<CurrentRepairList>();
            listCurrentRepair = await GetListBusPairForSearch(searchParams);
            int index = 1;
            foreach (var itemcurrent in listCurrentRepair)
            {
                itemcurrent.RowNum = index.ToString();
                index++;
            }
            OnSetDataPerPage(data, listCurrentRepair, itemHeader, ref page);
            data.ForEach(e =>
            {
                e.TotalPage = page - 1;
            });
            return data;
        }



        public HeaderReport SetValueHearder(RepairListData searchParams)
        {
            var data = new HeaderReport();
            data.CurrentDateTime = DateTime.Now.ToString("yyyyMMddHHmm");
            data.EigyoCdFrom = searchParams.BranchFrom.EigyoCd > 0 ? searchParams.BranchFrom.EigyoCd.ToString("D5") : "";
            data.EigyoCdTo = searchParams.BranchTo.EigyoCd > 0 ? searchParams.BranchTo.EigyoCd.ToString("D5") : "";
            data.SyaRyoCdFrom = searchParams.VehicleFrom.SyaRyoCd > 0 ? searchParams.VehicleFrom.SyaRyoCd.ToString("D5") : "";
            data.SyaRyoCdCdTo = searchParams.VehicleTo.SyaRyoCd > 0 ? searchParams.VehicleTo.SyaRyoCd.ToString("D5") : "";
            data.DateFrom = searchParams.StartDate.ToString("yyyyMMdd");
            data.DateTo = searchParams.EndDate.ToString("yyyyMMdd");
            data.SyainNm = searchParams.SyainNmItem.SyainNm;
            data.SyainCd = searchParams.SyainNmItem.SyainCd;
            data.OrderNm = searchParams.OutputOrder.StringValue;
            return data;
        }

        private void OnSetDataPerPage(
             List<RepairListReportPDF> listData
           , List<CurrentRepairList> listCurrentRepair
           , dynamic item
           , ref int page)
        {
            List<CurrentRepairList> listTemp = new List<CurrentRepairList>();
            listTemp = listCurrentRepair;
            var itemPerPage = 25;
            if (listTemp.Count > itemPerPage)
            {
                var count = Math.Ceiling(listTemp.Count * 1.0 / itemPerPage);
                for (int i = 0; i < count; i++)
                {
                    var onePage = new RepairListReportPDF();
                    var listPerPage = listTemp.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                    SetData(onePage, listPerPage, page, item, itemPerPage);
                    listData.Add(onePage);
                    page++;
                }
            }
            else
            {
                var onePage = new RepairListReportPDF();
                SetData(onePage, listTemp, page, item, itemPerPage);
                listData.Add(onePage);
                page++;
            }

        }

        private void SetData(
           RepairListReportPDF onePage,
           List<CurrentRepairList> listPerPage,
           int page,
           dynamic item,
           int itemPerPage)
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new CurrentRepairList());
            }
           /* int index = 1;
            foreach (var itemcurrent in listPerPage)
            {
                itemcurrent.RowNum = index.ToString();
                index++;
            }*/
            //onePage.EigyoCdEigyoNm = branchData.BusCurrent.EigyoNm;
            //onePage.AttendanceConfirmReport = listPerPage;
            onePage.HeaderReport = item;
            onePage.CurrentRepairList = listPerPage;
            onePage.PageNumber = page;
        }


        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<RepairListData>(queryParams);
            XtraReport report = new XtraReport();
            switch (searchParams.PaperSize)
            {
                case PaperSize.A3:
                    report = new Reports.ReportTemplate.RepairList.RepairListA3();
                    break;
                case PaperSize.A4:
                    report = new Reports.ReportTemplate.RepairList.RepairListA4();
                    break;
                case PaperSize.B4:
                    report = new Reports.ReportTemplate.RepairList.RepairListB4();
                    break;
                default:
                    break;
            }
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetPDFData(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<RepairListReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(RepairListReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }
        public Dictionary<string, string> GetFieldValues(RepairListData reportData)
        {
            try
            {
                var result = new Dictionary<string, string>
                {
                    [nameof(reportData.OutputSetting)] = $"{ ((int)reportData.OutputSetting).ToString()}",
                    [nameof(reportData.PaperSize)] = $"{((int)reportData.PaperSize).ToString()}",
                    [nameof(reportData.StartDate)] = reportData.StartDate.ToString("yyyyMMdd"),
                    [nameof(reportData.EndDate)] = reportData.EndDate.ToString("yyyyMMdd"),
                    [nameof(reportData.CompanyChartData)] = $"{string.Join('-', reportData.CompanyChartData.Select(c => c.CompanyCdSeq.ToString()))}",
                    [nameof(reportData.BranchFrom)] = reportData.BranchFrom ==null ? "0": reportData.BranchFrom.EigyoCdSeq.ToString(),
                    [nameof(reportData.BranchTo)] = reportData.BranchTo==null ? "0" :reportData.BranchTo.EigyoCdSeq.ToString(),
                    [nameof(reportData.VehicleFrom)] = reportData.VehicleFrom ==null ? "0" : reportData.VehicleFrom.SyaRyoCdSeq.ToString(),
                    [nameof(reportData.VehicleTo)] = reportData.VehicleTo==null ? "0": reportData.VehicleTo.SyaRyoCdSeq.ToString(),
                    [nameof(reportData.RepairFrom)] = reportData.RepairFrom==null ? "0": reportData.RepairFrom.RepairCdSeq.ToString(),
                    [nameof(reportData.RepairTo)] = reportData.RepairTo==null ?"0" : reportData.RepairTo.RepairCdSeq.ToString(),
                    [nameof(reportData.OutputOrder)] = $"{reportData.OutputOrder.IdValue}",
                };
                return result;
            }
            catch(Exception ex)
            {
                return new Dictionary<string, string>();
            } 
        }

        public void ApplyFilter(ref RepairListData reportData, List<CompanyChartData> companychartlst, List<DepartureOfficeData> vehicledispatchofficelst, List<TPM_SyaRyoData> Vehicles, List<RepairDivision> RepairDivision, Dictionary<string, string> filterValues)
        {
            try
            {
                foreach (var keyValue in filterValues)
                {
                    if (keyValue.Key == nameof(reportData.StartDate))
                    {
                        DateTime operationDate;
                        if (DateTime.TryParseExact(keyValue.Value, "yyyyMMdd", null, DateTimeStyles.None, out operationDate))
                        {
                            reportData.StartDate = operationDate;
                        }
                    }
                    if (keyValue.Key == nameof(reportData.EndDate))
                    {
                        DateTime operationDate;
                        if (DateTime.TryParseExact(keyValue.Value, "yyyyMMdd", null, DateTimeStyles.None, out operationDate))
                        {
                            reportData.EndDate = operationDate;
                        }
                    }
                    if (keyValue.Key == nameof(reportData.CompanyChartData))
                    {
                        reportData.CompanyChartData = new List<CompanyChartData>();
                        foreach (var companyItem in keyValue.Value.Split('-'))
                        {
                            int company;
                            if (int.TryParse(companyItem, out company))
                            {
                                reportData.CompanyChartData.Add(companychartlst.SingleOrDefault(c => c.CompanyCdSeq == company));
                            }
                        }
                    }
                    if (keyValue.Key == nameof(reportData.BranchFrom))
                    {
                        int office;
                        if (int.TryParse(keyValue.Value, out office))
                        {
                            reportData.BranchFrom = vehicledispatchofficelst.SingleOrDefault(c => c.EigyoCdSeq == office);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.BranchTo))
                    {
                        int officeto;
                        if (int.TryParse(keyValue.Value, out officeto))
                        {
                            reportData.BranchTo = vehicledispatchofficelst.SingleOrDefault(c => c.EigyoCdSeq == officeto);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.VehicleFrom))
                    {
                        int SyaRyo;
                        if (int.TryParse(keyValue.Value, out SyaRyo))
                        {
                            reportData.VehicleFrom = Vehicles.SingleOrDefault(c => c.SyaRyoCdSeq == SyaRyo);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.VehicleTo))
                    {
                        int office;
                        if (int.TryParse(keyValue.Value, out office))
                        {
                            reportData.VehicleTo = Vehicles.SingleOrDefault(c => c.SyaRyoCdSeq == office);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.RepairFrom))
                    {
                        int office;
                        if (int.TryParse(keyValue.Value, out office))
                        {
                            reportData.RepairFrom = RepairDivision.SingleOrDefault(c => c.RepairCdSeq == office);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.RepairTo))
                    {
                        int office;
                        if (int.TryParse(keyValue.Value, out office))
                        {
                            reportData.RepairTo = RepairDivision.SingleOrDefault(c => c.RepairCdSeq == office);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.OutputOrder))
                    {
                        int outValue;
                        if (int.TryParse(keyValue.Value, out outValue))
                        {
                            reportData.OutputOrder = RepairOutputOrderListData.OutputOrderlst.SingleOrDefault(x => x.IdValue == outValue);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.OutputSetting))
                    {
                        int value;
                        if (int.TryParse(keyValue.Value, out value))
                        {
                            var result = (OutputInstruction)value;
                            reportData.OutputSetting = result;
                        }
                    }
                    if (keyValue.Key == nameof(reportData.PaperSize))
                    {
                        int value;
                        if (int.TryParse(keyValue.Value, out value))
                        {
                            reportData.PaperSize = (PaperSize)value;
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
