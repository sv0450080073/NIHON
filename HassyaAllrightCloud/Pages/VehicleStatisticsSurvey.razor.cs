using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class VehicleStatisticsSurveyBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<VehicleStatisticsSurvey> _lang { get; set; }
        [Inject]
        protected IMonthlyTransportationService _service { get; set; }
        [Inject]
        protected IVehicleStatisticsSurveyService _vehicleStatisticsSurveyService { get; set; }
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        protected EditContext editFormContext { get; set; }
        protected VehicleStatisticsSurveySearchParam searchModel { get; set; } = new VehicleStatisticsSurveySearchParam();
        protected List<CompanyItem> listCompany = new List<CompanyItem>();
        protected List<EigyoItem> listEigyo = new List<EigyoItem>();
        protected List<ShippingItem> listShipping = new List<ShippingItem>();

        protected string Message { get; set; }
        protected MessageBoxType Type { get; set; }
        protected bool IsShow { get; set; }

        public byte itemPerPage { get; set; } = 10;
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        [Inject] protected IFilterCondition FilterConditionService { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);

                var commonValue = await _service.GetCommonListItems();
                listCompany = commonValue.Companys;
                listEigyo = commonValue.Eigyos;
                listShipping = commonValue.Shippings;

                var conditions = await FilterConditionService.GetFilterCondition(FormFilterName.VehicleStatisticsSurvey, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                if (conditions.Any())
                {
                    searchModel.EigyoFrom = listEigyo.FirstOrDefault(_ => _.EigyoCdSeq == int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(VehicleStatisticsSurveySearchParam.EigyoFrom))?.JoInput ?? "0"));
                    searchModel.EigyoTo = listEigyo.FirstOrDefault(_ => _.EigyoCdSeq == int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(VehicleStatisticsSurveySearchParam.EigyoTo))?.JoInput ?? "0"));
                    searchModel.ShippingFrom = listShipping.FirstOrDefault(_ => _.CodeKbnSeq == int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(VehicleStatisticsSurveySearchParam.ShippingFrom))?.JoInput ?? "0"));
                    searchModel.ShippingTo = listShipping.FirstOrDefault(_ => _.CodeKbnSeq == int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(VehicleStatisticsSurveySearchParam.ShippingTo))?.JoInput ?? "0"));
                    searchModel.OutputInstructionMode = int.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(VehicleStatisticsSurveySearchParam.OutputInstructionMode))?.JoInput ?? ((int)OutputInstruction.Preview).ToString());
                    var date = conditions.FirstOrDefault(_ => _.ItemNm == nameof(VehicleStatisticsSurveySearchParam.ProcessingDate));
                    searchModel.ProcessingDate = date == null ? DateTime.Now : DateTime.Parse(date.JoInput);
                }
                searchModel.Company = listCompany.FirstOrDefault();

                editFormContext = new EditContext(searchModel);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task SaveConditions()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(nameof(VehicleStatisticsSurveySearchParam.EigyoFrom), searchModel.EigyoFrom?.EigyoCdSeq.ToString() ?? "0");
            dict.Add(nameof(VehicleStatisticsSurveySearchParam.EigyoTo), searchModel.EigyoTo?.EigyoCdSeq.ToString() ?? "0");
            dict.Add(nameof(VehicleStatisticsSurveySearchParam.ShippingFrom), searchModel.ShippingFrom?.CodeKbnSeq.ToString() ?? "0");
            dict.Add(nameof(VehicleStatisticsSurveySearchParam.ShippingTo), searchModel.ShippingTo?.CodeKbnSeq.ToString() ?? "0");
            dict.Add(nameof(VehicleStatisticsSurveySearchParam.OutputInstructionMode), searchModel.OutputInstructionMode.ToString());
            dict.Add(nameof(VehicleStatisticsSurveySearchParam.ProcessingDate), searchModel.ProcessingDate.ToString());

            await FilterConditionService.SaveFilterCondtion(dict, FormFilterName.VehicleStatisticsSurvey, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
        }

        protected async Task OnInitSearchModel()
        {
            try
            {
                searchModel.EigyoFrom = null;
                searchModel.EigyoTo = null;
                searchModel.ShippingFrom = null;
                searchModel.ShippingTo = null;
                searchModel.OutputInstructionMode = (int)OutputInstruction.Preview;
                searchModel.ProcessingDate = DateTime.Now;
                await FilterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.VehicleStatisticsSurvey);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                jsRuntime.InvokeVoidAsync("EnterTab");
                if (firstRender)
                {
                    jsRuntime.InvokeVoidAsync("focusFirstItem", ".focus-popup");
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected async Task UpdateFormValue(string propertyName, dynamic value)
        {
            try
            {
                var propertyInfo = searchModel.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(searchModel, value, null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task Start()
        {
            try
            {
                if (editFormContext.Validate())
                {
                    var data = await _vehicleStatisticsSurveyService.GetPDFData(searchModel);
                    if (searchModel.OutputInstructionMode == (int)OutputInstruction.Preview)
                    {
                        if (data.Count > 0)
                        {
                            await SaveConditions();
                            var searchString = EncryptHelper.EncryptToUrl(searchModel);
                            await jsRuntime.InvokeVoidAsync("open", "vehiclestatisticssurveypreview?searchString=" + searchString, "_blank");
                        }
                        else
                        {
                            DataNotFound();
                        }
                    }
                    else
                    {
                        if (data.Count > 0)
                        {
                            await SaveConditions();
                            foreach (var item in data)
                            {
                                item.ProcessingDate = searchModel.ProcessingDate.ToString(CommonConstants.FormatYM);
                            }

                            var report = new HassyaAllrightCloud.Reports.ReportTemplate.VehicleStatisticsSurvey.VehicleStatisticsSurvey();
                            report.DataSource = data;

                            report.BeforePrint += (sender, e) =>
                            {
                                var temp = (sender as Reports.ReportTemplate.VehicleStatisticsSurvey.VehicleStatisticsSurvey);
                                var labels = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("lblVisible")).ToList();
                                foreach (var label in labels)
                                {
                                    label.PrintOnPage += (labelsender, labele) =>
                                    {
                                        var index = labele.PageIndex;
                                        if (data[index].Shipping == 4)
                                        {
                                            (labelsender as XRLabel).Visible = false;
                                        }
                                    };
                                }
                            };

                            await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                            {
                                report.CreateDocument();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    report.ExportToPdf(ms);

                                    byte[] exportedFileBytes = ms.ToArray();
                                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                                    jsRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "VehicleStatisticsSurvey");
                                }
                            });
                        }
                        else
                        {
                            DataNotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void DataNotFound()
        {
            IsShow = true;
            Type = MessageBoxType.Info;
            Message = _lang["DataNotFound"];
            StateHasChanged();
        }

        protected void OnTogglePopup(bool value)
        {
            try
            {
                if (!value)
                {
                    IsShow = !IsShow;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
