using BlazorContextMenu;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.AdvancePaymentDetails.Query;
using HassyaAllrightCloud.Application.ReportLayout.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Reports.DataSource;
using HassyaAllrightCloud.Reports.ReportTemplate;
using MediatR;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IAdvancePaymentDetailsService : IReportService
    {
        Task<List<SeikyuSakiSearch>> GetListAddressForSearch(int tenantCdSeq);
        Task<List<AdvancePaymentDetailsModel>> GetListAdvancePaymentDetailsHeader(AdvancePaymentDetailsSearchParam searchParams);
        Task<List<AdvancePaymentDetailsChildModel>> GetListAdvancePaymentDetailsChild(AdvancePaymentDetailsSearchParam searchParams);
        Task<int> GetAdvancePaymentDetailRowResult(AdvancePaymentDetailsSearchParam searchParams);
        Task<List<AdvancePaymentDetailsReportPDF>> GetAdvancePaymentDetailsReportPDFData(AdvancePaymentDetailsSearchParam searchParams);
        Task<bool> AdvancePaymentDetailReport(string option, List<string> ukeNo, IJSRuntime JSRuntime, IReportLayoutSettingService reportLayoutSettingService);
        public Task OnExportPdf(byte printMode, AdvancePaymentDetailsSearchParam searchParams, IJSRuntime JSRuntime, IReportLayoutSettingService reportLayoutSettingService, int type);
        public string FormatCodeNumber(string code);
    }
    public class AdvancePaymentDetailsService : IAdvancePaymentDetailsService
    {
        private readonly IMediator _mediator;
        private readonly IReportLayoutSettingService _reportLayoutSettingService;
        public AdvancePaymentDetailsService(IMediator mediator, IReportLayoutSettingService reportLayoutSettingService)
        {
            _mediator = mediator;
            _reportLayoutSettingService = reportLayoutSettingService;
        }

        public async Task<List<SeikyuSakiSearch>> GetListAddressForSearch(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListSeikyuSakiForSearchQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<AdvancePaymentDetailsModel>> GetListAdvancePaymentDetailsHeader(AdvancePaymentDetailsSearchParam searchParams)
        {
            return await _mediator.Send(new GetListAdvancePaymentDetailsHeaderQuery() { SearchParams = searchParams });
        }

        public async Task<List<AdvancePaymentDetailsChildModel>> GetListAdvancePaymentDetailsChild(AdvancePaymentDetailsSearchParam searchParams)
        {
            return await _mediator.Send(new GetListAdvancePaymentDetailsChildQuery() { SearchParams = searchParams });
        }

        public async Task<int> GetAdvancePaymentDetailRowResult(AdvancePaymentDetailsSearchParam searchParams)
        {
            return await _mediator.Send(new GetAdvancePaymentDetailRowResultQuery() { SearchParams = searchParams });
        }

        public async Task<List<AdvancePaymentDetailsReportPDF>> GetAdvancePaymentDetailsReportPDFData(AdvancePaymentDetailsSearchParam searchParams)
        {
            return await _mediator.Send(new GetAdvancePaymentDetailsReportPDFQuery() { SearchParams = searchParams });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<AdvancePaymentDetailsSearchParam>(queryParams);
            XtraReport report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.AdvancePaymentDetails, BaseNamespace.AdvancePaymentDetailsReportTemplate, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, searchParams.PaperSize.Value);

            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetAdvancePaymentDetailsReportPDFData(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<AdvancePaymentDetailsReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(AdvancePaymentDetailsReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }
        public async Task<bool> AdvancePaymentDetailReport(string option, List<string> ukeNo, IJSRuntime JSRuntime, IReportLayoutSettingService reportLayoutSettingService)
        {
            AdvancePaymentDetailsSearchParam searchParams = new AdvancePaymentDetailsSearchParam();
            searchParams.OutputSetting = option == "Preview" ? (byte)PrintMode.Preview : (byte)PrintMode.SaveAsPDF;
            searchParams.PaperSize = new PaperSizeDropdown() { Value = 1, Text = "A4"};
            searchParams.ReceptionNumber = String.Join(",", ukeNo.ToArray());
            searchParams.ScheduleYmdStart = null;
            searchParams.ScheduleYmdEnd = null;
            searchParams.AddressSpectify = null;
            searchParams.StartAddress = null;
            searchParams.EndAddress = null;
            int ReturnRecordNumber = await GetAdvancePaymentDetailRowResult(searchParams);
            if (ReturnRecordNumber == 0)
            {
                return true;
            }
            else
            {
                if (searchParams.OutputSetting == (byte)PrintMode.Preview)
                {
                    await OnExportPdf((byte)PrintMode.Preview, searchParams, JSRuntime, reportLayoutSettingService, 1);
                }
                else
                {
                    await OnExportPdf((byte)PrintMode.SaveAsPDF, searchParams, JSRuntime, reportLayoutSettingService, 1);
                }
                return false;
            }
        }
        public async Task OnExportPdf(byte printMode, AdvancePaymentDetailsSearchParam searchParams, IJSRuntime JSRuntime, IReportLayoutSettingService reportLayoutSettingService, int type)
        {
            if (type == 0 && !string.IsNullOrEmpty(searchParams.ReceptionNumber) && searchParams.ReceptionNumber.Trim().Length == 10)
            {
                searchParams.ReceptionNumber = FormatCodeNumber(new ClaimModel().TenantID.ToString()) + CommonUtil.FormatCodeNumber(searchParams.ReceptionNumber).ToString();
            } 
            if (printMode == (byte)PrintMode.Preview)
            {
                var searchString = EncryptHelper.EncryptToUrl(searchParams);
                await JSRuntime.InvokeVoidAsync("open", "AdvancePaymentDetailsPreview?searchString=" + searchString, "_blank");
            }
            else
            {
                var data = await GetAdvancePaymentDetailsReportPDFData(searchParams);
                XtraReport report = await reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.AdvancePaymentDetails, BaseNamespace.AdvancePaymentDetailsReportTemplate, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, searchParams.PaperSize.Value);
                report.DataSource = data;
                await new TaskFactory().StartNew(() =>
                {
                    report.CreateDocument();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        if (printMode == (byte)PrintMode.Print)
                        {
                            PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                            tool.Print();
                            return;
                        }
                        report.ExportToPdf(ms);

                        byte[] exportedFileBytes = ms.ToArray();
                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                        JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "AdvancePaymentDetail");
                    }
                });

            }
        }
        public string FormatCodeNumber(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return "";
            }
            else
            {
                return code.PadLeft(5, '0');
            }
        }
    }
}
