using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.HikiukeshoReport;
using HassyaAllrightCloud.Application.HikiukeshoReport.Commands;
using HassyaAllrightCloud.Application.ReportLayout.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IHikiukeshoReportService
    {
        public Task<int> GetHikiukeshoRowResult(TransportationContractFormData transportationContractForm);
        public Task<bool> UpdateHikiukeshoExportDate(TransportationContractFormData transportationContractForm);
        public void CallReportService(TransportationContractFormData transportationContractFormData, IEnumerable<BookingTypeData> SelectedBookingTypeItems, IJSRuntime JSRuntime, int type);
        public Task<bool> transportationContractReport(string Option, List<string> UkeNo, List<string> UnkRen, string OutType, IJSRuntime JSRuntime, IBookingTypeListService YoyKbnService, int type);
        public string FormatCodeNumber(string code);
    }

    public class HikiukeshoReportService : IHikiukeshoReportService
    {
        private IMediator _mediator;
        private IReportLayoutSettingService _reportLayoutSettingService;
        public HikiukeshoReportService(IMediator mediatR, IReportLayoutSettingService reportLayoutSettingService)
        {
            _mediator = mediatR;
            _reportLayoutSettingService = reportLayoutSettingService;
        }
        public async Task<int> GetHikiukeshoRowResult(TransportationContractFormData transportationContractForm)
        {
            return await _mediator.Send(new GetHikiukeshoRowResultQuery() { TransportationContract = transportationContractForm });
        }
        public async Task<bool> UpdateHikiukeshoExportDate(TransportationContractFormData transportationContractForm)
        {
            return await _mediator.Send(new UpdateHikiukeshoExportDateCommand() { TransportationContract = transportationContractForm });
        }
        public async void CallReportService(TransportationContractFormData transportationContractFormData, IEnumerable<BookingTypeData> SelectedBookingTypeItems, IJSRuntime JSRuntime, int type)
        {
            TransportationContractFormData transportationContractFormDataClone = (TransportationContractFormData)transportationContractFormData.Clone();
            if (type != 0 && !string.IsNullOrEmpty(transportationContractFormDataClone.UkeNumber) && transportationContractFormDataClone.UkeNumber.Trim().Length == 10)
            {
                transportationContractFormDataClone.UkeNumber = FormatCodeNumber(new ClaimModel().TenantID.ToString()) + CommonUtil.FormatCodeNumber(transportationContractFormDataClone.UkeNumber).ToString();
            }
            var gyosya = transportationContractFormData.Gyosya;
            var tokuiSaki = transportationContractFormDataClone.TokuiSaki;
            var tokuiSiten = transportationContractFormData.TokuiSiten;
            var ukeEigyoJo = transportationContractFormDataClone.UketsukeEigyoJo;
            var eigyoTantouSha = transportationContractFormDataClone.EigyoTantoSha;
            var inpTantouSha = transportationContractFormDataClone.InpSyainCd;

            transportationContractFormDataClone.Gyosya = null;
            transportationContractFormDataClone.TokuiSaki = null;
            transportationContractFormDataClone.TokuiSiten = null;
            transportationContractFormDataClone.UketsukeEigyoJo = null;
            transportationContractFormDataClone.EigyoTantoSha = null;
            transportationContractFormDataClone.InpSyainCd = null;
            transportationContractFormDataClone.YoyakuKbnList = null;

            var json = JsonConvert.SerializeObject(transportationContractFormDataClone);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var query = dictionary.Where(pair => pair.Value != null)
                                  .ToDictionary(pair => pair.Key, pair => pair.Value);

            query.Add("TenantCdSeq", new ClaimModel().TenantID.ToString());

            if (gyosya != null)
            {
                query.Add("GyosyaCd", gyosya.GyosyaCd.ToString());
            }
            if (tokuiSaki != null)
            {
                query.Add("TokuiCd", tokuiSaki.TokuiCd.ToString());
            }
            if (tokuiSiten != null)
            {
                query.Add("SitenCd", tokuiSiten.SitenCd.ToString());
            }

            if (ukeEigyoJo != null)
            {
                query.Add("UkeEigCd", ukeEigyoJo.EigyoCd.ToString());
            }

            if (eigyoTantouSha != null)
            {
                query.Add("EigSyainCd", eigyoTantouSha.SyainCd.ToString());
            }

            if (inpTantouSha != null)
            {
                query.Add("InpSyainCd", inpTantouSha.SyainCd.ToString());
            }

            if (SelectedBookingTypeItems.Any())
            {
                query.Add("YoyaKbnList", String.Join(',', SelectedBookingTypeItems.Select(x => x.YoyaKbn)));
            }

            if (transportationContractFormDataClone.UkeNumber != null)
            {
                query.Add("UkeNumberFull", transportationContractFormDataClone.UkeNumber);
            }

            if (transportationContractFormDataClone.DateFrom != null)
            {
                switch (transportationContractFormDataClone.DateTypeContract)
                {
                    case 1:
                        query.Add("StartDispatchDate", transportationContractFormDataClone.DateFrom?.ToString("yyyyMMdd"));
                        break;
                    case 2:
                        query.Add("StartArrivalDate", transportationContractFormDataClone.DateFrom?.ToString("yyyyMMdd"));
                        break;
                    case 3:
                        query.Add("StartReservationDate", transportationContractFormDataClone.DateFrom?.ToString("yyyyMMdd"));
                        break;
                }
            }

            if (transportationContractFormDataClone.DateTo != null)
            {
                switch (transportationContractFormDataClone.DateTypeContract)
                {
                    case 1:
                        query.Add("EndDispatchDate", transportationContractFormDataClone.DateTo?.ToString("yyyyMMdd"));
                        break;
                    case 2:
                        query.Add("EndArrivalDate", transportationContractFormDataClone.DateTo?.ToString("yyyyMMdd"));
                        break;
                    case 3:
                        query.Add("EndReservationDate", transportationContractFormDataClone.DateTo?.ToString("yyyyMMdd"));
                        break;
                }
            }

            if (transportationContractFormDataClone.PrintMode == ((int)PrintMode.Preview))
            {
                var claimModel = new ClaimModel();
                int CurrentTemplateId = await _mediator.Send(new GetReportCurrentTemplateQuery { TenantCdSeq = claimModel.TenantID, ReportId = ReportIdForSetting.TransportContract, EigyouCdSeq = claimModel.EigyoCdSeq });
                string ReportClassName = BaseNamespace.Report + BaseNamespace.TransportContract + CurrentTemplateId + PaperSize.A4;
                query.Add("ReportTemplate", ReportClassName);
                var uri = $"HikiukeshoReport?Params={EncryptHelper.EncryptToUrl(query)}";
                JSRuntime.InvokeAsync<object>("open", uri, "_blank");
            }
            else
            {
                await ExportReportAsPdf(transportationContractFormDataClone.PrintMode, query, JSRuntime);
            }
        }
        async Task ExportReportAsPdf(int printMode, Dictionary<string, string> parameters, IJSRuntime JSRuntime)
        {
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.TransportContract, BaseNamespace.TransportContract, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);
            var newSubReport = new HassyaAllrightCloud.Reports.ReportTemplate.Hikiukesho.SubReport();
            

            report.Parameters["UkeNo"].Value = !parameters.ContainsKey("UkeNumberFull") ? null : parameters["UkeNumberFull"];
            report.Parameters["OutputUnit"].Value = parameters["OutputUnit"];
            report.Parameters["StartDispatchDate"].Value = !parameters.ContainsKey("StartDispatchDate") ? null : parameters["StartDispatchDate"];
            report.Parameters["EndDispatchDate"].Value = !parameters.ContainsKey("EndDispatchDate") ? null : parameters["EndDispatchDate"];
            report.Parameters["StartArrivalDate"].Value = !parameters.ContainsKey("StartArrivalDate") ? null : parameters["StartArrivalDate"];
            report.Parameters["EndArrivalDate"].Value = !parameters.ContainsKey("EndArrivalDate") ? null : parameters["EndArrivalDate"];
            report.Parameters["StartReservationDate"].Value = !parameters.ContainsKey("StartReservationDate") ? null : parameters["StartReservationDate"];
            report.Parameters["EndReservationDate"].Value = !parameters.ContainsKey("EndReservationDate") ? null : parameters["EndReservationDate"];
            report.Parameters["TokuiCd"].Value = !parameters.ContainsKey("TokuiCd") ? null : parameters["TokuiCd"];
            report.Parameters["SitenCd"].Value = !parameters.ContainsKey("SitenCd") ? null : parameters["SitenCd"];
            report.Parameters["YoyaKbnList"].Value = !parameters.ContainsKey("YoyaKbnList") ? null : parameters["YoyaKbnList"];
            report.Parameters["YearlyContract"].Value = parameters["YearlyContract"];
            report.Parameters["OutputSelection"].Value = parameters["OutputSelection"];
            report.Parameters["TenantCdSeq"].Value = parameters["TenantCdSeq"];

            IEnumerable<XRLabel> allControls = report.AllControls<XRLabel>();
            XRSubreport subreport = report.AllControls<XRSubreport>().FirstOrDefault(x => x.Name == "subreport1");
            if (printMode == (int) PrintMode.SaveAsExcel)
            {
                
                XRLabel subreportLabel1 = newSubReport.AllControls<XRLabel>().FirstOrDefault(x => x.Name == "label1");
                if (subreportLabel1 != null) subreportLabel1.CanPublish = false;
                subreport.ReportSource = newSubReport;
                
                for (int t = 1; t <= 8; t++)
                {
                    var label = allControls.FirstOrDefault(x => x.Name == "label" + t);
                    if(label != null) label.CanPublish = false;
                }
            }

            await new System.Threading.Tasks.TaskFactory().StartNew(() =>
            {
                string fileType = "";
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {
                    switch (printMode)
                    {
                        case (int)PrintMode.SaveAsPDF:
                            fileType = "pdf";
                            report.ExportToPdf(ms);
                            break;
                        case (int)PrintMode.SaveAsExcel:
                            fileType = "xlsx";
                            report.ExportToXlsx(ms);
                            break;
                        case (int)PrintMode.Print:
                            PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                            tool.Print();
                            return;
                    }

                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, fileType, "HikiukeshoReport");
                }
            });
        }
        public async Task<bool> transportationContractReport(string Option, List<string> UkeNo, List<string> UnkRen, string OutType, IJSRuntime JSRuntime, IBookingTypeListService YoyKbnService, int type)
        {
            IEnumerable<BookingTypeData> SelectedBookingTypeItems = await YoyKbnService.GetBySiyoKbn();
            TransportationContractFormData transportationContractFormData = new TransportationContractFormData();
            transportationContractFormData = new TransportationContractFormData();
            transportationContractFormData.DateTypeContract = (int)DateTypeContract.Dispatch;
            transportationContractFormData.OutputSelection = (int)OutputSelection.All;
            transportationContractFormData.YearlyContract = (int)YearlyContract.NotOutput;
            transportationContractFormData.PrintMode = Option == "Preview" ? (int)PrintMode.Preview : (int)PrintMode.SaveAsPDF;
            transportationContractFormData.UkeNumber = String.Join(",", UkeNo.ToArray());
            transportationContractFormData.OutputUnit = OutType == "1" ? (int)OutputUnit.EachBooking : (int)OutputUnit.EachBusTypeBooking;
            transportationContractFormData.DateFrom = null;
            transportationContractFormData.DateTo = null;
            transportationContractFormData.YoyakuKbnList = SelectedBookingTypeItems.ToList();
            if(UnkRen.Count == 1)
            {
                transportationContractFormData.UnkRen = UnkRen[0]; 
            }
            if (UnkRen.Count > 1)
            {
                transportationContractFormData.UnkRen = "";
            }
            int ReturnRecordNumber = 0;
            ReturnRecordNumber = await GetHikiukeshoRowResult(transportationContractFormData);

            if (ReturnRecordNumber <= 0)
            {
                return true;
            }
            else
            {
                CallReportService(transportationContractFormData, SelectedBookingTypeItems, JSRuntime, type);
                return false;
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
